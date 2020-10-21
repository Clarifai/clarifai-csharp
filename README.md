![Clarifai logo](logo.png)

# DEPRECATED

## This API Client is no longer supported.

### Please use [Clarifai C# gRPC](https://github.com/Clarifai/clarifai-csharp-grpc) instead, which is faster and more feature-rich.

# Clarifai API C# Client

[![NuGet](https://img.shields.io/nuget/v/Clarifai.svg)](https://www.nuget.org/packages/Clarifai)
[![Build Status](https://travis-ci.org/Clarifai/clarifai-csharp.svg?branch=master)](https://travis-ci.org/Clarifai/clarifai-csharp)
[![Build status](https://ci.appveyor.com/api/projects/status/osiexiua9ig1w3as/branch/master?svg=true)](https://ci.appveyor.com/project/robertwenquan/clarifai-csharp-1dm15/branch/master)

* Try the Clarifai demo at: https://clarifai.com/demo
* Sign up for a free account at: https://clarifai.com/developer/signup/
* Read the developer guide at: https://clarifai.com/developer/guide/

## Installation

### Adding the Clarifai C# library to your project

Within Visual Studio IDE:

```
Install-Package Clarifai
```

with the `dotnet` command line tool:

```
dotnet add package Clarifai
```

###  Prerequisites

This library supports .NET Standard 1.3 which means it can be used on the following .NET implementations:

- .NET Framework 4.6+
- .NET Core 1.0+
- Mono 4.6+
- Xamarin.iOS 10.0+
- Xamarin.Mac 3.0+
- Xamarin.Android 7.0+
- UWP 10.0+

## Getting Started

Find complete code samples in the [docs](https://clarifai.com/developer/docs/).

```csharp
using System;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs.Inputs;

namespace hello_csharp
{
    internal class Program
    {
        public static async Task Main()
        {
            // With `CLARIFAI_API_KEY` defined as an environment variable
            var client = new ClarifaiClient();
            
            // When passed in as a string
            var client = new ClarifaiClient("YOUR_CLARIFAI_API_KEY");
            
            // When using async/await
            var res = await client.PublicModels.GeneralModel
                .Predict(new ClarifaiURLImage("https://samples.clarifai.com/metro-north.jpg"))
                .ExecuteAsync();
                            
            // When synchronous
            var res = client.PublicModels.GeneralModel
                .Predict(new ClarifaiURLImage("https://samples.clarifai.com/metro-north.jpg"))
                .ExecuteAsync()
                .Result;

            // Print the concepts
            foreach (var concept in res.Get().Data)
            {
                Console.WriteLine($"{concept.Name}: {concept.Value}");
            }
        }
    }
}
```

[Public models](https://www.clarifai.com/models) can easily be used:

```csharp
var response = await client.PublicModels.GeneralModel.Predict(new ClarifaiURLImage("IMAGE URL"))
    .ExecuteAsync();
```

As well as [custom trained]():

```csharp
var response = await client.Predict<Concept>(
        "YOUR_MODEL_ID",
        new ClarifaiURLImage("https://samples.clarifai.com/metro-north.jpg")
    )
    .ExecuteAsync();
```

### Generics and casting

The library uses generics to specify the type of an output. In the first example using the `Predict` method, we use `Concept` to indicate that the model is a concept model. If we'd use a model ID of our public color model, we'd use `Color` instead of `Concept`.

Not all methods can however use generics. For example, the `GetModels` is able to return multiple models and you cannot determine in advance (at compile time) the types of model that will be returned. The request therefore returns a list of `IModel` objects. You can cast these instances to specific models, if you know its type:

```csharp
if (models[0].OutputInfo.TypeExt == "concept")
{
    ConceptModel model = (ConceptModel) models[0];
}
```

In a similar way you may cast `IPrediction` when the actual class type is not known at compile time.

Please see more in the [Developer's Guide](https://clarifai.com/developer/guide/).

## Tests

`NUnit 3 VS Test Adapter` must be installed.

### Unit tests

Run all unit tests using .NET Core by running this command in the solution directory:

```
dotnet test Clarifai.UnitTests/Clarifai.UnitTests.csproj
```

### Integration tests

To successfully run integration tests, you have to have a valid Clarifai API key with all required permissions.

Create a new API key at the [API keys page](https://www.clarifai.com/developer/account/api-keys) and set it as an environmental variable `CLARIFAI_API_KEY`.

> Warning: The requests made by integration tests are run against the production system and will use your operations.


```
dotnet test Clarifai.IntegrationTests/Clarifai.IntegrationTests.csproj
```

## License

This project is licensed under the Apache 2.0 License - see the [LICENSE](LICENSE) file for details.
