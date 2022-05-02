using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class OtherReportsRepository
    {
        Exceptions newexception = new Exceptions();
        public List<SellingProductValue> GetTop5SellingProductValue(string GroupId, string connstr)
        {
            List<SellingProductValue> lstTop5SessingProductValue = new List<SellingProductValue>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lstTop5SessingProductValue = context.Database.SqlQuery<SellingProductValue>("sp_BOTS_Top5_SellingProductValue").ToList<SellingProductValue>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return lstTop5SessingProductValue;
        }

        public List<SellingProductValue> GetBottom5SellingProductValue(string GroupId, string connstr)
        {
            List<SellingProductValue> lstBottom5SessingProductValue = new List<SellingProductValue>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    lstBottom5SessingProductValue = context.Database.SqlQuery<SellingProductValue>("sp_BOTS_Bottom5_SellingProductValue").ToList<SellingProductValue>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return lstBottom5SessingProductValue;
        }
        public List<ReportForDownload> GetReportDownloadData(string groupId)
        {
            List<ReportForDownload> lstReportDownload = new List<ReportForDownload>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstReportDownload = context.ReportForDownloads.Where(x=>x.GroupId== groupId).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetReportDownloadData");
            }

            return lstReportDownload;
        }
    }
}
