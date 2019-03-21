using System;
using ECToken.Tests.Utils;
using VerizonDigital.CDN.TokenProvider.Security;
using Xunit;

namespace VerizonDigital.CDN.TokenProvider.Tests
{

    public class TokenBuilderTests
    {
        [Fact]
        public void EncryptV3_WithDateTimeAndClientIP_ReturnsEcnryptedTokenWithBoth()
        {
            // Arrange
            var generator = new TokenBuilder();
            var expireTime = DateTime.Now.AddMilliseconds(300);
            var clientIp = Faker.Internet.DomainName();
            var key = Faker.Name.FullName();

            // Act
            var token = generator.EncryptV3(key, expireTime, clientIp);

            // Assert

            Assert.NotNull(token);
            var decryptdToken = generator.DecryptV3(key, token);

            var expected = $"ec_expire={expireTime.FromEpoch()}&ec_clientip={clientIp}";
            Assert.Equal(expected, decryptdToken);
        }

        [Fact]
        public void EncryptV3_WithDateTimeOnly_ReturnsEncryptedTokenWithOnlyDate()
        {
            // Arrange
            var generator = new TokenBuilder();
            var expireTime = DateTime.Now.AddMilliseconds(300);
            var key = Faker.Name.FullName();

            // Act
            var token = generator.EncryptV3(key, expireTime);

            // Assert
            Assert.NotNull(token);
            var decryptdToken = generator.DecryptV3(key, token);

            var expected = $"ec_expire={expireTime.FromEpoch()}";
            Assert.Equal(expected, decryptdToken);
        }

        [Fact]
        public void NextRandomString_WithLength_ReturnsStringWithSpecifiedSize()
        {
            // Arrange
            var length = 50;
            var generator = new TokenBuilder();

            // Act
            var random = generator.NextRandomString(length);

            // Assert
            Assert.Equal(length, random.Length);
        }

        [Fact]
        public void NextRandomString_WithNoLength_ReturnsStringBetweenMINAndMAX()
        {
            // Arrange
            var lengthMin = 4;
            var lengthMax = 8;
            var generator = new TokenBuilder();

            // Act
            var random = generator.NextRandomString();

            // Assert
            Assert.True(random.Length >= lengthMin && random.Length <= lengthMax);
        }

        [Fact]
        public void NextRandomString_ReturnsString()
        {
            // Arrange
            var generator = new TokenBuilder();

            // Act
            var random = generator.NextRandomString();

            // Assert
            Assert.IsType<string>(random);
        }
    }
}
