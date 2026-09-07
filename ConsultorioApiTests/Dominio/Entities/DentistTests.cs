using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Dominio.Entities
{
    [TestClass]
    public class DentistTests
    {
        [TestMethod]
        public void Constructor_throwsBusinessRuleException_WhenNameIsNullOrEmpty()
        {
            //Arrange
            string dentistName = null!;
            string dentistEmail = "email@mail.com";
            Email email = new Email(dentistEmail);
            var exception = Assert.ThrowsException<BusinessRuleException>(() => new Dentist(dentistName, email));
        }
        [TestMethod]
        public void Constructor_ThrowsBusinessRuleException_EmailIsNull()
        {
            //Arrange
            string dentistName = "Dentist Name";
            Email email = null!;
            
            var exception = Assert.ThrowsException<BusinessRuleException>(() => new Dentist(dentistName, email));
        }
        [TestMethod]
        public void Constructor_CreatesDentist_WhenParametersAreValid()
        {
            //Arrange
            string dentistName = "Dentist Name";
            string dentistEmail = "email@mail.com";
            Email email = new Email(dentistEmail);

            //Act
            Dentist dentist = new Dentist(dentistName, email);

            //Assert
            Assert.IsNotNull(dentist);
        }
        [TestMethod]
        public void UpdateName_ThrowsBusinessRuleException_WhenNameIsNullOrEmpty()
        {
            //Arrange
            Dentist dentist = new Dentist("Dentist Name", new Email("email@mail.com"));

            //Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => dentist.UpdateName(null!));
        }
        [TestMethod]
        public void UpdateName_UpdatesName_WhenNameIsValid()
        {
            //Arrange
            Dentist dentist = new Dentist("Dentist Name", new Email("email@mail.com"));

            //Act
            dentist.UpdateName("Nuevo Nombre");

            //Assert
            Assert.AreEqual("Nuevo Nombre", dentist.Name);
        }
        [TestMethod]
        public void UpdateEmail_ThrowsBusinessRuleException_WhenEmailIsNull()
        {
            //Arrange
            Dentist dentist = new Dentist("Dentist Name", new Email("email@mail.com"));

            //Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => dentist.UpdateEmail(null!));
        }
        [TestMethod]
        public void UpdateEmail_UpdatesEmail_WhenEmailIsValid()
        {
            //Arrange
            Dentist dentist = new Dentist("Dentist Name", new Email("email@mail.com"));
            Email nuevoEmail = new Email("nuevo@mail.com");

            //Act
            dentist.UpdateEmail(nuevoEmail);

            //Assert
            Assert.AreEqual(nuevoEmail, dentist.Email);
        }
    }
}
