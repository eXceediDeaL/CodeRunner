using CodeRunner.Commands;
using CodeRunner.Diagnostics;
using CodeRunner.Extensions;
using CodeRunner.Extensions.Helpers;
using CodeRunner.Extensions.Helpers.Rendering;
using CodeRunner.Helpers;
using CodeRunner.Loggings;
using CodeRunner.Managements;
using CodeRunner.Managements.Extensions;
using CodeRunner.Pipelines;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.IO;
using System.Threading.Tasks;
using Builder = CodeRunner.Pipelines.PipelineBuilder<string[], CodeRunner.Pipelines.Wrapper<int>>;

namespace CodeRunner
{
    public static class MainPipelineExtensions
    {
        public static Builder ConfigureCliCommand(this Builder builder) => builder.Configure(nameof(ConfigureCliCommand),
            scope =>
            {
                Command command = new CliCommand().Build();
                scope.Add<Command>(command, ServicesExtensions.CliCommandId);
            });

        public static Builder ConfigureConsole(this Builder builder, IConsole console, TextReader input) => builder.Configure(nameof(ConfigureConsole),
            scope =>
            {
                scope.Add<IConsole>(console);
                scope.Add<TextReader>(input);
            });

        public static Builder ConfigureLogger(this Builder builder, ILogger logger) => builder.Configure(nameof(ConfigureLogger),
            scope => scope.Add<ILogger>(logger));

        public static Builder ConfigureHost(this Builder builder, IHost host) => builder.Configure(nameof(ConfigureHost),
            scope => scope.Add<IHost>(host));

        #region Extensions

        public static Builder UseExtensionsLoading(this Builder builder) => builder.UseExtensionsService().UseCommandsService().UseWorkspacesService();

        public static Builder UseExtensionsService(this Builder builder) => builder.Use(nameof(UseExtensionsService),
            async context =>
            {
                ExtensionCollection res = new ExtensionCollection();
                Manager manager = context.Services.GetManager();

                res.Load(new ExtensionLoader(typeof(Extensions.Builtin.Console.ConsoleExtension).Assembly));
                res.Load(new ExtensionLoader(typeof(Extensions.Builtin.Workspace.WorkspaceExtension).Assembly));

                await res.LoadFromManager(manager, context.Logs);

                context.Services.Add<ExtensionCollection>(res);
                return context.IgnoreResult();
            });

        public static Builder UseCommandsService(this Builder builder) => builder.Use(nameof(UseCommandsService),
            context =>
            {
                CommandCollection res = new CommandCollection();
                foreach (ExtensionLoader ext in context.Services.GetExtensions())
                {
                    foreach (Extensions.Commands.ICommandBuilder v in ext.Commands)
                        res.Register(v, ext.Extension);
                }
                context.Services.Add<CommandCollection>(res);
                return Task.FromResult(context.IgnoreResult());
            });

        public static Builder UseWorkspacesService(this Builder builder) => builder.Use(nameof(UseWorkspacesService),
            context =>
            {
                WorkspaceCollection res = new WorkspaceCollection();
                foreach (ExtensionLoader ext in context.Services.GetExtensions())
                {
                    foreach (Extensions.Managements.IWorkspaceProvider v in ext.Workspaces)
                        res.Register(v, ext.Extension);
                }
                context.Services.Add<WorkspaceCollection>(res);
                return Task.FromResult(context.IgnoreResult());
            });

        #endregion

        public static Builder UseReplCommandService(this Builder builder) => builder.Use(nameof(UseReplCommandService),
            context =>
            {
                Command command = new ReplCommand().Build();
                foreach (Command cmd in context.Services.GetCommands())
                {
                    command.AddCommand(cmd);
                }
                context.Services.Add<Command>(command, ServicesExtensions.ReplCommandId);
                return Task.FromResult(context.IgnoreResult());
            });

        public static Builder UseTestView(this Builder builder) => builder.Use(nameof(UseTestView),
            context =>
            {
                TestView.Console = context.Services.GetConsole();
                TestView.Workspace = context.Services.GetWorkspace();
                return Task.FromResult(context.IgnoreResult());
            });

        public static Builder UseCliCommand(this Builder builder) => builder.Use(nameof(UseCliCommand),
            async context =>
            {
                Parser cliCommand = CommandLines.CreateDefaultParser(context.Services.GetCliCommand(), context);
                IConsole console = context.Services.GetConsole();
                int exitCode = await cliCommand.InvokeAsync(context.Origin, console);
                if (!context.Services.TryGet<string>(out _, ServicesExtensions.ArgWorkspaceNameId)) // No workspace id, cliCommand interrupt by --help or --version
                {
                    context.IsStopped = true;
                }
                return exitCode;
            });

        public static Builder UseInitialWorkspace(this Builder builder) => builder.Use(nameof(UseInitialWorkspace),
            async context =>
            {
                WorkspaceCollection workspaces = context.Services.GetWorkspaces();
                string workspaceName = context.Services.Get<string>(ServicesExtensions.ArgWorkspaceNameId);
                Extensions.Managements.IWorkspaceProvider? defaultWs = workspaces.GetWorkspaceProviderByName(workspaceName);
                Assert.IsNotNull(defaultWs);

                ITerminal terminal = context.Services.GetConsole().GetTerminal();
                Templates.BaseTemplate<IWorkspace> provider = defaultWs.Provider;
                Templates.ResolveContext resolveContext = new Templates.ResolveContext();
                if (terminal.FillVariables(context.Services.GetInput(), provider.GetVariables(), resolveContext))
                {
                    context.Services.Add<IWorkspace>(await provider.Resolve(resolveContext));
                    return context.IgnoreResult();
                }
                else
                {
                    context.Logs.Error("Can't create workspace with not enough arguments.");
                    return -1;
                }
            });

        private static bool Prompt(PipelineContext context, ITerminal terminal)
        {
            IWorkItem? workItem = context.Services.GetWorkItem();
            if (workItem != null)
            {
                terminal.Output(workItem.Name);
            }
            terminal.Output("> ");
            return true;
        }

        public static Builder UseReplCommand(this Builder builder) => builder.Use(nameof(UseReplCommand),
            async context =>
            {
                ITerminal terminal = context.Services.GetConsole().GetTerminal();
                Command replCommand = context.Services.GetReplCommand();
                Parser replParser = CommandLines.CreateDefaultParser(replCommand, context);
                TextReader input = context.Services.GetInput();
                CommandCollection commands = context.Services.GetCommands();

                async Task<(bool, int)> doALine(string? line)
                {
                    if (line != null)
                    {
                        ParseResult result = replParser.Parse(line);
                        int exitCode;
                        ICommand currentCommand = result.CommandResult.Command;
                        if (currentCommand == replCommand)
                        {
                            exitCode = await replParser.InvokeAsync(result, terminal);
                        }
                        else
                        {
                            if (currentCommand is Command command && commands.Contains(command))
                            {
                                IExtension ext = commands.GetExtension(command);
                                string? targetCommandName = commands.GetBuilder(command).GetType().FullName;

                                context.Logs.Debug($"Command {targetCommandName} from extension {ext.Publisher}.{ext.Name} invoking.");
                                exitCode = await replParser.InvokeAsync(result, terminal);
                                context.Logs.Debug($"Command {targetCommandName} invoked with {exitCode}.");
                            }
                            else
                            {
                                exitCode = await replParser.InvokeAsync(result, terminal);
                            }
                        }

                        ExtensionHost host = (ExtensionHost)context.Services.GetHost();
                        if (host.RequestShutdown || host.RequestRestart)
                        {
                            return (true, exitCode);
                        }
                        if (exitCode != 0)
                        {
                            terminal.OutputErrorLine($"Executed with code {exitCode}.");
                        }
                        return (false, exitCode);
                    }
                    else
                    {
                        return (false, 0);
                    }
                }

                {
                    string cmd = context.Services.Get<string>(ServicesExtensions.ArgCommandId);
                    if (!string.IsNullOrEmpty(cmd))
                    {
                        return (await doALine(cmd)).Item2;
                    }
                }

                terminal.OutputLine(Environment.CurrentDirectory);

                while (Prompt(context, terminal) && !input.IsEndOfInput())
                {
                    string? line = input.InputLine();
                    if ((await doALine(line)).Item1)
                    {
                        break;
                    }
                }

                return 0;
            });
    }
}