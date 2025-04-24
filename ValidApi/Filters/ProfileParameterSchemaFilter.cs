using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ValidApi.Models;

namespace ValidApi.Filters
{
    public class ProfileParameterSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ProfileParameter))
            {
                schema.Example = new OpenApiObject
                {
                    ["profileName"] = new OpenApiString("User"),
                    ["parameters"] = new OpenApiObject
                    {
                        ["CanEdit"] = new OpenApiString("true"),
                        ["CanDelete"] = new OpenApiString("false")
                    }
                };
            }
        }
    }

}
