using System.CommandLine;
using System.CommandLine.Invocation;

namespace Parme.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var codeGenCommand = new Command("codegen", "Generate code for emitter logic")
            {
                new Argument<string>("inputFile", "Input emitter logic definition file"),
                new Argument<string>("outputFile", "Name of the file to generate"),
                new Argument<string>("language", "language for the generated output (valid values: \"csharp\")"),
                new Option<string>("--className", "Name to give the generated class"),
                new Option<string>("--namespace", "Namespace to put generated code in"),
            };
            
            codeGenCommand.Handler = CommandHandler.Create<string, string, string, string, string, IConsole>(CodeGenHandler.Handle);
            var commands = new RootCommand
            {
                codeGenCommand,
            };

            commands.Invoke(args);
        }
    }
}