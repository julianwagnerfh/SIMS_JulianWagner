using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SIMSAPI.Controllers;
using SIMSAPI.Models;
using StackExchange.Redis;
using LoginUserDto = SIMSAPI.Controllers.LoginUserDto;
using System.Net;

namespace SIMSAPI.Tests.Controllers
{
    public class RedisControllerTests
    {
        private readonly RedisController _controller;
        private readonly Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
        private readonly Mock<IDatabase> _mockDatabase;

        public RedisControllerTests()
        {
            _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            _mockDatabase = new Mock<IDatabase>();
            _mockConnectionMultiplexer.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);
            _controller = new RedisController(_mockConnectionMultiplexer.Object);
        }

        [Fact]
        public void Register_UserAlreadyExists_ReturnsConflict()
        {
            // Arrange
            var userDto = new RegisterUserDto { Username = "existingUser", Password = "password" };
            _mockDatabase.Setup(db => db.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(true);

            // Act
            var result = _controller.Register(userDto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginUserDto { Username = "testUser", Password = "wrongPassword" };
            _mockDatabase.Setup(db => db.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(true);
            _mockDatabase.Setup(db => db.HashGet(It.IsAny<RedisKey>(), "password", It.IsAny<CommandFlags>()))
                .Returns(BCrypt.Net.BCrypt.HashPassword("correctPassword"));

            // Act
            var result = _controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void DeleteUser_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            string username = "nonExistentUser";
            _mockDatabase.Setup(db => db.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).Returns(false);

            // Act
            var result = _controller.DeleteUser(username);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

    }
}
