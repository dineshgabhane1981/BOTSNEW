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

namespace BOTS_BL.Repository
{
   public class SalesLeadRepository
   {
        Exceptions newexception = new Exceptions();
        public bool AddSalesLead(SALES_tblLeads objtbllead)
        {
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

                    context.SALES_tblLeads.AddOrUpdate(objtbllead);
                    context.SaveChanges();
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
                    context.SALES_tblLeadTracking.AddOrUpdate(objsalestracking);
                    context.SaveChanges();

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
        public List<SALES_tblLeads> GetSalesLeads()
        {
            List<SALES_tblLeads> lstsaleslead = new List<SALES_tblLeads>();
            DateTime today = DateTime.Today;
            DateTime tommrowdt = today.AddDays(1);
            using(var context = new CommonDBContext())
            {
                lstsaleslead = context.SALES_tblLeads.Where(x => x.FollowupDate == today || x.FollowupDate == tommrowdt).ToList();
            }
            return lstsaleslead;
        }

   }
}
