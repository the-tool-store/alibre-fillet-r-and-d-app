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
        For Each objEdge As IADEdge In objIADFaces.Item(1).Edges
            Console.WriteLine(objEdge.IsSenseReversed)
            objFilletTargets.Add(objEdge)
            Session.Select(objFilletTargets)
        Next
        Debug.Print("****************************")
        Hook = Nothing
        Root = Nothing
    End Sub
End Module
