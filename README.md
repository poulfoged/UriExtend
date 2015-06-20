# UriExtend
Extends the .NET built in Uri with a fluent interface for adding to the query string.

UriExtend does not have any dependencies and builds heavily on built in types by simply extending the already immutable System.Uri.

UriExtend works by converting a provided anonymous type into a list of query-arguments. Arguments are mostly converted to (encoded) string representations with special handling for the following types:

* Boolean → true/false
* DateTime → e.g. 2008-09-22T14:01:54.9571247Z (ISO 8601)

Also lists are converted to multiple arguments with the same name.

Internally UriExtend uses System.UriBuilder for absolute uri's and custom regular expressions for relative (UriBuilder does not support relative urls).

## Example

To add arguments to an existing url:

```c#
var url = new Uri("http://example.com")
				.Query(new { Animal = "cat" };
```
Url will become **http://example.com?Animal=cat**

Note that UriExtend just tries to fill some of the gaps - System.UriBuilder already does a fine job of modifying other parts of absolute uris with a semi-fluent interface:

```c#
var url = new UriBuilder("http://example.com")
{ 
	Path = "/folder/file1",
	Scheme = "https",
	Port = 80,
	Host = "example2",
	Query = "Animal=cat"
}
.Uri;
```

## How to

Simply add the Nuget package:

`PM> Install-Package UriExtend`

Or since this library is really only a single file - just copy paste [Extensions.cs](https://github.com/poulfoged/UriExtend/blob/master/UriExtend/Extensions.cs) into your project.

## Requirements

You'll need .NET Framework 4 or later to use the precompiled binaries.

## License

UriExtend is under the MIT license.
