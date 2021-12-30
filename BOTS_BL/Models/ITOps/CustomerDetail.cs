namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [Key]
        [StringLength(12)]
        public string CustomerId { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(50)]
        public string CardNumber { get; set; }

        [StringLength(100)]
        public string EmailId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOJ { get; set; }

        [StringLength(1)]
        public string MaritalStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AnniversaryDate { get; set; }

        [StringLength(8)]
        public string EnrollingOutlet { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Points { get; set; }

        [StringLength(4)]
        public string MemberGroupId { get; set; }

        [StringLength(50)]
        public string CustomerCategory { get; set; }

        [StringLength(50)]
        public string BillingCustomerId { get; set; }

        [StringLength(1)]
        public string CustomerThrough { get; set; }

        public bool? IsSMS { get; set; }
    }

    public class CustomerDetailwithFeedback
    {
        public long SlNo { get; set; }

        public string MobileNo { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CardNumber { get; set; }

        public string EmailId { get; set; }

        public DateTime? DOB { get; set; }

        public string Gender { get; set; }

        public DateTime? DOJ { get; set; }

        public string MaritalStatus { get; set; }

        public DateTime? AnniversaryDate { get; set; }

        public string EnrollingOutlet { get; set; }

        public string Status { get; set; }

        public decimal? Points { get; set; }

        public string MemberGroupId { get; set; }

        public string CustomerCategory { get; set; }

        public string BillingCustomerId { get; set; }

        public string CustomerThrough { get; set; }

        public bool? IsSMS { get; set; }
        public bool IsFeedBackGiven { get; set; }
        public bool IsDateOfBirth { get; set; }
        public bool IsDOA { get; set; }
        public bool IsHowtoKnow { get; set; }
    }

}
