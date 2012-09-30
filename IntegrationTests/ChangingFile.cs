using System.IO;
using EPiServer.Find;
using FileShare2Find.Document;
using Machine.Specifications;

namespace IntegrationTests
{
    public class given_that_a_document_is_changed_in_the_watched_folders_when_searching_for_the_new_word
    {
        private static TestSupport testSupport = new TestSupport();
        private static IClient searchService;
        private static FileInfo fileInfo;
        static SearchResults<FileShareDocument> searchResults;
        private static string path = "C:\\test\\";

        private Establish context = () =>
        {
            Watcher.Instance.StartWatcher();
            Watcher.Instance.AwaitRefresh();
            fileInfo = testSupport.CreateTestFileAndCopyToFolder("C:\\temp\\");
            Watcher.Instance.AwaitRefresh();

        };

        private Because of = () =>
        {
            searchService = testSupport.GetSearchClient();
            using (StreamWriter sw = fileInfo.AppendText())
            {
                sw.WriteLine("Banana split");
            }	

            Watcher.Instance.AwaitRefresh();
            searchResults = searchService.Search<FileShareDocument>()
                .For("Banana")
                .GetResult();
        };

        It should_result_in_a_single_hit = () => searchResults.TotalMatching.ShouldEqual(1);

        private Cleanup after = () =>
        {
            searchService.Delete<FileShareDocument>(x => x.Name.Match("TempFile.txt"));
            fileInfo.Delete();
            Watcher.Instance.StopWatcher();
        };
    }
}
