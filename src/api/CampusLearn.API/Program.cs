var builder = WebApplication.CreateBuilder(args);

#region -- custom configurations --

builder.Services.Configure<ScheduleSetting>(builder.Configuration.GetSection("defaultSchedule"));
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("smtp"));

#endregion -- custom configurations --

#region -- db context --

builder.Services.AddSingleton<CampusLearnDbContext>();

#endregion -- db context --

#region -- repositories --

builder.Services.AddSingleton<IMessagingRepository, MessagingRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

#endregion -- repositories --

#region -- services --

builder.Services.AddSingleton<JwtTokenProvider>();
builder.Services.AddSingleton<SMTPManager>();
builder.Services.AddSingleton<ScheduleManager>();
builder.Services.AddSingleton<IMessagingServices, MessagingServices>();
builder.Services.AddSingleton<IUserService, UserService>();

#endregion -- services --

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

    c.AddSecurityRequirement(securityRequirement);

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = $"v1",
        Title = "CAMPUS LEARN API API v1",
        Description = "The Campus Learn System API provides RESTful endpoints to manage users, courses, enrollments, assignments, submissions, and grades. It is designed to integrate with student portals, faculty dashboards, and mobile learning apps."
    });
    c.ResolveConflictingActions(x => x.GetEnumerator().Current);
    c.OperationFilter<RemoveVersionFromParameter>();
    c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
});

builder.Services.AddAuthorization();
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
                var result = JsonConvert.SerializeObject(new { message = "Invalid or expired token." });
                return context.Response.WriteAsync(result);
            },
            OnChallenge = context =>
            {
                // Suppress the default behavior
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { message = "Authorization token is missing or invalid." });
                return context.Response.WriteAsync(result);
            },
            OnForbidden = context =>
            {
                // Suppress the default behavior
                //context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(new { message = "You are not authorized to access this function." });
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"CAMPUS LEARN API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();