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

    }
}
