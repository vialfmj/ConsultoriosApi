using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using FluentValidation;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.Utils
{
    [TestClass]
    public class SimpleMediatorTests
    {
        public class FakeRequest : IRequest<string>
        {
            public required string Name { get; set; }
        }
        public class FakeHandler : IRequestHandler<FakeRequest, string>
        {
            public Task<string> Handle(FakeRequest request)
            {
                return Task.FromResult("Handled");
            }
        }
        public class FakeValidator : AbstractValidator<FakeRequest>
        {
            public FakeValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
            }
        }

        [TestMethod]
        public async Task Send_CallsHandler()
        {
            //Arrange
            var request = new FakeRequest() { Name = "Test" };
            var useCaseMock = Substitute.For<IRequestHandler<FakeRequest, string>>();
            var serviceProviderMock = Substitute.For<IServiceProvider>();

            serviceProviderMock.GetService(typeof(IRequestHandler<FakeRequest, string>)).Returns(useCaseMock);

            var mediator = new SimpleMediator(serviceProviderMock);

            var result = await mediator.Send(request);

            await useCaseMock.Received(1).Handle(request);
        }
        [TestMethod]
        public async Task Send_ThrowsException_WhenHandlerNotFound()
        {
            //Arrange
            var request = new FakeRequest() { Name = "Name A"};
            var useCaseMock = Substitute.For<IRequestHandler<FakeRequest, string>>();
            var serviceProviderMock = Substitute.For<IServiceProvider>();
            serviceProviderMock.GetService(typeof(IRequestHandler<FakeRequest, string>)).Returns(null);

            var mediator = new SimpleMediator(serviceProviderMock);

            await Assert.ThrowsExceptionAsync<MediatorException>(async () => await mediator.Send(request));

        }
        [TestMethod]
        public void Send_ThrowsValidationException_WhenCommandIsInvalid()
        {
            //Arrange
            var request = new FakeRequest() { Name = "Name A" };
            var serviceProvider = Substitute.For<IServiceProvider>();
            var validator = new FakeValidator();
            serviceProvider.GetService(typeof(IValidator<FakeRequest>)).Returns(validator);

            var mediator = new SimpleMediator(serviceProvider);

            Assert.ThrowsExceptionAsync<ConsultoriosApi.Application.Exceptions.ValidationException>(async () => await mediator.Send(request));
        }
    }
}
