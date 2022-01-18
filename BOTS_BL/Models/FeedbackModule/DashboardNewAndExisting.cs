using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class DashboardNewAndExisting
    {
        public string MobileNo { get; set; }
        public int QuestionPoints { get; set; }
        public string MemberType { get; set; }
        public decimal AvgPoints { get; set; }
        public string OutletId { get; set; }
        public DateTime AddedDate { get; set; }
    }

    public class DashboardOutletWise
    {
        public string OutletName { get; set; }
        public double AvgPoints { get; set; }
    }
    public class DashboardSRWise
    {
        public string SRName { get; set; }
        public double AvgPoints { get; set; }
    }

    public class DashboardTimeWise
    {
        public int timeHr { get; set; }
        public decimal AvgPoints { get; set; }
    }
    public class DashboardSourceWise
    {
        public string SourceName { get; set; }
        public double AvgPoints { get; set; }
    }
}
