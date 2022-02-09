using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class CreateOwnReportViewModel
    {
        public List<CustomerTypeReport> listCustR { get; set; }
       public List<object> listCustr { get; set; }
        public List<TransactionTypeReport> listTxnR { get; set; }
        public List<SelectListItem> LstnontransactedList { get; set; }
        public List<SelectListItem> LstSpendList { get; set; }
        public List<SelectListItem> LstRedeemedList { get; set; }
        public List<SelectListItem> LstOutlet { get; set; }
        public DateTime StartDt { get; set; }
        public int? TotalCount { get; set; }
        public List<SelectListItem> LstBrandList { get; set; }
        public List<string> lstcolumnlist { get; set; }
        public List<string> lstcolumnIdlist { get; set; }
    }
}