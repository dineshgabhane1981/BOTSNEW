using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class PromoCampaignRepository
    {
        Exceptions newexception = new Exceptions();
        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstGroupDetails = new List<SelectListItem>();
            try
            {
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
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupDetails");
            }
            return lstGroupDetails;


        }
        public string GetGroupCount()
        {
            string Count;
            Count = string.Empty;
            try 
            {
                using (var context = new CommonDBContext())
                {
                    var Cunt = context.WAReports.Where(y => y.SMSStatus == "0" && y.Status == "1" ).Select(x=> x.GroupCode).Distinct().Count();
                    Count = Convert.ToString(Cunt);
                }
                    
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetGroupCount");
            }

            return Count;
        }

        public string GetGroupList(string RetailCategoryId)
        {
            string Count;
            Count = string.Empty;
            int Id = 0;

            if (!string.IsNullOrEmpty(RetailCategoryId))
            {
                Id = Convert.ToInt32(RetailCategoryId);
            }
                       
            try
            {
                using (var context = new CommonDBContext())
                {
                    if(RetailCategoryId == "")
                    {
                        Count = GetGroupCount();
                    }
                    else
                    {
                        var Cunt = context.Database.SqlQuery<RetailCount>("select count(*) as Count from tblGroupDetails T inner join tblCategory C on T.RetailCategory = C.CategoryId where C.CategoryId = @Id and T.IsActive = 1 Group by C.CategoryName", new SqlParameter("@Id", Id)).FirstOrDefault();
                        
                        if(Cunt != null)
                        {
                            Count = Convert.ToString(Cunt.Count);
                        }
                        else
                        {
                            Count = "0";
                        }
                        
                    }       
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupList");
            }

            return Count;
        }

        public WAInsData GetBOWABalance()
        {
            WAInsData WAInsDetails = new WAInsData();
            string Tokenid = ConfigurationManager.AppSettings["BOTokenid"].ToString();


            try
            {     
               var baseAddress = "https://bo.enotify.app/api/chackBal?token=" + Tokenid;
                  using (var client = new HttpClient())
                    {
                      using (var response1 = client.GetAsync(baseAddress).Result)
                        {
                           if (response1.IsSuccessStatusCode)
                             {
                                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                 var response2 = client.GetStringAsync(new Uri(baseAddress)).Result;
                                 JObject jsonObj = JObject.Parse(response2);
                                 IEnumerable<JToken> pricyProducts = jsonObj.SelectTokens("$..data");
                                   foreach (JToken T in pricyProducts)
                                     {
                                      //WAInsData Temp = new WAInsData();
                                      //Temp.InstanceName = Convert.ToString(T.InstanceName);
                                      //Temp.TokenId = Convert.ToString(TokenId);
                                      WAInsDetails.InstanceName = null;
                                      WAInsDetails.TokenId = null;
                                      WAInsDetails.quota = Convert.ToString(T["quota"]);
                                      WAInsDetails.Status1 = Convert.ToString(T["status"]);    
                                     }
                             }
                             else
                             {
                                Console.WriteLine("{0} ({1})", (int)response1.StatusCode, response1.ReasonPhrase);
                             }
                       }
                   }
                       
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetWAInsData");
            }
            return WAInsDetails;
        }

        public List<SelectListItem> GetRetailCategory()
        {
            List<SelectListItem> LstRetailCategory = new List<SelectListItem>();
            List<BOPromoRetailCategory>  ObjData = new List<BOPromoRetailCategory>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    ObjData = context.Database.SqlQuery<BOPromoRetailCategory>("select T.RetailCategory as RetailCategoryId,C.CategoryName as RetailCategoryName from tblGroupDetails T inner join tblCategory C on T.RetailCategory = C.CategoryId  Group by T.RetailCategory, C.CategoryName").ToList();
                
                    foreach(var item in ObjData)
                    {
                        LstRetailCategory.Add(new SelectListItem
                        { 
                            Text = item.RetailCategoryName,
                            Value = Convert.ToString(item.RetailCategoryId)
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "GetRetailCategory");
            }

            return LstRetailCategory;
        }

        public JsonData SendTestMessage(string File1, string Text1, string File2, string Text2, tblAuditBOPromo objaudit)
        {
            string ImageWithCaptionUrl, TextUrl, FileUrl;
            
            ImageWithCaptionUrl = ConfigurationManager.AppSettings["SendFileWithCaptionAPILink"].ToString();
            TextUrl = ConfigurationManager.AppSettings["SendTextAPILink"].ToString();
            FileUrl = ConfigurationManager.AppSettings["SendFilesAPILink"].ToString();

            List<GroupCode> Data = new List<GroupCode>();
            JsonData Obj = new JsonData();
            string _MobileNo, _TokenId, _FileUrl1, _FileUrl2, _Message1, _Message2;
            _MobileNo = string.Empty; 
            //_Url = string.Empty;
            _TokenId = ConfigurationManager.AppSettings["BOTokenid"].ToString();
            _FileUrl1 = File1;
            _FileUrl2 = File2;
            _Message1 = Text1;
            _Message2 = Text2;
            try
            {
                using (var context = new CommonDBContext())
                {
                    //Data = context.WAReports.Where(x => x.SMSStatus == "9" && x.Status == "1").ToList();
                    //Data = context.WAReports.Where(x => x.SMSStatus == "5" && x.Status == "1").ToList();
                    
                    Data = context.Database.SqlQuery<GroupCode>("select distinct W.GroupCode as GroupCodeId from WAReport W where W.SMSStatus = 9 and W.Status = 1 Group by W.GroupCode").ToList();

                    Thread Job = new Thread(() => RouteMessage(Data, File1, File2, Text1, Text2, ImageWithCaptionUrl, TextUrl, FileUrl, _TokenId));
                    Job.Start();

                    Obj.ResponseCode = "00";
                    Obj.ResponseMessage = "Success";

                    context.tblAuditBOPromoes.Add(objaudit);
                    context.SaveChanges();
                }                    
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "SendTestMessage");
            }
            return Obj;
        }

        public JsonData SendTestMessage(string Text1, string Text2, tblAuditBOPromo objaudit)
        {
            string ImageWithCaptionUrl, TextUrl, FileUrl;

            ImageWithCaptionUrl = ConfigurationManager.AppSettings["SendFileWithCaptionAPILink"].ToString();
            TextUrl = ConfigurationManager.AppSettings["SendTextAPILink"].ToString();
            FileUrl = ConfigurationManager.AppSettings["SendFilesAPILink"].ToString();

            List<GroupCode> Data = new List<GroupCode>();
            JsonData Obj = new JsonData();
            string _MobileNo, _TokenId;
            _MobileNo = string.Empty;
            _TokenId = ConfigurationManager.AppSettings["BOTokenid"].ToString();
            
            try
            {
                using (var context = new CommonDBContext())
                {
                    //Data = context.WAReports.Where(x => x.SMSStatus == "9" && x.Status == "1").ToList();
                    //Data = context.WAReports.Where(x => x.SMSStatus == "5" && x.Status == "1").ToList();
                    
                    Data = context.Database.SqlQuery<GroupCode>("select distinct W.GroupCode as GroupCodeId from WAReport W where W.SMSStatus = 9 and W.Status = 1 Group by W.GroupCode").ToList();

                    Thread Job = new Thread(() => RouteMessage(Data, Text1, Text2, TextUrl, _TokenId));
                    Job.Start();

                    Obj.ResponseCode = "00";
                    Obj.ResponseMessage = "Success";

                    context.tblAuditBOPromoes.Add(objaudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestMessage");
            }
            return Obj;
        }

        public JsonData SendMessage(string File1, string Text1, string File2, string Text2,string Category,tblAuditBOPromo objaudit)
        {
            string ImageWithCaptionUrl, TextUrl, FileUrl;

            ImageWithCaptionUrl = ConfigurationManager.AppSettings["SendFileWithCaptionAPILink"].ToString();
            TextUrl = ConfigurationManager.AppSettings["SendTextAPILink"].ToString();
            FileUrl = ConfigurationManager.AppSettings["SendFilesAPILink"].ToString();

            List<GroupCode> Data = new List<GroupCode>();
            JsonData Obj = new JsonData();
            string _MobileNo, _TokenId, _FileUrl1, _FileUrl2, _Message1, _Message2;
            _MobileNo = string.Empty;
            //_Url = string.Empty;
            _TokenId = ConfigurationManager.AppSettings["BOTokenid"].ToString();
            _FileUrl1 = File1;
            _FileUrl2 = File2;
            _Message1 = Text1;
            _Message2 = Text2;
            try
            {
                using (var context = new CommonDBContext())
                {
                    if(Category == "ALL")
                    {
                        //Data = context.Database.SqlQuery<WAReport>("select W.GroupCode,MIN(W.SlNo) as SlNo,MIN(W.GroupName) as GroupName,MIN(W.IPAddress) as IPAddress,MIN(W.DBName),MIN(W.DBId),MIN(W.DBPassword),MIN(W.SMSStatus),MIN(W.EmailId),MIN(W.BrandId),MIN(W.GroupId),MIN(W.Status) from WAReport W where W.SMSStatus = 0 and W.Status = 1 Group by W.GroupCode").ToList();
                        Data = context.Database.SqlQuery<GroupCode>("select distinct W.GroupCode as GroupCodeId from WAReport W where W.SMSStatus = 0 and W.Status = 1 Group by W.GroupCode").ToList();
                    }
                    else
                    {
                        Data = context.Database.SqlQuery<GroupCode>("select distinct W.GroupCode as GroupCodeId from tblGroupDetails T inner join WAReport W On W.GroupId = T.GroupId where T.IsActive = 1 and W.SMSStatus = 0 and W.Status = 1 and T.RetailCategory = @Category Group by W.GroupCode", new SqlParameter("@Category", Category)).ToList();
                    }

                    Thread Job = new Thread(() => RouteMessage(Data, File1, File2, Text1, Text2, ImageWithCaptionUrl, TextUrl, FileUrl, _TokenId));
                    Job.Start();

                    Obj.ResponseCode = "00";
                    Obj.ResponseMessage = "Success";

                    context.tblAuditBOPromoes.Add(objaudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestMessage");
            }
            return Obj;
        }

        public JsonData SendMessage(string Text1, string Text2, string Category, tblAuditBOPromo objaudit)
        {
            string ImageWithCaptionUrl, TextUrl, FileUrl;

            ImageWithCaptionUrl = ConfigurationManager.AppSettings["SendFileWithCaptionAPILink"].ToString();
            TextUrl = ConfigurationManager.AppSettings["SendTextAPILink"].ToString();
            FileUrl = ConfigurationManager.AppSettings["SendFilesAPILink"].ToString();

            List<GroupCode> Data = new List<GroupCode>();
            JsonData Obj = new JsonData();
            string _MobileNo, _TokenId;
            _MobileNo = string.Empty;
            //_Url = string.Empty;
            _TokenId = ConfigurationManager.AppSettings["BOTokenid"].ToString();
            
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (Category == "ALL")
                    {
                        //Data = context.Database.SqlQuery<WAReport>("select W.GroupCode,MIN(W.SlNo) as SlNo,MIN(W.GroupName) as GroupName,MIN(W.IPAddress) as IPAddress,MIN(W.DBName),MIN(W.DBId),MIN(W.DBPassword),MIN(W.SMSStatus),MIN(W.EmailId),MIN(W.BrandId),MIN(W.GroupId),MIN(W.Status) from WAReport W where W.SMSStatus = 0 and W.Status = 1 Group by W.GroupCode").ToList();
                        Data = context.Database.SqlQuery<GroupCode>("select distinct W.GroupCode as GroupCodeId from WAReport W where W.SMSStatus = 0 and W.Status = 1 Group by W.GroupCode").ToList();
                    }
                    else
                    {
                        Data = context.Database.SqlQuery<GroupCode>("select distinct W.GroupCode as GroupCodeId from tblGroupDetails T inner join WAReport W On W.GroupId = T.GroupId where T.IsActive = 1 and W.SMSStatus = 0 and W.Status = 1 and T.RetailCategory = @Category Group by W.GroupCode", new SqlParameter("@Category", Category)).ToList();
                    }

                    Thread Job = new Thread(() => RouteMessage(Data, Text1, Text2,TextUrl, _TokenId));
                    Job.Start();

                    Obj.ResponseCode = "00";
                    Obj.ResponseMessage = "Success";

                    context.tblAuditBOPromoes.Add(objaudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SendTestMessage");
            }
            return Obj;
        }

        public void RouteMessage(List<GroupCode> Data, string  File1, string File2, string Text1, string Text2, string ImageWithCaptionUrl, string TextUrl, string FileUrl, string _TokenId)
        {
            string _MobileNo;
            _MobileNo = string.Empty;           
            
            if (!string.IsNullOrEmpty(File1) && !string.IsNullOrEmpty(Text1) && !string.IsNullOrEmpty(File2) && !string.IsNullOrEmpty(Text2))
            {
                foreach (var item in Data)
                {
                    _MobileNo = item.GroupCodeId;

                    Thread _job1 = new Thread(() => WAImageCaption(_MobileNo, ImageWithCaptionUrl, _TokenId, File1, Text1));
                    _job1.Start();

                    Thread _job2 = new Thread(() => WAImageCaption(_MobileNo, ImageWithCaptionUrl, _TokenId, File2, Text2));
                    _job2.Start();

                    Thread.Sleep(2000);
                }

            }
            else if (!string.IsNullOrEmpty(File1) && !string.IsNullOrEmpty(Text1) && !string.IsNullOrEmpty(File2) && string.IsNullOrEmpty(Text2))
            {
                foreach (var item in Data)
                {
                    _MobileNo = item.GroupCodeId;

                    Thread _job1 = new Thread(() => WAImageCaption(_MobileNo, ImageWithCaptionUrl, _TokenId, File1, Text1));
                    _job1.Start();

                    Thread _job2 = new Thread(() => WAImage(_MobileNo, FileUrl, _TokenId, File2));
                    _job2.Start();

                    Thread.Sleep(2000);
                }
            }
            else if (!string.IsNullOrEmpty(File1) && !string.IsNullOrEmpty(Text1) && string.IsNullOrEmpty(File2) && string.IsNullOrEmpty(Text2))
            {
                foreach (var item in Data)
                {
                    _MobileNo = item.GroupCodeId;

                    Thread _job1 = new Thread(() => WAImageCaption(_MobileNo, ImageWithCaptionUrl, _TokenId, File1, Text1));
                    _job1.Start();

                    Thread.Sleep(2000);
                }

            }
            else if (!string.IsNullOrEmpty(File1) && string.IsNullOrEmpty(Text1) && string.IsNullOrEmpty(File2) && string.IsNullOrEmpty(Text2))
            {
                foreach (var item in Data)
                {
                    _MobileNo = item.GroupCodeId;

                    Thread _job1 = new Thread(() => WAImage(_MobileNo, FileUrl, _TokenId, File1));
                    _job1.Start();

                    Thread.Sleep(2000);
                }

            }
        }

        public void RouteMessage(List<GroupCode> Data, string Text1, string Text2, string TextUrl, string _TokenId)
        {
            string _MobileNo;
            _MobileNo = string.Empty;

            if (!string.IsNullOrEmpty(Text1) && !string.IsNullOrEmpty(Text2))
            {
                foreach (var item in Data)
                {
                    _MobileNo = item.GroupCodeId;

                    Thread _job1 = new Thread(() => WAText(_MobileNo, TextUrl, _TokenId, Text1));
                    _job1.Start();

                    Thread _job2 = new Thread(() => WAText(_MobileNo, TextUrl, _TokenId, Text2));
                    _job2.Start();

                    Thread.Sleep(2000);
                }

            }
            else if (!string.IsNullOrEmpty(Text1) && string.IsNullOrEmpty(Text2))
            {
                foreach (var item in Data)
                {
                    _MobileNo = item.GroupCodeId;

                    Thread _job1 = new Thread(() => WAText(_MobileNo, TextUrl, _TokenId, Text1));
                    _job1.Start();

                    Thread.Sleep(2000);
                }
            }
        }

        public void WAText(string _MobileNo, string _Url, string _TokenId,string _Message)
        {
            string responseString;
            try
            {
                _Message = _Message.Replace("#99", "&");
                _Message = HttpUtility.UrlEncode(_Message);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("token={0}", _TokenId);
                sbposdata.AppendFormat("&phone={0}", _MobileNo);
                sbposdata.AppendFormat("&message={0}", _Message);

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
            }
            catch (ArgumentException ex)
            {

                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {

                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {

                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }

        public void WAImage(string _MobileNo, string _Url, string _TokenId, string _ImageUrl1)
        {
            string responseString;
            try
            {
                //_MobileMessage = _MobileMessage.Replace("#99", "&");
                //_MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("token={0}", _TokenId);
                sbposdata.AppendFormat("&phone={0}", _MobileNo);
                //sbposdata.AppendFormat("&message={0}", _MobileMessage);
                sbposdata.AppendFormat("&link={0}", _ImageUrl1);
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
            }
            catch (ArgumentException ex)
            {

                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {

                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {

                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }

        public void WAImageCaption(string _MobileNo, string _Url, string _TokenId, string _ImageUrl1, string _Message)
        {
            string responseString;
            try
            {

                _Message = _Message.Replace("#99", "&");
                _Message = HttpUtility.UrlEncode(_Message);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("token={0}", _TokenId);
                sbposdata.AppendFormat("&phone={0}", _MobileNo);
                sbposdata.AppendFormat("&message={0}", _Message);
                sbposdata.AppendFormat("&link={0}", _ImageUrl1);
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
            }
            catch (ArgumentException ex)
            {

                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {

                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {

                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }
    }
}
