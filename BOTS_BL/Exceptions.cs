using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Repository;
using BOTS_BL.Models.CommonDB;
using context = System.Web.HttpContext;

namespace BOTS_BL
{
    class Exceptions
    {
        private static String exepurl = "";
        public void AddException(Exception ex)
        {
            tblErrorLog objerrorlog = new tblErrorLog();
            exepurl = context.Current.Request.Url.AbsolutePath;
            LoginRepository ObjLR = new LoginRepository();
            objerrorlog.ExceptionMsg = ex.Message.ToString();
            objerrorlog.ExceptionType = ex.GetType().Name.ToString();
            objerrorlog.ExceptionURL = exepurl;
            objerrorlog.ExceptionSource = ex.ToString();
            objerrorlog.Logdate = DateTime.Now;
            ObjLR.AddException(objerrorlog);

        }
    }
}
