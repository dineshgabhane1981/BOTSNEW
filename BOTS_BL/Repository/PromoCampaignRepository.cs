using BOTS_BL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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

       public WAInsData GetBOWABalance()
        {
            WAInsData WAInsDetails = new WAInsData();
            string Tokenid = "5fc8ed623629423c01ce4221";


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
    }
}
