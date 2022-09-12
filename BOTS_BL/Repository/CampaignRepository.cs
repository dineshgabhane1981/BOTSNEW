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

namespace BOTS_BL.Repository
{

    public class CampaignRepository
    {
        Exceptions newexception = new Exceptions();
        public CampaignTiles GetCampaignTilesData(string GroupId, string connstr)
        {
            CampaignTiles objCampaignTiles = new CampaignTiles();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignTiles = context.Database.SqlQuery<CampaignTiles>("sp_BOTS_CampaignMeasurement @pi_GroupId, @pi_Date, @pi_LoginId ", 
                        new SqlParameter("@pi_GroupId", GroupId), 
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", "")).FirstOrDefault<CampaignTiles>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignTiles;

        }
        public List<CampaignCelebrations> GetCampaignCelebrationsData(string GroupId, string connstr,string month, string year)
        {
            List<CampaignCelebrations> objCampaignCelebrations = new List<CampaignCelebrations>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignCelebrations = context.Database.SqlQuery<CampaignCelebrations>("sp_BOTS_CM_Celebration @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignCelebrations>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignCelebrations;
        }

        public List<CampaignCelebrationsData> GetCampaignCelebrationsSecondData(string GroupId, string connstr, string month, string year,string type)
        {
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignCelebrationsData = context.Database.SqlQuery<CampaignCelebrationsData>("sp_BOTS_CM_Celebration1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_Type",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_Type", type)).ToList<CampaignCelebrationsData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignCelebrationsData;
        }

        public List<CampaignPointsExpiry> GetCampaignPointsExpiryData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignPointsExpiry> objCampaignPointsExpiry = new List<CampaignPointsExpiry>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignPointsExpiry = context.Database.SqlQuery<CampaignPointsExpiry>("sp_BOTS_CM_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignPointsExpiry>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignPointsExpiry;
        }

        public List<CampaignCelebrationsData> GetCampaignPointsExpirySecondData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignCelebrationsData = context.Database.SqlQuery<CampaignCelebrationsData>("sp_BOTS_CM_PointsExpiry1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignCelebrationsData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignCelebrationsData;
        }

        public List<CampaignInactive> GetCampaignInactiveData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignInactive> objCampaignInactive = new List<CampaignInactive>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignInactive = context.Database.SqlQuery<CampaignInactive>("sp_BOTS_CM_InActive @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignInactive>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignInactive;
        }

        public List<CampaignInactiveData> GetCampaignInactiveSecondData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignInactiveData> objCampaignInactiveData = new List<CampaignInactiveData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignInactiveData = context.Database.SqlQuery<CampaignInactiveData>("sp_BOTS_CM_InActive1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignInactiveData>();

                    if(objCampaignInactiveData !=null)
                    {
                        foreach(var item in objCampaignInactiveData)
                        {
                            item.InActiveDateStr = item.InActiveDate.Value.ToString("yyyy-MM-dd");
                            item.TxnDateStr = item.TxnDate.Value.ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignInactiveData;
        }
        public List<Campaign> GetCampaignFirstData(string GroupId, string connstr, string month, string year)
        {
            List<Campaign> objCampaignData = new List<Campaign>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<Campaign>("sp_BOTS_CM_Campaign @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<Campaign>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<CampaignSecond> GetCampaignSecondData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignSecond>("sp_BOTS_CM_Campaign1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_CampaignId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignSecond>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<CampaignThird> GetCampaignThirdData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignThird>("sp_BOTS_CM_Campaign2 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_CampaignId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignThird>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }


        public List<CampaignSMSBlastFirst> GetCampaignSMSBlastFirstData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignSMSBlastFirst> objCampaignSMSBlastFirstData = new List<CampaignSMSBlastFirst>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignSMSBlastFirstData = context.Database.SqlQuery<CampaignSMSBlastFirst>("sp_BOTS_CM_SMSBlast @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignSMSBlastFirst>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignSMSBlastFirstData;
        }

        public List<CampaignSecond> GetSMSBlastsSecondData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignSecond>("sp_BOTS_CM_SMSBlast1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_CampaignId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignSecond>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<CampaignThird> GetSMSBlastsThirdData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignThird>("sp_BOTS_CM_SMSBlast2 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year, @pi_CampaignId",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignThird>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<OutletData> OutletData(string GroupId, string connstr)
        {

            List<OutletData> OutletData = new List<OutletData>();
            //List<CampaignOutet> CampaignOutet = new List<CampaignOutet>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {

                    //dataDashboard = context.Database.SqlQuery<ExecutiveSummary>("sp_Dashboard @pi_GroupId, @pi_Date", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString())).FirstOrDefault<ExecutiveSummary>();
                    //dataDashboard = context.Database.SqlQuery<ExecutiveSummary>("sp_BOTS_LoyaltyPerfromance @pi_GroupId, @pi_Date,@pi_LoginId,@pi_Month,@pi_Year,@pi_OutletId", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()), new SqlParameter("@pi_LoginId", LoginId), new SqlParameter("@pi_Month", ""), new SqlParameter("@pi_Year", ""), new SqlParameter("@pi_OutletId", "")).FirstOrDefault<ExecutiveSummary>();

                    OutletData = context.Database.SqlQuery<OutletData>("sp_OutletDashboard @pi_GroupId, @pi_Date", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd"))).ToList<OutletData>();
                   // OutletData = context.Database.SqlQuery<OutletData>("sp_OutletDashboard @pi_GroupId, @pi_Date", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date","2022-04-23")).ToList<OutletData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return OutletData;
        }

        public List<CommonSMSGateWayMaster> GatewayDetails(string GroupId, string connstr)
        {
            List<CommonSMSGateWayMaster> SMSGatewayDetails = new List<CommonSMSGateWayMaster>();
            SMSDetailsTemp SDT = new SMSDetailsTemp();
            string UserName, Password, SMSVendor;
            try
            {
                using (var context = new CommonDBContext())
                {

                    SMSGatewayDetails = (from c in context.CommonSMSGateWayMasters where (c.GroupId == GroupId && c.Status == "00") select c).ToList();

                    foreach (var item in SMSGatewayDetails)
                    {
                        

                        SDT.UserName = Convert.ToString(item.UserName);
                        SDT.Password = Convert.ToString(item.Password);
                        SDT.SMSVendor = Convert.ToString(item.SMSVendor);
                    }
                    SMSVendor = Convert.ToString(SDT.SMSVendor);
                    UserName = Convert.ToString(SDT.UserName);
                    Password = Convert.ToString(SDT.Password);
                    switch (SMSVendor)
                    {
                        case "TechnoCore":
                            
                            var baseAddress = "https://smsnotify.one/SMSApi/account/readstatus?userid="+ UserName + "&password="+ Password + "&output=json";
                            using (var client = new HttpClient())
                            {
                                using (var response1 = client.GetAsync(baseAddress).Result)
                                {
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                        var response2 = client.GetStringAsync(new Uri(baseAddress)).Result;
                                        JObject jsonObj = JObject.Parse(response2);
                                        IEnumerable<JToken> pricyProducts = jsonObj.SelectTokens("$..account");
                                        foreach (JToken item in pricyProducts)
                                        {
                                            CommonSMSGateWayMaster objbalance = new CommonSMSGateWayMaster();
                                            objbalance.smsBalance = Convert.ToString(item["smsBalance"]);
                                            SMSGatewayDetails.Add(objbalance);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("{0} ({1})", (int)response1.StatusCode, response1.ReasonPhrase);
                                    }
                                }
                            }
                            break;
                        case "VisionHLT":
                            
                            var baseAddress3 = "http://sms.visionhlt.com:8080/api/mt/GetBalance?User=" + UserName + "&Password=" + Password;
                            using (var client = new HttpClient())
                            {
                                using (var response1 = client.GetAsync(baseAddress3).Result)
                                {
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                        var response2 = client.GetStringAsync(new Uri(baseAddress3)).Result;
                                        JObject jsonObj = JObject.Parse(response2);
                                        var Balance = JObject.Parse(response2)["Balance"];
                                        CommonSMSGateWayMaster objbalance = new CommonSMSGateWayMaster();
                                        
                                        string Temp = Convert.ToString(Balance);
                                        Temp = Temp.Substring(Temp.IndexOf("|") + 7);
                                        objbalance.smsBalance = Temp;
                                        SMSGatewayDetails.Add(objbalance);
                                    }
                                    else
                                    {
                                        Console.WriteLine("{0} ({1})", (int)response1.StatusCode, response1.ReasonPhrase);
                                    }
                                }
                            }
                            break;
                        case "Pinnacle":

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_11671 = (HttpWebRequest)WebRequest.Create("https://api.pinnacle.in/index.php/checkbalance");
                            httpWebRequest_11671.ContentType = "application/json";
                            httpWebRequest_11671.Headers.Add("Apikey", Password);
                            httpWebRequest_11671.Method = "POST";
                            var httpResponse_11671 = (HttpWebResponse)httpWebRequest_11671.GetResponse();
                            using (var streamReader_11671 = new StreamReader(httpResponse_11671.GetResponseStream()))
                            {
                                var Balance = streamReader_11671.ReadToEnd();
                                JObject jsonObj = JObject.Parse(Balance);
                                IEnumerable<JToken> pricyProducts = jsonObj.SelectTokens("$..data");
                                foreach (JToken item in pricyProducts)
                                {
                                    CommonSMSGateWayMaster objbalance = new CommonSMSGateWayMaster();
                                    string Bal = Convert.ToString(item["balance"]);
                                    SMSGatewayDetails.Add(objbalance);
                                }
                            }
                            break;
                        default:
                            var baseAddress1 = "https://smsnotify.one/SMSApi/reseller/readuser?userid=0&password=0&output=json";
                            using (var client = new HttpClient())
                            {
                                using (var response1 = client.GetAsync(baseAddress1).Result)
                                {
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                        var response2 = client.GetStringAsync(new Uri(baseAddress1)).Result;
                                        JObject jsonObj = JObject.Parse(response2);
                                        
                                        CommonSMSGateWayMaster objbalance = new CommonSMSGateWayMaster();

                                        SMSGatewayDetails.Add(objbalance);
                                    }
                                    else
                                    {
                                        Console.WriteLine("{0} ({1})", (int)response1.StatusCode, response1.ReasonPhrase);
                                    }
                                }
                            }
                            break;

                    }


                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return SMSGatewayDetails;
        }

        public CustCount GetFiltData(string BaseType,string PointsBase,string Points,string PointsRange1, string OutletId, string GroupId, string connstr)
        {
            CustomerIdListAndCount objcount = new CustomerIdListAndCount();
            List < CustomerDetail> objcust = new List<CustomerDetail>();
            CustCount objcustAll = new CustCount();
            //var CustomerCount;
            // List<customerIdDetails> lstcustomerId = new List<customerIdDetails>();
            // var transcount = 0;
            using (var context = new BOTSDBContext(connstr))
            {
               
                var names = new string[] { "2", "4", "5", "7" };
                objcust = (from c in context.CustomerDetails where (names.Contains(c.CustomerThrough) && c.Status == "00") select c).ToList();

                //objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null).Count();
                //objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.EnrollingOutlet == OutletId).Count();
                
                

                try
                {
                    if (Points != "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                    }
                    if (BaseType == "1" && PointsBase == "" && Points == "" && OutletId == "")
                    {
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null).Count();
                    }
                    else if (BaseType == "1" && PointsBase == "" && Points == "" && OutletId != "")
                    {
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.EnrollingOutlet == OutletId).Count();
                    }
                    else if (BaseType == "2" && PointsBase == "" && Points == "" && OutletId == "")
                    {
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7")).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7")).Count();
                    }
                    else if (BaseType == "2" && PointsBase == "" && Points == "" && OutletId != "")
                    {
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7")).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.EnrollingOutlet == OutletId).Count();
                    }
                    else if (BaseType == "3" && PointsBase == "" && Points == "" && OutletId == "")
                    {
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.CustomerThrough == "1").Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.CustomerThrough == "1").Count();
                    }
                    else if (BaseType == "3" && PointsBase == "" && Points == "" && OutletId != "")
                    {
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.CustomerThrough == "1").Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && x.CustomerThrough == "1" && x.EnrollingOutlet == OutletId).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "1" && Points != "" && OutletId == "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points < DummyPoints).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points < DummyPoints).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "1" && Points != "" && OutletId != "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points < DummyPoints).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.EnrollingOutlet == OutletId && x.Points < DummyPoints).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "2" && Points != "" && OutletId == "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null &&(x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points > DummyPoints).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null &&(x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points > DummyPoints).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "2" && Points != "" && OutletId != "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points > DummyPoints).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points > DummyPoints && x.EnrollingOutlet == OutletId).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "3" && Points != "" && OutletId == "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points == DummyPoints).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points == DummyPoints).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "3" && Points != "" && OutletId != "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points == DummyPoints).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.Points == DummyPoints && x.EnrollingOutlet == OutletId).Count();
                    }

                    else if (BaseType == "4" && PointsBase == "4" && Points != "" && PointsRange1!="" && OutletId == "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        int DummyPoints2 = Convert.ToInt32(PointsRange1);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7")).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && (x.Points >= DummyPoints && x.Points <= DummyPoints2)).Count();
                    }
                    else if (BaseType == "4" && PointsBase == "4" && Points != "" && PointsRange1 != "" && OutletId != "")
                    {
                        int DummyPoints = Convert.ToInt32(Points);
                        int DummyPoints2 = Convert.ToInt32(PointsRange1);
                        objcustAll.CustCountALL = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && x.EnrollingOutlet == OutletId).Count();
                        objcustAll.CustFiltered = context.CustomerDetails.Where(x => x.Status == "00" && x.IsSMS == null && (x.CustomerThrough == "2" || x.CustomerThrough == "4" || x.CustomerThrough == "5" || x.CustomerThrough == "7") && (x.Points >= DummyPoints && x.Points <= DummyPoints2) && x.EnrollingOutlet == OutletId).Count();
                    }

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                
            }

            return objcustAll;
        }

        public List<CampaignSaveDetails> SaveCampaignData(string BaseType, string Equality, string Points, string OutletId,string Srcipt,string StartDate,string EndDate,string CampaignName,string SMSType, string ScriptType, string Scheduledatetime,string TempId,string PointsRange1, string GroupId, string connstr)
        {
            List<CampaignSaveDetails> Data = new List<CampaignSaveDetails>();

            DateTime Sched = new DateTime();
            if (string.IsNullOrEmpty(Scheduledatetime) == true)
            {
                 Scheduledatetime = "1900-01-01 00:00:00";

                 //Sched = Convert.ToDateTime(DateTime.ParseExact(Scheduledatetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            }
            //else
            //{
            //    Sched = Convert.ToDateTime(DateTime.ParseExact(Scheduledatetime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            //}

            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                     Data = context.Database.SqlQuery<CampaignSaveDetails>("sp_BOTS_CreateCampaign @pi_GroupId, @pi_Date,@pi_BaseType,@pi_Equality,@pi_Points,@pi_OutletId,@pi_Script,@pi_CampStartDate,@pi_CampEndDate,@pi_CampName,@pi_SMSType,@pi_ScriptType,@pi_Scheduledatetime,@pi_TempId,@pi_PointsRange1", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),new SqlParameter("@pi_BaseType", BaseType),new SqlParameter("@pi_Equality", Equality),new SqlParameter("@pi_Points", Points),new SqlParameter("@pi_OutletId", OutletId),new SqlParameter("@pi_Script", Srcipt),new SqlParameter("@pi_CampStartDate", StartDate),new SqlParameter("@pi_CampEndDate", EndDate),new SqlParameter("@pi_CampName", CampaignName),new SqlParameter("@pi_SMSType", SMSType), new SqlParameter("@pi_ScriptType", ScriptType), new SqlParameter("@pi_Scheduledatetime", Scheduledatetime), new SqlParameter("@pi_TempId", TempId), new SqlParameter("@pi_PointsRange1", PointsRange1)).ToList<CampaignSaveDetails>();                     
                }
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return Data;
        }
    
        public List<CampaignMemberDetail> CampDataDownload(string CampaignId,string GroupId,string connectionString)
        {
            List<CampaignMemberDetail> CmpData = new List<CampaignMemberDetail>();
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    CmpData = context.CampaignMemberDetails.Where(x => x.CampaignId == CampaignId).ToList();
                     //CmpData1 = Data.ToList<CampDownload>;
                    // CampData = CmpData.AsEnumerable().ToList<CampDownload>();

                }
                catch(Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
            }
                return CmpData;
        }

        public List<LisCampaign> GetCampList(string GroupId, string connectionString)
        {
            List<LisCampaign> CM = new List<LisCampaign>();
            List<LisCampaign> CM3 = new List<LisCampaign>();

            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    DateTime CDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    var CM1 = context.CampaignMasters.OrderByDescending(x => x.CampaignId).Select(x => new { x.CampaignId, x.CampaignName, x.StartDate, x.EndDate, x.Status, x.ControlBase, x.CampaignBase }).ToList();
                    
                    //var CM1 = ((from c in context.CampaignMasters orderby c.CampaignId descending  select c).Take(5)).ToList();

                    
                        foreach (var item in CM1)
                        {
                            LisCampaign itemData = new LisCampaign();
                            string D, D1;
                            //DateTime D1 = new DateTime();   
                            itemData.CampaignId = item.CampaignId;
                            itemData.CampaignName = item.CampaignName;
                            //D1 = item.StartDate;
                            //itemData.StartDate = D.ToString("yyyy-MM-dd");
                            itemData.StartDate = (item.StartDate); //DateTime.ToString()
                            itemData.EndDate = item.EndDate;
                            //D1 = Convert.ToDateTime(item.EndDate);
                            itemData.Status = item.Status;
                            itemData.ControlBase = Convert.ToString(item.ControlBase);
                            itemData.CampaignBase = Convert.ToString(item.CampaignBase);

                            CM.Add(itemData);
                        }
                        var CMD = ((from c in CM orderby c.CampaignId descending select c).Take(5)).ToList();

                    foreach (var item in CMD)
                    {
                        LisCampaign itemData = new LisCampaign();
                        itemData.CampaignId = item.CampaignId;
                        itemData.CampaignName = item.CampaignName;
                        itemData.StartDate = item.StartDate;
                        itemData.EndDate = item.EndDate;
                        itemData.Status = item.Status;
                        itemData.ControlBase = Convert.ToString(item.ControlBase);
                        itemData.CampaignBase = Convert.ToString(item.CampaignBase);

                        CM3.Add(itemData);
                    }

                    //var CMD = (from c in context.CampaignMasters select c.CampaignId,c.CampaignName,c.StartDate,c.EndDate).ToList();
                    //CmpData1 = Data.ToList<CampDownload>;
                    // CampData = CmpData.AsEnumerable().ToList<CampDownload>();

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                
            }
            return CM3;
        }

        public bool SendDLTData (string CampaignId,string GroupId, string connectionString)
        {
            SPResponse SR = new SPResponse();
            bool status = new bool();
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                   // var CM1 = context.CampaignMasters.Where(x => x.EndDate >= CDate);
                    var CM1 = context.CampaignMasters.Where(x => x.CampaignId == CampaignId).FirstOrDefault();
                    
                    if(CM1.DLTStatus == null)
                    {
                        CM1.DLTStatus = "Process";
                        context.CampaignMasters.AddOrUpdate(CM1);
                        context.SaveChanges();
                    }

                    
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }

            }
            return status;
        }

        public List<DLTDetailsLst> CampDLTDetailsLst(string GroupId, string connectionString)
        {
            List<DLTDetailsLst> DLTLst = new List<DLTDetailsLst>();

            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    
                    var CM1 = context.CampaignMasters.Where(x => x.DLTStatus != null).Select(x => new { x.CampaignId, x.CampaignName, x.Script, x.DLTScript, x.DLTStatus, x.DLTRejectReson, x.TemplateID, x.TemplateName, x.TemplateType }).ToList();
                    foreach (var item in CM1)
                    {
                        DLTDetailsLst itemData = new DLTDetailsLst();
                        itemData.CampaignId = item.CampaignId;
                        itemData.CampaignName = item.CampaignName;
                        itemData.Script = item.Script;
                        itemData.DLTScript = item.DLTScript;
                        itemData.DLTStatus = item.DLTStatus;
                        itemData.DLTRejectedReson = item.DLTRejectReson;
                        itemData.TemplateID = item.TemplateID;
                        itemData.TemplateName = item.TemplateName;
                        itemData.TemplateType = item.TemplateType;
                        DLTLst.Add(itemData);

                    }


                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }

            }
            return DLTLst;

        }

        public List<DLTDetailsLst> UpdateCampDLCLinkDLTStatus(string Campid, string status, string reason,string Groupid,string connectionString)
        {
            List<DLTDetailsLst> CM = new List<DLTDetailsLst>();
            bool sts = new bool();
            sts = false;
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {                 
                    var CM2 = context.CampaignMasters.Where(x => x.CampaignId == Campid).FirstOrDefault();
                    if (CM2.DLTStatus == "Process")
                    {
                        CM2.DLTStatus = status;
                    }
                    CM2.DLTStatus = status;
                    CM2.DLTRejectReson = reason;
                    context.CampaignMasters.AddOrUpdate(CM2);
                    context.SaveChanges();
                    sts = true;
                    var CM1 = context.CampaignMasters.Where(x => x.CampaignId == Campid).ToList();
                    foreach (var item in CM1)
                    {
                        DLTDetailsLst itemData = new DLTDetailsLst();
                        itemData.CampaignId = Campid;
                        itemData.Status = sts;
                        itemData.DLTStatus = status;
                        CM.Add(itemData);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, Groupid);
                }
                return CM;
            }
            
        }

        public List<DLTDetailsLst> UpdateCampDLTRejectStat(string Campid, string status, string reason, string Groupid, string connectionString)
        {
            List<DLTDetailsLst> CM = new List<DLTDetailsLst>();
            bool sts = new bool();
            sts = false;
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    var CM2 = context.CampaignMasters.Where(x => x.CampaignId == Campid).FirstOrDefault();
                    
                    CM2.DLTStatus = status;
                    CM2.DLTRejectReson = reason;
                    context.CampaignMasters.AddOrUpdate(CM2);
                    context.SaveChanges();
                    sts = true;
                    var CM1 = context.CampaignMasters.Where(x => x.CampaignId == Campid).ToList();
                    foreach (var item in CM1)
                    {
                        DLTDetailsLst itemData = new DLTDetailsLst();
                        itemData.CampaignId = Campid;
                        itemData.Status = sts;
                        //itemData.DLTStatus = status;
                        CM.Add(itemData);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, Groupid);
                }
                return CM;
            }

        }

        public bool SaveDLCCampaignDetails(string Campid, List<DLTDetailsLst> CampDetails, string GroupId, string connectionString)
        {
            bool status;
            status = false;
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    var CM2 = context.CampaignMasters.Where(x => x.CampaignId == Campid).FirstOrDefault();

                    foreach (var item in CampDetails)
                    {
                        //DLTDetailsLst itemData = new DLTDetailsLst();
                        
                        //CM2.CampaignName = item.CampaignName;
                        CM2.DLTStatus = item.DLTStatus;
                        CM2.TemplateID = item.TemplateID;
                        CM2.TemplateName = item.TemplateName;
                        CM2.TemplateType = item.TemplateType;
                        CM2.DLTScript = item.DLTScript;
                        //CM2.DLTRejectReson = item.DLTRejectedReson;
                        //itemData.DLTStatus = status;
                       // CM.Add(itemData);
                    }

                    //CM2.DLTStatus = status;
                    //CM2.DLTRejectReson = reason;
                    context.CampaignMasters.AddOrUpdate(CM2);
                    context.SaveChanges();
                    status = true;
                    //var CM1 = context.CampaignMasters.Where(x => x.CampaignId == Campid).ToList();
                    
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, GroupId);
                }
                return status;
            }
        }

        public DataSet SendTestSMSData(string CampaignId,string TestNumber, string GroupId, string connstr)
        {
            string Url, UserName, Password, Sender;
            DataSet Data = new DataSet();
            DataTable Data1 = new DataTable();
            Data1.Columns.Add("Mobileno");
            Data1.Columns.Add("SMSScript");
            Data1.Columns.Add("SMSBrandId");
            Data1.Columns.Add("Url");
            Data1.Columns.Add("UserName");
            Data1.Columns.Add("Password");
            Data1.Columns.Add("SenderId");
            string[] TestSplit = TestNumber.Split(',');

            try
            {
                using (var context = new BOTSDBContext(connstr))
                {             
                    SqlConnection _Con11 = new SqlConnection(Convert.ToString(connstr));
                    SqlCommand _Cmd11 = new SqlCommand();
                    _Cmd11.Connection = _Con11;
                    if (_Con11.State == ConnectionState.Closed)
                        _Con11.Open();
                    _Cmd11.CommandType = CommandType.StoredProcedure;
                    _Cmd11.CommandTimeout = 380000;
                    _Cmd11.CommandText = "sp_BOTS_SendCampTestSMS";
                    _Cmd11.Parameters.AddWithValue("@pi_CampaignId", CampaignId);
                    SqlDataAdapter _daCounterId11 = new SqlDataAdapter(_Cmd11);
                    _daCounterId11.Fill(Data);
                    _Con11.Close();

                    DataTable SMSTemp = Data.Tables["Table1"];

                    DataRow DR = null;
                    foreach (string info in TestSplit)
                    {

                        DR = Data1.NewRow();
                        DR["Mobileno"] = info;
                        for (int i = 0; i < 1; i++)
                        {
                            DR["SMSScript"] = (Convert.ToString(SMSTemp.Rows[i]["SMSScript"]));
                            DR["SMSBrandId"] = (Convert.ToString(SMSTemp.Rows[i]["SMSBrandId"]));
                            DR["Url"] = (Url = Convert.ToString(SMSTemp.Rows[i]["Url"]));
                            DR["UserName"] = (UserName = Convert.ToString(SMSTemp.Rows[i]["UserName"]));
                            DR["Password"] = (Password = Convert.ToString(SMSTemp.Rows[i]["Password"]));
                            DR["SenderId"] = (Sender = Convert.ToString(SMSTemp.Rows[i]["SenderId"]));

                            Data1.Rows.Add(DR);
                        }
                    }
                    
                     Thread _job = new Thread(() => SendSMS(Data1));
                    _job.Start();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return Data;
        }

        public void SendSMS(DataTable Data1)
        {
            if (Data1.Rows.Count > 0)
            {
                for (int j = 0; j < Data1.Rows.Count; j++)
                {
                    string _MobileMessage = Data1.Rows[j]["SMSScript"].ToString();
                    string _SMSBrandId = Data1.Rows[j]["SMSBrandId"].ToString();
                    string _Url = Data1.Rows[j]["Url"].ToString();
                    string _UserName = Data1.Rows[j]["UserName"].ToString();
                    string _Password = Data1.Rows[j]["Password"].ToString();
                    string _MobileNo = Data1.Rows[j]["MobileNo"].ToString();
                    string _Sender = Data1.Rows[j]["SenderId"].ToString();

                    switch (_SMSBrandId)
                    {
                        case "00001": //Techno Core Unicode
                            string date_00001 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _MobileMessage = _MobileMessage.Replace("#99", "&");
                            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                            string type_00001 = "unicode";
                            StringBuilder sbposdata_00001 = new StringBuilder();
                            sbposdata_00001.AppendFormat("userid={0}", _UserName);
                            sbposdata_00001.AppendFormat("&password={0}", _Password);
                            sbposdata_00001.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata_00001.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata_00001.AppendFormat("&msg={0}", _MobileMessage);
                            sbposdata_00001.AppendFormat("&senderid={0}", _Sender);
                            sbposdata_00001.AppendFormat("&msgType={0}", type_00001);
                            sbposdata_00001.AppendFormat("&format={0}", type_00001);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_00001 = (HttpWebRequest)WebRequest.Create(_Url);
                            UTF8Encoding encoding_00001 = new UTF8Encoding();
                            byte[] data_00001 = encoding_00001.GetBytes(sbposdata_00001.ToString());
                            httpWReq_00001.Method = "POST";
                            httpWReq_00001.ContentType = "application/x-www-form-urlencoded";
                            httpWReq_00001.ContentLength = data_00001.Length;
                            using (Stream stream_00001 = httpWReq_00001.GetRequestStream())
                            {
                                stream_00001.Write(data_00001, 0, data_00001.Length);
                            }
                            HttpWebResponse response_00001 = (HttpWebResponse)httpWReq_00001.GetResponse();
                            StreamReader reader_00001 = new StreamReader(response_00001.GetResponseStream());
                            string responseString_00001 = reader_00001.ReadToEnd();
                            reader_00001.Close();
                            response_00001.Close();
                            break;
                        case "00002": // Value First Unicode
                            string date_00002 = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _MobileMessage = _MobileMessage.Replace("#99", "&");
                            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                            string type_00002 = "unicode";
                            StringBuilder sbposdata_00002 = new StringBuilder();
                            sbposdata_00002.AppendFormat("userid={0}", _UserName);
                            sbposdata_00002.AppendFormat("&password={0}", _Password);
                            sbposdata_00002.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata_00002.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata_00002.AppendFormat("&msg={0}", _MobileMessage);
                            sbposdata_00002.AppendFormat("&senderid={0}", _Sender);
                            sbposdata_00002.AppendFormat("&msgType={0}", type_00002);
                            sbposdata_00002.AppendFormat("&format={0}", type_00002);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq_00002 = (HttpWebRequest)WebRequest.Create(_Url);
                            UTF8Encoding encoding_00002 = new UTF8Encoding();
                            byte[] data_00002 = encoding_00002.GetBytes(sbposdata_00002.ToString());
                            httpWReq_00002.Method = "POST";
                            httpWReq_00002.ContentType = "application/x-www-form-urlencoded";
                            httpWReq_00002.ContentLength = data_00002.Length;
                            using (Stream stream_00002 = httpWReq_00002.GetRequestStream())
                            {
                                stream_00002.Write(data_00002, 0, data_00002.Length);
                            }
                            HttpWebResponse response_00002 = (HttpWebResponse)httpWReq_00002.GetResponse();
                            StreamReader reader_00002 = new StreamReader(response_00002.GetResponseStream());
                            string responseString_00002 = reader_00002.ReadToEnd();
                            reader_00002.Close();
                            response_00002.Close();
                            break;
                        case "00003": //Vision HLT English
                            var httpWebRequest_00003 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00003.ContentType = "application/json";
                            httpWebRequest_00003.Method = "POST";

                            using (var streamWriter_00003 = new StreamWriter(httpWebRequest_00003.GetRequestStream()))
                            {

                                string json_00003 = "{\"Account\":" +
                                                "{\"APIKey\":\"" + _Password + "\"," +
                                                "\"SenderId\":\"" + _Sender + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"0\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
                                                "\"Text\":\"" + _MobileMessage + "\"}]" +
                                                "}";
                                streamWriter_00003.Write(json_00003);
                            }

                            var httpResponse_00003 = (HttpWebResponse)httpWebRequest_00003.GetResponse();
                            using (var streamReader_00003 = new StreamReader(httpResponse_00003.GetResponseStream()))
                            {
                                var result_00003 = streamReader_00003.ReadToEnd();
                            }
                            break; 
                        case "00004": //Vision HLT Unicode
                            var httpWebRequest_00004 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00004.ContentType = "application/json";
                            httpWebRequest_00004.Method = "POST";

                            using (var streamWriter_00004 = new StreamWriter(httpWebRequest_00004.GetRequestStream()))
                            {

                                string json_00004 = "{\"Account\":" +
                                                "{\"APIKey\":\"" + _Password + "\"," +
                                                "\"SenderId\":\"" + _Sender + "\"," +
                                                "\"Channel\":\"Trans\"," +
                                                "\"DCS\":\"8\"," +
                                                "\"SchedTime\":null," +
                                                "\"GroupId\":null}," +
                                                "\"Messages\":[{\"Number\":\"91" + _MobileNo + "\"," +
                                                "\"Text\":\"" + _MobileMessage + "\"}]" +
                                                "}";
                                streamWriter_00004.Write(json_00004);
                            }

                            var httpResponse_00004 = (HttpWebResponse)httpWebRequest_00004.GetResponse();
                            using (var streamReader_00004 = new StreamReader(httpResponse_00004.GetResponseStream()))
                            {
                                var result_00004 = streamReader_00004.ReadToEnd();
                            }
                            break;
                        case "00005": //Pinnacle English
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_00005 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00005.ContentType = "application/json";
                            httpWebRequest_00005.Headers.Add("Apikey", _Password);
                            httpWebRequest_00005.Method = "POST";
                            _MobileMessage = _MobileMessage.Replace("#99", "&");

                            using (var streamWriter_00005 = new StreamWriter(httpWebRequest_00005.GetRequestStream()))
                            {

                                string json_00005 = "{\"sender\":\"" + _Sender + "\"," +
                                "\"message\":[{\"number\":\"91" + _MobileNo + "\"," +
                                 "\"text\":\"" + _MobileMessage + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                                streamWriter_00005.Write(json_00005);
                            }

                            var httpResponse_00005 = (HttpWebResponse)httpWebRequest_00005.GetResponse();
                            using (var streamReader_00005 = new StreamReader(httpResponse_00005.GetResponseStream()))
                            {
                                var result_00005 = streamReader_00005.ReadToEnd();
                            }
                            break;
                        case "00006": //Pinnacle Unicode
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            var httpWebRequest_00006 = (HttpWebRequest)WebRequest.Create(_Url);
                            httpWebRequest_00006.ContentType = "application/json";
                            httpWebRequest_00006.Headers.Add("Apikey", _Password);
                            httpWebRequest_00006.Method = "POST";
                            _MobileMessage = _MobileMessage.Replace("#99", "&");

                            using (var streamWriter_00006 = new StreamWriter(httpWebRequest_00006.GetRequestStream()))
                            {

                                string json_00006 = "{\"sender\":\"" + _Sender + "\"," +
                                "\"message\":[{\"number\":\"91" + _MobileNo + "\"," +
                                 "\"text\":\"" + _MobileMessage + "\"}]," + "\"messagetype\":\"TXT\"," + "\"dltentityid\":null ," + "\"dlttempid\":null}";
                                streamWriter_00006.Write(json_00006);
                            }

                            var httpResponse_00006 = (HttpWebResponse)httpWebRequest_00006.GetResponse();
                            using (var streamReader_00006 = new StreamReader(httpResponse_00006.GetResponseStream()))
                            {
                                var result_00006 = streamReader_00006.ReadToEnd();
                            }
                            break;
                        default:
                            string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                            _MobileMessage = _MobileMessage.Replace("#99", "&");
                            _MobileMessage = HttpUtility.UrlEncode(_MobileMessage);
                            string type = "text";
                            StringBuilder sbposdata = new StringBuilder();
                            sbposdata.AppendFormat("userid={0}", _UserName);
                            sbposdata.AppendFormat("&password={0}", _Password);
                            sbposdata.AppendFormat("&sendMethod={0}", "quick");
                            sbposdata.AppendFormat("&mobile={0}", _MobileNo);
                            sbposdata.AppendFormat("&msg={0}", _MobileMessage);
                            sbposdata.AppendFormat("&senderid={0}", _Sender);
                            sbposdata.AppendFormat("&msgType={0}", type);
                            sbposdata.AppendFormat("&format={0}", type);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(_Url);
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
                            string responseString = reader.ReadToEnd();
                            reader.Close();
                            response.Close();
                            break;
                    }
                }
            }

        }

        public List<CampaignSaveDetails> SendSMSData(string CampaignId, string GroupId, string connstr)
        {
            List<CampaignSaveDetails> Data = new List<CampaignSaveDetails>();

            //int Points1 = Int32.Parse(Points);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    Data = context.Database.SqlQuery<CampaignSaveDetails>("sp_BOTS_SendCampSMS @pi_CampaignId", new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignSaveDetails>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return Data;
        }

        public List<CampaignSaveDetails> SaveCampaignDataWA(string BaseType, string Equality, string Points, string OutletId, string Srcipt, string StartDate, string EndDate, string CampaignName, string SMSType, string MessageType, string Scheduledatetime, string PointsRange1, string GroupId, string connstr)
        {
            List<CampaignSaveDetails> Data = new List<CampaignSaveDetails>();

            DateTime Sched = new DateTime();
            if (string.IsNullOrEmpty(Scheduledatetime) == true)
            {
                Scheduledatetime = "1900-01-01 00:00:00";
            }
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    Data = context.Database.SqlQuery<CampaignSaveDetails>("sp_BOTS_CreateCampaign @pi_GroupId, @pi_Date,@pi_BaseType,@pi_Equality,@pi_Points,@pi_OutletId,@pi_Script,@pi_CampStartDate,@pi_CampEndDate,@pi_CampName,@pi_SMSType,@pi_MessageType,@pi_Scheduledatetime,@pi_TempId,@pi_PointsRange1", new SqlParameter("@pi_GroupId", GroupId), new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")), new SqlParameter("@pi_BaseType", BaseType), new SqlParameter("@pi_Equality", Equality), new SqlParameter("@pi_Points", Points), new SqlParameter("@pi_OutletId", OutletId), new SqlParameter("@pi_Script", Srcipt), new SqlParameter("@pi_CampStartDate", StartDate), new SqlParameter("@pi_CampEndDate", EndDate), new SqlParameter("@pi_CampName", CampaignName), new SqlParameter("@pi_SMSType", SMSType), new SqlParameter("@pi_MessageType", MessageType), new SqlParameter("@pi_Scheduledatetime", Scheduledatetime), new SqlParameter("@pi_PointsRange1", PointsRange1)).ToList<CampaignSaveDetails>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return Data;
        }

    }

}
