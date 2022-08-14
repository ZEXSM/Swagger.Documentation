using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Swagger.Documentation.Test
{
    public class WriteMarkdownToDescriptionOperationFilterTest
    {
        [Theory(DisplayName = "Should be successful writing markdown in description")]
        [InlineData(null, nameof(FakeController.Get))]
        [InlineData(nameof(FakeController.Get), null)]
        public void WriteMarkdownToDescriptionOperationFilter_success(string operationId, string methodName)
        {
            var openApiOperation = new OpenApiOperation
            {
                OperationId = operationId
            };
            var operationFilterContext = CreateOperationFilterContext(methodName);

            var operationFilter = new WriteMarkdownToDescriptionOperationFilter();
            operationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.Description.Should().NotBeNullOrEmpty();
        }

        private static OperationFilterContext CreateOperationFilterContext(string methodName)
        {
            var methodInfo = typeof(FakeController).GetMethod(methodName ?? nameof(FakeController.Get));

            return new OperationFilterContext(
                apiDescription: new ApiDescription(),
                schemaRegistry: null,
                schemaRepository: null,
                methodInfo);
        }
    }
}