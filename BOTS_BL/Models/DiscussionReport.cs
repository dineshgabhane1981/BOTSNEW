using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DiscussionReport
    {

    }
    public class DiscussionDataForGraph
    {
        public string CustomerType { get; set; }
        public string GroupName { get; set; }
        public string RMAssignedName { get; set; }
        public string RMLoginId { get; set; }
        public int groupid { get; set; }
        public int count { get; set; }
        public double days { get; set; }
    }

    public class NoCustomerConnect
    {
        public string CustomerType { get; set; }
        public string GroupName { get; set; }
        public int Groupid { get; set; }
        public DateTime LastConnectDate { get; set; }
    }

    public class MostConnectedCustomers
    {
        public int Count { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string CustomerType { get; set; }
    }
}
