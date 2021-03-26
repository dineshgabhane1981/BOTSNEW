using BOTS_BL;
using BOTS_BL.Models.ChitaleModel;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chitale.Controllers
{
    public class EmployeeController : Controller
    {
        ChitaleException newexception = new ChitaleException();
        EmployeeRepository ER = new EmployeeRepository();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Top5Participants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<CustomerDetail> dataCustomerDetail = new List<CustomerDetail>();
                dataCustomerDetail = ER.GetTop5Participant(type, UserSession.CustomerId, UserSession.CustomerType);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataCustomerDetail)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.Points));
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult Bottom5Participants(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<CustomerDetail> dataCustomerDetail = new List<CustomerDetail>();
                dataCustomerDetail = ER.Bottom5Participants(type, UserSession.CustomerId, UserSession.CustomerType);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataCustomerDetail)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.Points));
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetTop5LostOppsParticipant(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<Top5LostParticipants> dataLostOpps = new List<Top5LostParticipants>();
                dataLostOpps = ER.GetTop5LostOppsParticipant(type, UserSession.CustomerId, UserSession.CustomerType);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataLostOpps)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.Points));
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetTop5TgtVsAchPerformanceEmp(string type)
        {
            List<object> lstData = new List<object>();
            try
            {
                var UserSession = (CustomerDetail)Session["ChitaleUser"];
                List<Top5TgtVsAchPerformanceEmp> dataTop5TgtVsAch = new List<Top5TgtVsAchPerformanceEmp>();
                dataTop5TgtVsAch = ER.GetTop5TgtVsAchPerformanceEmp(type, UserSession.CustomerId, UserSession.CustomerType);
                List<string> nameList = new List<string>();
                List<decimal> dataList = new List<decimal>();
                foreach (var item in dataTop5TgtVsAch)
                {
                    nameList.Add(item.CustomerName);
                    dataList.Add(Convert.ToDecimal(item.VolumeAchPercentage));
                }
                lstData.Add(nameList);
                lstData.Add(dataList);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return new JsonResult() { Data = lstData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult LeaderBoard()
        {
            return View();
        }
        public ActionResult OrderToInvoice()
        {
            return View();
        }
    }
}