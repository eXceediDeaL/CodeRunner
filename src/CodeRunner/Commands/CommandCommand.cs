using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class CommandCommand : BaseCommand<CommandCommand.CArgument>
    {
        public override string Name => "command";

        public override Command Configure()
        {
            Command res = new Command("command", "Manage commands.");
            res.AddAlias("cmd");
            res.AddCommand(new Commands.ListCommand().Build());
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
