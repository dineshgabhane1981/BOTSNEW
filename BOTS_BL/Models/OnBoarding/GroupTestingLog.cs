namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GroupTestingLog")]
    public partial class GroupTestingLog
    {
        public long Id { get; set; }

        [Required]
        [StringLength(4)]
        public string GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string APIType { get; set; }

        [Required]
        public string RequestPacket { get; set; }

        [Required]
        [StringLength(150)]
        public string RequestURL { get; set; }

        public string ResponsePacket { get; set; }

        [Required]
        [StringLength(50)]
        public string AddedBy { get; set; }

        public DateTime AddedDate { get; set; }
    }
}
