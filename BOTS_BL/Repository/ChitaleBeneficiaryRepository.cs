
using BOTS_BL.Models.ChitaleWebAppModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
   public class ChitaleBeneficiaryRepository
    {
        public CustomerDetails GetMemberDetails(string MobileNo)
        {
            CustomerDetails objcustomerDetails = new CustomerDetails();
            CustomerBeneficiaryDetail objcustomerBeneficiaryDetail = new CustomerBeneficiaryDetail();
            try
            {
                using (var contextNew = new ChitaleBCContext())
                {
                    objcustomerDetails = contextNew.CustomerDetails.Where(x => x.MobileNo == MobileNo ).FirstOrDefault();
                    //objcustomerBeneficiaryDetail = contextNew.CustomerBeneficiaryDetails.Where(x => x.MobileNo == MobileNo).FirstOrDefault();

                    //if(objcustomerBeneficiaryDetail != null)
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                //newexception.AddException(ex);
            }
            return objcustomerDetails;
        }

        public int AddpointsToExistingCust(string mobileNo, string BeneficiaryId,string Name)
        {
            int result = 0;
            CustomerDetails objcustomerDetails = new CustomerDetails();
            List<TransactionMaster> lsttransaction = new List<TransactionMaster>();
            TransactionMaster objtransactionMaster = new TransactionMaster();
            PointsExpiry objpointsExpiry = new PointsExpiry();
            
            List<PointsExpiry> lstpointsExpiry = new List<PointsExpiry>();
            try
            {
                using (var contextNew = new ChitaleBCContext())
                {
                    CustomerBeneficiaryDetail objcustomerBeneficiaryDetail = new CustomerBeneficiaryDetail();
                    objcustomerBeneficiaryDetail = contextNew.CustomerBeneficiaryDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    objcustomerDetails = contextNew.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                    if (objcustomerBeneficiaryDetail != null)
                    {
                        result = 0;
                    }
                    else
                    {
                        objcustomerBeneficiaryDetail = contextNew.CustomerBeneficiaryDetails.Where(x => x.BeneficiaryId == BeneficiaryId).FirstOrDefault();
                        if (objcustomerBeneficiaryDetail != null)
                        {
                            result = 1;
                        }
                        else
                        {
                            if (objcustomerDetails != null)
                            {
                                var point = objcustomerDetails.Points;
                                objcustomerDetails.Points = point + 100;
                                contextNew.CustomerDetails.AddOrUpdate(objcustomerDetails);
                                contextNew.SaveChanges();

                                objtransactionMaster.CustomerId = objcustomerDetails.CustomerId;
                                objtransactionMaster.CounterId = "1088101001";
                                objtransactionMaster.MobileNo = mobileNo;
                                objtransactionMaster.Datetime = DateTime.Now;
                                objtransactionMaster.TransType = "1";
                                objtransactionMaster.TransSource = "1";
                                objtransactionMaster.InvoiceNo = "B_CovidPoints";
                                objtransactionMaster.InvoiceAmt = 0;
                                objtransactionMaster.Status = "06";
                                objtransactionMaster.PointsEarned = 100;
                                objtransactionMaster.PointsBurned = 0;
                                objtransactionMaster.CampaignPoints = 0;
                                objtransactionMaster.TxnAmt = 0;
                                objtransactionMaster.CustomerPoints = objcustomerDetails.Points;
                                objtransactionMaster.Synchronization = "";
                                objtransactionMaster.SyncDatetime = null;
                                objtransactionMaster.BillType = "";
                                objtransactionMaster.ChitaleTxnStatus = "";

                                contextNew.TransactionMasters.Add(objtransactionMaster);
                                contextNew.SaveChanges();

                                lstpointsExpiry = contextNew.PointsExpiries.Where(x => x.MobileNo == mobileNo).ToList();
                                objpointsExpiry.MobileNo = mobileNo;
                                objpointsExpiry.CounterId = "1088101001";
                                objpointsExpiry.CustomerId = objcustomerDetails.CustomerId;
                                objpointsExpiry.BurnDate = null;
                                objpointsExpiry.Datetime = DateTime.Now;
                                objpointsExpiry.EarnDate = DateTime.Now;
                                DateTime today = DateTime.Today;
                                DateTime next = today.AddYears(1);
                                var currentmonth = DateTime.DaysInMonth(next.Year, next.Month);

                                if (next.Day < currentmonth)
                                {
                                    var days = (currentmonth - next.Day);
                                    next = today.AddDays(days).AddYears(1);
                                }
                                objpointsExpiry.ExpiryDate = next;
                                objpointsExpiry.Points = 100;
                                objpointsExpiry.Status = "00";
                                objpointsExpiry.InvoiceNo = "B_CovidPoints";
                                objpointsExpiry.GroupId = "1088";
                                objpointsExpiry.OriginalInvoiceNo = "";
                                objpointsExpiry.TransRefNo = null;

                                contextNew.PointsExpiries.Add(objpointsExpiry);
                                contextNew.SaveChanges();
                                CustomerBeneficiaryDetail objcustomerBeneficiaryDetail1 = new CustomerBeneficiaryDetail();

                                objcustomerBeneficiaryDetail1.BeneficiaryId = BeneficiaryId;
                                objcustomerBeneficiaryDetail1.CustomerId = objcustomerDetails.CustomerId;
                                objcustomerBeneficiaryDetail1.CustomerName = Name;
                                objcustomerBeneficiaryDetail1.MobileNo = mobileNo;
                                objcustomerBeneficiaryDetail1.Points = Convert.ToDecimal(objcustomerDetails.Points);
                                objcustomerBeneficiaryDetail1.EnrolledDate = DateTime.Now;

                                contextNew.CustomerBeneficiaryDetails.Add(objcustomerBeneficiaryDetail1);
                                contextNew.SaveChanges();
                                result = 2;
                            }
                            else
                            {
                                CustomerDetails objcustomerDetail = new CustomerDetails();
                                CustomerBeneficiaryDetail objcustomerBeneficiaryDetails = new CustomerBeneficiaryDetail();
                                var ObjMobileNo = contextNew.CustomerDetails.Where(x => x.MobileNo == mobileNo).FirstOrDefault();
                                if (ObjMobileNo == null)
                                {
                                    var CustomerId = contextNew.CustomerDetails.OrderByDescending(x => x.CustomerId).Select(y => y.CustomerId).FirstOrDefault();
                                    DateTime datet = new DateTime(1900,01,01);
                                    var NewId = Convert.ToInt64(CustomerId) + 1;
                                    objcustomerDetail.CustomerId = Convert.ToString(NewId);
                                    objcustomerDetail.Points = 100;
                                    objcustomerDetail.CustomerName = Name;
                                    objcustomerDetail.CustomerCategory = null;
                                    objcustomerDetail.CardNumber = "";
                                    objcustomerDetail.CustomerThrough = "2";
                                    objcustomerDetail.DOB = datet;
                                    objcustomerDetail.MaritalStatus = "";
                                    objcustomerDetail.MemberGroupId = "1000";
                                    objcustomerDetail.MobileNo = mobileNo;
                                    objcustomerDetail.Status = "00";
                                    objcustomerDetail.AnniversaryDate = datet;
                                    objcustomerDetail.DOJ = DateTime.Now;
                                    objcustomerDetail.EmailId = "";
                                    objcustomerDetail.EnrollingOutlet = "10881010";
                                    objcustomerDetail.Gender = "";
                                    objcustomerDetail.IsSMS = null;
                                    objcustomerDetail.BillingCustomerId = null;


                                    //objcustomerDetails.
                                    contextNew.CustomerDetails.Add(objcustomerDetail);
                                    contextNew.SaveChanges();

                                    objtransactionMaster.CustomerId = objcustomerDetail.CustomerId;
                                    objtransactionMaster.CounterId = "1088101001";
                                    objtransactionMaster.MobileNo = mobileNo;
                                    objtransactionMaster.Datetime = DateTime.Now;
                                    objtransactionMaster.TransType = "1";
                                    objtransactionMaster.TransSource = "1";
                                    objtransactionMaster.InvoiceNo = "B_CovidPoints";
                                    objtransactionMaster.InvoiceAmt = 0;
                                    objtransactionMaster.Status = "06";
                                    objtransactionMaster.PointsEarned = 100;
                                    objtransactionMaster.PointsBurned = 0;
                                    objtransactionMaster.CampaignPoints = 0;
                                    objtransactionMaster.TxnAmt = 0;
                                    objtransactionMaster.CustomerPoints = objcustomerDetail.Points;
                                    objtransactionMaster.Synchronization = "";
                                    objtransactionMaster.SyncDatetime = null;
                                    objtransactionMaster.BillType = "";
                                    objtransactionMaster.ChitaleTxnStatus = "";

                                    contextNew.TransactionMasters.AddOrUpdate(objtransactionMaster);
                                    contextNew.SaveChanges();

                                    lstpointsExpiry = contextNew.PointsExpiries.Where(x => x.MobileNo == mobileNo).ToList();
                                    objpointsExpiry.MobileNo = mobileNo;
                                    objpointsExpiry.CounterId = "1088101001";
                                    objpointsExpiry.CustomerId = objcustomerDetail.CustomerId;
                                    objpointsExpiry.BurnDate = null;
                                    objpointsExpiry.Datetime = DateTime.Now;
                                    objpointsExpiry.EarnDate = DateTime.Now;
                                    DateTime today = DateTime.Today;
                                    DateTime next = today.AddYears(1);
                                    var currentmonth = DateTime.DaysInMonth(next.Year, next.Month);

                                    if (next.Day < currentmonth)
                                    {
                                        var days = (currentmonth - next.Day);
                                        next = today.AddDays(days).AddYears(1);
                                    }
                                    objpointsExpiry.ExpiryDate = next;
                                    objpointsExpiry.Points = 100;
                                    objpointsExpiry.Status = "00";
                                    objpointsExpiry.InvoiceNo = "B_CovidPoints";
                                    objpointsExpiry.GroupId = "1088";
                                    objpointsExpiry.OriginalInvoiceNo = "";
                                    objpointsExpiry.TransRefNo = null;

                                    contextNew.PointsExpiries.AddOrUpdate(objpointsExpiry);
                                    contextNew.SaveChanges();

                                    objcustomerBeneficiaryDetails.BeneficiaryId = BeneficiaryId;
                                    objcustomerBeneficiaryDetails.CustomerId = objcustomerDetail.CustomerId;
                                    objcustomerBeneficiaryDetails.CustomerName = Name;
                                    objcustomerBeneficiaryDetails.MobileNo = mobileNo;
                                    objcustomerBeneficiaryDetails.Points = Convert.ToDecimal(objcustomerDetail.Points);
                                    objcustomerBeneficiaryDetails.EnrolledDate = DateTime.Now;

                                    contextNew.CustomerBeneficiaryDetails.AddOrUpdate(objcustomerBeneficiaryDetails);
                                    contextNew.SaveChanges();
                                    result = 2;
                                }
                            }
                        }
                        
                    }
                }
            }
           
            catch (Exception ex)
            {
                //newexception.AddException(ex);
            }
            return result;
        }
    }
}
