using Microsoft.EntityFrameworkCore;
using ToyParser.Models;

namespace ToyParser.Date
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }
        public ApplicationDbContext() : base(GetOptions())
        {
        }
        private static DbContextOptions<ApplicationDbContext> GetOptions()
        {
            var connectionString = "Server=ANDREY;Database=ToyParserDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}
