using System;
using System.Threading.Tasks;
using AuthenticationService.API.Entity;
using AuthenticationService.API.Models;
using AuthenticationService.API.Repository;
using AuthenticationService.API.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AuthenticationService.UnitTest
{
    public class AuthenticationServiceTests
    {
        [Test]
        public async Task LoginAsync_WithNonExistUser_ShouldReturnUserNotFoundError()
        {
            CreateTokenResponse tokenResponse = new()
            {
                IsSuccess = true,
                Token = "randomtoken"
            };

            LoginRequest request = new()
            {
                Email = "undefined",
                Password = "123456"
            };
            
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenService = new Mock<ITokenService>();
            API.Services.AuthenticationService authService = new  API.Services.AuthenticationService(mockTokenService.Object, mockUserRepository.Object);

            mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(default(User));

            //Act
            var result = await authService.LoginAsync(request);
            
            //Assert
            mockUserRepository.Verify(x => x.GetUserByEmail("undefined"), Times.Once);
            result.IsSuccess.Should().BeFalse();
            result.Token.Should().BeNull();
            result.ResultMessage.Should().Be("User Not Found.");
        }

        [Test]
        public async Task LoginAsync_WithWrongPassword_ShouldReturnPasswordNotMatchingError()
        {
            User user = new("Eren", "Yılmaz", "eryilmaz0@hotmail.com", "123456");
            
            LoginRequest loginRequest = new()
            {
                Email = "eryilmaz0@hotmail.com",
                Password = "wrongpassword"
            };
            
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(user);
            API.Services.AuthenticationService authService = new  API.Services.AuthenticationService(mockTokenService.Object, mockUserRepository.Object);
            
            //Act
            var result = await authService.LoginAsync(loginRequest);
            
            //Assert
            mockUserRepository.Verify(x => x.GetUserByEmail("eryilmaz0@hotmail.com"), Times.Once);
            result.IsSuccess.Should().BeFalse();
            result.ResultMessage.Should().Be("Password not Matching.");
            result.Token.Should().BeNull();
        }

        [Test]
        public async Task LoginAsync_WithTokenError_ShouldReturnTokenNotCreatedError()
        {
            User user = new("Eren", "Yılmaz", "eryilmaz0@hotmail.com", "123456");
            
            LoginRequest loginRequest = new()
            {
                Email = "eryilmaz0@hotmail.com",
                Password = "123456"
            };

            CreateTokenRequest createTokenRequest = new()
            {
                User = user
            };

            CreateTokenResponse createTokenResponse = new()
            {
                IsSuccess = false,
                Token = null
            };
            
            
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(user);
            mockTokenService.Setup(x => x.CreateTokenAsync(It.IsAny<CreateTokenRequest>())).ReturnsAsync(createTokenResponse);
            API.Services.AuthenticationService authService = new  API.Services.AuthenticationService(mockTokenService.Object, mockUserRepository.Object);
            
            //Act
            var result = await authService.LoginAsync(loginRequest);
            
            //Assert
            mockUserRepository.Verify(x => x.GetUserByEmail("eryilmaz0@hotmail.com"), Times.Once);
            mockTokenService.Verify(x => x.CreateTokenAsync(It.IsAny<CreateTokenRequest>()), Times.Once);
            result.IsSuccess.Should().BeFalse();
            result.ResultMessage.Should().Be("Token Not Created.");
            result.Token.Should().BeNull();
        }
        
        [Test]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnLoginSucceed()
        {
            User user = new("Eren", "Yılmaz", "eryilmaz0@hotmail.com", "123456");
            
            LoginRequest loginRequest = new()
            {
                Email = "eryilmaz0@hotmail.com",
                Password = "123456"
            };

            CreateTokenRequest createTokenRequest = new()
            {
                User = user
            };

            CreateTokenResponse createTokenResponse = new()
            {
                IsSuccess = true,
                Token = Guid.NewGuid().ToString()
            };
            
            
            var mockUserRepository = new Mock<IUserRepository>();
            var mockTokenService = new Mock<ITokenService>();
            mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>())).Returns(user);
            mockTokenService.Setup(x => x.CreateTokenAsync(It.IsAny<CreateTokenRequest>())).ReturnsAsync(createTokenResponse);
            API.Services.AuthenticationService authService = new  API.Services.AuthenticationService(mockTokenService.Object, mockUserRepository.Object);
            
            //Act
            var result = await authService.LoginAsync(loginRequest);
            
            //Assert
            mockUserRepository.Verify(x => x.GetUserByEmail("eryilmaz0@hotmail.com"), Times.Once);
            mockTokenService.Verify(x => x.CreateTokenAsync(It.IsAny<CreateTokenRequest>()), Times.Once);
            result.IsSuccess.Should().BeTrue();
            result.ResultMessage.Should().Be("Login Successfull.");
            result.Token.Should().NotBeNull();
        }
    }
}