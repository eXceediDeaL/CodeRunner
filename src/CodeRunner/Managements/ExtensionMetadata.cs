namespace CodeRunner.Managements
{
    public class ExtensionMetadata
    {
        public ExtensionMetadata(string rootPath, string name)
        {
            RootPath = rootPath;
            Name = name;
        }

        public string RootPath { get; }

        public string Name { get; }
    }
}
