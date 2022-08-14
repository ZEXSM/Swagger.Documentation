using System;
using System.IO;

namespace Swagger.Documentation
{
    internal class DocumentationResourceExtensions
    {
        public static string GetPath() => Path.Combine(AppContext.BaseDirectory, Contants.ApiDocs);
    }
}
