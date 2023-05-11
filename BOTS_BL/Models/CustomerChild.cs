namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerChild")]
    public partial class CustomerChild
    {
        [Key]
        public int SlNo { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }
        
        [StringLength(12)]
        public string CustomerId { get; set; }

        [StringLength(2)]
        public string ChildCount { get; set; }

        [StringLength(10)]
        public string Child1DOB { get; set; }

        [StringLength(10)]
        public string Pincode { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(1)]
        public string PromotionalSMS { get; set; }

        [StringLength(10)]
        public string Child2DOB { get; set; }

        [StringLength(10)]
        public string Child3DOB { get; set; }

        [StringLength(25)]
        public string City { get; set; }

        [StringLength(25)]
        public string Religion { get; set; }

        [StringLength(25)]
        public string LanguagePreferred { get; set; }

        [StringLength(100)]
        public string Area { get; set; }
    }
}
