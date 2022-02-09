using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class GroupConfig
    {       
        public long MemberBase { get; set; }
        public string CityName { get; set; }
        public string CategoryName { get; set; }
        public string BillingPartnerName { get; set; }
        public string BillingSystemName { get; set; }
        public string SourceByName { get; set; }
        public string CSAssignedName { get; set; }
    }
}
