using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using Chitale.ViewModel;

namespace Chitale.Controllers
{
    public class PointsLedgerController : Controller
    {
        ChitaleException newexception = new ChitaleException();
        PointsLedgerRepository PLR = new PointsLedgerRepository();
        ChitaleDashboardRepository CDR = new ChitaleDashboardRepository();
        // GET: PointsLedger
        public ActionResult Index()
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                //var UserSession = (CustomerDetail)Session["ChitaleUser"];
                //bojList = PLR.GetPointLedgerData(UserSession.CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return View();
        }

        public ActionResult GetPointLedgerData(string isBTD, string FrmDate, string ToDate)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                bojList = PLR.GetPointLedgerData(UserSession.CustomerId, isBTD, FrmDate, ToDate);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return PartialView("_Listing", bojList);
        }

        public ActionResult GetInvoiceOrders(string id, string CustomerId)
        {
            List<PointLedgerModel> bojList = new List<PointLedgerModel>();
            try
            {
                bojList = PLR.GetInvoiceData(id, CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return Json(bojList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TgtVsAch(string CustomerId, string CustomerType)
        {
            TgtVsAchViewModel objData = new TgtVsAchViewModel();
            try
            {
                var objCust = CDR.GetCustomerDetail(CustomerId, CustomerType);
                objCust.CustomerCategory = "Participant";
                Session["ChitaleUser"] = objCust;

                CustomerDetail UserSession = new CustomerDetail();
                UserSession = (CustomerDetail)Session["ChitaleUser"];

                objData.objOverAll = PLR.GetOverallData(UserSession.CustomerId);
                objData.objCategory = PLR.GetCategoryData(UserSession.CustomerId, "", "");
                objData.objSubCategory = PLR.GetSubCategoryData(UserSession.CustomerId);
                objData.objProducts = PLR.GetProductData(UserSession.CustomerId);

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
                objData.MonthItems = MonthItems;
                List<SelectListItem> YearItems = new List<SelectListItem>();
                for (int i = 0; i <= 10; i++)
                {
                    int year = DateTime.Now.Year;
                    YearItems.Add(new SelectListItem
                    {
                        Text = Convert.ToString(year - i),
                        Value = Convert.ToString(year - i)
                    });
                }
                objData.YearItems = YearItems;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return View(objData);
        }

        public ActionResult TgtVsAchFiltered(string CustomerId, string CustomerType, string MonthVal, string YearVal)
        {
            TgtVsAchViewModel objData = new TgtVsAchViewModel();
            try
            {
                if (CustomerType.Contains("#"))
                {
                    CustomerType = CustomerType.Remove(CustomerType.Length - 1, 1);
                }
                var objCust = CDR.GetCustomerDetail(CustomerId, CustomerType);
                objCust.CustomerCategory = "Participant";
                Session["ChitaleUser"] = objCust;

                CustomerDetail UserSession = new CustomerDetail();
                UserSession = (CustomerDetail)Session["ChitaleUser"];

                objData.objOverAll = PLR.GetOverallData(UserSession.CustomerId);
                objData.objCategory = PLR.GetCategoryData(UserSession.CustomerId, MonthVal, YearVal);
                objData.objSubCategory = PLR.GetSubCategoryData(UserSession.CustomerId);
                objData.objProducts = PLR.GetProductData(UserSession.CustomerId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return PartialView("_TgtVsAchList", objData);
        }

    }
}