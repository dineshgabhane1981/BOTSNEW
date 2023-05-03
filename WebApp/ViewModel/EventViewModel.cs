using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class EventViewModel
    {
        //public tblGroupDetail NeverOptFor { get; set; }
        public List<tblGroupDetail> lstNeverOptFor { get; set; }
        public List<tblGroupDetail> lstActive { get; set; }
    }
}