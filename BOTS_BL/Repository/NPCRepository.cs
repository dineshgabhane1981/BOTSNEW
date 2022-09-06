using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using System.Data.Entity.Migrations;

namespace BOTS_BL.Repository
{
    public class NPCRepository
    {
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
                    //newexception.AddException(ex, "SaveNPCData");
                }
            }
            return result;
        }

        public string CheckLogin(NPCLoginDetail objData)
        {
            string GroupId = "";
            using(var context =new CommonDBContext())
            {
                GroupId = context.NPCLoginDetails.Where(x => x.LoginId == objData.LoginId && x.Password == objData.Password).Select(y => y.GroupId).FirstOrDefault();
            }
            return GroupId;
        }
    }

}
