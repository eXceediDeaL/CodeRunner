using CodeRunner.Extensions.Helpers;
using CodeRunner.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.App.Helpers
{
    [TestClass]
    public class TResolveContext
    {
        [TestMethod]
        public void Basic()
        {
            ResolveContext context = new ResolveContext();
            _ = context.FromArgumentList(new string[] { "a=a", "b=b", "c=" });
            Assert.AreEqual("a", context.GetVariable<string>(new Variable("a")));
            Assert.AreEqual("b", context.GetVariable<string>(new Variable("b")));
            Assert.AreEqual("", context.GetVariable<string>(new Variable("c")));
        }
    }
}
