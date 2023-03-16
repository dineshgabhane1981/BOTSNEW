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

        public string DocType { get; set; }

        public SelectListItem[] DocumentType()
        {
            return new SelectListItem[3] { new SelectListItem() { Text = "ConfigurationSheet", Value = "ConfigurationSheet" }, new SelectListItem() { Text = "KnowledgeSeries", Value = "KnowledgeSeries" }, new SelectListItem() { Text = "Creatives", Value = "Creatives" } };
        }
    }
}