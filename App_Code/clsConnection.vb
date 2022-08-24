Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports System.Data
Imports System.Data.SqlClient
Imports System


Public Class clsConnection
    Implements IDisposable    'สำหรับใช้ในการปิด Disponse Object แบบ Auto ใน Class 

    Private disposedValue As Boolean = False        ' To detect redundant calls

    Private Shared _CurrentconnectionString As String
    Private Shared _objConnection As clsConnection
    Public Shared _objConn As SqlConnection
    Private _objCmd As SqlCommand
    Private _objDS As DataSet
    Private _objDT As DataTable
    Private _objDA As SqlDataAdapter

    'Private Sub New()

    'End Sub

    ''' <summary>
    ''' สำหัรบ Get Connection
    ''' </summary>
    ''' <param name="ConnectionString"></param>
    ''' <returns>Return ClsConnection</returns>
    ''' <remarks></remarks>
    Public Shared Function GetConnection(ByVal ConnectionString As String) As clsConnection
        'ถ้า Object Dispose
        'หรือมีการเปลี่ยนการเชื่อมต่อใหม่ให้ New Connection แต่ถ้าเหมือนกันเดิมให้ส่ง object Connection กลับไปเลย
        If (_objConn Is Nothing) OrElse (Not ConnectionString.Equals(_CurrentconnectionString)) Then
            _objConn = New SqlConnection(ConnectionString)
            _CurrentconnectionString = ConnectionString
            _objConnection = New clsConnection
        End If
        Return _objConnection
    End Function

    ''' <summary>
    ''' เปิด Concectiong
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OpenConnection()
        If (_objConn.State = ConnectionState.Closed) Then
            _objConn.Open()
        End If
    End Sub

    ''' <summary>
    ''' ปิด Connection
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseConnection()
        If (_objConn.State = ConnectionState.Open) Then
            _objDA = Nothing
            _objConn.Close()
            _objConn = Nothing
        End If
    End Sub


    ''' <summary>
    ''' DataTable สำหรับกรณี CommandType เป็น Text 
    ''' </summary>
    ''' <param name="SqlCommandString"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(ByVal SqlCommandString As String) As DataTable
        _objCmd = New SqlCommand(SqlCommandString, _objConn)
        _objCmd.CommandTimeout = 50 'วันที่เพิ่ม 11/ต.ค./2016 โดยสิทธิ์ราช เนื่องจากต้องปรับ Timeout ให้เยอะขึ้น เพราะว่ามันเตือน TimeOutExpire
        Return GetDataTable(_objCmd, CommandType.Text)
    End Function


    ''' <summary>
    ''' DataTable สำหรับกรณีส่ง SqlCommand มาให้
    ''' </summary>
    ''' <param name="ObjSqlCommand"></param>
    ''' <param name="CommandType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(ByVal ObjSqlCommand As SqlCommand, ByVal CommandType As CommandType) As DataTable
        Try
            'Me.OpenConnection()
            _objDT = New DataTable
            ObjSqlCommand.CommandType = CommandType
            ObjSqlCommand.Connection = _objConn
            ObjSqlCommand.CommandTimeout = 50 'วันที่เพิ่ม 11/ต.ค./2016 โดยสิทธิ์ราช เนื่องจากต้องปรับ Timeout ให้เยอะขึ้น เพราะว่ามันเตือน TimeOutExpire
            _objDA = New SqlDataAdapter(ObjSqlCommand)
            _objDA.Fill(_objDT)
            Return _objDT
        Catch ex As Exception
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
        Finally
            'Me.CloseConnection()
        End Try
    End Function



    Public Function GetDataSet(ByVal SqlCommandString As String) As DataSet
        _objCmd = New SqlCommand(SqlCommandString, _objConn)
        _objCmd.CommandTimeout = 50 'วันที่เพิ่ม 11/ต.ค./2016 โดยสิทธิ์ราช เนื่องจากต้องปรับ Timeout ให้เยอะขึ้น เพราะว่ามันเตือน TimeOutExpire
        Return GetDataSet(_objCmd, CommandType.Text)
    End Function


    ''' <summary>
    ''' DataSet 
    ''' </summary>
    ''' <param name="ObjSqlCommand"></param>
    ''' <param name="CommandType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet(ByVal ObjSqlCommand As SqlCommand, ByVal CommandType As CommandType) As DataSet
        Try
            'Me.OpenConnection()
            _objDS = New DataSet
            ObjSqlCommand.CommandType = CommandType
            ObjSqlCommand.Connection = _objConn
            ObjSqlCommand.CommandTimeout = 50 'วันที่เพิ่ม 11/ต.ค./2016 โดยสิทธิ์ราช เนื่องจากต้องปรับ Timeout ให้เยอะขึ้น เพราะว่ามันเตือน TimeOutExpire
            _objDA = New SqlDataAdapter(ObjSqlCommand)
            _objDA.Fill(_objDS)
            Return _objDS
        Catch ex As Exception
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
        Finally
            'Me.CloseConnection()
        End Try
    End Function



    ''' <summary>
    ''' การแสดงข้อมูลของ Stored Procedure ที่ใช้ Output
    ''' </summary>
    ''' <param name="StrSql">SQL Command</param>
    ''' <param name="Cmd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStoreProcedureOutput(ByVal StrSql As String, ByVal Cmd As SqlCommand) As SqlCommand
        Dim ErrorStep As String = ""
        Try
            'Me.OpenConnection()
            With Cmd
                .CommandType = CommandType.StoredProcedure
                .CommandText = StrSql
                ErrorStep = "Step 1 .Connection = _objConn"
                .Connection = _objConn
                ErrorStep = "Step 2 .ExecuteReader()"
                .CommandTimeout = 50 'วันที่เพิ่ม 11/ต.ค./2016 โดยสิทธิ์ราช เนื่องจากต้องปรับ Timeout ให้เยอะขึ้น เพราะว่ามันเตือน TimeOutExpire
                .ExecuteReader()
            End With
            Return Cmd
        Catch ex As Exception
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5} {6}", vbCrLf, Me.ToString(), vbCrLf, _
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message, ErrorStep))
        Finally
            'Me.CloseConnection()
        End Try
    End Function

    ''' <summary>
    ''' Use in Insert , update Or Delete Data with SQL Command
    ''' </summary>
    ''' <remarks></remarks>
    Public Function ExecuteData(ByVal sql As String) As String
        Try
            ExecuteData = ""
            'Me.OpenConnection()
            _objCmd = New SqlCommand
            With _objCmd
                .CommandType = CommandType.Text
                .CommandText = sql
                .Connection = _objConn
                .CommandTimeout = 50 'วันที่เพิ่ม 11/ต.ค./2016 โดยสิทธิ์ราช เนื่องจากต้องปรับ Timeout ให้เยอะขึ้น เพราะว่ามันเตือน TimeOutExpire
                .ExecuteNonQuery()
            End With
            ExecuteData = ""
        Catch ex As Exception
            ExecuteData = ex.ToString()
            Dim CurrentStack As New StackTrace()
            Throw New Exception(String.Format("{0}Class : {1}{2}Function : {3}{4}Error : {5}", vbCrLf, Me.ToString(), vbCrLf, _
                      CurrentStack.GetFrame(0).GetMethod().Name, vbCrLf, ex.Message))
        Finally
            'Me.CloseConnection()
        End Try
    End Function




    '    ' IDisposable
    '    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
    '        If Not Me.disposedValue Then
    '            If disposing Then
    '                ' TODO: free other state (managed objects).
    '            End If

    '            ' TODO: free your own state (unmanaged objects).
    '            ' TODO: set large fields to null.
    '        End If
    '        Me.disposedValue = True
    '    End Sub

#Region " IDisposable Support "
    '    ' This code added by Visual Basic to correctly implement the disposable pattern.
    '    Public Sub Dispose() Implements IDisposable.Dispose
    '        Me.CloseConnection()
    '        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '        Dispose(True)
    '        GC.SuppressFinalize(Me)
    '    End Sub

    Public Sub Dispose() Implements System.IDisposable.Dispose

        GC.SuppressFinalize(Me)
    End Sub
#End Region


End Class
