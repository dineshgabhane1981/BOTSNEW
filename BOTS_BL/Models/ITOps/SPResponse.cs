using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class SPResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseSucessCount { get; set; }
        public string ResponseFailCount { get; set; }
        public string ResponseInValidFormatCount { get; set; }
    }
}
