using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeRunner.Test
{
    public static class ExtensionDI
    {
        public static List<Assembly> ExtensionAssemblies { get; } = new List<Assembly>();
    }
}
