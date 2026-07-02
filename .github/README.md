# Alibre Fillet R&D

A set of experimental VB.NET apps for inspecting Alibre Design fillet and edge geometry through the AlibreX automation API.

These apps attach to a running Alibre Design part session, walk its bodies, faces, and edges, and report the underlying curve geometry. They also scaffold variable-radius fillet creation for prototyping. This is exploratory R&D code: the console project's fillet call is commented out, and the WPF front end is a bare window shell. It targets Alibre Design 29.0.0.29060 with the `AlibreX` automation library, built as x64 executables against .NET Framework 4.8.1 in VB.NET.

## Table Of Contents

- [What Is Here](#what-is-here)
- [Official Alibre Resources](#official-alibre-resources)
- [Requirements](#requirements)
- [Quick Start](#quick-start)
- [Installation](#installation)
- [Usage](#usage)
- [Key Files](#key-files)
- [Key Folders](#key-folders)
- [Notes](#notes)
- [License](#license)

## What Is Here

- Three front ends over the same AlibreX workflow: a console module (`0001`), a WPF window (`0002`), and a Windows Forms inspector (`0003`).
- Attachment to the active Alibre Design session through the `AlibreX.AutomationHook` automation object.
- Enumeration of a part's bodies, faces, and edges, with selection of those objects in the live session.
- Edge curve geometry reads covering lines, circles, arcs, ellipses, B-splines, and points, reporting properties such as start and end points, radius, axis, center, and direction vectors.
- Listing of 2D and 3D sketches with their figure types.
- Scaffolded variable-radius fillet creation via `AddVariableRadiusFilletFeature`, present but commented out in the console project.
- Sample Alibre part files for testing full-round fillet geometry.

## Official Alibre Resources

Alibre's official resources for API development and AI/LLM/agent workflows: <https://www.alibre.com/api/>

## Requirements

- Alibre Design with the `AlibreX` automation library (developed against `AlibreX.dll` from Alibre Design 29.0.0.29060).
- Windows with the .NET Framework 4.8.1 runtime (`net481`), x64.
- Visual Studio 2022 to build the solution.

## Quick Start

1. Open Alibre Design and open a part.
2. Build `source/Fillet_R&D.sln` in Visual Studio 2022.
3. Run the `0003` Windows Forms app to inspect the active part's faces, edges, and sketches.

## Installation

1. Clone or download the repository.
2. Open `source/Fillet_R&D.sln` in Visual Studio 2022.
3. Confirm the `AlibreX` reference `HintPath` in each `.vbproj` points to the `AlibreX.dll` in your Alibre Design installation, then build.

The build produces standalone executables. There is no add-on (`.adc`) manifest or installer; the apps run externally and connect to an already-open Alibre Design instance.

## Usage

1. Launch Alibre Design and open a part that contains the geometry you want to inspect.
2. Run one of the built executables:
   - `0001` (console): adds the active part's face edges to a selection set; the variable-radius fillet code is present but commented out for experimentation.
   - `0002` (WPF): a minimal window shell.
   - `0003` (Windows Forms): the inspector that lists faces, edges, and sketches and shows per-edge curve geometry as you make selections.
3. Each app reads the topmost (active) session, so the target part must be open and active in Alibre Design before launch.

## Key Files

| File | Purpose |
| --- | --- |
| `source/Fillet_R&D.sln` | Visual Studio solution grouping the three projects. |
| `source/0001/Program.vb` | Console entry point; selects face edges and scaffolds `AddVariableRadiusFilletFeature`. |
| `source/0001/0001.vbproj` | Console project targeting `net481`. |
| `source/0002/MainWindow.xaml` | WPF main window markup. |
| `source/0002/MainWindow.xaml.vb` | WPF window code-behind (empty shell). |
| `source/0002/0002.vbproj` | WPF project targeting `net481`. |
| `source/0003/Form1.vb` | Windows Forms inspector logic for faces, edges, sketches, and per-edge curve geometry. |
| `source/0003/Form1.Designer.vb` | Generated layout for the inspector form. |
| `source/0003/0003.vbproj` | Windows Forms project targeting `net481`. |
| `source/alibre.disclaimer.txt` | MIT license note and Alibre trademark statement. |
| `source/test-full-round-fillet-geometry.AD_PRT` | Sample Alibre part for testing fillet geometry. |

## Key Folders

| Folder | Purpose |
| --- | --- |
| `source/` | Solution, the three VB.NET projects, and sample part files. |
| `source/0001/` | Console project. |
| `source/0002/` | WPF project. |
| `source/0003/` | Windows Forms inspector project. |
| `documentation/` | Placeholder for documentation (currently empty). |
| `submodules/` | Placeholder for git submodules (currently empty). |
| `reviews/` | Code review notes. |
| `.github/` | Repository docs, issue templates, and pull request template. |

## Notes

- The apps require an already-open, active Alibre Design part session; they attach to the topmost session rather than opening files themselves.
- The console project's variable-radius fillet call and the WPF window are experimental stubs, not finished features.
- Verify each project's `AlibreX` reference path against your installed Alibre Design version before building.

## License

See [LICENSE](../LICENSE). Per `source/alibre.disclaimer.txt`, the project is licensed under the MIT License unless noted otherwise; Alibre, Alibre Design, and Alibre Script content and branding remain the property of Alibre, LLC.
