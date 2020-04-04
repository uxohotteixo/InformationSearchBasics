using System.IO;
using System.Linq;
using InformationSearchBasics.Constants;
using InformationSearchBasics.Libs.Csv.Csv;
using InformationSearchBasics.Libs.TermFrequencyParamsCalculation.Commands;

namespace InformationSearchBasics.SearchParamsCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourcePath = Path.Combine(PathConstants.LemmatizationResultPath);
            var resultPath = Path.Combine(PathConstants.SearchParamsPath);

            if (!Directory.Exists(resultPath))
                Directory.CreateDirectory(resultPath);

            // Tf data prepared.
            var tfCols = new[] { "term", "tf", "document" };
            var tfCalcResult = new TfCalculateCommand(sourcePath).Handle().ToArray();
            var tfTableData = tfCalcResult
                .Select(tfp => new object[] {
                    tfp.Term,
                    tfp.Value,
                    tfp.DocumentName });

            new CsvWriter(
                    new CsvFileInfo(Path.Combine(resultPath, "tf.csv"), ","),
                    new CsvTableInfo(tfCols, tfTableData))
                .Write();

            // Idf data prepared.
            var idfCols = new[] { "term", "idf" };
            var idfCalcResult = new IdfCalculateCommand(sourcePath).Handle().ToArray();
            var idfTableData = idfCalcResult
                .Select(tfp => new object[] {
                    tfp.Term,
                    tfp.Value });

            new CsvWriter(
                    new CsvFileInfo(Path.Combine(resultPath, "idf.csv"), ","),
                    new CsvTableInfo(idfCols, idfTableData))
                .Write();

            // Tf-Idf data prepared.
            var tfIdfCols = new[] { "term", "tf-idf", "document"};
            var tfIdfTableData = new TfIdfCalculateCommand(tfCalcResult, idfCalcResult)
                .Handle()
                .Select(tfp => new object[] {
                    tfp.Term,
                    tfp.Value,
                    tfp.DocumentName
                });

            new CsvWriter(
                    new CsvFileInfo(Path.Combine(resultPath, "tf-idf.csv"), ","),
                    new CsvTableInfo(tfIdfCols, tfIdfTableData))
                .Write();
        }
    }
}
