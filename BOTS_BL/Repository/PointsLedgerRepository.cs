﻿using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class PointsLedgerRepository
    {
        Exceptions newexception = new Exceptions();
        public List<PointLedgerModel> GetPointLedgerData(string CustomerId, string isBTD, string FrmDate, string ToDate)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    List<string> ObjInvoiceLst = new List<string>();
                    //This is For Purchase
                    if (isBTD == "1")
                    {
                        ObjInvoiceLst = context.InvoiceOrderMappings.Where(x => x.CustomerId == CustomerId).Select(y => y.InvoiceNo).Distinct().ToList();
                    }
                    else
                    {
                        var fromDate = Convert.ToDateTime(FrmDate);
                        var toDate = Convert.ToDateTime(ToDate);
                        ObjInvoiceLst = context.InvoiceOrderMappings.Where(x => x.CustomerId == CustomerId && x.InvoiceDate >= fromDate && x.InvoiceDate <= toDate).Select(y => y.InvoiceNo).Distinct().ToList();
                    }
                    if (ObjInvoiceLst != null)
                    {
                        foreach (var item in ObjInvoiceLst)
                        {
                            PointLedgerModel objPointLedger = new PointLedgerModel();
                            var objTransaction = context.TransactionMasters.Where(x => x.InvoiceNo == item && (x.Type == "Purchase")).FirstOrDefault();
                            if (objTransaction != null)
                            {
                                objPointLedger.CustomerId = CustomerId;
                                objPointLedger.TxnType = objTransaction.Type;
                                objPointLedger.SubType = "Invoice";
                                objPointLedger.RefNo = objTransaction.InvoiceNo;
                                objPointLedger.Amount = objTransaction.InvoiceAmt;
                                objPointLedger.AmountStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.InvoiceAmt));

                                if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.NormalPoints)))
                                {
                                    objPointLedger.BasePoints = objTransaction.NormalPoints;
                                    objPointLedger.BasePointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.NormalPoints));
                                }
                                else
                                {
                                    objPointLedger.BasePoints = 0;
                                    objPointLedger.BasePointsStr = "0.00";
                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.AddOnPoints)))
                                {
                                    objPointLedger.AddOnPoints = objTransaction.AddOnPoints;
                                    objPointLedger.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.AddOnPoints));
                                }
                                else
                                {
                                    objPointLedger.AddOnPoints = 0;
                                    objPointLedger.AddOnPointsStr = "0.00";
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.PenaltyPoints)))
                                {
                                    objPointLedger.LostOppPoints = objTransaction.PenaltyPoints;
                                    objPointLedger.LostOppPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.PenaltyPoints));
                                }
                                else
                                {
                                    objPointLedger.LostOppPoints = 0;
                                    objPointLedger.LostOppPointsStr = "0.00";
                                }
                                objPointLedger.OrderDate = objTransaction.OrderDatetime.Value.ToString("dd-MM-yyyy");
                                objPointLedger.RavanaDate = objTransaction.RavanaDate.Value.ToString("dd-MM-yyyy");
                                if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.TotalPoints)))
                                {
                                    objPointLedger.NetEarnPoints = objTransaction.TotalPoints;
                                    objPointLedger.NetEarnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.TotalPoints));
                                }
                                else
                                {
                                    objPointLedger.NetEarnPoints = 0;
                                    objPointLedger.NetEarnPointsStr = "0.00";
                                }
                                objPointLedger.DaysDiff = Convert.ToInt32((objTransaction.RavanaDate.Value.Date - objTransaction.OrderDatetime.Value.Date).Days);
                                bojList.Add(objPointLedger);
                            }
                        }
                    }


                    //This is For Tgt Vs Ach
                    List<TransactionMaster> tgtTransaction = new List<TransactionMaster>();
                    if (isBTD == "1")
                    {
                        tgtTransaction = context.TransactionMasters.Where(x => x.CustomerId == CustomerId && x.Type == "Tgt Vs Ach").ToList();
                    }
                    else
                    {
                        var fromDate = Convert.ToDateTime(FrmDate);
                        var toDate = Convert.ToDateTime(ToDate);
                        tgtTransaction = context.TransactionMasters.Where(x => x.CustomerId == CustomerId && x.OrderDatetime >= fromDate && x.OrderDatetime <= toDate && x.Type == "Tgt Vs Ach").ToList();
                    }

                    foreach (var item in tgtTransaction)
                    {
                        PointLedgerModel objPointLedger = new PointLedgerModel();
                        objPointLedger.CustomerId = CustomerId;
                        objPointLedger.TxnType = item.Type;
                        objPointLedger.SubType = "Invoice";
                        objPointLedger.RefNo = item.InvoiceNo;
                        objPointLedger.Amount = item.InvoiceAmt;
                        objPointLedger.AmountStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.InvoiceAmt));

                        if (!string.IsNullOrEmpty(Convert.ToString(item.NormalPoints)))
                        {
                            objPointLedger.BasePoints = item.NormalPoints;
                            objPointLedger.BasePointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.NormalPoints));
                        }
                        else
                        {
                            objPointLedger.BasePoints = 0;
                            objPointLedger.BasePointsStr = "0.00";
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(item.AddOnPoints)))
                        {
                            objPointLedger.AddOnPoints = item.AddOnPoints;
                            objPointLedger.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.AddOnPoints));
                        }
                        else
                        {
                            objPointLedger.AddOnPoints = 0;
                            objPointLedger.AddOnPointsStr = "0.00";
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item.PenaltyPoints)))
                        {
                            objPointLedger.LostOppPoints = item.PenaltyPoints;
                            objPointLedger.LostOppPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.PenaltyPoints));
                        }
                        else
                        {
                            objPointLedger.LostOppPoints = 0;
                            objPointLedger.LostOppPointsStr = "0.00";
                        }
                        objPointLedger.OrderDate = item.OrderDatetime.Value.ToString("dd-MM-yyyy");
                        objPointLedger.RavanaDate = item.RavanaDate.Value.ToString("dd-MM-yyyy");
                        if (!string.IsNullOrEmpty(Convert.ToString(item.TotalPoints)))
                        {
                            objPointLedger.NetEarnPoints = item.TotalPoints;
                            objPointLedger.NetEarnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.TotalPoints));
                        }
                        else
                        {
                            objPointLedger.NetEarnPoints = 0;
                            objPointLedger.NetEarnPointsStr = "0.00";
                        }
                        objPointLedger.DaysDiff = Convert.ToInt32((item.RavanaDate.Value.Date - item.OrderDatetime.Value.Date).Days);
                        bojList.Add(objPointLedger);
                    }

                    //This is For Sale
                    List<TransactionMaster> tgtTransactionSale = new List<TransactionMaster>();
                    if (isBTD == "1")
                    {
                        tgtTransactionSale = context.TransactionMasters.Where(x => x.CustomerId == CustomerId && x.TxnType == "Sale" && x.SubType == "Invoice").ToList();
                    }
                    else
                    {
                        var fromDate = Convert.ToDateTime(FrmDate);
                        var toDate = Convert.ToDateTime(ToDate);
                        tgtTransactionSale = context.TransactionMasters.Where(x => x.CustomerId == CustomerId && x.OrderDatetime >= fromDate && x.OrderDatetime <= toDate && x.TxnType == "Sale" && x.SubType == "Invoice").ToList();
                    }

                    foreach (var item in tgtTransactionSale)
                    {
                        PointLedgerModel objPointLedger = new PointLedgerModel();
                        objPointLedger.CustomerId = CustomerId;
                        objPointLedger.TxnType = item.Type;
                        objPointLedger.SubType = "Invoice";
                        objPointLedger.RefNo = item.InvoiceNo;
                        objPointLedger.Amount = item.InvoiceAmt;
                        objPointLedger.AmountStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.InvoiceAmt));

                        if (!string.IsNullOrEmpty(Convert.ToString(item.NormalPoints)))
                        {
                            objPointLedger.BasePoints = item.NormalPoints;
                            objPointLedger.BasePointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.NormalPoints));
                        }
                        else
                        {
                            objPointLedger.BasePoints = 0;
                            objPointLedger.BasePointsStr = "0.00";
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(item.AddOnPoints)))
                        {
                            objPointLedger.AddOnPoints = item.AddOnPoints;
                            objPointLedger.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.AddOnPoints));
                        }
                        else
                        {
                            objPointLedger.AddOnPoints = 0;
                            objPointLedger.AddOnPointsStr = "0.00";
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(item.PenaltyPoints)))
                        {
                            objPointLedger.LostOppPoints = item.PenaltyPoints;
                            objPointLedger.LostOppPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.PenaltyPoints));
                        }
                        else
                        {
                            objPointLedger.LostOppPoints = 0;
                            objPointLedger.LostOppPointsStr = "0.00";
                        }
                        objPointLedger.OrderDate = item.OrderDatetime.Value.ToString("dd-MM-yyyy");
                        objPointLedger.RavanaDate = item.RavanaDate.Value.ToString("dd-MM-yyyy");
                        if (!string.IsNullOrEmpty(Convert.ToString(item.TotalPoints)))
                        {
                            objPointLedger.NetEarnPoints = item.TotalPoints;
                            objPointLedger.NetEarnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.TotalPoints));
                        }
                        else
                        {
                            objPointLedger.NetEarnPoints = 0;
                            objPointLedger.NetEarnPointsStr = "0.00";
                        }
                        objPointLedger.DaysDiff = Convert.ToInt32((item.RavanaDate.Value.Date - item.OrderDatetime.Value.Date).Days);
                        bojList.Add(objPointLedger);
                    }
                }
            }

            catch (Exception ex)
            {
                newexception.AddException(ex, "GetPointLedgerData");
            }

            return bojList;
        }

        public List<PointLedgerModel> GetInvoiceData(string InvoiceNo, string CustomerId)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    var checkTrans = context.TransactionMasters.Where(x => x.InvoiceNo == InvoiceNo && x.CustomerId == CustomerId && x.Type != null).FirstOrDefault();
                    if (checkTrans.Type == "Purchase" || checkTrans.Type == "Sale")
                    {
                        var ObjInvoiceLst = context.InvoiceOrderMappings.Where(x => x.InvoiceNo == InvoiceNo).ToList();

                        if (ObjInvoiceLst != null)
                        {
                            foreach (var item in ObjInvoiceLst)
                            {
                                PointLedgerModel objPointLedger = new PointLedgerModel();
                                var objTransaction = context.TransactionMasters.Where(x => x.OrderNo == item.OrderNo && ((x.TxnType == "Purchase" && x.SubType == "Order")
                                || (x.TxnType == "Cancel" && x.SubType == "Cancel") || (x.TxnType == "Modified" && x.SubType == "Re-order")
                                || (x.TxnType == "Bal-order" && x.SubType == "Bal-order") || (x.TxnType == "Sale" && x.SubType == "Invoice"))).FirstOrDefault();
                                if (objTransaction != null)
                                {
                                    objPointLedger.TxnType = "";
                                    objPointLedger.SubType = objTransaction.SubType;
                                    objPointLedger.RefNo = objTransaction.OrderNo;

                                    objPointLedger.Amount = objTransaction.InvoiceAmt;
                                    objPointLedger.AmountStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.InvoiceAmt));

                                    if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.NormalPoints)))
                                    {
                                        objPointLedger.BasePoints = objTransaction.NormalPoints;
                                        objPointLedger.BasePointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.NormalPoints));
                                    }
                                    else
                                    {
                                        objPointLedger.BasePoints = 0;
                                        objPointLedger.BasePointsStr = "0.00";
                                    }

                                    if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.AddOnPoints)))
                                    {
                                        objPointLedger.AddOnPoints = objTransaction.AddOnPoints;
                                        objPointLedger.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.AddOnPoints));
                                    }
                                    else
                                    {
                                        objPointLedger.AddOnPoints = 0;
                                        objPointLedger.AddOnPointsStr = "0.00";
                                    }
                                    if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.PenaltyPoints)))
                                    {
                                        objPointLedger.LostOppPoints = objTransaction.PenaltyPoints;
                                        objPointLedger.LostOppPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.PenaltyPoints));
                                    }
                                    else
                                    {
                                        objPointLedger.LostOppPoints = 0;
                                        objPointLedger.LostOppPointsStr = "0.00";
                                    }
                                    objPointLedger.OrderDate = objTransaction.OrderDatetime.Value.ToString("dd-MM-yyyy");
                                    objPointLedger.RavanaDate = objTransaction.RavanaDate.Value.ToString("dd-MM-yyyy");
                                    if (!string.IsNullOrEmpty(Convert.ToString(objTransaction.TotalPoints)))
                                    {
                                        objPointLedger.NetEarnPoints = objTransaction.TotalPoints;
                                        objPointLedger.NetEarnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.TotalPoints));
                                    }
                                    else
                                    {
                                        objPointLedger.NetEarnPoints = 0;
                                        objPointLedger.NetEarnPointsStr = "0.00";
                                    }

                                    objPointLedger.DaysDiff = Convert.ToInt32((objTransaction.RavanaDate.Value.Date - objTransaction.OrderDatetime.Value.Date).Days);

                                    bojList.Add(objPointLedger);
                                }
                            }
                        }
                    }
                    if (checkTrans.Type == "Tgt Vs Ach")
                    {
                        var objTransaction = context.TransactionMasters.Where(x => x.InvoiceNo == InvoiceNo && x.CustomerId == CustomerId && (x.SubType == "Volume" || x.SubType == "Value")).ToList();
                        foreach (var item in objTransaction)
                        {
                            PointLedgerModel objPointLedger = new PointLedgerModel();
                            objPointLedger.TxnType = "";
                            objPointLedger.SubType = item.SubType;
                            objPointLedger.RefNo = item.InvoiceNo;
                            objPointLedger.OrderDate = item.OrderDatetime.Value.ToString("dd-MM-yyyy");
                            objPointLedger.AmountStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.InvoiceAmt));
                            objPointLedger.AchievedAmt = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.AchievedAmt));
                            objPointLedger.Variance = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.Variance));
                            objPointLedger.AchPercentage = Convert.ToString(item.AchPercentage);// String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.Variance));
                            if (!string.IsNullOrEmpty(Convert.ToString(item.AddOnPoints)))
                            {
                                objPointLedger.AddOnPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.AddOnPoints));
                            }
                            else
                            {
                                objPointLedger.AddOnPointsStr = "0.00";
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item.PenaltyPoints)))
                            {
                                objPointLedger.LostOppPointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.PenaltyPoints));
                            }
                            else
                            {
                                objPointLedger.LostOppPointsStr = "0.00";
                            }

                            bojList.Add(objPointLedger);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetInvoiceData");
            }

            return bojList;
        }

        public TgtvsAchMaster GetOverallData(string CustomerId, string MonthVal, string YearVal)
        {
            TgtvsAchMaster objTgtvsAchMaster = new TgtvsAchMaster();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    if (MonthVal == "" && YearVal == "")
                    {
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Over All"
                        && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).FirstOrDefault();
                        if (objTgtvsAchMaster != null)
                        {
                            objTgtvsAchMaster.ValuePoints = null;
                            objTgtvsAchMaster.VolumePoints = null;
                        }
                    }
                    else
                    {
                        var month = Convert.ToInt32(MonthVal);
                        var year = Convert.ToInt32(YearVal);
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Over All"
                        && x.Date.Value.Month == month && x.Date.Value.Year == year).FirstOrDefault();
                        if (DateTime.Now.Month == month && DateTime.Now.Year == year && objTgtvsAchMaster != null)
                        {
                            objTgtvsAchMaster.ValuePoints = null;
                            objTgtvsAchMaster.VolumePoints = null;
                        }
                    }
                }
                if (objTgtvsAchMaster != null)
                {
                    objTgtvsAchMaster.VolFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTgtvsAchMaster.VolumeTgt));
                    objTgtvsAchMaster.VolAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTgtvsAchMaster.VolumeAch));
                    objTgtvsAchMaster.VolPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTgtvsAchMaster.VolumePoints));
                    objTgtvsAchMaster.ValFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTgtvsAchMaster.ValueTgt));
                    objTgtvsAchMaster.ValAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTgtvsAchMaster.ValueAch));
                    objTgtvsAchMaster.ValPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTgtvsAchMaster.ValuePoints));
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetOverallData");
            }
            return objTgtvsAchMaster;
        }
        public List<TgtvsAchMaster> GetCategoryData(string CustomerId, string MonthVal, string YearVal)
        {
            List<TgtvsAchMaster> objTgtvsAchMaster = new List<TgtvsAchMaster>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    if (MonthVal == "" && YearVal == "")
                    {
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Category"
                        && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();
                        foreach (var item in objTgtvsAchMaster)
                        {
                            item.ValuePoints = null;
                            item.VolumePoints = null;
                        }
                    }
                    else
                    {
                        var month = Convert.ToInt32(MonthVal);
                        var year = Convert.ToInt32(YearVal);
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Category"
                        && x.Date.Value.Month == month && x.Date.Value.Year == year).ToList();
                        if (DateTime.Now.Month == month && DateTime.Now.Year == year)
                        {
                            foreach (var item in objTgtvsAchMaster)
                            {
                                item.ValuePoints = null;
                                item.VolumePoints = null;
                            }
                        }
                    }
                }
                if (objTgtvsAchMaster != null)
                {
                    if (objTgtvsAchMaster.Count > 0)
                    {
                        foreach (var item in objTgtvsAchMaster)
                        {
                            item.VolFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumeTgt));
                            item.VolAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumeAch));
                            item.VolPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumePoints));
                            item.ValFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValueTgt));
                            item.ValAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValueAch));
                            item.ValPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValuePoints));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetCategoryData");
            }
            return objTgtvsAchMaster;
        }
        public List<TgtvsAchMaster> GetSubCategoryData(string CustomerId, string MonthVal, string YearVal)
        {
            List<TgtvsAchMaster> objTgtvsAchMaster = new List<TgtvsAchMaster>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    if (MonthVal == "" && YearVal == "")
                    {
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "SubCategory"
                        && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();
                        foreach (var item in objTgtvsAchMaster)
                        {
                            item.ValuePoints = null;
                            item.VolumePoints = null;
                        }
                    }
                    else
                    {
                        var month = Convert.ToInt32(MonthVal);
                        var year = Convert.ToInt32(YearVal);
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "SubCategory"
                        && x.Date.Value.Month == month && x.Date.Value.Year == year).ToList();
                        if (DateTime.Now.Month == month && DateTime.Now.Year == year)
                        {
                            foreach (var item in objTgtvsAchMaster)
                            {
                                item.ValuePoints = null;
                                item.VolumePoints = null;
                            }
                        }
                    }
                }
                if (objTgtvsAchMaster != null)
                {
                    if (objTgtvsAchMaster.Count > 0)
                    {
                        foreach (var item in objTgtvsAchMaster)
                        {
                            item.VolFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumeTgt));
                            item.VolAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumeAch));
                            item.VolPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumePoints));
                            item.ValFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValueTgt));
                            item.ValAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValueAch));
                            item.ValPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValuePoints));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetSubCategoryData");
            }
            return objTgtvsAchMaster;
        }

        public List<TgtvsAchMaster> GetProductData(string CustomerId, string MonthVal, string YearVal)
        {
            List<TgtvsAchMaster> objTgtvsAchMaster = new List<TgtvsAchMaster>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    if (MonthVal == "" && YearVal == "")
                    {
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Product"
                        && x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();
                        foreach (var item in objTgtvsAchMaster)
                        {
                            item.ValuePoints = null;
                            item.VolumePoints = null;
                        }
                    }
                    else
                    {
                        var month = Convert.ToInt32(MonthVal);
                        var year = Convert.ToInt32(YearVal);
                        objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Product"
                        && x.Date.Value.Month == month && x.Date.Value.Year == year).ToList();
                        if (DateTime.Now.Month == month && DateTime.Now.Year == year)
                        {
                            foreach (var item in objTgtvsAchMaster)
                            {
                                item.ValuePoints = null;
                                item.VolumePoints = null;
                            }
                        }
                    }
                }
                if (objTgtvsAchMaster != null)
                {
                    if (objTgtvsAchMaster.Count > 0)
                    {
                        foreach (var item in objTgtvsAchMaster)
                        {
                            item.VolFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumeTgt));
                            item.VolAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumeAch));
                            item.VolPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.VolumePoints));
                            item.ValFocusStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValueTgt));
                            item.ValAchStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValueAch));
                            item.ValPtsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(item.ValuePoints));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetProductData");
            }
            return objTgtvsAchMaster;
        }

        public string GetNameOfParticipant(string CustomerId)
        {
            string Name = "";
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    Name = context.CustomerDetails.Where(x => x.CustomerId == CustomerId).Select(y => y.CustomerName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNameOfParticipant");
            }

            return Name;
        }


    }
}
