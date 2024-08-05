Imports System.Data.Linq

Namespace Database

    ''' <summary>
    ''' คลาสช่วยเรื่องดาต้าเบส
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DBFactory

        Private Shared _ApplicationManager As IApplicationManager
        Private Shared _DbCommon As IDbCommon
        Private Shared _ClassDataContext As Type

        ''' <summary>
        ''' รับคุณสมบัติคลาส IApplicationManager เผื่อระบุถึงประเภทของ App ซึ่งจะมีการจัดเก็บข้อมูลพื้นฐานต่างไว้ให้ใช้
        ''' </summary>
        ''' <param name="ApplicationManager"></param>
        ''' <remarks></remarks>
        Public Shared Sub RegisterApplicationManager(ApplicationManager As IApplicationManager)
            _ApplicationManager = ApplicationManager
        End Sub

        ''' <summary>
        ''' รับคุณสมบัติคลาส IData เผื่อระบุประเภทของฐานข้อมูลใช้กับกลุ่มคลาสข้อมูลพื้นฐาน
        ''' </summary>
        ''' <param name="DbCommon"></param>
        ''' <remarks></remarks>
        Public Shared Sub RegisterDbCommon(DbCommon As IDbCommon)
            _DbCommon = DbCommon
        End Sub

        ''' <summary>
        ''' คืนค่า IApplicationManager
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property GetApplicationManager() As IApplicationManager
            Get
                If _ApplicationManager Is Nothing Then
                    Throw New Exception("โปรดระบุ RegisterApplicationManager")
                End If
                Return _ApplicationManager
            End Get
        End Property

        ''' <summary>
        ''' คืนค่ากลุ่มคลาสข้อมูลพื้นฐาน
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property GetDB() As DBManager
            Get
                If _ApplicationManager Is Nothing OrElse _DbCommon Is Nothing Then
                    Throw New Exception("โปรดระบุ RegisterApplicationManager และ RegisterDbCommon")
                End If
                Return New DBManager With {.ApplicationManager = _ApplicationManager, .DbCommon = _DbCommon}
            End Get
        End Property

        Public Class LinqToSql(Of ClassDataContext As DataContext)

            ''' <summary>
            ''' คืนค่ากลุ่มคลาสข้อมูล Linq to sql
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Shared ReadOnly Property GetLinqToSql As LinqSqlManager(Of ClassDataContext)
                Get
                    Return New LinqSqlManager(Of ClassDataContext) With {.ApplicationManager = _ApplicationManager}
                End Get
            End Property

        End Class

    End Class

End Namespace


