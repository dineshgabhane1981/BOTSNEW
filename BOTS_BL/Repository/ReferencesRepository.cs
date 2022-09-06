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
using DocumentFormat.OpenXml.InkML;
using System.Runtime.Remoting.Contexts;

namespace BOTS_BL.Repository
{
    public class ReferencesRepository
    {
        public bool InsertReferal(tblReference objdata)
        {
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    objdata.Datetime = DateTime.Now;
                    context.tblReferences.AddOrUpdate(objdata);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
    }
}
