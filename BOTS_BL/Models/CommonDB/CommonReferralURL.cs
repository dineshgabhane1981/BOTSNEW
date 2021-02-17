namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonReferralURL")]
    public partial class CommonReferralURL
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string CounterId { get; set; }

        public string DataSource { get; set; }

        public string Data_Base { get; set; }

        public string DBPassword { get; set; }

        public string UserId { get; set; }
    }
}
