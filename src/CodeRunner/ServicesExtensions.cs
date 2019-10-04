using CodeRunner.Managements;
using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System.CommandLine;

namespace CodeRunner
{
    public static class ServicesExtensions
    {
        internal const string ReplCommandId = "repl";

        internal const string CliCommandId = "cli";

        internal const string ArgWorkspaceNameId = "arg-wsname";

        internal const string ArgCommandId = "arg-cmd";

        public static ExtensionCollection GetExtensions(this ServiceScope scope) => scope.Get<ExtensionCollection>();

        public static CommandCollection GetCommands(this ServiceScope scope) => scope.Get<CommandCollection>();

        public static WorkspaceCollection GetWorkspaces(this ServiceScope scope) => scope.Get<WorkspaceCollection>();

        public static Manager GetManager(this ServiceScope scope) => scope.Get<Manager>();

        public static Command GetReplCommand(this ServiceScope scope) => scope.Get<Command>(ReplCommandId);

        public static Command GetCliCommand(this ServiceScope scope) => scope.Get<Command>(CliCommandId);
    }
}