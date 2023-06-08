using BOTS_BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Repository
{
    public class ITCSRepository
    {
        Exceptions newexception = new Exceptions();
        public List<tblGroupDetail> GetNeverOptForGroups(bool status)
        {
            List<tblGroupDetail> lstData = new List<tblGroupDetail>();
            try
            {
                using (var context = new CommonDBContext())
                {
                    if (status)
                        lstData = context.tblGroupDetails.Where(x => x.IsActive == true).OrderBy(x => x.GroupName).ToList();
                    else
                        lstData = context.tblGroupDetails.Where(x => x.IsActive == false).OrderBy(x => x.GroupName).ToList();
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "GetNeverOptForGroups");
            }
            return lstData;
        }

        public bool DisableProgrammeDetails(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    GroupDetail obj1 = new GroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsActive = false;
                        groupDetail.IsLive = false;
                        
                        context.SaveChanges();
                        status = true;

                        
                        var groupdetails = context.GroupDetail.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if(groupdetails.ActiveStatus == "Yes")
                        {
                            groupdetails.ActiveStatus = "No";
                        }
                        else
                        {
                            groupdetails.ActiveStatus = "No";
                        }
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "DisableProgrammeDetails");
            }
            return status;
        }
        public bool EnableProgrammeDetails(string GroupId)
        {
            bool status = false;
            try
            {
                {
                    tblGroupDetail obj = new tblGroupDetail();
                    GroupDetail obj1 = new GroupDetail();
                    int varid = Convert.ToInt32(GroupId);
                    using (var context = new CommonDBContext())
                    {
                        var groupDetail = context.tblGroupDetails.Where(x => x.GroupId == varid).FirstOrDefault();
                        groupDetail.IsActive = true;
                        groupDetail.IsLive = true;
                        context.SaveChanges();
                        status = true;

                        var groupdetails = context.GroupDetail.Where(x => x.GroupId == GroupId).FirstOrDefault();
                        if (groupdetails.ActiveStatus == "No")
                        {
                            groupdetails.ActiveStatus = "Yes";
                        }
                        else
                        {
                            groupdetails.ActiveStatus = "Yes";
                        }
                        context.SaveChanges();
                        status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex, "EnableProgrammeDetails");
            }
            return status;
        }
    }
}
