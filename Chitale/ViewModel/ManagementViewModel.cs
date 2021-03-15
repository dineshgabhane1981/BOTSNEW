using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.ViewModel
{
    public class ManagementViewModel
    {
        public List<SelectListItem> ClusterList { get; set; }
        public List<SelectListItem> SubClusterList { get; set; }
        public List<SelectListItem> CityList { get; set; }
    }
}