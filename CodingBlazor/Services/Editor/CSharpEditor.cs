using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BlazorMonaco.Editor;
using CodingBlazor.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodingBlazor.Services.Editor; 

public class CSharpEditor : EditorBase{
    protected override async Task CompileCode() {
        var syntaxTree = CSharpSyntaxTree.ParseText(await Editor.GetValue());
        var sr = Assembly.Load("System.Runtime, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        var sl = Assembly.Load("System.Linq, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
        var compilation = CSharpCompilation.Create("DynamicAssembly").WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication)).AddReferences(MetadataReference.CreateFromFile(sl.Location),MetadataReference.CreateFromFile(sr.Location),MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location), MetadataReference.CreateFromFile(typeof(object).Assembly.Location), MetadataReference.CreateFromFile(typeof(decimal).Assembly.Location), MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), MetadataReference.CreateFromFile(typeof(GCSettings).Assembly.Location), MetadataReference.CreateFromFile(typeof(DynamicAttribute).Assembly.Location), MetadataReference.CreateFromFile(typeof(Marshal).Assembly.Location), MetadataReference.CreateFromFile(typeof(DllImportAttribute).Assembly.Location), MetadataReference.CreateFromFile(typeof(AssemblyTargetedPatchBandAttribute).Assembly.Location), MetadataReference.CreateFromFile(typeof(Unsafe).Assembly.Location), MetadataReference.CreateFromFile(typeof(Expression).Assembly.Location), MetadataReference.CreateFromFile(typeof(ExpressionType).Assembly.Location), MetadataReference.CreateFromFile(typeof(Expression<>).Assembly.Location)).AddSyntaxTrees(syntaxTree);

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);
        if (!result.Success) {
            Output = string.Join(Environment.NewLine, result.Diagnostics);
        }
        else {
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            var entryPoint = assembly.EntryPoint;
            if (entryPoint != null) {
                using var outputStream = new StringWriter();
                Console.SetOut(outputStream);

                object? instance = null;
                if (!entryPoint.IsStatic)
                    instance = assembly.CreateInstance(entryPoint.DeclaringType?.FullName);

                entryPoint.Invoke(instance, null);

                Output = outputStream.ToString();
            }
            else {
                Output = "No entry point found in the code.";
            }
        }
    }

    protected override LanguageData InitEditor(StandaloneCodeEditor editor) {
        base.InitEditor(editor);
        return new LanguageData {
            Name = EditorTypeHandler.GetLanguageString[typeof(CSharpEditor)],
            DefaultCode = CodeProvider.BaseCSharpCode()
        };
    }
}