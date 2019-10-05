using CodeRunner.Extensions;
using CodeRunner.Extensions.Commands;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands.Extensions
{
    public class ListCommand : BaseCommand<ListCommand.CArgument>
    {
        public override string Name => "extension.list";

        public override Command Configure()
        {
            Command res = new Command("list", "List all.");
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext pipeline, CancellationToken cancellationToken)
        {
            ITerminal terminal = console.GetTerminal();
            ExtensionCollection manager = pipeline.Services.GetExtensions();
            terminal.OutputTable(manager,
                new OutputTableColumnStringView<ExtensionLoader>(x => x.Extension.Name, nameof(IExtension.Name)),
                new OutputTableColumnStringView<ExtensionLoader>(x => x.Extension.Description ?? "N/A", nameof(IExtension.Description)),
                new OutputTableColumnStringView<ExtensionLoader>(x => x.Extension.Publisher ?? "N/A", nameof(IExtension.Publisher)),
                new OutputTableColumnStringView<ExtensionLoader>(x => x.Extension.Version.ToString() ?? "N/A", nameof(IExtension.Version))
            );
            return Task.FromResult(0);
        }

        public class CArgument
        {
        }
    }
}
