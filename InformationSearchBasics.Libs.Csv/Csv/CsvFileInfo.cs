namespace InformationSearchBasics.Libs.Csv.Csv
{
    public class CsvFileInfo
    {
        public CsvFileInfo(string saveToPath, string delimeter, bool isExist = false)
        {
            SaveToPath = saveToPath;
            Delimeter = delimeter;
            IsExist = isExist;
        }

        public string SaveToPath { get; set; }

        public bool IsExist { get; set; }

        public string Delimeter { get; set; }
    }
}