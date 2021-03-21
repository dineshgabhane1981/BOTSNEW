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
    public class TgtVsAchRepository
    {
        public List<TgtVsAchParticipantWise> GetTgtVsAchParticipantWise(string Cluster, string SubCluster, string City, string Month, string Year, string CustomerType)
        {
            List<TgtVsAchParticipantWise> objData = new List<TgtVsAchParticipantWise>();
            List<TgtvsAchMaster> objTgtData = new List<TgtvsAchMaster>();
            using (var context = new ChitaleDBContext())
            {
                if (Cluster == "All" && SubCluster == "All" && City == "All" && Month == "" && Year == "")
                {
                    objData = (from s in context.CustomerDetails
                               join sa in context.TgtvsAchMasters on s.CustomerId equals sa.CustomerId
                               where sa.ProductType == "Over All" && sa.Date.Value.Month == DateTime.Today.Month
                               select new TgtVsAchParticipantWise
                               {
                                   Type = sa.CustomerType,
                                   ID = sa.CustomerId,
                                   Name = s.CustomerName,
                                   Cluster = s.Cluster,
                                   SubCluster = s.SubCluster,
                                   City = s.City,
                                   VolTgt = sa.VolumeTgt,
                                   VolAch = sa.VolumeAch,
                                   VolAchPer = sa.VolumeAchPercentage,
                                   ValTgt = sa.ValueTgt,
                                   ValAch = sa.ValueAch,
                                   ValAchPer = sa.ValueAchPercentage,
                                   DateVal = sa.Date

                               }).ToList();

                }
                else
                {
                    objData = (from s in context.CustomerDetails
                               join sa in context.TgtvsAchMasters on s.CustomerId equals sa.CustomerId
                               where sa.ProductType == "Over All"
                               select new TgtVsAchParticipantWise
                               {
                                   Type = sa.CustomerType,
                                   ID = sa.CustomerId,
                                   Name = s.CustomerName,
                                   Cluster = s.Cluster,
                                   SubCluster = s.SubCluster,
                                   City = s.City,
                                   VolTgt = sa.VolumeTgt,
                                   VolAch = sa.VolumeAch,
                                   VolAchPer = sa.VolumeAchPercentage,
                                   ValTgt = sa.ValueTgt,
                                   ValAch = sa.ValueAch,
                                   ValAchPer = sa.ValueAchPercentage,
                                   DateVal = sa.Date

                               }).ToList();
                    if (Cluster != "All")
                    {
                        objData = objData.Where(x => x.DateVal.Value.Month == DateTime.Today.Month && x.Cluster == Cluster).ToList();
                    }
                    else if (SubCluster != "All")
                    {
                        objData = objData.Where(x => x.DateVal.Value.Month == DateTime.Today.Month && x.SubCluster == SubCluster).ToList();

                    }
                    else if (City != "All")
                    {
                        objData = objData.Where(x => x.DateVal.Value.Month == DateTime.Today.Month && x.City == City).ToList();
                    }
                    if (Month != "" && Year != "")
                    {
                        objData = objData.Where(x => x.DateVal.Value.Month == Convert.ToInt32(Month) && x.DateVal.Value.Year == Convert.ToInt32(Year)).ToList();
                    }
                    else
                    {
                        if (Month != "")
                        {
                            objData = objData.Where(x => x.DateVal.Value.Month == Convert.ToInt32(Month)).ToList();
                        }
                        if (Year != "")
                        {
                            objData = objData.Where(x => x.DateVal.Value.Year == Convert.ToInt32(Year)).ToList();
                        }
                    }
                }
                if (CustomerType != "0")
                {
                    objData = objData.Where(x => x.Type == CustomerType).ToList();
                }

            }

            return objData;
        }

    }
}
