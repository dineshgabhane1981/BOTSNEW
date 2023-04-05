using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class DocumentLibraryViewModel
    {
        public List<SelectListItem> lstGroupDetails { get; set; }
        public tblGroupDetail tblGroupDetails { get; set; }
        public string dept { get; set; }
        public SelectListItem[] Departments()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "HR", Value = "HR" }, new SelectListItem() { Text = "Sales", Value = "Sales" }, new SelectListItem() { Text = "Finance", Value = "Finance" } };
        }
        public SelectListItem[] Vendors()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Techocore", Value = "Techocore" }, new SelectListItem() { Text = "Vision", Value = "Vision" }, new SelectListItem() { Text = "Pinnacle", Value = "Pinnacle" }, new SelectListItem() { Text = "Value First", Value = "Value First" } };
        }
        public string DocType { get; set; }
        public string roleId { get; set; }
        public List<SelectListItem> lstDocumentType { get; set; }
        public string Vendor { get; set; }
    }
}