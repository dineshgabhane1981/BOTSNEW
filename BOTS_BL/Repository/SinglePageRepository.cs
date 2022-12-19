using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Migrations;

namespace BOTS_BL.Repository
{
    public class SinglePageRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public List<Tbl_SinglePageNonTransactingGroup> GetSinglePageNonTransactingGroups()
        {
            List<Tbl_SinglePageNonTransactingGroup> lstnontransactinggrp = new List<Tbl_SinglePageNonTransactingGroup>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    lstnontransactinggrp = context.Tbl_SinglePageNonTransactingGroup.Where(x => x.Date == DateTime.Today).OrderByDescending(p => p.DaysSinceLastTxn).ToList();
                    foreach (var item in lstnontransactinggrp)
                    {
                        item.DaySinceLastTxn = Convert.ToInt32(item.DaysSinceLastTxn);
                    }
                    lstnontransactinggrp = lstnontransactinggrp.OrderByDescending(x => x.DaySinceLastTxn).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSinglePageNonTransactingGroups");
            }

            return lstnontransactinggrp;
        }
        public List<Tbl_SinglePageNonTransactingOutlet> GetNonTransactingOutlet(string all)
        {
            List<Tbl_SinglePageNonTransactingOutlet> lstnontransactingoutlet = new List<Tbl_SinglePageNonTransactingOutlet>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (string.IsNullOrEmpty(all))
                    {
                        lstnontransactingoutlet = context.Tbl_SinglePageNonTransactingOutlet.Where(x => x.Date == DateTime.Today).OrderByDescending(i => i.DaysSinceLastTxn).Take(5).ToList();
                    }
                    else
                    {
                        lstnontransactingoutlet = context.Tbl_SinglePageNonTransactingOutlet.Where(x => x.Date == DateTime.Today).OrderByDescending(i => i.DaysSinceLastTxn).ToList();
                    }
                    foreach (var item in lstnontransactingoutlet)
                    {
                        item.DaySinceLastTxn = Convert.ToInt32(item.DaysSinceLastTxn);
                    }
                    lstnontransactingoutlet = lstnontransactingoutlet.OrderByDescending(x => x.DaySinceLastTxn).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNonTransactingOutlet");
            }

            return lstnontransactingoutlet;
        }

        public List<Tbl_SinglePageLowTransactingOutlet> GetLowTransactingOutlet(string all)
        {
            List<Tbl_SinglePageLowTransactingOutlet> lstlowtransactingoutlet = new List<Tbl_SinglePageLowTransactingOutlet>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (string.IsNullOrEmpty(all))
                    {
                        lstlowtransactingoutlet = context.Tbl_SinglePageLowTransactingOutlet.Where(x => x.Date == DateTime.Today).OrderBy(i => i.LowerByPercentage).Take(5).ToList();
                    }
                    else
                    {
                        lstlowtransactingoutlet = context.Tbl_SinglePageLowTransactingOutlet.Where(x => x.Date == DateTime.Today).OrderBy(i => i.LowerByPercentage).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLowTransactingOutlet");
            }

            return lstlowtransactingoutlet;
        }
        public Tbl_SinglePageSummaryTable GetSinglePageSummaryTable()
        {
            Tbl_SinglePageSummaryTable lstsummarytable = new Tbl_SinglePageSummaryTable();
            try
            {
                using (var context = new CommonDBContext())
                {
                    // lstsummarytable = context.Tbl_SinglePageSummaryTable.ToList();
                    var totalenroll = context.Tbl_SinglePageSummaryTable.Where(x => x.Date == DateTime.Today).Sum(i => i.TotalEnrolledBase);
                    var sumtxncountdaily = context.Tbl_SinglePageSummaryTable.Where(x => x.Date == DateTime.Today).Sum(i => i.TxnCountDaily);
                    var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var sumtxncountmtd = context.Tbl_SinglePageSummaryTable.Where(x => x.Date >= firstDayOfMonth).Sum(i => i.TxnCountMTD);
                    lstsummarytable.TotalEnrolledBase = totalenroll;
                    lstsummarytable.TxnCountDaily = sumtxncountdaily;
                    lstsummarytable.TxnCountMTD = sumtxncountmtd;

                }
            }
            catch (Exception ex)
            {
                // newexception.AddException(ex, GroupId);
            }

            return lstsummarytable;
        }
        public List<SinglepageLowerMetrics> GetLowerMetrics(String Id)
        {
            // SinglePageViewModel singlevm = new SinglePageViewModel();
            List<SinglepageLowerMetrics> lstlowermetrics = new List<SinglepageLowerMetrics>();
            List<Tbl_SinglePageLowReferralConversions> objlowreferralconversion = new List<Tbl_SinglePageLowReferralConversions>();
            List<Tbl_SinglePageLowerRedemptionRate> objlowerredemption = new List<Tbl_SinglePageLowerRedemptionRate>();
            List<Tbl_SinglePageLowProfileUpdates> objlowprofile = new List<Tbl_SinglePageLowProfileUpdates>();
            List<Tbl_SinglePageLowReferral> objlowreferral = new List<Tbl_SinglePageLowReferral>();
            List<Tbl_SinglePageHigherOnlyonceAndInactive> objhigheronlyonce = new List<Tbl_SinglePageHigherOnlyonceAndInactive>();
            DataSet dsm = new DataSet();
            using (var context = new CommonDBContext())
            {
                DateTime dt = DateTime.Now.Date;
                if (Id == "1")
                {
                    objlowerredemption = context.Tbl_SinglePageLowerRedemptionRate.Where(x => x.Date == dt).OrderByDescending(i => i.Value).ToList();
                    dsm = CreateDataTable(objlowerredemption, Id);
                    //.Where(x => x.Date == dt)
                }
                else if (Id == "3")
                {
                    objlowprofile = context.Tbl_SinglePageLowProfileUpdates.Where(x => x.Date == dt).OrderByDescending(i => i.Value).ToList();
                    dsm = CreateDataTable(objlowprofile, Id);

                }
                else if (Id == "2")
                {
                    objhigheronlyonce = context.Tbl_SinglePageHigherOnlyonceAndInactive.Where(x => x.Date == dt && x.CustomerVintage == "Higher Only Once").OrderByDescending(i => i.Value).ToList();
                    dsm = CreateDataTable(objhigheronlyonce, Id);

                }
                else if (Id == "6")
                {
                    objhigheronlyonce = context.Tbl_SinglePageHigherOnlyonceAndInactive.Where(x => x.Date == dt && x.CustomerVintage == "Higher InActive Rate").OrderByDescending(i => i.Value).ToList();
                    dsm = CreateDataTable(objhigheronlyonce, Id);

                }
                else if (Id == "4")
                {
                    objlowreferral = context.Tbl_SinglePageLowReferral.Where(x => x.Date == dt).OrderByDescending(i => i.Value).ToList();
                    dsm = CreateDataTable(objlowreferral, Id);

                }
                else if (Id == "5")
                {
                    objlowreferralconversion = context.Tbl_SinglePageLowReferralConversions.Where(x => x.Date == dt).OrderByDescending(i => i.Value).ToList();
                    dsm = CreateDataTable(objlowreferralconversion, Id);
                }


                DataSet ds = new DataSet();
                int fivecount = dsm.Tables[0].Rows.Count;
                int tencount = dsm.Tables[1].Rows.Count;//dtlessten.Rows.Count;
                int fifteencount = dsm.Tables[2].Rows.Count;//dtlessfifteen.Rows.Count;
                int thirtycount = dsm.Tables[3].Rows.Count;//dtlessthirty.Rows.Count;
                int fortycount = dsm.Tables[4].Rows.Count;//dtlessforty.Rows.Count;

                int[] arr = new int[] { fivecount, tencount, fifteencount, thirtycount, fortycount };
                Array.Sort<int>(arr, new Comparison<int>(
                  (i1, i2) => i2.CompareTo(i1)));

                int count = arr[0];
                DataTable Maindt = new DataTable();
                Maindt.Columns.Add("GroupNamelessthan5");
                Maindt.Columns.Add("Valuethanlessthan5", typeof(double));
                Maindt.Columns.Add("GroupNamelessthan10");
                Maindt.Columns.Add("Valuelessthan10", typeof(double));
                Maindt.Columns.Add("GroupNamelessthan15");
                Maindt.Columns.Add("Valuelessthan15", typeof(double));
                Maindt.Columns.Add("GroupNamelessthan30");
                Maindt.Columns.Add("Valuelessthan30", typeof(double));
                Maindt.Columns.Add("GroupNamelessthan40");
                Maindt.Columns.Add("Valuelessthan40", typeof(double));
                for (int a = 0; a < count; a++)
                {
                    DataRow dr = Maindt.NewRow();
                    if (dsm.Tables[0].Rows.Count >= a + 1)
                    {
                        dr["GroupNamelessthan5"] = Convert.ToString(dsm.Tables[0].Rows[a]["GroupName5"]);
                        dr["Valuethanlessthan5"] = Convert.ToString(dsm.Tables[0].Rows[a]["value5"]);
                    }
                    if (dsm.Tables[1].Rows.Count >= a + 1)
                    {
                        dr["GroupNamelessthan10"] = Convert.ToString(dsm.Tables[1].Rows[a]["GroupName10"]);
                        dr["Valuelessthan10"] = Convert.ToString(dsm.Tables[1].Rows[a]["value10"]);
                    }
                    if (dsm.Tables[2].Rows.Count >= a + 1)
                    {
                        dr["GroupNamelessthan15"] = Convert.ToString(dsm.Tables[2].Rows[a]["GroupName15"]);
                        dr["Valuelessthan15"] = Convert.ToString(dsm.Tables[2].Rows[a]["value15"]);
                    }
                    if (dsm.Tables[3].Rows.Count >= a + 1)
                    {
                        dr["GroupNamelessthan30"] = Convert.ToString(dsm.Tables[3].Rows[a]["GroupName30"]);
                        dr["Valuelessthan30"] = Convert.ToString(dsm.Tables[3].Rows[a]["value30"]);
                    }
                    if (dsm.Tables[4].Rows.Count >= a + 1)
                    {
                        dr["GroupNamelessthan40"] = Convert.ToString(dsm.Tables[4].Rows[a]["GroupName40"]);
                        dr["Valuelessthan40"] = Convert.ToString(dsm.Tables[4].Rows[a]["value40"]);
                    }


                    Maindt.Rows.Add(dr);

                }
                //
                //  List< SinglepageLowerMetrics > = Maindt.ToCollection<SinglepageLowerMetrics>();
                lstlowermetrics = Maindt.AsEnumerable()
                                  .Select(x => new SinglepageLowerMetrics()
                                  {
                                      GroupNamelessthan5 = WrapDbNullValue<string>(x.Field<string>("GroupNamelessthan5")),
                                      Valuethanlessthan5 = x.Field<double?>("Valuethanlessthan5"),
                                      GroupNamelessthan10 = WrapDbNullValue<string>(x.Field<string>("GroupNamelessthan10")),
                                      Valuelessthan10 = x.Field<double?>("Valuelessthan10"),
                                      GroupNamelessthan15 = WrapDbNullValue<string>(x.Field<string>("GroupNamelessthan15")),
                                      Valuelessthan15 = x.Field<double?>("Valuelessthan15"),
                                      GroupNamelessthan30 = WrapDbNullValue<string>(x.Field<string>("GroupNamelessthan30")),
                                      Valuelessthan30 = x.Field<double?>("Valuelessthan30"),
                                      GroupNamelessthan40 = WrapDbNullValue<string>(x.Field<string>("GroupNamelessthan40")),
                                      Valuelessthan40 = x.Field<double?>("Valuelessthan40")
                                  }).ToList();

            }


            return lstlowermetrics;

        }

        public DataSet CreateDataTable<T>(List<T> list, string Id)
        {
            DataSet dsmain = new DataSet();
            double val5;
            double val10;
            double val15;
            double val30;
            double val40;

            DataTable dtlessfive = new DataTable();
            dtlessfive.Columns.Add("GroupName5");
            dtlessfive.Columns.Add("value5");


            DataTable dtlessten = new DataTable();
            dtlessten.Columns.Add("GroupName10");
            dtlessten.Columns.Add("value10");

            DataTable dtlessfifteen = new DataTable();
            dtlessfifteen.Columns.Add("GroupName15");
            dtlessfifteen.Columns.Add("value15");


            DataTable dtlessthirty = new DataTable();
            dtlessthirty.Columns.Add("GroupName30");
            dtlessthirty.Columns.Add("value30");


            DataTable dtlessforty = new DataTable();
            dtlessforty.Columns.Add("GroupName40");
            dtlessforty.Columns.Add("value40");

            int count = typeof(T).GetProperties().Count();
            for (int y = 0; y < list.Count; y++)
            {
                object propertyValue;
                if (Id == "3")
                {
                    propertyValue = typeof(T).GetProperties()[5].GetValue(list[y], null);
                }
                else
                {
                    propertyValue = typeof(T).GetProperties()[4].GetValue(list[y], null);
                }
                var propertygroupname = typeof(T).GetProperties()[1].GetValue(list[y], null);

                if (Convert.ToInt32(propertyValue) < 5 && Convert.ToInt32(propertyValue) > 0)
                {
                    DataRow dr5 = dtlessfive.NewRow();
                    val5 = Convert.ToDouble(propertyValue);
                    dr5[0] = propertygroupname;
                    dr5[1] = val5;
                    dtlessfive.Rows.Add(dr5);

                }
                else if (Convert.ToInt32(propertyValue) < 10 && Convert.ToInt32(propertyValue) > 5)
                {
                    DataRow dr10 = dtlessten.NewRow();

                    val10 = Convert.ToDouble(propertyValue);
                    dr10[0] = propertygroupname;
                    dr10[1] = val10;
                    dtlessten.Rows.Add(dr10);

                }
                else if (Convert.ToInt32(propertyValue) < 15 && Convert.ToInt32(propertyValue) > 10)
                {
                    DataRow dr15 = dtlessfifteen.NewRow();
                    val15 = Convert.ToDouble(propertyValue);
                    dr15[0] = propertygroupname;
                    dr15[1] = val15;
                    dtlessfifteen.Rows.Add(dr15);

                }
                else if (Convert.ToInt32(propertyValue) < 30 && Convert.ToInt32(propertyValue) > 15)
                {
                    DataRow dr30 = dtlessthirty.NewRow();
                    val30 = Convert.ToDouble(propertyValue);
                    dr30[0] = propertygroupname;
                    dr30[1] = val30;
                    dtlessthirty.Rows.Add(dr30);

                }
                else if (Convert.ToInt32(propertyValue) < 40 && Convert.ToInt32(propertyValue) > 30)
                {
                    DataRow dr40 = dtlessforty.NewRow();
                    val40 = Convert.ToDouble(propertyValue);
                    dr40[0] = propertygroupname;
                    dr40[1] = val40;
                    dtlessforty.Rows.Add(dr40);

                }


            }
            dsmain.Tables.Add(dtlessfive);
            dsmain.Tables.Add(dtlessten);
            dsmain.Tables.Add(dtlessfifteen);
            dsmain.Tables.Add(dtlessthirty);
            dsmain.Tables.Add(dtlessforty);
            return dsmain;
        }
        private static T WrapDbNullValue<T>(object value)
        {
            if (value != null && value == DBNull.Value)
            {
                return default(T);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public CommunicationsinglePageData GetCommunicationWhatsAppExpiryData()
        {
            CommunicationsinglePageData lstSmsbalance = new CommunicationsinglePageData();
            Root objroot = new Root();
            Response objresponce = new Response();
            UserList objuserlist = new UserList();
            User objuser = new User();
            List<SMSBalance> lstbalance = new List<SMSBalance>();
            List<WhatsAppBalance> lstWAbalance = new List<WhatsAppBalance>();
            DateTime next10day = DateTime.Now.AddDays(10);
            DataSet retVal = new DataSet();
            try
            {
                //Technocore SMS Balance (Narayan)
                var baseAddress = "https://smsnotify.one/SMSApi/reseller/readuser?userid=Blueotrans&password=123456&output=json";
                using (var client = new HttpClient())
                {
                    using (var response1 = client.GetAsync(baseAddress).Result)
                    {
                        if (response1.IsSuccessStatusCode)
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var response2 = client.GetStringAsync(new Uri(baseAddress)).Result;
                            JObject jsonObj = JObject.Parse(response2);
                            IEnumerable<JToken> pricyProducts = jsonObj.SelectTokens("$..user");
                            foreach (JToken item in pricyProducts)
                            {
                                SMSBalance objbalance = new SMSBalance();
                                objbalance.OutletName = item["userName"].ToString();
                                objbalance.BrandName = item["userName"].ToString();
                                objbalance.SmsBalance = (string)item["smsBalance"];
                                objbalance.Status = (string)item["userStatus"];
                                objbalance.CompanyName = "Technocore";
                                lstbalance.Add(objbalance);
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response1.StatusCode, response1.ReasonPhrase);
                        }
                    }
                }
                //lstbalance = lstbalance.Where(x => x.SmsBalance != "0 Credits").ToList();
                foreach (var item in lstbalance)
                {
                    item.SmsBalance1 = Convert.ToInt32(item.SmsBalance.Substring(0, item.SmsBalance.IndexOf(" ")));
                }

                //vision SMS Balance
                baseAddress = "https://sms.visionhlt.com/api/mt/GetCreditReport?userid=4438%20&password=123456";
                using (var client = new HttpClient())
                {
                    using (var response1 = client.GetAsync(baseAddress).Result)
                    {
                        if (response1.IsSuccessStatusCode)
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var response2 = client.GetStringAsync(new Uri(baseAddress)).Result;

                            JObject jsonObj = JObject.Parse(response2);
                            var WAData = jsonObj.Children().ToList();
                            IEnumerable<JToken> pricyProducts = jsonObj.SelectTokens("CreditReports").Children().ToList();

                            foreach (JToken item in pricyProducts)
                            {
                                var lst = item.Children().ToList();
                                SMSBalance objbalance = new SMSBalance();
                                objbalance.BrandName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)lst[1]).Value);
                                objbalance.SmsBalance = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)lst[0]).Value);
                                objbalance.SmsBalance1 = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)lst[0]).Value);
                                objbalance.CompanyName = "Vision";
                                lstbalance.Add(objbalance);
                            }
                        }
                    }
                }
                //lstbalance = lstbalance.Where(x => x.SmsBalance1 < 1000).ToList();
                lstSmsbalance.objSMSBalance = lstbalance.OrderByDescending(x => x.SmsBalance1).ToList();

                //baseAddress = "https://technocorelogic.com/api/checkbalance?user=9511836639&pass=9930005673&url=https://www.enotify.app";
                baseAddress = "https://technocorelogic.com/api/checkbalance?user=919028099210&pass=9930005673&url=https://bo.enotify.app";
                using (var client = new HttpClient())
                {
                    using (var response1 = client.GetAsync(baseAddress).Result)
                    {
                        if (response1.IsSuccessStatusCode)
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var response2 = client.GetStringAsync(new Uri(baseAddress)).Result;
                            JObject jsonObj = JObject.Parse(response2);
                            var WAData = jsonObj.Children().ToList();
                            int count = 0;
                            foreach (JToken item in WAData)
                            {
                                if (count > 0)
                                {
                                    WhatsAppBalance objbalance = new WhatsAppBalance();
                                    objbalance.BrandName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)item).Value["ClientName"]);
                                    objbalance.OutletName = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)item).Value["InstanceName"]);
                                    objbalance.WABalance = Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)item).Value["BalanceQuota"]);
                                    objbalance.Status = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)item).Value["LoginStatus"]);
                                    objbalance.WAExpiryDate = Convert.ToDateTime(((Newtonsoft.Json.Linq.JProperty)item).Value["Expiry"]);
                                    objbalance.InstanceID = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)item).Value["InstanceID"]);
                                    lstWAbalance.Add(objbalance);
                                }
                                count++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} ({1})", (int)response1.StatusCode, response1.ReasonPhrase);
                        }
                    }
                }
                //List<WhatsAppBalance> lstWAbalance1 = new List<WhatsAppBalance>();
                //baseAddress = "https://technocorelogic.com/api/checkqueue?user=919561124670&pass=9930005673&url=https://www.enotify.app";
                //using (var client = new HttpClient())
                //{
                //    using (var response1 = client.GetAsync(baseAddress).Result)
                //    {
                //        if (response1.IsSuccessStatusCode)
                //        {
                //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //            var response2 = client.GetStringAsync(new Uri(baseAddress)).Result;
                //            JObject jsonObj = JObject.Parse(response2);
                //            var WAData = jsonObj.Children().ToList();
                //            WAData = jsonObj.SelectTokens("data").Children().ToList();
                //            foreach (JToken item in WAData)
                //            {
                //                var lst = item.Children().ToList();
                //                WhatsAppBalance objbalance = new WhatsAppBalance();                                
                //                objbalance.InstanceID = Convert.ToString(((Newtonsoft.Json.Linq.JProperty)lst[4]).Value);
                //                objbalance.queue= Convert.ToInt32(((Newtonsoft.Json.Linq.JProperty)lst[9]).Value);
                //                lstWAbalance1.Add(objbalance);
                //            }
                //        }
                //    }
                //}
                //foreach(var item in lstWAbalance)
                //{
                //    item.queue = lstWAbalance1.Where(x => x.InstanceID == item.InstanceID).Select(y => y.queue).FirstOrDefault();
                //}

                lstSmsbalance.objWhatsAppBalance = lstWAbalance.Where(y => y.WABalance < 1000).OrderByDescending(x => x.WABalance).ToList();
                DateTime checkDate = DateTime.Today.AddDays(10);

                lstSmsbalance.objWhatsAppExpiryDate = lstWAbalance.Where(x => x.WAExpiryDate < checkDate && x.WAExpiryDate > DateTime.Today).OrderBy(y => y.WAExpiryDate).ToList();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }
            return lstSmsbalance;
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public List<CitywiseReport> GetCityWiseData()
        {
            List<CityWiseData> objData = new List<CityWiseData>();
            List<CitywiseReport> lstCitywiseReport = new List<CitywiseReport>();
            List<CitywiseReport> lstCitywiseReportNew = new List<CitywiseReport>();
            List<CitywiseReport> lstFinalData = new List<CitywiseReport>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var groups = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true).ToList();
                    foreach (var item in groups)
                    {
                        try
                        {
                            var groupId = Convert.ToInt32(item.GroupId);
                            CityWiseData objItem = new CityWiseData();

                            objItem.GroupId = Convert.ToString(item.GroupId);
                            objItem.GroupName = item.GroupName;
                            var category = (from c in context.tblGroupDetails
                                            join ct in context.tblCategories on c.RetailCategory equals ct.CategoryId
                                            where c.GroupId == groupId
                                            select new
                                            {
                                                ct.CategoryName
                                            }).FirstOrDefault();
                            objItem.CategoryName = category.CategoryName;

                            var city = (from c in context.tblGroupDetails
                                        join ct in context.tblCities on c.City equals ct.CityId
                                        where c.GroupId == groupId
                                        select new
                                        {
                                            ct.CityName
                                        }).FirstOrDefault();

                            objItem.CityName = city.CityName;

                            string connStr = CR.GetCustomerConnString(Convert.ToString(item.GroupId));
                            using (var contextdb = new BOTSDBContext(connStr))
                            {

                                //objItem.MemberBase = contextdb.CustomerDetails.Count();
                                objItem.MemberBase = contextdb.CustomerDetails.Count();

                                var sqlQ = $"SELECT COUNT(*) as Count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BulkUploadCustList'";
                                var exist = contextdb.Database.SqlQuery<int>(sqlQ).FirstOrDefault();
                                if (exist > 0)
                                {
                                    objItem.MemberBulkUpload = contextdb.BulkUploadCustLists.Count();
                                    objItem.TotalMemberBase = objItem.MemberBase + contextdb.BulkUploadCustLists.Count();
                                }
                                else
                                {
                                    objItem.MemberBulkUpload = 0;
                                    objItem.TotalMemberBase = objItem.MemberBase;
                                }

                            }
                            objData.Add(objItem);
                        }
                        catch (Exception ex)
                        {
                            newexception.AddException(ex, "Inside For Loop GetCityWiseData");
                        }
                    }

                    var uniqueCities = objData.GroupBy(x => x.CityName).Select(y => y.First()).ToList();
                    var uniqueCategories = objData.GroupBy(x => x.CategoryName).Select(y => y.First()).ToList();

                    var Cities = context.tblCities.ToList();
                    var Categories = context.tblCategories.ToList();

                    foreach (var city in Cities)
                    {

                        foreach (var category in Categories)
                        {
                            CitywiseReport objReport = new CitywiseReport();
                            objReport.CityName = city.CityName;
                            objReport.CategoryName = category.CategoryName;
                            var lst = objData.Where(x => x.CityName == city.CityName && x.CategoryName == category.CategoryName).ToList();
                            var total = lst.Sum(x => x.TotalMemberBase);
                            objReport.MemberBase = total;
                            lstCitywiseReport.Add(objReport);
                        }

                    }

                    foreach (var city in Cities)
                    {
                        var count = lstCitywiseReport.Where(x => x.CityName == city.CityName && x.MemberBase != 0).Count();
                        if (count > 0)
                        {
                            var filterList = lstCitywiseReport.Where(x => x.CityName == city.CityName).ToList();
                            lstCitywiseReportNew.AddRange(filterList);
                        }
                    }
                    foreach (var category in Categories)
                    {
                        var count = lstCitywiseReportNew.Where(x => x.CategoryName == category.CategoryName && x.MemberBase != 0).Count();
                        if (count > 0)
                        {
                            var filterList = lstCitywiseReportNew.Where(x => x.CategoryName == category.CategoryName).ToList();
                            lstFinalData.AddRange(filterList);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCityWiseData");
            }
            return lstFinalData;
        }
        public List<TransactionWise> GetAllTransactionData()
        {
            List<TransactionWise> objData = new List<TransactionWise>();
            long count = 0;
            decimal? TotalInv = 0;
            decimal? TotalRedumptionInv = 0;
            decimal? TotalCustPoints = 0;
            List<tblGroupDetail> groups = new List<tblGroupDetail>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    groups = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true && x.GroupId != 1051).ToList();
                }
                foreach (var item in groups)
                {
                    TransactionWise objItem = new TransactionWise();
                    var groupId = Convert.ToInt32(item.GroupId);
                    objItem.GroupId = Convert.ToString(item.GroupId);
                    objItem.GroupName = item.GroupName;

                    string connStr = CR.GetCustomerConnString(Convert.ToString(item.GroupId));
                    using (var contextdb = new BOTSDBContext(connStr))
                    {
                        var fromDate = Convert.ToDateTime("2022-02-28");
                        var tillDate = Convert.ToDateTime("2022-04-01");
                        //objItem.TotalTransactionBase = contextdb.TransactionMasters.Where(x => x.Datetime > fromDate && x.Datetime < tillDate).Count();
                        objItem.TotalInv = contextdb.TransactionMasters.Where(x => x.Datetime > fromDate && x.Datetime < tillDate).Select(y => y.InvoiceAmt).AsQueryable().Sum();
                        //objItem.TotalRedumptionInv = contextdb.TransactionMasters.Where(x => x.Datetime > fromDate && x.Datetime < tillDate && x.TransType == "2").Select(y => y.InvoiceAmt).Sum();
                        //objItem.TotalCustPoints = contextdb.CustomerDetails.Select(x => x.Points).Sum();
                        //Invoice amount
                        //Invoice of Redumption
                        //Customer Points
                    }

                    objData.Add(objItem);
                }

                foreach (var item in objData)
                {
                    //count = count + item.TotalTransactionBase;
                    if (item.TotalInv != null)
                    {
                        TotalInv = TotalInv + item.TotalInv;
                    }
                    else
                    {
                        TotalInv = TotalInv + 0;
                    }
                    //if (item.TotalRedumptionInv != null)
                    //{
                    //    TotalRedumptionInv = TotalRedumptionInv + item.TotalRedumptionInv;
                    //}
                    //else
                    //{
                    //    TotalRedumptionInv = TotalRedumptionInv + 0;
                    //}
                    //TotalRedumptionInv = TotalRedumptionInv + item.TotalRedumptionInv;
                    //TotalCustPoints = TotalCustPoints + item.TotalCustPoints;
                }
            }
            catch (Exception ex)
            {

            }


            return objData;

        }

        public List<GroupWiseDetails> GetGroupWiseData()
        {
            List<GroupWiseDetails> ObjData = new List<GroupWiseDetails>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var groups = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true && x.GroupId != 1051).ToList();
                    foreach (var item in groups)
                    {
                        GroupWiseDetails objItem = new GroupWiseDetails();
                        var groupId = Convert.ToInt32(item.GroupId);
                        objItem.CustId = Convert.ToString(item.GroupId);
                        objItem.CustName = item.GroupName;
                        objItem.BusinessCategory = item.CustomerType;
                        var category = (from c in context.tblGroupDetails
                                        join ct in context.tblCategories on c.RetailCategory equals ct.CategoryId
                                        where c.GroupId == groupId
                                        select new
                                        {
                                            ct.CategoryName
                                        }).FirstOrDefault();
                        objItem.CustCategory = category.CategoryName;

                        var city = (from c in context.tblGroupDetails
                                    join ct in context.tblCities on c.City equals ct.CityId
                                    where c.GroupId == groupId
                                    select new
                                    {
                                        ct.CityName
                                    }).FirstOrDefault();

                        objItem.Location = city.CityName;

                        var CSName = (from c in context.tblGroupDetails
                                      join ct in context.tblRMAssigneds on c.RMAssigned equals ct.RMAssignedId
                                      where c.GroupId == groupId
                                      select new
                                      {
                                          ct.RMAssignedName
                                      }).FirstOrDefault();

                        objItem.CSName = CSName.RMAssignedName;

                        string connStr = CR.GetCustomerConnString(Convert.ToString(item.GroupId));
                        using (var contextdb = new BOTSDBContext(connStr))
                        {
                            objItem.CustCount = contextdb.CustomerDetails.Count();

                            var sqlQ = $"SELECT COUNT(*) as Count FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BulkUploadCustList'";
                            var exist = contextdb.Database.SqlQuery<int>(sqlQ).FirstOrDefault();
                            if (exist > 0)
                            {
                                objItem.BulkUploadCount = contextdb.BulkUploadCustLists.Count();
                                objItem.Total = objItem.CustCount + contextdb.BulkUploadCustLists.Count();
                            }
                            else
                            {
                                objItem.BulkUploadCount = 0;
                                objItem.Total = objItem.CustCount;
                            }
                        }

                        ObjData.Add(objItem);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetGroupWiseData");
            }
            ObjData = ObjData.OrderByDescending(x => x.Total).ToList();
            return ObjData;
        }
        public List<DiscussionDataForGraph> GetDiscussionData()
        {
            List<DiscussionDataForGraph> lstData = new List<DiscussionDataForGraph>();
            using (var context = new CommonDBContext())
            {
                var groups = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true).ToList();
                foreach (var group in groups)
                {
                    DiscussionDataForGraph objItem = new DiscussionDataForGraph();
                    objItem.groupid = group.GroupId;
                    objItem.CustomerType = group.CustomerType;
                    objItem.GroupName = group.GroupName;
                    string grpId = Convert.ToString(group.GroupId);
                    var discussedDate = context.BOTS_TblDiscussion.Where(y => y.GroupId == grpId && (y.CallType == 2 || y.CallType == 3 || y.CallType == 16 || y.CallType == 17)).OrderByDescending(x => x.UpdatedDate).Select(z => z.UpdatedDate).FirstOrDefault();
                    if (discussedDate.HasValue)
                    {
                        var days = (DateTime.Today - discussedDate.Value).Days;
                        if (days > 6)
                        {
                            objItem.days = days;
                            lstData.Add(objItem);
                            objItem.RMAssignedName = context.tblRMAssigneds.Where(x => x.RMAssignedId == group.RMAssigned).Select(y => y.RMAssignedName).FirstOrDefault();
                            objItem.RMLoginId = context.tblRMAssigneds.Where(x => x.RMAssignedId == group.RMAssigned).Select(y => y.LoginId).FirstOrDefault();
                        }
                    }

                }
            }
            return lstData;
        }
        public List<DiscussionDataForGraph> GetDiscussionDataForGraph(string type, string CSMember)
        {
            List<DiscussionDataForGraph> lstData = new List<DiscussionDataForGraph>();

            using (var context = new CommonDBContext())
            {
                var result = context.Database.SqlQuery<DiscussionDataForGraph>("sp_GetUntouchedDiscussionDataForGraph @type, @CSMember",
                              new SqlParameter("@type", type),
                              new SqlParameter("@CSMember", CSMember)).ToList<DiscussionDataForGraph>();

                var ACount = result.Where(x => x.CustomerType == "A").Count();
                var BCount = result.Where(x => x.CustomerType == "B").Count();
                var CCount = result.Where(x => x.CustomerType == "C").Count();

                DiscussionDataForGraph objdataA = new DiscussionDataForGraph();
                objdataA.CustomerType = "A";
                objdataA.count = ACount;
                lstData.Add(objdataA);

                DiscussionDataForGraph objdataB = new DiscussionDataForGraph();
                objdataB.CustomerType = "B";
                objdataB.count = BCount;
                lstData.Add(objdataB);

                DiscussionDataForGraph objdataC = new DiscussionDataForGraph();
                objdataC.CustomerType = "C";
                objdataC.count = CCount;
                lstData.Add(objdataC);
            }

            return lstData;
        }

        public List<DiscussionDataForGraph> GetCSData(string type, string CSMember)
        {
            List<DiscussionDataForGraph> lstData = new List<DiscussionDataForGraph>();

            using (var context = new CommonDBContext())
            {
                lstData = context.Database.SqlQuery<DiscussionDataForGraph>("sp_GetUntouchedDiscussionDataForGraph @type, @CSMember",
                              new SqlParameter("@type", type),
                              new SqlParameter("@CSMember", CSMember)).ToList<DiscussionDataForGraph>();

            }
            return lstData;
        }

        public double GetDays(string groupId)
        {
            double days = 0;
            using (var context = new CommonDBContext())
            {
                var lastUpdatedDate = context.BOTS_TblDiscussion.Where(a => a.GroupId == groupId && (a.CallType == 2 || a.CallType == 3)).OrderByDescending(x => x.UpdatedDate).Select(y => y.UpdatedDate).FirstOrDefault();
                if (lastUpdatedDate.HasValue)
                {
                    days = (DateTime.Today - lastUpdatedDate.Value).Days;
                }
            }

            return days;
        }

        public List<NoCustomerConnect> GetNoCustomerConnect(string CSWise)
        {
            List<NoCustomerConnect> lstData = new List<NoCustomerConnect>();

            using (var context = new CommonDBContext())
            {
                var groups = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true).ToList();
                if (!string.IsNullOrEmpty(CSWise))
                {
                    var csId = context.tblRMAssigneds.Where(x => x.LoginId == CSWise).Select(y => y.RMAssignedId).FirstOrDefault();
                    groups = groups.Where(x => x.RMAssigned.Value == csId).ToList();
                }
                foreach (var group in groups)
                {
                    string grpId = Convert.ToString(group.GroupId);
                    var discussedDate = context.BOTS_TblDiscussion.Where(y => y.GroupId == grpId).OrderByDescending(x => x.UpdatedDate).Select(z => z.UpdatedDate).FirstOrDefault();
                    if (discussedDate.HasValue)
                    {
                        var days = (DateTime.Today - discussedDate.Value).Days;
                        if (days > 30)
                        {
                            NoCustomerConnect objItem = new NoCustomerConnect();
                            objItem.Groupid = group.GroupId;
                            objItem.GroupName = group.GroupName;
                            objItem.CustomerType = group.CustomerType;
                            objItem.LastConnectDate = discussedDate.Value;

                            lstData.Add(objItem);
                        }
                    }
                }

            }
            return lstData;
        }

        public List<MostConnectedCustomers> GetMostConnectedCustomers(string CSWise)
        {
            List<MostConnectedCustomers> lstData = new List<MostConnectedCustomers>();
            using (var context = new CommonDBContext())
            {
                lstData = context.Database.SqlQuery<MostConnectedCustomers>("sp_GetMostConnectedCustomers @pi_Date, @addedBy", new SqlParameter("@pi_Date", DateTime.Today.AddDays(-30)), new SqlParameter("@addedBy", CSWise)).ToList<MostConnectedCustomers>();
                foreach (var item in lstData)
                {
                    int grpid = Convert.ToInt32(item.GroupId);
                    item.GroupName = context.tblGroupDetails.Where(x => x.GroupId == grpid && x.IsActive == true && x.IsLive == true).Select(y => y.GroupName).FirstOrDefault();
                    item.CustomerType = context.tblGroupDetails.Where(x => x.GroupId == grpid && x.IsActive == true && x.IsLive == true).Select(y => y.CustomerType).FirstOrDefault();

                }
            }

            return lstData;
        }
        public List<MostConnectedCustomers> GetLeastConnectedCustomers(string CSWise)
        {
            List<MostConnectedCustomers> lstData = new List<MostConnectedCustomers>();
            using (var context = new CommonDBContext())
            {
                lstData = context.Database.SqlQuery<MostConnectedCustomers>("sp_GetLeastConnectedCustomers @pi_Date,@addedBy", new SqlParameter("@pi_Date", DateTime.Today.AddDays(-180)), new SqlParameter("@addedBy", CSWise)).ToList<MostConnectedCustomers>();
                foreach (var item in lstData)
                {
                    int grpid = Convert.ToInt32(item.GroupId);
                    item.GroupName = context.tblGroupDetails.Where(x => x.GroupId == grpid && x.IsActive == true && x.IsLive == true).Select(y => y.GroupName).FirstOrDefault();
                    item.CustomerType = context.tblGroupDetails.Where(x => x.GroupId == grpid && x.IsActive == true && x.IsLive == true).Select(y => y.CustomerType).FirstOrDefault();

                }
            }

            return lstData;
        }

        public List<RenewalData> GetRenewalData()
        {
            List<RenewalData> objData = new List<RenewalData>();


            try
            {
                using (var context = new CommonDBContext())
                {
                    var groups = context.tblGroupDetails.Where(x => x.IsActive == true && x.IsLive == true).ToList();
                    foreach (var item in groups)
                    {
                        var groupId = Convert.ToString(item.GroupId);

                        string connStr = CR.GetCustomerConnString(groupId);
                        using (var contextNew = new BOTSDBContext(connStr))
                        {
                            try
                            {

                                var OutletList = contextNew.OutletDetails.Where(x => x.GroupId == groupId && !x.OutletName.ToLower().Contains("admin")).ToList();
                                //var outlets = OutletList.Where(x => x.OutletName.Contains("Admin"));
                                //foreach (var outlet in outlets)
                                //{
                                //    OutletList.Remove(outlet);
                                //}

                                foreach (var outlet in OutletList)
                                {
                                    RenewalData objItem = new RenewalData();


                                    objItem.GroupId = Convert.ToString(item.GroupId);
                                    objItem.GroupName = item.GroupName;
                                    objItem.OutletName = outlet.OutletName;
                                    DateTime? ProgramStartDate = contextNew.TransactionMasters.Where(x => x.CounterId.Contains(outlet.OutletId)).OrderBy(y => y.Datetime).Select(z => z.Datetime).FirstOrDefault();

                                    if (ProgramStartDate.HasValue)
                                    {
                                        var ProgramRenewalDate = ProgramStartDate.Value.AddYears(1);
                                        var day = ProgramStartDate.Value.Day;
                                        var month = ProgramStartDate.Value.Month;
                                        var year = ProgramStartDate.Value.Year;
                                        var currentYear = DateTime.Today.Year;



                                        DateTime nextRenewal = new DateTime(currentYear, month, day);
                                        if (nextRenewal < DateTime.Today)
                                        {
                                            ProgramRenewalDate = nextRenewal.AddYears(1);
                                        }
                                        else
                                        {
                                            ProgramRenewalDate = nextRenewal;
                                        }
                                        objItem.RenewalDate = ProgramRenewalDate;



                                    }

                                    objData.Add(objItem);

                                }

                            }

                            catch (Exception ex)
                            {

                            }
                        }

                    }
                    var nullList = objData.Where(x => x.RenewalDate == null).ToList();
                    objData = objData.Where(x => x.RenewalDate != null).ToList();

                    objData = objData.OrderBy(x => x.RenewalDate).ToList();
                    objData.AddRange(nullList);



                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, " GetRenewalData");
            }

            return (objData);
        }

        public List<tblRenewalData> GetRenewalByGroup()
        {
            List<tblRenewalData> lstData = new List<tblRenewalData>();
            try
            {
                using (var context = new CommonDBContext())
                {

                    var FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    var ToDate = new DateTime(DateTime.Today.AddMonths(3).Year, DateTime.Today.AddMonths(3).Month, 30);

                    lstData = context.tblRenewalDatas.Where(x => (x.RenewalDate > FromDate && x.RenewalDate < ToDate) || (x.NextPaymentDate > FromDate && x.NextPaymentDate < ToDate)).OrderByDescending(y => y.RenewalDate).ToList();

                    foreach (var item in lstData)
                    {
                        item.PaymentDateStr = item.PaymentDate.ToString("dd-MMM-yyyy");
                        item.RenewalDateStr = item.RenewalDate.ToString("dd-MMM-yyyy");
                        if (item.IsPartPayment)
                        {
                            item.PartialPayment = "Yes";
                            item.PartialPaymentDateStr = item.NextPaymentDate.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            item.PartialPayment = "No";
                            item.PartialPaymentDateStr = "--";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstData;
        }

        public bool AddPayment(tblRenewalData objPaymentData)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    context.tblRenewalDatas.AddOrUpdate(objPaymentData);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddPayment");
            }
            return status;
        }


        public List<NonTransacting> GetAllNonTransactingData()
        {
            List<NonTransacting> objData = new List<NonTransacting>();
            using (var context = new CommonDBContext())
            {
                objData = context.NonTransacting.OrderByDescending(x=>x.LastTxnDate).ToList();
            }
            return objData;
        }


    }

}