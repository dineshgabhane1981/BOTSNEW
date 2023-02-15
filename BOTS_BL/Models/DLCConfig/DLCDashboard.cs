using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BOTS_BL.Models
{
    public class DLCDashboard
    {
        public string RedirectToPage { get; set; }
        [NotMapped]
        public string MandatoryOrNot { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile1 { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile2 { get; set; }
        [NotMapped]
        public HttpPostedFileBase LogoFile3 { get; set; }

    }
}
