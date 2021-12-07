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

        public List<MeetingMatrix> GetMeetingMatrixReport(string SalesManager, string FrmDate, string ToDate, string MeetingOrCall)
        {
            MeetingMatrix objMeetingMatrix = new MeetingMatrix();
            List<MeetingMatrix> lstMeetingMatrix = new List<MeetingMatrix>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    DateTime FromDate = Convert.ToDateTime(FrmDate);
                    DateTime ToDateNew = Convert.ToDateTime(ToDate);
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
                                     && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew
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
                                     && c.ContactType == "Call" && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew
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
                                       && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew
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
                                           Comments = c.Comments

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
                                       && c.ContactType == "Call" && c.AddedDate >= FromDate && c.AddedDate <= ToDateNew
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
                                           Comments = c.Comments
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


        public List<PartnerReport> GetPartnerReportData(string FrmDate, string ToDate)
        {
            List<PartnerReport> lstData = new List<PartnerReport>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //DateTime FromDate = Convert.ToDateTime(FrmDate);
                    //DateTime ToDateNew = Convert.ToDateTime(ToDate);

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
                            
                            if (!string.IsNullOrEmpty(FrmDate) && !string.IsNullOrEmpty(FrmDate))
                            {
                                DateTime FromDate = Convert.ToDateTime(FrmDate);
                                DateTime ToDateNew = Convert.ToDateTime(ToDate);
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
                                 
                            }
                            else
                            {
                                objPartner.NoOfAccounts = context.BOTS_TblRetailMaster.Where(x => x.BillingPartner == item).Select(y => y.GroupId).Distinct().Count();
                                objPartner.NoOfOutlets = context.BOTS_TblRetailMaster.Where(x => x.BillingPartner == item).Sum(y => y.NoOfOutlets);
                                groupids = context.BOTS_TblRetailMaster.Where(x => x.BillingPartner == item).Select(y => y.GroupId).Distinct().ToList();
                            }                            
                            
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
    }
}
