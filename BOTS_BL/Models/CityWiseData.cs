using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class CityWiseData
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string CategoryName { get; set; }
        public string CityName { get; set; }
        public long MemberBase { get; set; }
        public long MemberBulkUpload { get; set; }
        public long TotalMemberBase { get; set; }

    }

    public class CitywiseReport
    {
        public string CategoryName { get; set; }
        public string CityName { get; set; }
        public long MemberBase { get; set; }
    }
}
