using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    
    public class EReceipt
    {
        public DateTime InvoiceDate { get; set; }
        public string CounterId { get; set; }
        public string ISDCode { get; set; }
        public POSCustomer objCustomer { get; set; }
        public POSBILL objPOSBILL { get; set; }
        public POSBillMOP objPOSBillMOP { get; set; }
        public List<POSBillItems> lstPOSBillItems { get; set; }
        public string StoreAddress { get; set; }
        public string BrandLogo { get; set; }
        public string WebsiteURL { get; set; }
        public string BrandName { get; set; }
        public string StoreContact { get; set; }
        public int ItemCount { get; set; }
        public string TotalTaxableValue { get; set; }
        public string TotalTaxValue { get; set; }
        public string TotalMRPValue { get; set; }
        public string TotalDiscountValue { get; set; }
        public string PointsEarnedWithThisBill { get; set; }
        public string TotalAvailablePoints { get; set; }
        public tblEReceiptConfig objConfig { get; set; }
        public string CustomerName { get; set; }

    }
    public class POSCustomer
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string ISDCode { get; set; }
        public string LPCardNo { get; set; }
        public string MName { get; set; }
        public DateTime? MemberSince { get; set; }
        public string PanNo { get; set; }
        public string PrefCommunicationMode { get; set; }
        public string Receivemessage { get; set; }
        public string Remarks { get; set; }
        public string Salutation { get; set; }
        public string SpouseName { get; set; }
        public string State { get; set; }
        public DateTime? dateofanniversary { get; set; }
        public DateTime? dateofbirth { get; set; }
        public string emailid { get; set; }
        public string gender { get; set; }
        public string isEmployee { get; set; }
        public string lastname { get; set; }
        public string membershipcardnumber { get; set; }
        public string mobile { get; set; }
        public string pincode { get; set; }
        public string profession { get; set; }
        public string HashedMobile { get; set; }
    }

    public class POSBillMOP
    {
        public string BaseAmt { get; set; }
        public string BaseTender { get; set; }
        public string CCNo { get; set; }
        public string CNRType { get; set; }
        public string CreditNoteNumber { get; set; }
        public string CurrencyId { get; set; }
        public string DisplayOrder { get; set; }
        public string ForexAmt { get; set; }
        public string ForexBalance { get; set; }
        public string ForexRate { get; set; }
        public string ForexTender { get; set; }
        public string IsDenoApplicable { get; set; }
        public string MOPDesc { get; set; }
        public string MOPId { get; set; }
        public string MOPName { get; set; }
        public string MOPShortCode { get; set; }
        public string MOPStlmDesc { get; set; }
        public string MOPType { get; set; }
        public string POSBillCNId { get; set; }
        public string POSBillDNId { get; set; }
        public string POSBillMOPId { get; set; }
        public string TPEDC { get; set; }
        public string Tender { get; set; }
        public string TpBillRefNumber { get; set; }
        public string TpEDCBankName { get; set; }
        public string TpEDCCardType { get; set; }
        public string TpEDCResponse { get; set; }
        public string WLTMobile { get; set; }
        public string WalletRefNumber { get; set; }
    }

    public class POSBillItems
    {
        public string Article { get; set; }
        public string BarCode { get; set; }
        public string BasicAmt { get; set; }
        public string CESSAmt { get; set; }
        public string CESSRate { get; set; }
        public string CGSTAmt { get; set; }
        public string CGSTRate { get; set; }
        public string Cat1 { get; set; }
        public string Cat2 { get; set; }
        public string Cat3 { get; set; }
        public string Cat4 { get; set; }
        public string Cat5 { get; set; }
        public string Cat6 { get; set; }
        public string Department { get; set; }
        public string DiscountAmt { get; set; }
        public string Division { get; set; }
        public string ExTaxAmt { get; set; }
        public string GrossAmt { get; set; }
        public string HSNCode { get; set; }
        public string IDiscountAmt { get; set; }
        public string IDiscountBasis { get; set; }
        public string IDiscountDesc { get; set; }
        public string IDiscountDisplay { get; set; }
        public string IDiscountFactor { get; set; }
        public string IGSTAmt { get; set; }
        public string IGSTRate { get; set; }
        public string IGrossAmt { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LPAmountSpendFactor { get; set; }
        public string LPDiscountAmt { get; set; }
        public string LPDiscountBenefit { get; set; }
        public string LPDiscountFactor { get; set; }
        public string LPPointBenefit { get; set; }
        public string LPPointEarnedFactor { get; set; }
        public string LPPointsCalculated { get; set; }
        public string MDiscountAmt { get; set; }
        public string MDiscountFactor { get; set; }
        public string MGrossAmt { get; set; }
        public string MRP { get; set; }
        public string MRPAmt { get; set; }
        public string NetAmt { get; set; }
        public string POSBillItemId { get; set; }
        public string POSOrderId { get; set; }
        public string PromoAmt { get; set; }
        public string PromoCode { get; set; }
        public string PromoDiscountFactor { get; set; }
        public string PromoDiscountType { get; set; }
        public string PromoName { get; set; }
        public string PromoNo { get; set; }
        public string Qty { get; set; }
        public string RSP { get; set; }
        public string RefBillDate { get; set; }
        public string RefBillNo { get; set; }
        public string RefPOSBillItemId { get; set; }
        public string RefStoreCUID { get; set; }
        public string Remarks { get; set; }
        public string ReturnReason { get; set; }
        public string RtQty { get; set; }
        public string SGSTAmt { get; set; }
        public string SGSTRate { get; set; }
        public string SalePrice { get; set; }
        public string SalesPersonFName { get; set; }
        public string SalesPersonId { get; set; }
        public string SalesPersonLName { get; set; }
        public string SalesPersonMName { get; set; }
        public string SalesPersonNumber { get; set; }
        public string Section { get; set; }
        public string SerialNo { get; set; }
        public string TaxAmt { get; set; }
        public string TaxDescription { get; set; }
        public string TaxPercent { get; set; }
        public string TaxableAmt { get; set; }
        
    }
    public class POSBILL
    {
        public DateTime BillDate { get; set; }
        public string BillOnlyDate { get; set; }
        public string BillOnlyTime { get; set; }
        public string AllowPointAccrual { get; set; }
        public string BasicAmt { get; set; }
        public string BillGUID { get; set; }
        public string BillId { get; set; }
        public string BillNo { get; set; }
        public string CashierName { get; set; }
        public string ChargeAmt { get; set; }
        public string CouponCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CustomerGSTIN { get; set; }
        public string CustomerGSTStateCode { get; set; }
        public string DiscountAmt { get; set; }
        public string DiscountId { get; set; }
        public string EMRREDCouponRef { get; set; }
        public string EMRREDPointRef { get; set; }
        public string ExTaxAmt { get; set; }
        public string GSTDocNumber { get; set; }
        public string GrossAmt { get; set; }
        public string IDiscountAmt { get; set; }
        public string LPDiscountAmt { get; set; }
        public string LPPointsEarned { get; set; }
        public string LPRedeemedPoints { get; set; }
        public string MDiscountAmt { get; set; }
        public string MDiscountDesc { get; set; }
        public string MRPAmt { get; set; }
        public string NetAmt { get; set; }
        public string NetPayable { get; set; }
        public string OwnerGSTINNo { get; set; }
        public string OwnerGSTINStateCode { get; set; }
        public string POSMode { get; set; }
        public string PromoAmt { get; set; }
        public string PromoBenefit { get; set; }
        public string PromoCode { get; set; }
        public string PromoName { get; set; }
        public string PromoNo { get; set; }
        public string Remarks { get; set; }
        public string ReturnAmt { get; set; }
        public string RoundOff { get; set; }
        public string SaleAmt { get; set; }
        public string StockPointId { get; set; }
        public string TerminalId { get; set; }
        public string TpCRMCouponCode { get; set; }
        public string TpCRMCouponType { get; set; }
        public string TpCRMCustomerMobile { get; set; }
        public string TpCRMCustomerName { get; set; }
        public string TpCRMDiscValidationCode { get; set; }
        public string TpCRMRedFactor { get; set; }
        public string TpCRMRedType { get; set; }
        public string TpCRMRefNumber { get; set; }
        public string TradeGroupID { get; set; }
        public string UDFDATE01 { get; set; }
        public string UDFDATE02 { get; set; }
        public string UDFDATE03 { get; set; }
        public string UDFDATE04 { get; set; }
        public string UDFDATE05 { get; set; }
        public string UDFNUM01 { get; set; }
        public string UDFNUM02 { get; set; }
        public string UDFNUM03 { get; set; }
        public string UDFNUM04 { get; set; }
        public string UDFNUM05 { get; set; }
        public string UDFSTRING1 { get; set; }
        public string UDFSTRING10 { get; set; }
        public string UDFSTRING2 { get; set; }
        public string UDFSTRING3 { get; set; }
        public string UDFSTRING4 { get; set; }
        public string UDFSTRING5 { get; set; }
        public string UDFSTRING6 { get; set; }
        public string UDFSTRING7 { get; set; }
        public string UDFSTRING8 { get; set; }
        public string UDFSTRING9 { get; set; }
        public string cardNo { get; set; }
        public string customerId { get; set; }
        public string discountBenefitId { get; set; }
        public string pointBenefit { get; set; }
    }
}

