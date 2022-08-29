namespace Ensek.Energy.Command.API.Infrastructure.Tests
{
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using Ensek.Energy.Command.API.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    [TestClass]
    [TestCategory($"Repository - {nameof(MeterReadingsRepository)}")]
    public class MeterReadingsRepositoryTests
    {
        private Mock<IDbConnection> _mockDbConnection;
        private Mock<IDbConnectionFactory> _mockDbConnectionFactory;
        private Mock<IDapperWrapper> _mockDapperWrapper;
        private MeterReadingsRepository _classUnderTest;

        [TestInitialize]
        public void Init()
        {
            _mockDbConnection = new Mock<IDbConnection>();

            _mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
            _mockDbConnectionFactory.Setup(x => x.GetConnection())
                .Returns(() => _mockDbConnection.Object);

            _mockDapperWrapper = new Mock<IDapperWrapper>();

            _classUnderTest = new MeterReadingsRepository(_mockDbConnectionFactory.Object, _mockDapperWrapper.Object);
        }

        [TestMethod]
        public async Task InsertMeterReadings_ValidReadings_CallsSproc()
        {
            var readings = new List<MeterReading>()
            {
                new MeterReading {AccountId=1234,MeterReadingDateTime = DateTime.Now, MeterReadValue = "99999"}
            };

            var result = await _classUnderTest.InsertMeterReadings(readings);

            _mockDapperWrapper.Verify(x => x.QuerySingleAsync<int>(It.IsAny<IDbConnection>(),
                "usp_Ensek_Readings_InsertMeterReadings", It.IsAny<object>(), CommandType.StoredProcedure));
        }
    }
}
