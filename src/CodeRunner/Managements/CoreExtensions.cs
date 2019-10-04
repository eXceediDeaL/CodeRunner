
using CodeRunner.Extensions.Managements;
using CodeRunner.Loggings;
using CodeRunner.Managements.Extensions;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public static class CoreExtensions
    {
        public static IWorkspaceProvider? GetWorkspaceProviderByName(this WorkspaceCollection collection, string name)
        {
            foreach (IWorkspaceProvider v in collection.GetProviders())
            {
                if (v.Name == name)
                    return v;
            }
            return null;
        }

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
