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
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Clears specified array values by setting all of the array values to the default value of
        /// <typeparamref name="T"/> parameter for use with the current <paramref name="index"/> and
        /// <paramref name="length"/> values and uses intended length difference based on the values set for
        /// <paramref name="index"/> and <paramref name="length"/> parameters.
        /// </summary>
        /// <remarks>
        /// Passing in the value zero for the <paramref name="length"/> parameter defaults the length to be the
        /// same length as the <paramref name="array"/> parameter with the <paramref name="index"/> subtracted
        /// difference. When the <paramref name="clearOnException"/> parameter is set to true, any exceptions
        /// being thrown are caught internally, and the <paramref name="array"/> parameter values are cleared
        /// to default values for the entire array, not just the considerations of the <paramref name="index"/>
        /// and <paramref name="length"/> parameters. After the arrays are cleared any caught exception is
        /// rethrown. Also, the <paramref name="clearAfterUse"/> parameter has the goal to clear all array
        /// values in the finally block. Setting <paramref name="rethrowOnException"/> parameter to false allows
        /// this method to operate as a simple try action, however one that gives more control of usage by
        /// also allowing exceptions to bubble up if <paramref name="rethrowOnException"/> is set to true.
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
        /// <paramref name="rethrowOnException"/> value is set to false.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true
        /// when the data being operated on is no longer needed after the call to this method.
        /// </param>
        /// <returns>The <paramref name="array"/> parameter instance to allow for method chaining.</returns>
        public static bool TryClear<T>(this T[] array, int index = 0, int length = 0, bool clearOnException = false,
            bool rethrowOnException = false, bool clearAfterUse = false)
        {
            try
            {
                // If the array has valid bounds, clear the array using the index and intended length.
                if (!array.TryValidateArrayBounds(out var intendedLength, index, length, clearOnException,
                    rethrowOnException, clearAfterUse)) throw new ArgumentOutOfRangeException(
                        nameof(array), "Unable to validate the array value successfully.");

                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null || !array.Any()) return true;

                // Clear the array based on the user array bounds and return the array. 
                Array.Clear(array, index, intendedLength);
                return true;
            }
            catch (Exception e)
            {
                // Clear array contents.
                if (clearOnException && array != null && array.Any()) Array.Clear(array, 0, array.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                if (rethrowOnException) throw; // TODO: Evaluate options.
                return false;
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
        /// <paramref name="rethrowOnException"/> value is set to false.
        /// </param>
        /// <param name="clearAfterUse">
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true
        /// when the data being operated on is no longer needed after the call to this method.
        /// </param>
        /// <returns>The <paramref name="array"/> parameter instance to allow for method chaining.</returns>
        public static bool TryValidateArrayBounds<T>(this T[] array, out int intendedLength, int index = 0,
            int length = 0, bool clearOnException = false, bool rethrowOnException = false, bool clearAfterUse = false)
        {
            // Assign defaults for intendedLength and isValidBounds.
            intendedLength = 0;
            
            try
            {
                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null || !array.Any())
                {
                    if (index == 0 && length == 0) return true;
                    throw new ArgumentOutOfRangeException(nameof(index), $"The '{nameof(index)}' and '{nameof(length)}'" +
                        $" parameters cannot hold a value other than zero for a null or empty '{nameof(array)}'.");
                }
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
                    intendedLength = Math.Max(arrayLength - index, 1);
                // Validate the intended index range is within the bounds of the array.
                var intendedIndexRange = index + intendedLength;
                if (intendedIndexRange > arrayLength) throw new IndexOutOfRangeException(
                    $"The \"{nameof(length)}\" parameter holds a value that causes the \"{nameof(index)}\" parameter" +
                    $" to be out of scope of the \"{nameof(array)}\" parameter length.");
                // The array bounds are valid and return the array.
                return true;
            }
            catch (Exception e)
            {
                // Clear array contents.
                if (clearOnException && array != null && array.Any()) Array.Clear(array, 0, array.Length);
                // TODO: may want to throw a new exception here in case the existing exception has critical or
                // TODO: security sensitive data which has the potential of hitting logs.
                if (rethrowOnException) throw; // TODO: Evaluate options.
                return false;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (clearAfterUse && array != null && array.Any()) Array.Clear(array, 0, array.Length); 
            }
        }

        /// <summary>
        /// Converts array using the custom encoding value of <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="encoding"/> is null the default encoding <seealso cref="DefaultEncoding"/> is used
        /// which is currently set to Encoding.UTF8.
        /// </remarks>
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
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true
        /// when the data being operated on is no longer needed after the call to this method.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static byte[] ToByteArray<T>(this T[] array, Encoding encoding = null, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;

            try
            {
                // If the array has valid bounds, clear the array using the index and intended length.
                if (!array.TryValidateArrayBounds(out var intendedLength, index, length, clearOnException,
                    rethrowOnException, clearAfterUse)) throw new ArgumentOutOfRangeException(
                        nameof(array), "Unable to validate the array value successfully.");

                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                byteResults = new byte[0];
                if (!array.Any()) return byteResults;

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
                if (encoding == null)
                    encoding = DefaultEncoding;
                charResults = encoding.ToCharacterArray(array);
                byteResults = encoding.ToByteArray(charResults);
                return byteResults;
            }
            catch (Exception e)
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
        /// <remarks>
        /// If <paramref name="encoding"/> is null the default encoding <seealso cref="DefaultEncoding"/> is used
        /// which is currently set to Encoding.UTF8.
        /// </remarks>
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
        /// For security aware applications, the <paramref name="clearAfterUse"/> parameter should be set to true
        /// when the data being operated on is no longer needed after the call to this method.
        /// </param>
        /// <returns>An array of converted and encoded values.</returns>
        public static char[] ToCharacterArray<T>(this T[] array, Encoding encoding = null, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            const bool rethrowOnException = true;
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;

            try
            {
                // If the array has valid bounds, clear the array using the index and intended length.
                if (!array.TryValidateArrayBounds(out var intendedLength, index, length, clearOnException,
                    rethrowOnException, clearAfterUse)) throw new ArgumentOutOfRangeException(
                    nameof(array), "Unable to validate the array value successfully.");

                // If the array has not been initialized or contains no elements than exit the function.
                if (array == null) return null;
                charResults = new char[0];
                if (!array.Any()) return charResults;

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
                if (encoding == null)
                    encoding = DefaultEncoding;
                byteResults = encoding.ToByteArray(array);
                charResults = encoding.ToCharacterArray(byteResults);
                return charResults;
            }
            catch (Exception e)
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="encoding"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="clearOnException"></param>
        /// <param name="clearAfterUse"></param>
        /// <returns></returns>
        public static byte[] ToByteArray<T>(this Encoding encoding, T[] array, int index = 0, int length = 0,
            bool clearOnException = false, bool clearAfterUse = false)
        {
            byte[] byteResults = null;
            char[] charResults = null;
            var exceptionThrown = false;

            try
            {
                if (encoding == null) throw new ArgumentNullException(nameof(encoding));
                if (array == null) return null;
                if (!array.Any()) return new byte[0];
                if (array is byte[])
                {
                    byteResults = array as byte[];
                    return byteResults;
                }

                charResults = array as char[];
                if (array is char[]) return charResults.Any() ? encoding.GetBytes(charResults) : new byte[0];
                return null;
            }
            catch (Exception e)
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

        public static char[] ToCharacterArray<T>(this Encoding encoding, T[] array)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            if (array == null) return null;
            if (!array.Any()) return new char[0];
            if (array is char[] charResults) return charResults;
            if (!(array is byte[] byteResults)) return null;
            return byteResults.Any() ? encoding.GetChars(byteResults) : new char[0];
        }
    }
}
