namespace CodingBlazor.Services; 

public static class CodeProvider {
    public static string BaseCode() => @"
using System;
using System.Collections.Generic;

    public class Program{
        static void Main(){
            Console.WriteLine(""Hello, World!"");
        }
}";
}