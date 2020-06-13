using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        public DbSet<Product> Products{ get; set; }


    }
}