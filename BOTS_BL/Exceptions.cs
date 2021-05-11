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
    public class Exceptions
    {
        private static String exepurl = "";
        public void AddException(Exception ex, string groupId)
        {
            tblErrorLog objerrorlog = new tblErrorLog();
            exepurl = context.Current.Request.Url.AbsolutePath;
            LoginRepository ObjLR = new LoginRepository();
            objerrorlog.ExceptionMsg = ex.Message.ToString();
            objerrorlog.ExceptionType = ex.GetType().Name.ToString();
            objerrorlog.ExceptionURL = exepurl;
            objerrorlog.ExceptionSource = ex.ToString();
            objerrorlog.Logdate = DateTime.Now;
            objerrorlog.GroupId = groupId;
            ObjLR.AddException(objerrorlog);

        }

        public void AddDummyException(string str)
        {
            tblErrorLog objerrorlog = new tblErrorLog();
            exepurl = context.Current.Request.Url.AbsolutePath;
            LoginRepository ObjLR = new LoginRepository();
            objerrorlog.ExceptionMsg = str;
            objerrorlog.ExceptionType = str;
            objerrorlog.ExceptionURL = exepurl;
            objerrorlog.ExceptionSource = str;
            objerrorlog.Logdate = DateTime.Now;
            ObjLR.AddException(objerrorlog);

        }


    }
}
