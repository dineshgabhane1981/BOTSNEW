using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.IO;
using BOTS_BL;
using System.Data.Entity.Validation;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;

namespace BOTS_BL.Repository
{
    public class OnBoardingRepository
    {
        Exceptions newexception = new Exceptions();
        CustomerOnBoardingRepository COBR = new CustomerOnBoardingRepository();
        public bool AddOnboardingCustomer(BOTS_TblGroupMaster objGroup, List<BOTS_TblRetailMaster> objLstRetail,
            BOTS_TblDealDetails objDeal, BOTS_TblPaymentDetails objPayment, List<BOTS_TblInstallmentDetails> objLstInstallment)
        {
            bool status = false;
            var GroupId = 0;
            using (var context = new CommonDBContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(objGroup.GroupId))
                        {
                            var LastGroupId = context.BOTS_TblGroupMaster.OrderByDescending(x => x.GroupId).Take(1).Select(y => y.GroupId).FirstOrDefault();

                            if (!string.IsNullOrEmpty(LastGroupId))
                            {
                                GroupId = Convert.ToInt32(LastGroupId) + 1;
                            }
                            else
                            {
                                GroupId = 2001;
                            }
                            objGroup.GroupId = Convert.ToString(GroupId);
                        }

                        var path = ConfigurationManager.AppSettings["CustomerDocuments"].ToString();
                        var ClientFolder = path + "\\" + GroupId;
                        if (!Directory.Exists(ClientFolder))
                        {
                            System.IO.Directory.CreateDirectory(ClientFolder);
                        }

                        if (objGroup.PANDocumentFile != null)
                        {
                            using (Stream inputStream = objGroup.PANDocumentFile.InputStream)
                            {
                                byte[] data;
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    inputStream.CopyTo(ms);
                                    data = ms.ToArray();
                                }
                                System.IO.File.WriteAllBytes(Path.Combine(ClientFolder, objGroup.PANDocumentFile.FileName), data);
                            }
                            objGroup.PANDocument = objGroup.PANDocumentFile.FileName;
                        }
                        if (objGroup.GSTDocumentFile != null)
                        {
                            using (Stream inputStream = objGroup.GSTDocumentFile.InputStream)
                            {
                                byte[] data;
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    inputStream.CopyTo(ms);
                                    data = ms.ToArray();
                                }
                                System.IO.File.WriteAllBytes(Path.Combine(ClientFolder, objGroup.GSTDocumentFile.FileName), data);
                            }
                            objGroup.GSTDocument = objGroup.GSTDocumentFile.FileName;
                        }

                                               
                        
                        context.BOTS_TblGroupMaster.AddOrUpdate(objGroup);
                        context.SaveChanges();

                        var lstRetail = context.BOTS_TblRetailMaster.Where(x => x.GroupId == objGroup.GroupId).ToList();
                        foreach(var item in lstRetail)
                        {
                            context.BOTS_TblRetailMaster.Remove(item);
                            context.SaveChanges();
                        }

                        foreach (var item in objLstRetail)
                        {
                            item.GroupId = Convert.ToString(objGroup.GroupId);
                            context.BOTS_TblRetailMaster.AddOrUpdate(item);
                            context.SaveChanges();
                        }

                        objDeal.GroupId = Convert.ToString(objGroup.GroupId);
                        context.BOTS_TblDealDetails.AddOrUpdate(objDeal);
                        context.SaveChanges();

                        objPayment.GroupId = Convert.ToString(objGroup.GroupId);
                        context.BOTS_TblPaymentDetails.AddOrUpdate(objPayment);
                        context.SaveChanges();

                        var lstInstallments = context.BOTS_TblInstallmentDetails.Where(x => x.GroupId == objGroup.GroupId).ToList();
                        foreach (var item in lstInstallments)
                        {
                            context.BOTS_TblInstallmentDetails.Remove(item);
                            context.SaveChanges();
                        }

                        foreach (var item in objLstInstallment)
                        {
                            item.GroupId = Convert.ToString(objGroup.GroupId);
                            context.BOTS_TblInstallmentDetails.AddOrUpdate(item);
                            context.SaveChanges();
                        }

                        transaction.Commit();
                        status = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        newexception.AddException(ex, "OnBoarding");
                        //throw ex;
                    }
                }
            }
            return status;
        }


        public List<OnBoardingListing> GetOnBoardingListings(string loginType)
        {
            List<OnBoardingListing> onBoardingListings = new List<OnBoardingListing>();
            List<BOTS_TblGroupMaster> lstGroups = new List<BOTS_TblGroupMaster>();
            using (var context = new CommonDBContext())
            {
                if (loginType == "1")
                {
                    lstGroups = context.BOTS_TblGroupMaster.ToList();
                }
                if (loginType == "5")
                {
                    lstGroups = context.BOTS_TblGroupMaster.Where(x => x.CustomerStatus == "Draft").ToList();
                }

                foreach (var item in lstGroups)
                {
                    OnBoardingListing objItem = new OnBoardingListing();
                    objItem.GroupId = Convert.ToInt32(item.GroupId);
                    objItem.GroupName = item.GroupName;
                    objItem.OwnerMobileNo = item.OwnerMobileNo;
                    var city = COBR.GetCityById(Convert.ToInt32(item.City));
                    objItem.City = city.CityName;
                    objItem.PaymentStatus = context.BOTS_TblDealDetails.Where(x => x.GroupId == item.GroupId).Select(y => y.PaymentStatus).FirstOrDefault();
                    onBoardingListings.Add(objItem);
                }
            }

            return onBoardingListings;
        }

        public BOTS_TblGroupMaster GetGroupMasterDetails(string GroupId)
        {
            BOTS_TblGroupMaster objData = new BOTS_TblGroupMaster();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblGroupMaster.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objData;
        }

        public BOTS_TblDealDetails GetDealMasterDetails(string GroupId)
        {
            BOTS_TblDealDetails objData = new BOTS_TblDealDetails();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblDealDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objData;
        }

        public BOTS_TblPaymentDetails GetPaymentDetails(string GroupId)
        {
            BOTS_TblPaymentDetails objData = new BOTS_TblPaymentDetails();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblPaymentDetails.Where(x => x.GroupId == GroupId).FirstOrDefault();
            }
            return objData;
        }

        public List<BOTS_TblRetailMaster> GetRetailDetails(string GroupId)
        {
            List<BOTS_TblRetailMaster> objData = new List<BOTS_TblRetailMaster>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblRetailMaster.Where(x => x.GroupId == GroupId).ToList();
            }
            return objData;
        }

        public List<BOTS_TblInstallmentDetails> GetInstallmentDetails(string GroupId)
        {
            List<BOTS_TblInstallmentDetails> objData = new List<BOTS_TblInstallmentDetails>();
            using (var context = new CommonDBContext())
            {
                objData = context.BOTS_TblInstallmentDetails.Where(x => x.GroupId == GroupId).ToList();
                foreach(var item in objData)
                {
                    item.PaymentDateStr = item.PaymentDate.ToString("yyyy-MM-dd");
                }
            }
            return objData;
        }
    }
}
