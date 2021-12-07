using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class SalesReports
    {
    }

    public class MeetingMatrix
    {
        public string SMName { get; set; }
        public string LoginId { get; set; }
        public decimal TotalMeeting { get; set; }        
        public int TotalUniqueMeeting { get; set; }
        public string TotalUniqueMeetingPer { get; set; }
        public string MeetingFollowUpRatio { get; set; }
        public decimal IntrestedCases { get; set; }
        public string IntrestedCasesPer { get; set; }
        public string FollowupCases { get; set; }
        public int UntouchedFollowUp { get; set; }        
        public string SalesConversion { get; set; }
        public string NotIntrestedCases { get; set; }
        public string NotIntrestedCasesPer { get; set; }
        public string NonIntegratedMeetingPer { get; set; }
        public int PersonalMeeting { get; set; }
        public string PersonalMeetingPer { get; set; }
        public int ZoomMeeting { get; set; }
        public string ZoomMeetingPer { get; set; }
    }

    public class SalesMatrix
    {
    }

    public class CallingMatrix
    {
    }    
}
