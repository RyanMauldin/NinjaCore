using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NinjaCore.Extensions.Models;
using NinjaCore.Extensions.Types;
using Xunit;

namespace NinjaCore.Extensions.Tests
{
    /// <summary>
    /// Tests for <seealso cref="ListExtensions" /> class extension methods.
    /// </summary>
    public class ListExtensionTests
    {
        public static List<BoundsMode> BoundsModes = new List<BoundsMode>
        { BoundsMode.Ninja, BoundsMode.List, BoundsMode.Array, BoundsMode.PassThrough };
        
        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryValidateAllBoundModesWithDefaultParametersShouldPass()
        {
            var ninjaCoreSettings = new NinjaCoreSettings();
            // Iterate through all BoundsMode values.
            foreach (var boundsMode in BoundsModes)
            {
                // Default Ninja Bounds Settings.
                NinjaCoreSettings.DefaultBoundsMode = boundsMode;
                // Test Multi Value List Scenario With Default Parameters .
                var value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
                var bounds = value.TryValidateBounds(ninjaCoreSettings: ninjaCoreSettings);
                bounds.Should().BeOfType<Bounds>();
                bounds.Should().NotBeNull();
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(value.Count);
                var bytes = value.ToByteArray(ninjaCoreSettings: ninjaCoreSettings);
                var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Null List Scenario With Default Parameters.
                bounds = ((List<int>) null).TryValidateBounds(ninjaCoreSettings: ninjaCoreSettings);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = ((List<int>) null).ToByteArray(ninjaCoreSettings: ninjaCoreSettings);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeNull();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Empty List Scenario With Default Parameters.
                value = new List<int>();
                bounds = value.TryValidateBounds(ninjaCoreSettings: ninjaCoreSettings);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = value.ToByteArray(ninjaCoreSettings: ninjaCoreSettings);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().BeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Single Value List Scenario With Default Parameters.
                value = new List<int> { int.MinValue };
                bounds = value.TryValidateBounds(ninjaCoreSettings: ninjaCoreSettings);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(1);
                bytes = value.ToByteArray(ninjaCoreSettings: ninjaCoreSettings);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
            }
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryValidateArrayBoundModesWithParametersShouldPass()
        {
            // Array Bounds Settings. (A list that acts like an array with index and length params.)
            var boundsMode = BoundsMode.Array;
            // Test Multi Value List Scenario With Parameters .
            var value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
            var bounds = value.TryValidateBounds(skip: 0, take: 5, boundsMode: boundsMode);
            bounds.Should().BeOfType<Bounds>();
            bounds.Should().NotBeNull();
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(value.Count);
            var bytes = value.ToByteArray(skip: 0, take: 5, boundsMode: boundsMode);
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Null List Scenario With Default Parameters.
            bounds = ((List<int>) null).TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = ((List<int>) null).ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeNull();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Empty List Scenario With Default Parameters.
            value = new List<int>();
            bounds = value.TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Single Value List Scenario With Default Parameters.
            value = new List<int> { int.MinValue };
            bounds = value.TryValidateBounds(skip: 0, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(1);
            bytes = value.ToByteArray(skip: 0, take: 1, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: 1, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(1);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 1, take: 0, boundsMode: boundsMode);
            bytes.Should().BeNullOrEmpty();
            value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
            bounds = value.TryValidateBounds(skip: 0, take: 6, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(6);
            bounds.InvalidBounds.Should().NotBeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: 5, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(5);
            bounds.IntendedTake.Should().Be(0);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bytes = value.ToByteArray(skip: 5, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: -3, take: 9, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(-3);
            bounds.IntendedTake.Should().Be(9);
            bounds.InvalidBounds.Should().NotBeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: 2, take: -4, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(2);
            bounds.IntendedTake.Should().Be(-4);
            bounds.InvalidBounds.Should().NotBeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: 5, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(5);
            bounds.IntendedTake.Should().Be(1);
            bounds.InvalidBounds.Should().NotBeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: -4, take: -1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(-4);
            bounds.IntendedTake.Should().Be(-1);
            bounds.InvalidBounds.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryValidatePassThroughBoundModesWithParametersShouldPass()
        {
            // PassThrough Settings.
            var boundsMode = BoundsMode.PassThrough;
            // Test Multi Value List Scenario With Parameters .
            var value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
            var bounds = value.TryValidateBounds(skip: 0, take: 5, boundsMode: boundsMode);
            bounds.Should().BeOfType<Bounds>();
            bounds.Should().NotBeNull();
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(value.Count);
            var bytes = value.ToByteArray(skip: 0, take: 5, boundsMode: boundsMode);
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Null List Scenario With Default Parameters.
            bounds = ((List<int>) null).TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = ((List<int>) null).ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeNull();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Empty List Scenario With Default Parameters.
            value = new List<int>();
            bounds = value.TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Single Value List Scenario With Default Parameters.
            value = new List<int> { int.MinValue };
            bounds = value.TryValidateBounds(skip: 0, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(1);
            bytes = value.ToByteArray(skip: 0, take: 1, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: 1, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(1);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 1, take: 0, boundsMode: boundsMode);
            bytes.Should().BeNullOrEmpty();
            value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
            bounds = value.TryValidateBounds(skip: 0, take: 6, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(6);
            bounds.InvalidBounds.Should().BeEmpty();
            bytes = value.ToByteArray(skip: 0, take: 6, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: 5, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(5);
            bounds.IntendedTake.Should().Be(0);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bytes = value.ToByteArray(skip: 5, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: -3, take: 9, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(-3);
            bounds.IntendedTake.Should().Be(9);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: 2, take: -4, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(2);
            bounds.IntendedTake.Should().Be(-4);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: 5, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(5);
            bounds.IntendedTake.Should().Be(1);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bytes = value.ToByteArray(skip: 5, take: 1, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: -4, take: -1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(-4);
            bounds.IntendedTake.Should().Be(-1);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryValidateNinjaAndListBoundModesWithParametersShouldPass()
        {
            // Ninja & List Bounds Settings. (Arrays that act like lists.)
            foreach (var boundsMode in BoundsModes.Where(
                mode => mode == BoundsMode.Ninja || mode == BoundsMode.List))
            {
                // Test Multi Value List Scenario With Parameters .
                var value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
                var bounds = value.TryValidateBounds(skip: 0, take: 5, boundsMode: boundsMode);
                bounds.Should().BeOfType<Bounds>();
                bounds.Should().NotBeNull();
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(value.Count);
                var bytes = value.ToByteArray(skip: 0, take: 5, boundsMode: boundsMode);
                var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Null List Scenario With Default Parameters.
                bounds = ((List<int>) null).TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = ((List<int>) null).ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeNull();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Empty List Scenario With Default Parameters.
                value = new List<int>();
                bounds = value.TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = value.ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().BeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Single Value List Scenario With Default Parameters.
                value = new List<int> {int.MinValue};
                bounds = value.TryValidateBounds(skip: 0, take: 1, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(1);
                bytes = value.ToByteArray(skip: 0, take: 1, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                bounds = value.TryValidateBounds(skip: 1, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(1);
                bounds.IntendedTake.Should().Be(0);
                bytes = value.ToByteArray(skip: 1, take: 0, boundsMode: boundsMode);
                bytes.Should().BeNullOrEmpty();
                value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
                bounds = value.TryValidateBounds(skip: 0, take: 6, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(6);
                bounds.InvalidBounds.Should().BeNullOrEmpty();
                bounds = value.TryValidateBounds(skip: 5, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(5);
                bounds.IntendedTake.Should().Be(0);
                bounds.InvalidBounds.Should().BeNullOrEmpty();
                bounds = value.TryValidateBounds(skip: -3, take: 9, boundsMode: boundsMode);
                bounds.IsValid.Should().BeFalse();
                bounds.IntendedSkip.Should().Be(-3);
                bounds.IntendedTake.Should().Be(9);
                bounds.InvalidBounds.Should().NotBeNullOrEmpty();
                bounds = value.TryValidateBounds(skip: 2, take: -4, boundsMode: boundsMode);
                bounds.IsValid.Should().BeFalse();
                bounds.IntendedSkip.Should().Be(2);
                bounds.IntendedTake.Should().Be(-4);
                bounds.InvalidBounds.Should().NotBeNullOrEmpty();
                bounds = value.TryValidateBounds(skip: 5, take: 1, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(5);
                bounds.IntendedTake.Should().Be(1);
                bounds.InvalidBounds.Should().BeNullOrEmpty();
                bytes = value.ToByteArray(skip: 5, take: 1, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().BeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                bounds = value.TryValidateBounds(skip: -4, take: -1, boundsMode: boundsMode);
                bounds.IsValid.Should().BeFalse();
                bounds.IntendedSkip.Should().Be(-4);
                bounds.IntendedTake.Should().Be(-1);
                bounds.InvalidBounds.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryClearAllBoundModesShouldPass()
        {
            var ninjaCoreSettings = new NinjaCoreSettings();
            // Iterate through all BoundsMode values.
            foreach (var boundsMode in BoundsModes)
            {
                // Default Ninja Bounds Settings.
                NinjaCoreSettings.DefaultBoundsMode = boundsMode;
                
                var value = Encoding.UTF8.GetBytes("0123456789").ToList();
                value.Should().BeOfType<List<byte>>();
                value.Should().NotBeNullOrEmpty();

                var partialClear = Encoding.UTF8.GetBytes("012345\0\089").ToList();
                partialClear.Should().BeOfType<List<byte>>();
                partialClear.Should().NotBeNullOrEmpty();

                var fullClear = Encoding.UTF8.GetBytes("\0\0\0\0\0\0\0\0\0\0").ToList();
                fullClear.Should().BeOfType<List<byte>>();
                fullClear.Should().NotBeNullOrEmpty();

                var isValid = value.TryClear(skip: 6, take: 2, ninjaCoreSettings: ninjaCoreSettings);
                value.Should().BeEquivalentTo(partialClear);
                isValid.Should().BeTrue();

                value = Encoding.UTF8.GetBytes("0123456789").ToList();
                isValid = value.TryClear(skip: 0, take: 10, ninjaCoreSettings: ninjaCoreSettings);
                value.Should().BeEquivalentTo(fullClear);
                isValid.Should().BeTrue();

                value = Encoding.UTF8.GetBytes("0123456789").ToList();
                isValid = value.TryClear(skip: 6, take: 2, clearAfterUse: true, ninjaCoreSettings: ninjaCoreSettings);
                value.Should().BeEquivalentTo(fullClear);
                isValid.Should().BeTrue();

                if (boundsMode != BoundsMode.Ninja && boundsMode != BoundsMode.List)
                    continue;

                value = Encoding.UTF8.GetBytes("0123456789").ToList();
                isValid = value.TryClear(skip: 10, take: 10, ninjaCoreSettings: ninjaCoreSettings);
                value.Should().NotBeEquivalentTo(fullClear);
                isValid.Should().BeTrue();

                isValid = value.TryClear(skip: 7, take: 10, clearAfterUse: true, ninjaCoreSettings: ninjaCoreSettings);
                value.Should().BeEquivalentTo(fullClear);
                isValid.Should().BeTrue();
            }
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArrayAsBoolShouldPass()
        {
            var value = new List<bool> { true, false, false, true, false };
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
            var value = new List<double> { 0,  double.MaxValue, double.MinValue, double.MaxValue, 0 };
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
            var value = new List<float> { 0, float.MaxValue, float.MinValue, float.MaxValue, 0 };
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
            var value = new List<int> { 0, int.MaxValue, int.MinValue, int.MaxValue, 0 };
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
            var value = new List<long> { 0, long.MaxValue, long.MinValue, long.MaxValue, 0 };
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
            var value = new List<short> { 0, short.MaxValue, short.MinValue, short.MaxValue, 0 };
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
            var value = new List<uint> { 0, uint.MaxValue, uint.MinValue, uint.MaxValue, 0 };
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
            var value = new List<ulong> { 0, ulong.MaxValue, ulong.MinValue, ulong.MaxValue, 0 };
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
            var value = new List<ushort> { 0, ushort.MaxValue, ushort.MinValue, ushort.MaxValue, 0};
            var bytes = value.ToByteArray();
            var expectedBytes = bytes.ToCharacterArray(Encoding.Unicode).ToByteArray(Encoding.Unicode);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
        }
    }
}
