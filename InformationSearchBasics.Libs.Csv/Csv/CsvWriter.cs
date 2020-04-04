using System.Globalization;
using System.IO;
using System.Linq;

namespace InformationSearchBasics.Libs.Csv.Csv
{
    public class CsvWriter
    {
        private readonly CsvFileInfo _csvFileInfo;

        private readonly CsvTableInfo _csvTableInfo;

        public CsvWriter(CsvFileInfo csvFileInfo, CsvTableInfo csvTableInfo)
        {
            _csvFileInfo = csvFileInfo;
            _csvTableInfo = csvTableInfo;
        }

        public void Write()
        {
            var delimeter = _csvFileInfo.Delimeter;
            using (var fs = new FileStream(_csvFileInfo.SaveToPath, FileMode.OpenOrCreate))
            using (var sw = new StreamWriter(fs))
            {
                var cols = string.Join(delimeter, _csvTableInfo.Columns);
                sw.WriteLine(cols);

                var nfi = new NumberFormatInfo {NumberDecimalSeparator = "."};

                foreach (var row in _csvTableInfo.TableData)
                {
                    sw.WriteLine(string.Join(delimeter, row
                        .Select(x => x is double arg ? arg.ToString(nfi) : x.ToString())));
                }
            }
        }
    }
}
