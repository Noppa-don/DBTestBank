Public Class MaxonetParentData
    Property StudentName As String
    Property TotalResult As Decimal
    Property Subjects As New List(Of SubjectResult)
    Property NumberOfTimes As String
    Property TotalTime As String
    Property Workings As String
    Property Recommend As String
    Property ActivitiesQuantityPercent As Decimal
    Property ActivitiesQuantityPercentTxt As String
    Property PracticesQuantityPercent As Decimal
    Property PracticesQuantityPercentTxt As String
    Property ActivitiesQuiz As New List(Of SubjectQuantityResult)
    Property PracticesQuiz As New List(Of SubjectQuantityResult)

    'm.StudentName = "ยังไม่ได้ลงทะเบียนนักเรียน"
    '    m.NumberOfTimes = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
    '    m.TotalTime = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
    '    m.Workings = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
    '    m.Recommend = "ยังไม่ได้เปิดใช้งานกับนักเรียน"
End Class

Public Class SubjectResult
    Property Name As String
    Property Result As String
End Class

Public Class SubjectQuantityResult
    Property Name As String
    Property Amount As Integer
    Property Percent As Decimal
End Class