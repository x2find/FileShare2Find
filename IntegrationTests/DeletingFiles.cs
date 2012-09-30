using System.IO;
using EPiServer.Find;
using FileShare2Find.Document;
using Machine.Specifications;

namespace IntegrationTests
{
    public class given_that_a_document_is_deleted_in_the_watched_folders
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
                                     fileInfo.Delete();
                                     Watcher.Instance.AwaitRefresh();
                                     searchResults = searchService.Search<FileShareDocument>()
                                         .For("title")
                                         .GetResult();
                                 };

        It should_result_in_a_empty_result = () => searchResults.TotalMatching.ShouldEqual(0);

        private Cleanup after = () =>
                                    {
                                        Watcher.Instance.StopWatcher();
                                    };
    }
}
