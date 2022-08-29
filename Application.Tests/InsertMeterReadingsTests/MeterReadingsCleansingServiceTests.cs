namespace Ensek.Energy.Command.API.Application.Tests.InsertMeterReadingsTests
{
    using Ensek.Energy.Command.API.Application.InsertMeterReadings;
    using Ensek.Energy.Command.API.Model;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory($"Services - {nameof(MeterReadingsCleansingService)}")]
    public class MeterReadingsCleansingServiceTests
    {
        private Mock<ValidationResult> _mockValidationResult;
        private Mock<IValidator<MeterReading>> _mockValidator;
        private MeterReadingsCleansingService _classUnderTest;
        private bool _validationResultIsValid;

        [TestInitialize]
        public void Init()
        {
            _validationResultIsValid = true;

            _mockValidationResult = new Mock<ValidationResult>();
            _mockValidationResult.Setup(x => x.IsValid).Returns(() => _validationResultIsValid);

            _mockValidator = new Mock<IValidator<MeterReading>>();
            _mockValidator.Setup(x => x.ValidateAsync(It.IsAny<MeterReading>(), default))
                .ReturnsAsync(() => _mockValidationResult.Object);

            _classUnderTest = new MeterReadingsCleansingService(_mockValidator.Object);
        }

        [TestMethod]
        public async Task Cleanse_ValidRequest_ReturnsLatestReading()
        {
            var latestReading = new MeterReading
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2022, 1, 1),
                MeterReadValue = "99999"
            };

            var oldReading = new MeterReading
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2012, 1, 1),
                MeterReadValue = "12345"
            };

            var request = new InsertMeterReadings.Request
            {
                MeterReadings = new List<MeterReading>
                {
                    latestReading,
                    oldReading
                }
            };

            var result = await _classUnderTest.Cleanse(request);

            using (var scope = new AssertionScope())
            {
                result.Should().NotBeEmpty();
                result.Should().HaveCount(1);
                result.First().AccountId.Should().Be(latestReading.AccountId);
                result.First().MeterReadingDateTime.Should().Be(latestReading.MeterReadingDateTime);
                result.First().MeterReadValue.Should().Be(latestReading.MeterReadValue);
            }
        }

        [TestMethod]
        public async Task Cleanse_ValidRequests_ReturnsReadings()
        {
            var reading1 = new MeterReading
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2022, 1, 1),
                MeterReadValue = "99999"
            };

            var reading2 = new MeterReading
            {
                AccountId = 7777,
                MeterReadingDateTime = new DateTime(2012, 1, 1),
                MeterReadValue = "12345"
            };

            var request = new InsertMeterReadings.Request
            {
                MeterReadings = new List<MeterReading>
                {
                    reading1,
                    reading2
                }
            };

            var result = await _classUnderTest.Cleanse(request);

            using (var scope = new AssertionScope())
            {
                result.Should().NotBeEmpty();
                result.Should().HaveCount(2);
                result.First().AccountId.Should().Be(reading1.AccountId);
                result.First().MeterReadingDateTime.Should().Be(reading1.MeterReadingDateTime);
                result.First().MeterReadValue.Should().Be(reading1.MeterReadValue);
                result.ElementAt(1).AccountId.Should().Be(reading2.AccountId);
                result.ElementAt(1).MeterReadingDateTime.Should().Be(reading2.MeterReadingDateTime);
                result.ElementAt(1).MeterReadValue.Should().Be(reading2.MeterReadValue);
            }
        }

        [TestMethod]
        public async Task Cleanse_SomeRequests_ReturnsSomeReadings()
        {
            var reading1 = new MeterReading
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2022, 1, 1),
                MeterReadValue = "99999"
            };

            var reading2 = new MeterReading
            {
                AccountId = 7777,
                MeterReadingDateTime = new DateTime(2022, 7, 1),
                MeterReadValue = "12345"
            };

            var mockFalseValidationResult = new Mock<ValidationResult>();
            mockFalseValidationResult.Setup(x => x.IsValid).Returns(false);

            var mockTrueValidationResult = new Mock<ValidationResult>();
            mockTrueValidationResult.Setup(x => x.IsValid).Returns(true);


            _mockValidator.Reset();
            _mockValidator.Setup(x => x.ValidateAsync(reading1, default))
                .ReturnsAsync(() => mockFalseValidationResult.Object);

            _mockValidator.Setup(x => x.ValidateAsync(reading2, default))
                .ReturnsAsync(() => mockTrueValidationResult.Object);

            var request = new InsertMeterReadings.Request
            {
                MeterReadings = new List<MeterReading>
                {
                    reading1,
                    reading2
                }
            };

            var result = await _classUnderTest.Cleanse(request);

            using (var scope = new AssertionScope())
            {
                result.Should().NotBeEmpty();
                result.Should().HaveCount(1);
                result.First().AccountId.Should().Be(reading2.AccountId);
                result.First().MeterReadingDateTime.Should().Be(reading2.MeterReadingDateTime);
                result.First().MeterReadValue.Should().Be(reading2.MeterReadValue);
            }
        }

        [TestMethod]
        public async Task Cleanse_InvalidRequests_ReturnsEmptyList()
        {
            _validationResultIsValid = false;
            var reading1 = new MeterReading
            {
                AccountId = 1234,
                MeterReadingDateTime = new DateTime(2022, 1, 1),
                MeterReadValue = "99999"
            };

            var reading2 = new MeterReading
            {
                AccountId = 7777,
                MeterReadingDateTime = new DateTime(2022, 7, 1),
                MeterReadValue = "12345"
            };

            var request = new InsertMeterReadings.Request
            {
                MeterReadings = new List<MeterReading>
                {
                    reading1,
                    reading2
                }
            };

            var result = await _classUnderTest.Cleanse(request);

            result.Should().BeEmpty();
        }
    }
}
