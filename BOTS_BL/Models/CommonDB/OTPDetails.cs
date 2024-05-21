namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OTPDetails")]
    public partial class OTPDetail
    {
        [Key]
        public int ID { get; set; }
        [StringLength(100)]
        public string EmailId { get; set; }
        
        public int OTP { get; set; }
        
        public DateTime SentDate { get; set; }
        public bool? IsUsed { get; set; }

    }
}
