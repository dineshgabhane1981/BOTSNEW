using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTS_BL.Models.ChitaleModel
{
    public class NoActionModelTile
    {
        public Int64 SS_All { get; set; }
        public Int64 SS_30 { get; set; }
        public Int64 SS_90 { get; set; }
        public Int64 SS_180 { get; set; }
        public Int64 SS_365 { get; set; }
        public Int64 Dist_All { get; set; }
        public Int64 Dist_30 { get; set; }
        public Int64 Dist_90 { get; set; }
        public Int64 Dist_180 { get; set; }
        public Int64 Dist_365 { get; set; }
    }
    public class NoActionParticipantData
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cluster { get; set; }
        public string SubCluster { get; set; }
        public string City { get; set; }
        public Int64 BalancePoints { get; set; }
        public DateTime LastInvoiceDate { get; set; }
        public Int64 DaysSinceLastInvoice { get; set; }
        public string LastInvoiceDateStr { get; set; }

    }
}
