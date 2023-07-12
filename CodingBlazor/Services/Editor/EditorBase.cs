using BlazorMonaco.Editor;
using CodingBlazor.Entities;
using Microsoft.AspNetCore.Components;

namespace CodingBlazor.Services.Editor; 

public abstract class EditorBase : ComponentBase{
    protected StandaloneCodeEditor Editor { get; set; } = null!;
    protected string? Output { get; set; }

    protected abstract Task CompileCode();

    protected virtual LanguageData InitEditor(StandaloneCodeEditor editor) {
        Editor = editor;
        return new LanguageData();
    }
}