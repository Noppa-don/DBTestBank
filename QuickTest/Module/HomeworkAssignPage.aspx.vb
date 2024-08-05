Imports System.Web.Services
Imports KnowledgeUtils.Json.Serialization
Imports KnowledgeUtils.System
Imports BusinessTablet360
Imports Microsoft.Practices.Unity

Public Class HomeworkAssignPage
    Inherits System.Web.UI.Page


#Region "Dependency"

    Public Sub New()
        CType(Context.ApplicationInstance, Global_asax).Container.BuildUp(Me.GetType, Me)
    End Sub

    Private _UserConfig As IUserConfigManager
    <Dependency()> Public Property UserConfig() As IUserConfigManager
        Get
            Return _UserConfig
        End Get
        Set(ByVal value As IUserConfigManager)
            _UserConfig = value
        End Set
    End Property

#End Region

#Region "Property"

    Private _DataClass As String
    Public Property DataClass() As String
        Get
            Return _DataClass
        End Get
        Set(ByVal value As String)
            _DataClass = value
        End Set
    End Property

    Private _TestSetId As String
    Public Property TestSetId() As String
        Get
            Return _TestSetId
        End Get
        Set(ByVal value As String)
            _TestSetId = value
        End Set
    End Property

    Private _TestSetName As String
    Public Property TestSetName() As String
        Get
            Return _TestSetName
        End Get
        Set(ByVal value As String)
            _TestSetName = value
        End Set
    End Property

    Private _DataSetTime As String
    Public Property DataSetTime() As String
        Get
            Return _DataSetTime
        End Get
        Set(ByVal value As String)
            _DataSetTime = value
        End Set
    End Property

    Private _DataModule As String
    Public Property DataModule() As String
        Get
            Return _DataModule
        End Get
        Set(ByVal value As String)
            _DataModule = value
        End Set
    End Property

    Private _DataAssign As String
    Public Property DataAssign() As String
        Get
            Return _DataAssign
        End Get
        Set(ByVal value As String)
            _DataAssign = value
        End Set
    End Property

    Private _ModuleId As String
    Public Property ModuleIdUI() As String
        Get
            Return _ModuleId
        End Get
        Set(ByVal value As String)
            _ModuleId = value
        End Set
    End Property

    Private _SchoolCode As String
    Public Property SchoolCode() As String
        Get
            Return _SchoolCode
        End Get
        Set(ByVal value As String)
            _SchoolCode = value
        End Set
    End Property

    Private _PageName As String
    Public Property PageName() As String
        Get
            Return _PageName
        End Get
        Set(ByVal value As String)
            _PageName = value
        End Set
    End Property
    Private _ClassName As String
    Public Property ClassName() As String
        Get
            Return _ClassName
        End Get
        Set(ByVal value As String)
            _ClassName = value
        End Set
    End Property

    Private _MA_Id As String
    Public Property MA_Id() As String
        Get
            Return _MA_Id
        End Get
        Set(ByVal value As String)
            _MA_Id = value
        End Set
    End Property

    Private _IsNew As Boolean
    Public Property IsNew() As Boolean
        Get
            Return _IsNew
        End Get
        Set(ByVal value As Boolean)
            _IsNew = value
        End Set
    End Property

#End Region

    Public IE As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If IE = "1" Then
        UserConfig.AddSchoolCode("1000001")
        SchoolCode = UserConfig.GetCurrentContext.School_Code

        TestSetId = "1fb29a3f-158e-45e0-9975-fefa6cb5153a"
        MA_Id = Nothing
        IsNew = True
        PageName = "Homework/DashboardHomeworkPage.aspx"
        IE = "1"
#Else
        'SchoolCode = UserConfig.GetCurrentContext.School_Code
        'ของจิง
        PageName = Request.QueryString("PageName")
        ClassName = Request.QueryString("ClassName")
        MA_Id = Request.QueryString("MAId") 'กรณีเคยสั่งการบ้านแล้วต้องส่งมา (คุมหน้าจอ)
        TestSetId = Request.QueryString("TestSetId") 'กรณีสั่งการบ้านใหม่ต้องส่งมา (คุมหน้าจอ)
        IsNew = CType(Request.QueryString("IsNew"), Boolean) 'กรณี คัดลอกควรส่งเป็น true
        UserConfig.AddUserId(Session("UserId"))
        UserConfig.AddSchoolCode(Session("SchoolCode"))
        SchoolCode = UserConfig.GetCurrentContext.School_Code
#End If
        

        'test
        'PageName = ""
        'MA_Id = "900bd0cb-1434-4970-8c05-a45a1755d54d"
        'MA_Id = "FC570AC4-1C26-4088-B3A4-99B7122DB6F8"
        'TestSetId = "4F053996-F74E-494C-9373-0024E1D9DBA9"
        'Dim IsNew = False

        If Not IsPostBack Then
            Dim Wc As New ModuleService
            '#If F5 = "1" Then
            '            Application("EnableUserSubjectClass") = "thai-K4,eng-K4,pisa-K10,thai-K10,social-K10,math-K10,science-K10,thai-K11,social-K11,math-K11,science-K11,thai-K12,social-K12,math-K12,science-K12,thai-K13,social-K13,math-K13,science-K13,thai-K14,social-K14,math-K14,science-K14,thai-K15,social-K15,math-K15,science-K15,pisa-K8"
            '#End If

            DataClass = Wc.GetTeacherClassCheckWithFile(UserConfig.GetCurrentContext.School_Code, UserConfig.GetCurrentContext.User_Id.ToString, Application("EnableUserSubjectClass").ToString).SerializeToJson
            Dim Assignment As Object = Nothing
            Dim ModuleId = Nothing
            Dim SelectTimeList = New List(Of Object)
            Dim SelectAssignmentList = New List(Of Object)

            If MA_Id <> "" Then
                Assignment = Wc.GetModuleAssignmentByMaId(New Guid(MA_Id))

                ModuleId = Assignment.Module_Id
                ModuleIdUI = Assignment.Module_Id.ToString

                SelectTimeList.Add(New With {.id = Assignment.Start_Date,
                                            .text = "start",
                                            .type = EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)(),
                                            .maId = Assignment.MA_Id,
                                            .mdId = Nothing})

                SelectTimeList.Add(New With {.id = Assignment.End_Date,
                                            .text = "end",
                                            .type = EnModuleUI.MdModuleHomeworkTime.GetEnumText(Of EnumModuleUI)(),
                                            .maId = Assignment.MA_Id,
                                            .mdId = Nothing})

                For Each Item In Wc.GetModuleAssignmentJoinByMaId(New Guid(MA_Id))
                    If Item.Class_Name IsNot Nothing Then
                        SelectAssignmentList.Add(New With {.id = Item.Class_Name,
                                                 .text = Item.Class_Name,
                                                 .type = EnModuleUI.MdClass.GetEnumText(Of EnumModuleUI)(),
                                                 .maId = Item.MAD_Id,
                                                 .mdId = Nothing})
                    End If
                    If Item.Room_Id IsNot Nothing Then
                        SelectAssignmentList.Add(New With {.id = Item.Room_Id,
                                                 .text = Item.Class_Room,
                                                 .type = EnModuleUI.MdRoom.GetEnumText(Of EnumModuleUI)(),
                                                 .maId = Item.MAD_Id,
                                                 .mdId = Nothing})
                    End If
                    If Item.Student_Id IsNot Nothing Then
                        SelectAssignmentList.Add(New With {.id = Item.Student_Id,
                                                 .text = Item.Student_Name,
                                                 .type = EnModuleUI.MdStudent.GetEnumText(Of EnumModuleUI)(),
                                                 .maId = Item.MAD_Id,
                                                 .mdId = Nothing})
                    End If
                Next
            End If

            If TestSetId <> "" Then
                TestSetName = Wc.GetTestSetByTestSetId(New Guid(TestSetId)).TestSet_Name
            End If

            Dim SelectModuleList As New List(Of Object)
            For Each Item In Wc.GetModuleDetailHomeworkByModuleId(ModuleId)
                SelectModuleList.Add(New With {.id = Item.Reference_Id,
                                         .text = Item.TestSet_Name,
                                         .type = Item.Md_Type,
                                         .maId = Nothing,
                                         .mdId = Item.ModuleDetail_Id})
            Next

            DataModule = SelectModuleList.SerializeToJson '***
            DataAssign = SelectAssignmentList.SerializeToJson '***
            DataSetTime = SelectTimeList.SerializeToJson '***
        End If
    End Sub

End Class