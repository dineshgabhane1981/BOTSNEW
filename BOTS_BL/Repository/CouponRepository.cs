using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using BOTS_BL.Models.IndividualDBModels;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class CouponRepository
    {
        Exceptions newexception = new Exceptions();
        public List<tblCouponUpload> GetAllCouponUpload(string conStr)
        {
            List<tblCouponUpload> lstData = new List<tblCouponUpload>();
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    lstData = context.tblCouponUploads.ToList();
                    foreach(var item in lstData)
                    {
                        var sum = context.tblCouponMappings.Where(x => x.EarnRuleId == item.CouponFileName).Sum(y => y.InvoiceAmount);
                        var count = context.tblCouponMappings.Where(x => x.EarnRuleId == item.CouponFileName && x.InvoiceAmount != null).Count();
                        item.CouponRedeemBiz = String.Format(new CultureInfo("en-IN", false), "{0:n0}", Convert.ToDouble(sum));
                        item.RedeemCount = count;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllCouponUpload");
            }
            return lstData;
        }
        public bool UploadCouponData(List<tblCouponMapping> lstData, tblCouponUpload objUploadData, string conStr)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    context.tblCouponUploads.Add(objUploadData);
                    context.SaveChanges();
                    foreach (var item in lstData)
                    {
                        context.tblCouponMappings.Add(item);
                        context.SaveChanges();

                        SendSMS(item, conStr);
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UploadCouponData");
            }

            return status;
        }
    
        public bool SaveCouponEarnRule(tblCouponRule objData,string conStr)
        {
            bool result = false;
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    context.tblCouponRules.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCouponEarnRule");
            }

            return result;
        }

        public bool EnableDisableCouponEarnRule(int ruleId, string conStr)
        {
            bool result = false;
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    var objData = context.tblCouponRules.Where(x => x.EarnRuleId == ruleId).FirstOrDefault();
                    if (objData.IsActive == true)                   
                        objData.IsActive = false;
                    else
                        objData.IsActive = true;

                    context.tblCouponRules.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCouponEarnRule");
            }

            return result;
        }
        public List<tblCouponRule> GetAllCouponRule(string conStr)
        {
            List<tblCouponRule> lstData = new List<tblCouponRule>();
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    lstData = context.tblCouponRules.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCouponEarnRule");
            }
            return lstData;
        }
    
        public List<tblCouponMapping> GetRedeemDetailedReport(string fileName, string conStr)
        {
            List<tblCouponMapping> objRedeemData = new List<tblCouponMapping>();
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    objRedeemData = context.tblCouponMappings.Where(x => x.EarnRuleId == fileName && x.InvoiceAmount != null).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRedeemDetailedReport");
            }
            return objRedeemData;
        }
        
        public void SendSMS(tblCouponMapping SMSData,string conStr)
        {
            //conStr For Testing 
            conStr = "Server=" + "13.233.58.231" + ";Initial Catalog=" + "MoolchandNew" + ";User Id=" + "Renaldo" + ";Password=" + "F59VM$KDE@KF!AW";
            
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    var SMSCredentialData = context.tblSMSWhatsAppCredentials.FirstOrDefault();
                    var SMSScript = context.tblSMSWhatsAppScriptMasters.Where(x => x.Id == "121").FirstOrDefault();

                    switch (SMSCredentialData.SMSVendor)
                    {
                        case "Vision":
                            string _MobileMessage = SMSScript.SMSScript;
                            _MobileMessage = _MobileMessage.Replace("#46", SMSData.CouponCode);
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(SMSCredentialData.SMSUrl);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {

                                string json = "{\"Account\":" +
                                                "{\"APIKey\":\"" + SMSCredentialData.SMSAPIKey + "\"," +
                                                "\"SenderId\":\"" + SMSCredentialData.SMSSenderId + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"8\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"91" + SMSData.MobileNo + "\"," +
                                                "\"Text\":\"" + _MobileMessage + "\"}]" +
                                                "}";
                                streamWriter.Write(json);
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendSMS");
            }
        }

        public List<CouponReportModel> GetCouponReport(string fromDate, string toDate, string earnOutletId, string burnOutletId, string GroupId)
        {
            List<CouponReportModel> lstData = new List<CouponReportModel>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.Database.CommandTimeout = 300;
                    var DBName = context.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    lstData = context.Database.SqlQuery<CouponReportModel>("sp_CouponReport @pi_GroupId, @pi_INDDatetime, @pi_FromDate, @pi_ToDate, @pi_EarnOutletId, @pi_BurnOutletId, @pi_DBName",
                            new SqlParameter("@pi_GroupId", GroupId),
                            new SqlParameter("@pi_INDDatetime", DateTime.Now.ToString("yyyy-MM-dd")),
                            new SqlParameter("@pi_FromDate", fromDate),
                            new SqlParameter("@pi_ToDate", toDate),
                            new SqlParameter("@pi_EarnOutletId", earnOutletId),
                            new SqlParameter("@pi_BurnOutletId", burnOutletId),
                            new SqlParameter("@pi_DBName", DBName)).ToList<CouponReportModel>();

                    foreach(var item in lstData)
                    {
                        if (item.CreatedDate.HasValue)
                            item.CreatedDateStr = item.CreatedDate.Value.ToString("dd-MM-yyy");
                        if (item.ExpiryDate.HasValue)
                            item.ExpiryDateStr = item.ExpiryDate.Value.ToString("dd-MM-yyy");
                        if (item.RedeemDate.HasValue)
                            item.RedeemDateStr = item.RedeemDate.Value.ToString("dd-MM-yyy");
                    }
                }
            }
            catch(Exception ex)
            {

            }


            return lstData;
        }

    }
}
