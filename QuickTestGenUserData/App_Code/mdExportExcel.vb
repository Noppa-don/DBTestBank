Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Module mdExportExcel
    Public Sub excelreport()
        'Dim fileName As String = "C:\Users\Public\Documents\WorksheetEx.xlsx"
        'Dim spreadSheet As SpreadsheetDocument = SpreadsheetDocument.Open(fileName, True)

        'Using (spreadSheet)
        '    ' Add a WorksheetPart.
        '    Dim newWorksheetPart As WorksheetPart = spreadSheet.WorkbookPart.AddNewPart(Of WorksheetPart)()
        '    newWorksheetPart.Worksheet = New Worksheet(New SheetData())

        '    ' Create a Sheets object.
        '    Dim sheets As Sheets = spreadSheet.WorkbookPart.Workbook.GetFirstChild(Of Sheets)()
        '    Dim relationshipId As String = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart)

        '    ' Get a unique ID for the new worksheet.
        '    Dim sheetId As UInteger = 1
        '    If (sheets.Elements(Of Sheet).Count > 0) Then
        '        sheetId = sheets.Elements(Of Sheet).Select(Function(s) s.SheetId.Value).Max + 1
        '    End If

        '    ' Give the new worksheet a name.
        '    Dim sheetName As String = ("mySheet" + sheetId.ToString())

        '    ' Append the new worksheet and associate it with the workbook.
        '    Dim sheet As Sheet = New Sheet
        '    sheet.Id = relationshipId
        '    sheet.SheetId = sheetId
        '    sheet.Name = sheetName
        '    sheets.Append(sheet)
        'End Using
        'Console.WriteLine("All done.")
        'Console.ReadKey()
    End Sub
End Module

