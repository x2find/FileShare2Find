using System.IO;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using FileShareToFind.Document;
using Machine.Specifications;

namespace IntegrationTests
{
    
    public class given_adding_a_document_to_the_watched_folders 
    {
        private static TestSupport testSupport = new TestSupport();
        private static IClient searchService;
        private static FileInfo fileInfo;
        static SearchResults<FileShareDocument> searchResults;
        private static string path = "C:\\test\\";
        Establish context = () =>
                                {
                                    Watcher.Instance.StartWatcher();
                                    Watcher.Instance.AwaitRefresh();
                                };

        private Because of = () =>
                                 {
                                     searchService = testSupport.GetSearchClient();
                                     fileInfo = testSupport.CreateTestFileAndCopyToFolder("C:\\temp\\");
                                     Watcher.Instance.AwaitRefresh();
                                     searchResults = searchService.Search<FileShareDocument>()
                                         .For("title")
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
