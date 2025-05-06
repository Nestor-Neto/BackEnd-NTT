using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class SwaggerConfiguration
{
    public static void ConfigureSwaggerGen(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Ambev Developer Evaluation API",
            Version = "v1",
            Description = "API para avaliação de desenvolvedores da Ambev"
        });

        options.SchemaGeneratorOptions.SchemaIdSelector = type => type.FullName;
        
        options.CustomSchemaIds(type => type.FullName);
        
        options.SchemaFilter<RemoveGuidDefaultValueFilter>();
    }
}

public class RemoveGuidDefaultValueFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Guid))
        {
            schema.Example = null;
            schema.Default = null;
        }
    }
} 