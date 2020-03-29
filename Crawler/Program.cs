using System;
using System.IO;
using System.Threading.Tasks;
using Crawler.Crawler;

namespace Crawler
{
    internal class Program
    {
        static async Task Main()
        {
            var projectPath = Path.GetFullPath(@"..\..\..\..\");
            await new DefaultCrawler(new CrawlerOptions(
                    new Uri("https://habr.com/ru/"), 4, 100, 10,
                    Path.Combine(projectPath, "CrawlerResults")))
                .Crawl();
        }
    }
}