using System.IO;
using System.Linq;
using InformationSearchBasics.Utils.Csv;
using SearchParamsCalculator.Commands;

namespace SearchParamsCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourcePath = Path.Combine(System.IO.Path.GetFullPath(@"..\..\..\..\"), "LemmatizationResult");
            var resultPath = Path.Combine(System.IO.Path.GetFullPath(@"..\..\..\..\"), "Statistic");

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
            var tfIdfTableData = new TfIdfCalculateCommand(sourcePath)
                .Handle(tfCalcResult, idfCalcResult)
                .Select(tfp => new object[] {
                    tfp.Term,
                    tfp.Value });

            new CsvWriter(
                    new CsvFileInfo(Path.Combine(resultPath, "tf-idf.csv"), ","),
                    new CsvTableInfo(tfCols, tfTableData))
                .Write();
        }
    }
}
