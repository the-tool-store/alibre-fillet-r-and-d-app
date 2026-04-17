Imports System
Imports AlibreX
Module Program
    Public Session As IADSession
    Public Hook As IAutomationHook
    Public Root As IADRoot
    Sub Main(args As String())
        Debug.Print("**********Add Variable Radius Fillet feature******************")
        Hook = GetObject(, "AlibreX.AutomationHook")
        Root = Hook.Root
        Session = Root.TopmostSession
        Dim objFilletTargets As IObjectCollector
        Dim objPartFeatures As IADPartFeatures
        Dim objVarFillet As IADFilletFeature
        Dim objIADBodies As IADBodies
        Dim objIADBody As IADBody
        Dim objIADFaces As IADFaces
        Dim startRad() As Object
        Dim endRad() As Object
        Dim startRadCollection As IObjectCollector
        Dim endRadCollection As IObjectCollector
        Dim objStartRad As IADParameter
        Dim objEndRad As IADParameter
        Dim objPartSession As IADPartSession
        objPartSession = Session
        objPartFeatures = objPartSession.Features
        objIADBodies = objPartSession.Bodies
        objIADBody = objIADBodies.Item(0)
        objIADFaces = objIADBody.Faces
        objFilletTargets = Root.NewObjectCollector
        ' objFilletTargets.Add(objIADBody.Edges.Item(8))
        'objFilletTargets.Add(objIADBody.Edges.Item(0))
        'objFilletTargets.Add(objIADFaces.Item(1))
        'Debug.Print(objIADFaces.Item(0).Edges.Count)
        For Each objEdge As IADEdge In objIADFaces.Item(1).Edges
            Console.WriteLine(objEdge.IsSenseReversed)
            'objEdge.Geometry.
            objFilletTargets.Add(objEdge)
            Session.Select(objFilletTargets)
        Next
        ' objFilletTargets.Add(objIADFaces.Item(6).Edges.Item(1))
        'ReDim startRad(0 To objFilletTargets.Count - 1)
        'ReDim endRad(0 To objFilletTargets.Count - 1)
        'startRad(0) = 0.1
        'endRad(0) = 0.2
        'startRad(1) = 0.3
        'endRad(1) = 0.4
        'startRad(2) = 0.5
        'endRad(2) = 0.6
        'objVarFillet = objPartFeatures.AddVariableRadiusFilletFeature(objFilletTargets, startRad, endRad, True, "var Radius fillet from API")
        'startRadCollection = objVarFillet.StartRadiusParams
        'objStartRad = startRadCollection.Item(0)
        'endRadCollection = objVarFillet.EndRadiusParams
        'objEndRad = endRadCollection.Item(0)
        'Session.SelectedObjects.Clear()
        'Debug.Print("The fillet's start radius is " & objStartRad.Value)
        'Debug.Print("The fillet's end radius is " & objEndRad.Value)
        Debug.Print("****************************")
        Hook = Nothing
        Root = Nothing
    End Sub
End Module
