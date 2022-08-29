namespace Ensek.Energy.Command.API.Application.Tests.InsertMeterReadingsTests
{
    using Ensek.Energy.Command.API.Application.InsertMeterReadings;
    using Ensek.Energy.Command.Model;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    [TestCategory("InsertMeterReadings - Validator")]
    public class ValidatorTests
    {
        private MeterReadingValidator _classUnderTest;

        [TestInitialize]
        public void Init()
        {
            _classUnderTest = new MeterReadingValidator();
        }

        [TestMethod]
        public void Validate_NullMeterReading_ThrowsException()
        {
            MeterReading reading = default;

            FluentActions.Invoking(() => _classUnderTest.Validate(reading))
                .Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Validate_InvalidMeterReading_ReturnsFalse()
        {
            var reading = new MeterReading();

            var result = _classUnderTest.Validate(reading);

            using (var scope = new AssertionScope())
            {
                result.IsValid.Should().BeFalse();
                result.Errors.Should().HaveCount(3);
                result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(reading.AccountId));
                result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(reading.MeterReadingDateTime));
                result.Errors.Select(x => x.PropertyName).Should().Contain(nameof(reading.MeterReadValue));
            }
        }

        [TestMethod]
        [DataRow("1")]
        [DataRow("999999")]
        [DataRow("VOID")]
        [DataRow("0X765")]
        [DataRow("-6575")]
        public void Validate_InvalidMeterReadValue_ReturnsFalse(string meterReadValue)
        {
            var reading = new MeterReading()
            {
                AccountId = 1234,
                MeterReadingDateTime = DateTime.Now,
                MeterReadValue = meterReadValue
            };

            var result = _classUnderTest.Validate(reading);

            using (var scope = new AssertionScope())
            {
                result.IsValid.Should().BeFalse();
                result.Errors.Should().HaveCount(1);
                result.Errors.First().PropertyName.Should().Be(nameof(reading.MeterReadValue));
                result.Errors.First().ErrorMessage.Should().Be("'Meter Read Value' is not in the correct format.");
            }
        }

        [TestMethod]
        public void Validate_ValidMeterReading_ReturnsTrue()
        {
            var reading = new MeterReading()
            {
                AccountId = 123,
                MeterReadingDateTime = DateTime.Now,
                MeterReadValue = "12345"
            };

            var result = _classUnderTest.Validate(reading);

            result.IsValid.Should().BeTrue();
        }
    }
}
