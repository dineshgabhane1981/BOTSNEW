using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models
{
    public class SMSTemplate
    {
        public int Id { get; set; }
        public int MessageId { get; set; }

        public string TemplateScript { get; set; }
    }
}
