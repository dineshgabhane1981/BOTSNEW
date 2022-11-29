using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;

namespace BOTS_BL.Repository
{
    public class VenusRepository
    {
        
        Exceptions newexception = new Exceptions();
       


        public bool SaveCompetitionData(string StudentName, string DOB, string SchoolName, string ClassStandard, string ParentName, string WhatsAppNo, string EmailId, string HomeAddress)
        {
            CompetitionDetail data = new CompetitionDetail();
            List<CompetitionDetail> Data = new List<CompetitionDetail>();
            bool status;
            status = false;
            
            SqlConnection _Con = new SqlConnection(Convert.ToString(ConfigurationManager.ConnectionStrings["BOTSDBContext"]));
            DataSet retVal = new DataSet();
            DataTable Tbl = new DataTable();
            SqlCommand cmdReport = new SqlCommand("sp_SaveCompetition", _Con);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

            using (cmdReport)
            {
                SqlParameter param1 = new SqlParameter("pi_StudentName", StudentName);
                SqlParameter param2 = new SqlParameter("pi_DOB", DOB);
                SqlParameter param3 = new SqlParameter("pi_SchoolName", SchoolName);
                SqlParameter param4 = new SqlParameter("pi_ClassStandard", ClassStandard);
                SqlParameter param5 = new SqlParameter("pi_ParentName", ParentName);
                SqlParameter param6 = new SqlParameter("pi_WhatsAppNo", WhatsAppNo);
                SqlParameter param7 = new SqlParameter("pi_EmailId", EmailId);
                SqlParameter param8 = new SqlParameter("pi_HomeAddress", HomeAddress);
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.Parameters.Add(param1);
                cmdReport.Parameters.Add(param2);
                cmdReport.Parameters.Add(param3);
                cmdReport.Parameters.Add(param4);
                cmdReport.Parameters.Add(param5);
                cmdReport.Parameters.Add(param6);
                cmdReport.Parameters.Add(param7);
                cmdReport.Parameters.Add(param8);

                daReport.Fill(retVal);

                Tbl = retVal.Tables[0];

                daReport.Fill(Tbl);
                if (Tbl.Rows[0]["ResponseCode"].ToString() == "0")
                {
                    status = true;
                    //string _MobileNo;
                    //_MobileNo = "91" + WhatsAppNo;
                    string _Message;

                    _Message = "Dear #01, Your registration for the Handwriting Competition is done successfully. See you soon! Thanks & Regards, Venus";

                    Thread _job = new Thread(() => SendWhatsText(StudentName, WhatsAppNo, _Message));
                    _job.Start();

                }



            }


            return status;

            
        }

        public void SendWhatsText(string _StudentName, string _MobileNo, string _Message)
        {
            string responseString;
            try
            {
                _Message = _Message.Replace("#99", "&");
                _Message = _Message.Replace("#01", _StudentName);
                _Message = HttpUtility.UrlEncode(_Message);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                string _Url = "https://bo.enotify.app/api/sendText?";
                sbposdata.AppendFormat(_Url);
                sbposdata.AppendFormat("token={0}", "5fc8ed623629423c01ce4221");
                sbposdata.AppendFormat("&phone=91{0}", _MobileNo);
                //sbposdata.AppendFormat("&link={0}", _ImageUrl);
                sbposdata.AppendFormat("&message={0}", _Message);
                //sbposdata.AppendFormat("&text={0}", _MobileMessage);
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
                //Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //_job.Start();
                responseString = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                //Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //_job.Start();
                responseString = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                //Thread _job = new Thread(() => SendSMSMessageTxn(_MobileNo, _MobileMessage, _UserName, _Password, _Sender, _Url));
                //_job.Start();
                responseString = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
        }
    }
}
