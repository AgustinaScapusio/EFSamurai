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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            @"Server = (localdb)\MSSQLLocalDB; " +
            "Database = EFSamurai; " +
            "Trusted_Connection = True;");
        }
    }

}
