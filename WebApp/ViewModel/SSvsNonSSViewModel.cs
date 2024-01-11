using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class SSvsNonSSViewModel
    {
        public tblSSNonSSReport objSSNonSSReport { get; set; }
        public List<SelectListItem> lstCategory { get; set; }
        public List<SelectListItem> lstSubCategory { get; set; }
        public int dummyCategoryCode { get; set; }
        public int dummySubCategoryCode { get; set; }
    }
}