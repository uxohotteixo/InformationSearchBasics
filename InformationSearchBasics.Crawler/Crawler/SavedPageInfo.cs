namespace InformationSearchBasics.Crawler.Crawler
{
    public class SavedPageInfo
    {
        public SavedPageInfo(string urlAddress, string htmlDocumentFileName)
        {
            UrlAddress = urlAddress;
            HtmlDocumentFileName = htmlDocumentFileName;
        }

        public string UrlAddress { get; set; }

        public string HtmlDocumentFileName { get; set; }
    }
}