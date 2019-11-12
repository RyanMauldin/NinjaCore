using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Xunit;

namespace NinjaCore.Extensions.Tests
{
    /// <summary>
    /// Tests for <seealso cref="IListExtensions" /> class extension methods.
    /// </summary>
    public class IListExtensionTests
    {
        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsBoolShouldPass()
        {
            var value = new List<bool>
            {
                true,
                false,
                false,
                true,
                false
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.ASCII).ToByteArray(Encoding.ASCII);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsCharShouldPass()
        {
            var value = "Hello World".ToCharArray().ToList();
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsDoubleShouldPass()
        {
            var value = new List<double>
            {
                0,
                double.MaxValue,
                double.MinValue,
                double.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsFloatShouldPass()
        {
            var value = new List<float>
            {
                0,
                float.MaxValue,
                float.MinValue,
                float.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsIntShouldPass()
        {
            var value = new List<int>
            {
                0,
                int.MaxValue,
                int.MinValue,
                int.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsLongShouldPass()
        {
            var value = new List<long>
            {
                0,
                long.MaxValue,
                long.MinValue,
                long.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsShortShouldPass()
        {
            var value = new List<short>
            {
                0,
                short.MaxValue,
                short.MinValue,
                short.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsUintShouldPass()
        {
            var value = new List<uint>
            {
                0,
                uint.MaxValue,
                uint.MinValue,
                uint.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsUlongShouldPass()
        {
            var value = new List<ulong>
            {
                0,
                ulong.MaxValue,
                ulong.MinValue,
                ulong.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsUshortShouldPass()
        {
            var value = new List<ushort>
            {
                0,
                ushort.MaxValue,
                ushort.MinValue,
                ushort.MaxValue,
                0
            };
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }
    }
}
