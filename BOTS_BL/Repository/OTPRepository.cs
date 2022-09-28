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
        CustomerRepository objCustRepo = new CustomerRepository();



        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstGroups= new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var GroupDetails = context.GroupDetail.Where(x => x.ActiveStatus == "yes").ToList();

                foreach (var item in GroupDetails)
                {
                    lstGroups.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)
                       
                    });
                }
            }
            return lstGroups;


        }

        //public MemberData GetOTPData(string GroupId, string MobileNo)
        //{
        //    MemberData objMemberData = new MemberData();
        //    try
        //    {
        //        GroupDetails objgroupDetails = new GroupDetails();
        //        //CustomerDetail objCustomerDetail = new CustomerDetail();
        //        OTPMaintenance objOTP = new OTPMaintenance();
        //        //string connStr = objCustRepo.GetCustomerConnString(GroupId);
        //        using (var context = new CommonDBContext())
        //        {
        //            objOTP = context.OTPMaintenances.Where(x => x.MobileNo == MobileNo).OrderByDescending(y => y.Datetime).FirstOrDefault();
        //        }
        //        if (objOTP != null)
        //        {

        //            objMemberData.MobileNo = objOTP.OTP;
        //            objMemberData.EnrolledOn = Convert.ToString(objOTP.Datetime);
        //            var OutletId = objOTP.CounterId.Substring(0, objOTP.CounterId.Length - 2);
        //            using (var context = new CommonDBContext())
        //            {
        //                objMemberData.EnrolledOutletName = context.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        newexception.AddException(ex, GroupId);
        //    }
        //    return objMemberData;
        //}


        public MemberData GetOTPData(string GroupId, string MobileNo)
        {
            MemberData objMemberData = new MemberData();
            try
            {
                CustomerDetail objCustomerDetail = new CustomerDetail();
                OTPMaintenance objOTP = new OTPMaintenance();
                string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new BOTSDBContext(connStr))
                {
                    objOTP = contextNew.OTPMaintenances.Where(x => x.MobileNo == MobileNo).OrderByDescending(y => y.Datetime).FirstOrDefault();
                }
                if (objOTP != null)
                {

                    objMemberData.MobileNo = objOTP.OTP;
                    objMemberData.EnrolledOn = Convert.ToString(objOTP.Datetime);
                    var OutletId = objOTP.CounterId.Substring(0, objOTP.CounterId.Length - 2);
                    using (var contextNew = new BOTSDBContext(connStr))
                    {
                        objMemberData.EnrolledOutletName = contextNew.OutletDetails.Where(x => x.OutletId == OutletId).Select(y => y.OutletName).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objMemberData;
        }
    }
}

