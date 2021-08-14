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

namespace BOTS_BL.Repository
{
    public class OnBoardingRepository
    {
        Exceptions newexception = new Exceptions();
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
                        var LastGroupId = context.BOTS_TblGroupMaster.OrderByDescending(x => x.GroupId).Take(1).Select(y => y.GroupId).FirstOrDefault();

                        if (!string.IsNullOrEmpty(LastGroupId))
                        {
                            GroupId = Convert.ToInt32(LastGroupId) + 1;
                        }
                        else
                        {
                            GroupId = 1001;
                        }
                        objGroup.GroupId = Convert.ToString(GroupId);
                        context.BOTS_TblGroupMaster.AddOrUpdate(objGroup);
                        context.SaveChanges();

                        foreach (var item in objLstRetail)
                        {
                            item.GroupId = Convert.ToString(GroupId);
                            context.BOTS_TblRetailMaster.AddOrUpdate(item);
                            context.SaveChanges();
                        }

                        objDeal.GroupId= Convert.ToString(GroupId);
                        context.BOTS_TblDealDetails.AddOrUpdate(objDeal);
                        context.SaveChanges();

                        objPayment.GroupId = Convert.ToString(GroupId);
                        context.BOTS_TblPaymentDetails.AddOrUpdate(objPayment);
                        context.SaveChanges();

                        foreach(var item in objLstInstallment)
                        {
                            item.GroupId = Convert.ToString(GroupId);
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
    }
}
