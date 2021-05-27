using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class MembersInformation
    {
        public long? TotalMemberCount { get; set; }
        public long? NameCount { get; set; }
        public decimal? NameCount_Percentage { get; set; }
        public long? GenderCount { get; set; }
        public decimal? GenderCount_Percentage { get; set; }
        public long? BirthDateCount { get; set; }
        public decimal? BirthDateCount_Percentage { get; set; }
        public long? MaritalStatusCount { get; set; }
        public decimal? MaritalStatusCount_Percentage { get; set; }
        public long? AnniversaryDateCount { get; set; }
        public decimal? AnniversaryDateCount_Percentage { get; set; }       
        public long? GenderSplitCount_Male { get; set; }
        public long? GenderSplitCount_Female  { get; set; }
        public long? GenderSplitCount_Others { get; set; }
        public long? GenderSplitCount_Null { get; set; }
        public long? MaritalStatusCount_M { get; set; }
        public long? MaritalStatusCount_U { get; set; }
        public long? MaritalStatusCount_Null { get; set; }
        public long? Age18to25 { get; set; }
        public long? Age26to35 { get; set; }
        public long? Age36to45 { get; set; }
        public long? Age46to55 { get; set; }
        public long? Age55Above { get; set; }
    }
}
