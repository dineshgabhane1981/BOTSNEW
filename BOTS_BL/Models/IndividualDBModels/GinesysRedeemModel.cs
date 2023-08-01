using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class GinesysRedeemModel
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceAmount { get; set; }
        public string StoreId { get; set; }
        public decimal? Points { get; set; }
        public decimal? PointsValue { get; set; }
    }
    public class BurnValidateResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string OTPValue { get; set; }
        public string BurnPointsAsAmount { get; set; }
        public string PointsValue { get; set; }
    }
}

