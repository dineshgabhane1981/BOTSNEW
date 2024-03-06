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


    }
}
