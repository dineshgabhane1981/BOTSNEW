using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            List<SelectListItem> lstGroups = new List<SelectListItem>();
            List<SelectListItem> lstGroupsByName = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var GroupDetails = context.GroupDetail.Where(x => x.ActiveStatus == "yes").ToList();

                //GroupDetails.OrderBy(u => u.GroupName).ToList();
                foreach (var item in GroupDetails)
                {
                    lstGroups.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)

                    });
                }
                lstGroupsByName = lstGroups.OrderBy(u => u.Text).ToList();
            }
            return lstGroupsByName;
        }

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

        public DataTable GetOTPDetails(string GroupId, string MobileNo)
        {
            string OTP, Source, DBId, password, DBName;
            OTP = string.Empty;
            Source = string.Empty;
            DBId = string.Empty;
            password = string.Empty;
            DBName = string.Empty;

            using (var context = new CommonDBContext())
            {
                var respo = context.DatabaseDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();

                Source = respo.IPAddress;
                DBId = respo.DBId;
                password = respo.DBPassword;
                DBName = respo.DBName;
            }
           
            DataTable dt = new DataTable();
            SqlConnection _Con = new SqlConnection("Data source = " + Source + "; initial catalog=" + DBName + ";user id=" + DBId + "; password=" + password + ";");
            SqlDataAdapter adapt = new SqlDataAdapter("select  top(1) case when len(O.CounterId)=5 Then 'Admin' Else(select OD.OutletName from OutletDetails OD where OD.OutletId=substring(O.CounterId,1,8)) End as OutletName,O.Datetime,O.OTP,C.Points as PointsAvail from OTPMaintenance O inner join CustomerDetails C on O.MobileNo = C.MobileNo where O.MobileNo='" + MobileNo + "' order by O.SlNo desc", _Con);
            _Con.Open();
            adapt.Fill(dt);
            _Con.Close();

            return dt;
        }
    }
}

