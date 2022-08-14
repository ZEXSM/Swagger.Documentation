using FluentAssertions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using Xunit;

namespace Swagger.Documentation.ExceptionTest
{
    public class WriteMarkdownToDescriptionOperationFilterTest
    {
        [Fact(DisplayName = "Should be an exception when writing markdown in description")]
        public void WriteMarkdownToDescriptionOperationFilter_directory_not_found_exception()
        {
            var openApiOperation = new OpenApiOperation();
            var operationFilterContext = CreateOperationFilterContext();

            var operationFilter = new WriteMarkdownToDescriptionOperationFilter();
            operationFilter.Invoking(i => i.Apply(openApiOperation, operationFilterContext))
                .Should()
                .Throw<DirectoryNotFoundException>()
                .WithMessage("Directory ApiDocs not found. See readme.");
        }

        private static OperationFilterContext CreateOperationFilterContext()
        {
            var methodInfo = typeof(Assert).GetMethod(nameof(Assert.NotEmpty));

            return new OperationFilterContext(
                apiDescription: null,
                schemaRegistry: null,
                schemaRepository: null,
                methodInfo);
        }
    }
}