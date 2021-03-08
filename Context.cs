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
            // Relationship: Model - Device
            builder.Entity<Model>()
                .HasMany(m => m.Devices)
                .WithOne(d => d.Model)
                .HasForeignKey(d => d.ModelId);

            // Relationship: Activity - Device 
            builder.Entity<Activity>()
                .HasMany(a => a.Devices)
                .WithOne(d => d.Activity)
                .HasForeignKey(d => d.ActivityId);

            // Relationship: Manufacturer - Model 
            builder.Entity<Manufacturer>()
                .HasMany(ma => ma.Models)
                .WithOne(mo => mo.Manufacturer)
                .HasForeignKey(mo => mo.ManufacturerId);

            // Relationship: Employee - Activity 
            builder.Entity<Employee>()
                .HasMany(e => e.Activities)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId);
        }

        /// <summary>
        /// database table Devices
        /// </summary>
        public DbSet<Device> Devices { get; set; }

        /// <summary>
        /// database table Employees
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// database table Activities
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// database table Laptops
        /// </summary>
        public DbSet<Laptop> Laptops { get; set; }

        /// <summary>
        /// database table Phones
        /// </summary>
        public DbSet<Phone> Phones { get; set; }

        /// <summary>
        /// database table Servers
        /// </summary>
        public DbSet<Server> Servers { get; set; }

        /// <summary>
        /// database table Workstations
        /// </summary>
        public DbSet<Workstation> Workstations { get; set; }

        /// <summary>
        /// database table Disposals
        /// </summary>
        public DbSet<Disposal> Disposals { get; set; }

        /// <summary>
        /// database table Maintenances
        /// </summary>
        public DbSet<Maintenance> Maintenances { get; set; }

        /// <summary>
        /// database table Operations
        /// </summary>
        public DbSet<Operation> Operations { get; set; }
        
        /// <summary>
        /// database table Stocks
        /// </summary>
        public DbSet<Stock> Stocks { get; set; }

        /// <summary>
        /// database table Stocks
        /// </summary>
        public DbSet<Manufacturer> Manufacturers { get; set; }

        /// <summary>
        /// database table Stocks
        /// </summary>
        public DbSet<Model> Models { get; set; }
    }

}