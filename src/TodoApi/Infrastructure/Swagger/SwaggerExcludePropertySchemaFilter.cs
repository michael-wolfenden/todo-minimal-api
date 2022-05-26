using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TodoApi.Infrastructure.Swagger;

// For updates (put) we want to bind like this (long id, UpdateTodo.Request request)
// where the id comes from the route but the request from the body

// The request also has an id (which we set via -> request with { Id = id }) but we don't want
// to expose it to swagger, so we can decorate with SwaggerIgnorePropertyAttribute

// public record Request([property: SwaggerIgnoreProperty] long Id, string Title, bool IsCompleted) 

[AttributeUsage(AttributeTargets.Property)]
public class SwaggerIgnorePropertyAttribute : Attribute
{
}

public class SwaggerExcludePropertySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }

        var excludedProperties = context.Type
            .GetProperties()
            .Where(propertyInfo => propertyInfo.GetCustomAttribute<SwaggerIgnorePropertyAttribute>() is not null);

        foreach (var excludedProperty in excludedProperties)
        {
            var schemaProperty = schema
                .Properties
                .FirstOrDefault(x => x.Key.Equals(excludedProperty.Name, StringComparison.OrdinalIgnoreCase));

            if (schemaProperty.Key is not null)
            {
                _ = schema.Properties.Remove(schemaProperty.Key);
            }
        }
    }
}
