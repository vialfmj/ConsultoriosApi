using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Contracts.Repositories.Models;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.SendAppointmentReminder;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Enums;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Appointments
{
    [TestClass]
    public class SendAppointmentReminderUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor.
        private IAppointmentsRepository repository;
        private INotificationService notificationService;
        private SendAppointmentReminderUseCase useCase;
#pragma warning restore CS8618

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IAppointmentsRepository>();
            notificationService = Substitute.For<INotificationService>();
            useCase = new SendAppointmentReminderUseCase(repository, notificationService);
        }

        [TestMethod]
        public async Task Handle_SendsOneReminderPerAppointment_WhenThereAreAppointmentsTomorrow()
        {
            // Arrange
            var appointment1 = CreateAppointment("Paciente A", "a@a.com", "Dentista A", "Consultorio A");
            var appointment2 = CreateAppointment("Paciente B", "b@b.com", "Dentista B", "Consultorio B");
            repository.GetFiltered(Arg.Any<AppointmentsFilterDTO>()).Returns(new List<Appointment> { appointment1, appointment2 });

            // Act
            await useCase.Handle(new SendAppointmentReminderCommand());

            // Assert
            await notificationService.Received(2).SendAppointmentReminderAsync(Arg.Any<AppointmentReminderDTO>());
            await notificationService.Received(1).SendAppointmentReminderAsync(
                Arg.Is<AppointmentReminderDTO>(d => d.Id == appointment1.Id && d.Patient == "Paciente A" && d.Patient_Email == "a@a.com"
                    && d.Dentist == "Dentista A" && d.Office == "Consultorio A"));
            await notificationService.Received(1).SendAppointmentReminderAsync(
                Arg.Is<AppointmentReminderDTO>(d => d.Id == appointment2.Id && d.Patient == "Paciente B" && d.Patient_Email == "b@b.com"
                    && d.Dentist == "Dentista B" && d.Office == "Consultorio B"));
        }

        [TestMethod]
        public async Task Handle_DoesNotSendAnything_WhenThereAreNoAppointments()
        {
            // Arrange
            repository.GetFiltered(Arg.Any<AppointmentsFilterDTO>()).Returns(new List<Appointment>());

            // Act
            await useCase.Handle(new SendAppointmentReminderCommand());

            // Assert
            await notificationService.DidNotReceive().SendAppointmentReminderAsync(Arg.Any<AppointmentReminderDTO>());
        }

        [TestMethod]
        public async Task Handle_FiltersScheduledAppointmentsOfTomorrowInArgentinaTime()
        {
            // Arrange
            AppointmentsFilterDTO? captured = null;
            repository.GetFiltered(Arg.Do<AppointmentsFilterDTO>(f => captured = f)).Returns(new List<Appointment>());
            var argentinaOffset = TimeSpan.FromHours(-3);

            // Act
            await useCase.Handle(new SendAppointmentReminderCommand());

            // Assert
            Assert.IsNotNull(captured);
            Assert.AreEqual(DateState.Scheduled, captured.State);
            Assert.AreEqual(argentinaOffset, captured.From!.Value.Offset);
            Assert.AreEqual(TimeSpan.Zero, captured.From.Value.TimeOfDay);
            Assert.AreEqual(TimeSpan.FromDays(1), captured.To!.Value - captured.From.Value);

            // "Mañana" en Argentina siempre queda entre 0 y 2 días respecto de ahora en UTC
            var hoursFromNow = (captured.From.Value - DateTimeOffset.UtcNow).TotalHours;
            Assert.IsTrue(hoursFromNow > 0 && hoursFromNow <= 27, $"La ventana arranca en un momento inesperado: {hoursFromNow}h desde ahora.");
        }

        [TestMethod]
        public async Task Handle_PropagatesException_WhenNotificationFails()
        {
            // Arrange
            var appointment = CreateAppointment("Paciente A", "a@a.com", "Dentista A", "Consultorio A");
            repository.GetFiltered(Arg.Any<AppointmentsFilterDTO>()).Returns(new List<Appointment> { appointment });
            notificationService.SendAppointmentReminderAsync(Arg.Any<AppointmentReminderDTO>()).Throws<InvalidOperationException>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await useCase.Handle(new SendAppointmentReminderCommand()));
        }

        [TestMethod]
        public void ToDto_MapsAppointmentData()
        {
            // Arrange
            var appointment = CreateAppointment("Paciente A", "a@a.com", "Dentista A", "Consultorio A");

            // Act
            var dto = appointment.ToDto();

            // Assert
            Assert.AreEqual(appointment.Id, dto.Id);
            Assert.AreEqual(appointment.TimeInterval.Start, dto.Date);
            Assert.AreEqual("Paciente A", dto.Patient);
            Assert.AreEqual("a@a.com", dto.Patient_Email);
            Assert.AreEqual("Dentista A", dto.Dentist);
            Assert.AreEqual("Consultorio A", dto.Office);
        }

        // GetFiltered de EF trae Patient/Dentist/Office via Include; aca los seteamos a mano porque tienen setter privado.
        private static Appointment CreateAppointment(string patientName, string patientEmail, string dentistName, string officeName)
        {
            var start = DateTime.UtcNow.AddDays(1);
            var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new TimeInterval(start, start.AddHours(1)));
            typeof(Appointment).GetProperty(nameof(Appointment.Patient))!.SetValue(appointment, new Patient(patientName, new Email(patientEmail)));
            typeof(Appointment).GetProperty(nameof(Appointment.Dentist))!.SetValue(appointment, new Dentist(dentistName, new Email("dentista@a.com")));
            typeof(Appointment).GetProperty(nameof(Appointment.Office))!.SetValue(appointment, new Office(officeName));
            return appointment;
        }
    }
}
