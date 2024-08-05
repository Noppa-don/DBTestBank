Public Class EncryptionPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'TempFolderForPDF=D:\Development2\QuickTest\Source\QuickTest\QuickTest\Report\|NeedEditButton=false|NewRelic.AppName=TonLenovo_VS_EDevel_QuickTest_Test|NeedJoinQ40=false|NeedQuizMode=true|NeedCheckmark=false|SuperUser=approve|NeedAddEvaluationIndex=false|AllowRegisteredTabletOwnerToLogIn=False|NeedPracticeMode=true|EnabledClassesInPracticeMode=k4,k5,k6,k7,k8,k9,k10,k11,k12,k13,k14,k15|DefaultSchoolCode=1000001|ClassSessionIdleTimeout=120|DefaultUserId=4AF763E3-C133-4B76-A1A2-6D69CF0909D9|NeedHomeWork=true|NeedReportButton=false|NeedAddNewQCatAndQsetButton=false|NeedDeleteQcatAndQset=false|NeedAddNewQuestionButton=false|NeedDeleteQuestionButton=false|NeedSelectSesstion=true|NeedChangePasswordMode=true|NeedManageSchoolInfo=true|NeedEditQuestionCategory=false|IsAllowPracticeMode=True|WarningTxt=ติดต่อศูนย์คอม|DebugAPI=false|RunMode=Full|ConnectedWithT360=True
        Dim a As String = ClsLanguage.GenerateMachineIdentification
        Dim b As String = ClsLanguage.GetBasePath()

        Path.InnerText = a & "................." & b



    End Sub

    Private Sub btnEncryption_Click(sender As Object, e As EventArgs) Handles btnEncryption.Click
        If txtAppSetting.Text IsNot Nothing And txtAppSetting.Text.Trim() <> "" Then
            Dim StrEncryption = KNConfigData.KnEncryption(txtAppSetting.Text)
            txtEncryption.Text = StrEncryption
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox2.Text = KNConfigData.DecryptData(TextBox1.Text)

        'Dim dt As New DataTable
        'dt.Columns.Add("RowNum", GetType(String), Nothing)
        'dt.Columns.Add("KeyName", GetType(String), Nothing)
        'dt.Columns.Add("KeyValue", GetType(String), Nothing)

        'Dim txtKey As String = TextBox2.Text
        'Dim ArrKeyValue() As String = txtKey.Split("|")

        'If ArrKeyValue.Count > 1 Then

        'End If
        'For i = 1 To ArrKeyValue.Count
        '    Dim ArrEachKey() As String = ArrKeyValue(i - 1).Split("*")
        '    Dim row = dt.NewRow()
        '    row(0) = i.ToString
        '    row(1) = ArrEachKey(0).ToString
        '    row(2) = ArrEachKey(1).ToString
        '    dt.Rows.Add(row)
        'Next
        'gvExcode.DataSource = dt
        'gvExcode.DataBind()
    End Sub
End Class