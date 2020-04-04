using System;
using System.Collections.Generic;
using System.Linq;
using InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Common;

namespace InformationSearchBasics.VectorSearch
{
    public class GetRelevantDocumentsCommand
    {
        private readonly string _searchQuery;
        private readonly IEnumerable<DocumentBasedFrequencyCalculationResult> _tfIdfParams;

        private IEnumerable<string> Words => _searchQuery
            .Trim()
            .Split(' ')
            .Select(w => w.ToLower())
            .ToArray();

        private double[] SearchQueryVector => ProduceVector(_tfIdfParams);

        public GetRelevantDocumentsCommand(string searchQuery, IEnumerable<DocumentBasedFrequencyCalculationResult> tfIdfParams)
        {
            _searchQuery = searchQuery;
            _tfIdfParams = tfIdfParams;
        }

        public IEnumerable<RelevantDocumentInfo> Handle()
            => _tfIdfParams
                .GroupBy(p => p.DocumentName)
                .Select(g 
                    => new RelevantDocumentInfo(CosineTheta(SearchQueryVector, ProduceVector(g.Select(v => v).ToArray())), g.Key))
                .Where(rd => rd.Value > 0)
                .OrderByDescending(rd => rd.Value)
                .ToArray();

        private double[] ProduceVector(IEnumerable<DocumentBasedFrequencyCalculationResult> tfIdfParams)
            => Words.Select(w => tfIdfParams
                .FirstOrDefault(p => p.Term == w)?.Value ?? 0)
                .ToArray();

        private double GetVectorLength(IEnumerable<double> vector)
            => vector.Aggregate((acc, current) => Math.Sqrt(current));

        private double CosineTheta(double[] v1, double[] v2)
        {
            var lengthV1 = GetVectorLength(v1);
            var lengthV2 = GetVectorLength(v2);

            if (lengthV1 == 0 || lengthV2 == 0) return 0;

            var dotProd = GetDotProduct(v1, v2);

            return dotProd / (lengthV1 * lengthV2);
        }

        private double GetDotProduct(double[] v1, double[] v2)
            => v1.Length != v2.Length ? 0d : v1.Select((t, i) => t * v2[i]).Sum();
    }
}
