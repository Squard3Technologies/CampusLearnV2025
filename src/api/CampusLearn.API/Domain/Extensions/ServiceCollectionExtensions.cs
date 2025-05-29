namespace CampusLearn.API.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSwagger(this IServiceCollection service)
    {
        service.AddSwaggerGen(c =>
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
                 Title = "CAMPUS LEARN API v1",
                 Description = "The Campus Learn System API provides RESTful endpoints to manage users, courses, enrollments, assignments, submissions, and grades. It is designed to integrate with student portals, faculty dashboards, and mobile learning apps."
             });
             c.ResolveConflictingActions(x => x.GetEnumerator().Current);
             c.OperationFilter<RemoveVersionFromParameter>();
             c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
         });
    }
}
