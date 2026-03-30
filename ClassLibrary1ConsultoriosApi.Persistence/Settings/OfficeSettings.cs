using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1ConsultoriosApi.Persistence.Settings
{
    public class OfficeSettings : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.Property(prop => prop.Name).HasMaxLength(150).IsRequired();
        }
    }
}
