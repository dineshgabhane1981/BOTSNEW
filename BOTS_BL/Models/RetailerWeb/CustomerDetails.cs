using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.RetailerWeb
{
    public class CustomerDetails
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string CardNo { get; set; }
        public string PointBalance { get; set; }
        public string TotalSpend { get; set; }
        public string LastTxnDate { get; set; }
        public string PackageAmount { get; set; }
        public string PackageRemainingAmount { get; set; }
        public string PackageValidity { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }

    }

    public class EarnResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string MobileNo { get; set; }
        public string AvailablePoints { get; set; }
        public string PointsEarned { get; set; }
        public string CustomerName { get; set; }
        public string BonusPoints { get; set; }
        public string RefNo { get; set; }
        public string CardNo { get; set; }
    }
    public class BurnValidationResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string OTPValue { get; set; }
        public string BurnPointsAsAmount { get; set; }
        public string PointsValue { get; set; }
    }
    public class BurnResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string PointsEarned { get; set; }
        public string PointsRedeemed { get; set; }
        public string AvailablePoints { get; set; }
    }
    public class DailyReport
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string Counterid { get; set; }
        public string TotalEnrolment { get; set; }
        public string EarnTxnCount { get; set; }
        public string EarnTxnAmt { get; set; }
        public string EarnPoints { get; set; }
        public string BurnTxnCount { get; set; }
        public string BurnTxnAmt { get; set; }
        public string BurnPoints { get; set; }
        public string TotalInvoiceAmt { get; set; }
    }

    public class MDRResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class MDRTxnDetails
    {
        public string Date { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceAmt { get; set; }
        public string Points { get; set; }
        public string Type { get; set; }
    }

    public class MDRData
    {
        public string Mobileno { get; set; }
        public string CustomerName { get; set; }
        public string AvailablePoints { get; set; }
        public string CardNo { get; set; }
        public string EnrolledOutlet { get; set; }
        public string EnrolledOn { get; set; }
        public string DOB { get; set; }

    }

    public class MDRDetails
    {
        public MDRResponse ObjMDRResponse { get; set; }

        public MDRData MDRData { get; set; }

        public List<MDRTxnDetails> ListMDRTxnDetails { get; set; }


    }
    public class Response
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class GTDData
    {
        public string CounterId { get; set; }
        public string Mobileno { get; set; }
        public string CardNo { get; set; }
        public string CustomerName { get; set; }
        public string TxnCount { get; set; }
    }

    public class GTDTxnData
    {
        public string Invoiceno { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string InvoiceAmt { get; set; }
        public string Points { get; set; }
    }
    public class GTDTxnDetails
    {
        public Response GTDResponse { get; set; }
        public GTDData GTDDataobj { get; set; }
        public List<GTDTxnData> LstGTDTxnData { get; set; }

    }

    public class CancelTxnDetails
    {
        public string MobileNo { get; set; }
        public string AvailablePoints { get; set; }
        public string PointsDebited { get; set; }
        public string InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public string PointsCredited { get; set; }
    }

    public class CancelData
    {
        public Response Cancelresponse { get; set; }
        public CancelTxnDetails CancelTxn { get; set; }

    }

    public class DynamicFieldInfo
    {
        public string Fieldid { get; set; }

        public string FieldTypeId { get; set; }

        public string FieldOptionId { get; set; }

        public string FieldValue { get; set; }

        public string FieldName { get; set; }
    }

    public class JSONDATA
    {
        public List<DynamicFieldInfo>[] JsonList1 { get; set; }
    }

    public class JSONDATA1
    {
        public List<DynamicFieldInfo>[] JsonListCust { get; set; }
    }

    public class CustDetails
    {
        public CustomerDetails objCDetails { get; set; }
        public JSONDATA objJsonData { get; set; }
        public JSONDATA1 objJsonCustData { get; set; }
    }


}
