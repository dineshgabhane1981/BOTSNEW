using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTPPage.ViewModel
{
    public class OTPViewModel
    {
        public List<SelectListItem> lstGroupDetails { get; set; }

        public tblGroupDetail tblGroupDetails { get; set; }

    }
}