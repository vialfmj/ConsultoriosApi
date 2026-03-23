using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Enums;
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
    public class AppointmentTests
    {
        private Guid _patientId = Guid.NewGuid();
        private Guid _dentistId = Guid.NewGuid();
        private Guid _officeId = Guid.NewGuid();
        private TimeInterval _timeInterval = new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2));
        
        [TestMethod]
        public void Constructor_CreatesAppointment_WhenValidParameters()
        {
            // Arrange

            // Act
            var appointment = new Appointment(_patientId, _dentistId, _officeId, _timeInterval);
            // Assert
            Assert.AreEqual(_patientId, appointment.PatientId);
            Assert.AreEqual(_dentistId, appointment.DentistId);
            Assert.AreEqual(_officeId, appointment.OfficeId);
            Assert.AreEqual(_timeInterval, appointment.TimeInterval);
            Assert.AreEqual(DateState.Scheduled, appointment.State);
            Assert.AreNotEqual(Guid.Empty, appointment.Id);

        }
        [TestMethod]
        public void Constructor_ThrowsBusinessException_WhenStartDateIsEarlierThanCurrentDate()
        {
            // Arrange
            TimeInterval timeInterval = new TimeInterval(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(2));
            // Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => new Appointment(_patientId, _dentistId, _officeId, timeInterval));
        }
        [TestMethod]
        public void Cancel_ChangesStateToCanceled_WhenAppointmentIsScheduled()
        {
            // Arrange
            var appointment = new Appointment(_patientId, _dentistId, _officeId, _timeInterval);
            // Act
            appointment.Cancel();
            // Assert
            Assert.AreEqual(DateState.Canceled, appointment.State);
        }
         [TestMethod]
        public void Cancel_ThrowsBusinessException_WhenAppointmentIsNotScheduled()
        {
            // Arrange
            var appointment = new Appointment(_patientId, _dentistId, _officeId, _timeInterval);
            appointment.Cancel();
            // Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => appointment.Cancel());
        }
        [TestMethod]
        public void Complete_ChangesStateToCompleted_WhenAppointmentIsScheduled()
        {
            // Arrange
            var appointment = new Appointment(_patientId, _dentistId, _officeId, _timeInterval);
            // Act
            appointment.Complete();
            // Assert
            Assert.AreEqual(DateState.Canceled, appointment.State);
        }
         [TestMethod]
         public void Complete_ThrowsBusinessException_WhenAppointmentIsNotScheduled()
        {
            // Arrange
            var appointment = new Appointment(_patientId, _dentistId, _officeId, _timeInterval);
            appointment.Complete();
            // Act & Assert
            Assert.ThrowsException<BusinessRuleException>(() => appointment.Complete());
        }
    }
}
