


using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PatientSettings : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(prop => prop.Name)
        .HasMaxLength(250)
        .IsRequired();

        builder.ComplexProperty(prop => prop.Email, action =>
        {

            action.Property(e => e.Valor).HasColumnName("Email").HasMaxLength(254);

        });
    }
}