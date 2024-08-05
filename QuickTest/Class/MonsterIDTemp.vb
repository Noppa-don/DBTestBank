Imports System.Drawing
Imports System.IO

Public Class MonsterIDTemp

    Private cachePath As String = "~/App_Data/MonsterID/cache/{0}"
    Private partsPath As String = "~/App_Data/MonsterID/parts"
    Private LogPath As String = HttpContext.Current.Server.MapPath("~")

    Private parts As String() = New String() {"legs", "arms", "body", "eyes", "mouth"} '{"legs", "hair", "arms", "body", "eyes", "mouth"}
    Private partcount As Integer() = New Integer() {10, 10, 10, 10, 10}
    Private maxSize As Integer = 250

    Public Sub CreateMonsterTest()
        Dim size As Integer = 179
        Dim sourcedir As String = HttpContext.Current.Server.MapPath(partsPath)

        Dim currentParts As Integer() = New Integer(parts.Length - 1) {}

        For i = 1 To 100
            Dim a As Integer = RandomNumber()
            Dim b As Integer = RandomNumber()
            Dim c As Integer = RandomNumber()
            Dim d As Integer = RandomNumber()
            Dim e As Integer = RandomNumber()
            Dim seed As String = String.Format("{0}{1}{2}{3}{4}", a, b, c, d, e)
            currentParts = {a, b, c, d, e}
            Dim filename As String = GetMonsterFilename(seed, size)
            If Not File.Exists(filename) Then
                Using bmp As New Bitmap(size, size)
                    Dim overlay As Bitmap
                    Dim gfx As Graphics = Graphics.FromImage(bmp)

                    gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver
                    gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality

                    For ii As Integer = 0 To currentParts.Length - 1
                        overlay = New Bitmap(Path.Combine(sourcedir, String.Format("{0}_{1}.png", parts(ii), currentParts(ii))))
                        Using overlay
                            gfx.DrawImage(overlay, New Rectangle(New Point(0), bmp.Size), New Rectangle(New Point(0), overlay.Size), GraphicsUnit.Pixel)
                        End Using
                    Next

                    gfx.Dispose()
                    gfx = Nothing

                    Dim path__1 As String = Path.GetDirectoryName(filename)
                    If Not Directory.Exists(path__1) Then
                        Directory.CreateDirectory(path__1)
                    End If

                    If Not File.Exists(path__1 & "\" & filename) Then
                        bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png)
                    End If

                End Using
            End If
        Next
    End Sub

    Dim r As New Random()
    Private Function RandomNumber()
        Return r.Next(0, 15) + 1
    End Function

    Public Sub CreateMonsterTemp()
        Try
            Dim size As Integer = 179
            Dim sourcedir As String = HttpContext.Current.Server.MapPath(partsPath)

            Dim currentParts As Integer() = New Integer(parts.Length - 1) {}

            For a As Integer = 1 To 3
                For b As Integer = 1 To 3
                    For c As Integer = 1 To 3
                        For d As Integer = 1 To 3
                            For e As Integer = 1 To 3
                                ' For i As Integer = 0 To currentParts.Length - 1
                                Dim seed As String = String.Format("{0}{1}{2}{3}{4}", a, b, c, d, e)
                                currentParts = {a, b, c, d, e}
                                Dim filename As String = GetMonsterFilename(seed, size)
                                If Not File.Exists(filename) Then
                                    Using bmp As New Bitmap(size, size)
                                        Dim overlay As Bitmap
                                        Dim gfx As Graphics = Graphics.FromImage(bmp)

                                        gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver
                                        gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality

                                        For ii As Integer = 0 To currentParts.Length - 1
                                            overlay = New Bitmap(Path.Combine(sourcedir, String.Format("{0}_{1}.png", parts(ii), currentParts(ii))))
                                            Using overlay
                                                gfx.DrawImage(overlay, New Rectangle(New Point(0), bmp.Size), New Rectangle(New Point(0), overlay.Size), GraphicsUnit.Pixel)
                                            End Using
                                        Next

                                        gfx.Dispose()
                                        gfx = Nothing

                                        Dim path__1 As String = Path.GetDirectoryName(filename)
                                        If Not Directory.Exists(path__1) Then
                                            Directory.CreateDirectory(path__1)
                                        End If

                                        If Not File.Exists(path__1 & "\" & filename) Then
                                            bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png)
                                        End If

                                    End Using
                                End If
                                'Next
                            Next
                        Next
                    Next
                Next
            Next
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function GetMonsterFilename(seed As Integer, size As Integer) As String
        Return Path.Combine(HttpContext.Current.Server.MapPath(String.Format(cachePath, size)), String.Format("Monster{0}.png", seed))
    End Function


End Class
