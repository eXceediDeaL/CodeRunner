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

namespace CodeRunner.Commands.Commands
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
            CommandCollection manager = pipeline.Services.GetCommands();
            terminal.OutputTable(manager,
                new OutputTableColumnStringView<Command>(x => x.Name, nameof(IWorkspaceProvider.Name)),
                new OutputTableColumnStringView<Command>(x => manager.GetExtension(x)?.Name ?? "N/A", "Extension")
            );
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
