namespace pro_web_a.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProjectDB : DbContext
    {
        public ProjectDB()
            : base("name=projectDBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<device> Devices { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<StopAt> StopAts { get; set; }
        public virtual DbSet<Train> Trains { get; set; }
        public virtual DbSet<user> Users { get; set; }
        public DbSet<LocationLog> Location { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<PinLocation> PinLocation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<device>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Route>()
                .HasMany(e => e.stations)
                .WithRequired(e => e.Route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.trains)
                .WithRequired(e => e.route)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Station>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Station>()
                .Property(e => e.Address)
                .IsFixedLength();

            modelBuilder.Entity<Station>()
                .Property(e => e.Telephone)
                .IsFixedLength();

            modelBuilder.Entity<Station>()
                .HasMany(e => e.Stops)
                .WithRequired(e => e.station)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Train>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Train>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Train>()
                .HasMany(e => e.devices)
                .WithRequired(e => e.train)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Train>()
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
