using System.Diagnostics;
using System.Threading;

namespace IntegrationTests
{
    public class Watcher
    {
        private int AwaitRefreshTime = 2000;
        private static Watcher instance = new Watcher();
        public static Process WatcherProcess { get; set; }
        public void StartWatcher()
        {
            WatcherProcess = Process.Start("FileShareToFind.exe","C:\\temp\\");   
        }

        public void StopWatcher()
        {
            WatcherProcess.Kill();
        }

        public void AwaitRefresh()
        {
            Thread.Sleep(AwaitRefreshTime);
        }
        public static Watcher Instance { get { return instance; } }
    }
}
