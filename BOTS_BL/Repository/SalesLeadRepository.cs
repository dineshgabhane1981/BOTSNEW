﻿using System;
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
    public class SalesLeadRepository
    {
        Exceptions newexception = new Exceptions();
        public int AddSalesLead(SALES_tblLeads objtbllead)
        {
            SALES_tblLeads objlead = new SALES_tblLeads();
            SALES_tblLeadTracking objsalestracking = new SALES_tblLeadTracking();
            int leadId = 0;
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (objtbllead.LeadId == 0)
                    {
                        context.SALES_tblLeads.Add(objtbllead);
                        context.SaveChanges();
                        leadId = objtbllead.LeadId;

                        objsalestracking.LeadId = objtbllead.LeadId;
                        objsalestracking.ContactType = objtbllead.ContactType;
                        objsalestracking.SpokeWith = objtbllead.SpokeWith;
                        objsalestracking.LeadStatus = objtbllead.LeadStatus;
                        objsalestracking.MeetingType = objtbllead.MeetingType;
                        objsalestracking.FollowupDate = objtbllead.FollowupDate;
                        objsalestracking.BillingPartner = objtbllead.BillingPartner;
                        objsalestracking.NoOfOutlet = objtbllead.NoOfOutlet;
                        objsalestracking.EcomIntegration = objtbllead.EcomIntegration;
                        objsalestracking.Address = objtbllead.Address;
                        objsalestracking.State = objtbllead.State;
                        objsalestracking.AlternateNo = objtbllead.AlternateNo;
                        objsalestracking.EmailId = objtbllead.EmailId;
                        objsalestracking.AuthorizedPerson = objtbllead.AuthorizedPerson;
                        objsalestracking.APMobileNo = objtbllead.APMobileNo;
                        objsalestracking.PriceQuoted = objtbllead.PriceQuoted;
                        objsalestracking.LeadSource = objtbllead.LeadSource;
                        objsalestracking.LeadSourceName = objtbllead.LeadSourceName;
                        objsalestracking.Comments = objtbllead.Comments;
                        objsalestracking.AddedBy = objtbllead.AddedBy;
                        objsalestracking.AddedDate = objtbllead.AddedDate;
                        objsalestracking.AssignedLead = objtbllead.AddedBy;

                        context.SALES_tblLeadTracking.AddOrUpdate(objsalestracking);
                        context.SaveChanges();

                    }
                    else
                    {
                        objlead = context.SALES_tblLeads.Where(x => x.LeadId == objtbllead.LeadId).FirstOrDefault();

                        objtbllead.UpdatedDate = DateTime.Now;
                        objtbllead.Category = objlead.Category;
                        objtbllead.Product = objlead.Product;
                        objtbllead.City = objlead.City;
                        objtbllead.LeadSource = objlead.LeadSource;
                        objtbllead.LeadSourceName = objlead.LeadSourceName;

                        if (objtbllead.MeetingType == "salesdone")
                        {
                            objtbllead.MeetingType = objlead.MeetingType;
                        }

                        context.SALES_tblLeads.AddOrUpdate(objtbllead);
                        context.SaveChanges();
                        //leadId = objtbllead.LeadId;

                        objsalestracking.LeadId = objlead.LeadId;
                        objsalestracking.ContactType = objtbllead.ContactType;
                        objsalestracking.SpokeWith = objtbllead.SpokeWith;
                        objsalestracking.LeadStatus = objtbllead.LeadStatus;
                        if (objtbllead.MeetingType == "salesdone")
                        {
                            objsalestracking.MeetingType = objlead.MeetingType;
                        }
                        else
                        {
                            objsalestracking.MeetingType = objtbllead.MeetingType;
                        }
                        objsalestracking.FollowupDate = objtbllead.FollowupDate;
                        objsalestracking.BillingPartner = objtbllead.BillingPartner;
                        objsalestracking.NoOfOutlet = objtbllead.NoOfOutlet;
                        objsalestracking.EcomIntegration = objtbllead.EcomIntegration;
                        objsalestracking.Address = objtbllead.Address;
                        objsalestracking.State = objtbllead.State;
                        objsalestracking.AlternateNo = objtbllead.AlternateNo;
                        objsalestracking.EmailId = objtbllead.EmailId;
                        objsalestracking.AuthorizedPerson = objtbllead.AuthorizedPerson;
                        objsalestracking.APMobileNo = objtbllead.APMobileNo;
                        objsalestracking.PriceQuoted = objtbllead.PriceQuoted;
                        objsalestracking.LeadSource = objtbllead.LeadSource;
                        objsalestracking.LeadSourceName = objtbllead.LeadSourceName;
                        objsalestracking.Comments = objtbllead.Comments;
                        objsalestracking.AddedBy = objtbllead.AddedBy;
                        objsalestracking.AddedDate = objtbllead.AddedDate;
                        objsalestracking.AssignedLead = objtbllead.AddedBy;

                        context.SALES_tblLeadTracking.AddOrUpdate(objsalestracking);
                        context.SaveChanges();

                        leadId = objtbllead.LeadId;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AddSalesLead");
            }
            return leadId;
        }
        public SALES_tblLeads GetsalesLeadByLeadId(int LeadId)
        {
            SALES_tblLeads objsaleslead = new SALES_tblLeads();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objsaleslead = context.SALES_tblLeads.Where(x => x.LeadId == LeadId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetsalesLeadByLeadId");
            }
            return objsaleslead;
        }
        public List<SalesLead> GetFollowupLeads(CustomerLoginDetail objCust)
        {
            List<SalesLead> lstsaleslead = new List<SalesLead>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstsaleslead = (from c in context.SALES_tblLeads
                                    join ct in context.tblCities on c.City equals ct.CityId.ToString()
                                    join cd in context.CustomerLoginDetails on c.AssignedLead equals cd.LoginId
                                    join bp in context.tblBillingPartners on c.BillingPartner equals bp.BillingPartnerId.ToString()
                                    into ps
                                    from bp in ps.DefaultIfEmpty()
                                    where (c.FollowupDate == today || c.FollowupDate == tommrowdt) && c.MeetingType != "salesdone"
                                    select new SalesLead
                                    {
                                        LeadId = c.LeadId,
                                        BusinessName = c.BusinessName,
                                        Category = c.Category,
                                        Product = c.Product,
                                        BillingPartner = bp.BillingPartnerName,
                                        NoOfOutlet = c.NoOfOutlet,
                                        Address = c.Address,
                                        State = c.State,
                                        City = c.City,
                                        Pincode = c.Pincode,
                                        ContactType = c.ContactType,
                                        SpokeWith = c.SpokeWith,
                                        MobileNo = c.MobileNo,
                                        AlternateNo = c.AlternateNo,
                                        EmailId = c.EmailId,
                                        AuthorizedPerson = c.AuthorizedPerson,
                                        APMobileNo = c.APMobileNo,
                                        LeadStatus = c.LeadStatus,
                                        PriceQuoted = c.PriceQuoted,
                                        MeetingType = c.MeetingType,
                                        FollowupDate = c.FollowupDate,
                                        LeadSource = c.LeadSource,
                                        LeadSourceName = c.LeadSourceName,
                                        Comments = c.Comments,
                                        AddedBy = c.AddedBy,
                                        AddedDate = c.AddedDate,
                                        UpdatedDate = c.UpdatedDate,
                                        CityName = ct.CityName,
                                        AssignedLead = c.AssignedLead,
                                        SalesManagerName = cd.UserName

                                    }).ToList();

                    var lstsalesleadold = (from c in context.SALES_tblLeads
                                           join ct in context.tblCities on c.City equals ct.CityId.ToString()
                                           join cd in context.CustomerLoginDetails on c.AssignedLead equals cd.LoginId
                                           join bp in context.tblBillingPartners on c.BillingPartner equals bp.BillingPartnerId.ToString()
                                           into ps
                                           from bp in ps.DefaultIfEmpty()
                                           where c.FollowupDate < today && c.UpdatedDate < today
                                           && c.LeadStatus != "NotInterested" && c.MeetingType != "salesdone" && c.MeetingType != "Closure"
                                           select new SalesLead
                                           {
                                               LeadId = c.LeadId,
                                               BusinessName = c.BusinessName,
                                               Category = c.Category,
                                               Product = c.Product,
                                               BillingPartner = bp.BillingPartnerName,
                                               NoOfOutlet = c.NoOfOutlet,
                                               Address = c.Address,
                                               State = c.State,
                                               City = c.City,
                                               Pincode = c.Pincode,
                                               ContactType = c.ContactType,
                                               SpokeWith = c.SpokeWith,
                                               MobileNo = c.MobileNo,
                                               AlternateNo = c.AlternateNo,
                                               EmailId = c.EmailId,
                                               AuthorizedPerson = c.AuthorizedPerson,
                                               APMobileNo = c.APMobileNo,
                                               LeadStatus = c.LeadStatus,
                                               PriceQuoted = c.PriceQuoted,
                                               MeetingType = c.MeetingType,
                                               FollowupDate = c.FollowupDate,
                                               LeadSource = c.LeadSource,
                                               LeadSourceName = c.LeadSourceName,
                                               Comments = c.Comments,
                                               AddedBy = c.AddedBy,
                                               AddedDate = c.AddedDate,
                                               UpdatedDate = c.UpdatedDate,
                                               CityName = ct.CityName,
                                               AssignedLead = c.AssignedLead,
                                               SalesManagerName = cd.UserName

                                           }).ToList();

                    lstsaleslead.AddRange(lstsalesleadold);
                    if (objCust.LoginType != "1" && objCust.LoginType != "5")
                    {
                        lstsaleslead = lstsaleslead.Where(x => x.AssignedLead == objCust.LoginId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetFollowupLeads");
            }
            return lstsaleslead;
        }
        public List<SalesLead> GetSalesLeads(CustomerLoginDetail objCust)
        {
            List<SalesLead> lstsaleslead = new List<SalesLead>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            try
            {
                using (var context = new CommonDBContext())
                {
                    // lstsaleslead = context.SALES_tblLeads.Where(x => x.FollowupDate == today || x.FollowupDate == tommrowdt).ToList();

                    lstsaleslead = (from c in context.SALES_tblLeads
                                    join ct in context.tblCities on c.City equals ct.CityId.ToString()
                                    join cd in context.CustomerLoginDetails on c.AssignedLead equals cd.LoginId
                                    join bp in context.tblBillingPartners on c.BillingPartner equals bp.BillingPartnerId.ToString()
                                    into ps
                                    from bp in ps.DefaultIfEmpty()
                                    where (c.FollowupDate == today || c.FollowupDate == tommrowdt)
                                    select new SalesLead
                                    {
                                        LeadId = c.LeadId,
                                        BusinessName = c.BusinessName,
                                        Category = c.Category,
                                        Product = c.Product,
                                        BillingPartner = bp.BillingPartnerName,
                                        NoOfOutlet = c.NoOfOutlet,
                                        Address = c.Address,
                                        State = c.State,
                                        City = c.City,
                                        Pincode = c.Pincode,
                                        ContactType = c.ContactType,
                                        SpokeWith = c.SpokeWith,
                                        MobileNo = c.MobileNo,
                                        AlternateNo = c.AlternateNo,
                                        EmailId = c.EmailId,
                                        AuthorizedPerson = c.AuthorizedPerson,
                                        APMobileNo = c.APMobileNo,
                                        LeadStatus = c.LeadStatus,
                                        PriceQuoted = c.PriceQuoted,
                                        MeetingType = c.MeetingType,
                                        FollowupDate = c.FollowupDate,
                                        LeadSource = c.LeadSource,
                                        LeadSourceName = c.LeadSourceName,
                                        Comments = c.Comments,
                                        AddedBy = c.AddedBy,
                                        AddedDate = c.AddedDate,
                                        UpdatedDate = c.UpdatedDate,
                                        CityName = ct.CityName,
                                        AssignedLead = c.AssignedLead,
                                        SalesManagerName = cd.UserName

                                    }).ToList();


                    if (objCust.LoginType != "1" && objCust.LoginType != "5")
                    {
                        lstsaleslead = lstsaleslead.Where(x => x.AssignedLead == objCust.LoginId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSalesLeads");
            }
            return lstsaleslead;
        }
        public List<SelectListItem> GetSalesManager()
        {
            List<SelectListItem> lstSalesManager = new List<SelectListItem>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var SalesManagers = context.CustomerLoginDetails.Where(x => x.UserStatus == true && (x.LoginType == "8" || x.LoginType == "5" || x.LoginType == "9")).ToList();

                    foreach (var item in SalesManagers)
                    {
                        lstSalesManager.Add(new SelectListItem
                        {
                            Text = item.UserName,
                            Value = Convert.ToString(item.LoginId)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSalesManager");
            }
            return lstSalesManager;
        }

        public List<SalesLead> GetSearchedLeads(string MobileNo, string BusinessName, string FrmDate, string ToDate, string LeadStatus,
            string ContactType, string MeetingType, string City, string BillingPartner, string SalesManager,string LeadType)
        {
            List<SalesLead> lstLeads = new List<SalesLead>();
            List<SalesLead> lstsaleslead = new List<SalesLead>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            try
            {
                using (var context = new CommonDBContext())
                {

                    lstLeads = (from c in context.SALES_tblLeads
                                join ct in context.tblCities on c.City equals ct.CityId.ToString()
                                join cd in context.CustomerLoginDetails on c.AssignedLead equals cd.LoginId
                                join bp in context.tblBillingPartners on c.BillingPartner equals bp.BillingPartnerId.ToString()
                                into ps
                                from bp in ps.DefaultIfEmpty()
                                select new SalesLead
                                {
                                    LeadId = c.LeadId,
                                    BusinessName = c.BusinessName,
                                    Category = c.Category,
                                    Product = c.Product,
                                    BillingPartner = bp.BillingPartnerName,
                                    NoOfOutlet = c.NoOfOutlet,
                                    Address = c.Address,
                                    State = c.State,
                                    City = c.City,
                                    Pincode = c.Pincode,
                                    ContactType = c.ContactType,
                                    SpokeWith = c.SpokeWith,
                                    MobileNo = c.MobileNo,
                                    AlternateNo = c.AlternateNo,
                                    EmailId = c.EmailId,
                                    AuthorizedPerson = c.AuthorizedPerson,
                                    APMobileNo = c.APMobileNo,
                                    LeadStatus = c.LeadStatus,
                                    PriceQuoted = c.PriceQuoted,
                                    MeetingType = c.MeetingType,
                                    FollowupDate = c.FollowupDate,
                                    LeadSource = c.LeadSource,
                                    LeadSourceName = c.LeadSourceName,
                                    Comments = c.Comments,
                                    AddedBy = c.AddedBy,
                                    AddedDate = c.AddedDate,
                                    UpdatedDate = c.UpdatedDate,
                                    CityName = ct.CityName,
                                    AssignedLead = c.AssignedLead,
                                    SalesManagerName = cd.UserName,
                                    LeadType = c.LeadType
                                }).ToList();

                    if (!string.IsNullOrEmpty(MobileNo))
                    {
                        lstLeads = lstLeads.Where(x => x.MobileNo == MobileNo).ToList();
                    }
                    if (!string.IsNullOrEmpty(BusinessName))
                    {
                        lstLeads = lstLeads.Where(x => x.BusinessName.ToLower().Contains(BusinessName.ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(LeadStatus) && LeadStatus != "Please Select")
                    {
                        lstLeads = lstLeads.Where(x => x.LeadStatus == LeadStatus).ToList();
                    }
                    if (!string.IsNullOrEmpty(ContactType) && ContactType != "Please Select")
                    {
                        lstLeads = lstLeads.Where(x => x.ContactType == ContactType).ToList();
                    }
                    if (!string.IsNullOrEmpty(MeetingType) && MeetingType != "Please Select")
                    {
                        lstLeads = lstLeads.Where(x => x.MeetingType == MeetingType).ToList();
                    }
                    if (!string.IsNullOrEmpty(City) && City != "Please Select")
                    {
                        lstLeads = lstLeads.Where(x => x.City == City).ToList();
                    }
                    if (!string.IsNullOrEmpty(BillingPartner) && BillingPartner != "Please Select")
                    {
                        lstLeads = lstLeads.Where(x => x.BillingPartner == BillingPartner).ToList();
                    }
                    if (!string.IsNullOrEmpty(SalesManager) && SalesManager != "Please Select")
                    {
                        lstLeads = lstLeads.Where(x => x.AssignedLead == SalesManager).ToList();

                    }
                    if (!string.IsNullOrEmpty(FrmDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        DateTime frmdt = Convert.ToDateTime(FrmDate).Date;
                        DateTime todt = Convert.ToDateTime(ToDate).Date;
                        lstLeads = lstLeads.Where(x => x.AddedDate >= frmdt && x.AddedDate <= todt).ToList();
                    }
                    if (!string.IsNullOrEmpty(LeadType))
                    {
                        lstLeads = lstLeads.Where(x => x.LeadType == LeadType).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSearchedLeads");
            }
            return lstLeads;
        }

        public List<LeadTracking> GetLeadTracking(string LeadId)
        {
            List<LeadTracking> lstLeadTracking = new List<LeadTracking>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    var Id = Convert.ToInt32(LeadId);

                    lstLeadTracking = (from c in context.SALES_tblLeadTracking
                                       join ct in context.CustomerLoginDetails on c.AddedBy equals ct.LoginId
                                       where c.LeadId == Id
                                       select new LeadTracking
                                       {
                                           AddedDate = c.AddedDate,
                                           AddedByName = ct.UserName,
                                           ContactType = c.ContactType,
                                           MeetingType = c.MeetingType,
                                           LeadStatus = c.LeadStatus,
                                           Comments = c.Comments
                                       }).AsNoTracking().OrderByDescending(x => x.AddedDate).ToList();

                    foreach (var item in lstLeadTracking)
                    {
                        if (item.AddedDate.HasValue)
                            item.AddedDateStr = item.AddedDate.Value.ToString("MM/dd/yyyy h:mmtt");
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLeadTracking");
            }
            return lstLeadTracking;
        }
        public bool UpdateStatus(int LeadId, string GroupId)
        {
            bool status = false;
            SALES_tblLeads objlead = new SALES_tblLeads();
            SALES_tblLeadTracking objsalestracking = new SALES_tblLeadTracking();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objlead = context.SALES_tblLeads.Where(x => x.LeadId == LeadId).FirstOrDefault();
                    objlead.UpdatedDate = DateTime.Now;
                    objlead.MeetingType = "salesdone";
                    objlead.GroupId = GroupId;
                    context.SALES_tblLeads.AddOrUpdate(objlead);
                    context.SaveChanges();

                    objsalestracking.LeadId = objlead.LeadId;
                    objsalestracking.ContactType = objlead.ContactType;
                    objsalestracking.SpokeWith = objlead.SpokeWith;
                    objsalestracking.LeadStatus = objlead.LeadStatus;
                    objsalestracking.MeetingType = "salesdone";

                    objsalestracking.FollowupDate = objlead.FollowupDate;
                    objsalestracking.BillingPartner = objlead.BillingPartner;
                    objsalestracking.NoOfOutlet = objlead.NoOfOutlet;
                    objsalestracking.EcomIntegration = objlead.EcomIntegration;
                    objsalestracking.Address = objlead.Address;
                    objsalestracking.State = objlead.State;
                    objsalestracking.AlternateNo = objlead.AlternateNo;
                    objsalestracking.EmailId = objlead.EmailId;
                    objsalestracking.AuthorizedPerson = objlead.AuthorizedPerson;
                    objsalestracking.APMobileNo = objlead.APMobileNo;
                    objsalestracking.PriceQuoted = objlead.PriceQuoted;
                    objsalestracking.LeadSource = objlead.LeadSource;
                    objsalestracking.LeadSourceName = objlead.LeadSourceName;
                    objsalestracking.Comments = objlead.Comments;
                    objsalestracking.AddedBy = objlead.AddedBy;
                    objsalestracking.AddedDate = DateTime.Now;

                    context.SALES_tblLeadTracking.AddOrUpdate(objsalestracking);
                    context.SaveChanges();
                    status = true;

                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UpdateStatus");
            }
            return status;

        }
        public bool LeadTransfer(int[] LeadId, string SaleManagerId, string addedby)
        {
            bool result = false;
            try
            {
                SALES_tblLeads objsaleslead = new SALES_tblLeads();
                SALES_tblLeadTracking objsalesleadtracking = new SALES_tblLeadTracking();
                // string connStr = objCustRepo.GetCustomerConnString(GroupId);
                using (var contextNew = new CommonDBContext())
                {

                    foreach (int leadid in LeadId)
                    {
                        if (leadid != 0)
                        {
                            objsaleslead = contextNew.SALES_tblLeads.Where(x => x.LeadId == leadid).FirstOrDefault();
                            objsaleslead.AssignedLead = SaleManagerId;
                            objsaleslead.UpdatedDate = DateTime.Now;
                            contextNew.SALES_tblLeads.AddOrUpdate(objsaleslead);
                            contextNew.SaveChanges();
                            objsalesleadtracking = contextNew.SALES_tblLeadTracking.Where(x => x.LeadId == leadid).FirstOrDefault();
                            if (objsalesleadtracking != null)
                            {
                                objsalesleadtracking.AssignedLead = SaleManagerId;
                                objsalesleadtracking.AddedBy = addedby;
                                objsalesleadtracking.AddedDate = DateTime.Now;
                                contextNew.SALES_tblLeadTracking.AddOrUpdate(objsalesleadtracking);
                                contextNew.SaveChanges();
                            }
                            result = true;
                        }
                    }
                    //}

                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "LeadTransfer");
            }
            return result;
        }
        public List<SalesCount> GetSalesCounts(DateTime Fromdt, DateTime Todt, string salesmanager)
        {
            // List<salesCountDetails> lstsalescountdetailes = new List<salesCountDetails>();
            SalesCount objsalescount = new SalesCount();
            salesCountDetails objsalesCountDetails = new salesCountDetails();
            List<salesCountDetails> lstsalesCountDetails = new List<salesCountDetails>();
            List<SalesCount> lstsalescount = new List<SalesCount>();
            List<SALES_tblLeadTracking> lstleadtracking = new List<SALES_tblLeadTracking>();
            List<SALES_tblLeads> lstleads = new List<SALES_tblLeads>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (!string.IsNullOrEmpty(salesmanager))
                    {
                        objsalescount.NoOfMeeting = context.SALES_tblLeadTracking.Where(x => x.MeetingType == "1stMeeting" && x.AddedDate >= Fromdt && x.AddedDate <= Todt && x.AssignedLead == salesmanager).Count();

                        decimal? totalamt = (decimal?)0.00;
                        var grouprecord = (from d in context.BOTS_TblDealDetails
                                           join g in context.BOTS_TblGroupMaster
                                           on d.GroupId equals g.GroupId
                                           join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                           where g.CreatedBy == salesmanager && g.CreatedDate >= Fromdt && g.CreatedDate <= Todt && g.CustomerStatus != "Draft"
                                           select new
                                           {
                                               GroupId = d.GroupId,
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
                                               createdby = g.CreatedBy

                                           }).Distinct().ToList();
                        foreach (var itemgrp in grouprecord)
                        {
                            if (itemgrp.PaymentFrequency == "2")
                            {
                                totalamt = totalamt + itemgrp.AdvanceAmount;
                                //Octaxstotalamt = TotalAmount;
                            }
                            else
                            {
                                totalamt = totalamt + itemgrp.AmountReceived;
                                // OctaPlustotalamt = TotalAmount;
                            }
                        }

                        objsalescount.TotalAmount = totalamt;
                        objsalescount.NoOfSalesDone = grouprecord.Count();
                        objsalescount.NoofEnrolledOutlet = grouprecord.Select(x => x.Noofoutlets).Sum();
                        objsalescount.ratio = objsalescount.NoofEnrolledOutlet / objsalescount.NoOfSalesDone;
                        objsalescount.octaxs = grouprecord.Where(x => x.Boproduct == "2").Count();
                        objsalescount.octaplus = grouprecord.Where(x => x.Boproduct == "1").Count();

                        objsalescount.NoOfBillingpartner = (from s in context.SALES_tblLeads
                                                            join r in context.BOTS_TblRetailMaster on
                                                            s.GroupId equals r.GroupId
                                                            where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                                            select new { r.BillingPartner }).Distinct().Count();

                        objsalescount.lstSalesCountDetail = (from s in context.SALES_tblLeads
                                                             join ct in context.CustomerLoginDetails on s.AddedBy equals ct.LoginId
                                                             join r in context.BOTS_TblRetailMaster on
                                                             s.GroupId equals r.GroupId
                                                             join g in context.BOTS_TblGroupMaster
                                                             on r.GroupId equals g.GroupId
                                                             join d in context.BOTS_TblDealDetails on
                                                             r.GroupId equals d.GroupId
                                                             join b in context.tblBillingPartners on
                                                             s.BillingPartner equals b.BillingPartnerId.ToString()
                                                             where (g.CreatedBy == salesmanager && g.CreatedDate >= Fromdt && g.CreatedDate <= Todt && g.CustomerStatus != "Draft")
                                                             select new salesCountDetails
                                                             {
                                                                 LeadId = s.LeadId,
                                                                 SalesManager = ct.UserName,
                                                                 BusinessName = r.BrandName,
                                                                 PaymentFrequency = d.PaymentFrequency,
                                                                 Product = r.BOProduct,
                                                                 BillingPartner = r.BillingProduct,
                                                                 OutletName = r.NoOfEnrolled,
                                                                 AdvanceAmount = d.AdvanceAmount,
                                                                 AmountReceived = d.AmountReceived

                                                             }).ToList();



                    }
                    else
                    {
                        objsalescount.NoOfMeeting = context.SALES_tblLeadTracking.Where(x => x.MeetingType == "1stMeeting" && x.AddedDate >= Fromdt && x.AddedDate <= Todt).Count();

                        decimal? totalamt = (decimal?)0.00;
                        var grouprecord = (from d in context.BOTS_TblDealDetails
                                           join g in context.BOTS_TblGroupMaster
                                           on d.GroupId equals g.GroupId
                                           join r in context.BOTS_TblRetailMaster on g.GroupId equals r.GroupId
                                           where g.CreatedDate >= Fromdt && g.CreatedDate <= Todt && g.CustomerStatus != "Draft"
                                           select new
                                           {
                                               GroupId = d.GroupId,
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
                                               createddate = g.CreatedDate

                                           }).Distinct().ToList();
                        foreach (var itemgrp in grouprecord)
                        {
                            if (itemgrp.PaymentFrequency == "2")
                            {
                                totalamt = totalamt + itemgrp.AdvanceAmount;
                                //Octaxstotalamt = TotalAmount;
                            }
                            else
                            {
                                totalamt = totalamt + itemgrp.AmountReceived;
                                // OctaPlustotalamt = TotalAmount;
                            }
                        }

                        objsalescount.TotalAmount = totalamt;
                        objsalescount.NoOfSalesDone = grouprecord.Count();
                        objsalescount.NoofEnrolledOutlet = grouprecord.Select(x => x.Noofoutlets).Sum();
                        objsalescount.ratio = objsalescount.NoofEnrolledOutlet / objsalescount.NoOfSalesDone;

                        objsalescount.octaxs = grouprecord.Where(x => x.Boproduct == "2").Count();
                        objsalescount.octaplus = grouprecord.Where(x => x.Boproduct == "1").Count();

                        objsalescount.NoOfBillingpartner = (from s in context.SALES_tblLeads
                                                            join r in context.BOTS_TblRetailMaster on
                                                            s.GroupId equals r.GroupId
                                                            where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                                            select new { r.BillingPartner }).Distinct().Count();

                        objsalescount.lstSalesCountDetail = (from s in context.SALES_tblLeads
                                                             join ct in context.CustomerLoginDetails on s.AddedBy equals ct.LoginId
                                                             join r in context.BOTS_TblRetailMaster on
                                                             s.GroupId equals r.GroupId
                                                             join g in context.BOTS_TblGroupMaster
                                                             on r.GroupId equals g.GroupId
                                                             join d in context.BOTS_TblDealDetails on
                                                             r.GroupId equals d.GroupId
                                                             join b in context.tblBillingPartners on
                                                             s.BillingPartner equals b.BillingPartnerId.ToString()
                                                             where (g.CreatedDate >= Fromdt && g.CreatedDate <= Todt && g.CustomerStatus != "Draft")
                                                             select new salesCountDetails
                                                             {
                                                                 LeadId = s.LeadId,
                                                                 SalesManager = ct.UserName,
                                                                 BusinessName = r.BrandName,
                                                                 PaymentFrequency = d.PaymentFrequency,
                                                                 Product = r.BOProduct,
                                                                 BillingPartner = r.BillingProduct,
                                                                 OutletName = r.NoOfEnrolled,
                                                                 AdvanceAmount = d.AdvanceAmount,
                                                                 AmountReceived = d.AmountReceived
                                                             }).ToList();
                    }
                }
                lstsalescount.Add(objsalescount);


            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSalesCounts");
            }
            return lstsalescount;
        }

        public bool isMobileNoExist(string MobileNo)
        {
            bool status = false;
            try
            {
                using (var contextNew = new CommonDBContext())
                {
                    var Lead = contextNew.SALES_tblLeads.Where(x => x.MobileNo == MobileNo).FirstOrDefault();
                    if (Lead != null)
                    {
                        if (!string.IsNullOrEmpty(Lead.MobileNo))
                        {
                            status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "isMobileNoExist");
            }
            return status;
        }

        public List<SelectListItem> GetRefferedName(string SourceType)
        {
            List<SelectListItem> lstRefferedName = new List<SelectListItem>();           
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (SourceType == "CustomerReference")
                    {
                        var CustomerList = context.tblGroupDetails.ToList();
                        foreach (var item in CustomerList)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.GroupName,
                                Value = item.GroupName
                            });
                        }
                    }
                    if (SourceType == "billingpartner")
                    {
                        var BillingPartners = context.tblBillingPartners.ToList();
                        foreach (var item in BillingPartners)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.BillingPartnerName,
                                Value = item.BillingPartnerName
                            });
                        }
                    }
                    if (SourceType == "channelpartner")
                    {
                        var ChannelPartners = context.tblChannelPartners.ToList();
                        foreach (var item in ChannelPartners)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.CPartnerName,
                                Value = Convert.ToString(item.CPId)
                            });
                        }
                    }
                    if (SourceType == "employeereferral")
                    {
                        var ChannelPartners = context.CustomerLoginDetails.Where(x => x.UserStatus == true && x.UserId == "1000" && x.LoginType != "5" && x.LoginType != "8" && x.LoginType != null).ToList();
                        foreach (var item in ChannelPartners)
                        {
                            lstRefferedName.Add(new SelectListItem
                            {
                                Text = item.UserName,
                                Value = Convert.ToString(item.LoginId)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetRefferedName");
            }
            lstRefferedName = lstRefferedName.OrderBy(x => x.Text).ToList();
            return lstRefferedName;
        }

        public List<NewReport> GetNewReport(string SalesManager, string City, string Category, string BillingPartner, string LeadSource, string LeadStatus,string LoginType,string UserName)
        {
            List<NewReport> lstData = new List<NewReport>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstData = context.Database.SqlQuery<NewReport>("sp_SalesLeadReport").ToList();
                    if (LoginType == "8")
                    {
                        lstData = lstData.Where(x => x.SalesManager == UserName).ToList();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(SalesManager) && SalesManager != "Please Select")
                        {
                            lstData = lstData.Where(x => x.SalesManager == SalesManager).ToList();
                        }
                        if (!string.IsNullOrEmpty(City) && City != "Please Select")
                        {
                            lstData = lstData.Where(x => x.CityName == City).ToList();
                        }
                        if (!string.IsNullOrEmpty(Category) && Category != "Please Select")
                        {
                            lstData = lstData.Where(x => x.CategoryName == Category).ToList();
                        }
                        if (!string.IsNullOrEmpty(BillingPartner) && BillingPartner != "Please Select")
                        {
                            lstData = lstData.Where(x => x.BillingPartnerName == BillingPartner).ToList();
                        }
                        if (!string.IsNullOrEmpty(LeadSource))
                        {
                            lstData = lstData.Where(x => x.LeadSource == LeadSource).ToList();
                        }
                        if (!string.IsNullOrEmpty(LeadStatus))
                        {
                            lstData = lstData.Where(x => x.LeadStatus == LeadStatus).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNewReport");
            }

            return lstData;
        }
    }
}
