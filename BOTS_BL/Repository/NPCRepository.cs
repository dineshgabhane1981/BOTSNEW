using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class NPCRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerRepository CR = new CustomerRepository();
        public bool SaveNPCData(tblNPCDetail objOutletData)
        {
            bool result = false;
            using (var context = new BOTSDBContext())
            {
                try
                {

                    context.tblNPCDetails.AddOrUpdate(objOutletData);
                    context.SaveChanges();
                    result = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "SaveNPCData");
                }
            }
            return result;
        }

        public string CheckLogin(NPCLoginDetail objData)
        {           
            string GroupId = "";
            try
            {
                using (var context = new CommonDBContext())
                {
                    //GroupId
                    var objNew = context.NPCLoginDetails.Where(x => x.LoginId == objData.LoginId && x.Password == objData.Password).FirstOrDefault();
                    if (objNew != null)
                        GroupId = objNew.GroupId;
                    else
                    {
                        newexception.AddDummyException("Object is Null");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "CheckLogin");
            }
            return GroupId;
        }

        public List<SelectListItem> GetOutlets(string groupId)
        {
            List<SelectListItem> lstOutlets = new List<SelectListItem>();
            string connectionString = CR.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    var objData = context.OutletDetails.ToList();
                    foreach (var item in objData)
                    {
                        SelectListItem lstItem = new SelectListItem();
                        lstItem.Value = Convert.ToString(item.OutletName);
                        lstItem.Text = item.OutletName;
                        lstOutlets.Add(lstItem);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetOutlets");
                }
            }
            return lstOutlets;
        }

        public List<SelectListItem> GetNPCEmployees(string groupId)
        {
            List<SelectListItem> lstNPCEmployees = new List<SelectListItem>();
            string connectionString = CR.GetCustomerConnString(groupId);
            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    var objData = context.tblEmployees.ToList();
                    foreach (var item in objData)
                    {
                        SelectListItem lstItem = new SelectListItem();
                        lstItem.Value = Convert.ToString(item.EmpName);
                        lstItem.Text = item.EmpName;
                        lstNPCEmployees.Add(lstItem);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetNPCEmployees");
                }
            }
            return lstNPCEmployees;
        }
        public List<SelectListItem> GetNPCCategory(string groupId)
        {
            List<SelectListItem> lstNPCCategory = new List<SelectListItem>();
            List<tblNPCCategory> objData = new List<tblNPCCategory>();
            //string ConnectionString = string.Empty;
            string connectionString = CR.GetCustomerConnString(groupId);

            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    objData = context.tblNPCCategories.ToList();

                    foreach (var item in objData)
                    {
                        SelectListItem lstItem = new SelectListItem();
                        lstItem.Value = Convert.ToString(item.CategoryId);
                        lstItem.Text = item.CategoryName;
                        lstNPCCategory.Add(lstItem);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetNPCCategory");
                }
            }

            return lstNPCCategory;
        }

        public List<SelectListItem> GetNPCSubCategory(string groupId,int cId)
        {
            List<SelectListItem> lstNPCSubCategory = new List<SelectListItem>();
            List<tblNPCSubCategory> objData = new List<tblNPCSubCategory>();
            //string ConnectionString = string.Empty;
            string connectionString = CR.GetCustomerConnString(groupId);

            using (var context = new BOTSDBContext(connectionString))
            {
                try
                {
                    objData = context.tblNPCSubCategories.Where(x => x.CategoryId == cId).ToList();

                    foreach (var item in objData)
                    {
                        SelectListItem lstItem = new SelectListItem();
                        lstItem.Value = Convert.ToString(item.SubCategoryname);
                        lstItem.Text = item.SubCategoryname;
                        lstNPCSubCategory.Add(lstItem);
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex, "GetNPCSubCategory");
                }
            }

            return lstNPCSubCategory;
        }

        public string GetLogo(string groupId)
        {
            string logoUrl = string.Empty;

            string connectionString = CR.GetCustomerConnString(groupId);
            try
            {
                using (var context = new BOTSDBContext(connectionString))
                {
                    var objbrandDetail = context.BrandDetails.Where(x => x.GroupId == groupId).FirstOrDefault();
                    logoUrl = objbrandDetail.BrandLogoUrl;
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetLogo");
            }
            return logoUrl;

        }

    }

}
