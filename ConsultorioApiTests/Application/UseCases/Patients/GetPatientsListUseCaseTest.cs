using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Patients
{
    [TestClass]
    public class GetPatientsListUseCaseTest
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IPatientsRepository repository;
        private GetPatientsListUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IPatientsRepository>();
            useCase = new GetPatientsListUseCase(repository);
        }
        [TestMethod]
        public async Task IfThereArePatients_ReturnsPatientsListDto()
        {
            var patients = new List<Patient>
            {
                new Patient("Paciente A", new Email("paciente.a@example.com")),
                new Patient("Paciente B", new Email("paciente.b@example.com"))
            };

            repository.GetFiltered(Arg.Any<PatientsFilterDTO>()).Returns(patients);
            repository.GetTotalRecordCount().Returns(patients.Count);

            var result = await useCase.Handle(new GetPatientsListQuery());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Total, patients.Count);
            Assert.AreEqual(result.Elements.Count, patients.Count);

            for (int i = 0; i < patients.Count; i++)
            {
                Assert.AreEqual(result.Elements[i].Id, patients[i].Id);
                Assert.AreEqual(result.Elements[i].Name, patients[i].Name);
                Assert.AreEqual(result.Elements[i].Email, patients[i].Email.Valor);
            }
        }
    }
}
