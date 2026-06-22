# Alibre Fillet R&D

A research and development workbench for exploring Alibre Design fillet and edge-geometry operations through the AlibreX automation API. It connects to a running Alibre Design part session and inspects faces, edges, and curve geometry as a basis for prototyping variable-radius fillet creation.

## Features
- Attaches to the active Alibre Design session via the `AlibreX.AutomationHook` automation object.
- Enumerates a part's bodies, faces, and edges, and selects them in the live session.
- Reads detailed edge curve geometry (lines, circles, arcs, ellipses, B-splines, points), reporting properties such as start/end points, radius, axis, center, and direction vectors.
- Lists 2D sketches and 3D sketches along with their figure types.
- Prototypes variable-radius fillet creation through `AddVariableRadiusFilletFeature` (scaffolded in the console project).
- Provides three experimental front ends: a console module, a WPF window, and a Windows Forms inspector UI.

## Requirements
- Alibre Design with the `AlibreX` automation library (developed against `AlibreX.dll` from Alibre Design 29.0.0.29060).
- Windows with the .NET Framework 4.8.1 runtime (`net481`), x64.

## Installation
1. Clone or download the repository.
2. Open `source/Fillet_R&D.sln` in Visual Studio 2022.
3. Verify the `AlibreX` reference `HintPath` in each `.vbproj` points to the `AlibreX.dll` in your Alibre Design installation, then build.

This project produces standalone executables. There is no add-on (`.adc`) manifest or installer; the apps run externally and connect to an already-open Alibre Design instance.

## Usage
1. Launch Alibre Design and open a part that contains the geometry you want to inspect.
2. Run one of the built executables:
   - `0001` (console): adds the active part's face edges to a selection set; the variable-radius fillet code is present but commented out for experimentation.
   - `0002` (WPF): a minimal window shell.
   - `0003` (Windows Forms): the main inspector, listing faces, edges, and sketches and displaying per-edge curve geometry as you make selections.
3. The tools read the topmost (active) session, so the target part must be open and active in Alibre Design before launching.

## License
See [LICENSE](../LICENSE). Per `source/alibre.disclaimer.txt`, the project is licensed under the MIT License unless noted otherwise; Alibre, Alibre Design, and Alibre Script content and branding remain the property of Alibre, LLC.
