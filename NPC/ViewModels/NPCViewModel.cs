using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;   

namespace NPC.ViewModels
{
    public class NPCViewModel
    {
        public tblNPCDetail tblNPCDetail { get; set; }
        public string Logo { get; set; }
        public BrandDetail objbrandDetail { get; set; }

        public OutletDetails objOutletDetail { get; set; }
        public List<SelectListItem> lstOutlets { get; set; }

        public tblNPCCategory objNPCCategory { get; set; }
        public tblNPCSubCategory objNPCSubCategory { get; set; }

        public List<SelectListItem> lstNPCCategory { get; set; }
        public List<SelectListItem> lstNPCSubCategory { get; set; }
        public List<SelectListItem> lstNPCEmployees { get; set; }

        //public SelectListItem[] CategoryName()
        //{
        //    return new SelectListItem[3] { new SelectListItem() { Text = "Gold", Value = "Gold" }, new SelectListItem() { Text = "Silver", Value = "Silver" }, new SelectListItem() { Text = "Diamond", Value = "Diamond" } };
        //}

        public SelectListItem[] SubCategoryName()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "24k", Value = "24k" }, new SelectListItem() { Text = "18k", Value = "18k" } };
        }

        public SelectListItem[] NextVisitDay()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "5", Value = "5" }, new SelectListItem() { Text = "10", Value = "10" }, new SelectListItem() { Text = "15", Value = "15" }, new SelectListItem() { Text = "20", Value = "20" }  };
        }


    }
}