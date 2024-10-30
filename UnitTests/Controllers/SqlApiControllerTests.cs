using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SIMSAPI.Controllers;
using System.Data;
using System.Data.SqlClient;

namespace SIMSAPI.Tests.Controllers
{
    public class SqlApiControllerTests
    {
        private readonly SIMSAPIController _controller;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public SqlApiControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _controller = new SIMSAPIController(_mockConfiguration.Object);
        }

        [Fact]
        public void GetIncidents_ReturnsAllIncidents()
        {
            // Arrange
            var query = "SELECT * FROM Incidents";
            var mockDataTable = new DataTable();
            mockDataTable.Columns.Add("Id", typeof(int));
            mockDataTable.Columns.Add("Beschreibung", typeof(string));
            mockDataTable.Rows.Add(1, "Test incident");

            // Mocken der Abfrage-Ergebnisse
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockReader = new Mock<IDataReader>();

            mockReader.SetupSequence(r => r.Read()).Returns(true).Returns(false); // nur einen Eintrag zurückgeben
            mockCommand.Setup(m => m.ExecuteReader()).Returns(mockReader.Object);
            mockConnection.Setup(m => m.CreateCommand()).Returns(mockCommand.Object);

            // Act
            var result = _controller.GetIncidents() as JsonResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}
