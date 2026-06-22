# alibre-fillet-r-and-d-app — Code Review (Correctness)

**Date:** 2026-06-20
**Scope:** Second-opinion review, code only (correctness bugs). Substantive logic lives only in `0001/Program.vb` and `0003/Form1.vb`; the rest is R&D scratch code with empty boilerplate forms.

**Summary: 3 bugs — 1 High, 2 Medium**

## High

**`source/0003/Form1.vb:152-157`** — The `AD_ELLIPTICAL_ARC` case reads the wrong properties for the values it labels. It outputs `a.Axis.Length` as "Major Radius", `a.Center.X` as "Minor Radius", `a.Start.X` as "Start Angle", and `a.MajorAxis` as "End Angle". `Center.X` is a coordinate (not a minor radius), `Start.X` is a point coordinate (not an angle), and `Axis.Length`/`MajorAxis` are not the major/end values claimed. Compare with the `AD_ELLIPSE` case (lines 120-131), which correctly exposes `MajorAxis`, `MinorMajorRatio`, `Center`, and `Axis` separately. The elliptical-arc branch therefore displays incorrect geometric data — a real correctness defect for a tool whose purpose is reporting geometry.

## Medium

**`source/0003/Form1.vb:65, 82, 87`** — `ListBox1_SelectedIndexChanged` unconditionally calls `objIADFaces.Item(ListBox1.SelectedIndex)` (and again at lines 82 and 87 via `PrintEdgeData`). When `SelectedIndex` is `-1` (no item selected — which the framework also raises after the list is cleared/repopulated), `Item(-1)` will throw an out-of-range/COM error. There is no guard for the `-1` case and no exception handling anywhere in the class, so this surfaces as an unhandled exception.

**`source/0003/Form1.vb:180-202`** — `ListBox5_SelectedValueChanged` (the handler for selecting an individual figure) ignores `ListBox5.SelectedIndex` entirely and instead iterates over *all* figures of the sketch chosen in `ListBox3` (`For Each item ... In sketches.Item(ListBox3.SelectedIndex).Figures`). Selecting one figure thus does not drill into that figure; it re-lists every figure of the parent sketch. The handler uses the wrong index source for its intended drill-down behavior.

## Notes (lower-confidence / not reported as bugs)

- `0001/Program.vb` lines 28, 35 hardcode `Bodies.Item(0)` / `Faces.Item(1)` and line 39 re-calls `Session.Select` every loop iteration; these are deliberate R&D scaffolding (most of the file is commented out) rather than shipping defects.
- `0002` (`MainWindow.xaml.vb`, `Application.xaml.vb`) and `0003/ApplicationEvents.vb` are empty boilerplate; the designer file `Form1.Designer.vb` is auto-generated. No correctness issues there.

---

## Fixes applied — 2026-06-20

- **[High] `source/0003/Form1.vb`** — the `AD_ELLIPTICAL_ARC` case now reads the correct ellipse geometry (`MajorAxis`, `MinorMajorRatio`, `Axis`, `Center`, mirroring the `AD_ELLIPSE` case) plus the genuine `Start`/`End` point members (labeled accurately, since no `StartAngle`/`EndAngle` property exists in the API).
- **[Medium] `source/0003/Form1.vb`** — `ListBox1_SelectedIndexChanged` and `PrintEdgeData` now early-return when `ListBox1.SelectedIndex < 0`.
- **[Medium] `source/0003/Form1.vb`** — `ListBox5_SelectedValueChanged` now drills into the single selected figure via `Figures.Item(ListBox5.SelectedIndex)` (guarded for `< 0`) instead of re-listing all figures.

*Caveat: changes applied to source; not verified by build.*
