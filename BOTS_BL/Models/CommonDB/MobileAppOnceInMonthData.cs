namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MobileAppOnceInMonthData")]
    public partial class MobileAppOnceInMonthData
    {
        [StringLength(100)]
        public string NoOfEnrolledMembers { get; set; }

        [StringLength(50)]
        public string TxnCount { get; set; }

        [StringLength(50)]
        public string NoOfPoints { get; set; }

        [StringLength(50)]
        public string NoOfRedemption { get; set; }

        [StringLength(50)]
        public string BusinessGenaratedThroughRedemption { get; set; }

        [StringLength(50)]
        public string AvgTicketSize { get; set; }

        [StringLength(50)]
        public string NoOfReferrals { get; set; }

        [StringLength(50)]
        public string NoOfReferralsConverted { get; set; }

        [StringLength(50)]
        public string BusinessThroughReferrals { get; set; }

        [StringLength(50)]
        public string BulkUploadConversion { get; set; }

        [StringLength(50)]
        public string NoOfProfileUpdated { get; set; }

        public long Id { get; set; }
    }
}
