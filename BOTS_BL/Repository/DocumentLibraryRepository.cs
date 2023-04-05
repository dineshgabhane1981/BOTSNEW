using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.S3.IO;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.Web.Mvc;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;

namespace BOTS_BL.Repository
{
    public class DocumentLibraryRepository
    {
        string AWSAccessKey, AWSSecretKey, AWSBucketName;
        //string AWSSecretKey = "";
        //string AWSBucketName = "";
        Exceptions newexception = new Exceptions();
        public bool UploadDocumentToS3(string fileData, string fileName, string Groupid, string GroupName, string Comment, string DocType, string Addedby, string Addeddate, string FinGroupid, string FinGroupName, string Department, string Vendor)
        {
            List<tblAWSAccessDetail> AccessDetails = new List<tblAWSAccessDetail>();
            bool status = false;
            try
            {
                using (var context = new CommonDBContext())
                {
                    AccessDetails = (from c in context.tblAWSAccessDetails where (c.SlNo == 1) select c).ToList();
                }
                foreach (var item in AccessDetails)
                {
                    AWSBucketName = Convert.ToString(item.BucketName);
                    AWSAccessKey = Convert.ToString(item.AccessKey);
                    AWSSecretKey = Convert.ToString(item.SecretKey);
                }

                IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.APSouth1);
                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                string path = @"Documents/" + GroupName;
                S3DirectoryInfo di = new S3DirectoryInfo(client, AWSBucketName, path);
                if (!di.Exists)
                {
                    di.Create();
                }
                request.BucketName = AWSBucketName;
                //request.Key = path + "/" + "test.xlsx";
                request.Key = path + "/" + fileName;

                byte[] newBytes = Convert.FromBase64String(fileData);
                MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                ms.Write(newBytes, 0, newBytes.Length);
                request.InputStream = ms;
                utility.Upload(request);
                status = true;

                string path2 = GroupName;

                using (var context = new CommonDBContext())
                {

                    tblDocumentsLibrary ObjDocLib = new tblDocumentsLibrary();
                    if (Convert.ToString(GroupName) == "Finance")
                    {
                        ObjDocLib.GroupId = Convert.ToString(FinGroupid);
                        if (FinGroupName == "Please Select")
                            ObjDocLib.GroupName = "Finance";
                        else
                            ObjDocLib.GroupName = Convert.ToString(FinGroupName);
                    }
                    else
                    {
                        ObjDocLib.GroupId = Convert.ToString(Groupid);
                        ObjDocLib.GroupName = Convert.ToString(GroupName);
                    }

                    ObjDocLib.DocumentType = Convert.ToString(DocType);
                    ObjDocLib.Path = path2 + "/" + fileName;
                    ObjDocLib.UploadedBy = Convert.ToString(Addedby);
                    ObjDocLib.UploadDate = Convert.ToDateTime(Addeddate);
                    ObjDocLib.Comments = Convert.ToString(Comment);
                    ObjDocLib.FileName = Convert.ToString(fileName);
                    if (Department != null && Department != "Please Select")
                        ObjDocLib.Department = Convert.ToString(Department);
                    else
                        ObjDocLib.Department = null;

                    if (Vendor != null && Vendor != "Please Select")
                        ObjDocLib.Vendor = Convert.ToString(Vendor);
                    else
                        ObjDocLib.Vendor = null;


                    context.tblDocumentsLibraries.Add(ObjDocLib);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "UploadDocumentToS3");
            }

            return status;
        }
        public List<SelectListItem> GetGroupDetails(string roleId, string loginId)
        {
            List<SelectListItem> lstGroupDetails = new List<SelectListItem>();
            List<tblGroupDetail> GroupDetails = new List<tblGroupDetail>();
            using (var context = new CommonDBContext())
            {
                if (roleId == "7")
                {
                    var rmAssignedId = context.tblRMAssigneds.Where(x => x.LoginId == loginId).Select(y => y.RMAssignedId).FirstOrDefault();
                    GroupDetails = context.tblGroupDetails.Where(x => x.IsActive == true && x.RMAssigned == rmAssignedId).ToList();
                }
                else
                {
                    GroupDetails = context.tblGroupDetails.Where(x => x.IsActive == true).ToList();
                    //var GroupDetails = context.WAReports.Where(x => x.SMSStatus == status).ToList();
                }

                foreach (var item in GroupDetails)
                {
                    lstGroupDetails.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)
                    });
                }
                lstGroupDetails = lstGroupDetails.OrderBy(x => x.Text).ToList();
            }
            return lstGroupDetails;
        }
        public List<tblDocumentsLibrary> GetDocLibData(string Groupid, string Dept,string Vendor)
        {
            List<tblDocumentsLibrary> ObjLibList = new List<tblDocumentsLibrary>();
            using (var context = new CommonDBContext())
            {
                try
                {
                    if(Vendor!=null & Dept!=null)
                    {
                        ObjLibList = context.tblDocumentsLibraries.Where(x => x.Vendor == Vendor).ToList();
                    }
                    else if (string.IsNullOrEmpty(Dept))
                        ObjLibList = context.tblDocumentsLibraries.Where(x => x.GroupId == Groupid && x.Department == null).ToList();
                    else
                    {
                        if (Dept == "Finance")
                            ObjLibList = context.tblDocumentsLibraries.Where(x => x.GroupId == Groupid && x.Department == Dept).ToList();
                        else
                            ObjLibList = context.tblDocumentsLibraries.Where(x => x.Department == Dept).ToList();
                    }


                    foreach (var item in ObjLibList)
                    {
                        item.UploadDateStr = item.UploadDate.Value.ToString("dd-MMM-yyyy");
                    }
                    ObjLibList = ObjLibList.OrderByDescending(x => x.UploadDate).ToList();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetDocLibData");
                }
            }
            return ObjLibList;
        }

        public List<SelectListItem> GetDolumentTypesByDept(string dept)
        {
            List<SelectListItem> lstDocumentType = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                try
                {
                    var DocumentTypes = context.tblDocumentTypes.Where(x => x.Dept == dept).ToList();
                    foreach (var item in DocumentTypes)
                    {
                        lstDocumentType.Add(new SelectListItem
                        {
                            Text = item.DocumentType,
                            Value = Convert.ToString(item.DocumentType)
                        });



                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetDolumentTypesByDept");
                }
            }
            return lstDocumentType;
        }
        public string GetDownloadData(string SLno)
        {
            bool status;
            status = false;
            List<tblDocumentsLibrary> ObjLibList = new List<tblDocumentsLibrary>();
            var slno = int.MaxValue;
            slno = (int)Convert.ToInt64(SLno);
            string FileName, Path;
            FileName = string.Empty;
            using (var context = new CommonDBContext())
            {
                try
                {
                    var ObjList = (from c in context.tblDocumentsLibraries where (c.SlNo == slno) select c).ToList();

                    foreach (var item in ObjList)
                    {
                        FileName = item.FileName;
                        Path = item.Path;
                        DownloadFileFromS3(FileName, Path);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetDownloadData");
                }

            }
            return FileName;

        }

        public string DownloadFileFromS3(string FileName, string Path)
        {
            string returnLocation = string.Empty;
            List<tblAWSAccessDetail> AccessDetails = new List<tblAWSAccessDetail>();
            try
            {

                //using (var context = new CRMContext())
                //{
                //var objDoc = context.tblCRMDocuments.FirstOrDefault(x => x.DocumentId == id);
                using (var context = new CommonDBContext())
                {
                    AccessDetails = (from c in context.tblAWSAccessDetails where (c.SlNo == 1) select c).ToList();
                }
                foreach (var item in AccessDetails)
                {
                    AWSBucketName = Convert.ToString(item.BucketName);
                    AWSAccessKey = Convert.ToString(item.AccessKey);
                    AWSSecretKey = Convert.ToString(item.SecretKey);
                }

                var filelink = Path;
                string _FilePath = ConfigurationManager.AppSettings["SharedLocation"];
                string FileLocation = _FilePath + "/" + FileName;
                FileStream fs = File.Create(FileLocation);
                fs.Close();
                string path = @"Documents/" + filelink;
                IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.APSouth1);
                TransferUtility fileTransferUtility = new TransferUtility(client);
                fileTransferUtility.Download(FileLocation, AWSBucketName, path);
                fileTransferUtility.Dispose();
                returnLocation = _FilePath;
                //}
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DownloadFileFromS3");
            }

            return returnLocation;
        }
    }
}
