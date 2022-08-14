using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Swagger.Documentation
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUISupportMarkdown(
            this IApplicationBuilder app,
            Action<SwaggerUIOptions> setupAction = default)
        {
            var documentationPath = DocumentationResourceExtensions.GetPath();

            if (!Directory.Exists(documentationPath))
            {
                throw new DirectoryNotFoundException($"Directory {Contants.ApiDocs} not found. See readme.");
            }

            var options = default(SwaggerUIOptions);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<SwaggerUIOptions>>().Value;
                setupAction?.Invoke(options);
            }

            if (options.ConfigObject.Urls == null)
            {
                options.ConfigObject.Urls = new[] { new UrlDescriptor { Name = $"{Assembly.GetEntryAssembly().GetName().Name} v1", Url = "v1/swagger.json" } };
            }

            options.InjectStylesheet("./markdown.css");
            options.InjectStylesheet("./override-swagger-ui.css");

            var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? string.Empty : $"/{options.RoutePrefix}";

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = routePrefix,
                FileProvider = new EmbeddedFileProvider(
                    typeof(SwaggerUIBuilderExtensions).Assembly,
                    $"{typeof(SwaggerUIBuilderExtensions).Namespace}.{nameof(Swashbuckle.AspNetCore.SwaggerUI)}")
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = routePrefix,
                FileProvider = new PhysicalFileProvider(documentationPath)
            });

            app.UseSwaggerUI(options);

            return app;
        }
    }
}
