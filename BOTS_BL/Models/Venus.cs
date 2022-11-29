using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BOTS_BL.Models
{
    public partial class Venus : DbContext
    {
        public Venus()
            : base("name=Venus")
        {
        }

        public virtual DbSet<CompetitionDetail> CompetitionDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
