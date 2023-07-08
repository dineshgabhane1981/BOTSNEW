using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOTS_BL.Models
{
    public class MemberData
    {
        public string MemberName { get; set; }
        public string MobileNo { get; set; }
        public string OldMobileNo { get; set; }
        public string CardNo { get; set; }
        public decimal? PointsBalance { get; set; }        
        public string EnrolledOutletName { get; set; }
        public string EnrolledOn { get; set; }      
        public string CustomerId { get; set; }
    }

    public class GroupData
    {
        //public string GroupId { get; set; }
        public string RMAssignedName { get; set; }
    }

}