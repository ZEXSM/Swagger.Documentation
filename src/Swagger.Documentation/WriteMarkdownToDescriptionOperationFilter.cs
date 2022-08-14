using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System.Text;

namespace Swagger.Documentation
{
    public class WriteMarkdownToDescriptionOperationFilter : IOperationFilter
    {
        private const string Controller = nameof(Controller);
        private const string Diagrams = nameof(Diagrams);

        /// <inheritdoc/>
        public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
        {
            var fileName = !string.IsNullOrWhiteSpace(openApiOperation.OperationId) ?
                openApiOperation.OperationId : operationFilterContext.MethodInfo.Name;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var resourceType = operationFilterContext.MethodInfo.DeclaringType;
                if (resourceType != default)
                {
                    var documentationPath = DocumentationResourceExtensions.GetPath();

                    if (!Directory.Exists(documentationPath))
                    {
                        throw new DirectoryNotFoundException($"Directory {Contants.ApiDocs} not found. See readme.");
                    }

                    var resourceName = resourceType.Name.Replace(Controller, string.Empty);
                    var resourceGroupName = operationFilterContext.ApiDescription?.GroupName ?? string.Empty;

                    var filePath = Path.Combine(documentationPath, resourceName, resourceGroupName, $"{fileName}.md");

                    if (File.Exists(filePath))
                    {
                        openApiOperation.Description = File.ReadAllText(filePath, Encoding.UTF8)
                            .Replace(
                                $"{Diagrams}/",
                                string.IsNullOrEmpty(resourceGroupName) ?
                                    $"{resourceName}/{Diagrams}/"
                                    :
                                    $"{resourceName}/{resourceGroupName}/{Diagrams}/");
                    }
                }
            }
        }
    }
}
