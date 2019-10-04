using CodeRunner.Managements;
using System.CommandLine;
using System.IO;

namespace CodeRunner.Helpers
{
    public static class TestView
    {
        public static IConsole? Console { get; internal set; }

        public static IWorkspace? Workspace { get; internal set; }

        public static TextReader? Input { get; set; }
    }
}
