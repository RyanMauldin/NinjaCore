using System;
using System.Net.Http;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Url extensions offer endpoint manipulation and transformation.
    /// </summary>
    /// <remarks>
    /// This class resolves issues as seen on the following urls:
    /// https://sebnilsson.com/blog/convert-c-uri-url-to-absolute-or-relative/
    /// https://stackoverflow.com/questions/7624987/whats-the-difference-between-uri-tostring-and-uri-absoluteuri
    /// </remarks>
    public static class UrlExtensions
    {
        public static readonly string DefaultResult;
        public static readonly char[] TrimCharacters;

        /// <summary>
        /// The static constructor initializes the static readonly values in this class.
        /// </summary>
        static UrlExtensions()
        {
            DefaultResult = string.Empty;
            TrimCharacters = new[] { '/', '\\', '#', '?', ':', ' ', '\t', '\n', '\r' };
        }

        /// <summary>
        /// This method cleans a url by returning a default result (currently an empty string), if the URL is null, empty,
        /// or all whitespace characters. This method trims both ends of the URL when trimming end slashes and other
        /// characters. This method can be called as follows: var absoluteUrl = absoluteUrlStringValue.ToCleanUrl();
        /// </summary>
        /// <param name="url">The URL value to clean.</param>
        /// <returns>A cleaned/trimmed result string.</returns>
        public static string ToCleanUrl(this string url)
        {
            // If the string is null or is all whitespace characters return the default result (empty string). Otherwise,
            // attempt to trim all un-necessary url symbol values.
            return string.IsNullOrWhiteSpace(url) ? DefaultResult : url.Trim(TrimCharacters);
        }

        /// <summary>
        /// This method cleans a URI by returning a default result (currently an empty string), if the URI is null, empty,
        /// or all whitespace characters. This method trims both ends of the URI when trimming end slashes and other
        /// characters. This method can be called as follows: var absoluteUrl = absoluteUri.ToCleanUrl();
        /// </summary>
        /// <param name="uri">The URI value to clean.</param>
        /// <returns>A cleaned/trimmed result string.</returns>
        public static string ToCleanUrl(this Uri uri)
        {
            // If url is null, return the default result, otherwise attempt to clean the url with the ToCleanUrl string
            // url extension method with the original URI string.
            return uri == null ? DefaultResult : uri.OriginalString.ToCleanUrl();
        }

        /// <summary>
        /// This method will return the relative URL from the URI passed in or it will return the default result.
        /// This method can be called as follows: var relativeUrl = absoluteUri.ToRelativeUrl();
        /// </summary>
        /// <param name="uri">The URI value to process.</param>
        /// <returns>
        /// An relative URL string that has been cleaned if the request. If the passed in URI is an absolute URI, the
        /// Path and Query values are preserved, however if it is not an absolute URI value that is passed in, it will
        /// simply return a cleaned/trimmed version of the original request string. If the URL is null, an empty string,
        /// or it is full of whitespace or other characters that get trimmed out, this method will return the value in
        /// DefaultResult variable (empty string).
        /// </returns>
        public static string ToRelativeUrl(this Uri uri)
        {
            // If the uri is null return the default result.
            if (uri == null) return DefaultResult;
            // If the uri is an absolute uri, return the path and query value, otherwise return the original request.
            var relativeUrl = uri.IsAbsoluteUri ? uri.PathAndQuery : uri.OriginalString;
            // Clean and trim
            return relativeUrl.ToCleanUrl();
        }

        /// <summary>
        /// This method will return the absolute URL from the  URI passed in, it's original value if it cannot be
        /// transformed, or it will return the default result (empty string). This method can be called as follows:
        /// var absoluteUrl = baseUri.ToAbsoluteUrl();
        /// </summary>
        /// <param name="uri">The URI value to process.</param>
        /// <returns>
        /// An absolute URL string that has been cleaned if the request is an absolute URI already or the original
        /// request string, if the URI is not an absolute URI. If the URL is null, an empty string, or it is full
        /// of whitespace or other characters that get trimmed out, this method will return the value in DefaultResult
        /// variable (empty string).
        /// </returns>
        public static string ToAbsoluteUrl(this Uri uri)
        {
            // If the uri is null return the default result.
            if (uri == null) return DefaultResult;

            // If the uri is an absolute uri, return the path and query
            // value. Otherwise, return the original request value.
            var url = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.OriginalString;

            // Clean and Trim
            return url.ToCleanUrl();
        }

        /// <summary>
        /// This method will return the absolute URL from the base address of the HTTP Client passed in. This method
        /// may also pass back the original value if the base address cannot be transformed, or it will return the
        /// default result (empty string). This can be used with our adapters in such that we can call:
        /// var baseUrl = client.ToAbsoluteUrl();
        /// </summary>
        /// <param name="client">The HttpClient with the base address to process.</param>
        /// <returns>An absolute url string.</returns>
        public static string ToAbsoluteUrl(this HttpClient client)
        {
            // If the Http Client is null, return the default result.
            if (client == null) return DefaultResult;

            // Fetch the HttpClient base address, and if the base address is unobtainable, try to simply return an
            // absolute Url value of the uri. Otherwise, return the default result.
            var baseAddress = client.BaseAddress;
            return baseAddress == null ? DefaultResult : baseAddress.ToAbsoluteUrl();
        }

        /// <summary>
        /// This method will return the absolute URL from the base address of the baseUrl parameter passed in. This
        /// method will also attempt to combine the absolute path from the base address with the relative address that
        /// may be stored in the URI parameter. This method may also pass back the original value of either the
        /// relative URI if the base address is not present or if it's not absolute, or this method will pass back the
        /// value of the base address if the URI relative address is not provided. This method may also return the
        /// default result (empty string). This can be used in such a way that we can call:
        /// var absoluteUrl = baseUrl.ToAbsoluteUrl(RelativeEndpointUri);
        /// </summary>
        /// <param name="baseUrl">The base address to process.</param>
        /// <param name="uri">The URI value to process and combine with the baseUrl.</param>
        /// <returns>An absolute url string.</returns>
        public static string ToAbsoluteUrl(this string baseUrl, Uri uri)
        {
            // We need to obtain the URI version of the baseUrl string parameter.
            Uri baseUri;

            // We need to clean the base URL string first to be as concise as possible, before turning it into a URI.
            baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? DefaultResult : baseUrl.ToCleanUrl();

            // If the URI parameter is null, we need to try and get out the base URI or return the default result.
            if (uri == null)
            {
                // If the base address is unobtainable, or if it is now an empty value, the URI is already empty and
                // so we cannot combine the values or do anything handy with this other than to return the default
                // result (empty string) and the calling application should error or log the error if needed.
                if (string.IsNullOrWhiteSpace(baseUrl))
                    return DefaultResult;

                // Create a new base URI.
                baseUri = new Uri(baseUrl, UriKind.Absolute);
                // Return the absolute URL of the base address or the default result (empty string).
                return baseUri.ToAbsoluteUrl() ?? DefaultResult;
            }

            // We have a non null URI, but our base address has a null, empty, or whitespace value. We need to return
            // the URI as an absolute value, or the default result.
            if (string.IsNullOrWhiteSpace(baseUrl))
                return uri.ToAbsoluteUrl() ?? DefaultResult;

            // We have a non null base address and a non null URI at this point so now we will attempt to resolve the
            // relative url from the URI and then combine it with the absolute URL value of the base address and then
            // will attempt to return a absolute URL that represents the endpoint. Otherwise, we need to return the
            // default result.
            var relativeUrl = uri.ToRelativeUrl();
            if (string.IsNullOrWhiteSpace(relativeUrl)) return baseUrl;

            // Fetch newly formed URI from the Base URL.
            baseUri = new Uri(baseUrl, UriKind.Absolute);
            if (baseUri == null)
                throw new Exception("Unable to create Base/Absolute URI.");

            // Fetch newly formed URI from the Relative URL.
            var relativeUri = new Uri(relativeUrl, UriKind.Relative);
            if (relativeUri == null)
                throw new Exception("Unable to create Relative URI.");

            // Now we will attempt to combine the URIs.
            if (Uri.TryCreate(baseUri, relativeUri, out var absoluteUri))
                return absoluteUri.ToCleanUrl(); // URLs are combined successfully, now clean and trim

            // The strings were not able to be combined together correctly if you reach this point in the code. Now
            // return the relative URL as it will mean a more specific path to hit for any receiving component, as
            // compared with the base URL, which is generic or general. If the relative URI does not have a value,
            // then choose the Base URI to represent the URI, otherwise return the default result.
            return relativeUri.ToAbsoluteUrl() ?? baseUri.ToAbsoluteUrl() ?? DefaultResult;
        }

        /// <summary>
        /// This method will return the absolute URL from the base address of the HTTP Client passed in. This method
        /// will also attempt to combine the absolute path from the base address of the Http Client with the relative
        /// address that may be stored in the URI parameter. This method may also pass back the original value of either
        /// the relative URI if the base address is not present, or the value of the base address if the URI is not
        /// provided, or it will return the default result (empty string). This can be used with our adapters as follows:
        /// var absoluteUrl = client.ToAbsoluteUrl(RelativeEndpointUri);
        /// </summary>
        /// <param name="client">The HttpClient with the base address to process.</param>
        /// <param name="uri">The URI value to process.</param>
        /// <returns>An absolute url string.</returns>
        public static string ToAbsoluteUrl(this HttpClient client, Uri uri)
        {
            // If client is null, attempt to resolve the absolute uri from the uri parameter. Otherwise, return the
            // default result (empty string).
            if (client == null) return uri?.ToAbsoluteUrl() ?? DefaultResult;

            // Fetch the HttpClient base address, and if the base address is unobtainable, try to simply return an
            // absolute Url value of the uri. Otherwise, return the default result.
            var baseUrl = client.ToAbsoluteUrl();
            if (string.IsNullOrWhiteSpace(baseUrl))
                return uri?.ToAbsoluteUrl() ?? DefaultResult;

            return baseUrl.ToAbsoluteUrl(uri);
        }
    }
}
