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

namespace BOTS_BL.Repository
{
    public class DocumentLibraryRepository
    {
        string AWSAccessKey = "";
        string AWSSecretKey = "";
        string AWSBucketName = "";
        
        public bool UploadDocumentToS3(string fileData)
        {

            bool status = false;
            try
            {
                IAmazonS3 client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.APSouth1);
                TransferUtility utility = new TransferUtility(client);
                TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
                string path = @"Documents";
                S3DirectoryInfo di = new S3DirectoryInfo(client, AWSBucketName, path);
                if (!di.Exists)
                {
                    di.Create();
                }
                request.BucketName = AWSBucketName;
                request.Key = path + "/" + "test.xlsx";

                byte[] newBytes = Convert.FromBase64String(fileData);
                MemoryStream ms = new MemoryStream(newBytes, 0, newBytes.Length);
                ms.Write(newBytes, 0, newBytes.Length);
                request.InputStream = ms;
                utility.Upload(request);
                status = true;
            }
            catch(Exception ex)
            {

            }

            return status;
        }

        

        public List<SelectListItem> GetGroupDetails()
        {
            List<SelectListItem> lstGroupDetails = new List<SelectListItem>();
            using (var context = new CommonDBContext())
            {
                string status = "0";
                var GroupDetails = context.tblGroupDetails.Where(x => x.IsActive == true).ToList();
                //var GroupDetails = context.WAReports.Where(x => x.SMSStatus == status).ToList();

                foreach (var item in GroupDetails)
                {
                    lstGroupDetails.Add(new SelectListItem
                    {
                        Text = item.GroupName,
                        Value = Convert.ToString(item.GroupId)
                    });
                }
            }
            return lstGroupDetails;


        }
    }

}
