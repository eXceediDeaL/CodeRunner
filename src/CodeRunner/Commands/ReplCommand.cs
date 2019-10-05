using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class ReplCommand : BaseCommand<ReplCommand.CArgument>
    {
        public override string Name => "repl";

        public override Command Configure()
        {
            RootCommand res = new RootCommand(Program.AppDescription);
            res.AddCommand(new ConfigCommand().Build());
            res.AddCommand(new ExtensionCommand().Build());
            res.AddCommand(new CommandCommand().Build());
            res.AddCommand(new WorkspaceCommand().Build());
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
