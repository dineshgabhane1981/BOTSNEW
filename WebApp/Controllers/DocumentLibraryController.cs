﻿using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using BOTS_BL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;
using System.Web.Script.Serialization;
using BOTS_BL;

namespace WebApp.Controllers
{
    public class DocumentLibraryController : Controller
    {
        DocumentLibraryRepository DLR = new DocumentLibraryRepository();
        Exceptions newexception = new Exceptions();
        // GET: DocumentLibrary
        public ActionResult Index()
        {
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            DocumentLibraryViewModel objdata = new DocumentLibraryViewModel();
            List<tblDocumentsLibrary> ObjList = new List<tblDocumentsLibrary>();
            objdata.lstGroupDetails = DLR.GetGroupDetails(userDetails.LoginType, userDetails.LoginId);
            objdata.lstDocumentType = DLR.GetDolumentTypesByDept("Customer");
            objdata.roleId = userDetails.LoginType;
            return View(objdata);
        }
        public bool UploadDocument(DocLibraryUploadFile objFileData)//string fileData, string fileName, string Groupid, string GroupName, string Comment, string DocType)
        {
            bool status = false;

            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            string Addedby = userDetails.LoginId;
            string Addeddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string fileData = objFileData.fileData;
            string fileName = objFileData.fileName;
            string Groupid = objFileData.Groupid;
            string GroupName = objFileData.GroupName;
            string Comment = objFileData.Comment;
            string DocType = objFileData.DocType;
            string FinGroupid = objFileData.FinGroupId;
            string FinGroupName = objFileData.FinGroupName;
            string Department = objFileData.Department;
            string Vendor = objFileData.Vendor;
            status = DLR.UploadDocumentToS3(fileData, fileName, Groupid, GroupName, Comment, DocType, Addedby, Addeddate, FinGroupid, FinGroupName, Department, Vendor);
            return status;
        }

        [HttpPost]
        public JsonResult ListDocuments(string GroupId, string Dept,string Vendor)
        {
            List<tblDocumentsLibrary> Doclist = new List<tblDocumentsLibrary>();
            try
            {
                var userDetails = (CustomerLoginDetail)Session["UserSession"];
                Doclist = DLR.GetDocLibData(GroupId, Dept, Vendor);
                //if (!string.IsNullOrEmpty(Dept))
                //{
                //    Doclist = DLR.GetDocLibData(GroupId, Dept);
                //}
                //else
                //{
                //    foreach (Dictionary<string, object> item in objData)
                //    {
                //        GroupId = Convert.ToString(item["GroupId"]);

                //        Doclist = DLR.GetDocLibData(GroupId, Dept);
                //    }
                //}
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "ListDocuments");
            }


            return new JsonResult() { Data = Doclist, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [HttpPost]
        public JsonResult GetDownloadDetails(string jsonData)
        {
            string Filename = string.Empty;
            var userDetails = (CustomerLoginDetail)Session["UserSession"];
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            json_serializer.MaxJsonLength = int.MaxValue;
            object[] objData = (object[])json_serializer.DeserializeObject(jsonData);
            try
            {
                foreach (Dictionary<string, object> item in objData)
                {
                    string SLno = Convert.ToString(item["SLno"]);

                    Filename = DLR.GetDownloadData(SLno);
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetDownloadDetails");
            }
            return new JsonResult() { Data = Filename, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    
        public JsonResult GetDocumentTypes(string Type)
        {   
              var objData = DLR.GetDolumentTypesByDept(Type);
                                     
            return new JsonResult() { Data = objData, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    
    }
}