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


namespace BOTS_BL.Repository
{
    public class LeadReportsRepository
    {
        Exceptions newexception = new Exceptions();

        public List<MeetingMatrix> GetMeetingMatrixReport(string isMTD,string SalesManager, string FrmDate, string ToDate, string MeetingOrCall)
        {
            MeetingMatrix objMeetingMatrix = new MeetingMatrix();
            List<MeetingMatrix> lstMeetingMatrix = new List<MeetingMatrix>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    DateTime FromDate = new DateTime();
                    DateTime ToDateNew = new DateTime();
                    if (!string.IsNullOrEmpty(FrmDate) && !string.IsNullOrEmpty(FrmDate))
                    {
                        FromDate = Convert.ToDateTime(FrmDate);
                        ToDateNew = Convert.ToDateTime(ToDate);                        
                            ToDateNew = ToDateNew.AddDays(1).Date.AddSeconds(-1);
                       
                    }
                    if(isMTD=="1")
                    {
                        var date = DateTime.Now;
                        FromDate = new DateTime(date.Year, date.Month, 1);
                        ToDateNew = DateTime.Today.AddDays(1).Date.AddSeconds(-1);
                    }
                    var SMDetails = context.CustomerLoginDetails.Where(x => x.LoginType == "8").ToList();
                    foreach (var item in SMDetails)
                    {
                        MeetingMatrix objMeetingMatrixItem = new MeetingMatrix();
                        objMeetingMatrixItem.LoginId = item.LoginId;
                        objMeetingMatrixItem.SMName = item.UserName;
                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.TotalMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && (x.ContactType == "OnlineMeeting" || x.ContactType == "PersonalMeeting")
                            && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }
                        else
                        {
                            objMeetingMatrixItem.TotalMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.ContactType == "Call"
                            && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }
                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.TotalUniqueMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.MeetingType == "1stMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }
                        else
                        {
                            objMeetingMatrixItem.TotalUniqueMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.MeetingType == "1stcall" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                        }

                        if (objMeetingMatrixItem.TotalMeeting != 0)
                        {
                            decimal? uniquePer = (objMeetingMatrixItem.TotalUniqueMeeting * 100) / objMeetingMatrixItem.TotalMeeting;
                            if (uniquePer != null && uniquePer > 0)
                                objMeetingMatrixItem.TotalUniqueMeetingPer = Convert.ToString(uniquePer.Value.ToString("F")) + "%";
                            else
                                objMeetingMatrixItem.TotalUniqueMeetingPer = "0.00%";
                        }
                        else
                        {
                            objMeetingMatrixItem.TotalUniqueMeetingPer = "0.00%";
                        }

                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.IntrestedCases = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.LeadStatus == "Interested" && (x.ContactType == "OnlineMeeting" || x.ContactType == "PersonalMeeting") && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().GroupBy(y => y.LeadId).Count();
                        }
                        else
                        {
                            objMeetingMatrixItem.IntrestedCases = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.LeadStatus == "Interested" && x.ContactType == "Call" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().GroupBy(y => y.LeadId).Count();
                        }

                        if (objMeetingMatrixItem.TotalMeeting != 0)
                        {
                            decimal? intrestedCasesPer = (objMeetingMatrixItem.IntrestedCases * 100) / objMeetingMatrixItem.TotalMeeting;
                            if (intrestedCasesPer != null && intrestedCasesPer > 0)
                                objMeetingMatrixItem.IntrestedCasesPer = Convert.ToString(intrestedCasesPer.Value.ToString("F")) + "%";
                            else
                                objMeetingMatrixItem.IntrestedCasesPer = "0.00%";
                        }
                        else
                        {
                            objMeetingMatrixItem.IntrestedCasesPer = "0.00%";
                        }

                        if (MeetingOrCall == "Meeting")
                        {
                            objMeetingMatrixItem.PersonalMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.ContactType == "PersonalMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                            if (objMeetingMatrixItem.TotalMeeting != 0)
                            {
                                decimal? PersonalMeetingPer = (objMeetingMatrixItem.PersonalMeeting * 100) / objMeetingMatrixItem.TotalMeeting;
                                if (PersonalMeetingPer != null && PersonalMeetingPer > 0)
                                    objMeetingMatrixItem.PersonalMeetingPer = Convert.ToString(PersonalMeetingPer.Value.ToString("F")) + "%";
                                else
                                    objMeetingMatrixItem.PersonalMeetingPer = "0.00%";
                            }
                            else
                            {
                                objMeetingMatrixItem.PersonalMeetingPer = "0.00%";
                            }

                            objMeetingMatrixItem.ZoomMeeting = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == item.LoginId && x.ContactType == "OnlineMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList().Count();
                            if (objMeetingMatrixItem.TotalMeeting != 0)
                            {
                                decimal? ZoomMeetingPer = (objMeetingMatrixItem.ZoomMeeting * 100) / objMeetingMatrixItem.TotalMeeting;
                                if (ZoomMeetingPer != null && ZoomMeetingPer > 0)
                                    objMeetingMatrixItem.ZoomMeetingPer = Convert.ToString(ZoomMeetingPer.Value.ToString("F")) + "%";
                                else
                                    objMeetingMatrixItem.ZoomMeetingPer = "0.00%";
                            }
                            else
                            {
                                objMeetingMatrixItem.ZoomMeetingPer = "0.00%";
                            }
                        }
                        else
                        {
                            objMeetingMatrixItem.PersonalMeeting = 0;
                            objMeetingMatrixItem.ZoomMeeting = 0;
                            objMeetingMatrixItem.PersonalMeetingPer = "0.00%";
                            objMeetingMatrixItem.ZoomMeetingPer = "0.00%";
                        }
                        int count = 0;
                        if (MeetingOrCall == "Meeting")
                        {
                            count = (from c in context.SALES_tblLeads
                                     join cc in context.SALES_tblLeadTracking on c.LeadId equals cc.LeadId
                                     where c.MeetingType == "Followup" && c.UpdatedDate == null
                                     && (c.ContactType == "OnlineMeeting" || c.ContactType == "PersonalMeeting")
                                     && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew && c.FollowupDate < DateTime.Today
                                     && c.AssignedLead == item.LoginId
                                     select new SalesLead
                                     {
                                         LeadId = c.LeadId
                                     }).ToList().Count();
                        }
                        if (MeetingOrCall == "Calling")
                        {
                            count = (from c in context.SALES_tblLeads
                                     join cc in context.SALES_tblLeadTracking on c.LeadId equals cc.LeadId
                                     where c.MeetingType == "Followup" && c.UpdatedDate == null
                                     && c.ContactType == "Call" && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew && c.FollowupDate < DateTime.Today
                                     && c.AssignedLead == item.LoginId
                                     select new SalesLead
                                     {
                                         LeadId = c.LeadId
                                     }).ToList().Count();
                        }

                        objMeetingMatrixItem.UntouchedFollowUp = count;
                        lstMeetingMatrix.Add(objMeetingMatrixItem);

                    }

                    if (!string.IsNullOrEmpty(SalesManager))
                    {
                        lstMeetingMatrix = lstMeetingMatrix.Where(x => x.LoginId == SalesManager).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "MeetingMatrix");
            }
            return lstMeetingMatrix;
        }

        public List<SalesLead> GetDetailMatrixReportData(string SalesManager, string FrmDate, string ToDate, string MeetingOrCall, string type)
        {
            List<SalesLead> lstData = new List<SalesLead>();
            List<SalesLead> newdata = new List<SalesLead>();
            DateTime FromDate = Convert.ToDateTime(FrmDate);
            DateTime ToDateNew = Convert.ToDateTime(ToDate);
            if (MeetingOrCall == "Meeting")
            {
                using (var context = new CommonDBContext())
                {
                    List<SALES_tblLeadTracking> data = new List<SALES_tblLeadTracking>();
                    if (type == "1")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && (x.ContactType == "OnlineMeeting" || x.ContactType == "PersonalMeeting")
                             && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type == "2")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.MeetingType == "1stMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type == "3")
                    {
                        newdata = (from c in context.SALES_tblLeads
                                       join cc in context.SALES_tblLeadTracking on c.LeadId equals cc.LeadId
                                       where c.MeetingType == "Followup" && c.UpdatedDate == null
                                       && (c.ContactType == "OnlineMeeting" || c.ContactType == "PersonalMeeting")
                                       && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew && c.FollowupDate<DateTime.Today
                                       && c.AssignedLead == SalesManager
                                       select new SalesLead
                                       {
                                           LeadId = c.LeadId,
                                           BusinessName = c.BusinessName,
                                           MobileNo = c.MobileNo,
                                           City = c.City,
                                           BillingPartner = c.BillingPartner,
                                           NoOfOutlet = c.NoOfOutlet,
                                           LeadStatus = c.LeadStatus,
                                           FollowupDate = c.FollowupDate,
                                           Comments = c.Comments,
                                           AddedDate = cc.AddedDate

                                       }).ToList();
                        foreach(var item in newdata)
                        {
                            var cId = Convert.ToInt32(item.City);
                            item.CityName = context.tblCities.Where(x => x.CityId == cId).Select(y => y.CityName).FirstOrDefault();

                            var bPartner = Convert.ToInt32(item.BillingPartner);
                            item.BillingPartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == bPartner).Select(y => y.BillingPartnerName).FirstOrDefault();
                        }
                    }
                    if (type == "4")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.LeadStatus == "Interested" && (x.ContactType == "OnlineMeeting" || x.ContactType == "PersonalMeeting") && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type == "5")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.ContactType == "PersonalMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type == "6")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.ContactType == "OnlineMeeting" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type != "3")
                    {
                        foreach (var item in data)
                        {
                            SalesLead lead = new SalesLead();
                            lead.BusinessName = context.SALES_tblLeads.Where(x => x.LeadId == item.LeadId).Select(y => y.BusinessName).FirstOrDefault();
                            lead.MobileNo = context.SALES_tblLeads.Where(x => x.LeadId == item.LeadId).Select(y => y.MobileNo).FirstOrDefault();
                            var cityid = context.SALES_tblLeads.Where(x => x.LeadId == item.LeadId).Select(y => y.City).FirstOrDefault();
                            var cId = Convert.ToInt32(cityid);
                            lead.CityName = context.tblCities.Where(x => x.CityId == cId).Select(y => y.CityName).FirstOrDefault();
                            var bPartner = Convert.ToInt32(item.BillingPartner);
                            lead.BillingPartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == bPartner).Select(y => y.BillingPartnerName).FirstOrDefault();
                            lead.NoOfOutlet = item.NoOfOutlet;
                            lead.LeadStatus = item.LeadStatus;
                            lead.FollowupDate = item.FollowupDate;
                            lead.Comments = item.Comments;
                            lead.AddedDate = item.AddedDate;
                            //lead.SalesManagerName = context.CustomerLoginDetails.Where(x => x.LoginId == item.AssignedLead).Select(y => y.UserName).FirstOrDefault();

                            lstData.Add(lead);
                        }
                    }
                    else
                    {
                        lstData = newdata;
                    }
                }
            }
            if (MeetingOrCall == "Calling")
            {
                using (var context = new CommonDBContext())
                {
                    List<SALES_tblLeadTracking> data = new List<SALES_tblLeadTracking>();
                    if (type == "1")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.ContactType == "Call"
                               && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();

                    }
                    if (type == "2")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.MeetingType == "1stcall" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type == "3")
                    {
                        newdata = (from c in context.SALES_tblLeads
                                       join cc in context.SALES_tblLeadTracking on c.LeadId equals cc.LeadId
                                       where c.MeetingType == "Followup" && c.UpdatedDate == null
                                       && c.ContactType == "Call" && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew && c.FollowupDate < DateTime.Today
                                       && c.AssignedLead == SalesManager
                                       select new SalesLead
                                       {
                                           LeadId = c.LeadId,
                                           BusinessName = c.BusinessName,
                                           MobileNo = c.MobileNo,
                                           City = c.City,
                                           BillingPartner = c.BillingPartner,
                                           NoOfOutlet = c.NoOfOutlet,
                                           LeadStatus = c.LeadStatus,
                                           FollowupDate = c.FollowupDate,
                                           Comments = c.Comments,
                                           AddedDate = cc.AddedDate
                                       }).ToList();

                    }
                    if (type == "4")
                    {
                        data = context.SALES_tblLeadTracking.Where(x => x.AssignedLead == SalesManager && x.LeadStatus == "Interested" && x.ContactType == "Call" && x.AddedDate >= FromDate && x.AddedDate <= ToDateNew).ToList();
                    }
                    if (type == "5")
                    {

                    }
                    if (type == "6")
                    {

                    }
                    if (type != "3")
                    {
                        foreach (var item in data)
                        {
                            SalesLead lead = new SalesLead();
                            lead.BusinessName = context.SALES_tblLeads.Where(x => x.LeadId == item.LeadId).Select(y => y.BusinessName).FirstOrDefault();
                            lead.MobileNo = context.SALES_tblLeads.Where(x => x.LeadId == item.LeadId).Select(y => y.MobileNo).FirstOrDefault();
                            var cityid = context.SALES_tblLeads.Where(x => x.LeadId == item.LeadId).Select(y => y.City).FirstOrDefault();
                            var cId = Convert.ToInt32(cityid);
                            lead.CityName = context.tblCities.Where(x => x.CityId == cId).Select(y => y.CityName).FirstOrDefault();
                            var bPartner = Convert.ToInt32(item.BillingPartner);
                            lead.BillingPartner = context.tblBillingPartners.Where(x => x.BillingPartnerId == bPartner).Select(y => y.BillingPartnerName).FirstOrDefault();
                            lead.NoOfOutlet = item.NoOfOutlet;
                            lead.LeadStatus = item.LeadStatus;
                            lead.FollowupDate = item.FollowupDate;
                            lead.Comments = item.Comments;
                            lead.AddedDate = item.AddedDate;
                            //lead.SalesManagerName = context.CustomerLoginDetails.Where(x => x.LoginId == item.AssignedLead).Select(y => y.UserName).FirstOrDefault();

                            lstData.Add(lead);
                        }
                    }
                    else
                    {
                        lstData = newdata;
                    }
                }
            }


            return lstData;
        }


        public List<PartnerReport> GetPartnerReportData(string FrmDate, string ToDate,string isMTD)
        {
            List<PartnerReport> lstData = new List<PartnerReport>();
            try
            {
                using (var context = new CommonDBContext())
                { 
                    var list = context.BOTS_TblRetailMaster.Select(x => x.BillingPartner).Distinct().ToList();
                    decimal? TotalAmountAll = 0;
                    List<string> groupids = new List<string>();
                    foreach (var item in list)
                    {
                        if (item != "" && item != "0")
                        {
                            PartnerReport objPartner = new PartnerReport();
                            var pId = Convert.ToInt32(item);
                            objPartner.PartnerName = context.tblBillingPartners.Where(x => x.BillingPartnerId == pId).Select(y => y.BillingPartnerName).FirstOrDefault();

                            DateTime FromDate = new DateTime();
                            DateTime ToDateNew = new DateTime();
                            if (!string.IsNullOrEmpty(FrmDate) && !string.IsNullOrEmpty(FrmDate))
                            {
                                FromDate = Convert.ToDateTime(FrmDate);
                                ToDateNew = Convert.ToDateTime(ToDate);
                                ToDateNew = ToDateNew.AddDays(1).Date.AddSeconds(-1);

                            }
                            if (isMTD == "1")
                            {
                                var date = DateTime.Now;
                                FromDate = new DateTime(date.Year, date.Month, 1);
                                ToDateNew = DateTime.Today.AddDays(1).Date.AddSeconds(-1);
                            }
                            
                                objPartner.NoOfAccounts = (from c in context.BOTS_TblGroupMaster
                                             join cc in context.BOTS_TblRetailMaster on c.GroupId equals cc.GroupId
                                             where cc.BillingPartner == item && c.CreatedDate >= FromDate && c.CreatedDate <= ToDateNew                                             
                                             select new SalesLead
                                             {
                                                 BusinessName = c.GroupId
                                             }).ToList().Count();

                                objPartner.NoOfOutlets = (from c in context.BOTS_TblGroupMaster
                                                           join cc in context.BOTS_TblRetailMaster on c.GroupId equals cc.GroupId
                                                           where cc.BillingPartner == item && c.CreatedDate >= FromDate && c.CreatedDate <= ToDateNew
                                                           select new SalesLead
                                                           {
                                                               noOfOutlet = cc.NoOfOutlets
                                                           }).Sum(x=>x.noOfOutlet);

                               groupids = (from c in context.BOTS_TblGroupMaster
                                                          join cc in context.BOTS_TblRetailMaster on c.GroupId equals cc.GroupId
                                                          where cc.BillingPartner == item && c.CreatedDate >= FromDate && c.CreatedDate <= ToDateNew
                                                          select new SalesLead
                                                          {
                                                              GroupId = c.GroupId
                                                          }).Select(x => x.GroupId).ToList();                                                        
                            
                            decimal? TotalAmount = 0;
                            foreach(var groupid in groupids)
                            {
                                var grouprecord = context.BOTS_TblDealDetails.Where(x => x.GroupId == groupid).FirstOrDefault();
                                if(grouprecord.PaymentFrequency=="2")
                                {
                                    TotalAmount = TotalAmount + grouprecord.AdvanceAmount;
                                }
                                else
                                {
                                    TotalAmount = TotalAmount + grouprecord.AmountReceived;
                                }
                            }
                            objPartner.TotalAmount = TotalAmount;
                            TotalAmountAll = TotalAmountAll + TotalAmount;

                            lstData.Add(objPartner);
                        }
                    }
                    foreach(var item in lstData)
                    {
                        var revenue = (item.TotalAmount * 100) / TotalAmountAll;
                        item.ContributionInRevenue = Math.Round((Decimal)revenue, 2);
                        if (item.NoOfOutlets != null)
                        {
                            var avgOutlet = item.TotalAmount / item.NoOfOutlets;
                            item.AvgRevenuePerOutlet = Math.Round((Decimal)avgOutlet, 2);
                        }
                        else
                        {
                            item.NoOfOutlets = 0;
                            item.AvgRevenuePerOutlet = 0;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPartnerReportData");
            }


            return lstData;
        }

        public List<SalesMatrix> GetSalesMatrix(string radiovalue, int month, int year, string sm)
        {
            List<SalesMatrix> lstsalesmatrix = new List<SalesMatrix>();
           // List<SalesMatrixDetail> lstsalesmatrixdetails = new List<SalesMatrixDetail>();
            
           
            using (var context = new CommonDBContext())
            {
                DateTime first = new DateTime();
                DateTime last = DateTime.MaxValue;

                if (month != 0 && year != 0)
                {
                    first = new DateTime(year, month, 1);
                    last = first.AddMonths(1).AddSeconds(-1);
                }
                else if (radiovalue != "")
                {
                    if (radiovalue == "btd")
                    {

                    }
                    else if (radiovalue == "mtd")
                    {
                        DateTime today = DateTime.Today;
                        first = new DateTime(today.Year, today.Month, 1);
                        last = today;
                    }
                    else if (radiovalue == "qtd")
                    {
                        DateTime today = DateTime.Today;
                        last = new DateTime(today.Year, today.Month, 1);
                        first = last.AddMonths(-3).AddSeconds(-1);

                    }
                }
                var SMDetails = context.CustomerLoginDetails.Where(x => x.LoginType == "8").ToList();
                if (sm != "")
                {
                    SMDetails = context.CustomerLoginDetails.Where(x => x.LoginId == sm).ToList();
                }

                foreach (var item in SMDetails)
                {
                    
                    SalesMatrix objsalesmatrix = new SalesMatrix();
                    objsalesmatrix.SMName = item.UserName;
                    var grouprecord = (from d in context.BOTS_TblDealDetails
                                       join g in context.BOTS_TblGroupMaster
                                       on d.GroupId equals g.GroupId
                                       join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                       join b in context.tblBillingPartners on r.BillingPartner equals b.BillingPartnerId.ToString()
                                       where g.CreatedBy == item.LoginId && g.CreatedDate >= first && g.CreatedDate <= last && g.CustomerStatus != "Draft"
                                       select new
                                       {
                                           GroupId = d.GroupId,
                                           SMId = g.CreatedBy,
                                           LoyaltyFees = d.LoyaltyFees,
                                           WAPaidPackFees = d.WAPaidPackFees,
                                           SMSPaidPackFees = d.SMSPaidPackFees,
                                           EcommIntegration = d.EcommIntegration,
                                           AnyOtherFees = d.AnyOtherFees,
                                           TotalFeesA = d.TotalFeesA,
                                           GST = d.GST,
                                           TotalFeesB = d.TotalFeesB,
                                           PaymentFrequency = d.PaymentFrequency,
                                           AnyOtherFeesDesc = d.AnyOtherFeesDesc,
                                           AmountReceived = d.AmountReceived,
                                           TDSDeducted = d.TDSDeducted,
                                           PaymentMode = d.PaymentMode,
                                           PaymentStatus = d.PaymentStatus,
                                           GSTRate = d.GSTRate,
                                           AdvanceAmount = d.AdvanceAmount,
                                           Boproduct = r.BOProduct,
                                           Noofoutlets = r.NoOfEnrolled,
                                           createddate = g.CreatedDate,
                                           billingPartner = b.BillingPartnerName,
                                           GroupName = g.GroupName

                                       }).Distinct().ToList();
                    
                    decimal? TotalAmount = 0;
                    decimal? Octaxstotalamt = 0;
                    decimal? OctaPlustotalamt = 0;
                    decimal? singleoutlettotalamt = 0;
                    decimal? Multioutlettotalamt = 0;
                    decimal? Lastmonthrevenue = 0;
                    decimal? PreviousMonthRevenue = 0;
                    foreach (var itemgrp in grouprecord)
                    {
                        if (itemgrp.PaymentFrequency == "2")
                        {
                            TotalAmount = TotalAmount + itemgrp.AdvanceAmount;
                            Octaxstotalamt = TotalAmount;
                        }
                        else
                        {
                            TotalAmount = TotalAmount + itemgrp.AmountReceived;
                            OctaPlustotalamt = TotalAmount;
                        }

                        if (itemgrp.Noofoutlets == 1)
                        {
                            if (itemgrp.PaymentFrequency == "2")
                            {
                                singleoutlettotalamt = singleoutlettotalamt + itemgrp.AdvanceAmount;

                            }
                            else
                            {
                                singleoutlettotalamt = singleoutlettotalamt + itemgrp.AmountReceived;

                            }
                        }

                    }
                    var octaxssalescount = grouprecord.Where(x => x.Boproduct == "2").Count();
                    var octaplussalescount = grouprecord.Where(x => x.Boproduct == "1").Count();
                    var singleoutletnoofsales = grouprecord.Where(x => x.Noofoutlets == 1).Count();
                    var Multioutletnoofsales = grouprecord.Where(x => x.Noofoutlets > 1).Distinct().ToList();

                    var count = Multioutletnoofsales.Count();

                    foreach (var itemmulti in Multioutletnoofsales)
                    {


                        if (itemmulti.PaymentFrequency == "2")
                        {
                            Multioutlettotalamt = Multioutlettotalamt + itemmulti.AdvanceAmount;

                        }
                        else
                        {
                            Multioutlettotalamt = Multioutlettotalamt + itemmulti.AmountReceived;

                        }

                    }
                    objsalesmatrix.SMId = item.LoginId;
                    objsalesmatrix.MultipleOutlet = count;
                    objsalesmatrix.TotalRevenue = TotalAmount;
                    objsalesmatrix.NoOfSales = grouprecord.Count();
                    if (octaplussalescount > 0)
                    {
                        decimal? avgrevplus = OctaPlustotalamt / octaplussalescount;
                        objsalesmatrix.AvgRevenueOctaPlus = Convert.ToDecimal(string.Format("{0:0.00}", avgrevplus));
                    }
                    else
                    {
                        objsalesmatrix.AvgRevenueOctaPlus = OctaPlustotalamt;
                    }
                    if (octaxssalescount > 0)
                    {
                        decimal? avgrevxs = Octaxstotalamt / octaxssalescount;
                        objsalesmatrix.AvgRevenueOctaXs = Convert.ToDecimal(string.Format("{0:0.00}", avgrevxs));
                    }
                    else
                    {
                        objsalesmatrix.AvgRevenueOctaXs = Octaxstotalamt;
                    }


                    if (singleoutletnoofsales > 0)
                    {
                        decimal? avg = singleoutlettotalamt / singleoutletnoofsales;
                        objsalesmatrix.AvgRevenuesingleoutlet = Convert.ToDecimal(string.Format("{0:0.00}", avg));
                    }
                    else
                    {
                        objsalesmatrix.AvgRevenuesingleoutlet = singleoutlettotalamt;
                    }
                    if (count > 0)
                    {
                        decimal? avgmulti = Multioutlettotalamt / count;
                        objsalesmatrix.AvgRevenueMultipleOutlet = Convert.ToDecimal(string.Format("{0:0.00}", avgmulti));
                    }
                    else
                    {
                        objsalesmatrix.AvgRevenueMultipleOutlet = Multioutlettotalamt;
                    }

                    var premonthcount = 0;
                    if (radiovalue != "none")
                    {
                        DateTime prefirst = new DateTime();
                        DateTime prelastdt = DateTime.MaxValue;
                        DateTime twomonthback = DateTime.Today;
                        if (radiovalue == "btd")
                        {
                            twomonthback = new DateTime();
                            prefirst = DateTime.MaxValue;
                        }
                        else if (radiovalue == "mtd")
                        {
                            prelastdt = DateTime.Today;
                            prefirst = prelastdt.AddMonths(-1).AddSeconds(-1);

                            twomonthback = prefirst.AddMonths(-1).AddSeconds(-1);

                        }
                        else if (radiovalue == "qtd")
                        {
                            prelastdt = DateTime.Today;
                            prefirst = prelastdt.AddMonths(-3).AddSeconds(-1);
                            twomonthback = prefirst.AddMonths(-3).AddSeconds(-1);
                        }

                        var previousmonthrevenue = (from d in context.BOTS_TblDealDetails
                                                    join g in context.BOTS_TblGroupMaster
                                                    on d.GroupId equals g.GroupId
                                                    join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                                    where g.CreatedBy == item.LoginId && g.CreatedDate >= prefirst && g.CreatedDate <= prelastdt && g.CustomerStatus != "Draft"
                                                    select new
                                                    {
                                                        GroupId = d.GroupId,
                                                        SMId = g.CreatedBy,
                                                        PaymentFrequency = d.PaymentFrequency,
                                                        AmountReceived = d.AmountReceived,
                                                        AdvanceAmount = d.AdvanceAmount,
                                                        Noofoutlets = r.NoOfEnrolled,
                                                        createddate = g.CreatedDate,
                                                       // billingPartner = r.BillingPartnerName

                                                    }).ToList();
                        foreach (var itemprerev in previousmonthrevenue)
                        {
                            if (itemprerev.PaymentFrequency == "2")
                            {
                                PreviousMonthRevenue = PreviousMonthRevenue + itemprerev.AdvanceAmount;

                            }
                            else
                            {
                                PreviousMonthRevenue = PreviousMonthRevenue + itemprerev.AmountReceived;

                            }
                        }

                        var lastmonthrevenue = grouprecord.Where(x => x.createddate < prefirst && x.createddate > twomonthback).ToList();
                        premonthcount = previousmonthrevenue.Count();
                        foreach (var itemrev in lastmonthrevenue)
                        {
                            if (itemrev.PaymentFrequency == "2")
                            {
                                Lastmonthrevenue = Lastmonthrevenue + itemrev.AdvanceAmount;

                            }
                            else
                            {
                                Lastmonthrevenue = Lastmonthrevenue + itemrev.AmountReceived;

                            }
                        }
                    }
                    else
                    {
                        // DateTime previousmonthfrom = new DateTime(first.Year, first.Month, 1);
                        DateTime previousmonthto = first.AddMonths(-1).AddSeconds(-1);
                        var previousmonthrevenue = (from d in context.BOTS_TblDealDetails
                                                    join g in context.BOTS_TblGroupMaster
                                                    on d.GroupId equals g.GroupId
                                                    where g.CreatedBy == item.LoginId && g.CreatedDate > previousmonthto && g.CreatedDate <= first && g.CustomerStatus != "Draft"
                                                    select new
                                                    {
                                                        GroupId = d.GroupId,
                                                        SMId = g.CreatedBy,
                                                        PaymentFrequency = d.PaymentFrequency,
                                                        AmountReceived = d.AmountReceived,
                                                        AdvanceAmount = d.AdvanceAmount,
                                                        createddate = g.CreatedDate

                                                    }).ToList();
                        foreach (var itemprerev in previousmonthrevenue)
                        {
                            if (itemprerev.PaymentFrequency == "2")
                            {
                                PreviousMonthRevenue = PreviousMonthRevenue + itemprerev.AdvanceAmount;

                            }
                            else
                            {
                                PreviousMonthRevenue = PreviousMonthRevenue + itemprerev.AmountReceived;

                            }
                        }

                        var lastmonthrevenue = grouprecord.Where(x => x.createddate > previousmonthto && x.createddate < first).ToList();
                        premonthcount = previousmonthrevenue.Count();


                        foreach (var itemrev in lastmonthrevenue)
                        {
                            if (itemrev.PaymentFrequency == "2")
                            {
                                Lastmonthrevenue = Lastmonthrevenue + itemrev.AdvanceAmount;

                            }
                            else
                            {
                                Lastmonthrevenue = Lastmonthrevenue + itemrev.AmountReceived;

                            }
                        }
                    }
                    decimal? difference = (decimal?)0.00;
                    if (TotalAmount > 0)
                    {
                        difference = Convert.ToDecimal(string.Format("{0:0.00}", (Lastmonthrevenue - PreviousMonthRevenue) / TotalAmount));
                    }
                    objsalesmatrix.PreviousMonthTotalRevenue = PreviousMonthRevenue;
                    objsalesmatrix.PreviousMonthNoOfSales = premonthcount;
                    objsalesmatrix.Revenuepercentage = difference;

                    //Irrespective of month selected 
                    var avgmonthrevenuemax = (from d in context.BOTS_TblDealDetails
                                              join g in context.BOTS_TblGroupMaster
                                              on d.GroupId equals g.GroupId
                                              where g.CreatedBy == item.LoginId && g.CustomerStatus != "Draft"
                                              select new
                                              {
                                                  createddate = g.CreatedDate

                                              }).Max(x => x.createddate);
                    var avgmonthrevenuemin = (from d in context.BOTS_TblDealDetails
                                              join g in context.BOTS_TblGroupMaster
                                              on d.GroupId equals g.GroupId
                                              where g.CreatedBy == item.LoginId && g.CustomerStatus != "Draft"
                                              select new
                                              {
                                                  createddate = g.CreatedDate

                                              }).Min(x => x.createddate);
                    int monTH = avgmonthrevenuemax.Month - avgmonthrevenuemin.Month;

                    decimal? btdtotalamt = (decimal?)0.00;
                    var BTDamt = (from d in context.BOTS_TblDealDetails
                                  join g in context.BOTS_TblGroupMaster
                                  on d.GroupId equals g.GroupId
                                  where g.CreatedBy == item.LoginId && g.CustomerStatus != "Draft"
                                  select new
                                  {
                                      GroupId = d.GroupId,
                                      PaymentFrequency = d.PaymentFrequency,
                                      AmountReceived = d.AmountReceived,
                                      AdvanceAmount = d.AdvanceAmount,
                                      createddate = g.CreatedDate

                                  }).ToList();

                    foreach (var itemamt in BTDamt)
                    {
                        if (itemamt.PaymentFrequency == "2")
                        {
                            btdtotalamt = btdtotalamt + itemamt.AdvanceAmount;

                        }
                        else
                        {
                            btdtotalamt = btdtotalamt + itemamt.AmountReceived;

                        }
                    }
                    var BTDSalesCount = BTDamt.Count();
                    if (monTH > 0)
                    {
                        //Irrespective of month selected
                        objsalesmatrix.AvgRevenuepermonth = Convert.ToDecimal(string.Format("{0:0.00}", (btdtotalamt / monTH)));
                        //Irrespective of month selected
                        objsalesmatrix.BTDNoofSalesDone = Convert.ToDecimal(string.Format("{0:0.00}", (BTDSalesCount / monTH)));
                    }
                    //Irrespective of month selected or btd qtd mtd
                    DateTime Today = DateTime.Today;
                    DateTime lastrevtodate = Today.AddMonths(-1).AddSeconds(-1);
                    DateTime lastrevfromdt = new DateTime(lastrevtodate.Year, lastrevtodate.Month, 1);
                    decimal? LastMonthRevenue = 0;
                    var lastmonthrev = (from d in context.BOTS_TblDealDetails
                                        join g in context.BOTS_TblGroupMaster
                                        on d.GroupId equals g.GroupId
                                        where g.CreatedBy == item.LoginId && g.CreatedDate >= lastrevfromdt && g.CreatedDate <= lastrevtodate && g.CustomerStatus != "Draft"
                                        select new
                                        {
                                            GroupId = d.GroupId,
                                            SMId = g.CreatedBy,
                                            PaymentFrequency = d.PaymentFrequency,
                                            AmountReceived = d.AmountReceived,
                                            AdvanceAmount = d.AdvanceAmount,
                                            createddate = g.CreatedDate

                                        }).ToList();
                    foreach (var itemrev in lastmonthrev)
                    {
                        if (itemrev.PaymentFrequency == "2")
                        {
                            LastMonthRevenue = LastMonthRevenue + itemrev.AdvanceAmount;

                        }
                        else
                        {
                            LastMonthRevenue = LastMonthRevenue + itemrev.AmountReceived;

                        }
                    }
                    objsalesmatrix.LastMonthRevenue = LastMonthRevenue;

                    lstsalesmatrix.Add(objsalesmatrix);
                }


            }
            
            return lstsalesmatrix;
        }

        public List<SalesMatrixDetail>GetSalesMatrixDetails(string radiovalue, int month, int year, string sm,string type)
        {
            List<SalesMatrixDetail> lstdetails = new List<SalesMatrixDetail>();
           // List<SalesMatrix> lstsalesmatrix = new List<SalesMatrix>();
            List<SalesMatrixDetail> lstsalesmatrixdetails = new List<SalesMatrixDetail>();


            using (var context = new CommonDBContext())
            {
                DateTime first = new DateTime();
                DateTime last = DateTime.MaxValue;

                if (month != 0 && year != 0)
                {
                    first = new DateTime(year, month, 1);
                    last = first.AddMonths(1).AddSeconds(-1);
                }
                else if (radiovalue != "")
                {
                    if (radiovalue == "btd")
                    {

                    }
                    else if (radiovalue == "mtd")
                    {
                        DateTime today = DateTime.Today;
                        first = new DateTime(today.Year, today.Month, 1);
                        last = today;
                    }
                    else if (radiovalue == "qtd")
                    {
                        DateTime today = DateTime.Today;
                        last = new DateTime(today.Year, today.Month, 1);
                        first = last.AddMonths(-3).AddSeconds(-1);

                    }
                }
                var SMDetails = context.CustomerLoginDetails.Where(x => x.LoginType == "8").ToList();
                if (sm != "")
                {
                    SMDetails = context.CustomerLoginDetails.Where(x => x.LoginId == sm).ToList();
                }

                foreach (var item in SMDetails)
                {

                    SalesMatrix objsalesmatrix = new SalesMatrix();
                    objsalesmatrix.SMName = item.UserName;
                    if (type == "current")
                    {
                        var grouprecord = (from d in context.BOTS_TblDealDetails
                                           join g in context.BOTS_TblGroupMaster
                                           on d.GroupId equals g.GroupId
                                           join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                           join b in context.tblBillingPartners on r.BillingPartner equals b.BillingPartnerId.ToString()
                                           where g.CreatedBy == item.LoginId && g.CreatedDate >= first && g.CreatedDate <= last && g.CustomerStatus != "Draft"
                                           select new
                                           {
                                               GroupId = d.GroupId,
                                               SMId = g.CreatedBy,
                                               LoyaltyFees = d.LoyaltyFees,
                                               WAPaidPackFees = d.WAPaidPackFees,
                                               SMSPaidPackFees = d.SMSPaidPackFees,
                                               EcommIntegration = d.EcommIntegration,
                                               AnyOtherFees = d.AnyOtherFees,
                                               TotalFeesA = d.TotalFeesA,
                                               GST = d.GST,
                                               TotalFeesB = d.TotalFeesB,
                                               PaymentFrequency = d.PaymentFrequency,
                                               AnyOtherFeesDesc = d.AnyOtherFeesDesc,
                                               AmountReceived = d.AmountReceived,
                                               TDSDeducted = d.TDSDeducted,
                                               PaymentMode = d.PaymentMode,
                                               PaymentStatus = d.PaymentStatus,
                                               GSTRate = d.GSTRate,
                                               AdvanceAmount = d.AdvanceAmount,
                                               Boproduct = r.BOProduct,
                                               Noofoutlets = r.NoOfEnrolled,
                                               createddate = g.CreatedDate,
                                               billingPartner = b.BillingPartnerName,
                                               GroupName = g.GroupName

                                           }).Distinct().ToList();
                        foreach (var itemdetail in grouprecord)
                        {
                            SalesMatrixDetail objsalesmatrixdetails = new SalesMatrixDetail();
                            objsalesmatrixdetails.SMName = item.UserName;
                            objsalesmatrixdetails.BusinessNm = itemdetail.GroupName;
                            if (itemdetail.Boproduct == "1")
                            {
                                objsalesmatrixdetails.Product = "Octa Plus";
                            }
                            else
                            {
                                objsalesmatrixdetails.Product = "Octa XS";
                            }

                            objsalesmatrixdetails.BillingPartner = itemdetail.billingPartner;
                            if (itemdetail.PaymentFrequency == "2")
                            {
                                objsalesmatrixdetails.Amount = itemdetail.AdvanceAmount;

                            }
                            else
                            {
                                objsalesmatrixdetails.Amount = itemdetail.AmountReceived;

                            }


                            objsalesmatrixdetails.NoofOutlet = itemdetail.Noofoutlets;
                            objsalesmatrixdetails.CreatedOn = itemdetail.createddate;

                            lstsalesmatrixdetails.Add(objsalesmatrixdetails);

                        }
                    }
                    if (type == "previous")
                    {
                        if (radiovalue != "none")
                        {
                            DateTime prefirst = new DateTime();
                            DateTime prelastdt = DateTime.MaxValue;
                            DateTime twomonthback = DateTime.Today;
                            if (radiovalue == "btd")
                            {
                                twomonthback = new DateTime();
                                prefirst = DateTime.MaxValue;
                            }
                            else if (radiovalue == "mtd")
                            {
                                prelastdt = DateTime.Today;
                                prefirst = prelastdt.AddMonths(-1).AddSeconds(-1);

                                twomonthback = prefirst.AddMonths(-1).AddSeconds(-1);

                            }
                            else if (radiovalue == "qtd")
                            {
                                prelastdt = DateTime.Today;
                                prefirst = prelastdt.AddMonths(-3).AddSeconds(-1);
                                twomonthback = prefirst.AddMonths(-3).AddSeconds(-1);
                            }

                            var previousmonthrevenue = (from d in context.BOTS_TblDealDetails
                                                        join g in context.BOTS_TblGroupMaster
                                                        on d.GroupId equals g.GroupId
                                                        join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                                        join b in context.tblBillingPartners on r.BillingPartner equals b.BillingPartnerId.ToString()
                                                        where g.CreatedBy == item.LoginId && g.CreatedDate >= prefirst && g.CreatedDate <= prelastdt && g.CustomerStatus != "Draft"
                                                        select new
                                                        {
                                                            GroupId = d.GroupId,
                                                            SMId = g.CreatedBy,
                                                            PaymentFrequency = d.PaymentFrequency,
                                                            AmountReceived = d.AmountReceived,
                                                            AdvanceAmount = d.AdvanceAmount,
                                                            createddate = g.CreatedDate,                                         
                                                            Boproduct = r.BOProduct,
                                                            Noofoutlets = r.NoOfEnrolled,
                                                            
                                                            //billingPartner = b.BillingPartnerName,
                                                            GroupName = g.GroupName

                                                        }).ToList();

                            foreach (var itemdetail in previousmonthrevenue)
                            {
                                SalesMatrixDetail objsalesmatrixdetails = new SalesMatrixDetail();
                                objsalesmatrixdetails.SMName = item.UserName;
                                objsalesmatrixdetails.BusinessNm = itemdetail.GroupName;
                                if (itemdetail.Boproduct == "1")
                                {
                                    objsalesmatrixdetails.Product = "Octa Plus";
                                }
                                else
                                {
                                    objsalesmatrixdetails.Product = "Octa XS";
                                }

                                //objsalesmatrixdetails.BillingPartner = itemdetail.billingPartner;
                                if (itemdetail.PaymentFrequency == "2")
                                {
                                    objsalesmatrixdetails.Amount = itemdetail.AdvanceAmount;

                                }
                                else
                                {
                                    objsalesmatrixdetails.Amount = itemdetail.AmountReceived;

                                }


                                objsalesmatrixdetails.NoofOutlet = itemdetail.Noofoutlets;
                                objsalesmatrixdetails.CreatedOn = itemdetail.createddate;

                                lstsalesmatrixdetails.Add(objsalesmatrixdetails);

                            }
                        }

                        else
                        {
                            // DateTime previousmonthfrom = new DateTime(first.Year, first.Month, 1);
                            DateTime previousmonthto = first.AddMonths(-1).AddSeconds(-1);
                            var previousmonthrevenue = (from d in context.BOTS_TblDealDetails
                                                        join g in context.BOTS_TblGroupMaster
                                                        on d.GroupId equals g.GroupId
                                                        join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                                       join b in context.tblBillingPartners on r.BillingPartner equals b.BillingPartnerId.ToString()
                                                        where g.CreatedBy == item.LoginId && g.CreatedDate > previousmonthto && g.CreatedDate <= first && g.CustomerStatus != "Draft"
                                                        select new
                                                        {
                                                            GroupId = d.GroupId,
                                                            SMId = g.CreatedBy,
                                                            PaymentFrequency = d.PaymentFrequency,
                                                            AmountReceived = d.AmountReceived,
                                                            AdvanceAmount = d.AdvanceAmount,
                                                            createddate = g.CreatedDate,                                                  
                                                            Boproduct = r.BOProduct,
                                                            Noofoutlets = r.NoOfEnrolled,                                                            
                                                           // billingPartner = b.BillingPartnerName,
                                                            GroupName = g.GroupName

                                                        }).ToList();

                            foreach (var itemdetail in previousmonthrevenue)
                            {
                                SalesMatrixDetail objsalesmatrixdetails = new SalesMatrixDetail();
                                objsalesmatrixdetails.SMName = item.UserName;
                                objsalesmatrixdetails.BusinessNm = itemdetail.GroupName;
                                if (itemdetail.Boproduct == "1")
                                {
                                    objsalesmatrixdetails.Product = "Octa Plus";
                                }
                                else
                                {
                                    objsalesmatrixdetails.Product = "Octa XS";
                                }

                               // objsalesmatrixdetails.BillingPartner = itemdetail.billingPartner;
                                if (itemdetail.PaymentFrequency == "2")
                                {
                                    objsalesmatrixdetails.Amount = itemdetail.AdvanceAmount;

                                }
                                else
                                {
                                    objsalesmatrixdetails.Amount = itemdetail.AmountReceived;

                                }


                                objsalesmatrixdetails.NoofOutlet = itemdetail.Noofoutlets;
                                objsalesmatrixdetails.CreatedOn = itemdetail.createddate;

                                lstsalesmatrixdetails.Add(objsalesmatrixdetails);

                            }
                        }
                    }
                }
            }
                
            return lstsalesmatrixdetails;

        }
    }
}
