using Backend.Core.Features.UserManagement.Security;
using Xunit;

namespace Backend.Tests.Unit.Core.Security
{
    public class RandomPasswordGeneratorFixture
    {
        [Fact]
        public void Generate_CreatesRandomPassword()
        {
            // Arrange
            var randomPasswordGenerator = new RandomPasswordGenerator();

            // Act
            string password1 = randomPasswordGenerator.Generate();
            string password2 = randomPasswordGenerator.Generate();

            // Assert
            Assert.NotEqual(password1, password2);
        }
    }
}