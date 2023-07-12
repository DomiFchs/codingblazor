using CodingBlazor.Services.Editor;

namespace CodingBlazor.Services; 

public static class EditorTypeHandler {

    public static readonly Dictionary<Type, string> GetLanguageString = new() {
        {typeof(CSharpEditor), "csharp"},
        {typeof(PythonEditor), "python"},
        {typeof(JsEditor), "javascript"}
    };
}