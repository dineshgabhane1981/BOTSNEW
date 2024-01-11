using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CategoryWiseSSvsNonSS
    {
        public Int64 TotalBusiness { get; set; }
        public Int64 TotalBusinessSS { get; set; }
        public Int64 TotalBusinessNonSS { get; set; }
        public decimal SSPercentage { get; set; }

        public string TotalBusinessStr { get; set; }
        public string TotalBusinessSSStr { get; set; }
        public string TotalBusinessNonSSStr { get; set; }
    }
}
