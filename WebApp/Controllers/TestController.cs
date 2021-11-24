using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            report obj = new report();
            obj.DailyEmailnew();
            return View();
        }
        public class report
        {
            SqlConnection _Con = new SqlConnection("Data source=13.233.128.61;initial catalog=CommonDBLoyalty;user id=sa; password=BO%Admin#LY!4@;");
            DataTable _dtConnection = new DataTable();
            string _IPAddress, _DBName, _DBPassword, _DBId, _ConnectionString, _GroupId, _BrandId, _GroupName, _ToEmailId;



            public StreamWriter DumpTxn(StreamWriter sw, string date, DataSet ds, string _groupid)
            {
                StringBuilder str = new StringBuilder();
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    sw.Write(column.ColumnName);
                    sw.Write(",");
                }

                sw.Write(sw.NewLine);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < ds.Tables[0].Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Write(sw.NewLine);
                sw.Close();
                return sw;
            }


            public void DailyEmailnew()
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

                _Con.Close();


                _IPAddress = "13.233.128.61";
                _DBName = "GoldMart";
                _DBPassword = "BO%Admin#LY!4@";
                _DBId = "sa";
                _GroupId = "1013";
                _BrandId = "10131";
                _GroupName = "Gold Mart Jewels Pvt Ltd ";
                _ToEmailId = "ashlesha85@gmail.com,ashlesha@blueocktopus.in";
                _ConnectionString = "Server=" + _IPAddress + ";Initial Catalog=" + _DBName + ";User Id=" + _DBId + ";Password=" + _DBPassword;


                DataSet _ds1 = new DataSet();
                SqlConnection _Con1 = new SqlConnection(Convert.ToString(_ConnectionString));
                SqlCommand _Cmd1 = new SqlCommand();
                _Cmd1.Connection = _Con1;
                if (_Con1.State == ConnectionState.Closed)
                    _Con1.Open();
                _Cmd1.CommandType = CommandType.StoredProcedure;
                _Cmd1.CommandTimeout = 380000;
                _Cmd1.CommandText = "sp_FeedbackReport";
                //_Cmd1.Parameters.AddWithValue("@pi_BrandId", _BrandId);

                //_Cmd1.Parameters.AddWithValue("@pi_FromDate", fromdt);
                //_Cmd1.Parameters.AddWithValue("@pi_Todate", todt);

                SqlDataAdapter _daCounterId1 = new SqlDataAdapter(_Cmd1);
                _daCounterId1.Fill(_ds1);
                _Con1.Close();


                //  string perioddt = fromdt + " - " + todt;
                string fileLocation_D = string.Empty;
                fileLocation_D = "C:\\File\\_WeeklyFeedBackReport.csv";
                StreamWriter sw_D = new StreamWriter(fileLocation_D, false);
                DumpTxn(sw_D, DateOnly, _ds1, _GroupId);


                string Emailheader = string.Empty;
                Emailheader = _GroupName + DateOnly + "WeeklyFeedBackReport";
                StringBuilder str = new StringBuilder();
                str.Append("<table>");
                str.Append("<tr>");

                str.AppendLine("<td>Dear Customer,</td>");
                str.AppendLine("</br>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");

                str.AppendLine("<td>Please find the detailed report attached.</td>");
                str.AppendLine("</br>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.AppendLine("<td>If you have any questions on this report, please do not reply to this email, as this email report is being sent from is an unmonitored email alias. Instead, write to info@blueocktopus.in or call us for information / clarification.</td>");
                str.AppendLine("</br>");
                str.AppendLine("</br>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.AppendLine("<td>Regards,</td>");
                str.AppendLine("</br>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.AppendLine("<td>Blue Ocktopus team</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.AppendLine("<td>info@blueocktopus.in</td>");
                str.AppendLine("</br>");
                str.AppendLine("</br>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td>&nbsp;</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.AppendLine("<td>Disclaimer: The information/contents of this e-mail message and any attachments are confidential and are intended solely for the addressee. Any review, re-transmission, dissemination or other use of, or taking of any action in reliance upon, this information by persons or entities other than the intended recipient is prohibited. If you have received this transmission in error, please immediately notify the sender by return e-mail and delete this message and its attachments. Any unauthorized use, copying or dissemination of this transmission is prohibited. Neither the confidentiality nor the integrity of this message can be vouched for following transmission on the internet.</td>");
                str.Append("</tr>");
                str.Append("</table>");

                MailMessage Msg = new MailMessage();
                Msg.From = new MailAddress(EmailId);
                Msg.To.Add(_ToEmailId);
                Msg.Subject = Emailheader;
                Msg.Body = str.ToString();

                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(fileLocation_D);
                Msg.Attachments.Add(attachment);


                Msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(SMTP);

                smtp.EnableSsl = true;
                smtp.Port = Port;
                smtp.Credentials = new System.Net.NetworkCredential(EmailId, Password);
                smtp.Send(Msg);
                Msg.Dispose();
                System.IO.File.Delete(fileLocation_D);





            }//end for loop
        }
    }
}