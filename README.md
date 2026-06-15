# Fillet R&D

Face, edge and general topology insight

![image](https://github.com/Testbed-for-Alibre-Design/Fillet_R-D/assets/5302428/a16ef596-5409-4423-8627-7ff12f950a43)

## Missing API's

TwoAdjacentFaces2 Method

Ref: https://help.solidworks.com/2014/english/api/sldworksapi/solidworks.interop.sldworks~solidworks.interop.sldworks.iedge~gettwoadjacentfaces2.html

Complete Edge API

Ref: https://help.solidworks.com/2014/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.IEdge_members.html

Complete Face API

Ref: https://help.solidworks.com/2014/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.IFace2_members.html

Ref:

https://docs.techsoft3d.com/exchange/2024/api/group__a3d__feature__fillet__module.html

https://docs.techsoft3d.com/exchange/latest/api/group__a3d__feature__description__fillet__module.html

https://help.solidworks.com/2014/english/api/sldworksapi/solidworks.interop.sldworks~solidworks.interop.sldworks.ifeaturemanager~featurefillet2.html

https://help.solidworks.com/2014/english/api/swconst/SolidWorks.Interop.swconst~SolidWorks.Interop.swconst.swFeatureFilletType_e.html


**It is well understood that Alibre Design does not have full-round fillet capabilities. My goal was to review another 3D-CAD API, compare its methods, and explore the possibility of creating a full-round fillet by using the existing Alibre APIs.**

## Projects

This repository includes a couple of small test applications used while exploring the Alibre API:

* `0001` – a console based prototype for variable radius fillet experimentation.
* `0003` – a Windows Forms test harness for general API trials.

An unused WPF sample that previously lived in the `0002` directory was removed as it served no purpose.
