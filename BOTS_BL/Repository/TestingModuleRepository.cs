using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;
using BOTS_BL.Models.OnBoarding;
using System.Data;


namespace BOTS_BL.Repository
{
    public class TestingModuleRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();

        public string GetLiveGroupId(string OBRGroupId)
        {
            string groupId = string.Empty;
            using (var context = new CommonDBContext())
            {
                groupId = context.GroupIdMappings.Where(x => x.OnboardingGroupId == OBRGroupId).Select(y => y.LiveGroupId).FirstOrDefault();
            }
            return groupId;
        }


        public List<SelectListItem> GetBillingPartners(string GroupId)
        {
            List<SelectListItem> lstData = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var BPIds = context.BOTS_TblRetailMaster.Where(x => x.GroupId == GroupId).Select(y => y.BillingPartner).ToList();

                foreach (var item in BPIds)
                {
                    var id = Convert.ToInt32(item);
                    var BPName = context.tblBillingPartners.Where(x => x.BillingPartnerId == id).Select(y => y.BillingPartnerName).FirstOrDefault();
                    lstData.Add(new SelectListItem
                    {
                        Text = BPName,
                        Value = Convert.ToString(BPName)
                    });
                }
            }

            return lstData;
        }
    
       public bool SaveAPIData(GroupTestingLog objgroupTesting)
        {
            bool result = false;
            string connStr = CR.GetCustomerConnString(objgroupTesting.GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                context.GroupTestingLogs.AddOrUpdate(objgroupTesting);
                context.SaveChanges();
                result = true;
            }
            return result;
        }

    }
}