using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOTS_BL.Repository;
using DLC.App_Start;
using DLC.ViewModel;
using BOTS_BL.Models;
using System.Web.Script.Serialization;
using BOTS_BL;

namespace DLC.Controllers
{
    public class PersonalDetailsController : Controller
    {
        DLCConfigRepository DCR = new DLCConfigRepository();
        Exceptions newexception = new Exceptions();
        // GET: PersonalDetails
        public ActionResult Index()
        {
            UpdateProfileViewModel objData = new UpdateProfileViewModel();
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            objData.lstProfileData = DCR.GetPublishDLCProfileConfig(sessionVariables.GroupId, sessionVariables.MobileNo);
            objData.dummyGender = objData.lstProfileData.Where(x => x.FieldName == "Gender").Select(y => y.Value).FirstOrDefault();
            objData.dummyMaritalStatus= objData.lstProfileData.Where(x => x.FieldName == "MaritalStatus").Select(y => y.Value).FirstOrDefault();
            objData.dummyDOB = objData.lstProfileData.Where(x => x.FieldName == "DateOfBirth").Select(y => y.DOBValue).FirstOrDefault();
            return View(objData);
        }
        public ActionResult UpdateProfileData(string jsonData)
        {
            bool status = false;
            var sessionVariables = (SessionVariables)Session["SessionVariables"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objProfileData = (object[])json_serializer.DeserializeObject(jsonData);
            DLCProfileData objData = new DLCProfileData();
            objData.MobileNo = sessionVariables.MobileNo;
            objData.BrandId = sessionVariables.BrandId;

            try
            {
                foreach (Dictionary<string, object> item in objProfileData)
                {
                    DateTime dob;
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Name"])))
                    {
                        objData.Name = Convert.ToString(item["Name"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Gender"])))
                    {
                        objData.Gender = Convert.ToString(item["Gender"]);
                    }
                    if (DateTime.TryParseExact(Convert.ToString(item["DateOfBirth"]), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dob))
                    {
                        objData.DateOfBirth = dob.ToString("yyyy-MM-dd"); // Keep the format "dd-MM-yyyy"
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["MaritalStatus"])))
                    {
                        objData.MaritalStatus = Convert.ToString(item["MaritalStatus"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Area"])))
                    {
                        objData.Area = Convert.ToString(item["Area"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["City"])))
                    {
                        objData.City = Convert.ToString(item["City"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Pincode"])))
                    {
                        objData.Pincode = Convert.ToString(item["Pincode"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(item["Email"])))
                    {
                        objData.Email = Convert.ToString(item["Email"]);
                    }
                    status = DCR.UpdateDLCProfileData(sessionVariables.GroupId, objData);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "SaveDashboard");
            }

            return new JsonResult() { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        
    }
}