using FluentAssertions;
using Xunit;

namespace NinjaCore.Extensions.Tests
{
    /// <summary>
    /// Tests for <seealso cref="Base64Extensions" /> class extension methods.
    /// </summary>
    public class Base64ExtensionTests
    {
        public const string TextTestValue = @"A test value which includes special characters ~!@#$%^&*()(_+_++--==";

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForBase64ConversionShouldPass()
        {
            var encodedValue = TextTestValue.ToBase64String();
            encodedValue.Should().BeOfType<string>();
            encodedValue.Should().NotBeNullOrWhiteSpace();
            var decodedValue = encodedValue.FromBase64String();
            decodedValue.Should().BeOfType<string>();
            decodedValue.Should().NotBeNullOrWhiteSpace();
            decodedValue.Should().BeEquivalentTo(TextTestValue);
        }
    }
}
