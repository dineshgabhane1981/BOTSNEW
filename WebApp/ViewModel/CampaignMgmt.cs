using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Models;

namespace WebApp.ViewModel
{
    public class CampaignMgmt
    {
        public List<SelectListItem> LstSMSCost { get; set; }
        public tblSMSCostMaster objSMSCost { get; set; }
        public List<LisCampaign> LstCampaign { get; set; }
        public List<PromoLisCampaign> LstPromo { get; set; }
        public string BaseType { get; set; }
        public SelectListItem[] BaseType1()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "All", Value = "1" }, new SelectListItem() { Text = "Member Base", Value = "2" }, new SelectListItem() { Text = "Non Transacted", Value = "3" }, new SelectListItem() { Text = "Based On Points", Value = "4" }};
        }
        public string PointsType { get; set; }
        public SelectListItem[] PointsType1()
        {
            return new SelectListItem[4] { new SelectListItem() { Text = "Less Than", Value = "1" }, new SelectListItem() { Text = "Greater Than", Value = "2" }, new SelectListItem() { Text = "Equal To", Value = "3" }, new SelectListItem() { Text = "Points Range", Value = "4" } };
        }

    }
}