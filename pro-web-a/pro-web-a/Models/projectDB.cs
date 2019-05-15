namespace pro_web_a.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class projectDB : DbContext
    {
        public projectDB()
            : base("name=projectDBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<device> devices { get; set; }
        public virtual DbSet<location> locations { get; set; }
        public virtual DbSet<log> logs { get; set; }
        public virtual DbSet<route> routes { get; set; }
        public virtual DbSet<station> stations { get; set; }
        public virtual DbSet<stopat> stopats { get; set; }
        public virtual DbSet<train> trains { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<device>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<route>()
                .HasMany(e => e.stations)
                .WithRequired(e => e.route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<route>()
                .HasMany(e => e.trains)
                .WithRequired(e => e.route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<station>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<station>()
                .Property(e => e.Address)
                .IsFixedLength();

            modelBuilder.Entity<station>()
                .Property(e => e.Telephone)
                .IsFixedLength();

            modelBuilder.Entity<station>()
                .HasMany(e => e.stopats)
                .WithRequired(e => e.station)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<train>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<train>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<train>()
                .HasMany(e => e.devices)
                .WithRequired(e => e.train)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<train>()
                .HasMany(e => e.logs)
                .WithRequired(e => e.train)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<train>()
                .HasMany(e => e.stopats)
                .WithRequired(e => e.train)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<user>()
                .Property(e => e.Uname)
                .IsFixedLength();

            modelBuilder.Entity<user>()
                .Property(e => e.Password)
                .IsFixedLength();
        }
    }
}
