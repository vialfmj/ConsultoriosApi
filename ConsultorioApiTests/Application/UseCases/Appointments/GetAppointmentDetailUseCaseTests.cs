using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentDetail;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Appointments
{
    [TestClass]
    public class GetAppointmentDetailUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IAppointmentsRepository repository;
        private GetAppointmentDetailUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IAppointmentsRepository>();
            useCase = new GetAppointmentDetailUseCase(repository);
        }

        [TestMethod]
        public async Task Handle_ReturnsDTO_WhenTurnoExiste()
        {
            // Arrange
            var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(1)));
            var query = new GetAppointmentDetailQuery { Id = appointment.Id };
            repository.GetById(appointment.Id).Returns(appointment);

            // Act
            var result = await useCase.Handle(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(appointment.Id, result.Id);
            Assert.AreEqual(appointment.PatientId, result.PatientId);
            Assert.AreEqual(appointment.DentistId, result.DentistId);
            Assert.AreEqual(appointment.OfficeId, result.OfficeId);
            Assert.AreEqual(appointment.TimeInterval.Start, result.Start);
            Assert.AreEqual(appointment.TimeInterval.End, result.End);
            Assert.AreEqual(appointment.State, result.State);
        }

        [TestMethod]
        public void Handle_ThrowsNotFoundException_WhenTurnoNoExiste()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new GetAppointmentDetailQuery { Id = id };
            repository.GetById(id).ReturnsNull();

            // Act & Assert
            Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(query));
        }
    }
}
