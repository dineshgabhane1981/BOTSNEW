using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class ParticipantRepository
    {
        ChitaleException newexception = new ChitaleException();
        public List<ParticipantList> GetParticipantList(string CustomerId, string CustomerType)
        {

            List<ParticipantList> lstparticipantLists = new List<ParticipantList>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    lstparticipantLists = context.Database.SqlQuery<ParticipantList>("Chitale_ParticipantList @pi_LoginId, @pi_Datetime, @pi_CustomerId, @pi_CustomerType",
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Datetime", DateTime.Now.ToString("yyyy-MM-dd")),
                        new SqlParameter("@pi_CustomerId", CustomerId),
                        new SqlParameter("@pi_CustomerType", CustomerType)).ToList<ParticipantList>();


                    

                    if (lstparticipantLists != null)
                    {
                        lstparticipantLists = lstparticipantLists.OrderBy(x => Convert.ToInt32(x.Rank)).ToList();
                    }
                    List<ParticipantList> lstparticipantListsNew = new List<ParticipantList>();
                    foreach (var item in lstparticipantLists)
                    {
                        if (item.Rank == "0")
                        {
                            lstparticipantListsNew.Add(item);                            
                        }
                    }
                    foreach (var item in lstparticipantListsNew)
                    {
                        lstparticipantLists.Remove(item);                        
                    }

                    lstparticipantLists.AddRange(lstparticipantListsNew);
                }


            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }

            return lstparticipantLists;
        }


        public RedemptionModel GetRedemptionData(string CustomerId)
        {
            RedemptionModel objData = new RedemptionModel();
            CustomerDetail objCustomerDetail = new CustomerDetail();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objCustomerDetail = context.CustomerDetails.Where(x => x.CustomerId == CustomerId).FirstOrDefault();
                    if (objCustomerDetail != null)
                    {
                        objData.DepositData = objCustomerDetail.Deposit;
                        objData.CreditData = objCustomerDetail.CashIncentive;
                        objData.InfraData = objCustomerDetail.InfraStructure;
                        objData.PromoData = objCustomerDetail.Promotion;

                        objData.DepositDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.Deposit));
                        objData.CreditDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.CashIncentive));
                        objData.InfraDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.InfraStructure));
                        objData.PromoDataStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDecimal(objCustomerDetail.Promotion));
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return objData;
        }

        public bool GenerateOTP(OTPMaintenance objOTP)
        {
            bool status = false;
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    context.OTPMaintenances.Add(objOTP);
                    context.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return status;
        }

        public string GetOTP(string MobileNo)
        {
            string OTP = string.Empty;
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    OTP = context.OTPMaintenances.Where(x => x.MobileNo == MobileNo).OrderByDescending(z => z.Datetime).Select(y => y.OTP).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return OTP;
        }

        public bool RedeemptionRequest(tblRedemptionRequest objRedeem)
        {
            bool status = false;
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    context.tblRedemptionRequests.Add(objRedeem);
                    context.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return status;
        }


        public List<OrderVsRavanaDay> GetOrderVsRavanaDayData(string CustomerId)
        {
            List<OrderVsRavanaDay> objData = new List<OrderVsRavanaDay>();
            using (var context = new ChitaleDBContext())
            {
                try
                {

                    objData = context.OrderVsRavanaDays.Where(x => x.CustomerId == CustomerId).ToList();

                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }

            return objData;
        }

        public NoActionModelTile GetNoActionParticipantsTilesData(string CustomerId, string CustomerType)
        {
            NoActionModelTile objData = new NoActionModelTile();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    //if (CustomerType == "Sales Executive")
                    //{
                    //    CustomerType = "SalesExecutive";
                    //}
                    //else if (CustomerType == "ASM (Sales Manager)")
                    //{
                    //    CustomerType = "AreaSalesManager";
                    //}
                    //else if (CustomerType == "Sales Officer")
                    //{
                    //    CustomerType = "SalesOfficer";
                    //}
                    //else if (CustomerType == "Sales Representative")
                    //{
                    //    CustomerType = "SalesRepresentative";
                    //}
                    //else if (CustomerType == "State Head")
                    //{
                    //    CustomerType = "StateHead";
                    //}
                    //else if (CustomerType == "Zonal Head")
                    //{
                    //    CustomerType = "ZonalHead";
                    //}
                    //else if (CustomerType == "National Head")
                    //{
                    //    CustomerType = "NationalHead";
                    //}
                    objData = context.Database.SqlQuery<NoActionModelTile>("Chitale_NoActionByParticipant1 @pi_Date, @pi_CustomerId, @pi_CustomerType",
                                  new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                                  new SqlParameter("@pi_CustomerId", CustomerId),
                                  new SqlParameter("@pi_CustomerType", CustomerType)).FirstOrDefault<NoActionModelTile>();
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }
            return objData;

        }

        public List<NoActionParticipantData> GetNoActionParticipantsData(string type, string CustomerId, string CustomerType)
        {
            List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    //if (CustomerType == "Sales Executive")
                    //{
                    //    CustomerType = "SalesExecutive";
                    //}
                    //else if (CustomerType == "ASM (Sales Manager)")
                    //{
                    //    CustomerType = "AreaSalesManager";
                    //}
                    //else if (CustomerType == "Sales Officer")
                    //{
                    //    CustomerType = "SalesOfficer";
                    //}
                    //else if (CustomerType == "Sales Representative")
                    //{
                    //    CustomerType = "SalesRepresentative";
                    //}
                    //else if (CustomerType == "State Head")
                    //{
                    //    CustomerType = "StateHead";
                    //}
                    //else if (CustomerType == "Zonal Head")
                    //{
                    //    CustomerType = "ZonalHead";
                    //}
                    //else if (CustomerType == "National Head")
                    //{
                    //    CustomerType = "NationalHead";
                    //}
                    objData = context.Database.SqlQuery<NoActionParticipantData>("Chitale_NoActionByParticipant2 @pi_Date, @pi_CustomerId, @pi_CustomerType,@pi_SelectedType",
                                  new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                                  new SqlParameter("@pi_CustomerId", CustomerId),
                                  new SqlParameter("@pi_CustomerType", CustomerType),
                                  new SqlParameter("@pi_SelectedType", type)).ToList<NoActionParticipantData>();

                    foreach (var item in objData)
                    {
                        item.LastInvoiceDateStr = item.LastInvoiceDate.ToString("dd-MM-yyyy");
                        item.BalancePointsStr = String.Format(new CultureInfo("en-IN", false), "{0:n}", Convert.ToDouble(item.BalancePoints));
                    }

                    if (objData != null)
                    {
                        objData = objData.OrderBy(x => Convert.ToInt32(x.DaysSinceLastInvoice)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    newexception.AddException(ex);
                }
            }
            return objData;

        }
    }
}
