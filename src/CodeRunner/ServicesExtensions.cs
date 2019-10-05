using CodeRunner.Extensions;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System.CommandLine;
using System.IO;

namespace CodeRunner
{
    public static class ServicesExtensions
    {
        internal const string ReplCommandId = "repl";

        internal const string CliCommandId = "cli";

        internal const string ArgWorkspaceNameId = "arg-wsname";

        internal const string ArgCommandId = "arg-cmd";

        public static ExtensionCollection GetExtensions(this IServiceScope scope) => scope.GetService<ExtensionCollection>();

        public static CommandCollection GetCommands(this IServiceScope scope) => scope.GetService<CommandCollection>();

        public static WorkspaceCollection GetWorkspaces(this IServiceScope scope) => scope.GetService<WorkspaceCollection>();

        public static Manager GetManager(this IServiceScope scope) => scope.GetService<Manager>();

        public static Command GetReplCommand(this IServiceScope scope) => scope.GetService<Command>(ReplCommandId);

        public static Command GetCliCommand(this IServiceScope scope) => scope.GetService<Command>(CliCommandId);

        public static IServiceScope CreateExtensionScope(this IServiceScope scope) => new ServiceSubscope(scope).NoAccess<Manager>()
            .NoAccess<ExtensionCollection>().NoAccess<CommandCollection>().NoAccess<WorkspaceCollection>()
            .NoAccess<Command>(ReplCommandId).NoAccess<Command>(CliCommandId)
            .NoWrite<IHost>().NoWrite<TextReader>().NoWrite<IConsole>().NoWrite<ILogger>().NoWrite<IWorkspace>();
    }
}