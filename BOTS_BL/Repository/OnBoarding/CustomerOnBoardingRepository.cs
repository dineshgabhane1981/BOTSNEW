using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOTS_BL.Models;
using BOTS_BL.Models.CommonDB;


namespace BOTS_BL.Repository
{
    public class CustomerOnBoardingRepository
    {
        Exceptions newexception = new Exceptions();
        public List<tblCategory> GetTblCategories()
        {
            List<tblCategory> objtblcategory = new List<tblCategory>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    objtblcategory = context.tblCategories.ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex,"onboarding_master");
            }
            return objtblcategory;
        }
    }
}
