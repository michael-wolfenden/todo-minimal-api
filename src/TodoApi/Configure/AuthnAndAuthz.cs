using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Configure;

public static class AuthnAndAuthz
{
    public static WebApplicationBuilder AddCustomAuthnAndAuthz(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            // require authentication for all endpoints
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = "https://demo.duendesoftware.com";
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new()
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                // validate audience is api
                options.Audience = "api";
            });

        return builder;
    }

    public static WebApplication UseCustomAuthnAndAuthz(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
