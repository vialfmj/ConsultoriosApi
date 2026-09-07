using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientDetail;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Patients
{
    [TestClass]
    public class GetPatientDetailUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IPatientsRepository repository;
        private GetPatientDetailUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IPatientsRepository>();
            useCase = new GetPatientDetailUseCase(repository);
        }

        [TestMethod]
        public async Task Handle_ReturnsDTO_WhenIsValid()
        {
            var patient = new Patient("Paciente A", new Email("paciente@a.com"));
            var id = patient.Id;
            var query = new GetPatientDetailQuery { Id = id };
            repository.GetById(id).Returns(patient);

            var result = await useCase.Handle(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(patient.Id, result.Id);
            Assert.AreEqual(patient.Name, result.Name);
            Assert.AreEqual(patient.Email.Valor, result.Email);
        }

        [TestMethod]
        public void Handle_ThrowsNotFoundException_WhenPatientNotFound()
        {
            Guid id = Guid.NewGuid();
            var query = new GetPatientDetailQuery { Id = id };

            repository.GetById(id).ReturnsNull();

            Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(query));
        }
    }
}
