using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class ITOPSNewViewModel
    {
        public string GroupId { get; set; }
        public List<CommonOTPDetails> ObjLstOTPDetails { get; set; }  
        public string LoginLevelddl { get; set; }
        public SelectListItem[] Levelddl()
        {
                return new SelectListItem[3] { new SelectListItem() { Text = "Group", Value = "1" }, new SelectListItem() { Text = "Brand", Value = "2" }, new SelectListItem() { Text = "Outlet", Value = "3" } };
        }
        

    }
}