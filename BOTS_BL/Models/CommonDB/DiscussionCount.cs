using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DiscussionCount
    {
        public string Name { get; set; }
        public int TotalCount { get; set; }
        public int TotalCountYesterday { get; set; }
        public int TotalWIPCount { get; set; }
        public int TotalWIPLast3Days { get; set; }
        public int TotalWIPBefore3Days { get; set; }
    }
    public class DiscussionMemberWiseCount
    {
        public string Name { get; set; }
        public int TotalCount { get; set; }
    }
}
