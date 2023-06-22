using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FileWatcherApp.Model;
using FileWatcherApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace FileWatcherApp.EfContext
{
    public class MyDbContext : DbContext
    {
        public DbSet<Trade> TradeDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite($"Data Source={ConstantFilePaths.databasePath}");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>().ToTable("TradeDatas");
            modelBuilder.Entity<Trade>()
        .HasKey(t => t.TradeID);
            // Other code
        }
    }
}
