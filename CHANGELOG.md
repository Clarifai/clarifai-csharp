# CHANGELOG

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
