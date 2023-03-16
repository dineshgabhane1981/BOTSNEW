using BOTS_BL.Models;
using BOTS_BL.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModel;

namespace WebApp.Controllers
{
    public class DocumentLibraryController : Controller
    {
        DocumentLibraryRepository DLR = new DocumentLibraryRepository();
        // GET: DocumentLibrary
        public ActionResult Index()
        {
            //tblGroupDetail objData = new tblGroupDetail();
            // objData = DLR.GetGroupDetails();
            //return View(objData);
            DocumentLibraryViewModel objdata = new DocumentLibraryViewModel();
            objdata.lstGroupDetails = DLR.GetGroupDetails();
            return View(objdata);
        }
        public bool UploadDocument(string fileData)
        {
            bool status = false;

            status = DLR.UploadDocumentToS3(fileData);
            return status;
        }

        

        //public ActionResult GroupList(string groupId)
        //{
        //    tblGroupDetail objData = new tblGroupDetail();
        //    var lstGroupList = DLR.GetGroupDetails();

        //    return View(objData);
        //}
    }
}