using System;

namespace Crawler.Crawler
{
    public class CrawlerOptions
    {
        public CrawlerOptions(Uri targetUri, int maxDepth, int maxPagesCount, int minWordsCount, string resultFilesFolderPath)
        {
            MaxDepth = maxDepth;
            MaxPagesCount = maxPagesCount;
            TargetUri = targetUri;
            MinWordsCount = minWordsCount;
            ResultFilesFolderPath = resultFilesFolderPath;
        }

        public int MaxDepth { get; protected set; }

        public Uri TargetUri { get; protected set; }

        public int MaxPagesCount { get; protected set; }

        public int MinWordsCount { get; protected set; }

        public string ResultFilesFolderPath { get; protected set; }
    }
}