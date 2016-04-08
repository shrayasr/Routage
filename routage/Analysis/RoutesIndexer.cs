using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace routage.Analysis
{
    public class RoutesIndexer
    {
        public List<string> Index()
        {
            var routeList = new List<string>();

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
                                .Select(attr => attr.ArgumentList.Arguments)
                                .Select(attr => attr.ToString())
                                .Select(route => route.Replace("\"", ""));

                routeList.AddRange(routes);
            }

            return routeList;
        }
    }
}