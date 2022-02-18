using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class TelecallerReport
    {


        public long SrNo { get; set; }


        public string MobileNo { get; set; }


        public string CustomerName { get; set; }


        public string Gender { get; set; }

        public DateTime? DOB { get; set; }

        public DateTime? DOA { get; set; }


        public decimal? Points { get; set; }

        public bool? IsSMSSend { get; set; }


        public string Comments { get; set; }


        public string AddedBy { get; set; }

        public DateTime? AddedDate { get; set; }


        public string OutletId { get; set; }

        public string OutletName { get; set; }
    }

}
