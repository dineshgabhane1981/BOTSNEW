using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Models.CommonDB
{
    public class BOPromoRetailCategory
    {
        public Int32 RetailCategoryId { get; set; }
        public string RetailCategoryName { get; set; }

        public List<SelectListItem> lstRetailCategory { get; set; }

        public string Count { get; set; }
    }

    public class RetailCount
    {
        public Int32 Count { get; set; }
    }
}
