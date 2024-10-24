﻿using System;
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
        public string SMSWASendStatus { get; set; }
        public string SMSScript { get; set; }
        public string SMSUrl { get; set; }
        public string SMSLoginId { get; set; }
        public string SMSPassword { get; set; }
        public string SMSAPIKey { get; set; }
        public string SMSSenderId { get; set; }
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
        public WAReportDetails ObjWADetails { get; set; } = new WAReportDetails();
        public List<ListWAGroupDetailsModel> lstWAAPIDetails { get; set; } = new List<ListWAGroupDetailsModel>();
    }
    public class WAReportDetails
    {
        public string Groupid { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string GroupCode { get; set; } = string.Empty;
        public string WAGroupName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string APIData { get; set; } = string.Empty;
    }
    public class ListWAGroupDetailsModel
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
    }


}
