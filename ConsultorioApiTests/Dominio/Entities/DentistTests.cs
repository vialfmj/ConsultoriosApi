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
    }
}
