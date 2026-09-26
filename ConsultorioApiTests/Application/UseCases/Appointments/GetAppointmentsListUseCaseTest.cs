using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Appointments
{
    [TestClass]
    public class GetAppointmentsListUseCaseTest
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IAppointmentsRepository repository;
        private GetAppointmentsListUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IAppointmentsRepository>();
            useCase = new GetAppointmentsListUseCase(repository);
        }

        [TestMethod]
        public async Task IfThereAreAppointments_ReturnsAppointmentsListDto()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(1))),
                new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    new TimeInterval(DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(2).AddHours(1)))
            };
            var query = new GetAppointmentsListQuery();

            repository.GetFiltered(Arg.Any<AppointmentsFilterDTO>()).Returns(appointments);
            repository.GetTotalRecordCount().Returns(appointments.Count);

            // Act
            var result = await useCase.Handle(query);

            // Assert
            await repository.Received(1).GetFiltered(query);
            Assert.IsNotNull(result);
            Assert.AreEqual(appointments.Count, result.Total);
            Assert.AreEqual(appointments.Count, result.Elements.Count);

            for (int i = 0; i < appointments.Count; i++)
            {
                Assert.AreEqual(appointments[i].Id, result.Elements[i].Id);
                Assert.AreEqual(appointments[i].PatientId, result.Elements[i].PatientId);
                Assert.AreEqual(appointments[i].DentistId, result.Elements[i].DentistId);
                Assert.AreEqual(appointments[i].OfficeId, result.Elements[i].OfficeId);
                Assert.AreEqual(appointments[i].TimeInterval.Start, result.Elements[i].Start);
                Assert.AreEqual(appointments[i].TimeInterval.End, result.Elements[i].End);
                Assert.AreEqual(appointments[i].State, result.Elements[i].State);
            }
        }
    }
}
