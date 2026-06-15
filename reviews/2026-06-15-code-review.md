# Code Review — alibre-fillet-r-and-d-app

- **Date:** 2026-06-15
- **Branch:** `review/2026-06-15-code-review` (branched from `the-tool-store` @ `55c8b06` — "cleanup push")
- **Reviewer:** Claude (Opus 4.8)
- **Scope:** Full repository review (three VB.NET projects under `source/`, solution file, GitHub community files, committed CAD artifacts)

---

## 1. Summary

This repo is an **R&D / scratchpad** for experimenting with Alibre Design's `AlibreX` COM
automation API around fillet geometry. It is a Visual Studio solution (`Fillet_R&D.sln`) containing
three VB.NET projects, named only by number:

- **`0001`** — a console-style `Exe` (`Sub Main` in `Program.vb`) that connects to a running
  Alibre instance and *would* add a variable-radius fillet feature — except the entire feature
  call and its setup are commented out. What remains just iterates the edges of one face and
  selects them.
- **`0002`** — a WPF app. It is empty scaffolding: a default `MainWindow`, no Alibre code, no logic.
  Pure dead weight.
- **`0003`** — a WinForms diagnostic UI (`Form1`) that walks the topmost part's faces / sketches /
  3D sketches and dumps edge and curve data into a wall of list boxes. This is the most developed
  project and is clearly a manual inspection tool.

This is **prototype/experimental code, not a shippable product** — and it does not pretend
otherwise (the README says "work in progress"). Judged as R&D, much of it is acceptable. But there
are real defects worth flagging: the projects are **hard-pinned to one exact Alibre version**, all
three will **throw immediately if Alibre isn't running or no part is open** (no error handling at
all), the headline "add fillet" logic is **entirely commented out**, project `0002` is **dead
scaffolding**, large **binary `.AD_PRT` CAD files are committed** to source control, and the README
links a **`LICENSE` file that does not exist**.

There is no installer, no CI workflow, and no `.csproj` packaging logic in this repo (unlike the
sibling `alibre-shapes-addon`), so the "broken build pipeline" class of findings does not apply
here. Findings are correspondingly fewer.

**Overall:** A working experimental harness with no packaging story. Fine as an internal R&D
sketch; needs the Critical/High items addressed before it could be relied on or handed to anyone else.

### Findings by severity

| Severity | Count |
|----------|-------|
| Critical | 1 |
| High     | 3 |
| Medium   | 4 |
| Low / Nit| 6 |

---

## 2. Critical

### C-1. The "add fillet" feature — the entire point of the app — is commented out
**File:** [Program.vb:31-57](source/0001/Program.vb)

`0001` is named for adding a variable-radius fillet, and prints
`"**********Add Variable Radius Fillet feature******************"`. But every line that actually
*creates* the fillet, populates the radius arrays, and reads the result back is commented out:

```vb
'objVarFillet = objPartFeatures.AddVariableRadiusFilletFeature(objFilletTargets, startRad, endRad, True, "var Radius fillet from API")
'startRadCollection = objVarFillet.StartRadiusParams
'objStartRad = startRadCollection.Item(0)
...
'Debug.Print("The fillet's start radius is " & objStartRad.Value)
```

What is left executing only loops over `objIADFaces.Item(1).Edges`, prints `IsSenseReversed`, and
selects the edges. The program does **nothing it claims to do**. Either restore and finish the
fillet call, or rename/repurpose the project so its name and banner are honest. As-is, anyone
running `0001` expecting a fillet gets a no-op.

---

## 3. High

### H-1. Hard-coded, version-pinned `AlibreX.dll` path in all three projects
**Files:** [0001.vbproj:17](source/0001/0001.vbproj), [0002.vbproj:27](source/0002/0002.vbproj), [0003.vbproj:23](source/0003/0003.vbproj)

```xml
<HintPath>C:\Program Files\Alibre Design 27.0.1.27039\Program\AlibreX.dll</HintPath>
```

All three `.vbproj` files reference the exact build `27.0.1.27039`. (Note: the sibling
`alibre-shapes-addon` was pinned to `28.1.1.28227` — so even within this repo family the pin is
inconsistent and already stale.) This breaks on any other Alibre version, any non-default install
location, and any machine where Program Files is localized/relocated. Resolve the install root from
the registry key the installer writes (`SOFTWARE\Alibre, LLC\Alibre Design`) or an MSBuild
property/env var, and keep the hard-coded path only as a last-resort fallback.

### H-2. No error handling: every entry point throws if Alibre isn't running or no part is open
**Files:** [Program.vb:9-29](source/0001/Program.vb), [Form1.vb:22-42](source/0003/Form1.vb)

Both `0001` and `0003` start with:

```vb
Hook = GetObject(, "AlibreX.AutomationHook")   ' throws if Alibre not running
Root = Hook.Root
Session = Root.TopmostSession                  ' Nothing if no document open
objPartSession = Session                        ' late-bound cast; throws if not a part
objIADBody = objIADBodies.Item(0)               ' throws if the part has no bodies
```

There is no `Try/Catch` anywhere. If Alibre isn't running, `GetObject` raises a COM exception and
the process dies. If the topmost session is an assembly/drawing rather than a part, the late-bound
assignment fails. If the part has zero bodies, `Item(0)` throws. For `0003` this happens inside
`Form1_Load`, so the form never even displays — the user just sees a crash. Wrap acquisition in
`Try/Catch` and show a clear "Open a part in Alibre first" message instead of an unhandled
exception.

### H-3. Committed binary CAD artifacts (`.AD_PRT`) in source control
**Files:** `source/test-full-round-fillet-geometry.AD_PRT` (~343 KB), `source/test-full-round-fillet-geometry-0.AD_PRT` (~282 KB)

`git ls-files` shows two binary Alibre part files tracked in `source/`. They are opaque binaries
(~625 KB combined) that bloat history and can never be meaningfully diffed or merged. The
`.gitignore` does **not** cover `*.AD_PRT` ([.gitignore](.gitignore) ignores `[Bb]in/` and
`[Oo]bj/` but no part files). If these are needed as test fixtures, move them to a clearly named
`fixtures/`/`test-data/` folder and document them; otherwise `git rm --cached` them and add
`*.AD_PRT` to `.gitignore`.

---

## 4. Medium

### M-1. Project `0002` is entirely dead scaffolding
**Files:** [MainWindow.xaml.vb:1-2](source/0002/MainWindow.xaml.vb), [MainWindow.xaml:9-10](source/0002/MainWindow.xaml), [Application.xaml.vb:1-4](source/0002/Application.xaml.vb)

`0002` is a default WPF template: an empty `MainWindow` (`<Grid></Grid>`), an empty `MainWindow`
class, empty application events, and an `AlibreX` reference that nothing uses. It contributes
nothing and adds a third project to build, a third stale `AlibreX` pin, and confusion about which
project matters. Delete it from the solution (and disk), or actually build something in it.

### M-2. `GetCurveTypeString` is reused to label `FigureType`, which is a different enum
**File:** [Form1.vb:171,177,183](source/0003/Form1.vb)

`GetCurveTypeString` ([Form1.vb:233-268](source/0003/Form1.vb)) maps `IADCurve.CurveType`
(`ADGeometryType`) values. It is then reused to stringify a **sketch figure's** `FigureType`:

```vb
ListBox5.Items.Add(GetCurveTypeString(item.FigureType))
...
ListBox7.Items.Add("Figure Type : " & GetCurveTypeString(item.FigureType))
```

`FigureType` for sketch figures is not guaranteed to use the same numeric encoding as
`ADGeometryType`, so the displayed labels may be wrong or "Unknown". Since this is a diagnostic
tool whose whole job is to report types accurately, mislabeling defeats the purpose. Use the
correct enum-to-string mapping for figure types (or at least print the raw numeric value alongside).

### M-3. No `Option Strict` / `Option Explicit` — pervasive late binding
**Files:** all `.vbproj` (none set `<OptionStrict>On</OptionStrict>`); e.g. [Program.vb:25](source/0001/Program.vb), [Form1.vb:26](source/0003/Form1.vb)

Assignments like `objPartSession = Session` and `objPartSession = ... .Bodies` rely on implicit
late-bound conversions across COM interfaces with no compile-time checking. Combined with the
zero error handling (H-2), type mismatches that could be caught at build time instead surface as
runtime crashes. Turn on `Option Strict On` (at minimum `Option Explicit On`) and fix the resulting
diagnostics; for COM interop you'll add explicit `CType`/`DirectCast` calls, which document intent.

### M-4. README links a `LICENSE` file that doesn't exist
**File:** [.github/README.md:38](.github/README.md)

```markdown
See [LICENSE](../LICENSE).
```

There is no `LICENSE` at the repo root (`git ls-files` shows none; only
`source/alibre.disclaimer.txt` mentions MIT). The link is dead. The disclaimer says "Everything is
licensed under the MIT License" — so add an actual root `LICENSE` file (MIT text) to back that
claim and fix the link, or point the link at `source/alibre.disclaimer.txt`.

---

## 5. Low / Nits

### L-1. Debug `MsgBox` left in `Form1_Load`
[Form1.vb:41](source/0003/Form1.vb): `MsgBox(ListBox1.Items.Count)` fires a modal dialog on every
form load. Leftover debugging — remove it.

### L-2. Meaningless project names `0001` / `0002` / `0003`
[Fillet_R&D.sln:5-10](source/Fillet_R&D.sln) and the matching folders. Numeric names convey nothing
about each project's purpose (console fillet experiment / empty WPF shell / WinForms inspector).
Rename to something descriptive (e.g. `FilletConsole`, `GeometryInspector`) — this also fixes the
auto-generated `RootNamespace` values `_0001`/`_0002`/`_0003`.

### L-3. Unused / placeholder imports and references
- [Form1.vb:2](source/0003/Form1.vb): `Imports System.Collections.Specialized.BitVector32` — an
  unusual, almost-certainly-unintended import of a *struct* as if it were a namespace; nothing in
  the file uses it.
- [Form1.vb:4](source/0003/Form1.vb): `Imports System.Windows.Forms.VisualStyles` — unused.
- [0002.vbproj:29](source/0002/0002.vbproj), [0003.vbproj:25](source/0003/0003.vbproj): explicit
  `<Reference Include="mscorlib" />` is redundant under the SDK-style project.

### L-4. Duplicate rounding helpers
[Form1.vb:272-280](source/0003/Form1.vb): `RoundToDigits` and `RoundToDigits4` are identical
(`Math.Round(input, 4)`), and `RoundToDigits2b` rounds to 2. Collapse into one
`RoundToDigits(value, digits)` helper. `DegreesToRadians` ([Form1.vb:269-271](source/0003/Form1.vb))
is defined but never called.

### L-5. Misleading labels in the elliptical-arc branch
[Form1.vb:152-157](source/0003/Form1.vb): the `AD_ELLIPTICAL_ARC` case prints
`"Minor Radius : " & a.Center.X`, `"Start Angle : " & a.Start.X`, `"End Angle : " & a.MajorAxis` —
the labels don't match the properties being read (a center coordinate is not a minor radius, etc.).
For a diagnostic tool this is actively misleading; map each label to the correct property.

### L-6. Empty event handlers and copy-paste list-box plumbing
[Form1.vb:166-167,203-204](source/0003/Form1.vb): `ListBox2_SelectedValueChanged` and
`ListBox6_SelectedValueChanged` are empty stubs. Several handlers re-query
`sketches.Item(ListBox3.SelectedIndex).Figures` independently (lines 170, 182), so selection state
across the boxes can drift out of sync. Remove dead handlers and centralize the figure lookup.

---

## 6. What looks good

- The COM acquisition pattern (`GetObject` → `Root` → `TopmostSession` → `Features`/`Bodies`/
  `Faces`) is the idiomatic AlibreX entry sequence, and the lifecycle nulls out `Hook`/`Root` on
  unload ([Program.vb:59-60](source/0001/Program.vb), [Form1.vb:18-21](source/0003/Form1.vb)).
- `0003`'s curve/geometry dispatch (`Select Case objEdge.Geometry.CurveType` over the full
  `ADGeometryType` set, [Form1.vb:89-163](source/0003/Form1.vb)) is a genuinely useful reference
  for what each AlibreX geometry interface exposes — good as an API exploration tool.
- The repo follows a consistent, documented layout (`source/`, `submodules/`, `documentation/`,
  `.github/`) and ships the standard community files (CODE_OF_CONDUCT, CONTRIBUTING, issue/PR
  templates).
- `.gitignore` is comprehensive for the standard VS/.NET `bin`/`obj` artifacts (its main gap is
  the `.AD_PRT` files in H-3).

---

## 7. Recommended fix order

1. **C-1** — restore/finish (or honestly retire) the fillet logic so `0001` does what it claims.
2. **H-2** — wrap COM acquisition in `Try/Catch` so the tools degrade gracefully instead of crashing.
3. **H-1** — unpin `AlibreX.dll` from one exact Alibre build (resolve via registry/env var).
4. **H-3 / M-1** — remove committed `.AD_PRT` binaries (or move to fixtures) and delete the dead
   `0002` project.
5. **M-2 / M-4 / L-5** — fix the incorrect type/label mappings in the inspector and the dead
   `LICENSE` link.
6. **M-3** — enable `Option Strict On` and resolve the late-binding diagnostics.
7. Sweep the remaining **L-*** cleanups (debug `MsgBox`, project renames, unused imports, duplicate
   helpers, empty handlers) when next touching the code.
