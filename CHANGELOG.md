# CHANGELOG

## [[1.4.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.4.0) - [NuGet](https://www.nuget.org/packages/Clarifai/1.4.0) - 2020-02-14

See the API changes for more details on the Face proto deprecation and the facedetect* model type cleanup:
https://docs.clarifai.com/product-updates/upcoming-api-changes

### Added
- DetectionModel

### Removed
- DemographicsModel, LogoModel, FaceConceptsModel, FaceDetectionModel, FocusModel.
- Feedback

## [[1.3.2]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.3.2) - [NuGet](https://www.nuget.org/packages/Clarifai/1.3.2) - 2019-11-23

### Added
- Expose each input's status object
- Getting model by version ID

### Fixed
- Skip unknown model types in GetModelsRequest

## [[1.3.1]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.3.1) - [NuGet](https://www.nuget.org/packages/Clarifai/1.3.1) - 2019-05-30

### Fixed
- Support pagination in search requests

## [[1.3.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.3.0) - [NuGet](https://www.nuget.org/packages/Clarifai/1.3.0) - 2018-12-10

### Added
- Sample milliseconds parameter for video prediction
- Support for workflow image file prediction
- Expose the Model property in the prediction response object

### Fixed
- On error response, set the ErrorDetails property of ClarifaiStatus

## [[1.2.0]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.2.0) - [NuGet](https://www.nuget.org/packages/Clarifai/1.2.0) - 2018-10-19

### Added
- Moderation solution

## [[1.1.3]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.1.3) - [NuGet](https://www.nuget.org/packages/Clarifai/1.1.3) - 2018-07-18

### Fixed
- Searching by input metadata

## [[1.1.2]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.1.2) - [NuGet](https://www.nuget.org/packages/Clarifai/1.1.2) - 2018-07-17

### Fixed
- Predict response deserialization

## [[1.1.1]](https://github.com/Clarifai/clarifai-csharp/releases/tag/1.1.1) - [NuGet](https://www.nuget.org/packages/Clarifai/1.1.1) - 2018-04-23

### Fixed
- Include regions in the ModifyInputRequest

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
