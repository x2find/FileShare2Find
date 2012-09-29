using System;
using System.IO;
using EPiServer.Find;

namespace IntegrationTests
{
    public class TestSupport
    {
        public FileInfo CreateTestFileAndCopyToFolder(string pathToCopyTo)
        {
            string path = Path.GetTempFileName();
            FileInfo fi1 = new FileInfo(path);

            //Create a file to write to. 
            using (StreamWriter sw = fi1.CreateText())
            {
                sw.WriteLine("Title");
                sw.WriteLine("Lots of happenings");
                sw.WriteLine("Bazinga");
            }

            try
            {
                //Copy the file.
                fi1.CopyTo(pathToCopyTo+"\\TempFile.txt");
                FileInfo fi2 = new FileInfo(pathToCopyTo + "\\TempFile.txt");

                return fi2;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            return null;
        }
        public IClient GetSearchClient()
        {
            return new Client("http://es-api01.episerver.com/EucUC5VgypCuGk7uuWeMl7HrvKQ72uEs/", "marcus_fileshare");
        }
    }
}
