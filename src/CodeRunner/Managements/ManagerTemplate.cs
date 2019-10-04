using CodeRunner.IO;
using CodeRunner.Templates;
using System.IO;
using System.Threading.Tasks;

namespace CodeRunner.Managements
{
    public class ManagerTemplate : DirectoryTemplate
    {
        public ManagerTemplate()
        {
            PackageDirectoryTemplate crRoot = Package.AddDirectory(Manager.PCRDir).WithAttributes(FileAttributes.Hidden);
            _ = crRoot.AddDirectory(Manager.PExtensionDir);
            _ = crRoot.AddFile(Manager.PSettings).UseTemplate(new TextFileTemplate(new StringTemplate(JsonFormatter.Serialize(new CRSettings()))));
        }

        private PackageDirectoryTemplate Package { get; set; } = new PackageDirectoryTemplate();

        public override Task<DirectoryInfo> ResolveTo(ResolveContext context, string path) => Package.ResolveTo(context, path);
    }
}
