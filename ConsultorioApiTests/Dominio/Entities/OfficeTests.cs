using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Dominio.Entities
{
    [TestClass]
    public class OfficeTests
    {
        [TestMethod]
        public void Constructor_throwsBusinessRuleException_WhenNameIsNullOrEmpty()
        {

            //Arrange
            string name = null!;

            //Act & Assert
            var exception = Assert.ThrowsException<BusinessRuleException>(() => new Office(name));

        }
    }
}
