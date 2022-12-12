using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class FeedbackData
    {
        public string Mobilenumber { get; set; }
        public int q1 { get; set; }
        public int q2 { get; set; }
        public int q3 { get; set; }
        public int q4 { get; set; }

        public string qq1 { get; set; }
        public string qq2 { get; set; }
        public string qq3 { get; set; }
        public string qq4 { get; set; }

        public string salesR { get; set; }
        public string outletid { get; set; }
        public string howtoknow { get; set; }
        public DateTime? datetime { get; set; }
        public string comments { get; set; }
        public string AudioStream { get; set; }

    }
}
