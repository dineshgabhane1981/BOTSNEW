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
        public bool AddSalesLead(SALES_tblLeads objtbllead)
        {
            SALES_tblLeads objlead = new SALES_tblLeads();
            SALES_tblLeadTracking objsalestracking = new SALES_tblLeadTracking();
            bool status = false;
            using (var context = new CommonDBContext())
            {
                if (objtbllead.LeadId == 0)
                {
                    context.SALES_tblLeads.Add(objtbllead);
                    context.SaveChanges();
                    status = true;
                }
                else
                {
                    objlead = context.SALES_tblLeads.Where(x => x.LeadId == objtbllead.LeadId).FirstOrDefault();
                     objtbllead.UpdatedDate = DateTime.Now;
                    objtbllead.Category = objlead.Category;
                    objtbllead.Product = objlead.Product;
                    objtbllead.City = objlead.City;
                    context.SALES_tblLeads.AddOrUpdate(objtbllead);
                    context.SaveChanges();
                    objsalestracking.LeadId = objlead.LeadId;
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
                    context.SALES_tblLeadTracking.AddOrUpdate(objsalestracking);
                    context.SaveChanges();
                    status = true;
                }
            }
                return status;
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
            using(var context = new CommonDBContext())
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
                var SalesManagers = context.CustomerLoginDetails.Where(x=>x.LoginType == "8").ToList();

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

        public List<SalesLead> GetSearchedLeads()
        {
            List<SalesLead> lstLeads = new List<SalesLead>();



            return lstLeads;
        }
    }
}
