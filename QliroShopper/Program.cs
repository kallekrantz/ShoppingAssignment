using System.IO;
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
        public OrderContext() {}
        public OrderContext(DbContextOptions<OrderContext> options): base(options){}
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }   
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            // Ensure that the tests will not use a real physical database.
            if (!optionsBuilder.IsConfigured)
            {
                var conString = new SqliteConnectionStringBuilder();
                conString.DataSource = "database.db";
                optionsBuilder.UseSqlite(conString.ToString());
            }
        }

    }
    
}
