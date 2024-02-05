using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Threading;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using BOTS_BL.Models.IndividualDBModels;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class CouponRepository
    {
        Exceptions newexception = new Exceptions();
        public List<tblCouponUpload> GetAllCouponUpload(string conStr)
        {
            List<tblCouponUpload> lstData = new List<tblCouponUpload>();
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    lstData = context.tblCouponUploads.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetAllCouponUpload");
            }
            return lstData;
        }
        public bool UploadCouponData(List<tblCouponMapping> lstData, tblCouponUpload objUploadData, string conStr)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    context.tblCouponUploads.Add(objUploadData);
                    context.SaveChanges();
                    foreach (var item in lstData)
                    {
                        context.tblCouponMappings.Add(item);
                        context.SaveChanges();
                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UploadCouponData");
            }

            return status;
        }
    
        public bool SaveCouponEarnRule(tblCouponRule objData,string conStr)
        {
            bool result = false;
            try
            {
                using (var context = new BOTSDBContext(conStr))
                {
                    context.tblCouponRules.AddOrUpdate(objData);
                    context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveCouponEarnRule");
            }

            return result;
        }
    
    }
}
