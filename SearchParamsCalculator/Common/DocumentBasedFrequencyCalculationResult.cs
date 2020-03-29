namespace InformationSearchBasics.SearchParamsCalculator.Common
{
    internal class DocumentBasedFrequencyCalculationResult : FrequencyCalculationResultBase
    {
        public DocumentBasedFrequencyCalculationResult(string term, double value, string documentName) : base(term, value)
        {
            Value = value;
            DocumentName = documentName;
            Term = term;
        }

        public string DocumentName { get; set; }
    }
}