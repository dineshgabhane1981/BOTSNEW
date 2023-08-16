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
using BOTS_BL.Models.SalesLead;
using BOTS_BL.Models.FeedBack;
using System.Web;
using System.Globalization;
using BOTS_BL.Models.FeedbackModule;
using System.Data.Entity.Core.Objects;

namespace BOTS_BL.Repository
{
    public class ExhibitionRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public bool AddExhibitionData(tblExhibitionData objData)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    bool WAStatus = SendWAMessage(objData.WelcomeScript, objData.MobileNo);
                    if (WAStatus)
                        objData.WelcomeScriptSentDate = DateTime.Now;

                    context.tblExhibitionDatas.AddOrUpdate(objData);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return status;
        }
        public bool AddCollctionData(tblCustomerDataCollection objData)
        {
            var GroupId = "1306";
            var connStr = CR.GetCustomerConnString(GroupId);

            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.tblCustomerDataCollections.AddOrUpdate(objData);
                            context.SaveChanges();

                            //Add Entry to CustomerDetails
                            CustomerDetail objCust = new CustomerDetail();
                            var CustomerId = context.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                            var NewId = Convert.ToInt64(CustomerId) + 1;
                            objCust.CustomerId = Convert.ToString(NewId);
                            objCust.MobileNo = objData.MobileNo;
                            objCust.CustomerName = objData.CustomerName;
                            objCust.DOJ = DateTime.Now;
                            objCust.EnrollingOutlet = "13061002";
                            objCust.Status = "00";
                            objCust.MemberGroupId = "1000";
                            objCust.CustomerThrough = "1";
                            objCust.Points = 2000;
                            context.CustomerDetails.AddOrUpdate(objCust);
                            context.SaveChanges();

                            //Add Entry to CustomerChild
                            //CustomerChild objChild = new CustomerChild();
                            //objChild.MobileNo= objData.MobileNo;
                            //objChild.CustomerId = Convert.ToString(NewId);
                            //objChild.ChildCount = "0";
                            //objChild.City = objData.City;
                            //context.CustomerChilds.AddOrUpdate(objChild);
                            //context.SaveChanges();

                            //Add Entry to TransactionMaster
                            TransactionMaster objTrans = new TransactionMaster();
                            objTrans.CounterId = "13061002";
                            objTrans.MobileNo= objData.MobileNo;
                            objTrans.Datetime = DateTime.Now;
                            objTrans.TransType = "1";
                            objTrans.TransSource = "2";
                            objTrans.InvoiceNo = "000";
                            objTrans.InvoiceAmt = 00;
                            objTrans.Status = "00";
                            objTrans.CustomerId= Convert.ToString(NewId);
                            objTrans.PointsEarned = 2000;
                            objTrans.PointsBurned = 0;
                            objTrans.CampaignPoints = 0;
                            objTrans.TxnAmt = 0;
                            objTrans.CustomerPoints = 2000;
                            context.TransactionMasters.AddOrUpdate(objTrans);
                            context.SaveChanges();

                            //Add entry to Points Expiry
                            PointsExpiry objExpiry = new PointsExpiry();
                            objExpiry.MobileNo= objData.MobileNo;
                            objExpiry.CounterId = "13061002";
                            objExpiry.EarnDate = DateTime.Now;
                            objExpiry.Points = 2000;
                            objExpiry.ExpiryDate = DateTime.Now.AddDays(15);
                            objExpiry.Status = "00";
                            objExpiry.InvoiceNo = "000";
                            objExpiry.GroupId = GroupId;
                            objExpiry.Datetime= DateTime.Now;
                            objExpiry.CustomerId = Convert.ToString(NewId);

                            context.PointsExpiries.AddOrUpdate(objExpiry);
                            context.SaveChanges();
                            transaction.Commit();
                            status = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            status = false;
                            newexception.AddException(ex, "GetNeverOptForGroups");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return status;
        }
        public bool CheckCustomerExist(string MobileNo)
        {
            bool status = true;
            using (var context = new CommonDBContext())
            {
                var data = context.tblExhibitionDatas.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                if (data == null)
                    status = false;
            }
            return status;
        }
        public bool CheckCustomerCollectionExist(string MobileNo)
        {
            var GroupId = "1306";
            var connStr = CR.GetCustomerConnString(GroupId);
            bool status = true;
            using (var context = new BOTSDBContext(connStr))
            {
                var data = context.tblCustomerDataCollections.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                if (data == null)
                    status = false;
            }
            return status;
        }
        public bool SendWAMessage(string msg, string MobileNo)
        {
            bool status = false;
            try
            {
                string responseString;
                var path = "https://blueocktopus.in/BitlyImages/BOTS_Introduction.MP4";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://bo.enotify.app/api/sendFileWithCaption?");
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone={0}", "91" + MobileNo);
                sbposdata.AppendFormat("&link={0}", path);
                sbposdata.AppendFormat("&message={0}", msg);

                string Url = sbposdata.ToString();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbposdata.ToString());
                httpWReq.Method = "POST";

                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
                response.Close();
                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendWAMessage");
            }


            return status;
        }

        public List<tblExhibitionData> GetExhibitionData()
        {
            List<tblExhibitionData> lstData = new List<tblExhibitionData>();
            using (var context = new CommonDBContext())
            {
                lstData = context.tblExhibitionDatas.ToList();
            }
            return lstData;
        }
        public void UpdateDateTime(tblExhibitionData objData)
        {
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblExhibitionDatas.AddOrUpdate(objData);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateDateTime");
            }
        }

        public int InquiryCount()
        {
            int Count;
            Count = default(int);

            try
            {
                using (var context = new CommonDBContext())
                {
                    Count = context.tblExhibitionDatas.Count();
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "InquiryCount");
            }

            return Count;
        }

    }
}
