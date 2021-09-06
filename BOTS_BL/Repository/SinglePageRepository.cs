using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
                    lstnontransactingoutlet = context.Tbl_SinglePageNonTransactingOutlet.OrderByDescending(i => i.DaysSinceLastTxn).ToList();
                    foreach (var item in lstnontransactingoutlet)
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
                    lstsummarytable.TotalEnrolledBase = totalenroll;
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

        public CommunicationsinglePageData GetCommunicationWhatsAppExpiryData()
        {
            CommunicationsinglePageData lstSmsbalance = new CommunicationsinglePageData();
            DateTime next10day = DateTime.Now.AddDays(10);
            List<SMSBalance> lstbalance = new List<SMSBalance>();

            DataSet retVal = new DataSet();

            try
            {
                SqlConnection sqlConn = new SqlConnection("Data Source=13.233.128.61;Initial Catalog=CommonDBLoyalty;user id = sa; password=BO%Admin#LY!4@");

                SqlCommand cmdReport = new SqlCommand("sp_GetCommunicationDataforSinglePage", sqlConn);
                SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
                using (cmdReport)
                {
                    cmdReport.CommandType = CommandType.StoredProcedure;
                    daReport.Fill(retVal);
                }
                if(retVal!=null)
                {   
                    lstSmsbalance.objSMSBalance = ConvertDataTable<SMSBalance>(retVal.Tables[0]);
                    lstSmsbalance.objWhatsAppBalance = ConvertDataTable<WhatsAppBalance>(retVal.Tables[1]);
                    lstSmsbalance.objVirtualSMSBalance = ConvertDataTable<VirtualSMSBalance>(retVal.Tables[2]);
                    lstSmsbalance.objWhatsAppExpiryDate = ConvertDataTable<WhatsAppExpiryDate>(retVal.Tables[3]);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }

            return lstSmsbalance;
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
