# Scale Finder
 Music Scale Finder

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Examples of use](#Examples-of-use)

## General info
This project will find the scale if you put in the root note and the scale type.
	
## Technologies
Project is created with:
* C# (.Net6.0) 
* Visual Studio 2022
	
## Setup

```
```

## Examples of use
```
ScaleFinder finder = new ScaleFinder();
Scale result = finder.FindScale(ScaleFinder.PitchC, ScaleFinder.AccidNatural, ScaleFinder.ModeMajor);
```