using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
   public class ResetSecurityKey
    {


        public List<LoginIdByOutlet> lstloginid { get; set; }


    }

    public class LoginIdByOutlet
    {
        

        public string CounterId { get; set; }
        public string securitykey { get; set; }

    }
}
