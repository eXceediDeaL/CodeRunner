using CodeRunner.Loggings;
using CodeRunner.Managements.Extensions;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public static class CoreExtensions
    {
        public static Task LoadFromManager(this ExtensionCollection extensions, Manager manager, LoggerScope logger)
        {
            if (manager.HasInitialized)
            {
                foreach (ExtensionMetadata v in manager.GetExtensions())
                {
                    try
                    {
                        ExtensionLoader loader = new ExtensionLoader(v.RootPath, v.Name);
                        extensions.Load(loader);
                    }
                    catch
                    {
                        logger.Warning($"Load extension at {v.RootPath} failed.");
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
