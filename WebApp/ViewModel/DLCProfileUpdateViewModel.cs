using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApp.ViewModel
{
    public class DLCProfileUpdateViewModel
    {
        public DLCProfileUpdate objDLCProfUpdt { get; set; }
        public SelectListItem[] MandatoryOrNot()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Mandatory", Value = "Mandatory" }, new SelectListItem() { Text = "Non Mandatory", Value = "Non Mandatory" } };
        }
    }
}