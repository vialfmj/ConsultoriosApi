using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.UseCases.Auth.Commands.RegisterUser;
using FluentValidation.TestHelper;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Auth
{
    [TestClass]
    public class RegisterUserUseCaseTests
    {
#pragma warning disable CS8618
        private IAuthService authService;
        private RegisterUserUseCase useCase;
#pragma warning restore CS8618

        [TestInitialize]
        public void Setup()
        {
            authService = Substitute.For<IAuthService>();
            useCase = new RegisterUserUseCase(authService);
        }

        [TestMethod]
        public async Task Handle_ReturnsUserId_WhenCommandIsValid()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new RegisterUserCommand { Email = "dentista@a.com", Password = "Password123", Role = AuthRoles.Dentist };
            authService.Register(command.Email, command.Password, command.Role).Returns(id);

            // Act
            var result = await useCase.Handle(command);

            // Assert
            Assert.AreEqual(id, result);
        }

        [TestMethod]
        public void Validator_Fails_WhenRoleIsNotAllowed()
        {
            var validator = new RegisterUserCommandValidator();
            var command = new RegisterUserCommand { Email = "a@a.com", Password = "Password123", Role = "Root" };

            validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Role);
        }

        [TestMethod]
        public void Validator_Fails_WhenPasswordIsTooShort()
        {
            var validator = new RegisterUserCommandValidator();
            var command = new RegisterUserCommand { Email = "a@a.com", Password = "corta", Role = AuthRoles.Admin };

            validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Password);
        }

        [TestMethod]
        public void Validator_Fails_WhenEmailIsInvalid()
        {
            var validator = new RegisterUserCommandValidator();
            var command = new RegisterUserCommand { Email = "no-es-email", Password = "Password123", Role = AuthRoles.Admin };

            validator.TestValidate(command).ShouldHaveValidationErrorFor(x => x.Email);
        }

        [TestMethod]
        public void Validator_Passes_WhenCommandIsValid()
        {
            var validator = new RegisterUserCommandValidator();
            var command = new RegisterUserCommand { Email = "a@a.com", Password = "Password123", Role = AuthRoles.Admin };

            validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
        }
    }
}
