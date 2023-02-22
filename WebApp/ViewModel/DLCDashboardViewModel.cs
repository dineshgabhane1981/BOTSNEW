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
        public SelectListItem[] LanguageList()
        {
            return new SelectListItem[14] { new SelectListItem() { Text = "Assamese", Value = "Assamese" },
                new SelectListItem() { Text = "Bengali", Value = "Bengali" },
                new SelectListItem() { Text = "English", Value = "English" },
                new SelectListItem() { Text = "Gujarati", Value = "Gujarati" },
                new SelectListItem() { Text = "Hindi", Value = "Hindi" },
                new SelectListItem() { Text = "Kannada", Value = "Kannada" },
                new SelectListItem() { Text = "Kashmiri", Value = "Kashmiri" },
                new SelectListItem() { Text = "Konkani", Value = "Konkani" },
                new SelectListItem() { Text = "Malayalam", Value = "Malayalam" },
                new SelectListItem() { Text = "Marathi", Value = "Marathi" },
                new SelectListItem() { Text = "Odia", Value = "Odia" },
                new SelectListItem() { Text = "Punjabi", Value = "Punjabi" },
                new SelectListItem() { Text = "Tamil", Value = "Tamil" },
                new SelectListItem() { Text = "Telugu", Value = "Telugu" }};
        }
    }
}