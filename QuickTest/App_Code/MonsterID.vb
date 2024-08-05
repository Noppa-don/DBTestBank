'
' * 
' * 
' * ASP.net MonsterID HttpHandler
' * 
' * written by Alexander Schuc <aschuc@furrred.net>
' * 
' * licensed under Ms-PL
' * http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
' * 
' * 
' * Based upon PHP implementation from Andreas Gohr
' * the graphics are from this guy too ;)
' * http://www.splitbrain.org/projects/monsterid
' * 
' * Idea for an indepentend ASP.net HttpHandler by Mads Kristensen
' * http://blog.madskristensen.dk/post/Cool-projects-that-don’t-exist.aspx
' * 
' * ----
' * 
' * Usage:
' * 
' * - Put MonsterID.cs into App_Code
' * - Copy MonsterID into App_Data
' *   Cached images are stored in App_Data/MonsterID/cache/[size]/Monster[seed].png
' * 
' * - Register the HttpHandler in your web.config
' * 
' * <httpHandlers>
' *			<add verb="GET" path="MonsterID.axd" type="furred.MonsterID" validate="false"/>
' *	</httpHandlers>
'	ให้มีไฟล์เปล่าชือ MonsterID.axd ขนาด 0 byte วางไว้ที่ folder ที่่จะ refer <img src  ด้วย
'
'	เพิ่ม <add name="MonsterID" path="MonsterID.axd" verb="*" type="furred.MonsterID" resourceType="File" preCondition="integratedMode" /> ใน block 
'	<system.webServer>
'		<handlers>
'		.......................
'		</handlers>
'		</system.webServer>
' *	
' * - Use it.. (something like that: <img src="MonsterID.axd?seed=blablabla&size=80" />)
' * 
' * ----
' * 
' * v0.2 - 01.08.2007 - security fix ;) - added size limit
' * v0.1 - 31.07.2007 - first release :D 
' * 
' 


Imports KnowledgeUtils.IO
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Security
Imports System.Globalization

Imports System.Drawing
Imports System.IO
Imports System

Namespace furred
    <WebService([Namespace]:="http://net.furred.www/MonsterID")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class MonsterID
        Implements IHttpHandler
        Protected Shared ReadOnly cachePath As String = "~/App_Data/MonsterID/cache/{0}"
        Protected Shared ReadOnly partsPath As String = "~/App_Data/MonsterID/parts"
        Protected Shared ReadOnly LogPath As String = HttpContext.Current.Server.MapPath("~")

        Protected Shared ReadOnly parts As String() = New String() {"legs", "arms", "body", "eyes", "mouth"} '{"legs", "hair", "arms", "body", "eyes", "mouth"}
        Protected Shared ReadOnly partcount As Integer() = New Integer() {15, 15, 15, 15, 15}
        Protected Shared ReadOnly maxSize As Integer = 250

        Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
            'ManageFile Mf = new ManageFile();
            'Mf.CreateFile(LogPath + "\\" + DateTime.Today.Millisecond.ToString()+ ".txt", "CreateMonster" + Environment.NewLine, true);;
            Dim request As HttpRequest = context.Request

            Dim seedSource As String
            Dim seed As Integer
            Dim size As Integer

            If Not String.IsNullOrEmpty(request.QueryString("seed")) Then
                seedSource = FormsAuthentication.HashPasswordForStoringInConfigFile(request.QueryString("seed"), "md5")

                If Not Integer.TryParse(seedSource.Substring(0, 6), NumberStyles.HexNumber, CultureInfo.CurrentCulture.NumberFormat, seed) Then
                    seed = CInt(DateTime.Now.Ticks)
                End If
            Else
                seed = CInt(DateTime.Now.Ticks)
            End If

            If Not Integer.TryParse(request.QueryString("size"), size) OrElse size < 1 OrElse size >= maxSize Then
                size = maxSize
            End If

            context.Response.ContentType = "image/png"
            context.Response.Cache.SetCacheability(HttpCacheability.[Public])
            context.Response.Cache.SetETag(String.Format("{0}:{1}", seed, size))
            context.Response.WriteFile(GetMonster(seed, size))
        End Sub

        Private Function GetMonster(seed As Integer, size As Integer) As String
            Dim filename As String = GetMonsterFilename(seed, size)
            If Not File.Exists(filename) Then
                CreateMonster(seed, size)
            End If

            Return filename
        End Function

        Protected Function GetMonsterFilename(seed As Integer, size As Integer) As String
            Dim filename As String = Path.Combine(HttpContext.Current.Server.MapPath(String.Format(cachePath, size)), String.Format("Monster{0}.png", seed))
            Return filename
        End Function

        Protected Sub CreateMonster(seed As Integer, size As Integer)
            'Dim Mf As New ManageFile()
            Try
                'Mf.CreateFile(LogPath + "\\" + seed + ".txt", "CreateMonster" + Environment.NewLine, true);
                Dim sourcedir As String = HttpContext.Current.Server.MapPath(partsPath)

                Dim rnd As New Random(seed)

                Dim currentParts As Integer() = New Integer(parts.Length - 1) {}

                For i As Integer = 0 To currentParts.Length - 1
                    currentParts(i) = rnd.[Next](1, partcount(i))
                Next

                Using bmp As New Bitmap(size, size)
                    Dim overlay As Bitmap
                    Dim gfx As Graphics = Graphics.FromImage(bmp)

                    gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver
                    gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality

                    For i As Integer = 0 To currentParts.Length - 1
                        overlay = New Bitmap(Path.Combine(sourcedir, String.Format("{0}_{1}.png", parts(i), currentParts(i))))
                        Using overlay
                            gfx.DrawImage(overlay, New Rectangle(New Point(0), bmp.Size), New Rectangle(New Point(0), overlay.Size), GraphicsUnit.Pixel)
                        End Using
                    Next

                    gfx.Dispose()
                    gfx = Nothing

                    Dim filename As String = GetMonsterFilename(seed, size)
                    Dim path__1 As String = Path.GetDirectoryName(filename)
                    If Not Directory.Exists(path__1) Then
                        Directory.CreateDirectory(path__1)
                    End If

                    'Mf.CreateFile(LogPath + "\\" + seed + ".txt", filename + Environment.NewLine, true);
                    bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png)
                End Using
            Catch ex As Exception
                'Mf.CreateFile(LogPath + "\\" + seed + ".txt", ex.Message + Environment.NewLine, true);
                Throw
            End Try

        End Sub

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property
    End Class
End Namespace
