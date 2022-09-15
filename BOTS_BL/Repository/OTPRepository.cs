using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class OTPRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();


        //public string CheckLogin(tblOTPDetail objData)
        //{
        //    string GroupId = "";
        //    using (var context = new DBTest())
        //    {
        //        GroupId = context.tblOTPDetails.Where(x => x.LoginId == objData.LoginId && x.Password == objData.Password).Select(y => y.GroupId).FirstOrDefault();
        //    }
        //    return GroupId;
        //}


        public List<SelectListItem> GetGroupDetails(string groupId)
        {
            List<SelectListItem> lstGroupDetails = new List<SelectListItem>();
            List<tblGroupDetail> objData = new List<tblGroupDetail>();
            //string ConnectionString = string.Empty;
            string connectionString = CR.GetCustomerConnString(groupId);

            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    objData = context.tblGroupDetails.ToList();

                    foreach (var item in objData)
                    {
                        SelectListItem lstItem = new SelectListItem();
                        lstItem.Value = Convert.ToString(item.GroupId);
                        lstItem.Text = item.GroupName;
                        lstGroupDetails.Add(lstItem);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return lstGroupDetails;
        }
    }
}

