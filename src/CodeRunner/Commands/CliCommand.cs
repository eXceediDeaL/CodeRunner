using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class CliCommand : BaseCommand<CliCommand.CArgument>
    {
        public override Command Configure()
        {
            RootCommand res = new RootCommand(Program.AppDescription);
            {
                Argument<string> arg = new Argument<string>(nameof(CArgument.Command))
                {
                    Arity = ArgumentArity.ExactlyOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Command)}".ToLower(), "Command to execute.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-c");
                res.AddOption(optCommand);
            }
            {
                Argument<bool> arg = new Argument<bool>(nameof(CArgument.Verbose), false)
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Verbose)}".ToLower(), "Enable debug mode for more logs.")
                {
                    Argument = arg
                };
                res.AddOption(optCommand);
            }
            {
                Argument<DirectoryInfo> arg = new Argument<DirectoryInfo>(nameof(CArgument.Directory), new DirectoryInfo(Directory.GetCurrentDirectory()))
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Directory)}".ToLower(), "Set working directory.")
                {
                    Argument = arg
                };
                optCommand.AddAlias("-d");
                optCommand.AddAlias("--dir");
                res.AddOption(optCommand);
            }
            return res;
        }

        protected override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            Environment.CurrentDirectory = argument.Directory!.FullName;

            {
                var workspaces = pipeline.Services.GetWorkspaces();
                var builtinFs = workspaces.GetProviders().First();
                pipeline.Services.Add<IWorkspace>(await builtinFs.Provider.Resolve(new Templates.ResolveContext()));
            }

            ILogger logger = pipeline.Services.GetLogger();

            if (argument.Command != "")
            {
                Parser repl = CommandLines.CreateDefaultParser(pipeline.Services.GetReplCommand(), pipeline);
                pipeline.IsStopped = true;
                return await repl.InvokeAsync(argument.Command, console);
            }

            if (argument.Verbose)
            {
            }
            else
            {
                _ = logger.UseLevelFilter(LogLevel.Information);
            }

            return 0;
        }

        public class CArgument
        {
            public string Command { get; set; } = "";

            public DirectoryInfo? Directory { get; set; }

            public bool Verbose { get; set; } = false;
        }
    }
}
