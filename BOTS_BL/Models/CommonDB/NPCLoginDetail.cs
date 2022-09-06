namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NPCLoginDetail
    {
        [Key]
        public int Slno { get; set; }
         
        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public string LoginId { get; set; }

        public string Password { get; set; }
    }
}
