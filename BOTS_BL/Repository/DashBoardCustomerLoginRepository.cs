using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BOTS_BL.Models.CommonDB;
using System.Web;
using System.Net;
using System.IO;

namespace BOTS_BL.Repository
{
    public class DashBoardCustomerLoginRepository
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        DashboardRepository DR = new DashboardRepository();
        public bool AddDashboardCustomerLogin(DashboardCustomerLogin objdashboardlogin)
        {
            bool status = false;
            CustomerLoginDetail objdashboardlogindeatils = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    CustomerLoginDetail objcustexist = new CustomerLoginDetail();
                    objdashboardlogindeatils = context.CustomerLoginDetails.Where(x => x.LoginId == objdashboardlogin.MobileNo && x.GroupId == objdashboardlogin.GroupId).FirstOrDefault();
                    if (objdashboardlogindeatils == null)
                    {

                        objcustexist.UserId = objdashboardlogin.GroupId;
                        objcustexist.LoginId = objdashboardlogin.MobileNo;
                        objcustexist.Password = "123";
                        objcustexist.UserName = objdashboardlogin.CustomerName;
                        if (objdashboardlogin.LoginType == "02")
                        {
                            objcustexist.LevelIndicator = "02";
                            objcustexist.CreatedDate = DateTime.UtcNow.Date;
                            objcustexist.LoginStatus = true;
                            objcustexist.UserStatus = true;


                        }
                        else
                        {
                            objcustexist.LevelIndicator = "04";
                            objcustexist.CreatedDate = DateTime.UtcNow.Date;
                            objcustexist.LoginStatus = true;
                            objcustexist.UserStatus = true;
                            objcustexist.OutletOrBrandId = objdashboardlogin.OutletOrBrandId;
                        }
                        objcustexist.GroupId = objdashboardlogin.GroupId;
                        objcustexist.EmailId = null;
                        objcustexist.MobileNo = null;
                        objcustexist.LoginType = null;

                        context.CustomerLoginDetails.AddOrUpdate(objcustexist);
                        context.SaveChanges();

                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddDashboardCustomerLogin");
            }

            return status;
        }

        public List<SelectListItem> GetLoginType()
        {
            List<SelectListItem> lstlogintype = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    // var RetailCategory = context.tblCategories.ToList();

                    lstlogintype.Add(new SelectListItem { Selected = true, Text = "---Select---", Value = "0" });
                    lstlogintype.Add(new SelectListItem { Text = "Admin Login", Value = "Admin" });
                    lstlogintype.Add(new SelectListItem { Text = "Outlet Login", Value = "Outlet" });

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLoginType");
            }
            return lstlogintype;
        }

        public List<DashboardCustomerLogin> GetDashboardcustomerlogin(string GroupId)
        {
            List<DashboardCustomerLogin> objdashboard = new List<DashboardCustomerLogin>();
            try
            {
                //  string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var context = new CommonDBContext())
                {
                    objdashboard = (from c in context.CustomerLoginDetails
                                    where c.GroupId == GroupId
                                    select new DashboardCustomerLogin
                                    {
                                        SlNo = c.SlNo,
                                        OutletOrBrandId = c.OutletOrBrandId,
                                        CustLoginType = c.OutletOrBrandId != null ? "OutletWise" : "Admin",
                                        GroupId = c.GroupId,
                                        LoginType = c.LoginType,
                                        MobileNo = c.MobileNo,
                                        LoginStatus = c.LoginStatus,
                                        CreatedDate = c.CreatedDate,
                                        LevelIndicator = c.LevelIndicator,
                                        UserName = c.UserName,
                                        Password = c.Password,
                                        LoginId = c.LoginId,
                                        UserId = c.GroupId,
                                        CustomerName = c.UserName,

                                    }).OrderByDescending(x => x.CreatedDate).ToList();

                    foreach (var item in objdashboard)
                    {
                        item.CreatedDateStr = item.CreatedDate.Value.ToString("MM/dd/yyyy");
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDashboardcustomerlogin");
            }
            return objdashboard;
        }

        public bool UpdateMaskedValue(int GroupId, string value)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    try
                    {
                        var groupdetails = context.tblGroupDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        groupdetails.IsMasked = Convert.ToBoolean(value);
                        context.tblGroupDetails.AddOrUpdate(groupdetails);
                        context.SaveChanges();
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        newexception.AddException(ex, "UpdateMaskedValue");
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateMaskedValue");
            }

            return status;
        }

        public MaskingOTPResponse SendOTP(string GroupId)
        {
            MaskingOTPResponse Obj = new MaskingOTPResponse();

            bool SMSStatus;
            SMSStatus = default;

            Random r = new Random();
            int randNum = r.Next(1000000);
            string sixDigitNumber = randNum.ToString("D6");

            int Id = Convert.ToInt32(GroupId);

            try
            {
                using (var context = new CommonDBContext())
                {
                    OTPDetail objOTPDetail = new OTPDetail();

                    var details = context.tblGroupDetails.Where(x => x.GroupId == Id).FirstOrDefault();
                    
                    objOTPDetail.OTP = Convert.ToInt32(sixDigitNumber);
                    objOTPDetail.SentDate = DateTime.Now;
                    objOTPDetail.EmailId = details.OwnerEmail;
                    context.OTPDetails.Add(objOTPDetail);
                    context.SaveChanges();

                    //status = true;
                    Obj.OTP = Convert.ToString(objOTPDetail.OTP);
                    Obj.status = true;

                    var _MobileMessage = "Dear Member, " + Convert.ToInt32(sixDigitNumber) + "  is your OTP. Sample SMS for OTP - Blue Ocktopus ";
                    var _UserName = "blueohttpotp";
                    var _Password = "bluoct87";
                    var _MobileNo = details.OwnerMobileNo;
                    var _Sender = "BLUEOC";
                    var _Url = "https://http2.myvfirst.com/smpp/sendsms?";

                    SMSStatus =  SendSMS(_MobileMessage, _UserName, _Password, _MobileNo, _Sender, _Url);

                    Obj.smsstatus = SMSStatus;
                }
                
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendOTP");
            }

            return Obj;
        }

        public bool SendSMS(string _MobileMessage, string _UserName, string _Password, string _MobileNo, string _Sender, string _Url)
        {
            bool status = false;
            try
            {
                _MobileMessage = _MobileMessage.Replace("#99", "&");
                _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                string type1 = "TEXT";
                StringBuilder sbposdata1 = new StringBuilder();
                sbposdata1.AppendFormat("username={0}", _UserName);
                sbposdata1.AppendFormat("&password={0}", _Password);
                sbposdata1.AppendFormat("&to={0}", _MobileNo);
                sbposdata1.AppendFormat("&from={0}", _Sender);//BLUEOC
                sbposdata1.AppendFormat("&text={0}", _MobileMessage);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq1 = (HttpWebRequest)WebRequest.Create(_Url);
                UTF8Encoding encoding1 = new UTF8Encoding();
                byte[] data1 = encoding1.GetBytes(sbposdata1.ToString());
                httpWReq1.Method = "POST";
                httpWReq1.ContentType = "application/x-www-form-urlencoded";
                httpWReq1.ContentLength = data1.Length;
                using (Stream stream1 = httpWReq1.GetRequestStream())
                {
                    stream1.Write(data1, 0, data1.Length);
                }
                HttpWebResponse response1 = (HttpWebResponse)httpWReq1.GetResponse();
                StreamReader reader1 = new StreamReader(response1.GetResponseStream());
                string responseString1 = reader1.ReadToEnd();
                reader1.Close();
                response1.Close();
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendSMS");
            }
            return status;
        }
    }
}
