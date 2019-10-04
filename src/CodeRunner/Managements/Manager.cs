using CodeRunner.IO;
using CodeRunner.Templates;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public class Manager : IManager<CRSettings>
    {
        internal const string PCRDir = ".cr";
        internal const string PExtensionDir = "extensions";
        internal const string PSettings = "settings.json";
        private readonly JsonFileLoader<CRSettings> _settingsLoader;

        public Manager(DirectoryInfo pathRoot)
        {
            PathRoot = pathRoot;
            CRRoot = new DirectoryInfo(Path.Join(PathRoot.FullName, PCRDir));
            ExtensionRoot = new DirectoryInfo(Path.Join(CRRoot.FullName, PExtensionDir));
            _settingsLoader = new JsonFileLoader<CRSettings>(new FileInfo(Path.Join(CRRoot.FullName, PSettings)));
        }

        public DirectoryInfo PathRoot { get; }

        public DirectoryInfo CRRoot { get; }

        public Task<CRSettings?> Settings => _settingsLoader.GetData();

        public DirectoryInfo ExtensionRoot { get; }

        public bool HasInitialized => CRRoot.Exists;

        public IEnumerable<ExtensionMetadata> GetExtensions()
        {
            foreach (DirectoryInfo v in ExtensionRoot.GetDirectories())
            {
                yield return new ExtensionMetadata(v.FullName, v.Name);
            }
        }

        public Task Clear()
        {
            CRRoot.Delete(true);
            return Task.CompletedTask;
        }

        public async Task Initialize() => _ = await new ManagerTemplate().ResolveTo(new ResolveContext(), PathRoot.FullName);
    }
}
