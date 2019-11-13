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
    /// Tests for <seealso cref="ArrayExtensions" /> class extension methods.
    /// </summary>
    public class ArrayExtensionTests
    {
        public static List<BoundsMode> BoundsModes = new List<BoundsMode>
            { BoundsMode.Ninja, BoundsMode.List, BoundsMode.Array, BoundsMode.PassThrough };

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryValidateAllBoundModesWithDefaultParametersShouldPass()
        {
            var settings = new NinjaCoreSettings();
            // Iterate through all BoundsMode values.
            foreach (var boundsMode in BoundsModes)
            {
                // Default Ninja Bounds Settings.
                settings.BoundsMode = boundsMode;
                // Test Multi Value List Scenario With Default Parameters .
                var value = Encoding.UTF8.GetBytes("0123456789");
                value.Should().BeOfType<byte[]>();
                var bounds = value.TryValidateBounds(settings: settings);
                bounds.Should().BeOfType<Bounds>();
                bounds.Should().NotBeNull();
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(value.Length);
                var bytes = value.ToByteArray(settings: settings);
                var expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Null List Scenario With Default Parameters.
                bounds = ((byte[])null).TryValidateBounds(settings: settings);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = ((byte[])null).ToByteArray(settings: settings);
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeNull();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Empty List Scenario With Default Parameters.
                value = new byte[0];
                bounds = value.TryValidateBounds(settings: settings);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = value.ToByteArray(settings: settings);
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().BeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Single Value List Scenario With Default Parameters.
                value = new byte[] { 0x01 };
                bounds = value.TryValidateBounds(settings: settings);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(1);
                bytes = value.ToByteArray(settings: settings);
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
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
            var value = Encoding.UTF8.GetBytes("0123456789");
            value.Should().BeOfType<byte[]>();
            var bounds = value.TryValidateBounds(skip: 0, take: 5, boundsMode: boundsMode);
            bounds.Should().BeOfType<Bounds>();
            bounds.Should().NotBeNull();
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(value.Length / 2);
            var bytes = value.ToByteArray(skip: 0, take: 5, boundsMode: boundsMode);
            var expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Null List Scenario With Default Parameters.
            bounds = ((byte[])null).TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = ((byte[])null).ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeNull();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Empty List Scenario With Default Parameters.
            value = new byte[0];
            bounds = value.TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Single Value List Scenario With Default Parameters.
            value = new byte[] { 0x01 };
            bounds = value.TryValidateBounds(skip: 0, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(1);
            bytes = value.ToByteArray(skip: 0, take: 1, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: 1, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(1);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 1, take: 0, boundsMode: boundsMode);
            bytes.Should().BeNullOrEmpty();
            value = Encoding.UTF8.GetBytes("0123456789");
            bounds = value.TryValidateBounds(skip: 0, take: 11, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(11);
            bounds.InvalidBounds.Should().NotBeNullOrEmpty();
            bounds = value.TryValidateBounds(skip: 10, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(10);
            bounds.IntendedTake.Should().Be(0);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bytes = value.ToByteArray(skip: 5, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
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
            bounds = value.TryValidateBounds(skip: 10, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeFalse();
            bounds.IntendedSkip.Should().Be(10);
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
            var value = Encoding.UTF8.GetBytes("0123456789");
            value.Should().BeOfType<byte[]>();
            var bounds = value.TryValidateBounds(skip: 0, take: 5, boundsMode: boundsMode);
            bounds.Should().BeOfType<Bounds>();
            bounds.Should().NotBeNull();
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(value.Length / 2);
            var bytes = value.ToByteArray(skip: 0, take: 5, boundsMode: boundsMode);
            var expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Null List Scenario With Default Parameters.
            bounds = ((byte[])null).TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = ((byte[])null).ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeNull();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Empty List Scenario With Default Parameters.
            value = new byte[0];
            bounds = value.TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().BeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            // Test Single Value List Scenario With Default Parameters.
            value = new byte[] { 0x01 };
            bounds = value.TryValidateBounds(skip: 0, take: 1, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(1);
            bytes = value.ToByteArray(skip: 0, take: 1, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: 1, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(1);
            bounds.IntendedTake.Should().Be(0);
            bytes = value.ToByteArray(skip: 1, take: 0, boundsMode: boundsMode);
            bytes.Should().BeNullOrEmpty();
            value = Encoding.UTF8.GetBytes("0123456789");
            bounds = value.TryValidateBounds(skip: 0, take: 6, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(0);
            bounds.IntendedTake.Should().Be(6);
            bounds.InvalidBounds.Should().BeEmpty();
            bytes = value.ToByteArray(skip: 0, take: 6, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
            bytes.Should().BeEquivalentTo(expectedBytes);
            bounds = value.TryValidateBounds(skip: 5, take: 0, boundsMode: boundsMode);
            bounds.IsValid.Should().BeTrue();
            bounds.IntendedSkip.Should().Be(5);
            bounds.IntendedTake.Should().Be(0);
            bounds.InvalidBounds.Should().BeNullOrEmpty();
            bytes = value.ToByteArray(skip: 5, take: 0, boundsMode: boundsMode);
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
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
            expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
            bytes.Should().BeOfType<byte[]>();
            bytes.Should().NotBeNullOrEmpty();
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
                var value = Encoding.UTF8.GetBytes("0123456789");
                value.Should().BeOfType<byte[]>();
                var bounds = value.TryValidateBounds(skip: 0, take: 5, boundsMode: boundsMode);
                bounds.Should().BeOfType<Bounds>();
                bounds.Should().NotBeNull();
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(value.Length / 2);
                var bytes = value.ToByteArray(skip: 0, take: 5, boundsMode: boundsMode);
                var expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Null List Scenario With Default Parameters.
                bounds = ((byte[])null).TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = ((byte[])null).ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeNull();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Empty List Scenario With Default Parameters.
                value = new byte[0];
                bounds = value.TryValidateBounds(skip: 0, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(0);
                bytes = value.ToByteArray(skip: 0, take: 0, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().BeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                // Test Single Value List Scenario With Default Parameters.
                value = new byte[] { 0x01 };
                bounds = value.TryValidateBounds(skip: 0, take: 1, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(0);
                bounds.IntendedTake.Should().Be(1);
                bytes = value.ToByteArray(skip: 0, take: 1, boundsMode: boundsMode);
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
                bytes.Should().BeEquivalentTo(expectedBytes);
                bounds = value.TryValidateBounds(skip: 1, take: 0, boundsMode: boundsMode);
                bounds.IsValid.Should().BeTrue();
                bounds.IntendedSkip.Should().Be(1);
                bounds.IntendedTake.Should().Be(0);
                bytes = value.ToByteArray(skip: 1, take: 0, boundsMode: boundsMode);
                bytes.Should().BeNullOrEmpty();
                value = Encoding.UTF8.GetBytes("0123456789");
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
                expectedBytes = bytes.ToCharacterArray(Encoding.UTF8).ToByteArray(Encoding.UTF8);
                bytes.Should().BeOfType<byte[]>();
                bytes.Should().NotBeNullOrEmpty();
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
        public void MockedTestForTryClear()
        {
            var settings = new NinjaCoreSettings();
            // Iterate through all BoundsMode values.
            foreach (var boundsMode in BoundsModes)
            {
                // Default Ninja Bounds Settings.
                settings.BoundsMode = boundsMode;

                var value = Encoding.UTF8.GetBytes("0123456789");
                value.Should().BeOfType<byte[]>();
                value.Should().NotBeNullOrEmpty();

                var partialClear = Encoding.UTF8.GetBytes("012345\0\089");
                partialClear.Should().BeOfType<byte[]>();
                partialClear.Should().NotBeNullOrEmpty();

                var fullClear = Encoding.UTF8.GetBytes("\0\0\0\0\0\0\0\0\0\0");
                fullClear.Should().BeOfType<byte[]>();
                fullClear.Should().NotBeNullOrEmpty();

                var isValid = value.TryClear(skip: 6, take: 2, settings: settings);
                value.Should().BeEquivalentTo(partialClear);
                isValid.Should().BeTrue();

                value = Encoding.UTF8.GetBytes("0123456789");
                isValid = value.TryClear(skip: 0, take: 10, settings: settings);
                value.Should().BeEquivalentTo(fullClear);
                isValid.Should().BeTrue();

                value = Encoding.UTF8.GetBytes("0123456789");
                isValid = value.TryClear(skip: 6, take: 2, clearAfterUse: true, settings: settings);
                value.Should().BeEquivalentTo(fullClear);
                isValid.Should().BeTrue();

                if (boundsMode != BoundsMode.Ninja && boundsMode != BoundsMode.List)
                    continue;

                value = Encoding.UTF8.GetBytes("0123456789");
                isValid = value.TryClear(skip: 10, take: 10, settings: settings);
                value.Should().NotBeEquivalentTo(fullClear);
                isValid.Should().BeTrue();

                isValid = value.TryClear(skip: 7, take: 10, clearAfterUse: true, settings: settings);
                value.Should().BeEquivalentTo(fullClear);
                isValid.Should().BeTrue();
            }
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForTryValidateArrayBounds()
        {
            var valueBytes = Encoding.UTF8.GetBytes("0123456789");
            valueBytes.Should().BeOfType<byte[]>();
            valueBytes.Should().NotBeNullOrEmpty();

            var partialValidateBytes = Encoding.UTF8.GetBytes("0123456789");
            partialValidateBytes.Should().BeOfType<byte[]>();
            partialValidateBytes.Should().NotBeNullOrEmpty();

            var fullClearBytes = Encoding.UTF8.GetBytes("\0\0\0\0\0\0\0\0\0\0");
            fullClearBytes.Should().BeOfType<byte[]>();
            fullClearBytes.Should().NotBeNullOrEmpty();

            var boundsValidationResult = valueBytes.TryValidateBounds(skip: 6, take: 2);
            valueBytes.Should().BeEquivalentTo(partialValidateBytes);
            boundsValidationResult.IsValid.Should().BeTrue();
            boundsValidationResult.IntendedTake.Should().Be(2);

            boundsValidationResult = valueBytes.TryValidateBounds(skip: 6, take: 2, clearAfterUse: true);
            boundsValidationResult.IsValid.Should().BeTrue();
            boundsValidationResult.IntendedTake.Should().Be(2);
            valueBytes.Should().BeEquivalentTo(fullClearBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToByteArray()
        {
            var valueBytes = Encoding.UTF8.GetBytes("0123456789");
            valueBytes.Should().BeOfType<byte[]>();
            valueBytes.Should().NotBeNullOrEmpty();

            var partialValidateBytes = Encoding.UTF8.GetBytes("67");
            partialValidateBytes.Should().BeOfType<byte[]>();
            partialValidateBytes.Should().NotBeNullOrEmpty();

            var fullClearBytes = Encoding.UTF8.GetBytes("\0\0\0\0\0\0\0\0\0\0");
            fullClearBytes.Should().BeOfType<byte[]>();
            fullClearBytes.Should().NotBeNullOrEmpty();

            var byteArrayResult = valueBytes.ToByteArray(skip: 6, take: 2);
            byteArrayResult.Should().BeOfType<byte[]>();
            byteArrayResult.Should().NotBeNullOrEmpty();
            byteArrayResult.Should().BeEquivalentTo(partialValidateBytes);

            byteArrayResult = valueBytes.ToByteArray(skip: 6, take: 2, clearAfterUse: true);
            byteArrayResult.Should().BeOfType<byte[]>();
            byteArrayResult.Should().NotBeNullOrEmpty();
            byteArrayResult.Should().BeEquivalentTo(partialValidateBytes);
            valueBytes.Should().BeEquivalentTo(fullClearBytes);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MockedTestForToCharacterArray()
        {
            var valueBytes = Encoding.UTF8.GetBytes("0123456789");
            valueBytes.Should().BeOfType<byte[]>();
            valueBytes.Should().NotBeNullOrEmpty();

            var partialValidateBytes = Encoding.UTF8.GetBytes("67");
            var partialValidateChars = Encoding.UTF8.GetChars(partialValidateBytes);
            partialValidateBytes.Should().BeOfType<byte[]>();
            partialValidateBytes.Should().NotBeNullOrEmpty();
            partialValidateChars.Should().BeOfType<char[]>();
            partialValidateChars.Should().NotBeNullOrEmpty();

            var fullClearBytes = Encoding.UTF8.GetBytes("\0\0\0\0\0\0\0\0\0\0");
            var fullClearChars = Encoding.UTF8.GetChars(fullClearBytes);
            fullClearBytes.Should().BeOfType<byte[]>();
            fullClearBytes.Should().NotBeNullOrEmpty();
            fullClearChars.Should().BeOfType<char[]>();
            fullClearChars.Should().NotBeNullOrEmpty();

            var charArrayResult = valueBytes.ToCharacterArray(skip: 6, take: 2);
            charArrayResult.Should().BeOfType<char[]>();
            charArrayResult.Should().NotBeNullOrEmpty();
            charArrayResult.Should().BeEquivalentTo(partialValidateChars);

            charArrayResult = valueBytes.ToCharacterArray(skip: 6, take: 2, clearAfterUse: true);
            charArrayResult.Should().BeOfType<char[]>();
            charArrayResult.Should().NotBeNullOrEmpty();
            charArrayResult.Should().BeEquivalentTo(partialValidateChars);
            valueBytes.Should().BeEquivalentTo(fullClearChars);
        }
    }
}
