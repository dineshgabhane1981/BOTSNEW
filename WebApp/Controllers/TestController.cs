using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            Report obj = new Report();
            obj.DailyEmail();
            return View();
        }

        public class Report
        {

            SqlConnection _Con = new SqlConnection("Data source=13.233.128.61;initial catalog=CommonDBLoyalty;user id=sa; password=BO%Admin#LY!4@;");
            DataTable _dtConnection = new DataTable();
            string _IPAddress, _DBName, _DBPassword, _DBId, _ConnectionString, _GroupId, _BrandId, _GroupName, _ToEmailId, _ServiceFlag;


            public void DailyEmail()
            {
                string EmailId = "report@blueocktopus.in";
                string Password = "Report@123";
                int Port = 587;
                string SMTP = "smtp.zoho.com";
                string strcon = string.Empty;
                string DateOnly = string.Empty;
                string FromDate = string.Empty;
                TimeZoneInfo IND_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
                DateTime date1 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IND_ZONE);
                date = date.AddDays(-1);
                date1 = date1.AddDays(-7);
                DateOnly = date.ToString("yyyy-MM-dd");
                FromDate = date1.ToString("yyyy-MM-dd");
                DataSet dt = new DataSet();
                // strcon = "";
                SqlCommand _Cmd = new SqlCommand();
                _Cmd.Connection = _Con;
                if (_Con.State == ConnectionState.Closed)
                    _Con.Open();
                _Cmd.CommandText = "Select GroupName,IPAddress,DBName,DBPassword,DBId,GroupId,BrandId,EmailId,ServiceFlag from DailySMSWhatsAppBalanceAlerts";
                SqlDataAdapter _daCounterId = new SqlDataAdapter(_Cmd);
                _daCounterId.Fill(_dtConnection);
                //_Con.Close();
                DataSet _ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Brand Name");
                dt1.Columns.Add("Outlet Name");
                dt1.Columns.Add("WA Balance", typeof(int));
                dt1.Columns.Add("WA expiry date");
                dt1.Columns.Add("SMS Balance", typeof(int));
                dt1.Columns.Add("Virtual SMS Balance", typeof(int));
                if (_dtConnection.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtConnection.Rows.Count; i++)
                    {

                        _IPAddress = _dtConnection.Rows[i]["IPAddress"].ToString();
                        _DBName = _dtConnection.Rows[i]["DBName"].ToString();
                        _DBPassword = _dtConnection.Rows[i]["DBPassword"].ToString();
                        _DBId = _dtConnection.Rows[i]["DBId"].ToString();
                        _GroupId = _dtConnection.Rows[i]["GroupId"].ToString();
                        _BrandId = _dtConnection.Rows[i]["BrandId"].ToString();
                        _GroupName = _dtConnection.Rows[i]["GroupName"].ToString();
                        _ToEmailId = _dtConnection.Rows[i]["EmailId"].ToString();
                        _ServiceFlag = _dtConnection.Rows[i]["ServiceFlag"].ToString();
                        _ConnectionString = "Server=" + _IPAddress + ";Initial Catalog=" + _DBName + ";User Id=" + _DBId + ";Password=" + _DBPassword;



                        SqlConnection _Con1 = new SqlConnection(Convert.ToString(_ConnectionString));
                        SqlCommand _Cmd1 = new SqlCommand();
                        _Cmd1.Connection = _Con1;
                        if (_Con1.State == ConnectionState.Closed)
                            _Con1.Open();
                        //  _Cmd1.CommandText = "select  * from SMSDetails where BrandId=" + _BrandId + "";
                        _Cmd1.CommandText = "select SMSDetails.*,OutletDetails.OutletName,BrandDetails.BrandName  from SMSDetails inner join OutletDetails on SMSDetails.OutletId = OutletDetails.OutletId inner join BrandDetails on BrandDetails.BrandId = OutletDetails.BrandId where SMSDetails.BrandId =" + _BrandId + "";
                        DataTable dtsmsdetails = new DataTable();
                        SqlDataAdapter _daCounterId1 = new SqlDataAdapter(_Cmd1);
                        _daCounterId1.Fill(dtsmsdetails);
                        Dictionary<string, dynamic> whatsappbalance;
                        Dictionary<string, dynamic> smsbalance;
                        Dictionary<string, dynamic> Virtualsmsbalance;
                        int whatsAppbaln = -1;
                        string whatsexpdt = "";
                        int Virtualbalance = -1;
                        int balance = -1;
                        for (int j = 0; j < dtsmsdetails.Rows.Count; j++)
                        {
                            if (_ServiceFlag == "1")
                            {
                                string WhatsappTokenId = dtsmsdetails.Rows[j]["WhatsappTokenId"].ToString();


                                if (!string.IsNullOrEmpty(WhatsappTokenId))
                                {
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                                    WebClient client = new WebClient();
                                    client.Headers["Content-type"] = "application/json";
                                    client.Encoding = Encoding.UTF8;
                                    var uri = string.Concat("https://enotify.app/api/checkBal?token=", WhatsappTokenId);
                                    var result = client.DownloadString(uri);
                                    whatsappbalance = (new JavaScriptSerializer()).Deserialize<Dictionary<string, dynamic>>(result);
                                    if (whatsappbalance["status"].Equals("success"))
                                    {
                                        string whatsappexpirydt = whatsappbalance["data"]["expiryDate"];
                                        string whatsappdate = whatsappexpirydt.Substring(0, 19);
                                        whatsexpdt = Convert.ToDateTime(whatsappdate).ToString("yyyy-MM-dd");
                                        whatsAppbaln = Convert.ToInt32(whatsappbalance["data"]["quota"]);
                                    }
                                    else
                                    {
                                        whatsexpdt = "NA";
                                        // whatsAppbaln = "NA";
                                    }

                                }
                                else
                                {
                                    whatsexpdt = "NA";
                                    // whatsAppbaln = "NA";
                                }
                            }
                            else if (_ServiceFlag == "2")
                            {
                                string smsUserNm = dtsmsdetails.Rows[j]["TxnUserName"].ToString();
                                string smsPassword = dtsmsdetails.Rows[j]["TxnPassword"].ToString();
                                if (!string.IsNullOrEmpty(smsUserNm) && !string.IsNullOrEmpty(smsPassword))
                                {

                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                                    WebClient client = new WebClient();
                                    client.Headers["Content-type"] = "application/json";
                                    client.Encoding = Encoding.UTF8;
                                    var uri = string.Concat("https://smsnotify.one/SMSApi/account/readstatus?userid=", smsUserNm, "&password=", smsPassword, "&output=json");
                                    var result = client.DownloadString(uri);
                                    smsbalance = (new JavaScriptSerializer()).Deserialize<Dictionary<string, dynamic>>(result);
                                    if (smsbalance["response"]["status"].Equals("success"))
                                    {
                                        Dictionary<string, dynamic> accountdata = smsbalance["response"]["account"];
                                        balance = Convert.ToInt32(accountdata["smsBalance"]);
                                    }
                                    else
                                    {
                                        //balance = "NA";
                                    }

                                }
                                else
                                {
                                    //  balance = "NA";
                                }
                            }
                            else if (_ServiceFlag == "3")
                            {
                                string VirtualsmsUserNm = dtsmsdetails.Rows[j]["VirtualSmsUserName"].ToString();
                                string VirtualsmsPassword = dtsmsdetails.Rows[j]["VirtualSmsPassword"].ToString();
                                if (!string.IsNullOrEmpty(VirtualsmsUserNm) && !string.IsNullOrEmpty(VirtualsmsPassword))
                                {

                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                                    WebClient client = new WebClient();
                                    client.Headers["Content-type"] = "application/json";
                                    client.Encoding = Encoding.UTF8;
                                    var uri = string.Concat("https://smsnotify.one/SMSApi/account/readstatus?userid=", VirtualsmsUserNm, "&password=", VirtualsmsPassword, "&output=json");
                                    var result = client.DownloadString(uri);
                                    Virtualsmsbalance = (new JavaScriptSerializer()).Deserialize<Dictionary<string, dynamic>>(result);
                                    if (Virtualsmsbalance["response"]["status"].Equals("success"))
                                    {
                                        Dictionary<string, dynamic> accountdata = Virtualsmsbalance["response"]["account"];
                                        Virtualbalance = Convert.ToInt32(accountdata["smsBalance"]);
                                    }


                                }
                                else
                                {
                                    // Virtualbalance = "NA";
                                }
                            }
                            string BrandName = dtsmsdetails.Rows[j]["BrandName"].ToString();
                            string OutletName = dtsmsdetails.Rows[j]["OutletName"].ToString();

                            SqlCommand _Cmd2 = new SqlCommand();
                            _Cmd2.Connection = _Con;
                            if (_Con.State == ConnectionState.Closed)
                                _Con.Open();
                            _Cmd2.CommandText = "Insert into SMSWABalanceData(BrandName,OutletName,WABalance,WAExpiryDate,SMSBalance,VirtualSMSBalance,Date) values(@BrandName1, @OutletName1,@whatsAppbaln,@whatsexpdt, @balance,@Virtualbalance,@Date1)";
                            _Cmd2.Parameters.AddWithValue("BrandName1", BrandName);
                            _Cmd2.Parameters.AddWithValue("OutletName1", OutletName);
                            _Cmd2.Parameters.AddWithValue("whatsAppbaln", whatsAppbaln);
                            _Cmd2.Parameters.AddWithValue("whatsexpdt", whatsexpdt);
                            _Cmd2.Parameters.AddWithValue("balance", balance);
                            _Cmd2.Parameters.AddWithValue("Virtualbalance", Virtualbalance);
                            _Cmd2.Parameters.AddWithValue("Date1", DateTime.Today);
                            _Cmd2.ExecuteNonQuery();

                            DataRow dr = dt1.NewRow();
                            dr[0] = BrandName;
                            dr[1] = OutletName;
                            dr[2] = whatsAppbaln;
                            dr[3] = whatsexpdt;
                            dr[4] = balance;
                            dr[5] = Virtualbalance;
                            dt1.Rows.Add(dr);



                        }
                        _Con1.Close();

                    }
                    _ds1.Tables.Add(dt1);
                    _dtConnection.Clear();
                }

                dt1 = dt1.AsEnumerable()
                  .OrderBy(r => r.Field<int>("WA Balance"))
                   .ThenBy(r => r.Field<int>("SMS Balance"))
                  .CopyToDataTable();

                string Emailheader = string.Empty;
                Emailheader = DateOnly + "DailySMSWhatsAppAlerts";
                StringBuilder str = new StringBuilder();

                for (int j = 0; j < dt1.Rows.Count; j++)
                {

                    if (j == 0)
                    {
                        str.Append("<table>");
                        str.Append("<tr>");
                        str.AppendLine("<td style ='background-color:  #ff0000;'>Please find following Report</td>");
                        str.AppendLine("</br>");
                        str.Append("</tr>");
                        str.Append("</table>");
                        str.Append("<table style='border:2px solid;border-collapse:collapse;'>");
                        str.Append("<tr style='border:2px solid;border-collapse:collapse;'>");
                        foreach (DataColumn column in dt1.Columns)
                        {

                            str.Append("<th style = 'background-color: #E0E6F8;font-weight:bold;border:2px solid;border-collapse:collapse;'>");
                            str.Append(column.ColumnName);
                            str.Append("</th>");

                        }

                        str.Append("</tr>");
                    }

                    str.Append("<tr style='border:2px solid;border-collapse:collapse;'>");
                    str.Append("<td style='border:2px solid;border-collapse:collapse;'align='right'>");
                    str.Append(dt1.Rows[j]["Brand Name"].ToString());
                    str.Append("</td>");
                    str.Append("<td style='border:2px solid;border-collapse:collapse;'align='right'>");
                    str.Append(dt1.Rows[j]["Outlet Name"].ToString());
                    str.Append("</td>");
                    str.Append("<td style='border:2px solid;border-collapse:collapse;'align='right'>");
                    if (Convert.ToInt32(dt1.Rows[j]["WA Balance"]) == -1)
                    {
                        str.Append("NA");
                    }
                    else
                    {
                        str.Append(dt1.Rows[j]["WA Balance"].ToString());
                    }
                    str.Append("</td>");
                    str.Append("<td style='border:2px solid;border-collapse:collapse;'align='right'>");
                    str.Append(dt1.Rows[j]["WA expiry date"].ToString());
                    str.Append("</td>");
                    str.Append("<td style='border:2px solid;border-collapse:collapse;'align='right'>");
                    if (Convert.ToInt32(dt1.Rows[j]["SMS Balance"]) == -1)
                    {
                        str.Append("NA");
                    }
                    else
                    {
                        str.Append(dt1.Rows[j]["SMS Balance"].ToString());
                    }
                    str.Append("</td>");
                    str.Append("<td style='border:2px solid;border-collapse:collapse;'align='right'>");
                    if (Convert.ToInt32(dt1.Rows[j]["Virtual SMS Balance"]) == -1)
                    {
                        str.Append("NA");
                    }
                    else
                    {
                        str.Append(dt1.Rows[j]["Virtual SMS Balance"].ToString());
                    }

                    str.Append("</td>");
                    str.Append("</tr>");


                }
                str.Append("</table>");


                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress(EmailId);
                _ToEmailId = Convert.ToString(ConfigurationManager.AppSettings["Toemail"]);
                Msg.To.Add(_ToEmailId);
                Msg.Subject = Emailheader;
                Msg.Body = str.ToString();
                Msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(SMTP);
                smtp.EnableSsl = true;
                smtp.Port = Port;
                smtp.Credentials = new System.Net.NetworkCredential(EmailId, Password);
                smtp.Send(Msg);
                Msg.Dispose();



            }//end for loop


        }

    }
}