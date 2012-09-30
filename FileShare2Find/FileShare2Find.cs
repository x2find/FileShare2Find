using System;

namespace FileShare2Find
{
    public class FileShare2Find
    {
        public static void Main()
        {
            string[] args = System.Environment.GetCommandLineArgs();

            // If a directory is not specified, exit program.
            if (args.Length != 2)
            {
                // Display the proper way to call the program.
                Console.WriteLine("Usage: FileShareToFind.exe (directory)");
                return;
            }
            new Monitor(args[1]);
        }
    }
}
