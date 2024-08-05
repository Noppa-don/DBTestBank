Imports System.Web.Script.Serialization
Imports System.Data.Entity.Design.PluralizationServices
Imports System.Globalization
Imports System.Web

Public Class selectedSearch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Call LoadAllData()
        End If
    End Sub
    Private Shared Sub LoadAllData()
        If HttpContext.Current.Application("DictData") Is Nothing Then
            Dim dictData As Dictionary(Of String, String)
            Dim objLoader As New Loader
            dictData = objLoader.LoadAllData()
            objLoader = Nothing
            HttpContext.Current.Application("DictData") = dictData
            dictData = Nothing
        End If
    End Sub


    <Services.WebMethod()>
    Public Shared Function translateFromSelected(ByVal selectedText As String)
        LoadAllData()   'เช็คอีกรอบ เผื่อ application data มันหลุดไป, จะได้มีการ โหลดจาก database ขึ้นมาใหม่อีกรอบนึง (แต่โดยปกติไม่ควรจะต้องโหลดใหม่)
        Dim dictData As Dictionary(Of String, String)
        dictData = CType(HttpContext.Current.Application("DictData"), Dictionary(Of String, String))

        Dim returnValue As String = ""
        Dim returnFileMp3 As String = ""

        Dim ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"))

        If ps.IsPlural(selectedText) Then
            selectedText = ps.Singularize(selectedText)
        End If

        If dictData.TryGetValue(selectedText.Trim().ToLower(), returnValue) OrElse dictData.TryGetValue(selectedText.Trim(), returnValue) Then
            Dim fileName As String = StrToHex(selectedText.Trim().ToLower()) + ".mp3"
            'Dim fileName As String = "63-61-6D-70.mp3"
            Dim mp3PathName As String = HttpContext.Current.Server.MapPath("../dictionary/mp3")
            Dim fullFileName As String = System.IO.Path.Combine(mp3PathName, fileName)
            If (System.IO.File.Exists(fullFileName)) Then
                'returnValue = returnValue.Replace("$F$", "<a href=""#"" onclick=""playSound('../dictionary/mp3/" & fileName & "');""><img src='../images/dictionary/realvoice.png' /></a>")
                returnFileMp3 = "<a href=""#"" style='margin-left:5px;' onclick=""playSound('../dictionary/mp3/" & fileName & "');""><img style='width: 70px;height: 70px;position: absolute;' src='../images/dictionary/realvoice.png' /></a>"
            End If
            'Else
            '    returnValue = returnValue.Replace("$F$", "")
            'End If
            'Return returnValue

            returnValue = returnValue.Replace("$F$", "")
        Else
            'Return ""
            returnValue = ""
        End If
        Dim jsonString = New With {.returnValue = returnValue, .returnFileMp3 = returnFileMp3}
        Dim js As New JavaScriptSerializer()
        translateFromSelected = js.Serialize(jsonString)
    End Function

    Private Shared Function StrToHex(ByVal Data As String) As String
        Dim sVal As String
        Dim sHex As String = ""
        While Data.Length > 0
            sVal = Conversion.Hex(Strings.Asc(Data.Substring(0, 1).ToString()))
            Data = Data.Substring(1, Data.Length - 1)
            sHex = sHex & sVal & "-"
        End While
        Return sHex.Substring(0, sHex.Length - 1)
    End Function

End Class