#!/usr/bin/env dotnet-script
#r "nuget: Microsoft.CodeAnalysis.CSharp, 4.8.0"
#r "nuget: Microsoft.CodeAnalysis.CSharp.Workspaces, 4.8.0"

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using System.Linq;
using System.Text;

// Configuration
var projectPath = Args.FirstOrDefault() ?? Directory.GetCurrentDirectory();
var outputFile = "class-dependency-graph.dot";

Console.WriteLine($"Analyzing project at: {projectPath}");

// Find all C# files
var csFiles = Directory.GetFiles(projectPath, "*.cs", SearchOption.AllDirectories)
    .Where(f => !f.Contains("\\obj\\") && !f.Contains("\\bin\\"))
    .ToList();

Console.WriteLine($"Found {csFiles.Count} C# files");

var dependencies = new Dictionary<string, HashSet<string>>();

foreach (var file in csFiles)
{
    var code = File.ReadAllText(file);
    var tree = CSharpSyntaxTree.ParseText(code);
    var root = tree.GetRoot();

    // Get all class declarations
    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

    foreach (var classDecl in classes)
    {
        var className = GetFullClassName(classDecl);
        if (!dependencies.ContainsKey(className))
        {
            dependencies[className] = new HashSet<string>();
        }

        // Find constructor parameters (constructor injection)
        var constructors = classDecl.DescendantNodes().OfType<ConstructorDeclarationSyntax>();
        foreach (var ctor in constructors)
        {
            foreach (var param in ctor.ParameterList.Parameters)
            {
                var typeName = param.Type?.ToString();
                if (typeName != null && !IsBuiltInType(typeName))
                {
                    dependencies[className].Add(typeName);
                }
            }
        }

        // Find field/property types
        var fields = classDecl.DescendantNodes().OfType<FieldDeclarationSyntax>();
        foreach (var field in fields)
        {
            var typeName = field.Declaration.Type.ToString();
            if (!IsBuiltInType(typeName))
            {
                dependencies[className].Add(typeName);
            }
        }

        var properties = classDecl.DescendantNodes().OfType<PropertyDeclarationSyntax>();
        foreach (var prop in properties)
        {
            var typeName = prop.Type.ToString();
            if (!IsBuiltInType(typeName))
            {
                dependencies[className].Add(typeName);
            }
        }

        // Find base class
        if (classDecl.BaseList != null)
        {
            foreach (var baseType in classDecl.BaseList.Types)
            {
                var typeName = baseType.Type.ToString();
                if (!IsBuiltInType(typeName) && !typeName.StartsWith("I"))
                {
                    dependencies[className].Add(typeName);
                }
            }
        }
    }
}

// Generate DOT file
var sb = new StringBuilder();
sb.AppendLine("digraph ClassDependencies {");
sb.AppendLine("  rankdir=LR;");
sb.AppendLine("  node [shape=box, style=rounded];");
sb.AppendLine();

// Add all nodes
foreach (var className in dependencies.Keys)
{
    var simpleName = className.Split('.').Last();
    sb.AppendLine($"  \"{className}\" [label=\"{simpleName}\"];");
}

sb.AppendLine();

// Add edges
foreach (var kvp in dependencies)
{
    foreach (var dependency in kvp.Value)
    {
        // Only add edge if the dependency is also in our project
        if (dependencies.ContainsKey(dependency))
        {
            sb.AppendLine($"  \"{kvp.Key}\" -> \"{dependency}\";");
        }
    }
}

sb.AppendLine("}");

File.WriteAllText(outputFile, sb.ToString());
Console.WriteLine($"Class dependency graph written to: {outputFile}");

// Helper methods
string GetFullClassName(ClassDeclarationSyntax classDecl)
{
    var namespaceName = classDecl.Ancestors()
        .OfType<BaseNamespaceDeclarationSyntax>()
        .FirstOrDefault()?.Name.ToString() ?? "";
    
    return string.IsNullOrEmpty(namespaceName) 
        ? classDecl.Identifier.Text 
        : $"{namespaceName}.{classDecl.Identifier.Text}";
}

bool IsBuiltInType(string typeName)
{
    var builtInTypes = new[] { 
        "string", "int", "bool", "double", "float", "decimal", "long", 
        "short", "byte", "char", "object", "void", "var",
        "String", "Int32", "Boolean", "Double", "Single", "Decimal",
        "List", "Dictionary", "IEnumerable", "IList", "Task"
    };
    
    return builtInTypes.Any(t => typeName.Contains(t)) || typeName.Contains("<");
}
