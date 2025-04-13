using Xunit;
using Moq;
using BackendAPI.WebApi.Services;
using BackendAPI.WebApi.Repositories;
using BackendAPI.WebApi.Dtos;
using Microsoft.Extensions.Configuration;

namespace BackendTests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ShouldReturnTrue_IfUserDoesNotExist() {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByUsernameAsync("newuser"))
                    .ReturnsAsync((BackendAPI.WebApi.Models.User?)null);

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string> {
                ["Jwt:Key"] = "SomeSecretKey"
            }!).Build();

            var authService = new AuthService(config, mockRepo.Object);

            // Act
            var result = await authService.RegisterAsync(new UserDto { Username = "newuser", Password = "Password123@" });

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFalse_IfUserExists() {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetByUsernameAsync("existing"))
                    .ReturnsAsync(new BackendAPI.WebApi.Models.User { Username = "existing" });

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string> {
                ["Jwt:Key"] = "SomeSecretKey"
            }!).Build();

            var authService = new AuthService(config, mockRepo.Object);
            
            // Act
            var result = await authService.RegisterAsync(new UserDto { Username = "existing", Password = "Password123@" });

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_IfCredentialsValid() {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();

            // user with hashed password
            var hashed = BCrypt.Net.BCrypt.HashPassword("Password123");
            mockRepo.Setup(r => r.GetByUsernameAsync("john"))
                    .ReturnsAsync(new BackendAPI.WebApi.Models.User { Username = "john", PasswordHash = hashed });

            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string> {
                ["Jwt:Key"] = "uRGLDlGsY6qwxq(5(B2():AgA^8{V%1bN(^s'LW;Wp>b5#n9w6wU,KC0=^IJDF{"
            }!).Build();

            var authService = new AuthService(config, mockRepo.Object);

            // Act
            var token = await authService.LoginAsync(new UserDto { Username = "john", Password = "Password123" });

            // Assert
            Assert.NotNull(token);
            Assert.True(token.Length > 10);
        }
    }
}