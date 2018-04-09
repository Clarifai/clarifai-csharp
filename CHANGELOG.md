# CHANGELOG

## [[1.1.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.1.0) - [NuGet](https://www.nuget.org/packages/Clarifai/1.1.0) - 2018-04-09

### Added
- Support for custom face recognition

## [[1.0.1]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.0.1) - [NuGet](https://www.nuget.org/packages/Clarifai/1.0.1) - 2018-02-22

### Removed
- Dependency Microsoft.Extensions.PlatformAbstractions

## [[1.0.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.0.0) - [NuGet](https://www.nuget.org/packages/Clarifai/1.0.0) - 2018-01-26

### Added
- Automatic pulling of api key from env var https://github.com/Clarifai/clarifai-csharp/commit/8b9b7a5482bffe36bf8aeac61baadf8f30bf02fc

## [[0.5.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/0.5.0) - [NuGet](https://www.nuget.org/packages/Clarifai/0.5.0) - 2017-12-08

### Added
- WorkflowBatchPredictRequest for a list of inputs

### Changed
- Renamed BatchPredict to Predict. Method is now overloaded for a list
- WorkflowPredictRequest changed to support a single input

## [[0.4.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/0.4.0) - [NuGet](https://www.nuget.org/packages/Clarifai/0.4.0) - 2017-12-04

### Added
- New PredictRequest for single prediction
- Searching by image

### Changed
- Renamed PredictRequest to BatchPredictRequest
- Pagination was made to work for all parameter combinations

### Fixed
- Geopoint longitude and latitude being reversed
