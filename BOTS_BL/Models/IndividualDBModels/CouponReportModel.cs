using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CouponReportModel
    {
        public string IssuedOutlet { get; set; }
        public string MobileNo { get; set; }
        public string IssuedInvoiceNo { get; set; }
        public string CouponCode { get; set; }
        public string CouponValue { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RedeemDate { get; set; }
        public string ExpiryDateStr { get; set; }
        public string CreatedDateStr { get; set; }
        public string RedeemDateStr { get; set; }
        public string RedeemedInvoiceNo { get; set; }
        public long? RedeemedInvoiceAmt { get; set; }
        public string OfferCode { get; set; }
        public string RedeemedOutlet { get; set; }
    }
}
