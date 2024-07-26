using DLC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using DLC.App_Start;
using BOTS_BL.Models;
using System.Text.RegularExpressions;

namespace DLC.Controllers
{
    public class DashboardController : Controller
    {
        CustomerRepository objCustRepo = new CustomerRepository();
        DLCConfigRepository DCR = new DLCConfigRepository();
        // GET: Dashboard
        public ActionResult Index()
        {
            DashboardViewModel objData = new DashboardViewModel();
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            objData.objDashboardConfig = sessionVariables.objDashboardConfig;
            objData.dLCDashboardContent = DCR.GetDLCDashboardContent(sessionVariables.GroupId, sessionVariables.MobileNo);


            string connStr = objCustRepo.GetCustomerConnString(sessionVariables.GroupId);
            using (var context = new BOTSDBContext(connStr))
            {
                //objData.EarnPoints = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileno).Select(y => y.Points).FirstOrDefault();
                var earnPoint = context.tblCustPointsMasters.Where(x => x.MobileNo == sessionVariables.MobileNo && x.IsActive == true && x.PointsType== "Base").Sum(y => y.Points) ?? 0;
                var BonousPoint = context.tblCustPointsMasters.Where(x => x.MobileNo == sessionVariables.MobileNo && x.IsActive == true && x.PointsType== "Bonus").Sum(y => y.Points) ?? 0;
                var PointsToRS = context.tblRuleMasters.Select(x => x.PointsAllocation).FirstOrDefault();
                var custumerName = context.tblCustDetailsMasters.Where(x => x.MobileNo == sessionVariables.MobileNo).Select(y => y.Name).FirstOrDefault();
                var pointsinRs = earnPoint * PointsToRS ?? 0;
                pointsinRs = Math.Round(pointsinRs, 2);

                ViewBag.earnPoint = earnPoint;
                ViewBag.bonousPoint = BonousPoint;
                ViewBag.pointsinRs = pointsinRs;

                ViewBag.customerName = custumerName;
                
            }
            return View(objData);
        }

        public ActionResult GetUserDetails(string mobileNumber)
        {
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            var groupId = sessionVariables.GroupId;
            string connStr =  objCustRepo.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connStr))
            {
                try
                {
                    var custDetails = context.tblCustDetailsMasters.Where(x => x.MobileNo == mobileNumber).FirstOrDefault();
                    if (custDetails != null)
                    {
                        if (string.IsNullOrEmpty(custDetails.Name))
                        {
                            return Json(new { success = true, missingField = "Name", id = custDetails.Id }, JsonRequestBehavior.AllowGet);
                        }
                        else if (custDetails.Gender == null) 
                        {
                            return Json(new { success = true, missingField = "Gender", id = custDetails.Id }, JsonRequestBehavior.AllowGet);
                        }
                        else if (custDetails.MobileNo == null) 
                        {
                            return Json(new { success = true, missingField = "MobileNumber", id = custDetails.Id }, JsonRequestBehavior.AllowGet);
                        }
                       
                        else if (custDetails.AnniversaryDate == null)
                        {
                            return Json(new { success = true, missingField = "AnniversaryDate", id = custDetails.Id }, JsonRequestBehavior.AllowGet);
                        }
                        else if (custDetails.DOB == null)
                        {
                            return Json(new { success = true, missingField = "BirthDate", id = custDetails.Id }, JsonRequestBehavior.AllowGet);
                        }
                        

                        return Json(new { success = true, missingField = "" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { success = false, message = "User not found." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            
        }
        public class UserDetailUpdateModel
        {
            public string Id { get; set; }
            public string Field { get; set; }
            public string Value { get; set; }
        }



        [HttpPost]
        public JsonResult UpdateUserDetails(UserDetailUpdateModel model)
        {
            // Perform the update logic here
            try
            {
                var sessionVariables = (SessionVariables)Session["SessionVariables"];
                if (sessionVariables == null)
                    throw new Exception("Session is null.");

                var groupId = sessionVariables.GroupId;
                string connStr = objCustRepo.GetCustomerConnString(groupId);

                using (var context = new BOTSDBContext(connStr))
                {
                    var custDetails = context.tblCustDetailsMasters.FirstOrDefault(x => x.Id == model.Id.ToString());
                    if (custDetails != null)
                    {
                        switch (model.Field.ToLower())
                        {
                            case "name":
                                custDetails.Name = model.Value;
                                break;
                            case "gender":
                                custDetails.Gender = model.Value;
                                break;
                            
                            case "mobilenumber":
                                custDetails.MobileNo = model.Value;
                                break;
                            case "emailid":
                                custDetails.Email = model.Value;
                                break;
                            case "cardnumber":
                                custDetails.CardNo = model.Value;
                                break;
                            case "anniversarydate":
                                
                                custDetails.AnniversaryDate = DateTime.Today;
                                break;
                            case "birthdate":

                                custDetails.DOB = DateTime.Today;
                                break;
                            case "category":

                                custDetails.Category = model.Value;
                                break;
                            default:
                                return Json(new { success = false, message = "Invalid field." });
                        }
                        sessionVariables.Flag = "false";
                        Session["SessionVariables"] = sessionVariables;

                        var flag = sessionVariables.Flag;
                        context.SaveChanges();
                        return Json(new { success = true, message = "User details updated successfully." });
                        //change flag status
                        
                       
                    }
                    else
                    {
                        return Json(new { success = false, message = "User not found." });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }

       




    }
}