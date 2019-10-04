using CodeRunner.Extensions.Commands;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace CodeRunner.Commands
{
    public class ExtensionCommand : BaseCommand<ExtensionCommand.CArgument>
    {
        public override Command Configure()
        {
            Command res = new Command("extension", "Manage extensions.");
            res.AddAlias("ext");
            return res;
        }

        protected override Task<int> Handle(CArgument argument, IConsole console, InvocationContext context, PipelineContext operation, CancellationToken cancellationToken) => Task.FromResult(0);

        public class CArgument
        {
        }
    }
}
