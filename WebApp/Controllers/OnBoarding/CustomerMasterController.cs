using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WebApp.Controllers.OnBoarding
{
    public class CustomerMasterController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        CustomerOnBoardingRepository COR = new CustomerOnBoardingRepository();
        Exceptions newexception = new Exceptions();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CategoryMaster()
        {
            List<CategoryDetails> lsttblcategory = new List<CategoryDetails>();
            lsttblcategory = COR.GetTblCategories();
            return View(lsttblcategory);
        }
        public ActionResult CityMaster()
        {
            List<CityDetails> lsttblcity = new List<CityDetails>();
            lsttblcity = COR.GetCityList();
            return View(lsttblcity);
        }
        public ActionResult CsMaster()
        {
            List<RMAssignedDetails> lsttblRMAssigned = new List<RMAssignedDetails>();
            lsttblRMAssigned = COR.GetRMList();
            return View(lsttblRMAssigned);
        }
        public ActionResult SourceMaster()
        {
            List<SourcedDetails> lsttblsourceBy = new List<SourcedDetails>();
            lsttblsourceBy = COR.GetTblSourceBy();
            return View(lsttblsourceBy);
        }
        public ActionResult BillingPartnerMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblCategory objcategory = new tblCategory();
                foreach (Dictionary<string, object> item in objData)
                {

                    objcategory.CategoryName = Convert.ToString(item["CategoryNm"]);
                    objcategory.CreatedBy = userDetails.LoginId;
                    objcategory.CreatedDate = DateTime.Now;

                }
                result = COR.AddCategory(objcategory);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddCity(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblCity objcity = new tblCity();
                foreach (Dictionary<string, object> item in objData)
                {

                    objcity.CityName = Convert.ToString(item["cityNm"]);
                    objcity.CreatedBy = userDetails.LoginId;
                    objcity.CreatedDate = DateTime.Now;

                }
                result = COR.AddCity(objcity);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSource(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblSourcedBy objsource = new tblSourcedBy();
                foreach (Dictionary<string, object> item in objData)
                {

                    objsource.SourcedbyName = Convert.ToString(item["SourceNm"]);
                    objsource.CreatedBy = userDetails.LoginId;
                    objsource.CreatedDate = DateTime.Now;

                }
                result = COR.AddSource(objsource);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddRM(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblRMAssigned objRM = new tblRMAssigned();
                foreach (Dictionary<string, object> item in objData)
                {

                    objRM.RMAssignedName = Convert.ToString(item["CSNm"]);
                    objRM.CreatedBy = userDetails.LoginId;
                    objRM.CreatedDate = DateTime.Now;

                }
                result = COR.AddRM(objRM);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCategory(int CategoryId)
        {
           
            tblCategory objcategory = new tblCategory();
           // var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
               
                objcategory = COR.GetCategoryById(CategoryId);

            }
            catch (Exception ex)
            {

            }
            return Json(objcategory, JsonRequestBehavior.AllowGet);
            

        }


    }
}