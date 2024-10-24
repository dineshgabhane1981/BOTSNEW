﻿using BOTS_BL;
using BOTS_BL.Models;
using BOTS_BL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace VenusCompetitionLink.Controllers
{
    public class HomeController : Controller
    {
        Exceptions newexception = new Exceptions();
        VenusRepository VR = new VenusRepository();
        public ActionResult Index()
        {
            //var Slno = VR.GetSLno();
            //ViewBag.RegNo = Slno;
            return View();
        }

        

        public JsonResult SaveDetails(string jsonData)
        {
            string StudentName = string.Empty;
            string  DOB = string.Empty;
            string Gender = string.Empty;
            string SchoolName = string.Empty;  
            string ClassStandard = string.Empty;
            string ParentName = string.Empty;
            string WhatsAppNo = string.Empty;
            string EmailId = string.Empty;
            string HomeAddress = string.Empty;
            //string RegNo = string.Empty;

           
            CompetitionDetail objdata = new CompetitionDetail();
            
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            foreach (Dictionary<string, object> item in objData)
            {
                StudentName = Convert.ToString(item["StudentFullName"]);
                DOB = Convert.ToString(item["DateOfBirth"]);
                Gender = Convert.ToString(item["Gender"]);
                SchoolName = Convert.ToString(item["SchoolName"]);
                ClassStandard = Convert.ToString(item["Class"]);
                ParentName = Convert.ToString(item["ParentsName"]);
                WhatsAppNo = Convert.ToString(item["WhatsAppNoOfParent"]);
                EmailId = Convert.ToString(item["EmailIdOfParent"]);
                HomeAddress = Convert.ToString(item["HomeAddress"]);
                //RegNo = Convert.ToString(item["RegNo"]);
            }
            var data = VR.SaveCompetitionData(StudentName, DOB,Gender, SchoolName, ClassStandard, ParentName, WhatsAppNo, EmailId, HomeAddress);
            return new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}