using System.IO;

namespace InformationSearchBasics.Constants
{
    public class PathConstants
    {
        public static string BaseResultPath = Path.GetFullPath(@"..\..\..\..\Results");

        public static string CrawlerResultPath = Path.Combine(BaseResultPath, "CrawlerResults");

        public static string LemmatizationResultPath = Path.Combine(BaseResultPath, "LemmatizationResult");

        public static string SearchParamsPath = Path.Combine(BaseResultPath, "Statistic");
    }
}
