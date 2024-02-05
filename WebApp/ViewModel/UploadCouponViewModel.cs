using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.OnBoarding;

namespace WebApp.ViewModel
{
    public class UploadCouponViewModel
    {
        public List<tblCouponUpload> lstCouponUpload { get; set; }
        public List<SelectListItem> lstOutlet { get; set; }
        public List<SelectListItem> lstCategory { get; set; }
        public List<SelectListItem> lstProduct { get; set; }
        public string dummyCategory { get; set; }
        public string dummyProduct { get; set; }
        public SelectListItem[] RedeemDays()
        {
            return new SelectListItem[7] { new SelectListItem() { Text = "Monday", Value = "Monday" },
                new SelectListItem() { Text = "Tuesday", Value = "Tuesday" },
                new SelectListItem() { Text = "Wednesday", Value = "Wednesday" },
                new SelectListItem() { Text = "Thursday", Value = "Thursday" },
                new SelectListItem() { Text = "Friday", Value = "Friday" },
                new SelectListItem() { Text = "Saturday", Value = "Saturday" },
                new SelectListItem() { Text = "Sunday", Value = "Sunday" }};
        }
    }
}