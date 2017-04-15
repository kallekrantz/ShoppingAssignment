using System.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace QliroShopper.Tests.Utils {
    public class TestSqliteSetup : ManagedSqliteConnection
    {
        public TestSqliteSetup(string connection_string) : base(connection_string)
        {
            Assert.Equal(ConnectionState.Open, this.State);
            Options = new DbContextOptionsBuilder<OrderContext>().UseSqlite(this).Options;
            using (var context = new OrderContext(Options))
            {   
                Assert.True(context.Database.EnsureCreated());
                context.SaveChanges();
            }
        }

        public DbContextOptions<OrderContext> Options { get; private set; }

    }
}