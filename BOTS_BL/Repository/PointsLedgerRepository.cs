using BOTS_BL.Models.ChitaleModel;
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
        public List<PointLedgerModel> GetPointLedgerData(string CustomerId)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();

            using (var context = new ChitaleDBContext())
            {
                var ObjInvoiceLst = context.InvoiceOrderMappings.Where(x => x.CustomerId == CustomerId).ToList();

                if (ObjInvoiceLst != null)
                {
                    foreach (var item in ObjInvoiceLst)
                    {
                        PointLedgerModel objPointLedger = new PointLedgerModel();
                        var objTransaction = context.TransactionMasters.Where(x => x.InvoiceNo == item.InvoiceNo && x.Type == "Purchase").FirstOrDefault();
                        if (objTransaction != null)
                        {
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
            }

            return bojList;
        }

        public List<PointLedgerModel> GetInvoiceData(string InvoiceNo)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();

            using (var context = new ChitaleDBContext())
            {
                var ObjInvoiceLst = context.InvoiceOrderMappings.Where(x => x.InvoiceNo == InvoiceNo).ToList();

                if (ObjInvoiceLst != null)
                {
                    foreach (var item in ObjInvoiceLst)
                    {
                        PointLedgerModel objPointLedger = new PointLedgerModel();
                        var objTransaction = context.TransactionMasters.Where(x => x.OrderNo == item.OrderNo && x.TxnType == "Purchase" && x.SubType== "Order").FirstOrDefault();
                        if (objTransaction != null)
                        {
                            objPointLedger.TxnType = "";
                            objPointLedger.SubType = objTransaction.SubType;
                            objPointLedger.RefNo = objTransaction.OrderNo;

                            objPointLedger.Amount = objTransaction.InvoiceAmt;
                            objPointLedger.AmountStr= String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objTransaction.InvoiceAmt));

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

            return bojList;
        }

        public TgtvsAchMaster GetOverallData(string CustomerId)
        {
            TgtvsAchMaster objTgtvsAchMaster = new TgtvsAchMaster();
            using (var context = new ChitaleDBContext())
            {
                objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Over All").FirstOrDefault();
            }

            return objTgtvsAchMaster;
        }
        public List<TgtvsAchMaster> GetCategoryData(string CustomerId)
        {
            List<TgtvsAchMaster> objTgtvsAchMaster = new List<TgtvsAchMaster>();
            using (var context = new ChitaleDBContext())
            {
                objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Category").ToList();
            }

            return objTgtvsAchMaster;
        }
        public List<TgtvsAchMaster> GetSubCategoryData(string CustomerId, string CategoryCode)
        {
            List<TgtvsAchMaster> objTgtvsAchMaster = new List<TgtvsAchMaster>();
            using (var context = new ChitaleDBContext())
            {
                objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "SubCategory" && x.CategoryCode == CategoryCode).ToList();
            }

            return objTgtvsAchMaster;
        }

        public List<TgtvsAchMaster> GetProductData(string CustomerId, string SubCategoryCode)
        {
            List<TgtvsAchMaster> objTgtvsAchMaster = new List<TgtvsAchMaster>();
            using (var context = new ChitaleDBContext())
            {
                objTgtvsAchMaster = context.TgtvsAchMasters.Where(x => x.CustomerId == CustomerId && x.ProductType == "Product" && x.SubCategoryCode == SubCategoryCode).ToList();
            }

            return objTgtvsAchMaster;
        }


    }
}
