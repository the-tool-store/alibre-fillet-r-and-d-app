# Fillet R&D

> Note: This repository is undergoing significant changes and is currently a work in progress.

| Item | Value |
| --- | --- |
| Type | R&D desktop application |
| Primary stack | VB.NET, XAML, Alibre automation |

## Overview
This repository contains the source and supporting assets for Fillet R&D, organized under the standardized repository layout.

## Repository Layout
- source/: project source, solution or project files, and runtime assets.
- submodules/: external git submodules used by the repository when required.
- documentation/: supplementary notes, changelogs, and non-GitHub documentation.
- .github/: repository README, templates, and GitHub-specific community files.
- `source/Fillet_R&D.sln`: key source or build entry point.
- `source/0001/0001.vbproj`: key source or build entry point.
- `source/0002/0002.vbproj`: key source or build entry point.
- `source/0003/0003.vbproj`: key source or build entry point.

## Requirements
- Windows development environment.
- A .NET build environment compatible with the projects under source/.
- Alibre Design installed if you need to run, debug, or validate the Alibre integration.

## Build and Use
1. Open `source/Fillet_R&D.sln` in your preferred IDE.
2. Restore dependencies and build from the source/ layout.
3. Use the notes in documentation/ and .github/README.md as the primary repository guide.

## Current Limitations
- The repository has been normalized for layout consistency; any path-sensitive tooling should be revalidated against the new folder structure.
- Existing runtime behavior and project-specific limitations remain unchanged.

## License
See [LICENSE](../LICENSE).

