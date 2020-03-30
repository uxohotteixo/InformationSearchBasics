using System;
using System.Collections.Generic;
using System.Linq;
using InformationSearchBasics.SearchParamsCalculator.Common;

namespace InformationSearchBasics.SearchParamsCalculator.Commands
{
    internal class TfIdfCalculateCommand
    {
        private readonly string _docsFolderPath;

        public TfIdfCalculateCommand(string docsFolderPath)
        {
            _docsFolderPath = docsFolderPath;
        }

        public IEnumerable<DocumentBasedFrequencyCalculationResult> Handle(
            IEnumerable<DocumentBasedFrequencyCalculationResult> tfCalculation,
            IEnumerable<FrequencyCalculationResultBase> idfCalculation)
        {
            return tfCalculation.Join(idfCalculation,
                tf => tf.Term,
                idf => idf.Term,
                (tf, idf)
                    => new DocumentBasedFrequencyCalculationResult(tf.Term, Math.Round(tf.Value * idf.Value, 5), tf.DocumentName))
                .OrderByDescending(f => f.Value)
                .ThenBy(f => f.Term);
        }
    }
}
