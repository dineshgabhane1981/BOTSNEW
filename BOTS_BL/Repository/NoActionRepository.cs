using BOTS_BL.Models.ChitaleModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BOTS_BL.Repository
{
    public class NoActionRepository
    {
        public NoActionModelTile GetNoActionParticipantsTilesData()
        {
            NoActionModelTile objData = new NoActionModelTile();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    objData = context.Database.SqlQuery<NoActionModelTile>("Chitale_NoActionByParticipant1 @pi_Date, @pi_CustomerId, @pi_CustomerType",
                                  new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                                  new SqlParameter("@pi_CustomerId", ""),
                                  new SqlParameter("@pi_CustomerType", "")).FirstOrDefault<NoActionModelTile>();
                }
                catch (Exception ex)
                {

                }
            }
            return objData;

        }

        public List<NoActionParticipantData> GetNoActionParticipantsData(string type)
        {
            List<NoActionParticipantData> objData = new List<NoActionParticipantData>();
            using (var context = new ChitaleDBContext())
            {
                try
                {
                    objData = context.Database.SqlQuery<NoActionParticipantData>("Chitale_NoActionByParticipant2 @pi_Date, @pi_CustomerId, @pi_CustomerType,@pi_SelectedType",
                                  new SqlParameter("@pi_Date", DateTime.Now.ToString("yyyy-MM-dd")),
                                  new SqlParameter("@pi_CustomerId", ""),
                                  new SqlParameter("@pi_CustomerType", ""),
                                  new SqlParameter("@pi_SelectedType", type)).ToList<NoActionParticipantData>();

                    foreach(var item in objData)
                    {
                        item.LastInvoiceDateStr = item.LastInvoiceDate.ToString("dd-MM-yyyy");
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return objData;

        }
    }
}
