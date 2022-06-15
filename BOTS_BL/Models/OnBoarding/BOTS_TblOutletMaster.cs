namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BOTS_TblOutletMaster
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Required]
        [StringLength(5)]
        public string BrandId { get; set; }

        [Required]
        [StringLength(8)]
        public string OutletId { get; set; }

        [Required]
        [StringLength(50)]
        public string OutletName { get; set; }

        [StringLength(50)]
        public string AreaName { get; set; }

        [StringLength(50)]
        public string AuthorisedPerson { get; set; }

        [StringLength(10)]
        public string RegisterMobileNo { get; set; }

        [StringLength(50)]
        public string RegisterEmail { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(25)]
        public string Latitude { get; set; }

        [StringLength(25)]
        public string Longitude { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(6)]
        public string PinCode { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public string BrandName { get; set; }

        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string StateName { get; set; }
       
        public string PreferredLanguage { get; set; }
    }
}
