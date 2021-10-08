namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChannelPartner")]
    public partial class tblChannelPartner
    {
        [Key]
        public int CPId { get; set; }

        [Required]
        [StringLength(150)]
        public string CPartnerName { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ChannelPartnerDetails
    {
        public int ChannelPartnerId { get; set; }
        public string ChannelPartnerName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public string UserName { get; set; }
    }
}
