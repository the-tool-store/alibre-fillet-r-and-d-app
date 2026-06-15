Imports AlibreX
Imports System.Collections.Specialized.BitVector32
Imports System.ComponentModel
Imports System.Windows.Forms.VisualStyles
Public Class Form1
    Dim Session As IADSession
    Dim Hook As IAutomationHook
    Dim Root As IADRoot
    Dim SelectTargets As IObjectCollector
    Dim objPartFeatures As IADPartFeatures
    Dim objIADBodies As IADBodies
    Dim objIADBody As IADBody
    Dim objIADFaces As IADFaces
    Dim objPartSession As IADPartSession
    Dim newClear As IObjectCollector
    Dim sketches As IADSketches
    Dim sketches3D As IAD3DSketches
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Hook = Nothing
        Root = Nothing
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Hook = GetObject(, "AlibreX.AutomationHook")
        Root = Hook.Root
        Session = Root.TopmostSession
        objPartSession = Session
        AAA()
        BBB()
        CCC()
    End Sub
    Sub AAA()
        objPartFeatures = objPartSession.Features
        objIADBodies = objPartSession.Bodies
        objIADBody = objIADBodies.Item(0)
        objIADFaces = objIADBody.Faces
        SelectTargets = Root.NewObjectCollector
        TreeView1.Nodes.Add(objPartSession.Name)
        For i = 0 To objIADFaces.Count - 1
            ListBox1.Items.Add("Face/Surface Selected : " & i)
        Next
        MsgBox(ListBox1.Items.Count)
    End Sub
    Sub BBB()
        objPartFeatures = objPartSession.Features
        sketches = objPartSession.Sketches
        TreeView2.Nodes.Add(objPartSession.Name)
        For Each item As IADSketch In sketches
            ListBox3.Items.Add(item.Name)
        Next
    End Sub
    Sub CCC()
        objPartFeatures = objPartSession.Features
        sketches3D = objPartSession.Sketches3D
        TreeView2.Nodes.Add(objPartSession.Name)
        For Each item As IAD3DSketch In sketches3D
            ListBox4.Items.Add(item.Name)
        Next
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TreeView1.Nodes.Clear()
        TreeView1.Nodes.Add(objPartSession.Name)
        newClear = Root.NewObjectCollector
        SelectTargets.Clear()
        ListBox2.Items.Clear()
        For Each objEdge As IADEdge In objIADFaces.Item(ListBox1.SelectedIndex).Edges
            AddEdgeNode(objEdge)
            ListBox2.Items.Add("Edge StartVertex : " & objEdge.StartVertex.Point.X & " , " & objEdge.StartVertex.Point.Y & " , " & objEdge.StartVertex.Point.Z _
                               & " Edge EndVertex : " & objEdge.EndVertex.Point.X & " , " & objEdge.EndVertex.Point.Y & " , " & objEdge.EndVertex.Point.Z _
                               & " Edge CurveType : " & GetCurveTypeString(objEdge.Geometry.CurveType)
                               )
            Dim curve As IADCurve = CType(objEdge.Geometry, IADCurve)
            ListBox2.Items.Add(curve.PointAtParam(0).X)
            ListBox2.Items.Add(curve.PointAtParam(0).Y)
            ListBox2.Items.Add(curve.PointAtParam(0).Z)
            ListBox2.Items.Add(curve.PointAtParam(1).X)
            ListBox2.Items.Add(curve.PointAtParam(1).Y)
            ListBox2.Items.Add(curve.PointAtParam(1).Z)
            ListBox2.Items.Add(curve.Type)
            ListBox2.Items.Add(curve.CurveType)
        Next
        PrintEdgeData()
        SelectTargets.Add(objIADFaces.Item(ListBox1.SelectedIndex))
        Session.Select(SelectTargets)
    End Sub
    Sub PrintEdgeData()
        ListBox9.Items.Clear()
        For Each objEdge As IADEdge In objIADFaces.Item(ListBox1.SelectedIndex).Edges
            ListBox9.Items.Add(" Edge CurveType : " & GetCurveTypeString(objEdge.Geometry.CurveType))
            Select Case objEdge.Geometry.CurveType
                Case ADGeometryType.AD_LINE
                    Dim a As IADLine = CType(objEdge.Geometry, IADLine)
                    ListBox9.Items.Add("IsClosed : " & a.IsClosed)
                    ListBox9.Items.Add("StartPoint.X : " & RoundToDigits(a.StartPoint.X))
                    ListBox9.Items.Add("StartPoint.Y : " & RoundToDigits(a.StartPoint.Y))
                    ListBox9.Items.Add("StartPoint.Z : " & RoundToDigits(a.StartPoint.Z))
                    ListBox9.Items.Add("PointAtParam0.X : " & a.PointAtParam(0).X)
                    ListBox9.Items.Add("PointAtParam0.Y : " & a.PointAtParam(0).Y)
                    ListBox9.Items.Add("PointAtParam0.Z : " & a.PointAtParam(0).Z)
                    ListBox9.Items.Add("PointAtParam1.X : " & a.PointAtParam(1).X)
                    ListBox9.Items.Add("PointAtParam1.Y : " & a.PointAtParam(1).Y)
                    ListBox9.Items.Add("PointAtParam1.Z : " & a.PointAtParam(1).Z)
                    ListBox9.Items.Add("****************************")
                    ListBox9.Items.Add("ParamAtPoint : " & a.ParamAtPoint(a.StartPoint))
                    ListBox9.Items.Add("****************************")
                    ListBox9.Items.Add("DirectionVector.X : " & RoundToDigits(a.DirectionVector.X))
                    ListBox9.Items.Add("DirectionVector.Y : " & RoundToDigits(a.DirectionVector.Y))
                    ListBox9.Items.Add("DirectionVector.Z : " & RoundToDigits(a.DirectionVector.Z))
                    ListBox9.Items.Add("DirectionVector.Length : " & RoundToDigits(a.DirectionVector.Length))
                Case ADGeometryType.AD_CIRCLE
                    Dim a As IADCircle = CType(objEdge.Geometry, IADCircle)
                    ListBox9.Items.Add("IsClosed : " & a.IsClosed)
                    ListBox9.Items.Add("Radius : " & a.Radius)
                    ListBox9.Items.Add("Axis.X : " & a.Axis.X)
                    ListBox9.Items.Add("Axis.Y : " & a.Axis.Y)
                    ListBox9.Items.Add("Axis.Z : " & a.Axis.Z)
                    ListBox9.Items.Add("Axis.Length : " & a.Axis.Length)
                    ListBox9.Items.Add("Center.X : " & a.Center.X)
                    ListBox9.Items.Add("Center.Y : " & a.Center.Y)
                    ListBox9.Items.Add("Center.Z : " & a.Center.Z)
                Case ADGeometryType.AD_ELLIPSE
                    Dim a As IADEllipse = CType(objEdge.Geometry, IADEllipse)
                    ListBox9.Items.Add("IsClosed : " & a.IsClosed)
                    ListBox9.Items.Add("MajorAxis : " & a.MajorAxis)
                    ListBox9.Items.Add("MinorMajorRatio : " & a.MinorMajorRatio)
                    ListBox9.Items.Add("Axis.X : " & a.Axis.X)
                    ListBox9.Items.Add("Axis.Y : " & a.Axis.Y)
                    ListBox9.Items.Add("Axis.Z : " & a.Axis.Z)
                    ListBox9.Items.Add("Axis.Length : " & a.Axis.Length)
                    ListBox9.Items.Add("Center.X : " & a.Center.X)
                    ListBox9.Items.Add("Center.Y : " & a.Center.Y)
                    ListBox9.Items.Add("Center.Z : " & a.Center.Z)
                Case ADGeometryType.AD_BSPLINE
                    Dim a As IADBsplineCurve = CType(objEdge.Geometry, IADBsplineCurve)
                    ListBox9.Items.Add("IsClosed : " & a.IsClosed)
                Case ADGeometryType.AD_CIRCULAR_ARC
                    Dim a As IADCircularArc = CType(objEdge.Geometry, IADCircularArc)
                    ListBox9.Items.Add("IsClosed : " & a.IsClosed)
                    ListBox9.Items.Add("Radius : " & a.Radius)
                    ListBox9.Items.Add("Axis.X : " & a.Axis.X)
                    ListBox9.Items.Add("Axis.Y : " & a.Axis.Y)
                    ListBox9.Items.Add("Axis.Z : " & a.Axis.Z)
                    ListBox9.Items.Add("Axis.Length : " & a.Axis.Length)
                    ListBox9.Items.Add("Start.X : " & a.Start.X)
                    ListBox9.Items.Add("Start.Y : " & a.Start.Y)
                    ListBox9.Items.Add("Start.Z : " & a.Start.Z)
                    ListBox9.Items.Add("Center.X : " & a.Center.X)
                    ListBox9.Items.Add("Center.Y : " & a.Center.Y)
                    ListBox9.Items.Add("Center.Z : " & a.Center.Z)
                    ListBox9.Items.Add("End.X : " & a.End.X)
                    ListBox9.Items.Add("End.Y : " & a.End.Y)
                    ListBox9.Items.Add("End.Z : " & a.End.Z)
                Case ADGeometryType.AD_ELLIPTICAL_ARC
                    Dim a As IADEllipticalArc = CType(objEdge.Geometry, IADEllipticalArc)
                    ListBox9.Items.Add("Major Radius : " & a.Axis.Length)
                    ListBox9.Items.Add("Minor Radius : " & a.Center.X)
                    ListBox9.Items.Add("Start Angle : " & a.Start.X)
                    ListBox9.Items.Add("End Angle : " & a.MajorAxis)
                Case ADGeometryType.AD_POINT
                    Dim a As IADPoint = CType(objEdge.Geometry, IADPoint)
                    ListBox9.Items.Add("Point.X : " & a.X)
                    ListBox9.Items.Add("Point.Y : " & a.Y)
                    ListBox9.Items.Add("Point.Z : " & a.Z)
            End Select
        Next
    End Sub
    Private Sub ListBox3_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedValueChanged
        ListBox5.Items.Clear()
        For Each item As IADSketchFigure In sketches.Item(ListBox3.SelectedIndex).Figures
            ListBox5.Items.Add(GetCurveTypeString(item.FigureType))
        Next
    End Sub
    Private Sub ListBox4_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox4.SelectedValueChanged
        ListBox6.Items.Clear()
        For Each item As IAD3DSketchFigure In sketches3D.Item(ListBox4.SelectedIndex).Figures
            ListBox6.Items.Add(GetCurveTypeString(item.FigureType))
        Next
    End Sub
    Private Sub ListBox5_SelectedValueChanged(sender As Object, e As EventArgs) Handles ListBox5.SelectedValueChanged
        ListBox7.Items.Clear()
        For Each item As IADSketchFigure In sketches.Item(ListBox3.SelectedIndex).Figures
            ListBox7.Items.Add("Figure Type : " & GetCurveTypeString(item.FigureType))
            Select Case item.FigureType
                Case ADGeometryType.AD_LINE
                    Dim a As IADSketchLine = CType(item, IADSketchLine)
                    Dim objStartPoint As AlibreX.IADSketchPoint
                    objStartPoint = a.Start
                    Dim objEndPoint As AlibreX.IADSketchPoint
                    objEndPoint = a.End
                    Dim dblLength As Double
                    dblLength = a.Length
                    ListBox7.Items.Add(dblLength)
                Case ADGeometryType.AD_BSPLINE
                Case ADGeometryType.AD_CIRCLE
                Case ADGeometryType.AD_ELLIPSE
                Case ADGeometryType.AD_ELLIPTICAL_ARC
                Case ADGeometryType.AD_CIRCULAR_ARC
                Case ADGeometryType.AD_POINT
            End Select
        Next
    End Sub
    Private Sub AddEdgeNode(ByVal edge As IADEdge)
        Dim topologyTypeString As String = GetTopologyTypeString(edge.TopologyType)
        Dim curveTypeString As String = GetCurveTypeString(edge.Geometry.CurveType)
        Dim topologyNode As TreeNode = TreeView1.Nodes(0).Nodes.Add("TopologyType: " & topologyTypeString)
        topologyNode.Nodes.Add(curveTypeString)
    End Sub
    Private Function GetTopologyTypeString(ByVal topologyType As Integer) As String
        Select Case topologyType
            Case 0
                Return "AD_BODY"
            Case 1
                Return "AD_LUMP"
            Case 2
                Return "AD_SHELL"
            Case 3
                Return "AD_FACE"
            Case 4
                Return "AD_LOOP"
            Case 5
                Return "AD_COEDGE"
            Case 6
                Return "AD_EDGE"
            Case 7
                Return "AD_VERTEX"
            Case Else
                Return "Unknown"
        End Select
    End Function
    Private Function GetCurveTypeString(ByVal curveType As Integer) As String
        Select Case curveType
            Case 0
                Return "AD_LINE"
            Case 1
                Return "AD_CIRCLE"
            Case 2
                Return "AD_ELLIPSE"
            Case 3
                Return "AD_BSPLINE"
            Case 5
                Return "AD_CIRCULAR_ARC"
            Case 6
                Return "AD_ELLIPTICAL_ARC"
            Case 7
                Return "AD_PLANE"
            Case 8
                Return "AD_CYLINDER"
            Case 9
                Return "AD_CONE"
            Case 10
                Return "AD_SPHERE"
            Case 11
                Return "AD_TORUS"
            Case 12
                Return "AD_POINT"
            Case 13
                Return "AD_BSURF"
            Case 14
                Return "AD_SHAPEPATTERN"
            Case 15
                Return "AD_SKETCHTEXT"
            Case Else
                Return "Unknown"
        End Select
    End Function
    Function DegreesToRadians(degrees As Double) As Double
        Return degrees * Math.PI / 180.0
    End Function
    Public Function RoundToDigits(ByVal input As Double) As Double
        Return Math.Round(input, 4)
    End Function
    Public Function RoundToDigits2b(ByVal input As Double) As Double
        Return Math.Round(input, 2)
    End Function
    Public Function RoundToDigits4(ByVal input As Double) As Double
        Return Math.Round(input, 4)
    End Function
End Class
