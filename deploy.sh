#! /usr/bin/env bash
set -e

rm -rf Clarifai/bin/Release
$HOME/.dotnet/dotnet restore
$HOME/.dotnet/dotnet pack -c Release
$HOME/.dotnet/dotnet nuget push Clarifai/bin/Release/Clarifai.*.nupkg -k $NUGET_API_KEY -s https://www.nuget.org/api/v2/package

