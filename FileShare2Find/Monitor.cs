using System;
using System.IO;
using EPiServer.Find;
using FileShare2Find.Document;

namespace FileShare2Find
{
    public class Monitor
    {
        private IClient client = new Client("http://es-api01.episerver.com/EucUC5VgypCuGk7uuWeMl7HrvKQ72uEs/", "marcus_fileshare");

        public Monitor(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDelete;
            watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            while (Console.Read() != 'q');
        }

        //Add document to index
        private void OnCreated(object source, FileSystemEventArgs eventArgs)
        {
            var fileShareDocument = new FileShareDocument(eventArgs.Name, eventArgs.FullPath);
            client.Index(fileShareDocument);
            Console.WriteLine("File added: {0} {1}", eventArgs.FullPath, eventArgs.ChangeType);
        }

        //Re-Index it changed
        private void OnChanged(object source, FileSystemEventArgs eventArgs)
        {
            var fileShareDocument = new FileShareDocument(eventArgs.Name, eventArgs.FullPath);
            client.Index(fileShareDocument);
            Console.WriteLine("File added: {0} {1}", eventArgs.FullPath, eventArgs.ChangeType);
        }

        //Delete document from index
        private void OnDelete(object source, FileSystemEventArgs eventArgs)
        {
            var fileShareDocument = new FileShareDocument(eventArgs.Name);
            client.Delete<FileShareDocument>(fileShareDocument.NameHashed);
            Console.WriteLine("File Deleted: {0} ", eventArgs.FullPath);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {

            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
    

}
