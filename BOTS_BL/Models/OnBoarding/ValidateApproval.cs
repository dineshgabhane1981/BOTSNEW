using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.OnBoarding
{
    public class ValidateApproval
    {
        public bool Outletstatus { get; set; }
        public bool Earnstatus { get; set; }
        public bool Burnstatus { get; set; }
        public int CommCount { get; set; }
        public bool CommounicationStatus { get; set; }
        public bool CommSMSWAStatus { get; set; }

    }
}
