using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;


namespace BOTS_BL.Repository
{

    public class CampaignRepository
    {
        Exceptions newexception = new Exceptions();
        public CampaignTiles GetCampaignTilesData(string GroupId, string connstr)
        {
            CampaignTiles objCampaignTiles = new CampaignTiles();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignTiles = context.Database.SqlQuery<CampaignTiles>("sp_BOTS_CampaignMeasurement @pi_GroupId, @pi_Date, @pi_LoginId ", 
                        new SqlParameter("@pi_GroupId", GroupId), 
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", "")).FirstOrDefault<CampaignTiles>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignTiles;

        }
        public List<CampaignCelebrations> GetCampaignCelebrationsData(string GroupId, string connstr,string month, string year)
        {
            List<CampaignCelebrations> objCampaignCelebrations = new List<CampaignCelebrations>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignCelebrations = context.Database.SqlQuery<CampaignCelebrations>("sp_BOTS_CM_Celebration @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignCelebrations>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignCelebrations;
        }

        public List<CampaignCelebrationsData> GetCampaignCelebrationsSecondData(string GroupId, string connstr, string month, string year,string type)
        {
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignCelebrationsData = context.Database.SqlQuery<CampaignCelebrationsData>("sp_BOTS_CM_Celebration1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_Type", type)).ToList<CampaignCelebrationsData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignCelebrationsData;
        }

        public List<CampaignPointsExpiry> GetCampaignPointsExpiryData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignPointsExpiry> objCampaignPointsExpiry = new List<CampaignPointsExpiry>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignPointsExpiry = context.Database.SqlQuery<CampaignPointsExpiry>("sp_BOTS_CM_PointsExpiry @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignPointsExpiry>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignPointsExpiry;
        }

        public List<CampaignCelebrationsData> GetCampaignPointsExpirySecondData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignCelebrationsData> objCampaignCelebrationsData = new List<CampaignCelebrationsData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignCelebrationsData = context.Database.SqlQuery<CampaignCelebrationsData>("sp_BOTS_CM_PointsExpiry1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignCelebrationsData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignCelebrationsData;
        }

        public List<CampaignInactive> GetCampaignInactiveData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignInactive> objCampaignInactive = new List<CampaignInactive>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignInactive = context.Database.SqlQuery<CampaignInactive>("sp_BOTS_CM_InActive @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignInactive>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignInactive;
        }

        public List<CampaignInactiveData> GetCampaignInactiveSecondData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignInactiveData> objCampaignInactiveData = new List<CampaignInactiveData>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignInactiveData = context.Database.SqlQuery<CampaignInactiveData>("sp_BOTS_CM_InActive1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignInactiveData>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }

            return objCampaignInactiveData;
        }
        public List<Campaign> GetCampaignFirstData(string GroupId, string connstr, string month, string year)
        {
            List<Campaign> objCampaignData = new List<Campaign>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<Campaign>("sp_BOTS_CM_Campaign @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<Campaign>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<CampaignSecond> GetCampaignSecondData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignSecond>("sp_BOTS_CM_Campaign1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignSecond>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<CampaignThird> GetCampaignThirdData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignThird>("sp_BOTS_CM_Campaign2 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignThird>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }


        public List<CampaignSMSBlastFirst> GetCampaignSMSBlastFirstData(string GroupId, string connstr, string month, string year)
        {
            List<CampaignSMSBlastFirst> objCampaignSMSBlastFirstData = new List<CampaignSMSBlastFirst>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignSMSBlastFirstData = context.Database.SqlQuery<CampaignSMSBlastFirst>("sp_BOTS_CM_SMSBlast @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year)).ToList<CampaignSMSBlastFirst>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignSMSBlastFirstData;
        }

        public List<CampaignSecond> GetSMSBlastsSecondData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignSecond> objCampaignData = new List<CampaignSecond>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignSecond>("sp_BOTS_CM_SMSBlast1 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignSecond>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }

        public List<CampaignThird> GetSMSBlastsThirdData(string GroupId, string connstr, string month, string year, string CampaignId)
        {
            List<CampaignThird> objCampaignData = new List<CampaignThird>();
            try
            {
                using (var context = new BOTSDBContext(connstr))
                {
                    objCampaignData = context.Database.SqlQuery<CampaignThird>("sp_BOTS_CM_SMSBlast2 @pi_GroupId, @pi_Date, @pi_LoginId, @pi_Month, @pi_Year",
                        new SqlParameter("@pi_GroupId", GroupId),
                        new SqlParameter("@pi_Date", DateTime.Now.ToShortDateString()),
                        new SqlParameter("@pi_LoginId", ""),
                        new SqlParameter("@pi_Month", month),
                        new SqlParameter("@pi_Year", year),
                        new SqlParameter("@pi_CampaignId", CampaignId)).ToList<CampaignThird>();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, GroupId);
            }
            return objCampaignData;
        }
    }
}
