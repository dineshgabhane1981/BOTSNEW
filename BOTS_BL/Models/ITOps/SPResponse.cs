using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class SPResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseSucessCount { get; set; }
        public string ResponseFailCount { get; set; }
        public string ResponseInValidFormatCount { get; set; }
    }
    public class DLCSPResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string WAMessage { get; set; }
        public string WATokenId { get; set; }
        public string WAUrl { get; set; }
        public string MobileNo { get; set; }
    }

    public class NewCustDetails
    {
        public string BillingPartnerUrl { get; set; }
        public string DLCLink { get; set; }
        public List<CounterIdDetails> lstCounterIdDetails { get; set; }
        public bool result { get; set; }
    }

    public class CounterIdDetails
    {
        public string OutletName { get; set; }
        public string CounterId { get; set; }
        public string Securitykey { get; set; }
    }

    public class WADetailsSummary
    {
        public WAReportDetails ObjWADetails { get; set; }
        public List<ListWAGroupDetailsModel> lstWAAPIDetails { get; set; }
    }
    public class WAReportDetails
    {
        public string Groupid { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string WAGroupName { get; set; }
        public string Status { get; set; }   

        public string APIData { get; set; }
    }
    public class ListWAGroupDetailsModel
    {
        public string id { get; set; }
        public string name { get; set; }
    }


}
