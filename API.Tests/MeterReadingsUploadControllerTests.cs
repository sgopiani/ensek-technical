namespace Ensek.Energy.Command.API.Tests
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using Ensek.Energy.Command.API.Controllers;
    using Ensek.Energy.Command.API.Application.InsertMeterReadings;
    using Ensek.Energy.Command.API.Model;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory($"Controllers - {nameof(MeterReadingsUploadController)}")]
    public class MeterReadingsUploadControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private Mock<IReader> _mockCsvReader;
        private Mock<IFactory> _mockCsvFactory;

        private MeterReadingsUploadController _classUnderTest;

        [TestInitialize]
        public void Init()
        {
            _mockMediator = new Mock<IMediator>();


            _mockCsvReader = new Mock<IReader>();
            _mockCsvReader.SetupGet(x => x.Context).Returns(new CsvContext(new CsvConfiguration(CultureInfo.InvariantCulture)));

            _mockCsvFactory = new Mock<IFactory>();
            _mockCsvFactory
                .Setup(x => x.CreateReader(It.IsAny<TextReader>(), It.IsAny<CultureInfo>()))
                .Returns(() => _mockCsvReader.Object);

            _classUnderTest = new MeterReadingsUploadController(_mockMediator.Object, _mockCsvFactory.Object);
        }

        [TestMethod]
        public void UploadMeterReadingsCSV_GivenNullFile_ThrowsException()
        {
            FluentActions.Invoking(() => _classUnderTest.UploadMeterReadingsCSV(default))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [TestMethod]
        public void UploadMeterReadingsCSV_GivenEmptyFile_ThrowsException()
        {
            var formFile = Mock.Of<IFormFile>();
            FluentActions.Invoking(() => _classUnderTest.UploadMeterReadingsCSV(formFile))
                .Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task UploadMeterReadingsCSV_GivenInvalidFile_ThrowsException()
        {
            var formFile = new Mock<IFormFile>();

            formFile.SetupGet(x => x.Length).Returns(1);
            formFile.SetupGet(x => x.FileName).Returns("failureTest.jpeg");

            var result = await _classUnderTest.UploadMeterReadingsCSV(formFile.Object);

            result.Should().BeOfType<UnsupportedMediaTypeResult>();
        }

        [TestMethod]
        public async Task UploadMeterReadingsCSV_GivenValidFile_CallsMediatR()
        {
            Mock<IFormFile> formFile = CreateMockFormFile();

            await _classUnderTest.UploadMeterReadingsCSV(formFile.Object);

            _mockCsvReader.Verify(x => x.GetRecords<MeterReading>(), Times.Once);
            _mockMediator.Verify(x => x.Send(It.IsAny<InsertMeterReadings.Request>(), default), Times.Once);

        }

        private static Mock<IFormFile> CreateMockFormFile()
        {
            var formFile = new Mock<IFormFile>();

            formFile.SetupGet(x => x.Length).Returns(1);
            formFile.SetupGet(x => x.FileName).Returns("successTest.csv");
            formFile.Setup(x => x.OpenReadStream()).Returns(Stream.Null);
            return formFile;
        }
    }
}