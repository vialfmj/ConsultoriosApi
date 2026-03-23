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
    public class PatientTests
    {
        [TestMethod]
        public void Constructor_ThrowsBusinessRuleException_WhenNameIsNullOrEmpty()
        {
            //Arrange
            string patientName = null!;
            Email email = new Email("email@email.com");


            var exception = Assert.ThrowsException<BusinessRuleException>(() => new Patient(patientName, email));

        }
        [TestMethod]
        public void Constructor_ThrowsBusinessRuleException_WhenEmailIsNull()
        {
            //Arrange
            string patientName = "Patient Name";
            Email email = null!;
            var exception = Assert.ThrowsException<BusinessRuleException>(() => new Patient(patientName, email));
        }
        [TestMethod]
        public void Constructor_CreatesPatient_WhenParametersAreValid()
        {
            //Arrange
            string patientName = "Patient Name";
            string patientEmail = "email@email.com";
            Email email = new Email(patientEmail);

            //Act
            Patient patient = new Patient(patientName, email);

            //Assert
            Assert.IsNotNull(patient);
        }
    }
}
