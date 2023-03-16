using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModel
{
    public class DocumentLibraryViewModel
    {
        public List<SelectListItem> lstGroupDetails { get; set; }

        public tblGroupDetail tblGroupDetails { get; set; }
    }
}