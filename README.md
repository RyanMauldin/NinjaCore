# NinjaCore
.NET Core Extension Libraries

**NinjaCore** is an open source project aimed at creating high quality .NET Core / .NET Standard extension methods and services that will be available as NuGet packages. The current version at the time of this writing is .NET Core 3.0 / .NET Standard 2.1.

**Code Samples:**

*NinjaCore.Extensions.Base64Extensions ...*

```

public const string TextTestValue = @"A test value which includes special characters ~!@#$%^&*()(_+_++--==";

[Fact]
[Trait("Category", "Mocked")]
public void MockedTestForBase64ConversionShouldPass()
{
	var encodedValue = TextTestValue.ToBase64String();
	encodedValue.Should().BeOfType<string>();
	encodedValue.Should().NotBeNullOrWhiteSpace();
	var decodedValue = encodedValue.FromBase64String();
	decodedValue.Should().BeOfType<string>();
	decodedValue.Should().NotBeNullOrWhiteSpace();
	decodedValue.Should().BeEquivalentTo(TextTestValue);
}

```
