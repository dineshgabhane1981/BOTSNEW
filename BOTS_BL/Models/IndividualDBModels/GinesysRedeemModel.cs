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
        public string billGUID { get; set; }
        public string StoreId { get; set; }
        public string Points { get; set; }
        public string PointsValue { get; set; }
        public string PointsToRedeem { get; set; }
    }
    public class BurnValidateResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string OTPValue { get; set; }
        public string BurnPointsAsAmount { get; set; }
        public string PointsValue { get; set; }
        public string BurnCouponAmount { get; set; }
        public int AllowPointAccrual { get; set; }
        public string OfferCode { get; set; }
        public string MinVal { get; set; }
        public string MaxVal { get; set; }

    }
    public class GinesysRedeemCouponModel
    {
        public string MobileNo { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceAmount { get; set; }
        public string billGUID { get; set; }
        public string StoreId { get; set; }        
    }
}

