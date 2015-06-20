using System;
using NUnit.Framework;

namespace UriExtend.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        private static object[] _testCases =
        {
            new object[] {new Uri("/", UriKind.Relative), new { Animal = "cat" }, new Uri("/?Animal=cat", UriKind.Relative) },
            new object[] {new Uri("/?a=b&c=d", UriKind.Relative), new { Animal = "cat" }, new Uri("/?a=b&c=d&Animal=cat", UriKind.Relative) },
            new object[] {new Uri("?a=b&c=d", UriKind.Relative), new { Animal = "cat" }, new Uri("?a=b&c=d&Animal=cat", UriKind.Relative) },
            new object[] {new Uri("/#fragment", UriKind.Relative), new { Animal = "cat" }, new Uri("/?Animal=cat#fragment", UriKind.Relative) },
            new object[] {new Uri("/dir1/dir2/file?a=b&c=d", UriKind.Relative), new { Animal = "cat" }, new Uri("/dir1/dir2/file?a=b&c=d&Animal=cat", UriKind.Relative) },
            new object[] {new Uri("/dir1/dir2/file?a=b&c=d#fragment", UriKind.Relative), new { Animal = "cat" }, new Uri("/dir1/dir2/file?a=b&c=d&Animal=cat#fragment", UriKind.Relative) },
            new object[] {new Uri("http://www.example.com"), new { Animal = "cat" }, new Uri("http://www.example.com/?Animal=cat") },
            new object[] {new Uri("https://www.example.com"), new { Animal = "cat" }, new Uri("https://www.example.com/?Animal=cat") },
            new object[] {new Uri("http://www.example.com"), new { Animals = new[] { "cat", "dog", "pigion" } }, new Uri("http://www.example.com/?Animals=cat&Animals=dog&Animals=pigion") },
            new object[] {new Uri("http://www.example.com?car=bmw"), new { Animal = "cat" }, new Uri("http://www.example.com/?car=bmw&Animal=cat") },
            new object[] {new Uri("http://www.example.com"), new { Animal = true }, new Uri("http://www.example.com/?Animal=true") },
            new object[] {new Uri("http://www.example.com:1234"), new { Animal = true }, new Uri("http://www.example.com:1234/?Animal=true") },
            new object[] {new Uri("http://www.example.com:80"), new { Animal = true }, new Uri("http://www.example.com:80/?Animal=true") },
            new object[] {new Uri("http://www.example.com"), new { Arg = "with space" }, new Uri("http://www.example.com/?Arg=with+space") },
            new object[] {new Uri("http://www.example.com"), new { Arg = "with#hash" }, new Uri("http://www.example.com/?Arg=with%23hash") },
            new object[] {new Uri("http://www.example.com"), new { Arg = "# %" }, new Uri("http://www.example.com/?Arg=%23+%25") },
            new object[] {new Uri("http://www.example.com"), new { Today = new DateTime(2000, 1, 1) }, new Uri("http://www.example.com/?Today=2000-01-01T00:00:00.0000000") },
            new object[] {new Uri("http://www.example.com"), new { Number = 42 }, new Uri("http://www.example.com/?Number=42") },
            new object[] {new Uri("http://www.example.com"), new { Span = TimeSpan.FromMinutes(10)}, new Uri("http://www.example.com/?Span=00:10:00") },
        };

        [TestCaseSource(typeof(ExtensionsTests), "_testCases")]
        public void Can_add_query(Uri source, object parameters, Uri expected)
        {
            ////Act
            var result = source
                .AddQuery(parameters);

            ////Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
