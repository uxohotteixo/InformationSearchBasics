using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Common;

namespace InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Commands
{
    public class TfCalculateCommand
    {
        private readonly string _docsFolderPath;

        public TfCalculateCommand(string docsFolderPath)
        {
            _docsFolderPath = docsFolderPath;
        }

        public IEnumerable<DocumentBasedFrequencyCalculationResult> Handle()
        {
            var result = new List<DocumentBasedFrequencyCalculationResult>();

            foreach (var fileName in Directory.EnumerateFiles(_docsFolderPath, "*.txt"))
            {
                var fileText = File.ReadAllText(fileName);

                if (string.IsNullOrEmpty(fileText)) continue;

                var tfParamsList = fileText
                    .Split(' ')
                    .Where(w => !string.IsNullOrEmpty(w))
                    .GroupBy(w => w)
                    .Select(g => new DocumentBasedFrequencyCalculationResult(
                        g.Key,
                         Math.Round(g.Count() / (double)fileText.Length, 5),
                        fileName.Replace(_docsFolderPath + "\\", "")));

                result.AddRange(tfParamsList);
            }

            return result.OrderByDescending(tfp => tfp.Value)
                .ThenBy(tfp => tfp.Term);
        }
    }
}
