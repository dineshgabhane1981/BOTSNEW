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
        
        public string Logo { get; set; }
        public string EventName { get; set; }
        public List<tblGroupDetail> lstNeverOptFor { get; set; }
        public List<tblGroupDetail> lstActive { get; set; }
        public List<EventDetail> lstEvent { get; set; }

        public EventDetail objEvent { get; set; }

        public ListOfLink objlistlink { get; set; }

        public string EvtType { get; set; }

        public SelectListItem[] EventType()
        {
            return new SelectListItem[5] { new SelectListItem() { Text = "Corporate", Value = "Corporate" }, new SelectListItem() { Text = "Society", Value = "Society" }, new SelectListItem() { Text = "Door-to-Door", Value = "Door-to-Door" }, new SelectListItem() { Text = "Sponsorship", Value = "Sponsorship" }, new SelectListItem() { Text = "In-House", Value = "In-House" } };
        }
    }

    public class ListOfLink
    { 
       public string Place { get; set; }
       public string Url { get; set; }
    }
}