using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NoMercy.Networking;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NoMercy.Server.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions> {

    public void Configure(SwaggerGenOptions options)
    {
        foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

            options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{NoMercyConfig.AuthBaseUrl}protocol/openid-connect/auth"),
                        Scopes = new Dictionary<string, string> {
                            ["openid"] = "openid" ,
                            ["profile"] = "profile" 
                        }
                    }
                }
            });

            OpenApiSecurityScheme keycloakSecurityScheme = new()
            {
                Reference = new OpenApiReference
                {
                    Id = "Keycloak",
                    Type = ReferenceType.SecurityScheme
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            };

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { keycloakSecurityScheme, Array.Empty<string>() },
                {
                    new OpenApiSecurityScheme
                        { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                    []
                }
            });
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        return new OpenApiInfo {
            Title = "NoMercy API",
            Version = description.ApiVersion.ToString(),
            Description = "NoMercy API" + (description.IsDeprecated ? " (deprecated)" : ""),
            Contact = new OpenApiContact
            {
                Name = "NoMercy",
                Email = "info@nomercy.tv",
                Url = new Uri("https://nomercy.tv")
            },
            TermsOfService = new Uri("https://nomercy.tv/terms-of-service")
        };
    }
}