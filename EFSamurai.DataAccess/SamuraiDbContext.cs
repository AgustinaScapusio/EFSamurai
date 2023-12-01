using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFSamurai.Domain;
using EFSamurai.Domain.Entities;


namespace EFSamurai.DataAccess
{
    public class SamuraiDbContext : DbContext
    {
        public DbSet<Samurai> Samurai => Set<Samurai>();
        public DbSet<Quote> Quote => Set<Quote>();
        public DbSet<SecretIdentity> SecretIdentity => Set<SecretIdentity>();
        public DbSet<SamuraiBattle> SamuraiBattle {  get; set; }
        public DbSet<BattleLog> BattleLogs => Set<BattleLog>();
        public DbSet<BattleEvent> BattleEvents => Set<BattleEvent>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            @"Server = (localdb)\MSSQLLocalDB; " +
            "Database = EFSamurai; " +
            "Trusted_Connection = True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(c => new { c.SamuraiId, c.BattleId });
        }

    }

}
