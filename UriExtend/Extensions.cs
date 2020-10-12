using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace UriExtend
{
    /// <summary>
    /// Adds extension methods to <see cref="System.Uri"/>
    /// </summary>
    public static class UriExtensions
    {
        private static readonly Regex queryPart = new Regex(@"[^\?#]*\??([^#]*)", RegexOptions.Compiled);

        /// <summary>
        /// Builds the uri's query string
        /// </summary>
        /// <param name="url">Uri to add parameter to</param>
        /// <param name="parameters">An anonymous object which will be converted to query parameters</param>
        /// <param name="urlEncoder">Optional url-encoder to override the default WebUtility.UrlEncode</param>
        /// <returns></returns>
        public static Uri AddQuery(this Uri url, object parameters, Func<string, string> urlEncoder = null)
        {
            urlEncoder = urlEncoder ?? WebUtility.UrlEncode;

            var query = ToQueryString(parameters, urlEncoder);

            if (url.IsAbsoluteUri)
            {
                var uriBuilder = new UriBuilder(url) {Port = url.Authority.EndsWith(url.Port.ToString(CultureInfo.InvariantCulture)) ? url.Port : -1};
                if (string.IsNullOrWhiteSpace(uriBuilder.Query))
                    uriBuilder.Query = ToQueryString(parameters, urlEncoder);
                else
                    uriBuilder.Query = string.Format("{0}&{1}", uriBuilder.Query.Substring(1), ToQueryString(parameters, urlEncoder));

                return uriBuilder.Uri;
            }
            
            var uriString = queryPart.Replace(url.ToString(), match => match.Value.Contains("?") ? (match.Value + "&" + query) : (match.Value + "?" + query), 1);
            return new Uri(uriString, UriKind.Relative);
        }

        private static string ToQueryString(object parameters, Func<string, string> urlEncoder)
        {
            var propertyList = parameters
                .GetType()
                .GetTypeInfo()
                .GetProperties()
                .ToList();

            var namesList = propertyList.Select(p => GetValue(parameters, p, urlEncoder)).Where(i => !string.IsNullOrWhiteSpace(i));
            return string.Join("&", namesList);
        }

        private static string GetValue(dynamic parameters, PropertyInfo p, Func<string, string> urlEncoder)
        {
            var value = p.GetValue(parameters, null);

            if (value == null)
                return "";

            if (p.PropertyType == typeof(DateTime))
                return string.Format("{0}={1}", p.Name, Encode(value.ToString("o"), urlEncoder));

            if ((!(value is IEnumerable enumerable)) || (p.PropertyType == typeof(string)))
                return string.Format("{0}={1}", p.Name, Encode(value, urlEncoder));

            var list = enumerable
                .Cast<object>()
                .ToList();

            return string.Join("&", list.Select(d => string.Format("{0}={1}", p.Name, Encode(d, urlEncoder))));
        }

        private static object Encode(object getValue, Func<string, string> urlEncoder)
        {
            if (getValue is string x)
                return urlEncoder(x);
            
            if (getValue is bool)
                return getValue.ToString().ToLowerInvariant();
            
            return getValue;
        }
    }
}


