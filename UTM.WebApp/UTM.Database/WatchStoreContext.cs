using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using UTM.Domain;

namespace UTM.Database
{
    public class WatchStoreContext : DbContext
    {
        public WatchStoreContext() : base("name=WatchStoreContext")
        {
        }

        public DbSet<Watch> Watches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}