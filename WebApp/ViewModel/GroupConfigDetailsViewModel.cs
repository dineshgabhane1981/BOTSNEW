using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class GroupConfigDetailsViewModel
    {
        public tblGroupDetail objGroupDetails { get; set; }
        public List<BrandDetail> lstBrandDetails { get; set; }
    }
}