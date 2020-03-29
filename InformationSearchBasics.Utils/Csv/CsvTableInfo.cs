using System;
using System.Collections.Generic;

namespace InformationSearchBasics.Utils.Csv
{
    public class CsvTableInfo
    {
        public CsvTableInfo(IEnumerable<string> columns, IEnumerable<IEnumerable<object>> tableData)
        {
            Columns = columns ?? throw new ArgumentNullException();
            TableData = tableData ?? throw new ArgumentNullException();
        }

        public IEnumerable<string> Columns { get; set; }

        public IEnumerable<IEnumerable<object>> TableData { get; set; }
    }
}