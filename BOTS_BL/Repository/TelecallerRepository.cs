using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
   public class TelecallerRepository
    {
        Exceptions newexception = new Exceptions();

        public TelecallerCustomerData GetTelecallerCustomer(string MobileNo,string GroupId,string connstr)
        {
            TelecallerCustomerData objteledata = new TelecallerCustomerData();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objteledata = context.Database.SqlQuery<TelecallerCustomerData>("SP_BOTS_TelecallerData @pi_mobileNo",
                           new SqlParameter("@pi_mobileNo", MobileNo)).FirstOrDefault<TelecallerCustomerData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objteledata;

        }
        public bool SaveTelecallerTracking(string connstr,string AddedBy,string MobileNo,string CustomerNm,string Gender,DateTime DateofBirth,DateTime DateOfAnni,int PointsGiven,string OutletId,string Comments)
        {
            bool status = false;
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    TelecallerTracking objtracking = new TelecallerTracking();
                    objtracking.AddedBy = AddedBy;
                    objtracking.AddedDate = DateTime.Now;
                    objtracking.Comments = Comments;
                    objtracking.CustomerName = CustomerNm;
                    objtracking.DOA = DateOfAnni;
                    objtracking.DOB = DateofBirth;
                    objtracking.Gender = Gender;
                    objtracking.IsSMSSend = false;
                    objtracking.MobileNo = MobileNo;
                    objtracking.OutletId = OutletId;
                    objtracking.Points = 0;
                    
                    context.TelecallerTrackings.Add(objtracking);
                    context.SaveChanges();                 
                    
                    status = true;
                }
               
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "");
            }

            return status;
        }
    }
}
