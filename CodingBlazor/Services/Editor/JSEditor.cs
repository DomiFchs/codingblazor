using System.Text;
using BlazorMonaco.Editor;
using CodingBlazor.Entities;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime.Interop;

namespace CodingBlazor.Services.Editor; 

public class JsEditor : EditorBase{
    protected override async Task CompileCode() {
        var engine = new Engine();
        var consoleOutput = new StringBuilder();

        void ConsoleLog(object output) {
            consoleOutput.AppendLine(output.ToString());
        }

        engine.SetValue("console", new {
            log = new Action<object>(ConsoleLog)
        });

        try
        {
            engine.Execute(await Editor.GetValue());
            Output = consoleOutput.ToString();
        }
        catch (Exception ex)
        {
            // You might want to return error messages to the user or handle them differently.
            Output = ex.Message;
        }
    }

    protected override LanguageData InitEditor(StandaloneCodeEditor editor) {
        base.InitEditor(editor);
        return new LanguageData {
            Name = EditorTypeHandler.GetLanguageString[typeof(JsEditor)],
            DefaultCode = CodeProvider.BaseJsCode()
        };
    }
}