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
using System.Data.Entity.Migrations;
using System.Data;

namespace BOTS_BL.Repository
{
    public class TelecallerRepository
    {
        Exceptions newexception = new Exceptions();

        public TelecallerCustomerData GetTelecallerCustomer(string MobileNo, string GroupId, string connstr)
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            try
            {
                using (var contextNew = new CommonDBContext())
                {
                    contextNew.Database.CommandTimeout = 300;
                    var DBName = contextNew.tblDatabaseDetails.Where(x => x.GroupId == GroupId).Select(y => y.DBName).FirstOrDefault();
                    objteledata = contextNew.Database.SqlQuery<TelecallerCustomerData>("sp_TelecallerData @pi_GroupId, @pi_Date, @pi_MobileNo, @pi_DBName",
                          new SqlParameter("@pi_GroupId", MobileNo),
                          new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                          new SqlParameter("@pi_MobileNo", MobileNo),
                          new SqlParameter("@pi_DBName", DBName)).FirstOrDefault<TelecallerCustomerData>();
                }
                //using (var context = new BOTSDBContext(connstr))
                //{
                //    objteledata = context.Database.SqlQuery<TelecallerCustomerData>("SP_BOTS_TelecallerData @pi_mobileNo",
                //           new SqlParameter("@pi_mobileNo", MobileNo)).FirstOrDefault<TelecallerCustomerData>();
                //}
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTelecallerCustomer");
            }
            return objteledata;

        }
        public bool SaveTelecallerTracking(string connstr, string AddedBy, string MobileNo, string CustomerNm, string Gender, DateTime? DateofBirth, DateTime? DateOfAnni, int PointsGiven, string OutletId, string Comments, string PointsValidity, string RedeemingOutlet, string GroupId)
        {
            bool status = false;
            bool updatetable = false;
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    var objcustomer = context.tblCustDetailsMasters.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    TelecallerTracking objtracking = new TelecallerTracking();
                    if (objcustomer != null)
                    {
                        if (objcustomer.Name != CustomerNm)
                        {
                            if (CustomerNm != null)
                            {
                                updatetable = true;
                                objcustomer.Name = CustomerNm;
                            }
                        }
                        if (objcustomer.Gender != Gender)
                        {
                            if (Gender != null)
                            {
                                updatetable = true;
                                objcustomer.Gender = Gender;
                            }
                        }
                        if (objcustomer.DOB != DateofBirth)
                        {
                            if (DateofBirth != null)
                            {
                                updatetable = true;
                                objcustomer.DOB = DateofBirth;
                            }
                        }
                        if (objcustomer.AnniversaryDate != DateOfAnni)
                        {
                            if (DateOfAnni != null)
                            {
                                updatetable = true;
                                objcustomer.AnniversaryDate = DateOfAnni;
                            }
                        }
                        if (Comments != null)
                        {
                            updatetable = true;
                        }
                        if (updatetable)
                        {
                            context.tblCustDetailsMasters.AddOrUpdate(objcustomer);
                            context.SaveChanges();

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

                            // SMSDetail objsMSDetail = new SMSDetail(); 
                            // string Message = "";
                            // SendWhatsTextOnly(MobileNo, Message, objsMSDetail.WhatsAppTokenId);
                            status = true;
                        }
                        if (GroupId == "1263")
                        {
                            tblCustPointsMaster objCustPoint = new tblCustPointsMaster();
                            objCustPoint.MobileNo = MobileNo;
                            if (!string.IsNullOrEmpty(Convert.ToString(PointsGiven)))
                                objCustPoint.Points = Convert.ToDecimal(PointsGiven);
                            else
                                objCustPoint.Points = 0;
                            objCustPoint.PointsType = "Bonus";
                            objCustPoint.PointsDesc = "Telecaller";
                            objCustPoint.StartDate = DateTime.Now;
                            objCustPoint.EndDate = DateTime.Now.AddDays(Convert.ToInt32(PointsValidity));
                            objCustPoint.IsActive = true;
                            objCustPoint.MinInvoiceAmtRequired = 0;
                            objCustPoint.MobileNoPtsId = MobileNo + "Telecaller";
                            context.tblCustPointsMasters.Add(objCustPoint);
                            context.SaveChanges();


                            tblTxnBonusMaster objBonusMaster = new tblTxnBonusMaster();
                            objBonusMaster.MobileNo = MobileNo;
                            objBonusMaster.CounterId = OutletId + "01";
                            objBonusMaster.OutletId = OutletId;
                            objBonusMaster.TxnType = "Telecaller";
                            objBonusMaster.TxnDatetime = DateTime.Now;
                            objBonusMaster.TxnReceivedDatetime = DateTime.Now;
                            objBonusMaster.InvoiceNo = "Telecaller";
                            objBonusMaster.InvoiceAmt = 0;
                            objBonusMaster.IsActive = true;
                            objBonusMaster.PointsEarned = Convert.ToDecimal(PointsGiven);
                            objBonusMaster.PointsBurned = 0;
                            objBonusMaster.CampaignPoints = 0;
                            objBonusMaster.OriginalInvAmt = 0;
                            objBonusMaster.CustBalancePts = 0;
                            objBonusMaster.TxnBy = AddedBy;
                            objBonusMaster.BonusIdBackEnd = MobileNo + "Telecaller";
                            context.tblTxnBonusMasters.Add(objBonusMaster);
                            context.SaveChanges();
                            status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveTelecallerTracking");
            }

            return status;
        }
        public List<TelecallerReport> GetTelecallerReportData(DateTime fromdt, DateTime todate, string connstr, string GroupId)
        {
            List<TelecallerReport> lsttelereportdata = new List<TelecallerReport>();
            todate = todate.AddDays(1).Date.AddSeconds(-1);
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lsttelereportdata = (from t in context.TelecallerTrackings
                                         join o in context.tblOutletMasters on t.OutletId equals o.OutletId
                                         where t.AddedDate > fromdt && t.AddedDate < todate && t.MobileNo !=""
                                         select new TelecallerReport
                                         {
                                             MobileNo = t.MobileNo,
                                             CustomerName = t.CustomerName,
                                             Gender = t.Gender,
                                             DOB = t.DOB,
                                             DOA = t.DOA,
                                             Points = t.Points,
                                             OutletId = t.OutletId,
                                             OutletName = o.OutletName,
                                             IsSMSSend = t.IsSMSSend,
                                             Comments = t.Comments,
                                             AddedBy = t.AddedBy,
                                             AddedDate = t.AddedDate
                                         }).ToList();

                }
                using (var context = new CommonDBContext())
                {
                    lsttelereportdata = (from t in lsttelereportdata
                                         join c in context.CustomerLoginDetails
                                            on t.AddedBy equals c.LoginId
                                         select new TelecallerReport
                                         {
                                             MobileNo = t.MobileNo,
                                             CustomerName = t.CustomerName,
                                             Gender = t.Gender,
                                             DOB = t.DOB,
                                             DOA = t.DOA,
                                             Points = t.Points,
                                             OutletId = t.OutletId,
                                             OutletName = t.OutletName,
                                             IsSMSSend = t.IsSMSSend,
                                             Comments = t.Comments,
                                             AddedBy = c.UserName,
                                             AddedDate = t.AddedDate

                                         }).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetTelecallerReportData");
            }
            lsttelereportdata = lsttelereportdata.OrderBy(x => x.AddedDate).ToList();
            foreach (var item in lsttelereportdata)
            {
                item.AddedDate = item.AddedDate.Value.AddHours(5).AddMinutes(30);
            }
            return lsttelereportdata;

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

        public bool SaveEnroll(string connstr, string LoginId, string OutletId, string MobileNo, string CustName, string CrdNo, string Gender, string DOB, string DOA, string Comment, string BonusPoints, string PointsValidity, string RedeemingOutlet, string GroupId)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    tblCustDetailsMaster objCustDetails = new tblCustDetailsMaster();
                    objCustDetails.MobileNo = MobileNo;
                    objCustDetails.Name = CustName;
                    objCustDetails.Id = GroupId + MobileNo;
                    if (!string.IsNullOrEmpty(DOB))
                        objCustDetails.DOB = Convert.ToDateTime(DOB);
                    if (!string.IsNullOrEmpty(DOA))
                        objCustDetails.AnniversaryDate = Convert.ToDateTime(DOA);
                    if (!string.IsNullOrEmpty(CrdNo))
                        objCustDetails.CardNo = CrdNo;
                    if (!string.IsNullOrEmpty(Gender))
                        objCustDetails.Gender = Gender;
                    if (!string.IsNullOrEmpty(OutletId))
                        objCustDetails.EnrolledOutlet = OutletId;

                    objCustDetails.DOJ = DateTime.Now;
                    objCustDetails.IsActive = true;
                    objCustDetails.DisableTxn = false;
                    objCustDetails.DisableSMSWAPromo = false;
                    objCustDetails.EnrolledBy = LoginId;
                    objCustDetails.CurrentEnrolledOutlet = OutletId;
                    objCustDetails.DisableSMSWATxn = false;
                    context.tblCustDetailsMasters.Add(objCustDetails);
                    context.SaveChanges();

                    tblCustInfo objCustInfo = new tblCustInfo();
                    objCustInfo.MobileNo = MobileNo;
                    objCustInfo.Name = CustName;
                    context.tblCustInfoes.Add(objCustInfo);
                    context.SaveChanges();

                    if (GroupId == "1263")
                    {
                        tblCustPointsMaster objCustPoint = new tblCustPointsMaster();
                        objCustPoint.MobileNo = MobileNo;
                        if (!string.IsNullOrEmpty(BonusPoints))
                            objCustPoint.Points = Convert.ToDecimal(BonusPoints);
                        else
                            objCustPoint.Points = 0;
                        objCustPoint.PointsType = "Bonus";
                        objCustPoint.PointsDesc = "Telecaller";
                        objCustPoint.StartDate = DateTime.Now;
                        objCustPoint.EndDate = DateTime.Now.AddDays(Convert.ToInt32(PointsValidity));
                        objCustPoint.IsActive = true;
                        objCustPoint.MinInvoiceAmtRequired = 0;
                        objCustPoint.MobileNoPtsId = MobileNo + "Telecaller";
                        context.tblCustPointsMasters.Add(objCustPoint);
                        context.SaveChanges();
                    }

                    TelecallerTracking objTracking = new TelecallerTracking();
                    objTracking.MobileNo = MobileNo;
                    objTracking.CustomerName = CustName;
                    objTracking.Gender = Gender;
                    if (!string.IsNullOrEmpty(DOB))
                        objTracking.DOB = Convert.ToDateTime(DOB);
                    if (!string.IsNullOrEmpty(DOA))
                        objTracking.DOA = Convert.ToDateTime(DOA);
                    if (!string.IsNullOrEmpty(BonusPoints))
                        objTracking.Points = Convert.ToDecimal(BonusPoints);
                    objTracking.IsSMSSend = false;
                    objTracking.Comments = Comment;
                    objTracking.AddedBy = LoginId;
                    objTracking.AddedDate = DateTime.Now;
                    objTracking.OutletId = OutletId;
                    context.TelecallerTrackings.Add(objTracking);
                    context.SaveChanges();

                    if (GroupId == "1263")
                    {
                        tblTxnBonusMaster objBonusMaster = new tblTxnBonusMaster();
                        objBonusMaster.MobileNo = MobileNo;
                        objBonusMaster.CounterId = OutletId + "01";
                        objBonusMaster.OutletId = OutletId;
                        objBonusMaster.TxnType = "Telecaller";
                        objBonusMaster.TxnDatetime = DateTime.Now;
                        objBonusMaster.TxnReceivedDatetime = DateTime.Now;
                        objBonusMaster.InvoiceNo = "Telecaller";
                        objBonusMaster.InvoiceAmt = 0;
                        objBonusMaster.IsActive = true;
                        objBonusMaster.PointsEarned = Convert.ToDecimal(BonusPoints);
                        objBonusMaster.PointsBurned = 0;
                        objBonusMaster.CampaignPoints = 0;
                        objBonusMaster.OriginalInvAmt = 0;
                        objBonusMaster.CustBalancePts = 0;
                        objBonusMaster.TxnBy = LoginId;
                        objBonusMaster.BonusIdBackEnd = MobileNo + "Telecaller";
                        context.tblTxnBonusMasters.Add(objBonusMaster);
                        context.SaveChanges();
                    }
                    status = true;


                    //ObjJSON = context.Database.SqlQuery<JsonData>(" sp_TeleCalEnroll @pi_CounterId, @pi_MobileNo,@pi_DOB,@pi_CustomerName,@pi_CardNo,@pi_Gender,@pi_Anniversary,@pi_Datetime,@pi_AddBy,@pi_Comment",
                    //new SqlParameter("@pi_CounterId", CounterId), new SqlParameter("@pi_MobileNo", MobileNo), new SqlParameter("@pi_DOB", DOB), new SqlParameter("@pi_CustomerName", CustName), new SqlParameter("@pi_CardNo", CrdNo), new SqlParameter("@pi_Gender", Gender), new SqlParameter("@pi_Anniversary", DOA), new SqlParameter("@pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), new SqlParameter("@pi_AddBy", LoginId), new SqlParameter("@pi_Comment", Comment)).ToList<JsonData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveEnroll");
            }

            return status;
        }
    }
}
