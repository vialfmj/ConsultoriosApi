using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Dentists
{
    [TestClass]
    public class GetDentistsListUseCaseTest
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IDentistsRepository repository;
        private GetDentistsListUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IDentistsRepository>();
            useCase = new GetDentistsListUseCase(repository);
        }
        [TestMethod]
        public async Task IfThereAreDentists_ReturnsDentistsListDto()
        {
            var dentists = new List<Dentist>
            {
                new Dentist("Dentista A", new Email("dentista.a@example.com")),
                new Dentist("Dentista B", new Email("dentista.b@example.com"))
            };

            repository.GetFiltered(Arg.Any<DentistsFilterDTO>()).Returns(dentists);
            repository.GetTotalRecordCount().Returns(dentists.Count);

            var result = await useCase.Handle(new GetDentistsListQuery());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Total, dentists.Count);
            Assert.AreEqual(result.Elements.Count, dentists.Count);

            for (int i = 0; i < dentists.Count; i++)
            {
                Assert.AreEqual(result.Elements[i].Id, dentists[i].Id);
                Assert.AreEqual(result.Elements[i].Name, dentists[i].Name);
                Assert.AreEqual(result.Elements[i].Email, dentists[i].Email.Valor);
            }
        }
    }
}
