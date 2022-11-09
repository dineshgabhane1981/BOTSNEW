using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class PromoCampaignRepository
    {
        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstGroupDetails = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                string status = "0";
                //var GroupDetails = context.tblGroupDetails.Where(x => x.IsActive == true).ToList();
                var GroupDetails = context.WAReports.Where(x => x.SMSStatus == status).ToList();

                foreach (var item in GroupDetails)
                {
                    lstGroupDetails.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)
                    });
                }
            }
            return lstGroupDetails;


        }
    }
}
