using System;
using System.Linq;
using System.Text;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Array extensions methods to make code cleaner where feasible in regards to a security conscious mindset.
    /// There are numerous flags to enhance performance in regards to using these calls multiple ways.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Clears specified array values by setting all of the array values to the default value of
        /// <typeparamref name="T"/> parameter for use with the current <paramref name="index"/> and
        /// <paramref name="length"/> values and uses intended length difference based on the values
        /// set for <paramref name="index"/> and <paramref name="length"/> parameters.
        /// </summary>
        /// <remarks>
        /// This method facilitates chaining and fluent syntax. Passing in the value zero for the
        /// <paramref name="length"/> parameter defaults the length to be the same length as the
        /// <paramref name="array"/> parameter with the <paramref name="index"/> subtracted difference. When the
        /// <paramref name="clearOnException"/> parameter is set to true, any exceptions being thrown are caught
        /// internally, and the <paramref name="array"/> parameter values are cleared to default values for the
        /// entire array, not just the considerations of the <paramref name="index"/> and <paramref name="length"/>
        /// parameters. After the arrays are cleared any caught exception is rethrown. Also, the "clearAfterUse"
        /// parameter found in other related extension methods has been left out of this method intentionally,
        /// as the goal is to clear the array within specific set bounds from namely the index and length parameters,
        /// however here the clearOnException can be set to true, and the array value gets completely cleared
        /// if something fails.
        /// </remarks>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="array">The array to clear.</param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="rethrowOnException">
        /// After any internal exceptions are caught and array values cleared, the exception is rethrown unless
        /// <paramref name="rethrowOnException"/> value is set to false. Setting <paramref name="rethrowOnException"/>
        /// to false allows this method to operate as a simple try action, however one that gives more control of
        /// usage by also allowing exceptions to bubble up if <paramref name="rethrowOnException"/> is set to true.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that
        /// all you now need sitting around in memory as far as sensitive data is now in the result array that got
        /// passed by value back to your calling method, however the data stuck and pointed to by the variable
        /// in this method still in need of being cleared, after the fact of the result being returned and
        /// copied to the calling code array value variable.
        /// </param>
        /// <returns>The <paramref name="array"/> parameter instance to allow for method chaining.</returns>
        public static T[] TryClear<T>(this T[] array, int index = 0, int length = 0, bool clearOnException = false,
            bool rethrowOnException = false, bool clearAfterUse = false)
        {
            try
            {
                // If the array has not been initialized or contains no elements, exit the function.
                if (array == null || !array.Any()) return array;

                // If the array has valid bounds, clear the array using the index and intended length.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid)
                    throw new IndexOutOfRangeException();

                // Clear the array based on the user array bounds and return the array. 
                Array.Clear(array, index, intendedLength);
                return array;
            }
            catch (Exception)
            {
                // Clear array contents.
                if (clearOnException && array != null && array.Any()) Array.Clear(array, 0, array.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                if (rethrowOnException) throw; // TODO: Evaluate options.
                return array;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && array != null && array.Any()) Array.Clear(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Validates the specified array is valid for use with the current <paramref name="index"/> and
        /// <paramref name="length"/> values and provides back the intended length difference using the
        /// <paramref name="intendedLength"/> parameter based on the values set for <paramref name="index"/> and
        /// <paramref name="length"/> parameters.
        /// </summary>
        /// <remarks>
        /// This method facilitates chaining and fluent syntax. Passing in the value zero for the
        /// <paramref name="length"/> parameter defaults the length to be the same length as the
        /// <paramref name="array"/> parameter with the <paramref name="index"/> subtracted difference. When the
        /// <paramref name="clearOnException"/> parameter is set to true, any exceptions being thrown are caught
        /// internally, and the <paramref name="array"/> parameter values are cleared to default values for the
        /// entire array, not just the considerations of the <paramref name="index"/> and <paramref name="length"/>
        /// parameters. After the arrays are cleared any caught exception is rethrown.
        /// </remarks>
        /// <typeparam name="T">The generic type parameter.</typeparam>
        /// <param name="array">The array to validate bounds.</param>
        /// <param name="intendedLength">
        /// The value that would be used as 'length' in any additional calls against the <paramref name="array"/> parameter.
        /// If <paramref name="length"/> was passed in with a value of zero, the intended length gets set to the difference
        /// between the value of the <paramref name="index"/> parameter and the <paramref name="array"/> parameter length.
        /// Otherwise it is set to the value of the <paramref name="length"/> parameter if it held a valid range.
        /// </param>
        /// <param name="isValid"></param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="rethrowOnException">
        /// After any internal exceptions are caught and array values cleared, the exception is rethrown unless
        /// <paramref name="rethrowOnException"/> value is set to false. Setting <paramref name="rethrowOnException"/>
        /// to false allows this method to operate as a simple try validator, however one that gives more control of
        /// usage by also allowing exceptions to bubble up if <paramref name="rethrowOnException"/> is set to true.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications or practical purposes, the <paramref name="clearAfterUse"/> parameter
        /// can be set to true which allows the finally block to attempt to call <seealso cref="TryClear{T}"/> on the
        /// <paramref name="array"/> parameter, as the idea is that you no longer need this value after we
        /// get our validation result <paramref name="isValid"/> and perhaps this value is in fact sensitive data,
        /// so removing sensitive values from memory is a good practice if we do not need the result array
        /// to have the same data after a call to this method. I mean why have to call another method invocation
        /// chained after this method call, to clear the data in the <paramref name="array"/> parameter, if this
        /// method ends up being the final method in the call chain, and the result value is never utilized.
        /// </param>
        /// <returns>The <paramref name="array"/> parameter instance to allow for method chaining.</returns>
        public static T[] TryValidateArrayBounds<T>(this T[] array, out int intendedLength, out bool isValid, int index = 0,
            int length = 0, bool clearOnException = false, bool rethrowOnException = false, bool clearAfterUse = false)
        {
            // Assign defaults for intendedLength and isValidBounds.
            intendedLength = 0;
            isValid = false;

            try
            {
                // If the array has not been initialized or contains no elements, exit the function.
                if (array == null || !array.Any()) return array;
                // Index cannot be less than 0.
                if (index < 0) throw new ArgumentOutOfRangeException(nameof(index),
                    $"The '{nameof(index)}' parameter cannot hold a value less than zero.");
                // Length cannot be less than 0.
                if (length < 0) throw new ArgumentOutOfRangeException(nameof(length),
                    $"The '{nameof(length)}' parameter cannot hold a value less than zero.");
                // Capture array bounds.
                var arrayLength = array.Length;
                // Validate the index is within the bounds of the array.
                if (index >= arrayLength) throw new IndexOutOfRangeException(
                    $"The '{nameof(index)}' parameter holds a value out of scope of the '{nameof(array)}' parameter length.");
                // Grab the valid intended length for the operations request for this array.
                intendedLength = length;
                if (intendedLength == 0)
                    intendedLength = arrayLength - index;
                // Validate the intended index range is within the bounds of the array.
                var intendedIndexRange = index + intendedLength;
                if (intendedIndexRange >= arrayLength) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");
                // If the intended index range, which is computed by adding the values of the index and length parameters,
                // is greater than the the length of the array parameter than throw an index out of range exception.
                if (arrayLength < intendedIndexRange) throw new IndexOutOfRangeException(
                    $"The index location '{intendedIndexRange}' does not exist in the '{nameof(array)}' parameter.");
                // The array bounds are valid and return the array.
                isValid = true;
                return array;
            }
            catch (Exception)
            {
                isValid = false;
                // Clear array contents.
                if (clearOnException && array != null && array.Any()) Array.Clear(array, 0, array.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                if (rethrowOnException) throw; // TODO: Evaluate options.
                return array;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && array != null && array.Any()) Array.Clear(array, 0, array.Length); 
            }
        }

        /// <summary>
        /// Converts array using the UTF8 encoding value.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static byte[] ToByteArray(this byte[] array, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;
            var encoding = Encoding.UTF8;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                byteResults = new byte[0];
                if (!array.Any()) return byteResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                charResults = encoding.GetChars(array);
                byteResults = encoding.GetBytes(charResults);
                return byteResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                    if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static byte[] ToByteArray(this byte[] array, Encoding encoding, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                byteResults = new byte[0];
                if (!array.Any()) return byteResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                charResults = encoding.GetChars(array);
                byteResults = encoding.GetBytes(charResults);
                return byteResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                    if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the UTF8 encoding value.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static byte[] ToByteArray(this char[] array, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            var exceptionThrown = false;
            var encoding = Encoding.UTF8;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                byteResults = new byte[0];
                if (!array.Any()) return byteResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                byteResults = encoding.GetBytes(array);
                return byteResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static byte[] ToByteArray(this char[] array, Encoding encoding, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            var exceptionThrown = false;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                byteResults = new byte[0];
                if (!array.Any()) return byteResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                byteResults = encoding.GetBytes(array);
                return byteResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the UTF8 encoding value.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static char[] ToCharacterArray(this byte[] array, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            char[] charResults = null;
            var exceptionThrown = false;
            var encoding = Encoding.UTF8;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                charResults = new char[0];
                if (!array.Any()) return charResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                charResults = encoding.GetChars(array);
                return charResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static char[] ToCharacterArray(this byte[] array, Encoding encoding, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            char[] charResults = null;
            var exceptionThrown = false;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                charResults = new char[0];
                if (!array.Any()) return charResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                charResults = encoding.GetChars(array);
                return charResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the UTF8 encoding value.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static char[] ToCharacterArray(this char[] array, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;
            var encoding = Encoding.UTF8;

            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                charResults = new char[0];
                if (!array.Any()) return charResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                byteResults = encoding.GetBytes(array);
                charResults = encoding.GetChars(byteResults);
                return charResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                    if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                }
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="index">The index location where the clear array command will begin.</param>
        /// <param name="length">
        /// The length of elements to clear for <paramref name="array"/> parameter, starting at the value of the
        /// <paramref name="index"/> parameter location. If the <paramref name="length"/> is set to 0, the value
        /// will be automatically calculated to the end of the array.
        /// </param>
        /// <param name="clearOnException">
        /// For security aware applications, the <paramref name="clearOnException"/> parameter should be set to true.
        /// The <paramref name="clearOnException"/> setting activates code when exceptions are thrown that clears the
        /// values of all internal scoped array values including the <paramref name="array"/> parameter values. When
        /// the values of all arrays are cleared, the exception is rethrown.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true.
        /// When <paramref name="clearAfterUse"/> parameter is set to true, the finally block attempts to call
        /// <seealso cref="TryClear{T}"/> on the <paramref name="array"/> parameter, as the idea is that,
        /// all you now need sitting around in memory as far as sensitive data is now in the result array.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static char[] ToCharacterArray(this char[] array, Encoding encoding, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;
            
            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                charResults = new char[0];
                if (!array.Any()) return charResults;

                // Validate if the array has valid bounds prior to any processing.
                array.TryValidateArrayBounds(out var intendedLength, out var isValid, index, length, clearOnException,
                    rethrowOnException, clearAfterUse);
                if (!isValid) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");

                if (index > 0)
                {
                    // Rebase array before resize.
                    for (var i = 0; i < intendedLength; i++)
                        array[i] = array[i + index];
                }

                // Attempt array resize.
                if (array.Length != intendedLength)
                    Array.Resize(ref array, intendedLength);

                // Convert the array values and return the results.
                byteResults = encoding.GetBytes(array);
                charResults = encoding.GetChars(byteResults);
                return charResults;
            }
            catch (Exception)
            {
                exceptionThrown = true;
                if (!clearOnException) throw;

                // Clear array contents.
                if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                throw; // TODO: Evaluate options.
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && !(exceptionThrown && clearOnException))
                {
                    if (array != null && array.Any()) Array.Clear(array, 0, array.Length);
                    if (byteResults != null && byteResults.Any()) Array.Clear(byteResults, 0, byteResults.Length);
                    if (charResults != null && charResults.Any()) Array.Clear(charResults, 0, charResults.Length);
                }
            }
        }
    }
}
