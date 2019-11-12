using System;
using System.Collections.Generic;
using System.Linq;
using NinjaCore.Extensions.Models;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Extension methods that work specifically on IList.
    /// </summary>
    public static class IListExtensions
    {
        // bool // char // double // float // int // long // short // unint // ulong // ushort
        
        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<bool> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
           var exceptionThrown = false;
           boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
           clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<char> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<double> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<float> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<int> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<long> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<short> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<uint> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns><returns>A byte array.</returns></returns>
        public static byte[] ToByteArray(this IList<ulong> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }

        /// <summary>
        /// Converts an IList to a encoded list of bytes with the respect to the source to target encodings and can be
        /// use with the <paramref name="skip"/> and <paramref name="take"/> parameters if provided.
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>A byte array.</returns>
        public static byte[] ToByteArray(this IList<ushort> list, int? skip = null, int? take = null, BoundsMode? boundsMode = null,
            bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // If the list has valid bounds than process list.
                var boundsValidationResult = list.TryValidateBounds(skip, take, boundsMode, clearAfterUse: false, ninjaCoreSettings);
                if (!boundsValidationResult.IsValid || boundsValidationResult.InvalidBounds.Count > 0)
                    throw boundsValidationResult.ToException(nameof(list));

                // Do basic validation check and return early if there is no values to process.
                if (list == null) return null;
                if (!list.Any()) return new byte[0];

                return list.Skip(boundsValidationResult.IntendedSkip).Take(boundsValidationResult.IntendedTake)
                    .SelectMany(BitConverter.GetBytes).ToArray();
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear array contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear array contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }


        /// <summary>
        /// Validates the specified array is valid for use with the <paramref name="skip"/> and
        /// <paramref name="take"/> parameters if provided, and provides back the <seealso cref="BoundsValidationResult"/>
        /// summary of validity, error messages, and intended measurements. 
        /// </summary>
        /// <param name="list">The list to operate on.</param>
        /// <param name="skip">The start index for <paramref name="list"/> parameter.</param>
        /// <param name="take"> The length of elements to take from the <paramref name="list"/> parameter.</param>
        /// <param name="boundsMode">The <seealso cref="BoundsMode"/> rules of how to treat the <paramref name="list"/> data type.</param>
        /// <param name="clearAfterUse">
        /// If <paramref name="clearAfterUse"/> parameter is set to true and the <paramref name="list"/> parameter is
        /// not a readonly copy, than there will be an attempt made to clear the <paramref name="list"/> by zeroing
        /// out or defaulting the values in the <paramref name="list"/>, even in the event an exception is thrown.
        /// This helps with application security by giving developers the ability to clear plain text or critical data
        /// along route of a fluent syntax style chain of expression blocks; aiding in code readability and security.
        /// </param>
        /// <param name="ninjaCoreSettings">The ninja core settings object.</param>
        /// <returns>The <seealso cref="BoundsValidationResult"/> parameter.</returns>
        public static BoundsValidationResult TryValidateBounds<T>(this IList<T> list, int? skip = null, int? take = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null, NinjaCoreSettings ninjaCoreSettings = null)
        {
            var exceptionThrown = false;
            boundsMode = InternalNinjaCoreSettings.GetBoundsMode(ninjaCoreSettings, boundsMode);
            clearAfterUse = InternalNinjaCoreSettings.GetClearAfterUse(ninjaCoreSettings, clearAfterUse);

            try
            {
                // Capture list bounds.
                var listLength = list?.Count ?? 0;
                var invalidBounds = new List<InvalidBounds>();
                var intendedSkip = skip;
                var intendedTake = take;

                // Allowing less than zero if something different subscribes to this interface.
                if (listLength <= 0)
                {
                    // If the list has not been initialized or contains no elements than assume this is a valid response.
                    switch (boundsMode)
                    {
                        case BoundsMode.Array:
                            // If skip only has a default value.
                            if (!intendedSkip.HasValue) intendedSkip = 0;
                            // Negative skip value is not allowed in array mode.
                            else if (intendedSkip < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                            // Skip value is too large for array mode with length of zero.
                            else if (intendedSkip > 0) invalidBounds.Add(new InvalidBounds(nameof(skip),
                                $"The '{nameof(skip)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));

                            // If take only has a default value.
                            if (!intendedTake.HasValue) intendedTake = 0;
                            // Negative take value is not allowed in array mode.
                            else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            // Skip value is too large for array mode with length of zero.
                            else if (intendedTake.Value > 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
                            break;
                        case BoundsMode.Ninja:
                        case BoundsMode.List:
                            // If skip only has a default value.
                            if (!intendedSkip.HasValue) intendedSkip = 0;
                            // Negative skip values are not allowed in list mode.
                            else if (intendedSkip < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                            // Skip can be greater than or equal to the list length in list mode.

                            // If take only has a default value.
                            if (!intendedTake.HasValue) intendedTake = 0;
                            // Negative take value is not allowed in list mode.
                            else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            // Take can be greater than or equal to the list length in list mode.
                            break;
                        // case BoundsMode.PassThrough:
                        default:
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
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedTake.Value > listLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > 0 && intendedSkip.Value <= listLength)
                            {
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedSkip.Value + intendedTake.Value > listLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > listLength)
                            {
                                // Skip value is too large for array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(skip),
                                    $"The '{nameof(skip)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedTake.Value > listLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            else if (intendedSkip.Value < 0)
                            {
                                // Negative skip values are not allowed in array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Skip and take value is too large for array mode.
                                else if (intendedTake.Value > listLength) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
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
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > 0 && intendedSkip.Value <= listLength)
                            {
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            // Skip value is too large for array mode if skip is the index.
                            else if (intendedSkip.Value > listLength)
                            {
                                // Skip value is too large for array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(skip),
                                    $"The '{nameof(skip)}' parameter holds a value out of bounds of the '{nameof(list)}' parameter."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            else if (intendedSkip.Value < 0)
                            {
                                // Negative skip values are not allowed in array mode.
                                invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(skip)}' parameter cannot hold a value less than zero."));
                                // If take only has a default value.
                                if (!intendedTake.HasValue) intendedTake = listLength;
                                // Nothing to take.
                                else if (intendedTake.Value == 0) intendedTake = 0;
                                // Negative take value is not allowed in array mode.
                                else if (intendedTake.Value < 0) invalidBounds.Add(new InvalidBounds(nameof(take),
                                    $"The '{nameof(take)}' parameter cannot hold a value less than zero."));
                            }
                            break;
                        // case BoundsMode.PassThrough:
                        default:
                            intendedSkip ??= 0;
                            intendedTake ??= 0;
                            break;
                    }
                }

                // Make sure take has a value.
                intendedTake ??= 0;

                // The list bounds are valid and return the list.
                return new BoundsValidationResult(intendedSkip.Value, intendedTake.Value, invalidBounds);
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear list contents.
                if (!clearAfterUse.Value || list == null || list.IsReadOnly || list.Count <= 0) throw;
                for (var i = 0; i < list.Count; i++)
                    list[i] = default;
                throw;
            }
            finally
            {
                // Clear list contents after return value has been copied to calling code.
                if (!exceptionThrown && clearAfterUse.Value && list != null && !list.IsReadOnly && list.Count > 0)
                    for (var i = 0; i < list.Count; i++)
                        list[i] = default;
            }
        }
    }
}
