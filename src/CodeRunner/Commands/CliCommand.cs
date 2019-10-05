using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class CliCommand : BaseCommand<CliCommand.CArgument>
    {
        public override string Name => "cli";

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

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            Environment.CurrentDirectory = argument.Directory!.FullName;

            {
                Manager manager = new Manager(new DirectoryInfo(Environment.CurrentDirectory));
                pipeline.Services.Add<Manager>(manager);
            }

            pipeline.Services.Add<string>("fs", ServicesExtensions.ArgWorkspaceNameId);
            pipeline.Services.Add<string>(argument.Command, ServicesExtensions.ArgCommandId);

            ILogger logger = pipeline.Services.GetLogger();

            if (argument.Verbose)
            {
            }
            else
            {
                _ = logger.UseLevelFilter(LogLevel.Information);
            }

            return Task.FromResult(0);
        }

        public class CArgument
        {
            public string Command { get; set; } = "";

            public DirectoryInfo? Directory { get; set; }

            public bool Verbose { get; set; } = false;
        }
    }
}
