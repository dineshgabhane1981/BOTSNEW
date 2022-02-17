using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BOTS_BL.Repository
{
   public class TelecallerRepository
    {
        Exceptions newexception = new Exceptions();

        public TelecallerCustomerData GetTelecallerCustomer(string MobileNo,string GroupId,string connstr)
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objteledata = context.Database.SqlQuery<TelecallerCustomerData>("SP_BOTS_TelecallerData @pi_mobileNo",
                           new SqlParameter("@pi_mobileNo", MobileNo)).FirstOrDefault<TelecallerCustomerData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objteledata;

        }
        public bool SaveTelecallerTracking(string connstr,string AddedBy,string MobileNo,string CustomerNm,string Gender,DateTime DateofBirth,DateTime DateOfAnni,int PointsGiven,string OutletId,string Comments)
        {
            bool status = false;
            bool updatetable = false;
            bool updatecustnm = false;
            bool updategender = false;
            bool updatedob = false;
            bool updatedoa = false;
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    CustomerDetail objcustomer = context.CustomerDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    TelecallerTracking objtracking = new TelecallerTracking();
                    if (objcustomer != null)
                    {
                        if (objcustomer.CustomerName == CustomerNm)
                        {
                            updatetable = false;
                        }
                        else
                        {
                            if (CustomerNm != null)
                            {
                                updatetable = true;
                                updatecustnm = true;
                            }
                        }
                        if (objcustomer.Gender == Gender)
                        {
                            updatetable = false;
                        }
                        else
                        {
                            if(Gender != null)
                            {
                                updatetable = true;
                            }
                        }
                        if(objcustomer.DOB == DateofBirth)
                        {
                            updatetable = false;
                        }
                        else
                        {
                            if (DateofBirth != null)
                            {
                                updatetable = true;
                            }
                        }
                        if (objcustomer.AnniversaryDate == DateOfAnni)
                        {
                            updatetable = false;
                        }
                        else
                        {
                            if (DateOfAnni != null)
                            {
                                updatetable = true;
                            }
                        }
                        if (updatetable)
                        {
                            if(updatecustnm)
                            {
                                
                               
                            }
                            objtracking.AddedBy = AddedBy;
                            objtracking.AddedDate = DateTime.Now;
                            objtracking.Comments = Comments;
                            objtracking.CustomerName = CustomerNm;
                            objtracking.DOA = DateOfAnni;
                            objtracking.DOB = DateofBirth;
                            objtracking.Gender = Gender;
                            objtracking.IsSMSSend = false;
                            objtracking.MobileNo = MobileNo;
                            objtracking.OutletId = OutletId;
                            objtracking.Points = PointsGiven;
                            context.TelecallerTrackings.Add(objtracking);
                            context.SaveChanges();

                            SMSDetail objsMSDetail = new SMSDetail(); 
                            string Message = "";
                            SendWhatsTextOnly(MobileNo, Message, objsMSDetail.WhatsAppTokenId);
                            status = true;
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }

            return status;
        }

        public void SendWhatsTextOnly(string _MobileNo, string _Message, string _Token)
        {
            string responseString;
            try
            {
                _Message = _Message.Replace("#99", "&");
                _Message = HttpUtility.UrlEncode(_Message);
                //string type = "TEXT";
                StringBuilder sbposdata = new StringBuilder();
                sbposdata.AppendFormat("https://enotify.app/api/sendText?");
                sbposdata.AppendFormat("token={0}", _Token);
                sbposdata.AppendFormat("&phone=91{0}", _MobileNo);
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
    }
}
