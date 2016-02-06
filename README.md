# FakeSystemWeb

[![NuGet Version](https://img.shields.io/nuget/vpre/FakeSystemWeb.svg)](https://www.nuget.org/packages/FakeSystemWeb)
[![NuGet Downloads](https://img.shields.io/nuget/dt/FakeSystemWeb.svg)](https://www.nuget.org/packages/FakeSystemWeb)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/mergut/FakeSystemWeb/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/vxk7b3pg64k5n9wq/branch/master?svg=true)](https://ci.appveyor.com/project/mergut/fakesystemweb/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/mergut/FakeSystemWeb/badge.svg?branch=master)](https://coveralls.io/github/mergut/FakeSystemWeb?branch=master)

Fake implementations of base classes in System.Web for unit tests.

## Installation
FakeSystemWeb is available as a NuGet package. You can install it using the NuGet Package Manager Console window:
```
PM> Install-Package FakeSystemWeb
```

## Usage

```csharp
// Method to be tested
public bool IsHeaderPresent(HttpContextBase context)
{
    return context.Request.Headers["header"] == "value";
}

[Test]
public void IsHeaderPresentTest()
{
    var context = new FakeHttpContext();
    context.Request.Headers.Add("header", "value");

    var result = IsHeaderPresent(context);

    Assert.True(result);
}
```
