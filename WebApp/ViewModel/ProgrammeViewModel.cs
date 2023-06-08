using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModel
{
    public class ProgrammeViewModel
    {
        public List<tblGroupDetail> lstActive { get; set; }

        public List<tblGroupDetail> lstNotActive { get; set; }
    }
}