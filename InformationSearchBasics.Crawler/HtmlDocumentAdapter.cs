using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace InformationSearchBasics.Crawler
{
    public class HtmlDocumentAdapter
    {
        private HtmlDocument OriginalDocument { get; set; }

        public HtmlDocumentAdapter(Stream htmlDocStream)
        {
            OriginalDocument = new HtmlDocument();
            OriginalDocument.Load(htmlDocStream);
        }

        public int GetInnerTextLength()
        {
            var rootNode = OriginalDocument.DocumentNode;
            return rootNode != null ? new Regex(@"[\p{IsCyrillic}]+")
                .Matches(rootNode.InnerText)
                .Select(x => x.ToString())
                .Count() : 0;
        }

        public IEnumerable<string> GetDocumentHrefUrls(string baseUrl)
        {
            var hrefNodes = OriginalDocument.DocumentNode
                .SelectNodes("//a[@href]");

            if (hrefNodes != null)
            {
                return hrefNodes
                    .Where(node => node != null)
                    .Select(node => node.Attributes["href"].Value)
                    .Where(url => !string.IsNullOrEmpty(url))
                    .Select(url => NormalizeUrl(baseUrl, url.Trim()))
                    .Where(url => url.StartsWith("http") || url.StartsWith("https"));
            }

            return Enumerable.Empty<string>();
        }

        private static string NormalizeUrl(string baseUrl, string url)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
                uri = new Uri(new Uri(baseUrl), uri);
            return uri.ToString();
        }

        public override string ToString()
        {
            return OriginalDocument.DocumentNode.OuterHtml;
        }
    }
}