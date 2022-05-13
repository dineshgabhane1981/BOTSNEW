using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class RenewalData
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string OutletName { get; set; }
        public DateTime? RenewalDate { get; set; }
    }
}
