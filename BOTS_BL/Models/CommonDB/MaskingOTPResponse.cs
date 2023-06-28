using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.CommonDB
{
    public class MaskingOTPResponse
    {
        public bool status { get; set; }
        public string OTP { get; set; }
        public bool smsstatus { get; set; }
    }
}
