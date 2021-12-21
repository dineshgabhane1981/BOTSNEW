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
        public string SMName { get; set; }
        public string SMId { get; set; }
        public decimal? TotalRevenue { get; set; }
        public int NoOfSales { get; set; }
        public decimal? PreviousMonthTotalRevenue { get; set; }
        public int PreviousMonthNoOfSales { get; set; }
        public decimal? AvgRevenueOctaXs { get; set; }     
        public decimal? AvgRevenueOctaPlus { get; set; }      
        public decimal? AvgRevenuesingleoutlet { get; set; }      
        public decimal? AvgRevenueMultipleOutlet { get; set; }       
        public decimal? LastMonthRevenue { get; set; }
        public decimal? Revenuepercentage { get; set; }
        public decimal? AvgRevenuepermonth { get; set; }
        public decimal? BTDNoofSalesDone { get; set; }
        public int MultipleOutlet { get; set; }
        public List<SalesMatrixDetail> lstsalesmatrixdetails { get; set; }
    }
    public class SalesMatrixDetail
    {
        public string SMName { get; set; }
        public string BusinessNm { get; set; }
        public string Product { get; set; }
        public string BillingPartner { get; set; }
        public decimal? Amount { get; set; }
        public long? NoofOutlet { get; set; }
        public DateTime CreatedOn { get; set; }

    }
        public class CallingMatrix
    {
    }    
}
