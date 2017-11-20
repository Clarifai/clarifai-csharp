# Clarifai API C# Client

[![NuGet](https://img.shields.io/nuget/v/Clarifai.svg)](https://www.nuget.org/packages/Clarifai)
[![Build Status](https://travis-ci.org/Clarifai/clarifai-csharp.svg?branch=master)](https://travis-ci.org/Clarifai/clarifai-csharp)
[![Build status](https://ci.appveyor.com/api/projects/status/osiexiua9ig1w3as/branch/master?svg=true)](https://ci.appveyor.com/project/robertwenquan/clarifai-csharp-1dm15/branch/master)

## Getting Started

These instructions will show you how to use the Clarifai API in C#.

First and foremost, you need an instance of `ClarifaiClient`.

```cs
var client = new ClarifaiClient("YOUR API KEY").
```

Using `client` you can access all the available API functionality.

One of the methods the client implements is `Predict`. If you want recognize concepts on a certain image, do the following:

```cs
var response = await Client.Predict<Concept>("SOME CONCEPT MODEL ID", new ClarifaiURLImage("IMAGE URL"))
    .ExecuteAsync();
```

There are several public models that have been pretrained that you can use. Go to the [public models page](https://www.clarifai.com/models), find their model ID (located on each model page has its ID listed), and plug it in the code above. Or even better, use the `PublicModels` and use `Predict` on the model directly, like this:

```cs
ClarifaiResponse<List<ClarifaiOutput<Concept>>> response =
    await Client.PublicModels.GeneralModel.Predict(
        new ClarifaiURLImage("IMAGE URL"))
    .ExecuteAsync();
```

If you want to use your own custom model, you'll have to pass the model ID directly, or use `client.SearchModels("my-model-name")` to get the model instance (you have to train the custom model before using it, please see more [in the Train section of the Developer's Guide](https://clarifai.com/developer/guide/train#train)).

After the prediction process has been finished and the response has arrived, the returned object `ClarifaiResponse` will contain a list of `ConceptOutput` objects - in this case, there'll be only one instance in the list since we specified one image - and concepts that were recognized on the image (plus some other metadata).

The following code will take the first (and only) `ClarifaiOutput` and print all the predicted concept IDs, along with prediction confidences:

```cs
foreach (Concept concept in response.Get()[0].Data) {
    Console.WriteLine(concept.ID + " " + concept.Value);
}

```

### Synchronous/asynchronous requests

The client utilizes the C# async/await functionality. If wanting to use the library in a simple synchronous way, see the previous example. Using the client asynchronously is easy as well. Simply drop the `async` before the request call and do it at some later point.

```cs
var responseTask = Client.Predict<Concept>("SOME CONCEPT MODEL ID", new ClarifaiURLImage("IMAGE URL"))
    .ExecuteAsync();

Console.WriteLine("This line will (most likely) be executed before the arrival of the response.");

var response = await responseTask;

Console.WriteLine("This line will be executed after the arrival of the response.");
// Now you can get the response content with response.Get() as with the previous example.
```

### Generics and casting

The library uses generics to specify the type of an output. In the first example using the `Predict` method, we use `Concept` indicate that the model ID is a concept model. If we'd use a model ID of our public color model, we'd use `Color` instead of `Concept`.

Not all methods can however use generics. For example, the `GetModels` is able to return multiple models and you cannot determine in advance (at compile time) the types of model that will be returned. The request therefore returns a list of `IModel` objects. You can cast these instances to specific models, if you know its type.

```cs
if (models[0].OutputInfo.TypeExt == "concept")
{
	ConceptModel model = (ConceptModel) models[0];
}
```

In a similar way you may cast `IPrediction` when the actual class type is not known at compile time.

Please see more in the [Developer's Guide](https://clarifai.com/developer/guide/).

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

This library supports .NET Standard 1.3 which means it can be used in the following .NET implementations:

- .NET Framework 4.6+
- .NET Core 1.0+
- Mono 4.6+
- Xamarin.iOS 10.0+
- Xamarin.Mac 3.0+
- Xamarin.Andoid 7.0+
- UWP 10.0+

The library can be used on any platform supporting one of the above libraries.

## Running the tests

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


