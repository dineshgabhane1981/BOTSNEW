using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;
namespace WebApp.ViewModel
{
    public class BrandAndOutletViewModel
    {
        public List<BrandDetail> lstBrands { get; set; }
        public List<OutletDetail> lstOutlets { get; set; }
    }
}