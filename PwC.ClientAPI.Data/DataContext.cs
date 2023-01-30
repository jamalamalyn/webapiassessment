using Microsoft.EntityFrameworkCore;
using PwC.ClientAPI.Domain.Models;
using System.Linq;
using System;
using PwC.ClientAPI.Domain.Interfaces;

namespace PwC.ClientAPI.Domain
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
        public DbSet<Client> Clients { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Client && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Client)entityEntry.Entity).UpdateDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Client)entityEntry.Entity).CreateDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}

