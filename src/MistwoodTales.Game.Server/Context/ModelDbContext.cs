using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MistwoodTales.Game.Server.Data.Models;

namespace MistwoodTales.Game.Server.Context
{
    class ModelDbContext : DbContext
    {
        public ModelDbContext()
        {

            _connectionString =
                "User ID=mistwoodtales;Password=mistwoodtales;Host=localhost;Port=5432;Database=mistwoodtales;Pooling=true;";
        }

        public ModelDbContext(string connectionString)
        {

            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        //public ModelDbContext(DbContextOptions<ModelDbContext> options) : base(options)
        //{
        //}



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseNpgsql(_connectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Account>().HasKey(m => m.Id);
            //modelBuilder.Entity<Character>().HasKey(m => m.Id);
            // shadow properties
            //modelBuilder.Entity<Account>().Property<DateTime>(nameof(Account.CreationDate));
            //modelBuilder.Entity<Character>().Property<DateTime>(nameof(Character.CreationDate));
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }



        public override EntityEntry Update(object entity)
        {
            ChangeTracker.DetectChanges();

            updateUpdatedProperty<Account>();
            updateUpdatedProperty<Character>();
            return base.Update(entity);
        }

        private void updateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in modifiedSourceInfo)
            {
                entry.Property("CreationDate").CurrentValue = DateTime.UtcNow;
            }
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Map> Maps { get; set; }


        /// <summary>
        /// Remove all data from DB
        /// </summary>
        public void Wipe()
        {
            using (var tran = Database.BeginTransaction())
            {
                Characters.RemoveRange(Characters);
                Accounts.RemoveRange(Accounts);
                Maps.RemoveRange(Maps);
                SaveChanges();
                tran.Commit();
            }
        }

        /// <summary>
        /// Insert inital data into empty DB
        /// </summary>
        public void SeedInitialDataIfEmpty()
        {
            if(Accounts.Any() 
                || Characters.Any()
                || Maps.Any())
                return;
            using (var tran = Database.BeginTransaction())
            {
                var map = new Map() {Height = 255, Width = 500};
                Maps.Add(map);
                var acc = new Account()
                {
                    Login = "login",
                    Password = "password",
                    CreationDate = DateTime.UtcNow
                };
                Accounts.Add(acc);
                var character = new Character()
                {
                    Gold = 100,
                    MaxHP = 200,
                    HP = 200,
                    Level = 1,
                    Map = map,
                    Account = acc,
                    CreationDate = DateTime.UtcNow,
                    Name = "Gatmeat",
                    XP = 0,
                };
                Characters.Add(character);
                SaveChanges();
                tran.Commit();
            }
        }
    }
}
