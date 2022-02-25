using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportInventoryApp.Models;

namespace TransportInventoryApp.Data
{
    public class TransportInventoryContext : DbContext
    {
        public TransportInventoryContext(DbContextOptions<TransportInventoryContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
