namespace InformationSearchBasics.Libs.Csv.Csv
{
    public class CsvFileInfo
    {
        public CsvFileInfo(string saveToPath, string delimeter)
        {
            SaveToPath = saveToPath;
            Delimeter = delimeter;
        }

        public string SaveToPath { get; set; }

        public string Delimeter { get; set; }
    }
}