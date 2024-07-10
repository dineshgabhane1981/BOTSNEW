using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class EReceiptViewModel
    {
        public tblEReceiptConfig objEReceiptConfig { get; set; }
        public List<string> lstBannerImages { get; set; }
    }
}