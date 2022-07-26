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
}
