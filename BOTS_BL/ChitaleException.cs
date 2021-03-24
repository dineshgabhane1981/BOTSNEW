using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models.ChitaleModel;
using context = System.Web.HttpContext;
using BOTS_BL.Repository;

namespace BOTS_BL
{
    public class ChitaleException
    {
        private static String exepurl = "";
        public void AddException(Exception ex)
        {
            tblErrorLog objerrorlog = new tblErrorLog();
            exepurl = context.Current.Request.Url.AbsolutePath;
            ChitaleDashboardRepository ObjLR = new ChitaleDashboardRepository();
            objerrorlog.ExceptionMsg = ex.Message.ToString();
            objerrorlog.ExceptionType = ex.GetType().Name.ToString();
            objerrorlog.ExceptionURL = exepurl;
            objerrorlog.ExceptionSource = ex.ToString();
            objerrorlog.Logdate = DateTime.Now;
            ObjLR.AddException(objerrorlog);

        }

        public void AddDummyException(string str)
        {
            tblErrorLog objerrorlog = new tblErrorLog();
            exepurl = context.Current.Request.Url.AbsolutePath;
            ChitaleDashboardRepository ObjLR = new ChitaleDashboardRepository();
            objerrorlog.ExceptionMsg = str;
            objerrorlog.ExceptionType = str;
            objerrorlog.ExceptionURL = exepurl;
            objerrorlog.ExceptionSource = str;
            objerrorlog.Logdate = DateTime.Now;
            ObjLR.AddException(objerrorlog);

        }
    }
}
