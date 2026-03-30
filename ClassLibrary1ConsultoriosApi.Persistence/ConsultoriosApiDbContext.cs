using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1ConsultoriosApi.Persistence
{
    public class ConsultoriosApiDbContext : DbContext
    {
        public ConsultoriosApiDbContext(DbContextOptions<ConsultoriosApiDbContext> options) : base(options)
        {
            
        }
        protected ConsultoriosApiDbContext()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof (ConsultoriosApiDbContext).Assembly);
        }
        public DbSet<Office> Offices { get; set; }
    }
}
