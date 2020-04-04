using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using InformationSearchBasics.Constants;
using InformationSearchBasics.Libs.Lemmatizer;

namespace InformationSearchBasics.TokenizationLemmatization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var documentsFolderPath = PathConstants.CrawlerResultPath;

            var resultFolderPath = PathConstants.LemmatizationResultPath;

            if (!Directory.Exists(resultFolderPath))
                Directory.CreateDirectory(resultFolderPath);

            var regex = new Regex(@"[\p{IsCyrillic}]+");

            foreach (var file in Directory.EnumerateFiles(documentsFolderPath, "*.html"))
            {
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    var document = new HtmlDocument();
                    document.Load(fs);

                    var rootNode = document.DocumentNode;

                    if (rootNode == null) continue;

                    var tokens = regex.Matches(rootNode.InnerText)
                        .Select(x => x.ToString())
                        .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x));

                    var tokenized = string.Join(" ", tokens);

                    var lemmas = new Lemmatizer("./mystem/mystem.exe").LemmatizeText(tokenized)
                        .Trim()
                        .Replace("   ", " ");

                    var outputPath = Path.Combine(resultFolderPath, file
                        .Replace(documentsFolderPath + "\\", "")
                        .Replace(".html", ".txt"));

                    using (var stream = new FileStream(outputPath, FileMode.OpenOrCreate))
                    {
                        var buffer = Encoding.UTF8.GetBytes(lemmas);
                        await stream.WriteAsync(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }
}