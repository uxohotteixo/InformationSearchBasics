using System;
using System.IO;
using System.Linq;

namespace InformationSearchBasics.Index
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Index is under construction, wait...");

            var index = new IndexBuilder(Path.Combine(System.IO.Path.GetFullPath(@"..\..\..\..\"), "LemmatizationResult"))
                .Build()
                .Index;

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
    }
}
