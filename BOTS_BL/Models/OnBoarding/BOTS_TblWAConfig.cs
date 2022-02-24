namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblWAConfig
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        public bool IsWA { get; set; }

        [StringLength(150)]
        public string WANumber { get; set; }

        [StringLength(150)]
        public string WAUsername { get; set; }

        [StringLength(150)]
        public string WAPassword { get; set; }

        [StringLength(250)]
        public string WAlink { get; set; }

        public string EnrolmentWAScript { get; set; }

        public string EnrollmentEarnWAScript { get; set; }

        public string EarnWAScript { get; set; }

        public string OTPWAScript { get; set; }

        public string BurnWAScript { get; set; }

        public string CancelEarnWAScript { get; set; }

        public string CancelBurnWAScript { get; set; }

        public string AnyCancelWAScript { get; set; }

        public string BalanceInquiryWAScript { get; set; }

        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
