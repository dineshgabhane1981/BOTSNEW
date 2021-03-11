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
                            objPointLedger.TxnType = "Purchase";
                            objPointLedger.SubType = "Invoice";
                            objPointLedger.RefNo = objTransaction.InvoiceNo;
                            objPointLedger.Amount = objTransaction.InvoiceAmt;
                            objPointLedger.BasePoints = objTransaction.NormalPoints;
                            objPointLedger.AddOnPoints = objTransaction.AddOnPoints;
                            objPointLedger.LostOppPoints = objTransaction.PenaltyPoints;
                            objPointLedger.OrderDate = objTransaction.OrderDatetime.Value.ToString("dd-MM-yyyy");
                            objPointLedger.RavanaDate = objTransaction.RavanaDate.Value.ToString("dd-MM-yyyy");
                            objPointLedger.NetEarnPoints = objTransaction.TotalPoints;
                            objPointLedger.DaysDiff = Convert.ToInt32((objTransaction.RavanaDate - objTransaction.OrderDatetime).Value.TotalDays);
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
                        var objTransaction = context.TransactionMasters.Where(x => x.OrderNo == item.OrderNo && x.Type == "Purchase").FirstOrDefault();
                        if (objTransaction != null)
                        {
                            objPointLedger.TxnType = "";
                            objPointLedger.SubType = objTransaction.SubType;
                            objPointLedger.RefNo = objTransaction.InvoiceNo;
                            objPointLedger.Amount = objTransaction.InvoiceAmt;
                            objPointLedger.BasePoints = objTransaction.NormalPoints;
                            objPointLedger.AddOnPoints = objTransaction.AddOnPoints;
                            objPointLedger.LostOppPoints = objTransaction.PenaltyPoints;
                            objPointLedger.OrderDate = objTransaction.OrderDatetime.Value.ToString("dd-MM-yyyy");
                            objPointLedger.RavanaDate = objTransaction.RavanaDate.Value.ToString("dd-MM-yyyy");
                            objPointLedger.NetEarnPoints = objTransaction.TotalPoints;
                            objPointLedger.DaysDiff = Convert.ToInt32((objTransaction.RavanaDate - objTransaction.OrderDatetime).Value.TotalDays);

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
