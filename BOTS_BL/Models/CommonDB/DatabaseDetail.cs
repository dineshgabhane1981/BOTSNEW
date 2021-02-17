namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DatabaseDetail
    {
        [Key]
        public long SlNo { get; set; }

        [StringLength(10)]
        public string CounterId { get; set; }

        [StringLength(10)]
        public string SecurityKey { get; set; }

        [StringLength(200)]
        public string IPAddress { get; set; }

        [StringLength(25)]
        public string DBName { get; set; }

        [StringLength(25)]
        public string DBPassword { get; set; }

        [StringLength(25)]
        public string DBId { get; set; }

        [StringLength(1)]
        public string EncryptionStatus { get; set; }

        [StringLength(50)]
        public string LoginId { get; set; }

        [StringLength(5)]
        public string GroupId { get; set; }
    }
}
