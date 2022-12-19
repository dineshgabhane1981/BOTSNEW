using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblNonTransactedGroups")]
    public class NonTransacting
    {
        [Key]
        public Int64 SlNo { get; set; }
        public string GroupName { get; set; }
        public decimal TxnCount { get; set; }
        public DateTime? LastTxnDate { get; set; }        
    }
}
