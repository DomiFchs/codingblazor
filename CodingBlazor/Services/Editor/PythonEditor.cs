using BlazorMonaco.Editor;
using CodingBlazor.Entities;
using IronPython.Hosting;

namespace CodingBlazor.Services.Editor; 

public class PythonEditor : EditorBase{
    protected override async Task CompileCode() {
        try
        {
            // Create a new ScriptEngine instance
            var engine = Python.CreateEngine();

            // Create a new MemoryStream to capture standard output
            var outputStream = new MemoryStream();
            var outputWriter = new StreamWriter(outputStream);

            // Set the standard output to our StreamWriter
            engine.Runtime.IO.SetOutput(outputStream, outputWriter);

            // Execute the Python code
            engine.Execute(await Editor.GetValue());

            // Flush the writer to make sure all output has been written to the MemoryStream
            await outputWriter.FlushAsync();

            // Get a string from the MemoryStream and return it
            outputStream.Position = 0; // Reset the position of the stream to the beginning
            var reader = new StreamReader(outputStream);
            var output = await reader.ReadToEndAsync();

            // Clean up
            outputWriter.Close();
            reader.Close();

            Output =  output;
        }
        catch (Exception ex)
        {
            // If there's an error, return the error message
            Output =  $"Error: {ex.Message}";
        }
    }

    protected override LanguageData InitEditor(StandaloneCodeEditor editor) {
        base.InitEditor(editor);
        return new LanguageData {
            Name = EditorTypeHandler.GetLanguageString[typeof(PythonEditor)],
            DefaultCode = CodeProvider.BasePythonCode()
        };
    }
}