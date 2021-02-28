using LifecycleManagementAPI.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace LifecycleManagementAPI
{

    /// <summary>
    /// Database context that allows us to specify the tables of a database
    /// </summary>
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model>()
                .HasMany(c => c.Devices)
                .WithOne(a => a.Model)
                .HasForeignKey(a => a.ModelId);
        }

        /// <summary>
        /// database table Categories
        /// </summary>
        public DbSet<Model> Categories { get; set; }

        /// <summary>
        /// database table Devices
        /// </summary>
        public DbSet<Device> Devices { get; set; }
    }

}