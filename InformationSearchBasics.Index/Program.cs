using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InformationSearchBasics.Constants;
using InformationSearchBasics.Libs.Csv.Csv;

namespace InformationSearchBasics.Index
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Index is under construction, wait...");

            var index = new IndexBuilder(Path.Combine(PathConstants.LemmatizationResultPath))
                .Build()
                .Index;

            Console.WriteLine("Index was built successfully. Saving result... \n\r");

            SaveIndex(index);

            Console.WriteLine("Test Boolean Search...\n\r");

            while (true)
            {
                var resultDocs = new DocumentsProvider(index)
                    .Provide(Console.ReadLine() ?? throw new ArgumentNullException())
                    .ToList();

                Console.WriteLine(resultDocs.Count != 0
                    ? string.Join(" ", resultDocs.OrderBy(x => x)) + "\n\r"
                    : "There are no results for this request...\n\r");
            }
        }

        private static void SaveIndex(Dictionary<string, IEnumerable<string>> index)
        {
            if (!Directory.Exists(PathConstants.InvertedIndexResultPath))
                Directory.CreateDirectory(PathConstants.InvertedIndexResultPath);

            new CsvWriter(
                    new CsvFileInfo(Path.Combine(PathConstants.InvertedIndexResultPath, "index.csv"), ","),
                    new CsvTableInfo(new[] {"Word", "Documents"},
                        index.Select(x => new[] {x.Key, string.Join(", ", x.Value)})))
                .Write();
        }
    }
}
