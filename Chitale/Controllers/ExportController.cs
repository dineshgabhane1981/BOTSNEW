using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ClosedXML.Excel;
using System.IO;

namespace Chitale.Controllers
{
    public class ExportController : Controller
    {
        ChitaleException newexception = new ChitaleException();
        ParticipantRepository pr = new ParticipantRepository();
        PointsLedgerRepository PLR = new PointsLedgerRepository();
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        TgtVsAchRepository TAR = new TgtVsAchRepository();
        NoActionRepository NAR = new NoActionRepository();
        EmployeeRepository ER = new EmployeeRepository();
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }

        //Export Functions for Participant Category
        public ActionResult ExportParticipantList()
        {
            try
            {

                System.Data.DataTable table = new System.Data.DataTable();
                var UserSession = (CustomerDetail)Session["ChitaleUser"];

                List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
                lstparticipantLists = pr.GetParticipantList(UserSession.CustomerId, UserSession.Type);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ParticipantList));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                if (UserSession.Type == "SuperStockiest")
                {
                    foreach (ParticipantList item in lstparticipantLists)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);

                        List<ParticipantList> lstRetailerLists = new List<ParticipantList>();
                        lstRetailerLists = pr.GetParticipantList(item.Id, item.ParticipantType);
                        foreach (ParticipantList itemRetailer in lstRetailerLists)
                        {
                            DataRow row1 = table.NewRow();
                            foreach (PropertyDescriptor prop in properties)
                                row1[prop.Name] = prop.GetValue(itemRetailer) ?? DBNull.Value;
                            table.Rows.Add(row1);
                        }
                    }
                }
                else
                {
                    foreach (ParticipantList item in lstparticipantLists)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                        table.Rows.Add(row);
                    }
                }

                var count = table.Rows.Count;

                string ReportName = "Participant List";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    table.TableName = ReportName;
                    wb.Worksheets.Add(table);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }

        }

        public ActionResult ExportFocusVsAch(string Month, string Year)
        {
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                System.Data.DataTable tableToExport = new System.Data.DataTable();

                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("VolumeFocus");
                tableToExport.Columns.Add("VolumeAch");
                tableToExport.Columns.Add("VolumeAch%");
                tableToExport.Columns.Add("VolumePoints");
                tableToExport.Columns.Add("ValueFocus");
                tableToExport.Columns.Add("ValueAch");
                tableToExport.Columns.Add("ValueAch%");
                tableToExport.Columns.Add("ValuePoints");

                CustomerDetail UserSession = new CustomerDetail();
                UserSession = (CustomerDetail)Session["ChitaleUser"];

                TgtVsAchViewModel objData = new TgtVsAchViewModel();
                objData.objOverAll = PLR.GetOverallData(UserSession.CustomerId, Month, Year);
                objData.objCategory = PLR.GetCategoryData(UserSession.CustomerId, Month, Year);
                objData.objSubCategory = PLR.GetSubCategoryData(UserSession.CustomerId, Month, Year);
                objData.objProducts = PLR.GetProductData(UserSession.CustomerId, Month, Year);


                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(TgtvsAchMaster));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(objData.objOverAll) ?? DBNull.Value;
                table.Rows.Add(row);

                foreach (TgtvsAchMaster item1 in objData.objCategory)
                {
                    DataRow row1 = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row1[prop.Name] = prop.GetValue(item1) ?? DBNull.Value;

                    table.Rows.Add(row1);

                    foreach (TgtvsAchMaster item2 in objData.objSubCategory)
                    {
                        if (item1.CategoryCode == item2.CategoryCode)
                        {
                            DataRow row2 = table.NewRow();
                            foreach (PropertyDescriptor prop in properties)
                                row2[prop.Name] = prop.GetValue(item2) ?? DBNull.Value;

                            table.Rows.Add(row2);

                            foreach (TgtvsAchMaster item3 in objData.objProducts)
                            {
                                if (item2.SubCategoryCode == item3.SubCategoryCode)
                                {
                                    DataRow row3 = table.NewRow();
                                    foreach (PropertyDescriptor prop in properties)
                                        row3[prop.Name] = prop.GetValue(item3) ?? DBNull.Value;

                                    table.Rows.Add(row3);
                                }
                            }
                        }
                    }

                }

                foreach (DataRow item in table.Rows)
                {
                    DataRow newDr = tableToExport.NewRow();
                    newDr["Name"] = item["Name"];
                    newDr["Type"] = item["ProductType"];
                    newDr["VolumeFocus"] = item["VolumeTgt"];
                    newDr["VolumeAch"] = item["VolumeAch"];
                    newDr["VolumeAch%"] = item["VolumeAchPercentage"];
                    newDr["VolumePoints"] = item["VolumePoints"];
                    newDr["ValueFocus"] = item["ValueTgt"];
                    newDr["ValueAch"] = item["ValueAch"];
                    newDr["ValueAch%"] = item["ValueAchPercentage"];
                    newDr["ValuePoints"] = item["ValuePoints"];

                    tableToExport.Rows.Add(newDr);
                }

                string ReportName = "Focus Vs Ach";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }

            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }

        }


        //Export Functions for Management Category
        public ActionResult ExportParticipantListManagement(string Cluster, string SubCluster, string City)
        {
            try
            {
                int clusterId = 0;
                int subClusterId = 0;
                int cityId = 0;
                if (!string.IsNullOrEmpty(Cluster))
                {
                    clusterId = Convert.ToInt32(Cluster);
                }
                if (!string.IsNullOrEmpty(SubCluster))
                {
                    subClusterId = Convert.ToInt32(SubCluster);
                }
                if (!string.IsNullOrEmpty(City))
                {
                    cityId = Convert.ToInt32(City);
                }

                System.Data.DataTable table = new System.Data.DataTable();

                List<ParticipantListForManagement> lstSuperStockiest = new List<ParticipantListForManagement>();
                lstSuperStockiest = MDR.GetParticipantListForMgt(clusterId, subClusterId, cityId);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ParticipantListForManagement));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (ParticipantListForManagement item1 in lstSuperStockiest)
                {
                    DataRow row1 = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row1[prop.Name] = prop.GetValue(item1) ?? DBNull.Value;

                    table.Rows.Add(row1);

                    List<ParticipantListForManagement> lstDistributor = new List<ParticipantListForManagement>();
                    lstDistributor = MDR.GetSubParticipantListForMgt(item1.Id, item1.ParticipantType);
                    foreach (ParticipantListForManagement item2 in lstDistributor)
                    {
                        DataRow row2 = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row2[prop.Name] = prop.GetValue(item2) ?? DBNull.Value;

                        table.Rows.Add(row2);

                        List<ParticipantListForManagement> lstRetailer = new List<ParticipantListForManagement>();
                        lstRetailer = MDR.GetSubParticipantListForMgt(item2.Id, item2.ParticipantType);
                        foreach (ParticipantListForManagement item3 in lstRetailer)
                        {
                            DataRow row3 = table.NewRow();
                            foreach (PropertyDescriptor prop in properties)
                                row3[prop.Name] = prop.GetValue(item3) ?? DBNull.Value;

                            table.Rows.Add(row3);
                        }
                    }
                }
                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Current Rank");
                tableToExport.Columns.Add("Last Month Rank");
                tableToExport.Columns.Add("Purchase Points");
                tableToExport.Columns.Add("Sale Points");
                tableToExport.Columns.Add("Add On Points");
                tableToExport.Columns.Add("Lost Opp Points");
                tableToExport.Columns.Add("Redeemed Points");
                tableToExport.Columns.Add("Balanced Points");

                foreach (DataRow dr in table.Rows)
                {
                    DataRow drExport = tableToExport.NewRow();
                    drExport["Type"] = dr["ParticipantType"];
                    drExport["ID"] = dr["Id"];
                    drExport["Name"] = dr["ParticipantName"];
                    drExport["Current Rank"] = dr["CurrentRank"];
                    drExport["Last Month Rank"] = dr["LastMonthRank"];
                    drExport["Purchase Points"] = dr["PurchasePoints"];
                    drExport["Sale Points"] = dr["SalePoints"];
                    drExport["Add On Points"] = dr["AddOnPoints"];
                    drExport["Lost Opp Points"] = dr["LostOppPoints"];
                    drExport["Redeemed Points"] = dr["RedeemedPoints"];
                    drExport["Balanced Points"] = dr["BalancedPoints"];

                    tableToExport.Rows.Add(drExport);
                }
                string ReportName = "Participant List Management";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }

        }

        public ActionResult ExportLeaderBoardManagement(string Cluster, string SubCluster, string City, string Type)
        {
            try
            {
                int clusterId = 0;
                int subClusterId = 0;
                int cityId = 0;
                if (!string.IsNullOrEmpty(Cluster))
                {
                    clusterId = Convert.ToInt32(Cluster);
                }
                if (!string.IsNullOrEmpty(SubCluster))
                {
                    subClusterId = Convert.ToInt32(SubCluster);
                }
                if (!string.IsNullOrEmpty(City))
                {
                    cityId = Convert.ToInt32(City);
                }

                List<LeaderBoardForMgt> listLeaderBoard = new List<LeaderBoardForMgt>();
                listLeaderBoard = MDR.GetLeaderBoardForMgts(Type, clusterId, subClusterId, cityId);

                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Balance Points");
                tableToExport.Columns.Add("Current Overall Rank");
                tableToExport.Columns.Add("Current Cluster Rank");
                tableToExport.Columns.Add("Current Star Rating");
                tableToExport.Columns.Add("Last Month Overall Rank");
                tableToExport.Columns.Add("Last Month Cluster Rank");
                tableToExport.Columns.Add("Last Star Rating");
                tableToExport.Columns.Add("Movement");

                foreach (var item in listLeaderBoard)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["ID"] = item.ID;
                    dr["Name"] = item.Name;
                    dr["Balance Points"] = item.NormalPoints;
                    dr["Current Overall Rank"] = item.CurrentOverallRank;
                    dr["Current Cluster Rank"] = item.CurrentClusterRank;
                    dr["Current Star Rating"] = item.CurrentStarRating;
                    dr["Last Month Overall Rank"] = item.LastMonthOverallRank;
                    dr["Last Month Cluster Rank"] = item.LastMonthClusterRank;
                    dr["Last Star Rating"] = item.LastStarRating;
                    dr["Movement"] = item.RankMovement;

                    tableToExport.Rows.Add(dr);

                }

                string ReportName = "Leader Board Management";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }

        }

        public ActionResult ExportParticipantWiseManagement(string Cluster, string SubCluster, string City, string Month, string Year, string Type)
        {
            try
            {
                List<TgtVsAchParticipantWise> objParticipantData = new List<TgtVsAchParticipantWise>();
                objParticipantData = TAR.GetTgtVsAchParticipantWise(Cluster, SubCluster, City, Month, Year, Type);

                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("SubCluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Date");
                tableToExport.Columns.Add("Volume Focus");
                tableToExport.Columns.Add("Volume Ach");
                tableToExport.Columns.Add("Volume Ach Percentage");
                tableToExport.Columns.Add("Value Focus");
                tableToExport.Columns.Add("Value Ach");
                tableToExport.Columns.Add("Value Ach Percentage");

                foreach (var item in objParticipantData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.Type;
                    dr["ID"] = item.ID;
                    dr["Name"] = item.Name;
                    dr["Cluster"] = item.Cluster;
                    dr["SubCluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Date"] = item.MonthYear;
                    dr["Volume Focus"] = item.VolTgt;
                    dr["Volume Ach"] = item.VolAch;
                    dr["Volume Ach Percentage"] = item.VolAchPer;
                    dr["Value Focus"] = item.ValTgt;
                    dr["Value Ach"] = item.ValAch;
                    dr["Value Ach Percentage"] = item.ValAchPer;

                    tableToExport.Rows.Add(dr);
                }
                string ReportName = "FocusVsAchParticipantWise";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }
        }

        public ActionResult ExportOrdertoRavanaDaysManagement(string Cluster, string SubCluster, string City, string Type)
        {
            try
            {
                List<OrderVsRavanaDay> objOrderData = new List<OrderVsRavanaDay>();
                objOrderData = MDR.GetOrderVsRavanaDayData(Cluster, SubCluster, City, Type);

                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("Sub Cluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Order Count");
                tableToExport.Columns.Add("Avg Diff in Days");
                tableToExport.Columns.Add("Days Diff 1");
                tableToExport.Columns.Add("Days Diff 2");
                tableToExport.Columns.Add("Days Diff 3");
                tableToExport.Columns.Add("Days Diff 4");

                foreach (var item in objOrderData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.CustomerType;
                    dr["ID"] = item.CustomerId;
                    dr["Name"] = item.CustomerName;
                    dr["Cluster"] = item.Cluster;
                    dr["Sub Cluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Order Count"] = item.OrderCount;
                    dr["Avg Diff in Days"] = item.AvgDiffInDays;
                    dr["Days Diff 1"] = item.Diff1;
                    dr["Days Diff 2"] = item.Diff2;
                    dr["Days Diff 3"] = item.Diff3;
                    dr["Days Diff 4"] = item.Diff4;
                    tableToExport.Rows.Add(dr);
                }

                string ReportName = "OrdertoRavanaDaysManagement";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }            
        }

        public ActionResult ExportInvoiceToOrderManagement(string Cluster, string SubCluster, string City, string FromDate, string Todate, string Type)
        {
            try
            {
                List<InvoiceToOrder> objOrderData = new List<InvoiceToOrder>();
                objOrderData = MDR.GetInvoiceToOrderData(Cluster, SubCluster, City, Type, FromDate, Todate);
                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("Sub Cluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Inv Date");
                tableToExport.Columns.Add("Inv Number");
                tableToExport.Columns.Add("Inv Amount");
                tableToExport.Columns.Add("Order Amount");
                tableToExport.Columns.Add("Variance");

                foreach(var item in objOrderData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.CustomerType;
                    dr["ID"] = item.CustomerId;
                    dr["Name"] = item.CustomerName;
                    dr["Cluster"] = item.Cluster;
                    dr["Sub Cluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Inv Date"] = item.InvDate;
                    dr["Inv Number"] = item.InvNumber;
                    dr["Inv Amount"] = item.InvAmount;
                    dr["Order Amount"] = item.OrderAmount;
                    dr["Variance"] = item.Variance;

                    tableToExport.Rows.Add(dr);
                }

                string ReportName = "InvoiceToOrderManagement";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }
            
        }
        
        public ActionResult ExportNoActionParticipantManagement(string Type)
        {
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleManagement"];
                List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
                objData = NAR.GetNoActionParticipantsData(Type, UserSession.CustomerId, UserSession.CustomerType);
                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("Sub Cluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Last Invoice Date");
                tableToExport.Columns.Add("Balance Points");                
                tableToExport.Columns.Add("Days Since Last Inv");

                foreach(var item in objData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.Type;
                    dr["ID"] = item.Id;
                    dr["Name"] = item.Name;
                    dr["Cluster"] = item.Cluster;
                    dr["Sub Cluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Last Invoice Date"] = item.LastInvoiceDate;
                    dr["Balance Points"] = item.BalancePoints;                    
                    dr["Days Since Last Inv"] = item.DaysSinceLastInvoice;

                    tableToExport.Rows.Add(dr);
                }

                string ReportName = "NoActionParticipantManagement";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }
            return null;
        }


        //Export Functions for Employee Category
        public ActionResult ExportParticipantListEmployee(string Cluster, string SubCluster, string City)
        {
            try
            {
                 
                if (string.IsNullOrEmpty(Cluster))
                {
                    Cluster = "0";
                }
                if (string.IsNullOrEmpty(SubCluster))
                {
                    SubCluster = "0";
                }
                if (string.IsNullOrEmpty(City))
                {
                    City = "0";
                }
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
                System.Data.DataTable table = new System.Data.DataTable();

                List<ParticipantListForManagement> lstSuperStockiest = new List<ParticipantListForManagement>();
                lstSuperStockiest = ER.GetParticipantListForEmp(Cluster, SubCluster, City, UserSession.CustomerId, UserSession.CustomerType);

                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ParticipantListForManagement));
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (ParticipantListForManagement item1 in lstSuperStockiest)
                {
                    DataRow row1 = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row1[prop.Name] = prop.GetValue(item1) ?? DBNull.Value;

                    table.Rows.Add(row1);

                    List<ParticipantListForManagement> lstDistributor = new List<ParticipantListForManagement>();
                    lstDistributor = ER.GetSubParticipantListForEmp(item1.Id, item1.ParticipantType, UserSession.CustomerType, UserSession.CustomerId);
                    foreach (ParticipantListForManagement item2 in lstDistributor)
                    {
                        DataRow row2 = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row2[prop.Name] = prop.GetValue(item2) ?? DBNull.Value;

                        table.Rows.Add(row2);

                        List<ParticipantListForManagement> lstRetailer = new List<ParticipantListForManagement>();
                        lstRetailer = ER.GetSubParticipantListForEmp(item2.Id, item2.ParticipantType, UserSession.CustomerType, UserSession.CustomerId);
                        foreach (ParticipantListForManagement item3 in lstRetailer)
                        {
                            DataRow row3 = table.NewRow();
                            foreach (PropertyDescriptor prop in properties)
                                row3[prop.Name] = prop.GetValue(item3) ?? DBNull.Value;

                            table.Rows.Add(row3);
                        }
                    }
                }
                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Current Rank");
                tableToExport.Columns.Add("Last Month Rank");
                tableToExport.Columns.Add("Purchase Points");
                tableToExport.Columns.Add("Sale Points");
                tableToExport.Columns.Add("Add On Points");
                tableToExport.Columns.Add("Lost Opp Points");
                tableToExport.Columns.Add("Redeemed Points");
                tableToExport.Columns.Add("Balanced Points");

                foreach (DataRow dr in table.Rows)
                {
                    DataRow drExport = tableToExport.NewRow();
                    drExport["Type"] = dr["ParticipantType"];
                    drExport["ID"] = dr["Id"];
                    drExport["Name"] = dr["ParticipantName"];
                    drExport["Current Rank"] = dr["CurrentRank"];
                    drExport["Last Month Rank"] = dr["LastMonthRank"];
                    drExport["Purchase Points"] = dr["PurchasePoints"];
                    drExport["Sale Points"] = dr["SalePoints"];
                    drExport["Add On Points"] = dr["AddOnPoints"];
                    drExport["Lost Opp Points"] = dr["LostOppPoints"];
                    drExport["Redeemed Points"] = dr["RedeemedPoints"];
                    drExport["Balanced Points"] = dr["BalancedPoints"];

                    tableToExport.Rows.Add(drExport);
                }
                string ReportName = "Participant List Management";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }

        }

        public ActionResult ExportOrderToInvoiceEmployee(string Cluster, string SubCluster, string City,string FromDate,string Todate, string CustomerType)
        {
            try
            {                                
                var UserSession = (CustomerDetail)Session["ChitaleEmployee"];
                List<InvoiceToOrder> objOrderData = new List<InvoiceToOrder>();
                System.Data.DataTable table = new System.Data.DataTable();
                objOrderData = ER.GetInvoiceToOrderData(Cluster, SubCluster, City, CustomerType, FromDate, Todate, UserSession.CustomerId, UserSession.CustomerType);

                System.Data.DataTable tableToExport = new System.Data.DataTable();
                tableToExport.Columns.Add("Type");
                tableToExport.Columns.Add("ID");
                tableToExport.Columns.Add("Name");
                tableToExport.Columns.Add("Cluster");
                tableToExport.Columns.Add("Sub Cluster");
                tableToExport.Columns.Add("City");
                tableToExport.Columns.Add("Inv Date");
                tableToExport.Columns.Add("Inv Number");
                tableToExport.Columns.Add("Inv Amount");
                tableToExport.Columns.Add("Order Amount");
                tableToExport.Columns.Add("Variance");

                foreach (var item in objOrderData)
                {
                    DataRow dr = tableToExport.NewRow();
                    dr["Type"] = item.CustomerType;
                    dr["ID"] = item.CustomerId;
                    dr["Name"] = item.CustomerName;
                    dr["Cluster"] = item.Cluster;
                    dr["Sub Cluster"] = item.SubCluster;
                    dr["City"] = item.City;
                    dr["Inv Date"] = item.InvDate;
                    dr["Inv Number"] = item.InvNumber;
                    dr["Inv Amount"] = item.InvAmount;
                    dr["Order Amount"] = item.OrderAmount;
                    dr["Variance"] = item.Variance;

                    tableToExport.Rows.Add(dr);
                }
                string ReportName = "InvoiceToOrderEmployee";
                string fileName = ReportName + ".xlsx";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    tableToExport.TableName = ReportName;
                    wb.Worksheets.Add(tableToExport);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
                return null;
            }

        }

    }
}