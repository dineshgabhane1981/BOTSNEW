using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class VelocityReportViewModel
    {
        public List<tblVelocityMain> lstMain { get; set; }
        public List<tblVelocityMonthwise> lstMonthwise { get; set; }
    }
}