namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerLoginDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string UserId { get; set; }

        [StringLength(50)]
        public string LoginId { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(2)]
        public string LevelIndicator { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }

        public bool? LoginStatus { get; set; }

        public bool? UserStatus { get; set; }

        [StringLength(10)]
        public string MobileNo { get; set; }

        [StringLength(1)]
        public string LoginType { get; set; }

        [StringLength(10)]
        public string GroupId { get; set; }

        [StringLength(150)]
        public string EmailId { get; set; }

        [StringLength(10)]
        public string OutletOrBrandId { get; set; }

        [NotMapped]
        public string CustomerName { get; set; }

        [NotMapped]
        public string connectionString { get; set; }
        [NotMapped]
        public bool IsFeedback { get; set; }
        [NotMapped]
        public bool IsEvent { get; set; }
        [NotMapped]
        public bool IsCoupon { get; set; }
    }

    public class DashboardCustomerLogin
    {
        public long SlNo { get; set; }
        public string CustLoginType { get; set; }
        public string OutletOrBrandId { get; set; }
        public string GroupId { get; set; }
        public string LoginType { get; set; }
        public string MobileNo { get; set; }
        public bool? LoginStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LevelIndicator { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoginId { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string CreatedDateStr { get; set; }
    }
}
