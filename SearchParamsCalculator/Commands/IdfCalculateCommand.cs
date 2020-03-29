using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SearchParamsCalculator.Common;

namespace SearchParamsCalculator.Commands
{
    internal class IdfCalculateCommand
    {
        private readonly string _docsFolderPath;

        public IdfCalculateCommand(string docsFolderPath)
        {
            _docsFolderPath = docsFolderPath;
        }

        public IEnumerable<FrequencyCalculationResultBase> Handle()
        {
            var result = new List<FrequencyCalculationResultBase>();
            var dict = new Dictionary<string, IEnumerable<string>>();

            var docs = Directory.EnumerateFiles(_docsFolderPath, "*.txt").ToArray();
            var docsCount = docs.Length;

            foreach (var fileName in docs)
            {
                var tokens = File.ReadAllText(fileName).Split(' ').Where(w => !string.IsNullOrEmpty(w));
                var replacedName = fileName.Replace(_docsFolderPath + "\\", "");

                foreach (var token in tokens)
                {
                    if (!dict.ContainsKey(token))
                        dict.Add(token, new[] { replacedName });
                    else
                        dict[token] = dict[token].Append(replacedName);
                }
            }

            var keysArr = dict.Keys.ToArray();
            foreach (var key in keysArr)
            {
                dict[key] = dict[key].Distinct();
            }

            return dict.Select(kv =>
                new FrequencyCalculationResultBase(
                    kv.Key,
                    Math.Round(Math.Log2((double)docsCount / kv.Value.Count()), 5)))
                .OrderBy(f => f.Value);
        }
    }
}