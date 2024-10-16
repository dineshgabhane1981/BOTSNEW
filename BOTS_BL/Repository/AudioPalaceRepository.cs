﻿using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using BOTS_BL.Models;

namespace BOTS_BL.Repository
{
    public class AudioPalaceRepository
    {
        Exceptions newexception = new Exceptions();

        

        public LoginDetail AuthenticateUser(LoginDetail objData)
        {
            //DatabaseDetail DBDetails = new DatabaseDetail();
            LoginDetail userDetail = new LoginDetail();
            try
            {
                using (var context = new BOTSDBContext())
                {
                    userDetail = context.LoginDetails.Where(a => a.LoginId == objData.LoginId && a.Password == objData.Password).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "AuthenticateUser");
            }
            return userDetail;

        }

        


        public AudioPalace BulkTransaction(DataTable dt)
        {
            AudioPalace Obj = new AudioPalace();
            int c = 0;
            Obj.Status = false;
            try
            {
                Obj.TbleRWCount = Convert.ToString(dt.Rows.Count);

                for (int i = 0; i < dt.Rows.Count; i ++)
                {
                    
                    var conStr = ConfigurationManager.ConnectionStrings["BOTSDBContext"].ToString();
                    using (var context = new BOTSDBContext(conStr))
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlConnection Con = new SqlConnection(conStr);
                        cmd.Connection = Con;
                        cmd.CommandTimeout = 80000;

                        cmd.Parameters.AddWithValue("@pi_OutletCode", Convert.ToString(dt.Rows[i]["OutletCode"]));
                        cmd.Parameters.AddWithValue("@pi_MobileNo", Convert.ToString(dt.Rows[i]["MobileNo"]));
                        cmd.Parameters.AddWithValue("@pi_CustomerName", Convert.ToString(dt.Rows[i]["CustomerName"]));
                        cmd.Parameters.AddWithValue("@pi_TxnDateText", dt.Rows[i]["TxnDate"]);      
                        cmd.Parameters.AddWithValue("@pi_CategoryCode", Convert.ToString(dt.Rows[i]["CategoryCode"]));
                       // cmd.Parameters.AddWithValue("@pi_CategoryName", Convert.ToString(dt.Rows[i]["CategoryName"]));
                        cmd.Parameters.AddWithValue("@pi_SubCategoryCode", Convert.ToString(dt.Rows[i]["SubCategoryCode"]));
                        //cmd.Parameters.AddWithValue("@pi_SubCategoryName", Convert.ToString(dt.Rows[i]["SubCategoryName"]));
                        cmd.Parameters.AddWithValue("@pi_ProductCode", Convert.ToString(dt.Rows[i]["ProductCode"]));
                        //cmd.Parameters.AddWithValue("@pi_ProductName", Convert.ToString(dt.Rows[i]["ProductName"]));
                        cmd.Parameters.AddWithValue("@pi_ProductCount", Convert.ToString(dt.Rows[i]["ProductCount"]));
                        cmd.Parameters.AddWithValue("@pi_InvoiceAmt", dt.Rows[i]["InvoiceAmt"]);
                        Con.Open();
                        cmd.CommandText = "sp_RetailAppTxnUpload";
                        cmd.CommandType = CommandType.StoredProcedure;
                        string _returnvalue = (string)cmd.ExecuteScalar();
                        Con.Close();

                        

                        
                        Obj.Status  = true;
                        if(_returnvalue == "0")
                        {
                            c++;
                        }

                    }
                    ;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "BulkInsert");
            }
            Obj.DBInsertCount = Convert.ToString(c);
            Obj.DBFailedCount = Convert.ToString(c);
            return Obj;
        }
    }
}
