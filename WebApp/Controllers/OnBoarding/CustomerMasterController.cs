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
            List<BillingPartnerDetails> lstbillingpartner = new List<BillingPartnerDetails>();
            lstbillingpartner = COR.GetBillingPartnerList();
            return View(lstbillingpartner);
        }
        public ActionResult SourceType()
        {
            List<SourcedTypeDetails> lstsourcetype = new List<SourcedTypeDetails>();
            lstsourcetype = COR.GetTblSourceType();
            return View(lstsourcetype);
        }
        public ActionResult BillingPartnerProductMaster()
        {
            var lstbillingpartner = COR.GetBillingPartner();
            ViewBag.lstBillingPartner = lstbillingpartner;
            return View();
        }
        public ActionResult ChannelPartner()
        {
            var lstchannelpartner = COR.GetChannelPartner();
            ViewBag.lstChannelPartner = lstchannelpartner;
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string categoryId = "";
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
                    categoryId = Convert.ToString(item["CategoryId"]);
                    objcategory.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (categoryId != "")
                    {
                        objcategory.CategoryId = Convert.ToInt32(categoryId);
                    }
                    else
                    {

                    }
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
            string CityId = "";
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
                    CityId = Convert.ToString(item["CityId"]);
                    objcity.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (CityId != "")
                    {
                        objcity.CityId = Convert.ToInt32(CityId);
                    }
                    else
                    {

                    }

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
            string SourceId = "";
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
                    SourceId = Convert.ToString(item["SourceId"]);
                    objsource.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (SourceId != "")
                    {
                        objsource.SourcedbyId = Convert.ToInt32(SourceId);
                    }
                    else
                    {

                    }

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
            string CSId = "";
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
                    CSId = Convert.ToString(item["CSId"]);
                    objRM.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (CSId != "")
                    {
                        objRM.RMAssignedId = Convert.ToInt32(CSId);
                    }
                    else
                    {

                    }

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

        [HttpPost]
        public ActionResult GetCity(int CityId)
        {

            tblCity objcity = new tblCity();
            try
            {
                objcity = COR.GetCityById(CityId);

            }
            catch (Exception ex)
            {

            }
            return Json(objcity, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public ActionResult GetBillingPartnerProduct(int BillingpartnerId)
        {

            List<BillingPartnerProductDetails> lstbilling = new List<BillingPartnerProductDetails>();
            try
            {
                lstbilling = COR.GetBillingPartnerProductById(BillingpartnerId);

            }
            catch (Exception ex)
            {

            }
            return Json(lstbilling, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public ActionResult GetRM(int RMId)
        {

            tblRMAssigned objRM = new tblRMAssigned();
            try
            {
                objRM = COR.GetRMById(RMId);

            }
            catch (Exception ex)
            {

            }
            return Json(objRM, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult GetSource(int SourceId)
        {

            tblSourcedBy objsource = new tblSourcedBy();
            try
            {
                objsource = COR.GetSourceById(SourceId);

            }
            catch (Exception ex)
            {

            }
            return Json(objsource, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult AddBillingPartner(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string BillingPartnerId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblBillingPartner objbillingpartner = new tblBillingPartner();
                foreach (Dictionary<string, object> item in objData)
                {

                    objbillingpartner.BillingPartnerName = Convert.ToString(item["BillingPartnerNm"]);
                    objbillingpartner.CreatedBy = userDetails.LoginId;
                    objbillingpartner.CreatedDate = DateTime.Now;
                    BillingPartnerId = Convert.ToString(item["BillingPartnerId"]);
                    objbillingpartner.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (BillingPartnerId != "")
                    {
                        objbillingpartner.BillingPartnerId = Convert.ToInt32(BillingPartnerId);
                    }
                    else
                    {

                    }
                }
                result = COR.AddBillingPartner(objbillingpartner);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSourceType(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string SourceTypeId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                tblSourceType objsourcetype = new tblSourceType();
                foreach (Dictionary<string, object> item in objData)
                {

                    objsourcetype.SourceTypeName = Convert.ToString(item["SourceTypeNm"]);
                    objsourcetype.CreatedBy = userDetails.LoginId;
                    objsourcetype.CreatedDate = DateTime.Now;
                    SourceTypeId = Convert.ToString(item["SourceTypeId"]);
                    objsourcetype.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (SourceTypeId != "")
                    {
                        objsourcetype.SourceTypeId = Convert.ToInt32(SourceTypeId);
                    }
                    else
                    {

                    }
                }
                result = COR.AddSourceType(objsourcetype);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBillingPartner(int BillingpartnerId)
        {

            tblBillingPartner objbillingpartner = new tblBillingPartner();
            try
            {
                objbillingpartner = COR.GetBillingPartnerById(BillingpartnerId);

            }
            catch (Exception ex)
            {

            }
            return Json(objbillingpartner, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult GetSourceType(int SourceTypeId)
        {

            tblSourceType objSourceType = new tblSourceType();
            try
            {
                objSourceType = COR.GetSourceTypeById(SourceTypeId);

            }
            catch (Exception ex)
            {

            }
            return Json(objSourceType, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public ActionResult GetBillingPartnerProductByPartner(int BillingpartnerProductId)
        {

            BOTS_TblBillingPartnerProduct lstbillingpartnerproduct = new BOTS_TblBillingPartnerProduct();
            try
            {
                lstbillingpartnerproduct = COR.GetBillingPartnerProductByProductId(BillingpartnerProductId);
            }
            catch (Exception ex)
            {

            }
            return Json(lstbillingpartnerproduct, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddBillingPartnerProduct(string jsonData)
        {
            SPResponse result = new SPResponse();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string BillingPartnerId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                BOTS_TblBillingPartnerProduct objbillingpartnerproduct = new BOTS_TblBillingPartnerProduct();
                foreach (Dictionary<string, object> item in objData)
                {

                    objbillingpartnerproduct.BillingPartnerProductName = Convert.ToString(item["BillingPartnerproductNm"]);
                    objbillingpartnerproduct.BillingPartnerId = Convert.ToInt32(item["BillingPartnerId"]);
                    objbillingpartnerproduct.CreatedBy = userDetails.LoginId;
                    objbillingpartnerproduct.CreatedDate = DateTime.Now;
                    BillingPartnerId = Convert.ToString(item["BillingPartnerId"]);
                    objbillingpartnerproduct.IsActive = Convert.ToBoolean(item["IsActive"]);
                    if (BillingPartnerId != "")
                    {
                        objbillingpartnerproduct.BillingPartnerId = Convert.ToInt32(BillingPartnerId);
                    }
                    else
                    {

                    }
                }
                result = COR.AddBillingPartnerProduct(objbillingpartnerproduct);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveBillingPartner(int BillingpartnerId)
        {
            SPResponse result = new SPResponse();
            try
            {
                // tblBillingPartner objbillingpartner = COR.GetBillingPartnerById(BillingpartnerId);
                result = COR.ActiveInactiveBillingPartner(BillingpartnerId);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveCategory(int CategoryId)
        {
            SPResponse result = new SPResponse();
            try
            {
                // tblCategory objcategory = COR.GetCategoryById(CategoryId);
                result = COR.ActiveInactiveCategory(CategoryId);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveCity(int Cityid)
        {
            SPResponse result = new SPResponse();
            try
            {
                // tblCity objcity = COR.GetCityById(Cityid);
                result = COR.ActiveInactiveCity(Cityid);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveCustomerSuccess(int RmAssignedId)
        {
            SPResponse result = new SPResponse();
            try
            {

                result = COR.ActiveInactiveCS(RmAssignedId);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveSourceBy(int Sourcedbyid)
        {
            SPResponse result = new SPResponse();
            try
            {

                result = COR.ActiveInactiveSourceBy(Sourcedbyid);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveSourceType(int SourceTypeid)
        {
            SPResponse result = new SPResponse();
            try
            {

                result = COR.ActiveInactiveSourceType(SourceTypeid);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveInactiveBillingPartnerProduct(int BillingPartnerProductId)
        {
            SPResponse result = new SPResponse();
            try
            {
                result = COR.ActiveInactiveBillingPartnerProduct(BillingPartnerProductId);
            }
            catch (Exception ex)
            {

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryMasterList()
        {
            List<CategoryDetails> lsttblcategory = new List<CategoryDetails>();
            lsttblcategory = COR.GetTblCategories();
            return PartialView("_CategoryMaster", lsttblcategory);
        }

        public ActionResult CityMasterList()
        {
            List<CityDetails> lsttblcity = new List<CityDetails>();
            lsttblcity = COR.GetCityList();
            return PartialView("_CityMaster", lsttblcity);
        }

        public ActionResult CSMasterList()
        {
            List<RMAssignedDetails> lsttblRM = new List<RMAssignedDetails>();
            lsttblRM = COR.GetRMList();
            return PartialView("_CsMaster", lsttblRM);
        }

        public ActionResult SourceMasterList()
        {
            List<SourcedDetails> lsttblSource = new List<SourcedDetails>();
            lsttblSource = COR.GetTblSourceBy();
            return PartialView("_SourceMaster", lsttblSource);
        }

        public ActionResult SourceTypeMasterList()
        {
            List<SourcedTypeDetails> lsttblSourcetype = new List<SourcedTypeDetails>();
            lsttblSourcetype = COR.GetTblSourceType();
            return PartialView("_SourceType", lsttblSourcetype);
        }

        public ActionResult BillingPartnerList()
        {
            List<BillingPartnerDetails> lsttblbillingpartner = new List<BillingPartnerDetails>();
            lsttblbillingpartner = COR.GetBillingPartnerList();
            return PartialView("_BillingPartnerMaster", lsttblbillingpartner);
        }
    }
}