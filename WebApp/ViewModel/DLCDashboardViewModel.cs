using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class DLCDashboardViewModel
    {
        public tblDLCDashboardConfig objDLCDashboard { get; set; }
        public SelectListItem[] RedirectToPage()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Dashboard", Value = "Dashboard" }, new SelectListItem() { Text = "Update Profile", Value = "Update Profile" }, new SelectListItem() { Text = "Gift Points", Value = "Gift Points" }, new SelectListItem() { Text = "Transaction History", Value = "Transaction History" } };
        }
    }
}