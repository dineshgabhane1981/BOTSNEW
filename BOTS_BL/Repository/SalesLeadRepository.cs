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
    public class SalesLeadRepository
    {
        Exceptions newexception = new Exceptions();
        public int AddSalesLead(SALES_tblLeads objtbllead)
        {
            SALES_tblLeads objlead = new SALES_tblLeads();
            SALES_tblLeadTracking objsalestracking = new SALES_tblLeadTracking();
            int leadId = 0;
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
            return leadId;
        }
        public SALES_tblLeads GetsalesLeadByLeadId(int LeadId)
        {
            SALES_tblLeads objsaleslead = new SALES_tblLeads();
            using (var context = new CommonDBContext())
            {
                objsaleslead = context.SALES_tblLeads.Where(x => x.LeadId == LeadId).FirstOrDefault();
            }
            return objsaleslead;
        }
        public List<SalesLead> GetSalesLeads(CustomerLoginDetail objCust)
        {
            List<SalesLead> lstsaleslead = new List<SalesLead>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            using (var context = new CommonDBContext())
            {
                // lstsaleslead = context.SALES_tblLeads.Where(x => x.FollowupDate == today || x.FollowupDate == tommrowdt).ToList();

                lstsaleslead = (from c in context.SALES_tblLeads
                                join ct in context.tblCities on c.City equals ct.CityId.ToString()
                                join cd in context.CustomerLoginDetails on c.AssignedLead equals cd.LoginId
                                where (c.FollowupDate == today || c.FollowupDate == tommrowdt)
                                select new SalesLead
                                {
                                    LeadId = c.LeadId,
                                    BusinessName = c.BusinessName,
                                    Category = c.Category,
                                    Product = c.Product,
                                    BillingPartner = c.BillingPartner,
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
                                    AssignedLead = cd.UserName

                                }).ToList();
                if (objCust.LoginType != "1" && objCust.LoginType != "5")
                {
                    lstsaleslead = lstsaleslead.Where(x => x.AssignedLead == objCust.LoginId).ToList();
                }
            }
            return lstsaleslead;
        }
        public List<SelectListItem> GetSalesManager()
        {
            List<SelectListItem> lstSalesManager = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                var SalesManagers = context.CustomerLoginDetails.Where(x => x.LoginType == "8").ToList();

                foreach (var item in SalesManagers)
                {
                    lstSalesManager.Add(new SelectListItem
                    {
                        Text = item.UserName,
                        Value = Convert.ToString(item.LoginId)
                    });
                }
            }
            return lstSalesManager;
        }

        public List<SalesLead> GetSearchedLeads(string MobileNo, string BusinessName, string FrmDate, string ToDate, string LeadStatus,
            string ContactType, string MeetingType, string City, string BillingPartner, string SalesManager)
        {
            List<SalesLead> lstLeads = new List<SalesLead>();
            List<SalesLead> lstsaleslead = new List<SalesLead>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            using (var context = new CommonDBContext())
            {

                lstLeads = (from c in context.SALES_tblLeads
                            join ct in context.tblCities on c.City equals ct.CityId.ToString()
                            join cd in context.CustomerLoginDetails on c.AssignedLead equals cd.LoginId
                            select new SalesLead
                            {
                                LeadId = c.LeadId,
                                BusinessName = c.BusinessName,
                                Category = c.Category,
                                Product = c.Product,
                                BillingPartner = c.BillingPartner,
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
                                AssignedLead = cd.UserName
                            }).ToList();

                if (!string.IsNullOrEmpty(MobileNo))
                {
                    lstLeads = lstLeads.Where(x => x.MobileNo == MobileNo).ToList();
                }
                if (!string.IsNullOrEmpty(BusinessName))
                {
                    lstLeads = lstLeads.Where(x => x.BusinessName == BusinessName).ToList();
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
            }


            return lstLeads;
        }

        public List<LeadTracking> GetLeadTracking(string LeadId)
        {
            List<LeadTracking> lstLeadTracking = new List<LeadTracking>();
            using (var context = new CommonDBContext())
            {
                var Id = Convert.ToInt32(LeadId);

                lstLeadTracking = (from c in context.SALES_tblLeadTracking
                                   join ct in context.CustomerLoginDetails on c.AddedBy equals ct.LoginId
                                   where c.LeadId == Id
                                   select new LeadTracking
                                   {
                                       AddedDate = c.AddedDate.ToString(),
                                       AddedByName = ct.UserName,
                                       ContactType = c.ContactType,
                                       MeetingType = c.MeetingType,
                                       LeadStatus = c.LeadStatus,
                                       Comments = c.Comments
                                   }).AsNoTracking().OrderByDescending(x => x.AddedDate).ToList();
            }
            return lstLeadTracking;
        }
        public bool UpdateStatus(int LeadId, string GroupId)
        {
            bool status = false;
            SALES_tblLeads objlead = new SALES_tblLeads();
            SALES_tblLeadTracking objsalestracking = new SALES_tblLeadTracking();
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
            using (var context = new CommonDBContext())
            {
                if (!string.IsNullOrEmpty(salesmanager))
                {
                    objsalescount.NoOfMeeting = context.SALES_tblLeadTracking.Where(x => x.MeetingType == "1stMeeting" && x.AddedDate >= Fromdt && x.AddedDate <= Todt && x.AssignedLead == salesmanager).Count();
                    objsalescount.NoOfSalesDone = context.SALES_tblLeads.Where(x => x.MeetingType == "salesdone" && x.UpdatedDate >= Fromdt && x.UpdatedDate <= Todt && x.AssignedLead == salesmanager).Count();


                    objsalescount.NoOfBrand = (from s in context.SALES_tblLeads
                                               join r in context.BOTS_TblRetailMaster on
                                               s.GroupId equals r.GroupId
                                               where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                               select new { s.GroupId }).Count();

                    objsalescount.NoOfOutlet = (from s in context.SALES_tblLeads
                                                join r in context.BOTS_TblRetailMaster on
                                                s.GroupId equals r.GroupId
                                                where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                                select new { r.NoOfEnrolled }).Count();

                    objsalescount.TotalAmount = (from s in context.SALES_tblLeads
                                                 join d in context.BOTS_TblDealDetails on
                                                 s.GroupId equals d.GroupId
                                                 where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                                 select new
                                                 {
                                                     d.TotalFeesA
                                                 }).Sum(x => x.TotalFeesA);
                    if (string.IsNullOrEmpty(Convert.ToString(objsalescount.TotalAmount)))
                    {
                        objsalescount.TotalAmount = 0;
                    }

                    objsalescount.octaplus = (from s in context.SALES_tblLeads
                                              join r in context.BOTS_TblRetailMaster on
                                              s.GroupId equals r.GroupId
                                              where (s.MeetingType == "salesdone" && r.BOProduct == "1" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                              select new { r.BOProduct }).Count();

                    objsalescount.octaxs = (from s in context.SALES_tblLeads
                                            join r in context.BOTS_TblRetailMaster on
                                            s.GroupId equals r.GroupId
                                            where (s.MeetingType == "salesdone" && r.BOProduct == "2" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                            select new { r.BOProduct }).Count();

                    objsalescount.NoOfBillingpartner = (from s in context.SALES_tblLeads
                                                        join r in context.BOTS_TblRetailMaster on
                                                        s.GroupId equals r.GroupId
                                                        where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                                        select new { r.BillingPartner }).Distinct().Count();

                    objsalescount.lstSalesCountDetail = (from s in context.SALES_tblLeads
                                                         join r in context.BOTS_TblRetailMaster on
                                                         s.GroupId equals r.GroupId
                                                         join d in context.BOTS_TblDealDetails on
                                                         r.GroupId equals d.GroupId
                                                         where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt && s.AssignedLead == salesmanager)
                                                         select new salesCountDetails
                                                         {
                                                             LeadId = s.LeadId,
                                                             BusinessName = r.BrandName,
                                                             Product = r.BOProduct,
                                                             BillingPartner = r.BillingProduct,
                                                             Amount = d.TotalFeesA,
                                                             OutletName = r.NoOfEnrolled

                                                         }).ToList();

                }
                else
                {
                    objsalescount.NoOfMeeting = context.SALES_tblLeadTracking.Where(x => x.MeetingType == "1stMeeting" && x.AddedDate >= Fromdt && x.AddedDate <= Todt).Count();
                    objsalescount.NoOfSalesDone = context.SALES_tblLeads.Where(x => x.MeetingType == "salesdone" && x.UpdatedDate >= Fromdt && x.UpdatedDate <= Todt).Count();


                    objsalescount.NoOfBrand = (from s in context.SALES_tblLeads
                                               join r in context.BOTS_TblRetailMaster on
                                               s.GroupId equals r.GroupId
                                               where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                               select new { s.GroupId }).Count();

                    objsalescount.NoOfOutlet = (from s in context.SALES_tblLeads
                                                join r in context.BOTS_TblRetailMaster on
                                                s.GroupId equals r.GroupId
                                                where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                                select new { r.NoOfEnrolled }).Count();

                    objsalescount.TotalAmount = (from s in context.SALES_tblLeads
                                                 join d in context.BOTS_TblDealDetails on
                                                 s.GroupId equals d.GroupId
                                                 where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                                 select new
                                                 {
                                                     d.TotalFeesA
                                                 }).Sum(x => x.TotalFeesA);
                    if (string.IsNullOrEmpty(Convert.ToString(objsalescount.TotalAmount)))
                    {
                        objsalescount.TotalAmount = 0;
                    }

                    objsalescount.octaplus = (from s in context.SALES_tblLeads
                                              join r in context.BOTS_TblRetailMaster on
                                              s.GroupId equals r.GroupId
                                              where (s.MeetingType == "salesdone" && r.BOProduct == "1" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                              select new { r.BOProduct }).Count();

                    objsalescount.octaxs = (from s in context.SALES_tblLeads
                                            join r in context.BOTS_TblRetailMaster on
                                            s.GroupId equals r.GroupId
                                            where (s.MeetingType == "salesdone" && r.BOProduct == "2" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                            select new { r.BOProduct }).Count();

                    objsalescount.NoOfBillingpartner = (from s in context.SALES_tblLeads
                                                        join r in context.BOTS_TblRetailMaster on
                                                        s.GroupId equals r.GroupId
                                                        where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                                        select new { r.BillingPartner }).Distinct().Count();

                    objsalescount.lstSalesCountDetail = (from s in context.SALES_tblLeads
                                                         join r in context.BOTS_TblRetailMaster on
                                                         s.GroupId equals r.GroupId
                                                         join d in context.BOTS_TblDealDetails on
                                                         r.GroupId equals d.GroupId
                                                         join b in context.tblBillingPartners on
                                                         s.BillingPartner equals b.BillingPartnerId.ToString()
                                                         where (s.MeetingType == "salesdone" && s.UpdatedDate >= Fromdt && s.UpdatedDate <= Todt)
                                                         select new salesCountDetails
                                                         {
                                                             LeadId = s.LeadId,
                                                             BusinessName = r.BrandName,
                                                             Product = r.BOProduct,
                                                             BillingPartner = b.BillingPartnerName,
                                                             Amount = d.TotalFeesA,
                                                             OutletName = r.NoOfEnrolled

                                                         }).ToList();
                }
            }
            lstsalescount.Add(objsalescount);

            return lstsalescount;
        }

        public bool isMobileNoExist(string MobileNo)
        {
            bool status = false;
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
            return status;
        }
    }
}
