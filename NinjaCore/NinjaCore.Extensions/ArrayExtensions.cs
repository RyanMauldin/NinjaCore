using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NinjaCore.Extensions.Models;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Array extensions methods to make code cleaner where feasible in regards to a security conscious mindset.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array">The array to operate on.</param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="skip">The start index for <paramref name="array"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="array"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="array"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="array"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="array"/> by zeroing
        /// out or defaulting the values in the <paramref name="array"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ToByteArray(this byte[] array, Encoding encoding = null, int? skip = null, int? take = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);
            encoding = InternalNinjaCoreSettings.GetEncoding(ninjaCoreSettings, encoding);
            char[] charResults = null;

            try
            {
                // If the array has valid bounds than process list.
                var bounds = array.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!bounds.IsValid || bounds.InvalidBounds.Count > 0)
                    throw bounds.ToException();

                // Do basic validation check and return early if there is no values to process.
                if (array == null) return null;
                if (!array.Any()) return new byte[0];

                // Convert the array values and return the results.
                charResults = encoding.GetChars(array, bounds.IntendedSkip, bounds.IntendedTake);
                if (!charResults.Any()) return new byte[0];
                var byteResults = encoding.GetBytes(charResults);
                return byteResults.Any() ? byteResults : new byte[0];
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                if (clearAfterUse.Value && charResults != null && !charResults.IsReadOnly && charResults.Length > 0)
                    Array.Clear(charResults, 0, charResults.Length);
                throw;
            }
            finally
            {
                // Clear array contents.
                if (!exceptionThrown && clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                if (!exceptionThrown && clearAfterUse.Value && charResults != null && !charResults.IsReadOnly && charResults.Length > 0)
                    Array.Clear(charResults, 0, charResults.Length);
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array">The array to operate on.</param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="skip">The start index for <paramref name="array"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="array"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="array"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="array"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="array"/> by zeroing
        /// out or defaulting the values in the <paramref name="array"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ToByteArray(this char[] array, Encoding encoding = null, int? skip = null, int? take = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);
            encoding = InternalNinjaCoreSettings.GetEncoding(ninjaCoreSettings, encoding);

            try
            {
                // If the array has valid bounds than process list.
                var bounds = array.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!bounds.IsValid || bounds.InvalidBounds.Count > 0)
                    throw bounds.ToException();

                // Do basic validation check and return early if there is no values to process.
                if (array == null) return null;
                if (!array.Any()) return new byte[0];

                // Convert the array values and return the results.
                var byteResults = encoding.GetBytes(array, bounds.IntendedSkip, bounds.IntendedTake);
                return byteResults.Any() ? byteResults : new byte[0];
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                throw;
            }
            finally
            {
                // Clear array contents.
                if (!exceptionThrown && clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array">The array to operate on.</param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="skip">The start index for <paramref name="array"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="array"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="array"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="array"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="array"/> by zeroing
        /// out or defaulting the values in the <paramref name="array"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>A char array.</returns>
        public static char[] ToCharacterArray(this byte[] array, Encoding encoding = null, int? skip = null, int? take = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);
            encoding = InternalNinjaCoreSettings.GetEncoding(ninjaCoreSettings, encoding);

            try
            {
                // If the array has valid bounds than process list.
                var bounds = array.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!bounds.IsValid || bounds.InvalidBounds.Count > 0)
                    throw bounds.ToException();

                // If the array has not been initialized or contains no elements than exit the function.
                if (encoding == null)
                    encoding = NinjaCoreSettings.DefaultEncoding;

                // Do basic validation check and return early if there is no values to process.
                if (array == null) return null;
                if (!array.Any()) return new char[0];

                var charResults = encoding.GetChars(array, bounds.IntendedSkip, bounds.IntendedTake);
                return charResults.Any() ? charResults : new char[0];
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                throw;
            }
            finally
            {
                // Clear array contents.
                if (!exceptionThrown && clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array">The array to operate on.</param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="skip">The start index for <paramref name="array"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="array"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="array"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="array"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="array"/> by zeroing
        /// out or defaulting the values in the <paramref name="array"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>A char array.</returns>
        public static char[] ToCharacterArray(this char[] array, Encoding encoding = null, int? skip = null, int? take = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);
            encoding = InternalNinjaCoreSettings.GetEncoding(ninjaCoreSettings, encoding);
            byte[] byteResults = null;

            try
            {
                // If the array has valid bounds than process list.
                var bounds = array.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!bounds.IsValid || bounds.InvalidBounds.Count > 0)
                    throw bounds.ToException();

                // Do basic validation check and return early if there is no values to process.
                if (array == null) return null;
                if (!array.Any()) return new char[0];

                byteResults = encoding.GetBytes(array, bounds.IntendedSkip, bounds.IntendedTake);
                if (!byteResults.Any()) return new char[0];
                var charResults = encoding.GetChars(byteResults);
                return charResults.Any() ? charResults : new char[0];
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                if (clearAfterUse.Value && byteResults != null && !byteResults.IsReadOnly && byteResults.Length > 0)
                    Array.Clear(byteResults, 0, byteResults.Length);
                throw;
            }
            finally
            {
                // Clear array contents.
                if (!exceptionThrown && clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                if (!exceptionThrown && clearAfterUse.Value && byteResults != null && !byteResults.IsReadOnly && byteResults.Length > 0)
                    Array.Clear(byteResults, 0, byteResults.Length);
            }
        }

        /// <summary>
        /// Clears specified array values by setting all of the array values to the default value of
        /// <typeparamref name="T"/> parameter for use with the current <paramref name="skip"/> and
        /// <paramref name="take"/> values.
        /// </summary>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="array">The array to operate on.</param>
        /// <param name="skip">The start index for <paramref name="array"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="array"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="array"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="array"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="array"/> by zeroing
        /// out or defaulting the values in the <paramref name="array"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>True if the clear succeeded, false if not.</returns>
        public static bool TryClear<T>(this T[] array, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the array has valid bounds than process list.
                var bounds = array.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!bounds.IsValid || bounds.InvalidBounds.Count > 0)
                    throw bounds.ToException();

                // Do basic validation check and return early if there is no values to process.
                if (array == null) return true;
                if (array.IsReadOnly) return false;
                if (!array.Any()) return true;

                // Clear the array based on the user array bounds and return the array.
                var arrayLength = array.Length;
                var intendedSkip = bounds.IntendedSkip;
                var intendedTake = bounds.IntendedTake;
                var intendedRange = intendedSkip + intendedTake;
                switch (boundsMode)
                {
                    case BoundsMode.Ninja:
                    case BoundsMode.List:
                        if (intendedSkip >= arrayLength) return true;

                        if (intendedRange <= arrayLength)
                        {
                            Array.Clear(array, intendedSkip, intendedTake);
                            return true;
                        }

                        intendedRange = arrayLength - Math.Max(intendedSkip, 0);
                        Array.Clear(array, intendedSkip, intendedRange);
                        return true;
                    case BoundsMode.Array:
                    case BoundsMode.PassThrough:
                        Array.Clear(array, bounds.IntendedSkip, bounds.IntendedTake);
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
                throw;
            }
            finally
            {
                // Clear array contents.
                if (!exceptionThrown && clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Validates the specified array is valid for use with the <paramref name="skip"/> and
        /// <paramref name="take"/> parameters if provided, and provides back the <seealso cref="Bounds"/>
        /// summary of validity, error messages, and intended measurements. 
        /// </summary>
        /// <param name="array">The array to operate on.</param>
        /// <param name="skip">The start index for <paramref name="array"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="array"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="array"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="array"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="array"/> by zeroing
        /// out or defaulting the values in the <paramref name="array"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>The <seealso cref="Bounds"/> parameter.</returns>
        public static Bounds TryValidateBounds<T>(this T[] array, int? skip = null, int? take = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // Capture array bounds.
                var arrayLength = array?.Length ?? 0;
                var invalidBounds = new List<InvalidBounds>();
                var intendedSkip = skip;
                var intendedTake = take;

                // Allowing less than zero if something different subscribes to this interface.
                if (arrayLength <= 0)
                {
                    // If the array has not been initialized or contains no elements than assume this is a valid response.
                    switch (boundsMode)
                    {
                        case BoundsMode.Array:
                            // If skip only has a default value.
                            if (!intendedSkip.HasValue) intendedSkip = 0;
                            // Negative skip value is not allowed in array mode.
                            else if (intendedSkip < 0) invalidBounds.Add(new InvalidBounds(nameof(skip),
                                $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                            // Skip value is too large for array mode with length of zero.
                            else if (intendedSkip > 0) invalidBounds.Add(new InvalidBounds(nameof(skip),
                                $"The '{nameof(skip)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));

                            // If take only has a default value.
                            if (!intendedTake.HasValue) intendedTake = 0;
                            // Negative take value is not allowed in array mode.
                            else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            // Skip value is too large for array mode with length of zero.
                            else if (intendedTake.Value > 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                            break;
                        case BoundsMode.Ninja:
                        case BoundsMode.List:
                            // If skip only has a default value.
                            if (!intendedSkip.HasValue) intendedSkip = 0;
                            // Negative skip values are not allowed in list mode.
                            else if (intendedSkip < 0) invalidBounds.Add(new InvalidBounds(nameof(skip),
                                $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                            // Skip can be greater than or equal to the list length in list mode.

                            // If take only has a default value.
                            if (!intendedTake.HasValue) intendedTake = 0;
                            // Negative take value is not allowed in list mode.
                            else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            // Take can be greater than or equal to the list length in list mode.
                            break;
                        case BoundsMode.PassThrough:
                            intendedSkip ??= 0;
                            intendedTake ??= 0;
                            break;
                    }
                }
                else
                {
                    // If the list has not been initialized or contains no elements than assume this is a valid response.
                    switch (boundsMode)
                    {
                        case BoundsMode.Array:
                            // If skip only has a default value.
                            if (!intendedSkip.HasValue || intendedSkip.Value == 0)
                            {
                                // Default skip value.
                                intendedSkip = 0;
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedTake.Value > arrayLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > 0 && intendedSkip.Value <= arrayLength)
                            {
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedSkip.Value + intendedTake.Value > arrayLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > arrayLength)
                            {
                                // Skip value is too large for array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(skip),
                                    $"The '{nameof(skip)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedTake.Value > arrayLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            else if (intendedSkip.Value < 0)
                            {
                                // Negative skip values are not allowed in array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(skip),
                                    $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedTake.Value > arrayLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            break;
                        case BoundsMode.Ninja:
                        case BoundsMode.List:
                            // If skip only has a default value.
                            if (!intendedSkip.HasValue || intendedSkip.Value == 0)
                            {
                                // Default skip value.
                                intendedSkip = 0;
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > 0 && intendedSkip.Value <= arrayLength)
                            {
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > arrayLength)
                            {
                                // Skip value is too large for array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(skip),
                                    $"The '{nameof(skip)}' parameter holds a value out of bounds of the '{nameof(array)}' parameter."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            else if (intendedSkip.Value < 0)
                            {
                                // Negative skip values are not allowed in array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(skip),
                                    $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = arrayLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            break;
                        case BoundsMode.PassThrough:
                            intendedSkip ??= 0;
                            intendedTake ??= arrayLength;
                            break;
                    }
                }

                // Make sure take has a value.
                intendedSkip ??= 0;
                intendedTake ??= 0;

                // The list bounds are valid and return the list.
                return new Bounds(nameof(array), intendedSkip.Value, intendedTake.Value, invalidBounds);
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || array == null || array.IsReadOnly || array.Length <= 0) throw;
                Array.Clear(array, 0, array.Length);
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && array != null && !array.IsReadOnly && array.Length > 0)
                    Array.Clear(array, 0, array.Length);
            }
        }
    }
}