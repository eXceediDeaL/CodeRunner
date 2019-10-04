namespace CodeRunner
{
    public class ExtensionHost : Extensions.IHost
    {
        public bool RequestShutdown { get; private set; }

        public void Shutdown() => RequestShutdown = true;
    }
}