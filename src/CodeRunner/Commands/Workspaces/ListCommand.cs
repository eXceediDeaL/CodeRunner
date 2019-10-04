using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Extensions.Managements;
using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Workspaces
{
    public class ListCommand : BaseCommand<ListCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("list", "List all.");
            return res;
        }

        protected override Task<int> Handle(ListCommand.CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            WorkspaceCollection manager = pipeline.Services.GetWorkspaces();
            terminal.OutputTable(manager.GetProviders(),
                new OutputTableColumnStringView<IWorkspaceProvider>(x => x.Name, nameof(IWorkspaceProvider.Name)),
                new OutputTableColumnStringView<IWorkspaceProvider>(x => manager.GetExtension(x)?.Name ?? "N/A", "Extension")
            );
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
