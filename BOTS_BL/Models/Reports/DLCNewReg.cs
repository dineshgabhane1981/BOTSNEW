using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.Reports
{
    public class DLCNewReg
    {
        public Int64 Source { get; set; }
        public DateTime? SourceCreatedDate { get; set; }
        public string SourceDesc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Int64 UniqueRegCount { get; set; }
        public Int64 TotalTxnCount { get; set; }
        public Int64 BusinessGenerated { get; set; } 
    }
}
