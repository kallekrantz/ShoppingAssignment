using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QliroShopper.Models;

namespace QliroShopper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
    public class OrderContext: DbContext {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            var conString = new SqliteConnectionStringBuilder();
            conString.DataSource = "database.db";
            optionsBuilder.UseSqlite(conString.ToString());
        }

    }
    
}
