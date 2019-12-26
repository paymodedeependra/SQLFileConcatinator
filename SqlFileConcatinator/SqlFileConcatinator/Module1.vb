Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Data.Linq
Imports System.Collections.Generic


Module Module1

    Sub Main()
        Dim InPath As String = ConfigurationManager.AppSettings("InputFilePath")
        Dim OutPath As String = ConfigurationManager.AppSettings("OutputFilePath")
        Dim FileFormat As String = ConfigurationManager.AppSettings("FileFormat")
        Dim di As DirectoryInfo = New DirectoryInfo(InPath)
        Dim Files
        Dim FileSet As New List(Of FileInfo)
        Dim inDate As Date = ConfigurationManager.AppSettings("FromDate")
        Dim contents As String = ""
        Dim Builder As New StringBuilder
        Dim FileDate As String = System.DateTime.Now.ToString("dd/MM/yyyy")
        Dim FileName As String = New String(OutPath & "FinalSqlScript-" & FileDate & ".sql")

        Dim csvs = FileFormat.Split(",")
        For Each csv In csvs
            Files = di.GetFiles(csv)
            For Each f In Files
                FileSet.Add(f)
            Next
        Next

        File.Create(FileName).Dispose()
        For Each File In FileSet
            Dim fi As String = File.Name
            Dim crDate As Date = File.LastWriteTime
            If crDate >= inDate Then
                Using streamReader As StreamReader = New StreamReader(InPath & fi)
                    contents = streamReader.ReadToEnd()
                    Builder.Append(contents).AppendLine()
                End Using
            End If
        Next
        If Builder IsNot Nothing Then
            Using writer As StreamWriter = New StreamWriter(FileName, False)
                writer.WriteLine(Builder)
            End Using
        End If
    End Sub
End Module
