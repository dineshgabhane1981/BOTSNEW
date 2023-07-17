using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class ProductDetailsMaster
    {
        public Int64 CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public Int64 SubCategoryCode { get; set; }

        public string SubCategoryName { get; set; }

        public Int64 ProductCode { get; set; }

        public string ProductName { get; set; }
    }
}
