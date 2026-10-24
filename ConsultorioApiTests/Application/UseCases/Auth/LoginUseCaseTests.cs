using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Auth.Commands.Login;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Auth
{
    [TestClass]
    public class LoginUseCaseTests
    {
#pragma warning disable CS8618
        private IAuthService authService;
        private LoginUseCase useCase;
#pragma warning restore CS8618

        [TestInitialize]
        public void Setup()
        {
            authService = Substitute.For<IAuthService>();
            useCase = new LoginUseCase(authService);
        }

        [TestMethod]
        public async Task Handle_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new LoginCommand { Email = "admin@a.com", Password = "Password123" };
            var expected = new AuthResultDTO { Token = "token", ExpiresAt = DateTime.UtcNow.AddHours(1) };
            authService.Login(command.Email, command.Password).Returns(expected);

            // Act
            var result = await useCase.Handle(command);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task Handle_ThrowsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var command = new LoginCommand { Email = "admin@a.com", Password = "mala" };
            authService.Login(command.Email, command.Password).Throws(new UnauthorizedException());

            // Act & Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedException>(async () => await useCase.Handle(command));
        }
    }
}
