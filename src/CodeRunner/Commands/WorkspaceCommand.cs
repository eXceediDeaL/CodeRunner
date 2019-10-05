using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class WorkspaceCommand : BaseCommand<WorkspaceCommand.CArgument>
    {
        public override string Name => "workspace";

        public override Command Configure()
        {
            Command res = new Command("workspace", "Manage workspaces.");
            res.AddAlias("ws");
            res.AddCommand(new Workspaces.ListCommand().Build());
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
