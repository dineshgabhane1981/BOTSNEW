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
using System.Data.Entity.Infrastructure;
using BOTS_BL.Models.IndividualDBModels;
using System.Globalization;

namespace BOTS_BL.Repository
{
    public class EReceiptRepository
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        public EReceipt GetEReceiptJSON(string invoiceNo, string groupId)
        {
            tblTempTxnJSON objData = new tblTempTxnJSON();
            EReceipt receipt = new EReceipt();
            try
            {
                string connStr = CR.GetCustomerConnString(groupId);
                using (var context = new BOTSDBContext(connStr))
                {
                    receipt.objConfig = context.tblEReceiptConfigs.FirstOrDefault();
                                        
                    objData = context.tblTempTxnJSONs.Where(x => x.InvoiceNo == invoiceNo).FirstOrDefault();
                    if (objData != null)
                    {
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        json_serializer.MaxJsonLength = int.MaxValue;
                        object[] receiptObj = (object[])json_serializer.DeserializeObject(objData.JSON);
                        foreach (Dictionary<string, object> item in receiptObj)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(item["ISDCode"])))
                            {
                                receipt.ISDCode = Convert.ToString(item["ISDCode"]);
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item["CounterId"])))
                            {
                                receipt.CounterId = Convert.ToString(item["CounterId"]);
                                var brandId = receipt.CounterId.Substring(0, 5);
                                var BrandDetails = context.tblBrandMasters.Where(x => x.BrandId == brandId).FirstOrDefault();
                                receipt.BrandLogo = BrandDetails.BrandLogoUrl;
                                receipt.BrandName = BrandDetails.BrandName;
                                receipt.WebsiteURL = BrandDetails.WebsiteURL;

                                var outletId = receipt.CounterId.Substring(0, 8);
                                var OutletDetails = context.tblOutletMasters.Where(x => x.OutletId == outletId).FirstOrDefault();
                                receipt.StoreAddress = OutletDetails.Address;
                                receipt.StoreContact = OutletDetails.Phone;
                            }
                            if (!string.IsNullOrEmpty(Convert.ToString(item["Datetime"])))
                            {
                                receipt.InvoiceDate = Convert.ToDateTime(item["Datetime"]);
                            }

                            object[] objCust = new object[1];
                            objCust[0] = item["POSCustomer"];
                            POSCustomer objCustomer = new POSCustomer();
                            foreach (Dictionary<string, object> itemCustomer in (object[])objCust[0])
                            {
                                objCustomer.ISDCode = Convert.ToString(itemCustomer["ISDCode"]);
                                objCustomer.mobile = Convert.ToString(itemCustomer["mobile"]);
                                var first = objCustomer.mobile.Substring(0, 1);
                                var last = objCustomer.mobile.Substring(7, 3);
                                objCustomer.HashedMobile = first + "XXXXXX" + last;
                                receipt.objCustomer = objCustomer;

                                var outletId = receipt.CounterId.Substring(0, 8);
                                var pointsThisBill = context.tblTxnDetailsMasters.Where(x => x.MobileNo == objCustomer.mobile && x.InvoiceNo == invoiceNo && x.OutletId == outletId).Select(y => y.PointsEarned).FirstOrDefault();
                                var totalPoints = context.tblCustPointsMasters.Where(x => x.MobileNo == objCustomer.mobile).Sum(y => y.Points);
                                receipt.PointsEarnedWithThisBill = Convert.ToString(pointsThisBill);
                                receipt.TotalAvailablePoints = Convert.ToString(totalPoints);
                            }

                            object[] objectPOSBILL = new object[1];
                            objectPOSBILL[0] = item["POSBILL"];
                            POSBILL objPOSBILL = new POSBILL();
                            foreach (Dictionary<string, object> itemPOSBILL in (object[])objectPOSBILL[0])
                            {
                                objPOSBILL.BillDate = Convert.ToDateTime(itemPOSBILL["BillDate"]);
                                objPOSBILL.BillOnlyDate = objPOSBILL.BillDate.ToString("dd MMM yyyy");
                                objPOSBILL.BillOnlyTime = objPOSBILL.BillDate.ToLongTimeString();

                                objPOSBILL.BasicAmt = Convert.ToString(itemPOSBILL["BasicAmt"]);
                                objPOSBILL.BillGUID = Convert.ToString(itemPOSBILL["BillGUID"]);
                                objPOSBILL.BillId = Convert.ToString(itemPOSBILL["BillId"]);
                                objPOSBILL.BillNo = Convert.ToString(itemPOSBILL["BillNo"]);
                                objPOSBILL.CashierName = Convert.ToString(itemPOSBILL["CashierName"]);
                                objPOSBILL.ChargeAmt = Convert.ToString(itemPOSBILL["ChargeAmt"]);
                                objPOSBILL.CreatedBy = Convert.ToString(itemPOSBILL["CreatedBy"]);
                                objPOSBILL.CreatedOn = Convert.ToDateTime(itemPOSBILL["CreatedOn"]);
                                objPOSBILL.DiscountAmt = Convert.ToString(itemPOSBILL["DiscountAmt"]);
                                objPOSBILL.ExTaxAmt = Convert.ToString(itemPOSBILL["ExTaxAmt"]);
                                objPOSBILL.GrossAmt = Convert.ToString(itemPOSBILL["GrossAmt"]);
                                objPOSBILL.LPPointsEarned = Convert.ToString(itemPOSBILL["LPPointsEarned"]);
                                objPOSBILL.LPRedeemedPoints = Convert.ToString(itemPOSBILL["LPRedeemedPoints"]);
                                objPOSBILL.MDiscountAmt = Convert.ToString(itemPOSBILL["MDiscountAmt"]);
                                objPOSBILL.MDiscountDesc = Convert.ToString(itemPOSBILL["MDiscountDesc"]);
                                objPOSBILL.MRPAmt = Convert.ToString(itemPOSBILL["MRPAmt"]);
                                objPOSBILL.NetAmt = Convert.ToString(itemPOSBILL["NetAmt"]);
                                objPOSBILL.NetPayable = Convert.ToString(itemPOSBILL["NetPayable"]);
                                objPOSBILL.OwnerGSTINNo = Convert.ToString(itemPOSBILL["OwnerGSTINNo"]);
                                objPOSBILL.OwnerGSTINStateCode = Convert.ToString(itemPOSBILL["OwnerGSTINStateCode"]);
                                objPOSBILL.POSMode = Convert.ToString(itemPOSBILL["POSMode"]);
                                objPOSBILL.Remarks = Convert.ToString(itemPOSBILL["Remarks"]);
                                objPOSBILL.ReturnAmt = Convert.ToString(itemPOSBILL["ReturnAmt"]);
                                objPOSBILL.RoundOff = Convert.ToString(itemPOSBILL["RoundOff"]);
                                objPOSBILL.SaleAmt = Convert.ToString(itemPOSBILL["SaleAmt"]);
                                objPOSBILL.TerminalId = Convert.ToString(itemPOSBILL["TerminalId"]);
                                objPOSBILL.cardNo = Convert.ToString(itemPOSBILL["cardNo"]);
                                objPOSBILL.customerId = Convert.ToString(itemPOSBILL["customerId"]);
                                objPOSBILL.pointBenefit = Convert.ToString(itemPOSBILL["pointBenefit"]);
                                objPOSBILL.OwnerGSTINNo = Convert.ToString(itemPOSBILL["OwnerGSTINNo"]);
                                objPOSBILL.OwnerGSTINStateCode = Convert.ToString(itemPOSBILL["OwnerGSTINStateCode"]);
                                objPOSBILL.GSTDocNumber = Convert.ToString(itemPOSBILL["GSTDocNumber"]);

                                receipt.objPOSBILL = objPOSBILL;

                            }
                            object[] objectPOSBillMOP = new object[1];
                            objectPOSBillMOP[0] = item["POSBillMOP"];
                            POSBillMOP objPOSBillMOP = new POSBillMOP();
                            foreach (Dictionary<string, object> itemBillMOP in (object[])objectPOSBillMOP[0])
                            {
                                objPOSBillMOP.MOPName = Convert.ToString(itemBillMOP["MOPName"]);
                                objPOSBillMOP.MOPName = objPOSBillMOP.MOPName.ToUpper();
                                receipt.objPOSBillMOP = objPOSBillMOP;
                            }

                            object[] objectPOSBillItems = new object[1];
                            objectPOSBillItems[0] = item["POSBillItems"];
                            List<POSBillItems> lstItems = new List<POSBillItems>();

                            foreach (var itemPOSBillItems in (object[])objectPOSBillItems[0])
                            {
                                decimal? TotalTaxableValue = 0;
                                decimal? TotalTaxValue = 0;
                                decimal? TotalMRPValue = 0;
                                if (itemPOSBillItems.GetType().ToString() == "System.Object[]")
                                {                                    
                                    foreach (Dictionary<string, object> billItems in (object[])itemPOSBillItems)
                                    {
                                        POSBillItems objItem = new POSBillItems();
                                        objItem.Article = Convert.ToString(billItems["Article"]);
                                        objItem.BarCode = Convert.ToString(billItems["BarCode"]);
                                        objItem.BasicAmt = Convert.ToString(billItems["BasicAmt"]);
                                        objItem.CESSAmt = Convert.ToString(billItems["CESSAmt"]);
                                        objItem.CESSRate = Convert.ToString(billItems["CESSRate"]);
                                        objItem.CGSTAmt = Convert.ToString(billItems["CGSTAmt"]);
                                        objItem.CGSTRate = Convert.ToString(billItems["CGSTRate"]);
                                        objItem.Cat1 = Convert.ToString(billItems["Cat1"]);
                                        objItem.Cat2 = Convert.ToString(billItems["Cat2"]);
                                        objItem.Cat3 = Convert.ToString(billItems["Cat3"]);
                                        objItem.Cat4 = Convert.ToString(billItems["Cat4"]);
                                        objItem.Cat5 = Convert.ToString(billItems["Cat5"]);
                                        objItem.Cat6 = Convert.ToString(billItems["Cat6"]);
                                        objItem.Department = Convert.ToString(billItems["Department"]);
                                        objItem.DiscountAmt = Convert.ToString(billItems["DiscountAmt"]);
                                        objItem.Division = Convert.ToString(billItems["Division"]);
                                        objItem.ExTaxAmt = Convert.ToString(billItems["ExTaxAmt"]);
                                        objItem.GrossAmt = Convert.ToString(billItems["GrossAmt"]);
                                        objItem.HSNCode = Convert.ToString(billItems["HSNCode"]);
                                        objItem.IDiscountAmt = Convert.ToString(billItems["IDiscountAmt"]);
                                        objItem.IDiscountBasis = Convert.ToString(billItems["IDiscountBasis"]);
                                        objItem.IDiscountDesc = Convert.ToString(billItems["IDiscountDesc"]);
                                        objItem.IDiscountDisplay = Convert.ToString(billItems["IDiscountDisplay"]);
                                        objItem.IDiscountFactor = Convert.ToString(billItems["IDiscountFactor"]);
                                        //objItem.IGSTAmt = Convert.ToString(billItems["IGSTAmt"]);
                                        //objItem.IGSTRate = Convert.ToString(billItems["IGSTRate"]);
                                        objItem.IGrossAmt = Convert.ToString(billItems["IGrossAmt"]);
                                        objItem.ItemId = Convert.ToString(billItems["ItemId"]);
                                        objItem.ItemName = Convert.ToString(billItems["ItemName"]);
                                        objItem.LPAmountSpendFactor = Convert.ToString(billItems["LPAmountSpendFactor"]);
                                        objItem.LPDiscountAmt = Convert.ToString(billItems["LPDiscountAmt"]);
                                        objItem.LPDiscountBenefit = Convert.ToString(billItems["LPDiscountBenefit"]);
                                        objItem.LPDiscountFactor = Convert.ToString(billItems["LPDiscountFactor"]);
                                        objItem.LPPointBenefit = Convert.ToString(billItems["LPPointBenefit"]);
                                        objItem.LPPointEarnedFactor = Convert.ToString(billItems["LPPointEarnedFactor"]);
                                        objItem.LPPointsCalculated = Convert.ToString(billItems["LPPointsCalculated"]);
                                        objItem.MDiscountAmt = Convert.ToString(billItems["MDiscountAmt"]);
                                        objItem.MDiscountFactor = Convert.ToString(billItems["MDiscountFactor"]);
                                        objItem.MGrossAmt = Convert.ToString(billItems["MGrossAmt"]);
                                        objItem.MRP = Convert.ToString(billItems["MRP"]);
                                        objItem.MRPAmt = Convert.ToString(billItems["MRPAmt"]);
                                        objItem.NetAmt = Convert.ToString(billItems["NetAmt"]);
                                        objItem.POSBillItemId = Convert.ToString(billItems["POSBillItemId"]);
                                        objItem.POSOrderId = Convert.ToString(billItems["POSOrderId"]);
                                        objItem.PromoAmt = Convert.ToString(billItems["PromoAmt"]);
                                        objItem.PromoCode = Convert.ToString(billItems["PromoCode"]);
                                        objItem.PromoDiscountFactor = Convert.ToString(billItems["PromoDiscountFactor"]);
                                        objItem.PromoDiscountType = Convert.ToString(billItems["PromoDiscountType"]);
                                        objItem.PromoName = Convert.ToString(billItems["PromoName"]);
                                        objItem.PromoNo = Convert.ToString(billItems["PromoNo"]);
                                        objItem.Qty = Convert.ToString(billItems["Qty"]);
                                        objItem.RSP = Convert.ToString(billItems["RSP"]);
                                        objItem.RefBillDate = Convert.ToString(billItems["RefBillDate"]);
                                        objItem.RefBillNo = Convert.ToString(billItems["RefBillNo"]);
                                        objItem.RefPOSBillItemId = Convert.ToString(billItems["RefPOSBillItemId"]);
                                        objItem.RefStoreCUID = Convert.ToString(billItems["RefStoreCUID"]);
                                        objItem.Remarks = Convert.ToString(billItems["Remarks"]);
                                        objItem.ReturnReason = Convert.ToString(billItems["ReturnReason"]);
                                        objItem.RtQty = Convert.ToString(billItems["RtQty"]);
                                        objItem.SGSTAmt = Convert.ToString(billItems["SGSTAmt"]);
                                        objItem.SGSTRate = Convert.ToString(billItems["SGSTRate"]);
                                        objItem.SalePrice = Convert.ToString(billItems["SalePrice"]);
                                        objItem.SalesPersonFName = Convert.ToString(billItems["SalesPersonFName"]);
                                        objItem.SalesPersonId = Convert.ToString(billItems["SalesPersonId"]);
                                        objItem.SalesPersonLName = Convert.ToString(billItems["SalesPersonLName"]);
                                        objItem.SalesPersonMName = Convert.ToString(billItems["SalesPersonMName"]);
                                        objItem.SalesPersonNumber = Convert.ToString(billItems["SalesPersonNumber"]);
                                        objItem.Section = Convert.ToString(billItems["Section"]);
                                        objItem.SerialNo = Convert.ToString(billItems["SerialNo"]);
                                        objItem.TaxAmt = Convert.ToString(billItems["TaxAmt"]);
                                        objItem.TaxDescription = Convert.ToString(billItems["TaxDescription"]);
                                        objItem.TaxPercent = Convert.ToString(billItems["TaxPercent"]);
                                        objItem.TaxableAmt = Convert.ToString(billItems["TaxableAmt"]);
                                        TotalTaxableValue += Convert.ToDecimal(objItem.TaxableAmt);
                                        TotalTaxValue += Convert.ToDecimal(objItem.TaxAmt);
                                        TotalMRPValue += Convert.ToDecimal(objItem.MRPAmt);
                                        lstItems.Add(objItem);

                                    }
                                    receipt.lstPOSBillItems = lstItems;
                                    receipt.ItemCount = lstItems.Count();
                                    receipt.TotalMRPValue = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(TotalMRPValue));
                                    receipt.TotalTaxableValue = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(TotalTaxableValue));
                                    receipt.TotalTaxValue = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(TotalTaxValue));
                                }
                                else
                                {
                                    foreach (Dictionary<string, object> billItems in (object[])objectPOSBillItems[0])
                                    {
                                        POSBillItems objItem = new POSBillItems();
                                        objItem.Article = Convert.ToString(billItems["Article"]);
                                        objItem.BarCode = Convert.ToString(billItems["BarCode"]);
                                        objItem.BasicAmt = Convert.ToString(billItems["BasicAmt"]);
                                        objItem.CESSAmt = Convert.ToString(billItems["CESSAmt"]);
                                        objItem.CESSRate = Convert.ToString(billItems["CESSRate"]);
                                        objItem.CGSTAmt = Convert.ToString(billItems["CGSTAmt"]);
                                        objItem.CGSTRate = Convert.ToString(billItems["CGSTRate"]);
                                        objItem.Cat1 = Convert.ToString(billItems["Cat1"]);
                                        objItem.Cat2 = Convert.ToString(billItems["Cat2"]);
                                        objItem.Cat3 = Convert.ToString(billItems["Cat3"]);
                                        objItem.Cat4 = Convert.ToString(billItems["Cat4"]);
                                        objItem.Cat5 = Convert.ToString(billItems["Cat5"]);
                                        objItem.Cat6 = Convert.ToString(billItems["Cat6"]);
                                        objItem.Department = Convert.ToString(billItems["Department"]);
                                        objItem.DiscountAmt = Convert.ToString(billItems["DiscountAmt"]);
                                        objItem.Division = Convert.ToString(billItems["Division"]);
                                        objItem.ExTaxAmt = Convert.ToString(billItems["ExTaxAmt"]);
                                        objItem.GrossAmt = Convert.ToString(billItems["GrossAmt"]);
                                        objItem.HSNCode = Convert.ToString(billItems["HSNCode"]);
                                        objItem.IDiscountAmt = Convert.ToString(billItems["IDiscountAmt"]);
                                        objItem.IDiscountBasis = Convert.ToString(billItems["IDiscountBasis"]);
                                        objItem.IDiscountDesc = Convert.ToString(billItems["IDiscountDesc"]);
                                        objItem.IDiscountDisplay = Convert.ToString(billItems["IDiscountDisplay"]);
                                        objItem.IDiscountFactor = Convert.ToString(billItems["IDiscountFactor"]);
                                        //objItem.IGSTAmt = Convert.ToString(billItems["IGSTAmt"]);
                                        //objItem.IGSTRate = Convert.ToString(billItems["IGSTRate"]);
                                        objItem.IGrossAmt = Convert.ToString(billItems["IGrossAmt"]);
                                        objItem.ItemId = Convert.ToString(billItems["ItemId"]);
                                        objItem.ItemName = Convert.ToString(billItems["ItemName"]);
                                        objItem.LPAmountSpendFactor = Convert.ToString(billItems["LPAmountSpendFactor"]);
                                        objItem.LPDiscountAmt = Convert.ToString(billItems["LPDiscountAmt"]);
                                        objItem.LPDiscountBenefit = Convert.ToString(billItems["LPDiscountBenefit"]);
                                        objItem.LPDiscountFactor = Convert.ToString(billItems["LPDiscountFactor"]);
                                        objItem.LPPointBenefit = Convert.ToString(billItems["LPPointBenefit"]);
                                        objItem.LPPointEarnedFactor = Convert.ToString(billItems["LPPointEarnedFactor"]);
                                        objItem.LPPointsCalculated = Convert.ToString(billItems["LPPointsCalculated"]);
                                        objItem.MDiscountAmt = Convert.ToString(billItems["MDiscountAmt"]);
                                        objItem.MDiscountFactor = Convert.ToString(billItems["MDiscountFactor"]);
                                        objItem.MGrossAmt = Convert.ToString(billItems["MGrossAmt"]);
                                        objItem.MRP = Convert.ToString(billItems["MRP"]);
                                        objItem.MRPAmt = Convert.ToString(billItems["MRPAmt"]);
                                        objItem.NetAmt = Convert.ToString(billItems["NetAmt"]);
                                        objItem.POSBillItemId = Convert.ToString(billItems["POSBillItemId"]);
                                        objItem.POSOrderId = Convert.ToString(billItems["POSOrderId"]);
                                        objItem.PromoAmt = Convert.ToString(billItems["PromoAmt"]);
                                        objItem.PromoCode = Convert.ToString(billItems["PromoCode"]);
                                        objItem.PromoDiscountFactor = Convert.ToString(billItems["PromoDiscountFactor"]);
                                        objItem.PromoDiscountType = Convert.ToString(billItems["PromoDiscountType"]);
                                        objItem.PromoName = Convert.ToString(billItems["PromoName"]);
                                        objItem.PromoNo = Convert.ToString(billItems["PromoNo"]);
                                        objItem.Qty = Convert.ToString(billItems["Qty"]);
                                        objItem.RSP = Convert.ToString(billItems["RSP"]);
                                        objItem.RefBillDate = Convert.ToString(billItems["RefBillDate"]);
                                        objItem.RefBillNo = Convert.ToString(billItems["RefBillNo"]);
                                        objItem.RefPOSBillItemId = Convert.ToString(billItems["RefPOSBillItemId"]);
                                        objItem.RefStoreCUID = Convert.ToString(billItems["RefStoreCUID"]);
                                        objItem.Remarks = Convert.ToString(billItems["Remarks"]);
                                        objItem.ReturnReason = Convert.ToString(billItems["ReturnReason"]);
                                        objItem.RtQty = Convert.ToString(billItems["RtQty"]);
                                        objItem.SGSTAmt = Convert.ToString(billItems["SGSTAmt"]);
                                        objItem.SGSTRate = Convert.ToString(billItems["SGSTRate"]);
                                        objItem.SalePrice = Convert.ToString(billItems["SalePrice"]);
                                        objItem.SalesPersonFName = Convert.ToString(billItems["SalesPersonFName"]);
                                        objItem.SalesPersonId = Convert.ToString(billItems["SalesPersonId"]);
                                        objItem.SalesPersonLName = Convert.ToString(billItems["SalesPersonLName"]);
                                        objItem.SalesPersonMName = Convert.ToString(billItems["SalesPersonMName"]);
                                        objItem.SalesPersonNumber = Convert.ToString(billItems["SalesPersonNumber"]);
                                        objItem.Section = Convert.ToString(billItems["Section"]);
                                        objItem.SerialNo = Convert.ToString(billItems["SerialNo"]);
                                        objItem.TaxAmt = Convert.ToString(billItems["TaxAmt"]);
                                        objItem.TaxDescription = Convert.ToString(billItems["TaxDescription"]);
                                        objItem.TaxPercent = Convert.ToString(billItems["TaxPercent"]);
                                        objItem.TaxableAmt = Convert.ToString(billItems["TaxableAmt"]);
                                        TotalTaxableValue += Convert.ToDecimal(objItem.TaxableAmt);
                                        TotalTaxValue += Convert.ToDecimal(objItem.TaxAmt);
                                        TotalMRPValue += Convert.ToDecimal(objItem.MRPAmt);
                                        lstItems.Add(objItem);

                                    }
                                    receipt.lstPOSBillItems = lstItems;
                                    receipt.ItemCount = lstItems.Count();
                                    receipt.TotalMRPValue = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(TotalMRPValue));
                                    receipt.TotalTaxableValue = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(TotalTaxableValue));
                                    receipt.TotalTaxValue = String.Format(new CultureInfo("en-IN", false), "{0:n2}", Convert.ToDouble(TotalTaxValue));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetEReceiptJSON");
            }
            return receipt;
        }

        public tblEReceiptConfig GetEReceiptConfig(string connStr)
        {
            tblEReceiptConfig objData = new tblEReceiptConfig();
            using (var context = new BOTSDBContext(connStr))
            {
                objData = context.tblEReceiptConfigs.FirstOrDefault();
            }
            return objData;
        }
        public bool SaveConfig(tblEReceiptConfig objData, string connStr)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(connStr))
                {
                    context.tblEReceiptConfigs.AddOrUpdate(objData);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveConfig");
            }
            return status;
        }
    }
}
