using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class GroupWiseDetails
    {
        public string CustId { get; set; }
        public string CustName { get; set; }
        public string CustCategory { get; set; }
        public string BusinessCategory { get; set; }
        public string Location { get; set; }
        public string CSName { get; set; }
        public Int64 CustCount { get; set; }
        public Int64 BulkUploadCount { get; set; }
        public Int64 Total { get; set; }
    }
}
