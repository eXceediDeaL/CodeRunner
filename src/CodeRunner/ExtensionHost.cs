namespace CodeRunner
{
    public class ExtensionHost : Extensions.IHost
    {
        public bool RequestShutdown { get; private set; }

        public bool RequestRestart { get; private set; }

        public void Shutdown() => RequestShutdown = true;

        public void Restart() => RequestRestart = true;

        public void SendMessage(string message)
        {

        }
    }
}