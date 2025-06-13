using CampusLearn.DataLayer.Constants;
using CampusLearn.Services.Domain.Authorization;

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

#region -- cors configuration --

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });

    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:8082") // or your domain
            .AllowAnyHeader()
            .AllowAnyMethod());
});

#endregion -- cors configuration --

#region -- custom configurations --

builder.Services.Configure<ScheduleSetting>(builder.Configuration.GetSection("defaultSchedule"));
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("smtp"));

#endregion -- custom configurations --

#region -- db context --

builder.Services.AddSingleton<CampusLearnDbContext>();
builder.Services.AddSingleton<Bootstraper>();

#endregion -- db context --

#region -- repositories --

builder.Services.AddSingleton<INotificationRepository, NotificationRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IEnquiryRepository, EnquiryRepository>();
builder.Services.AddSingleton<IQuizRepository, QuizRepository>();
builder.Services.AddSingleton<IChatRepository, ChatRepository>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<IModuleRepository, ModuleRepository>();
builder.Services.AddSingleton<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddSingleton<IDiscussionRepository, DiscussionRepository>();

#endregion -- repositories --

#region -- services --

builder.Services.AddSingleton<JwtTokenProvider>();
builder.Services.AddSingleton<SMTPManager>();
builder.Services.AddSingleton<ScheduleManager>();
builder.Services.AddSingleton<INotificationService, NotificationService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IEnquiryService, EnquiryService>();
builder.Services.AddSingleton<IQuizService, QuizService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IModuleService, ModuleService>();
builder.Services.AddSingleton<ISubscriptionService, SubscriptionService>();
builder.Services.AddSingleton<IDiscussionService, DiscussionService>();

#endregion -- services --

builder.Services.AddSingleton<IAuthorizationHandler, HierarchicalRoleHandler>();
builder.Services.AddHostedService<PersonalHostedService>();
builder.Services.AddControllers();

builder.Services.AddApiVersioning(v =>
{
    v.AssumeDefaultVersionWhenUnspecified = true;
    v.DefaultApiVersion = new ApiVersion(1, 0);
    v.ReportApiVersions = true;
    v.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-version"),
        new MediaTypeApiVersionReader("version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationRoles.Learner, policy =>
        policy.Requirements.Add(new HierarchicalRoleRequirement(UserRoles.Learner)));

    options.AddPolicy(AuthorizationRoles.Tutor.ToString(), policy =>
        policy.Requirements.Add(new HierarchicalRoleRequirement(UserRoles.Tutor)));

    options.AddPolicy(AuthorizationRoles.Lecturer.ToString(), policy =>
        policy.Requirements.Add(new HierarchicalRoleRequirement(UserRoles.Lecturer)));

    options.AddPolicy(AuthorizationRoles.Administrator.ToString(), policy =>
        policy.Requirements.Add(new HierarchicalRoleRequirement(UserRoles.Administrator)));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };

        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new
                {
                    message = "Invalid or expired token.",
                    details = context.Exception?.InnerException?.Message ?? context.Exception?.Message
                });
                return context.Response.WriteAsync(result);
            },
            OnChallenge = context =>
            {
                // Suppress the default behavior
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new
                {
                    message = "Authorization token is missing or invalid."
                });
                return context.Response.WriteAsync(result);
            },
            OnForbidden = context =>
            {
                // Suppress the default behavior
                //context.HandleResponse();
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { message = "Access denied. You do not have permission." });
                return context.Response.WriteAsync(result);
            }
        };
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Error = e.Value.Errors.First().ErrorMessage
            });

        return new BadRequestObjectResult(new
        {
            message = "Validation failed.",
            errors = errors
        });
    };
});

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.ListenAnyIP(5008); // Listen on port 8080
//    serverOptions.ListenAnyIP(7209, listenOptions => listenOptions.UseHttps());
//});

var app = builder.Build();

app.UseCors("AllowAngularApp");

// Serve files from "Uploads" directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Configuration["FileUploadPath"].ToString())),
    //Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/files"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"CAMPUS LEARN API v1");
    });
}

var boostrapper = app.Services.GetRequiredService<Bootstraper>();
await boostrapper.Migrations();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
