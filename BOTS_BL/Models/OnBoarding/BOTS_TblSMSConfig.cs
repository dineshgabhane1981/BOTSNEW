namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblSMSConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        public bool IsSMS { get; set; }

        [StringLength(150)]
        public string SMSSenderID { get; set; }

        [StringLength(150)]
        public string SMSUsername { get; set; }

        [StringLength(150)]
        public string SMSPassword { get; set; }

        [StringLength(250)]
        public string SMSlink { get; set; }

        public string EnrolmentSMSScript { get; set; }

        public string EnrollmentEarnSMSScript { get; set; }

        public string EarnSMSScript { get; set; }

        public string OTPSMSScript { get; set; }

        public string BurnSMSScript { get; set; }

        public string CancelEarnSMSScript { get; set; }

        public string CancelBurnSMSScript { get; set; }

        public string AnyCancelSMSScript { get; set; }

        public string BalanceInquirySMSScript { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
