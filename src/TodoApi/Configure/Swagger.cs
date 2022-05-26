using Microsoft.OpenApi.Models;
using TodoApi.Infrastructure.Swagger;

namespace TodoApi.Configure;

public static class Swagger
{
    public static WebApplicationBuilder AddCustomSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(o =>
        {
            o.SchemaFilter<SwaggerExcludePropertySchemaFilter>();

            // TodoApi.Todos.Features.CreateTodo+Response => TodoApi.Todos.Features.CreateTodoResponse
            o.CustomSchemaIds(type => type.FullName?.Replace("+", ""));

            o.AddSecurityDefinition(
                "oauth2",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://demo.duendesoftware.com/connect/authorize"),
                            TokenUrl = new Uri("https://demo.duendesoftware.com/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "api", $"{builder.Environment.ApplicationName} API access" }
                            }
                        }
                    }
                });

            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                        Scheme = "oauth2",
                        Name = "oauth2",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        return builder;
    }

    public static WebApplication UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            o.OAuthClientId("interactive.public");
            o.OAuthUsePkce();
        });

        return app;
    }
}
