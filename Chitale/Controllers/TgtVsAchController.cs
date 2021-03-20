using BOTS_BL;
using BOTS_BL.Repository;
using Chitale.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class TgtVsAchController : Controller
    {
        ManagementDashboardRepository MDR = new ManagementDashboardRepository();
        Exceptions newexception = new Exceptions();
        // GET: TgtVsAch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Participantwise()
        {
            ManagementViewModel objModel = new ManagementViewModel();
            objModel.ClusterList = MDR.GetClusterList();
            objModel.SubClusterList = MDR.GetSubClusterList();
            objModel.CityList = MDR.GetCityList();
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            List<SelectListItem> MonthItems = new List<SelectListItem>();
            int Month = 1;
            foreach (var item in names)
            {
                MonthItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(item),
                    Value = Convert.ToString(Month)
                });
                Month++;
            }
            MonthItems.RemoveAt(12);
            objModel.MonthsList = MonthItems;
            List<SelectListItem> YearItems = new List<SelectListItem>();
            for (int i=0;i<=10;i++)
            {
                int year = DateTime.Now.Year;
                YearItems.Add(new SelectListItem
                {
                    Text = Convert.ToString(year -i),
                    Value = Convert.ToString(year - i)
                });
            }
            objModel.YearList = YearItems;

            return View(objModel);
        }
        public ActionResult Productwise()
        {
            return View();
        }
    }
}