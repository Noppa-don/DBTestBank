Friend NotInheritable Class CommonSubjectsText
    'ชื่อวิชา (ภาษาไทย)  choosesubject
    Public Const thai As String = "ไทย"
    Public Const math As String = "คณิตฯ"
    Public Const home As String = "การงาน"
    Public Const social As String = "สังคม"
    Public Const english = "ภาษาอังกฤษ"
    Public Const art = "ศิลปะ"
    Public Const health As String = "สุขศึกษาฯ"
    Public Const science As String = "วิทยาศาสตร์"
    ''' <summary>
    ''' subjectId
    ''' </summary>
    Public Const ThaiSubjectId As String = "E7EDF837-4A6A-4E69-A62D-158F26A2BB7D"
    Public Const MathSubjectId As String = "A4B9F5CB-2D3C-4F6A-8666-FD2620E69723"
    Public Const EnglishSubjectId As String = "FB677859-87DA-4D8D-A61E-8A76566D69D8"
    Public Const SocialSubjectId As String = "FDA224D9-CEBE-4642-ACD0-D7F7282E36AE"
    Public Const ScienceSubjectId As String = "58802565-23BB-4F22-8238-E983AC781B0F"
    Public Const HomeSubjectId As String = "47A224EF-3348-41B7-84D0-7250648F8706"
    Public Const HealthSubjectId As String = "6A4A7294-F5A7-4D64-ADBC-73DC14377737"
    Public Const ArtSubjectId As String = "73C4639B-267C-4B7E-B5A4-1B4EBB428019"
    Public Const PisaSubjectId As String = "7F2C522A-FF65-4CA7-9DCD-EC5102A38731"
    Public Const SkillSubjectId As String = "20F40054-3004-4DAB-A56A-19AA0C37517C"
    Public Const ReliSubjectId As String = "6ED7935B-DC29-4513-81D4-472436E6AC9E"
    Public Const SelfSubjectId As String = "2D66B87B-D6A6-4AE4-BBEA-819EAD67D13F"
    Public Structure GroupSubjectName
        Public Const Thai As String = "กลุ่มสาระการเรียนรู้ภาษาไทย"
        Public Const Art As String = "กลุ่มสาระการเรียนรู้ศิลปะ"
        Public Const Home As String = "กลุ่มสาระการเรียนรู้การงานอาชีพและเทคโนโลยี"
        Public Const Health As String = "กลุ่มสาระการเรียนรู้สุขศึกษาและพลศึกษา"
        Public Const English As String = "กลุ่มสาระการเรียนรู้ภาษาต่างประเทศ"
        Public Const Social As String = "กลุ่มสาระการเรียนรู้สังคมศึกษาศาสนาและวัฒนธรรม"
        Public Const Science As String = "กลุ่มสาระการเรียนรู้วิทยาศาสตร์"
        Public Const Math As String = "กลุ่มสาระการเรียนรู้คณิตศาสตร์"
    End Structure

    Public Structure SubjectBookName
        Public Const Thai As String = "ภาษาไทย"
        Public Const Art As String = "ศิลปะ"
        Public Const Home As String = "การงานอาชีพและเทคโนโลยี"
        Public Const Health As String = "สุขศึกษาและพลศึกษา"
        Public Const English As String = "ภาษาอังกฤษ"
        Public Const Social As String = "สังคมศึกษา ศาสนา และวัฒนธรรม"
        Public Const Science As String = "วิทยาศาสตร์"
        Public Const Math As String = "คณิตศาสตร์"
    End Structure

    Public Structure SubjectShortThaiName
        Public Const Thai As String = "ไทย"
        Public Const Art As String = "ศิลปะ"
        Public Const Home As String = "การงานฯ"
        Public Const Health As String = "สุขศึกษาฯ"
        Public Const English As String = "อังกฤษ"
        Public Const Social As String = "สังคมฯ"
        Public Const Science As String = "วิทย์ฯ"
        Public Const Math As String = "คณิตฯ"
        Public Const PISA As String = "PISA"
    End Structure

    Public Structure SubjectShortEngName
        Public Const Thai As String = "thai"
        Public Const Art As String = "art"
        Public Const Home As String = "career"
        Public Const Health As String = "health"
        Public Const English As String = "eng"
        Public Const Social As String = "social"
        Public Const Science As String = "science"
        Public Const Math As String = "math"
        Public Const PISA As String = "PISA"
    End Structure

End Class
Friend NotInheritable Class PrefixAnswer
    Public Shared Thai() As String = {"ก", "ข", "ค", "ง", "จ", "ฉ", "ช", "ซ", "ฌ", "ญ", "ฎ", "ฏ", "ฑ", "ฒ", "ณ", "ด", "ต"}
    Public Shared Eng() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i"}
End Class
