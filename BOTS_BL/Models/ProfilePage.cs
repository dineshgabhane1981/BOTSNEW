using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class ProfilePage
    {
        public int GroupId { get; set; }
        public string Logo { get; set; }
        public string LegalName { get; set; }
        public string RetailName { get; set; }
        public string OwnerName { get; set; }
        public string OwnerNumber { get; set; }
        public string City { get; set; }
        public string RetailCategory { get; set; }
        public string OutletEnrolled { get; set; }
        public string SMSGateway { get; set; }
        public string GSTNo { get; set; }
        public string SMSBalance { get; set; }
        public string WhatsAppBalance { get; set; }
        public string NextPaymentDate { get; set; }
        public string NextPaymentAmount { get; set; }
    }
}
