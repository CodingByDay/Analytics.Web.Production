using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dash.Models
{
    public class BatchEditDataEvent
    {
        public string RowKey { get; set; } // Matches e.key
        public string ColumnName { get; set; } // Matches e.focusedColumn.fieldName
        public string CellValue { get; set; } // Matches e.rowValues[e.focusedColumn.index]
    }
}