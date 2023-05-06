using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class EventViewModel
    {
        //public tblGroupDetail NeverOptFor { get; set; }
        public List<tblGroupDetail> lstNeverOptFor { get; set; }
        public List<tblGroupDetail> lstActive { get; set; }
        public List<EventDetail> lstEvent { get; set; }

        public EventDetail objEvent { get; set; }

        public string EvtType { get; set; }

        public SelectListItem[] EventType()
        {
            return new SelectListItem[2] { new SelectListItem() { Text = "Type1", Value = "Type1" }, new SelectListItem() { Text = "Type2", Value = "Type2" } };
        }
    }
}