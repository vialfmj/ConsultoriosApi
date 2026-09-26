using ConsultoriosApi.Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppointmentSettings : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        // El value object TimeInterval se mapea como ComplexProperty, igual que Email en Patient/Dentist.
        builder.ComplexProperty(prop => prop.TimeInterval, action =>
        {
            action.Property(t => t.Start).HasColumnName("Start").HasColumnType("datetime2");
            action.Property(t => t.End).HasColumnName("End").HasColumnType("datetime2");
        });

        // FKs explícitas con Restrict: un turno no se borra en cascada si se borra
        // el paciente/dentista/consultorio asociado. Además, si las tres fueran Cascade,
        // SQL Server rechazaría el modelo por "multiple cascade paths" (las tres FKs
        // podrían intentar borrar el mismo Appointment por rutas distintas).
        builder.HasOne(a => a.Patient)
            .WithMany()
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Dentist)
            .WithMany()
            .HasForeignKey(a => a.DentistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Office)
            .WithMany()
            .HasForeignKey(a => a.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
