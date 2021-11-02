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
        public List<SalesLead> GetSalesLeads()
        {
            List<SalesLead> lstsaleslead = new List<SalesLead>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            using (var context = new CommonDBContext())
            {
                // lstsaleslead = context.SALES_tblLeads.Where(x => x.FollowupDate == today || x.FollowupDate == tommrowdt).ToList();

                lstsaleslead = (from c in context.SALES_tblLeads
                                join ct in context.tblCities on c.City equals ct.CityId.ToString()
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
                                    CityName = ct.CityName

                                }).ToList();
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
                                CityName = ct.CityName
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
                    lstLeads = lstLeads.Where(x => x.AddedBy == SalesManager).ToList();

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
                                   }).AsNoTracking().ToList();
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
    }
}
