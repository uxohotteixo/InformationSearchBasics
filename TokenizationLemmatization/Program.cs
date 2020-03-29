using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyStem.Sharp;

namespace TokenizationLemmatization
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var projectPath = System.IO.Path.GetFullPath(@"..\..\..\..\");
            var documentsFolderPath = Path.Combine(projectPath, "CrawlerResults");

            var resultFolderPath = Path.Combine(projectPath, "LemmatizationResult");

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
                        .Where(x => !string.IsNullOrEmpty(x));

                    var tokenized = string.Join(" ", tokens);

                    var lemmas = new Lemmatizer("./mystem/mystem.exe").LemmatizeText(tokenized);

                    var a = file
                        .Replace(documentsFolderPath + "\\", "")
                        .Replace(".html", ".txt");

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