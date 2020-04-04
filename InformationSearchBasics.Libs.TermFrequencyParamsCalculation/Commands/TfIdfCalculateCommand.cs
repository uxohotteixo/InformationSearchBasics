using System;
using System.Collections.Generic;
using System.Linq;
using InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Common;

namespace InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Commands
{
    public class TfIdfCalculateCommand
    {
        private readonly IEnumerable<DocumentBasedFrequencyCalculationResult> _tfCalculation;
        private readonly IEnumerable<FrequencyCalculationResultBase> _idfCalculation;

        public TfIdfCalculateCommand(
            IEnumerable<DocumentBasedFrequencyCalculationResult> tfCalculation,
            IEnumerable<FrequencyCalculationResultBase> idfCalculation
            )
        {
            _tfCalculation = tfCalculation;
            _idfCalculation = idfCalculation;
        }

        public IEnumerable<DocumentBasedFrequencyCalculationResult> Handle()
        {
            return _tfCalculation.Join(_idfCalculation,
                tf => tf.Term,
                idf => idf.Term,
                (tf, idf)
                    => new DocumentBasedFrequencyCalculationResult(tf.Term, Math.Round(tf.Value * idf.Value, 10), tf.DocumentName))
                .OrderByDescending(f => f.Value)
                .ThenBy(f => f.Term)
                .ToArray();
        }
    }
}
