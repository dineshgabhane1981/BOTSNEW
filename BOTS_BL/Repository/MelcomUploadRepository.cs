using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL;
using BOTS_BL.Repository;
using System.IO;
using System.Web;

namespace BOTS_BL.Repository
{
    
    public class MelcomUploadRepository
    {
        Exceptions newexception = new Exceptions();
        public CustomerLoginDetail AuthenticateUser(CustomerLoginDetail objData)
        {
            //DatabaseDetail DBDetails = new DatabaseDetail();
            CustomerLoginDetail userDetail = new CustomerLoginDetail();
            try
            {
                using (var context = new CommonDBContext())
                {
                    //userDetail = context.LoginDetails.Where(a => a.LoginId == objData.LoginId && a.Password == objData.Password).FirstOrDefault();
                    userDetail = context.CustomerLoginDetails.Where(a => a.LoginId == objData.LoginId && a.Password == objData.Password).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");
            }
            return userDetail;

        }

        public bool SaveFile1(string FileDoc1, string FileName1)
        {
            bool status = default;
            
            try
            {
                FileName1 = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName1;

                byte[] imageBytes = Convert.FromBase64String(FileDoc1);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                var path = HttpContext.Current.Server.MapPath("~/Content/DataSheet/" + FileName1);
                FileStream fileNew = new FileStream(path, FileMode.Create, FileAccess.Write);
                ms.WriteTo(fileNew);
                fileNew.Close();
                ms.Close();

                status = true;
            }
            catch(Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");
            }

            return status;
        }
        public bool SaveFile2(string FileDoc2, string FileName2)
        {
            bool status = default;

            try
            {
                FileName2 = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName2;

                byte[] imageBytes1 = Convert.FromBase64String(FileDoc2);
                MemoryStream ms1 = new MemoryStream(imageBytes1, 0, imageBytes1.Length);
                ms1.Write(imageBytes1, 0, imageBytes1.Length);
                var path = HttpContext.Current.Server.MapPath("~/Content/DataSheet/" + FileName2);
                FileStream fileNew1 = new FileStream(path, FileMode.Create, FileAccess.Write);
                ms1.WriteTo(fileNew1);
                fileNew1.Close();
                ms1.Close();

                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");
            }

            return status;
        }
        public bool SaveFile3(string FileDoc3, string FileName3)
        {
            bool status = default;

            try
            {
                FileName3 = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName3;

                byte[] imageBytes3 = Convert.FromBase64String(FileDoc3);
                MemoryStream ms3 = new MemoryStream(imageBytes3, 0, imageBytes3.Length);
                ms3.Write(imageBytes3, 0, imageBytes3.Length);
                var path = HttpContext.Current.Server.MapPath("~/Content/DataSheet/" + FileName3);
                FileStream fileNew3 = new FileStream(path, FileMode.Create, FileAccess.Write);
                ms3.WriteTo(fileNew3);
                fileNew3.Close();
                ms3.Close();

                status = true;
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");
            }

            return status;
        }
    }
}
