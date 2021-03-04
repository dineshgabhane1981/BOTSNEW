using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
  public class Dashboardsummary
    {
                
            public string CustomerId { get; set; }
            public string CustomerType { get; set; }
            public string TxnType { get; set; }
            public int NormalPoints { get; set; }

            List<Dashboardsummary> lstorderpoints { get; set; }
       
    }
}
