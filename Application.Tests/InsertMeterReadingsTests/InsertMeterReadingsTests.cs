namespace Ensek.Energy.Command.API.Application.Tests.InsertMeterReadingsTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Ensek.Energy.Command.API.Application.InsertMeterReadings;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ensek.Energy.Command.API.Application.InsertMeterReadings.Interfaces;
    using Ensek.Energy.Command.Infrastructure.Interfaces;
    using Moq;
    using Ensek.Energy.Command.Model;
    using FluentAssertions.Execution;
    using FluentAssertions;

    [TestClass]
    [TestCategory($"Hadlers - {nameof(InsertMeterReadings)}")]
    public class InsertMeterReadingsTests
    {
        private Mock<IMeterReadingsCleansingService> _mockCleansingService;
        private Mock<IMeterReadingsRepository> _mockRepository;
        private InsertMeterReadings _classUnderTest;

        [TestInitialize]
        public void Init()
        {
            _mockCleansingService = new Mock<IMeterReadingsCleansingService>();
            _mockRepository = new Mock<IMeterReadingsRepository>();

            _classUnderTest = new InsertMeterReadings(_mockCleansingService.Object, _mockRepository.Object);
        }

        [TestMethod]
        public async Task Handle_GivenNoReadings_ReturnsZero()
        {
            var request = new InsertMeterReadings.Request()
            {
                MeterReadings = new List<MeterReading>()
            };

            var result = await _classUnderTest.Handle(request, default);

            using (var scope = new AssertionScope())
            {
                result.Sucessful.Should().Be(0);
                result.Failures.Should().Be(0);
            }

            _mockRepository.VerifyNoOtherCalls();
            _mockCleansingService.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Handle_GivenCleansingServiceReturnsNothing_DoesNotCallRepository()
        {
            var request = new InsertMeterReadings.Request()
            {
                MeterReadings = new List<MeterReading>()
                {
                    new MeterReading()
                }
            };

            var result = await _classUnderTest.Handle(request, default);
            using (var scope = new AssertionScope())
            {
                result.Sucessful.Should().Be(0);
                result.Failures.Should().Be(1);
            }

            _mockCleansingService.Verify(x => x.Cleanse(request), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Handle_GivenRepositoryReturnsNothing_ReturnsCounts()
        {
            var request = new InsertMeterReadings.Request()
            {
                MeterReadings = new List<MeterReading>()
                {
                    new MeterReading()
                }
            };


            var result = await _classUnderTest.Handle(request, default);
            using (var scope = new AssertionScope())
            {
                result.Sucessful.Should().Be(0);
                result.Failures.Should().Be(1);
            }

            _mockCleansingService.Verify(x => x.Cleanse(request), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Handle_GivenRepositoryReturnsSucessful_ReturnsCounts()
        {
            var request = new InsertMeterReadings.Request()
            {
                MeterReadings = new List<MeterReading>()
                {
                    new MeterReading()
                }
            };

            _mockCleansingService.Setup(x => x.Cleanse(request))
                .ReturnsAsync(() => request.MeterReadings);

            _mockRepository
                .Setup(x => x.InsertMeterReadings(It.IsAny<IEnumerable<MeterReading>>()))
                .ReturnsAsync(1);


            var result = await _classUnderTest.Handle(request, default);
            using (var scope = new AssertionScope())
            {
                result.Sucessful.Should().Be(1);
                result.Failures.Should().Be(0);
            }

            _mockCleansingService.Verify(x => x.Cleanse(request), Times.Once);
            _mockRepository.Verify(x => x.InsertMeterReadings(It.IsAny<IEnumerable<MeterReading>>()), Times.Once);
        }
    }
}
