using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using routage.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace routage.Analysis
{
    public class RoutesIndexer
    {
        public List<RouteDetails> GetRoutes()
        {
            var routeList = new List<RouteDetails>();

            var slnPath = @"C:\Users\shrayasr\work\code\ixm\";
            var files = Directory.GetFiles(slnPath, "*.cs", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
                var syntaxRoot = tree.GetRoot();

                var routeAttrs = syntaxRoot
                                    .DescendantNodes()
                                    .OfType<AttributeSyntax>()
                                    .Where(attr => attr.Name.ToString().ToUpper() == "ROUTE");

                var routes = routeAttrs
                                .Select(routeAttr =>
                                {
                                    var fn = routeAttr.Parent.Parent as MethodDeclarationSyntax;

                                    HttpMethod routeMethod = fn.AttributeLists
                                                                    .Where(attr => attr.ToString().ToUpper() == "[HTTPPOST]")
                                                                    .FirstOrDefault() == null ? HttpMethod.GET : HttpMethod.POST;

                                    var route = routeAttr.ArgumentList.Arguments.Select(arg => arg.ToString().Replace("\"", "")).FirstOrDefault();
                                    var fnName = fn.Identifier.Text;
                                    var cls = (routeAttr.Parent.Parent.Parent as ClassDeclarationSyntax).Identifier.Text;

                                    return new RouteDetails
                                    {
                                        Route = route,
                                        Method = routeMethod.ToString(),
                                        FunctionName = fnName,
                                        ClassName = cls
                                    };
                                });

                routeList.AddRange(routes);
            }

            return routeList;
        }
    }

    enum HttpMethod
    {
        GET,
        POST
    }

}