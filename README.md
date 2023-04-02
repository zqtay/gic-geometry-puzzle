# GIC Geometry Puzzle
A simple geometry puzzle that tests if coordinates are inside or outside of a shape.  
This console app is written in C# with .NET 6.0.  

## Setup
Check if .NET SDK 6.0 or later is installed in your environment.  
If not, download from Microsoft here: [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
```
dotnet sdk check
```
Clone the repository:  
```
git clone https://github.com/zqtay/gic-geometry-puzzle.git
```
Go to working directory:  
```
cd gic-geometry-puzzle
```  
Build projects:  
```
dotnet build
```  

## Run
Go to PuzzleApp directory:
```
cd gic-geometry-puzzle\src\PuzzleApp
```
Run app in console:
```
dotnet run
```

## Test
Go to UnitTests directory:
```
cd gic-geometry-puzzle\tests\UnitTests
```
Run tests:
```
dotnet test -v normal
```

## Publish
Go to PuzzleApp directory:
```
cd gic-geometry-puzzle\src\PuzzleApp
```
Build as Windows x64 executable file:
```
dotnet publish --output ./publish --runtime win-x64 --configuration Release -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true
```