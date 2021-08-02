namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMSGatewayMaster")]
    public partial class SMSGatewayMaster
    {
        [Key]
        [StringLength(50)]
        public string SMSGatewayId { get; set; }

        [StringLength(100)]
        public string SMSGatewayName { get; set; }
    }
}
