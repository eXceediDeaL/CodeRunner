using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Managements;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Configs
{
    public class InitCommand : BaseCommand<InitCommand.CArgument>
    {
        public override string Name => "config.init";

        public override Command Configure()
        {
            Command res = new Command("init", "Initialize or uninitialize code-runner directory.");
            {
                Argument<bool> arg = new Argument<bool>(nameof(CArgument.Delete), false)
                {
                    Arity = ArgumentArity.ZeroOrOne
                };
                Option optCommand = new Option($"--{nameof(CArgument.Delete)}".ToLower(), "Remove all code-runner files.")
                {
                    Argument = arg
                };
                res.AddOption(optCommand);
            }
            return res;
        }

        protected override async Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            Manager manager = pipeline.Services.GetManager();
            if (argument.Delete)
            {
                await manager.Clear();
            }
            else
            {
                await manager.Initialize();
            }
            CodeRunner.Extensions.IHost host = pipeline.Services.GetHost();
            ((ExtensionHost)host).Restart();
            return 0;
        }

        public class CArgument
        {
            public bool Delete { get; set; } = false;
        }
    }
}
