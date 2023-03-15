namespace BOTS_BL.Models.CommonDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAWSAccessDetail
    {
        [Key]
        public long SlNo { get; set; }

        public string BucketName { get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }
    }
}
