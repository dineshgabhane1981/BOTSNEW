using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class EReceipt
    {
        public string Address { get; set; }
        public string StorePAN { get; set; }
        public string StoreCIN { get; set; }
        public string GSTIN { get; set; }
        public string StoreCode { get; set; }
        public string MobileNo { get; set; }
        public string AmountPaid { get; set; }
        public string Quantity { get; set; }
        
    }
}
