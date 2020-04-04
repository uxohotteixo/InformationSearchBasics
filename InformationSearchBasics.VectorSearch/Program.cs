using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using InformationSearchBasics.Constants;
using InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Commands;

namespace InformationSearchBasics.VectorSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourcePath = Path.Combine(PathConstants.LemmatizationResultPath);

            while (true)
            {
                Console.Write("Input search query: ");

                var query = Console.ReadLine() ?? throw new ArgumentNullException();
                var normalizer = query.ToLower().Trim();

                var searchResult =
                    new GetRelevantDocumentsCommand(normalizer,
                            new TfIdfCalculateCommand(
                                    new TfCalculateCommand(sourcePath).Handle(),
                                    new IdfCalculateCommand(sourcePath).Handle())
                                .Handle())
                        .Handle();

                if (!searchResult.Any())
                {
                    Console.WriteLine($"On request {query} nothing found.\r\n");
                    continue;
                }

                foreach (var item in searchResult)
                {
                    Console.WriteLine($"Search key param (Cosine theta): {item.Value}");
                    Console.WriteLine($"Relevant document: {item.DocumentName}");
                }
                Console.WriteLine();
            }
        }
    }
}