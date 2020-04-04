using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InformationSearchBasics.Libs.Csv.Csv;

namespace InformationSearchBasics.Crawler.Crawler
{
    public class DefaultCrawler
    {
        private readonly CrawlerOptions _options;

        private readonly Queue<string> _traversalQueue = new Queue<string>();

        private int Balance => _options.MaxPagesCount - _processedPagesCount;

        private int _processedPagesCount;

        private Uri RootSiteUri => _options.TargetUri;

        private readonly string _filesLocationPrefix;

        private readonly List<string> _crawledUrls = new List<string>();

        private readonly IDictionary<string, string> _indexed = new Dictionary<string, string>();

        public DefaultCrawler(CrawlerOptions options)
        {
            _options = options;
            _filesLocationPrefix = options.ResultFilesFolderPath;

            CreateResultDirIfNotExist();
            _traversalQueue.Enqueue(options.TargetUri.OriginalString);

        }

        public async Task Crawl()
        {
            var index = 1;

            while (_traversalQueue.Count != 0)
            {
                var pageUrl = _traversalQueue.Peek();

                var response = await GetResponse(pageUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _traversalQueue.Dequeue();
                    continue;
                }

                var htmlDoc = new HtmlDocumentAdapter(await response.Content.ReadAsStreamAsync());

                if (htmlDoc.GetInnerTextLength() < _options.MinWordsCount)
                {
                    _traversalQueue.Dequeue();
                    continue;
                }

                await SavePage(index, pageUrl, htmlDoc);
                _indexed.TryAdd(index + ".html", pageUrl);
                _processedPagesCount++;

                _crawledUrls.Add(_traversalQueue.Dequeue());

                if (Balance != 0)
                {
                    UpdateQueue(htmlDoc);
                }

                index++;
            }

            if (_indexed.Count != 0) SaveIndex();
        }

        private void UpdateQueue(HtmlDocumentAdapter htmlDoc)
        {
            var urls = htmlDoc
                .GetDocumentHrefUrls(RootSiteUri.AbsoluteUri)
                .Except(_crawledUrls)
                .Distinct()
                .ToArray();

            if (urls.Length == 0) return;

            var limit = _traversalQueue.Count >= Balance ? 0 : Balance - _traversalQueue.Count;
            var limitedUrls = limit > urls.Length ? urls : urls.Take(limit).ToArray();
            foreach (var url in limitedUrls)
            {
                _traversalQueue.Enqueue(url);
            }
        }

        private void SaveIndex()
        {
            var csvFilePath = _filesLocationPrefix + "./" + "index.csv";
            new CsvWriter(
                new CsvFileInfo(csvFilePath, ","),
                new CsvTableInfo(new[] { "Document name", "Url" },
                    _indexed.Select(kv => new object[] { kv.Key, kv.Value })))
                .Write();
        }

        private void CreateResultDirIfNotExist()
        {
            if (!Directory.Exists(_filesLocationPrefix))
                Directory.CreateDirectory(_filesLocationPrefix);
        }

        private async Task<HttpResponseMessage> GetResponse(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(url);
            }
        }

        private async Task SavePage(long pageNumber, string pageUrl, HtmlDocumentAdapter html)
        {
            using (var stream1 = new FileStream(_filesLocationPrefix + "/" + pageNumber + ".html", FileMode.OpenOrCreate))
            {
                var buffer = Encoding.UTF8.GetBytes(html.ToString());
                await stream1.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}