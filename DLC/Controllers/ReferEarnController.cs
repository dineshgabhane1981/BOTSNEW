using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DLC.Controllers
{
    public class ReferEarnController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        ReportsRepository RR = new ReportsRepository();
        CustomerRepository objCustRepo = new CustomerRepository();
        // GET: ReferEarn
        public ActionResult Index()
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var lstData = DCR.GetMWPReferTNC(sessionVariables.GroupId);
            return View(lstData);
        }
        [HttpPost]
        public ActionResult DLCReferFriend(string FirstName, string FirstMobile, string SecondName, string SecondMobile, string ThirdName, string ThirdMobile)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            status =DCR.DLCReferFriend(sessionVariables.GroupId, sessionVariables.MobileNo, sessionVariables.BrandId, FirstMobile, FirstName, SecondMobile, SecondName, ThirdMobile, ThirdName);
            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        //public List<MWP_ReferTNC> GetMWPReferTNC(string groupId)
        //{
        //    List<MWP_ReferTNC> lstData = new List<MWP_ReferTNC>();
        //    string connStr = objCustRepo.GetCustomerConnString(groupId);
        //    using (var context = new BOTSDBContext(connStr))
        //    {
        //        try
        //        {
        //            lstData = context.MWP_ReferTNC.ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            newexception.AddException(ex, "GetMWPReferTNC " + groupId);
        //        }
        //        return lstData;
        //    }
        //}
        public bool DLCReferFriend(string groupId, string MobileNo, string BrandId, string firstMobileNo, string firstName, string secondMobileNo, string secondName, string thirdMobileNo, string thirdName)
        {
            bool status = false;
            string DBName = String.Empty;
            string connStr = objCustRepo.GetCustomerConnString(groupId);
            using (var context = new CommonDBContext())
            {
                DBName = context.tblDatabaseDetails.Where(x => x.GroupId == groupId).Select(y => y.DBName).FirstOrDefault();
            }
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    var result = context.Database.SqlQuery<SPResponse>("sp_DLCRefer @pi_MobileNo, @pi_BrandId, @pi_Datetime, " +
                        "@pi_1stMobileNo, @pi_1stName, @pi_2ndMobileNo,@pi_2ndName,@pi_3rdMobileNo,@pi_3rdName, @pi_DBName",
                                  new SqlParameter("@pi_MobileNo", MobileNo),
                                  new SqlParameter("@pi_BrandId", BrandId),
                                  new SqlParameter("@pi_Datetime", DateTime.Now),
                                  new SqlParameter("@pi_1stMobileNo", firstMobileNo),
                                  new SqlParameter("@pi_1stName", firstName),
                                  new SqlParameter("@pi_2ndMobileNo", secondMobileNo),
                                  new SqlParameter("@pi_2ndName", secondName),
                                  new SqlParameter("@pi_3rdMobileNo", thirdMobileNo),
                                  new SqlParameter("@pi_3rdName", thirdName),
                                   new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<SPResponse>();


                    if (result.ResponseCode=="0")
                        status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "DLCReferFriend" + groupId);
                }
            }
            return status;
        }
    }
}