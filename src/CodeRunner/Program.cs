using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.CommandLine.Rendering;
using System.Text;
using System.Threading.Tasks;

namespace CodeRunner
{
    public enum EnvironmentType
    {
        Production,
        Development,
        Test
    }

    public static class Program
    {
        internal static readonly string AppDescription = string.Join(System.Environment.NewLine,
            "Code Runner, a CLI tool to run code.",
            "Copyright (c) StardustDL. All rights reserved.",
            "Open source with Apache License 2.0 on https://github.com/StardustDL/CodeRunner.");

        public static EnvironmentType Environment { get; set; } = EnvironmentType.Production;

        public static async Task<int> Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            ILogger logger = new Logger();
            ExtensionCollection extensions = new ExtensionCollection();
            {
                extensions.Load(new ExtensionLoader(typeof(Extensions.Builtin.Console.ConsoleExtension).Assembly));
                extensions.Load(new ExtensionLoader(typeof(Extensions.Builtin.Workspace.WorkspaceExtension).Assembly));
            }

            PipelineBuilder<string[], Wrapper<int>> builder = new PipelineBuilder<string[], Wrapper<int>>();

            _ = builder.ConfigureLogger(logger)
                .ConfigureExtensions(extensions)
                .ConfigureHost(new ExtensionHost())
                .ConfigureCliCommand();

            if (Environment == EnvironmentType.Test)
            {
                if (TestView.Input == null)
                {
                    throw new NullReferenceException(nameof(TestView.Input));
                }

                _ = builder.ConfigureConsole(new TestTerminal(), TestView.Input);
            }
            else
            {
                _ = builder.ConfigureConsole(new SystemConsole(), Console.In);
            }

            _ = builder.UseWorkspacesService()
                       .UseCommandsService()
                       .UseReplCommandService();

            _ = builder.UseCliCommand();

            if (Environment == EnvironmentType.Test)
            {
                _ = builder.UseTestView();
            }

            _ = builder.UseReplCommand();

            Pipeline<string[], Wrapper<int>> pipeline = await builder.Build(args, logger);
            PipelineResult<Wrapper<int>> result = await pipeline.Consume();
            if (result.IsOk)
            {
                return result.Result!;
            }
            else
            {
                Console.Error.WriteLine(result.Exception!.ToString());
                return -1;
            }
        }
    }
}