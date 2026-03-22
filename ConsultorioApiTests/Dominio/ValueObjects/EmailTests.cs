using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio;
using System.Security.Cryptography.X509Certificates;
using ConsultoriosApi.Dominio.ValueObjects;

namespace ConsultorioApiTests.Dominio.ValueObjects
{
    [TestClass]
    public class EmailTests
    {
        [TestMethod]
        public void Constructor_NullOrEmptyEmail_ThrowsBusinessRuleException()
        {
            // Arrange
            string? email = null;
            // Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => new ConsultoriosApi.Dominio.ValueObjects.Email(email!));
        }
        [TestMethod]
        public void Constructor_InvalidEmail_ThrowsBusinessRuleException()
        {
            //Arrange
            string email = "franco.com";
            //Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => new ConsultoriosApi.Dominio.ValueObjects.Email(email));
        }
        [TestMethod]
        public void Constructor_ValidEmail_DoesntThrowsBusinessRuleException() 
        {
            string email = "franco@email.com";
            new Email(email);
        }
    }
}
