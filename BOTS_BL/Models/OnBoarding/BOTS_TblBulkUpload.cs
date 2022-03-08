namespace BOTS_BL.Models.OnBoarding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblBulkUpload
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupId { get; set; }

        [StringLength(50)]
        public string CustId { get; set; }

        [StringLength(250)]
        public string CustName { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string OutletId { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        public string Status { get; set; }

        [StringLength(50)]
        public string DOB { get; set; }

        [StringLength(50)]
        public string AOB { get; set; }

        [StringLength(150)]
        public string EmailId { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Area { get; set; }

        [StringLength(100)]
        public string CustomerCategory { get; set; }

        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string Points { get; set; }
    }
}
