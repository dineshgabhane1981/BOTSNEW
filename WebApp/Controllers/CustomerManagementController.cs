using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System.Web.Script.Serialization;
using BOTS_BL;
using WebApp.App_Start;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class CustomerManagementController : Controller
    {
        CustomerRepository CR = new CustomerRepository();
        Exceptions newexception = new Exceptions();
        OnBoardingRepository OBR = new OnBoardingRepository();
        FeedbackModuleRepository FMR = new FeedbackModuleRepository();
        ReportsRepository RR = new ReportsRepository();
        // GET: CustomerManagement
        public ActionResult Index()
        {
            CustomerDashboardViewModel customerDashboardViewModel = new CustomerDashboardViewModel();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.CustomerName = "";
            }
            catch (Exception ex)
            {
                newexception.AddException(ex,"");
            }
            return View();
        }

        public ActionResult GetOnboardingCustomer()
        {
            CustomerDashboardViewModel customerDashboardViewModel = new CustomerDashboardViewModel();
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            userDetails.CustomerName = "";
            customerDashboardViewModel.onBoardingListings = OBR.GetOnBoardingListings(userDetails);
            return PartialView("_OnBoardingCustomer", customerDashboardViewModel);
        }


        public ActionResult GetLiveCustomer()
        {
            CustomerDashboardViewModel customerDashboardViewModel = new CustomerDashboardViewModel();
            customerDashboardViewModel.customerListings = CR.GetAllCustomer();
            return PartialView("_LiveCustomer", customerDashboardViewModel);
        }


        public ActionResult SelectProduct()
        {
            return View();
        }

        public ActionResult GoToDashboard(string groupId)
        {
            try
            {
                CommonFunctions common = new CommonFunctions();
                groupId = common.DecryptString(groupId);
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                userDetails.GroupId = groupId;
                userDetails.connectionString = CR.GetCustomerConnString(groupId);
                userDetails.CustomerName= CR.GetCustomerName(groupId);
                userDetails.IsFeedback = CR.GetIsFeedback(groupId);

                Session["UserSession"] = userDetails;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, groupId);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult AddNewCustomer(string ProductType, string groupId)
        {
            CustomerViewModel objCustomerViewModel = new CustomerViewModel();
            try
            {
                tblGroupDetail objGroupDetail = new tblGroupDetail();
                tblModulesPayment objModulesPayment = new tblModulesPayment();

                if (!string.IsNullOrEmpty(groupId))
                {
                    CommonFunctions common = new CommonFunctions();
                    groupId = common.DecryptString(groupId);
                    objGroupDetail = CR.GetGroupDetails(Convert.ToInt32(groupId));
                    var LogoURL = System.Configuration.ConfigurationManager.AppSettings["LogoURL"];
                    objGroupDetail.Logo = LogoURL + objGroupDetail.Logo;

                    objModulesPayment = CR.GetModulesAndPayments(Convert.ToInt32(groupId));
                    objModulesPayment.GroupId = Convert.ToInt32(groupId);
                }
                if (!string.IsNullOrEmpty(ProductType))
                    objGroupDetail.ProductType = Convert.ToInt32(ProductType);
                objCustomerViewModel.objGroupData = objGroupDetail;
                objCustomerViewModel.objModulesPayment = objModulesPayment;

                objCustomerViewModel.lstRetailCategory = CR.GetRetailCategory();
                objCustomerViewModel.lstCity = CR.GetCity();
                objCustomerViewModel.lstSourcedBy = CR.GetSourcedBy();
                objCustomerViewModel.lstRMAssigned = CR.GetRMAssigned();
                objCustomerViewModel.lstBillingPartner = CR.GetBillingPartner();
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, groupId);
            }
            return View(objCustomerViewModel);
        }

        [HttpPost]
        public ActionResult AddGroupDetails(CustomerViewModel objCustomerViewModel)
        {
            int GroupId = 0;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                
                objCustomerViewModel.objGroupData.CreatedDate = DateTime.Now;
                objCustomerViewModel.objGroupData.CreatedBy = Convert.ToInt32(userDetails.UserId);
                GroupId = CR.AddGroupDetails(objCustomerViewModel.objGroupData);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return Json(GroupId, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModulesAndPayments(CustomerViewModel objCustomerViewModel)
        {
            bool status = false;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            try
            {
                
                objCustomerViewModel.objModulesPayment.CreatedDate = DateTime.Now;
                objCustomerViewModel.objModulesPayment.CreatedBy = Convert.ToInt32(userDetails.UserId);
                status = CR.AddModulesAndPayments(objCustomerViewModel.objModulesPayment);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, userDetails.GroupId);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool AddBrandAndOutlet(string jsonData)
        {
            bool result = false;
            string GroupId = "";
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                json_serializer.MaxJsonLength = int.MaxValue;
                object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
                
                List<BrandDetail> lstBrands = new List<BrandDetail>();
                List<OutletDetail> lstOutlets = new List<OutletDetail>();
                foreach (Dictionary<string, object> item in objData)
                {
                    GroupId = Convert.ToString(item["GroupId"]);
                    BrandDetail objBrand1 = new BrandDetail();
                    objBrand1.BrandId = Convert.ToString(item["BrandId1"]);
                    objBrand1.GroupId = Convert.ToString(item["GroupId"]);
                    objBrand1.BrandName = Convert.ToString(item["Brand1Name"]);
                    objBrand1.Category = Convert.ToString(item["Brand1Category"]);
                    objBrand1.CreatedDate = DateTime.Now;
                    lstBrands.Add(objBrand1);
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Brand2Name"])))
                    {
                        BrandDetail objBrand2 = new BrandDetail();
                        objBrand2.BrandId = Convert.ToString(item["BrandId2"]);
                        objBrand2.GroupId = Convert.ToString(item["GroupId"]);
                        objBrand2.BrandName = Convert.ToString(item["Brand2Name"]);
                        objBrand2.Category = Convert.ToString(item["Brand2Category"]);
                        objBrand2.CreatedDate = DateTime.Now;
                        lstBrands.Add(objBrand2);
                    }

                    foreach (Dictionary<string, object> itemOutlet in (object[])item["OutletJson"])
                    {
                        OutletDetail objOutletDetail = new OutletDetail();
                        objOutletDetail.GroupId = Convert.ToString(item["GroupId"]);
                        objOutletDetail.BrandId = Convert.ToString(itemOutlet["BrandId"]);
                        objOutletDetail.OutletId = Convert.ToString(itemOutlet["OutletId"]);
                        objOutletDetail.OutletName = Convert.ToString(itemOutlet["OutletName"]);
                        objOutletDetail.City = Convert.ToString(itemOutlet["City"]);
                        objOutletDetail.Address = Convert.ToString(itemOutlet["Area"]);
                        objOutletDetail.PinCode = Convert.ToString(itemOutlet["Pincode"]);
                        objOutletDetail.Latitude = Convert.ToString(itemOutlet["Geotags"]);
                        //objOutletDetail.Latitude = Convert.ToString(itemOutlet["CashierCount"]);
                        objOutletDetail.CreatedDate = DateTime.Now;
                        lstOutlets.Add(objOutletDetail);
                    }

                }
                result = CR.AddBrandAndOutlet(GroupId, lstBrands, lstOutlets);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return result;
        }

        public ActionResult GetBrandAndOutletData(string groupId)
        {
            BrandAndOutletViewModel objData = new BrandAndOutletViewModel();
            try
            {
                objData.lstBrands = CR.GetAllBrandsByGroupId(groupId);
                objData.lstOutlets = CR.GetAllOutletsByGroupId(groupId);
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, groupId);
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
    
        //public ActionResult GetCustomerDetails(string groupId)
        //{             
        //    var objData = OBR.GetOnBoardingCustomerDetails(groupId);

        //    return Json(objData, JsonRequestBehavior.AllowGet);
        //}
       
        public ActionResult GetCustomerConfigDetails(string groupId)
        {
            GroupConfigDetailsViewModel objData = new GroupConfigDetailsViewModel();
            objData.objGroupDetails = CR.GetGroupDetails(Convert.ToInt32(groupId));
            objData.objGroupDetails.Logo = FMR.GetLogo(groupId);
            
            objData.lstBrandDetails = CR.GetAllBrandsByGroupId(groupId);
            objData.objGroupConfig = CR.GetGroupConfig(objData.objGroupDetails);
            objData.objGroupConfig.MemberBase = CR.GetMemberBase(groupId);
            objData.lstOutlets = CR.GetAllOutletsByGroupId(groupId);
            objData.objEarnConfig = CR.GetPointsEarnConfig(groupId);
            objData.objBurnConfig = CR.GetPointsBurnConfig(groupId);

            var connectionString = CR.GetCustomerConnString(groupId);            
            objData.lstOutletList = RR.GetOutletList(groupId, connectionString);
            objData.objSMSDetails = CR.GetAllSMSDetails(groupId);
            if(objData.objSMSDetails !=null)
            {
                objData.SMSDetailsCount = 1;
            }
            else
            {
                objData.SMSDetailsCount = 0;
            }
            if(objData.SMSDetailsCount==1)
            {
                objData.objSMSConfig = CR.GetSMSEmailMasterDetails(groupId);
                objData.objWAConfig = CR.GetWAEmailMasterDetails(groupId);
            }
            objData.lstMWPDetails = CR.GetDLCDetails(groupId);
            objData.objMWPSourceMaster = CR.GetMWPSourceMaster(groupId);
            objData.lstUniquePoints = CR.GetUniquePoints(groupId);

            return PartialView("_GroupConfigDetails", objData);
        }


    }
}
