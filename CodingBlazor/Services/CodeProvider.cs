namespace CodingBlazor.Services; 

public static class CodeProvider {
    public static string BaseCSharpCode() => @"
using System;
using System.Collections.Generic;

    public class Program{
        static void Main(){
            Console.WriteLine(""Hello, World!"");
        }
}";

    public static string BasePythonCode() => @"
print(""Hello, World!"")";

    public static string BaseJsCode() => @"
console.log(""Hello, world!"");";
}