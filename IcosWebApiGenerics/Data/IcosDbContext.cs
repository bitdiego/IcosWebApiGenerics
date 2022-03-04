using IcosWebApi.Models.Obj;
using Microsoft.EntityFrameworkCore;

namespace IcosWebApi.Data
{
    public class IcosDbContext : DbContext
    {
        public IcosDbContext(DbContextOptions<IcosDbContext> options) : base(options)
        {

        }
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BADMList> BADMList { get; set; }

        public DbSet<GRP_LOCATION> GRP_LOCATION { get; set; }
        public DbSet<GRP_UTC_OFFSET> GRP_UTC_OFFSET { get; set; }
        public DbSet<GRP_LAND_OWNERSHIP> GRP_LAND_OWNERSHIP { get; set; }
        public DbSet<GRP_TOWER> GRP_TOWER { get; set; }
        public DbSet<GRP_CLIM_AVG> GRP_CLIM_AVG { get; set; }
        public DbSet<GRP_DM> GRP_DM { get; set; }
        /*public DbSet<GRP_INST> GRP_INST { get; set; }
        public DbSet<GRP_LOGGER> GRP_LOGGER { get; set; }
        public DbSet<GRP_FILE> GRP_FILE { get; set; }
        public DbSet<GRP_EC> GRP_EC { get; set; }
        public DbSet<GRP_ECSYS> GRP_ECSYS { get; set; }
        public DbSet<GRP_ECWEXCL> GRP_ECWEXCL { get; set; }
        public DbSet<GRP_BM> GRP_BM { get; set; }
        public DbSet<GRP_STO> GRP_STO { get; set; }*/
    }
}
