using CodeRunner;
using CodeRunner.Helpers;
using CodeRunner.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Test.App
{
    [TestClass]
    public class Integrate
    {
        private async Task UsingInput(string content, Func<TextReader, Task> action)
        {
            using MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            using StreamReader sr = new StreamReader(ms);
            await action(sr);
        }

        /*async Task<int[]> Execute(string workingDirectory, params string[] commands)
        {
            List<int> res = new List<int>();
            foreach (var cmd in commands)
            {
                res.Add(await Program.Main(new string[] { "-d", workingDirectory, "-c", cmd, "--verbose" }));
            }
            return res.ToArray();
        }*/

        private async Task<int> ExecuteInRepl(string workingDirectory, params string[] commands)
        {
            int res = -1;
            await UsingInput(string.Join('\n', commands), async input =>
            {
                TestView.Input = input;
                res = await Program.Main(new string[] { "-d", workingDirectory, "--verbose" });
            });
            return res;
        }

        [TestInitialize]
        public void Setup() => Program.Environment = EnvironmentType.Test;

        [TestMethod]
        public async Task Basic()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "--version"));
        }

        [TestMethod]
        public async Task Init()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "init"));

            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "init --delete"));
        }

        [TestMethod]
        public async Task NewNowDir()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "init", "new c a"));
            Assert.IsTrue(File.Exists(Path.Join(td.Directory.FullName, "a.c")));
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "now -f a.c"));
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "new dir a", "now -d a", "run dir"));
        }

        [TestMethod]
        public async Task Run()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "init", "run hello -- name=sun", "\n"));
            StringAssert.Contains(TestView.Console!.Out.ToString(), "hello sun");
        }

        [TestMethod]
        public async Task Debug()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "debug"));
        }

        [TestMethod]
        public async Task Clear()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "clear"));
        }

        [TestMethod]
        public async Task Template()
        {
            using TempDirectory td = new TempDirectory();
            Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "init", "template list"));
            StringAssert.Contains(TestView.Console!.Out.ToString(), "python");
        }

        [TestMethod]
        public async Task Operation()
        {
            {
                using TempDirectory td = new TempDirectory();
                Assert.AreEqual(0, await ExecuteInRepl(td.Directory.FullName, "init", "operation list"));
                StringAssert.Contains(TestView.Console!.Out.ToString(), "hello");
            }
        }
    }
}
