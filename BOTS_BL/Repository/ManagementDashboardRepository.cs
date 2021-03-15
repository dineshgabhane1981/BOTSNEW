using BOTS_BL.Models;
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
    public class ManagementDashboardRepository
    {
        Exceptions newexception = new Exceptions();

        public List<SelectListItem> GetClusterList()
        {
            List<ClusterMaster> objlist = new List<ClusterMaster>();
            List<SelectListItem> clusterItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objlist = context.ClusterMasters.ToList();
                    foreach (var item in objlist)
                    {
                        clusterItems.Add(new SelectListItem
                        {
                            Text = item.Cluster,
                            Value = Convert.ToString(item.SlNo)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return clusterItems;
        }
        public List<SelectListItem> GetSubClusterList()
        {
            List<SubClusterMaster> objsublist = new List<SubClusterMaster>();
            List<SelectListItem> subclusterItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objsublist = context.SubClusterMasters.ToList();
                    foreach (var item in objsublist)
                    {
                        subclusterItems.Add(new SelectListItem
                        {
                            Text = item.SubCluster,
                            Value = Convert.ToString(item.SlNo)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return subclusterItems;
        }
        public List<SelectListItem> GetCityList()
        {
            List<CityMaster> objCitylist = new List<CityMaster>();
            List<SelectListItem> CityItems = new List<SelectListItem>();
            try
            {
                using (var context = new ChitaleDBContext())
                {
                    objCitylist = context.CityMasters.ToList();
                    foreach (var item in objCitylist)
                    {
                        CityItems.Add(new SelectListItem
                        {
                            Text = item.City,
                            Value = Convert.ToString(item.SlNo)
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                newexception.AddException(ex);
            }
            return CityItems;
        }
    }
}
