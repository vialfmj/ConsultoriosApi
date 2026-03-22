using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ConsultoriosApi.Dominio.ValueObjects;

namespace ConsultorioApiTests.Dominio.ValueObjects
{
    [TestClass]
    public class TimeIntervalTests
    {
        [TestMethod]
        public void Constructor_EndTimeBeforeStartTime_ThrowsBusinessRuleException()
        {
            // Arrange
            var startTime = DateTime.UtcNow;
            var endTime = DateTime.UtcNow.AddDays(-1);
            // Act & Assert
            Assert.ThrowsException<ConsultoriosApi.Dominio.Exceptions.BusinessRuleException>(() => new ConsultoriosApi.Dominio.ValueObjects.TimeInterval(startTime, endTime));
        }
        [TestMethod]
        public void Constructor_ValidInterval_DoesntThrowBusinessRuleException()
        {
            new TimeInterval(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        }
    }
}
