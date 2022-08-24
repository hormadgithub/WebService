Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports System.Data
Imports System.Data.SqlClient

Public Class clsDBUtilities
    Private objCurrentVar As clsCurrentVar
    Private objUtilities As clsUtilities

    Public Sub New(ByVal CurrentVar As clsCurrentVar)
        Me.objCurrentVar = CurrentVar
    End Sub

    '''' <summary>
    '''' สำหรับเลือกค่า Return จาก Field ที่เราได้เลือกมาจาก Table แต่ไม่สามารถเพิ่มเงื่อนไข Column ได้
    '''' Column นั้นต้องอยู่ใน Table เท่านั้น
    '''' </summary>
    '''' <param name="dbfName"></param>
    '''' <param name="FldReturn"></param>
    '''' <param name="Condition"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function Rtn_Val(ByVal dbfName As String, ByVal FldReturn As String, ByVal Condition As String) As String
    '    Rtn_Val = ""
    '    Try
    '        Dim StrBD = New StringBuilder
    '        StrBD.AppendFormat("select ({0}) as FldReturn ", FldReturn).AppendLine()
    '        StrBD.AppendFormat("from    {0}", dbfName).AppendLine()
    '        StrBD.AppendFormat("where  {0}", Condition)

    '        Dim dt As New DataTable
    '        'Me.objGlobalVar = New clsGlobalVar()
    '        Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
    '            dt = objCon.GetDataTable(StrBD.ToString())
    '            If dt.Rows.Count > 0 Then
    '                If IsDBNull(dt.Rows(0)(Trim("FldReturn"))) Then
    '                    Rtn_Val = ""
    '                Else
    '                    Rtn_Val = Convert.ToString(dt.Rows(0)(Trim("FldReturn"))).Trim
    '                End If
    '            Else
    '                Rtn_Val = ""
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        Dim CurrentStack As New StackTrace()
    '        Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
    '                  CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
    '    End Try

    '    Return Rtn_Val
    'End Function

    '''' <summary>
    '''' สำหรับ Count จำนวน Rows
    '''' </summary>
    '''' <param name="TBname">ชื่อ Table</param>
    '''' <param name="ColumnName">ชื่อ Column ที่ต้องการ Count</param>
    '''' <param name="strCondition">เงื่อนไขการ Count</param>
    '''' <param name="MoreOneTable">ตรวจสอบว่ามีการ Join กันมากกว่า 1 Table หรือไม่ เริ่มต้นให้เป็น False แสดงว่า Table เดียว</param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function Rtn_CountRows(ByVal TBname As String, ByVal ColumnName As String, ByVal strCondition As String, _
    '                              Optional ByVal MoreOneTable As Boolean = False) As Integer
    '    Rtn_CountRows = 0
    '    Try
    '        Dim StrBD As New StringBuilder
    '        If strCondition = "" Then
    '            StrBD.AppendFormat("SELECT COUNT({0}) AS Total_Record FROM ({1})t1 ", ColumnName, TBname).AppendLine()
    '        Else
    '            If MoreOneTable Then 'แสดงว่ามีการใช้งานหลาย Table เลยต้องใช้ Query ตัวนี้
    '                StrBD.AppendFormat("SELECT COUNT({0}) AS Total_Record ", ColumnName).AppendLine()
    '                StrBD.AppendFormat("FROM   {0}", TBname).AppendLine()
    '                StrBD.AppendFormat("where  {0}", strCondition)
    '            Else
    '                StrBD.AppendFormat("SELECT COUNT({0}) AS Total_Record FROM {1} t1 WITH (NOLOCK) ", ColumnName, TBname).AppendLine()
    '                StrBD.AppendFormat("where  {0}", strCondition)
    '            End If
    '        End If

    '        Dim dt As New DataTable
    '        'Me.objGlobalVar = New clsGlobalVar()
    '        Using objConn As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
    '            dt = objConn.GetDataTable(StrBD.ToString())
    '            If dt.Rows.Count > 0 Then
    '                Rtn_CountRows = dt.Rows(0)("Total_Record").ToString()
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        Dim CurrentStack As New StackTrace()
    '        Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
    '                  CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
    '    End Try

    '    Return Rtn_CountRows
    'End Function

    '''' <summary>
    '''' Run CScode fn_ProspectCustomer
    '''' </summary>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function Runno_ProspectCustomer() As String
    '    Runno_ProspectCustomer = ""
    '    Try
    '        Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
    '            Dim StrBD As New StringBuilder
    '            Dim dt As New DataTable
    '            StrBD.AppendFormat("select  Top 1 replace(pCScode,'x','') as CScode").AppendLine()
    '            StrBD.AppendFormat("from    fn_ProspectCustomer").AppendLine()
    '            StrBD.AppendFormat(" Order by pCScode desc")

    '            dt = objCon.GetDataTable(StrBD.ToString())
    '            If dt.Rows.Count > 0 Then
    '                Runno_ProspectCustomer = dt.Rows(0)("CScode")
    '                Runno_ProspectCustomer = String.Format("x{0}", (CInt(Runno_ProspectCustomer) + 1).ToString("00000"))
    '            Else
    '                Runno_ProspectCustomer = "x00001"
    '            End If
    '        End Using

    '        Return Runno_ProspectCustomer
    '    Catch ex As Exception
    '        Dim CurrentStack As New StackTrace()
    '        Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
    '                  CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
    '    End Try
    'End Function

    '''' <summary>
    '''' Run CScode fn_ProspectContactPerson
    '''' </summary>
    '''' <param name="CScode">รหัสลูกค้า</param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function Runno_ProspectContactPerson(ByVal CScode As String) As String
    '    Runno_ProspectContactPerson = ""
    '    Try
    '        Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
    '            Dim StrBD As New StringBuilder
    '            Dim dt As New DataTable
    '            StrBD.AppendFormat("select  isnull(max(PCTPcode),0) PCTPcode").AppendLine()
    '            StrBD.AppendFormat("from    fn_ProspectContactPerson").AppendLine()
    '            StrBD.AppendFormat("where   PCScode='{0}'", CScode.Trim())

    '            dt = objCon.GetDataTable(StrBD.ToString())
    '            If dt.Rows.Count > 0 Then
    '                Runno_ProspectContactPerson = dt.Rows(0)("PCTPcode")
    '                If (Runno_ProspectContactPerson.Trim() <> "") Then
    '                    Dim i As Integer = 0
    '                    i = InStr(Runno_ProspectContactPerson, "-")
    '                    Runno_ProspectContactPerson = Mid(Runno_ProspectContactPerson, i + 1, Runno_ProspectContactPerson.Trim().Length)
    '                    Runno_ProspectContactPerson = String.Format("{0}-{1}", CScode, (CInt(Runno_ProspectContactPerson.Trim()) + 1).ToString("00"))
    '                End If
    '            Else
    '                Runno_ProspectContactPerson = String.Format("x{0}-01", CScode.Trim())
    '            End If
    '        End Using

    '        Return Runno_ProspectContactPerson
    '    Catch ex As Exception
    '        Dim CurrentStack As New StackTrace()
    '        Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
    '                  CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
    '    End Try
    'End Function


End Class
