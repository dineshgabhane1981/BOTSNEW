using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;

namespace BOTS_BL.Repository
{
    public class SinglePageRepository
    {
        Exceptions newexception = new Exceptions();
        public List<Tbl_SinglePageNonTransactingGroup> GetSinglePageNonTransactingGroups()
        {
            List<Tbl_SinglePageNonTransactingGroup> lstnontransactinggrp = new List<Tbl_SinglePageNonTransactingGroup>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //var query = from obj in context.Tbl_SinglePageNonTransactingGroup
                    //            where SqlFunctions.Format(obj.Number, "0.00").Contains("03.1")
                    //            select obj;
                    
                    lstnontransactinggrp = context.Tbl_SinglePageNonTransactingGroup.OrderByDescending(p => p.DaysSinceLastTxn).ToList();
                    foreach (var item in lstnontransactinggrp)
                    {
                        item.DaySinceLastTxn = Convert.ToInt32(item.DaysSinceLastTxn);
                    }
                    lstnontransactinggrp = lstnontransactinggrp.OrderByDescending(x => x.DaySinceLastTxn).ToList();
                }
            }
            catch (Exception ex)
            {
               // newexception.AddException(ex, GroupId);
            }
            
            return lstnontransactinggrp;
        }
        public List<Tbl_SinglePageNonTransactingOutlet> GetNonTransactingOutlet()
        {
            List<Tbl_SinglePageNonTransactingOutlet> lstnontransactingoutlet = new List<Tbl_SinglePageNonTransactingOutlet>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstnontransactingoutlet = context.Tbl_SinglePageNonTransactingOutlet.OrderByDescending(i =>i.DaysSinceLastTxn).ToList();
                    foreach(var item in lstnontransactingoutlet)
                    {
                        item.DaySinceLastTxn = Convert.ToInt32(item.DaysSinceLastTxn);
                    }
                    lstnontransactingoutlet = lstnontransactingoutlet.OrderByDescending(x => x.DaySinceLastTxn).ToList();
                }
            }
            catch (Exception ex)
            {
                // newexception.AddException(ex, GroupId);
            }

            return lstnontransactingoutlet;
        }

        public List<Tbl_SinglePageLowTransactingOutlet> GetLowTransactingOutlet()
        {
            List<Tbl_SinglePageLowTransactingOutlet> lstlowtransactingoutlet = new List<Tbl_SinglePageLowTransactingOutlet>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    lstlowtransactingoutlet = context.Tbl_SinglePageLowTransactingOutlet.OrderBy(i => i.LowerByPercentage).ToList();
                }
            }
            catch (Exception ex)
            {
                // newexception.AddException(ex, GroupId);
            }

            return lstlowtransactingoutlet;
        }
        public Tbl_SinglePageSummaryTable GetSinglePageSummaryTable()
        {
            Tbl_SinglePageSummaryTable lstsummarytable = new Tbl_SinglePageSummaryTable();
            try
            {
                using (var context = new CommonDBContext())
                {
                   // lstsummarytable = context.Tbl_SinglePageSummaryTable.ToList();
                    var totalenroll = context.Tbl_SinglePageSummaryTable.Sum(i => i.TotalEnrolledBase);
                    var sumtxncountdaily = context.Tbl_SinglePageSummaryTable.Sum(i => i.TxnCountDaily);
                    var sumtxncountmtd = context.Tbl_SinglePageSummaryTable.Sum(i => i.TxnCountMTD);
                    lstsummarytable.TotalEnrolledBase= totalenroll;
                    lstsummarytable.TxnCountDaily = sumtxncountdaily;
                    lstsummarytable.TxnCountMTD = sumtxncountmtd;

                }
            }
            catch (Exception ex)
            {
                // newexception.AddException(ex, GroupId);
            }

            return lstsummarytable;
        }

        public List<CommunicationsinglePageData> GetCommunicationWhatsAppExpiryData()
        {
            List<CommunicationsinglePageData> lstSmsbalance = new List<CommunicationsinglePageData>();
            DateTime next10day = DateTime.Now.AddDays(10);
            List<SMSBalance> lstbalance = new List<SMSBalance>();

            DataSet retVal = new DataSet();
            
            SqlConnection sqlConn = new SqlConnection("Data Source=13.233.128.61;Initial Catalog=CommonDBLoyalty;user id = sa; password=BO%Admin#LY!4@");

            SqlCommand cmdReport = new SqlCommand("sp_GetCommunicationDataforSinglePage", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            using (cmdReport)
            {                 
                cmdReport.CommandType = CommandType.StoredProcedure;                 
                daReport.Fill(retVal);
            }

            int count = 10;
            count = retVal.Tables.Count;
            int newcount = count + 10;
            try
            {
                using (var context = new CommonDBContext())
                {
                    newcount++;
                    //Int32 locID = Convert.ToInt32(SMSBalance);
                    // var id = context.SMSWABalanceData.Select(x => Convert.ToInt32(x.SMSBalance));
                    lstSmsbalance = context.Database.SqlQuery<CommunicationsinglePageData>("sp_GetCommunicationDataforSinglePage").ToList<CommunicationsinglePageData>();
                    //lstbalance = (from x in context.SMSWABalanceData
                    //    where int.Parse(x.SMSBalance) < 1000 && int.Parse(x.SMSBalance) != -1
                    //    select new SMSBalance{
                    //        BrandName =x.BrandName,
                    //        OutletName=x.OutletName,
                    //        SmsBalance=x.SMSBalance
                    //        }).ToList();
                    newcount = lstSmsbalance.Count();


                    newcount++;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }

            return lstSmsbalance;
        }
    }
}
