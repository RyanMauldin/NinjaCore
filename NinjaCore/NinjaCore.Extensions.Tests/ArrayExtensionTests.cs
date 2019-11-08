using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace NinjaCore.Extensions.Tests
{
    /// <summary>
    /// Tests for <seealso cref="ArrayExtensions" /> class extension methods.
    /// </summary>
    public class ArrayExtensionTests
    {
        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForArrayClear()
        {
            const string value = "0123456789";
            var bytes = Encoding.UTF8.GetBytes(value);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();

            const string expectedValue = "0000000000";
            var expectedBytes = Encoding.UTF8.GetBytes(expectedValue);
            expectedBytes.Should().BeOfType<byte[]>();
            expectedBytes.Should().NotBeNullOrEmpty();

            bytes.TryClear();
            bytes.Should().BeOfType<string>();
            bytes.Should().NotBeNullOrEmpty();

            bytes.Should().Should().BeEquivalentTo(expectedBytes);
        }
    }
}
