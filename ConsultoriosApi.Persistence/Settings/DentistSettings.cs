using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DentistSettings : IEntityTypeConfiguration<Dentist>
{
    public void Configure(EntityTypeBuilder<Dentist> builder)
    {
        builder.Property(prop => prop.Name)
        .HasMaxLength(150)
        .IsRequired();

        builder.ComplexProperty(prop => prop.Email, action =>
        {

            action.Property(e => e.Valor).HasColumnName("Email").HasMaxLength(254);

        });
    }
}
