namespace InformationSearchBasics.VectorSearch
{
    public class RelevantDocumentInfo
    {
        public RelevantDocumentInfo(double value, string documentName)
        {
            DocumentName = documentName;
            Value = value;
        }

        public string DocumentName { get; set; }

        public double Value { get; set; }
    }
}
