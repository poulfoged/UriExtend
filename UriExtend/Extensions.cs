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
        private static Regex queryPart = new Regex(@"[^\?#]*\??([^#]*)", RegexOptions.Compiled);

        /// <summary>
        /// Builds the uri's query string
        /// </summary>
        /// <param name="url">Uri to add parameter to</param>
        /// <param name="parameters">An anonymous object which will be converted to query parameters</param>
        /// <returns></returns>
        public static Uri AddQuery(this Uri url, object parameters)
        {
            var query = ToQueryString(parameters);

            if (url.IsAbsoluteUri)
            {
                var uriBuilder = new UriBuilder(url) {Port = url.Authority.EndsWith(url.Port.ToString(CultureInfo.InvariantCulture)) ? url.Port : -1};
                if (string.IsNullOrWhiteSpace(uriBuilder.Query))
                    uriBuilder.Query = ToQueryString(parameters);
                else
                    uriBuilder.Query = string.Format("{0}&{1}", uriBuilder.Query.Substring(1), ToQueryString(parameters));

                return uriBuilder.Uri;
            }
            
            var uriString = queryPart.Replace(url.ToString(), match => match.Value.Contains("?") ? (match.Value + "&" + query) : (match.Value + "?" + query), 1);
            return new Uri(uriString, UriKind.Relative);
        }

        private static string ToQueryString(object parameters)
        {
            var propertyList = parameters
                .GetType()
                .GetTypeInfo()
                .GetProperties()
                .ToList();

            var namesList = propertyList.Select(p => GetValue(parameters, p)).Where(i => !string.IsNullOrWhiteSpace(i));
            return string.Join("&", namesList);
        }

        private static string GetValue(dynamic parameters, PropertyInfo p)
        {
            var value = p.GetValue(parameters, null);

            if (value == null)
                return "";

            if (p.PropertyType == typeof(DateTime))
                return string.Format("{0}={1}", p.Name, Encode(value.ToString("o")));
            
            var enumerable = value as IEnumerable;
            if ((enumerable == null) || (p.PropertyType == typeof(string)))
                return string.Format("{0}={1}", p.Name, Encode(value));

            var list = enumerable
                .Cast<object>()
                .ToList();

            return string.Join("&", list.Select(d => string.Format("{0}={1}", p.Name, Encode(d))));
        }

        private static object Encode(object getValue)
        {
            if (getValue is string)
                return WebUtility.UrlEncode((string) getValue);
            
            if (getValue is bool)
                return getValue.ToString().ToLowerInvariant();
            
            return getValue;
        }
    }
}


