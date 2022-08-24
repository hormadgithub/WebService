Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports System

Public Class clsUtilities

    ''' <summary>
    ''' สำหรับตรวจสอบ single Quote,Double Quote ของคำสั่ง SQL
    ''' </summary>
    ''' <param name="sender">Object Textbox ที่ต้องการ Replace</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckQuoteSQL(ByVal sender As String) As String
        Try
            CheckQuoteSQL = ""
            Dim Sg_Q As String = "'"
            Dim Db_Q As String = """"
            Dim ReplaceSg_Q As String = "''"
            Dim ReplaceDb_Q As String = """"
            CheckQuoteSQL = sender.ToString
            CheckQuoteSQL = CheckQuoteSQL.ToString.Replace(Sg_Q, ReplaceSg_Q)
            CheckQuoteSQL = CheckQuoteSQL.ToString.Replace(Db_Q, ReplaceDb_Q)
            Return CheckQuoteSQL
        Catch ex As Exception
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
        End Try
    End Function

    ''' <summary>
    ''' ฟังก์ชันในการ Format Date yyyyMMdd
    ''' </summary>
    ''' <param name="GetDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DateyyyyMMdd(ByVal GetDate As Date) As String
        Try
            DateyyyyMMdd = Format(GetDate, "yyyyMMdd")
            Return DateyyyyMMdd
        Catch ex As Exception
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf,
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
        End Try
    End Function

    ''' <summary>
    ''' ฟังก์ชันในการ Format Date yyyy-MM-dd
    ''' </summary>
    ''' <param name="GetDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Dateyyyy_MM_dd(ByVal GetDate As Date) As String
        Dateyyyy_MM_dd = Format(GetDate, "yyyy-MM-dd")
        Return Dateyyyy_MM_dd
    End Function

    ''' <summary>
    ''' ฟังก์ชันในการ Format Date dd/MM/yyyy
    ''' </summary>
    ''' <param name="GetDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Date_ddMMyyyy(ByVal GetDate As Date) As String
        Date_ddMMyyyy = Format(GetDate, "dd/MM/yyyy")
        Return Date_ddMMyyyy
    End Function

    ''' <summary>
    ''' ฟังก์ชันในการ Format Date JAN 1,2012
    ''' </summary>
    ''' <param name="sDate">วันที่</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConvertStrToDateEng(ByVal sDate As Date) As String
        Try
            ConvertStrToDateEng = ""
            Dim sDayEng As String
            Dim sYearEng As String

            If Not IsDate(sDate) Then
                ConvertStrToDateEng = "Error."
                Exit Function
            End If
            sDayEng = sDate.Day
            sYearEng = sDate.Year

            Select Case sDate.Month
                Case 1
                    ConvertStrToDateEng = "JAN"
                Case 2
                    ConvertStrToDateEng = "FEB"
                Case 3
                    ConvertStrToDateEng = "MAR"
                Case 4
                    ConvertStrToDateEng = "APR"
                Case 5
                    ConvertStrToDateEng = "MAY"
                Case 6
                    ConvertStrToDateEng = "JUN"
                Case 7
                    ConvertStrToDateEng = "JUL"
                Case 8
                    ConvertStrToDateEng = "AUG"
                Case 9
                    ConvertStrToDateEng = "SEP"
                Case 10
                    ConvertStrToDateEng = "OCT"
                Case 11
                    ConvertStrToDateEng = "NOV"
                Case 12
                    ConvertStrToDateEng = "DEC"
            End Select
            'CStrToDateEng = "" & CStrToDateEng & " " & sDayEng & "," & sYearEng & ""
            ConvertStrToDateEng = String.Format("{0} {1}{2}{3}", ConvertStrToDateEng, sDayEng, ",", sYearEng)
            Return ConvertStrToDateEng
        Catch ex As Exception
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
        End Try
    End Function


End Class
