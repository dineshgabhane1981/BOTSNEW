namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEReceiptFeedback")]
    public partial class tblEReceiptFeedback
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(250)]
        public string MemberName { get; set; }

        public int Rating { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
