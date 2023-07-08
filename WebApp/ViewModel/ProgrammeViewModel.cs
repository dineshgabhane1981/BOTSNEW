using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class ProgrammeViewModel
    {
        public List<tblGroupDetail> lstActive { get; set; }

        public List<tblGroupDetail> lstNotActive { get; set; }

        public List<SelectListItem> lstGroupDetails { get; set; }
        public List<SelectListItem> lstRMAssigned { get; set; }


        public tblGroupDetail tblGroupDetails { get; set; }
        public tblRMAssigned tblRMAssigned { get; set; }

        public string Message { get; set; }
        
        
        public SelectListItem[] MessageType()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Enrollment", Value = "Enrollment" }, new SelectListItem() { Text = "Earn", Value = "Earn" }, new SelectListItem() { Text = "Burn", Value = "Burn" }, new SelectListItem() { Text = "Cancel", Value = "Cancel" } };
        }
        public WhatsAppSMSMaster objWhatsAppSMSMaster { get; set; }
        public string Script { get; set; }
    }
}