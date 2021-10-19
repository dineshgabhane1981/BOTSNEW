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
            bool status = false;
            using (var context = new CommonDBContext())
            {
                context.SALES_tblLeads.Add(objtbllead);
                context.SaveChanges();
                status = true;


            }
                return status;
        }
    }
}
