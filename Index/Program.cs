using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Index
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Index is under construction, wait...");

            var index = new IndexBuilder(Path.Combine(System.IO.Path.GetFullPath(@"..\..\..\..\"), "LemmatizationResult"))
                .Build()
                .Index;

            Console.WriteLine("Test Boolean Search...");

            while (true)
            {
                var resultDocs = new DocumentsProvider(index)
                    .Provide(Console.ReadLine() ?? throw new ArgumentNullException())
                    .ToList();

                if (resultDocs.Count != 0)
                    resultDocs.ForEach(Console.WriteLine);
                else
                    Console.WriteLine("There are no results for this request...");
            }
        }
    }
}
