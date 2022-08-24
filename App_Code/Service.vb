Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization
Imports System.Diagnostics
Imports System
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports Newtonsoft.Json
'
' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
'<WebService(Namespace:="http://tempuri.org/")> _
'<WebService(Namespace:="http://58.181.171.24/Webservice/")>

<WebService(Namespace:="localhost/webservice/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.ComponentModel.ToolboxItem(False)> _
<System.Web.Script.Services.ScriptService()> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Service
    Inherits System.Web.Services.WebService

    Private objCurrentVar As clsCurrentVar
    Private objUtilities As clsUtilities
    Private objDBUtilities As clsDBUtilities

    ' Start for Barcode Systerm

    ''' <summary>
    ''' Mobile_CheckLogin
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strUserID">strOption</param>
    ''' <param name="strPassword">strOption</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_CheckLogin(ByVal strDataBaseName As String, ByVal strUserID As String, ByVal strPassword As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_CheckLogin '{0}','{1}'", Me.objUtilities.CheckQuoteSQL(strUserID), Me.objUtilities.CheckQuoteSQL(strPassword))

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            'Dim ds As New DataSet
            'ds.DataSetName = "PneumaxDB"
            'dt.TableName = "Staff"
            'ds.Tables.Add(dt)
            'Dim simpleresult As String = ds.GetXml
            'Return simpleresult
            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_CheckLogin",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strField">strField</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetDataSpinner(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.AppendFormat("select	Distinct {0} as ResultReturn ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetDataSpinner",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_SummaryHandHeldOperate
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strOption">strOption</param>
    ''' <param name="strWorkType">strWorkType </param>
    ''' <param name="strDeviceName">strDeviceName</param>
    ''' <param name="strFromDate">strFromDate</param>
    ''' <param name="strToDate">strToDate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_SummaryHandHeldOperate(ByVal strDataBaseName As String, ByVal strOption As String, ByVal strWorkType As String,
                                                  ByVal strDeviceName As String, ByVal strFromDate As String, ByVal strToDate As String) As String
        Dim objResult As New clsResultSQL()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("exec Mobile_SummaryHandHeldOperate '{0}','{1}','{2}','{3}','{4}'",
                                   strOption, strWorkType, strDeviceName, strFromDate, strToDate).AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "Mobile_SummaryHandHeldOperate"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_SummaryRFCheckOut
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strOption">strOption</param>
    ''' <param name="strWorkType">strWorkType </param>
    ''' <param name="strStfcode">strStfcode</param>
    ''' <param name="strFromDate">strFromDate</param>
    ''' <param name="strToDate">strToDate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_SummaryRFCheckOut(ByVal strDataBaseName As String, ByVal strOption As String, ByVal strWorkType As String,
                                                  ByVal strStfcode As String, ByVal strFromDate As String, ByVal strToDate As String) As String
        Dim objResult As New clsResultSQL()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("exec Mobile_SummaryRFCheckOut '{0}','{1}','{2}','{3}','{4}'",
                                   strOption, strWorkType, strStfcode, strFromDate, strToDate).AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "Mobile_SummaryRFCheckOut"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_SummaryRFCheckIn
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strOption">strOption</param>
    ''' <param name="strWorkType">strWorkType </param>
    ''' <param name="strStfcode">strStfcode</param>
    ''' <param name="strFromDate">strFromDate</param>
    ''' <param name="strToDate">strToDate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_SummaryRFCheckIn(ByVal strDataBaseName As String, ByVal strOption As String, ByVal strWorkType As String,
                                                  ByVal strStfcode As String, ByVal strFromDate As String, ByVal strToDate As String) As String
        Dim objResult As New clsResultSQL()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("exec Mobile_SummaryRFCheckIn '{0}','{1}','{2}','{3}','{4}'",
                                   strOption, strWorkType, strStfcode, strFromDate, strToDate).AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "Mobile_SummaryRFCheckIn"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strPartnid">strPartnid</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetProductDetail(ByVal strDataBaseName As String, ByVal strPartnid As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetProductDetail '{0}'", Me.objUtilities.CheckQuoteSQL(strPartnid))


                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetProductDetail",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strOption">strOption</param>
    ''' <param name="strStfcode">strStfcode</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ListDocument(ByVal strDataBaseName As String, ByVal strOption As String, ByVal strStfcode As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())


                strBD.Length = 0
                strBD.AppendFormat("Mobile_ListDocument '{0}','{1}'", strOption, strStfcode)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_ListDocument" + strOption,
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' สำหรับดึงข้อมูลรายการสินค้าที่ต้องจัดเก็บ
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' ''' 
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ListDoctypeReceive(ByVal strDataBaseName As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())


                strBD.Length = 0
                strBD.AppendFormat("Mobile_ListDoctypeReceive")

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_ListDoctypeReceive",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDoctype">strOption</param>
    ''' <param name="strStfcode">strStfcode</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ListDocument_Receive(ByVal strDataBaseName As String, ByVal strDoctype As String, ByVal strStfcode As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())


                strBD.Length = 0
                strBD.AppendFormat("Mobile_ListDocument_Receive '{0}','{1}'", strDoctype, strStfcode)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_ListDocument_Receive" + strDoctype,
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDoctype">strOption</param>
    ''' <param name="strStfcode">strStfcode</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ListDocument_Store(ByVal strDataBaseName As String, ByVal strDoctype As String, ByVal strStfcode As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())


                strBD.Length = 0
                strBD.AppendFormat("Mobile_ListDocument_Store '{0}','{1}'", strDoctype, strStfcode)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_ListDocument_Store" + strDoctype,
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDoctype">strOption</param>
    ''' <param name="strStfcode">strStfcode</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ListDocument_Store_Parttube(ByVal strDataBaseName As String, ByVal strDoctype As String, ByVal strStfcode As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())


                strBD.Length = 0
                strBD.AppendFormat("Mobile_ListDocument_Store_Parttube '{0}','{1}'", strDoctype, strStfcode)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_ListDocument_Store_Parttube" + strDoctype,
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' สำหรับดึงข้อมูลรายการสินค้าที่ต้องจัดเก็บ
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' ''' 
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ListDoctypeStore(ByVal strDataBaseName As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())


                strBD.Length = 0
                strBD.AppendFormat("Mobile_ListDoctypeStore")

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_ListDoctypeStore",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strField">strField</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ReturnValue(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                strBD.AppendFormat("select Distinct	{0} as ResultReturn ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Mobile_ReturnValue",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strField1">strField1</param>
    ''' <param name="strField2">strField2</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ReturnTwoValue(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField1 As String, ByVal strField2 As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                strBD.AppendFormat("select	{0} as ResultReturnValue1 , {1} as ResultReturnValue2", strField1, strField2).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Mobile_ReturnTwoValue",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strField">strField</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_MaxValue(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.AppendFormat("select	isnull(max({0}),'') as ResultReturn ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                    If (dt.Rows.Count > 0) Then

                    End If
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Mobile_MaxValue",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strField">strField</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_MinValue(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.AppendFormat("select	isnull(Min({0}),'') as ResultReturn ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Mobile_MinValue",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strField">strField</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_SumValue(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.AppendFormat("select	isnull(Sum({0}),0) as ResultReturn ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Mobile_SumValue",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับ คืนค่าข้อมูลตาม พารามิเตอร์ ที่ส่งมา
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_CountRecord(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strCondition As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.AppendFormat("select	count(*) as ResultReturn ").AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                strBD.AppendFormat("where   {0}", strCondition)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                    If (dt.Rows.Count > 0) Then

                    End If
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Mobile_CountRecord",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับตรวจสอบเลขที่เอกสารว่ามีหรือไม่
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strTableName">strDocno</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_FN_GetRunno(ByVal strDataBaseName As String, ByVal strTableName As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.PneumaxDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                strBD.AppendFormat("select dbo.FN_Mobile_GetRunno('{0}') as Runno ", strTableName).AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_FN_GetRunno",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' spClearDocumentError
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strOptionType">P-Picking R-Receive  S-Sotre </param>
    ''' <param name="strOptionClear">ทั้งหมดหรือเฉพาะ part ที่ยังไม่เสร็จ </param>
    ''' <param name="strDocNo">เลขที่เอกสารที่ต้องการ Clear เพื่อทำกาจัดใหม่</param>
    ''' <param name="strSTfCode">รหัสพนักงานที่ทำการ Clear เอกสาร</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ClearDocNoError(ByVal strDataBaseName As String, ByVal strOptionType As String, ByVal strOptionClear As String, ByVal strDocNo As String, ByVal strSTfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("exec Mobile_ClearDocNoError '{0}','{1}','{2}','{3}',{4}", strDocNo, strOptionType, strOptionClear, strSTfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' ส่งคืนประเภทเอกสาร
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDocno">strDocno</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetDoctype(ByVal strDataBaseName As String, ByVal strDocno As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetDoctype '{0}' ", Me.objUtilities.CheckQuoteSQL(strDocno))

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "FN_Mobile_GetDoctype",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDocno">strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetPickPartDetail(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strPartnid As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetPickPartDetail '{0}','{1}'", Me.objUtilities.CheckQuoteSQL(strDocno), Me.objUtilities.CheckQuoteSQL(strPartnid))


                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetPickPartDetail",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' Mobile_Picking_Hold_Reset  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo"> strDocNo</param>
    ''' <param name="strPartnid"> strPartnid</param>
    ''' <param name="strOption"> strOption</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Picking_Hold_Reset(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strPartnid As String, ByVal strOption As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Picking_Hold_Reset '{0}','{1}','{2}','{3}',{4}", strDocNo, strPartnid, strOption, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Picking_Jobtube_Hold_Reset  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo"> strDocNo</param>
    ''' <param name="strPartnid"> strPartnid</param>
    ''' <param name="strOption"> strDocNo</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Picking_Jobtube_Hold_Reset(ByVal strDataBaseName As String,
                                                      ByVal strDocNo As String,
                                                      ByVal strPartnid As String,
                                                      ByVal strOption As String,
                                                      ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Picking_Jobtube_Hold_Reset '{0}','{1}','{2}','{3}',{4}", strDocNo, strPartnid, strOption, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strKind">strKind</param>
    ''' <param name="strQty">strQty</param>
    ''' <summary>
    ''' Mobile_UPD_Tmp_RFCheckOut  
    ''' </summary>
    ''' 
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_UPD_Tmp_RFCheckOut(ByVal strDataBaseName As String,
                                             ByVal strDocno As String,
                                             ByVal strPartnid As String,
                                             ByVal strKind As String,
                                             ByVal strQty As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_UPD_Tmp_RFCheckOut '{0}','{1}','{2}',{3},{4}",
                                   strDocno, strPartnid, strKind, strQty, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strDigitno">strDigitno</param>
    ''' <param name="strQty">strQty</param>
    ''' <param name="strDstbQty">strDstbQty</param>''' 
    ''' <param name="strKind">strKind</param>
    ''' <param name="strQtyCut">strQtyCut</param>
    ''' <summary>
    ''' Mobile_UPD_Tmp_RFCheckOut_Jobtube  
    ''' </summary>
    ''' 
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_UPD_Tmp_RFCheckOut_Jobtube(ByVal strDataBaseName As String,
                                             ByVal strDocno As String,
                                             ByVal strPartnid As String,
                                             ByVal strDigitno As String,
                                             ByVal strQty As String,
                                             ByVal strDstbQty As String,
                                             ByVal strKind As String,
                                             ByVal strQtyCut As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_UPD_Tmp_RFCheckOut_Jobtube '{0}','{1}','{2}',{3},{4},'{5}',{6},{7}",
                                   strDocno, strPartnid, strDigitno, strQty, strDstbQty, strKind, strQtyCut, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function

    ''' <summary>
    ''' Mobile_Clear_Tmp_RFCheckOut  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Clear_Tmp_RFCheckOut(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strPartnid As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Clear_Tmp_RFCheckOut '{0}','{1}','{2}',{3}", strDocNo, strPartnid, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function





    ''' <summary>
    ''' Mobile_Clear_Tmp_RFCheckOut_Jobtube  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strDigitno">strDigitno</param>
    ''' <param name="strQty">strQty</param>
    ''' <param name="strDstbQty">strDstbQty</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Clear_Tmp_RFCheckOut_Jobtube(ByVal strDataBaseName As String,
                                                        ByVal strDocNo As String,
                                                        ByVal strPartnid As String,
                                                        ByVal strDigitno As String,
                                                        ByVal strQty As String,
                                                        ByVal strDstbQty As String,
                                                        ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Clear_Tmp_RFCheckOut_Jobtube '{0}','{1}','{2}',{3},{4},'{5}',{6}",
                                                                         strDocNo, strPartnid, strDigitno, strQty, strDstbQty, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Insert_RFCheckOut_PartSerialNo 
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strSerialno">strSerialno</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Insert_RFCheckOut_PartSerialNo(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strPartnid As String, ByVal strSerialno As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Insert_RFCheckOut_PartSerialNo'{0}','{1}','{2}',{3}", strDocNo, strPartnid, strSerialno, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strDigitno">strDigitno</param>
    ''' 
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetPartTubeDetail(ByVal strDataBaseName As String, ByVal strPartnid As String, ByVal strDigitno As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetPartTubeDetail '{0}','{1}'", Me.objUtilities.CheckQuoteSQL(strPartnid), Me.objUtilities.CheckQuoteSQL(strDigitno))


                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetPartTubeDetail",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDocno">strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strDigitno">strDigitno</param>
    ''' 
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetPickPart_JobtubeDetail(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strPartnid As String, ByVal strDigitno As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetPickPart_JobtubeDetail '{0}','{1}','{2}'", Me.objUtilities.CheckQuoteSQL(strDocno), Me.objUtilities.CheckQuoteSQL(strPartnid), Me.objUtilities.CheckQuoteSQL(strDigitno))


                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetPickPart_JobtubeDetail",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Insert_Tmp_RFPartTube 
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strDigitno">strDigitno</param>
    ''' <param name="strQty">strDigitno</param>
    ''' <param name="strDstbQty">strDigitno</param>
    ''' <param name="strDigitnoCut">strDigitnoCut</param>
    ''' <param name="strCutLength">strCutLength</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Insert_Tmp_RFPartTube(ByVal strDataBaseName As String,
                                                 ByVal strDocNo As String,
                                                 ByVal strPartnid As String,
                                                 ByVal strDigitno As String,
                                                 ByVal strQty As String,
                                                 ByVal strDstbQty As String,
                                                 ByVal strDigitnoCut As String,
                                                 ByVal strCutLength As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Insert_Tmp_RFPartTube'{0}','{1}','{2}',{3},{4},'{5}',{6},{7}",
                                                                strDocNo, strPartnid, strDigitno, strQty, strDstbQty, strDigitnoCut, strCutLength, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_ConfirmDocument  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">เลขที่เอกสารที่ต้องการ Confirm</param>
    ''' <param name="strStfCode">รหัสพนักงานที่ทำการ Confirm</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_ConfirmDocument(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_ConfirmDocument '{0}','{1}',{2}", strDocNo, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Update_RFCheckOut_PartSerialNo  
    ''' </summary>
    ''' <param name="strDataBaseName">strDataBaseName</param>
    ''' <param name="strDocNo">เลขที่เอกสารที่ต้องการ Delivery</param>
    ''' <param name="strSerialno">strSerialno</param>
    ''' <param name="strDeliveryCode">strDeliveryCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Update_RFCheckOut_PartSerialNo(ByVal strDataBaseName As String, ByVal strDocNo As String,
                                                          ByVal strSerialno As String, ByVal strDeliveryCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Update_RFCheckOut_PartSerialNo '{0}','{1}','{2}',{3}", strDocNo, strSerialno, strDeliveryCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' Mobile_DeliveryDocument  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">เลขที่เอกสารที่ต้องการ Delivery</param>
    ''' <param name="strDeliveryCode">รหัสพนักงานที่ทำการ Delivery</param>
    ''' <param name="strReceiveCode">รหัสพนักงานที่ทำการ Receive</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_DeliveryDocument(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strDeliveryCode As String, ByVal strReceiveCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_DeliveryDocument '{0}','{1}','{2}',{3}", strDocNo, strDeliveryCode, strReceiveCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function





    ''' <summary>
    ''' Mobile_Update_ProductExWH  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">เลขที่ Packing</param>
    ''' <param name="strStfCode">รหัสพนักงาน</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Update_ProductExWH(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Update_ProductExWH '{0}','{1}',{2}", strDocno, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Cancel_ProductExWH  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">เลขที่ Packing</param>
    ''' <param name="strStfCode">รหัสพนักงาน</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Cancel_ProductExWH(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Cancel_ProductExWH '{0}','{1}',{2}", strDocno, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Update_ProductChecked  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">เลขที่ Packing</param>
    ''' <param name="strStfCode">รหัสพนักงาน</param>
    ''' <param name="strOption">strOption</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Update_ProductChecked(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strStfCode As String, ByVal strOption As String, ByVal strPartnid As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Update_ProductChecked '{0}','{1}','{2}','{3}',{4}", strDocno, strStfCode, strOption, strPartnid, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' Mobile_GetPhysicalCount
    ''' ใช้ร่วมกัน GenNo Weekno Issue Period Location
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTableName">Table Name</param>
    ''' <param name="strField">Field List</param>
    ''' <param name="strCondition">Condition</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetPhysicalCount(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        Try
            clsCurrentVar.DataBaseName = strDataBaseName
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                strBD.AppendFormat("select	distinct {0} ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                If strCondition <> "" Then
                    strBD.AppendFormat(" where  {0} ", strCondition).AppendLine()
                End If

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetPhysicalCount",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_GetListPart
    ''' ใช้ร่วมกัน GenNo Weekno Issue Period Location
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTableName">Table Name</param>
    ''' <param name="strField">Field List</param>
    ''' <param name="strCondition">Condition</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetListPart(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strField As String, ByVal strCondition As String) As String
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                strBD.AppendFormat("select	distinct {0} ", strField).AppendLine()
                strBD.AppendFormat("from	{0} ", strTableName).AppendLine()
                If strCondition <> "" Then
                    strBD.AppendFormat(" where  {0} ", strCondition).AppendLine()
                End If

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using
            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetListPart",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Insert_Tmp_RFPhysicalCount  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strSTfCode">รหัสพนักงานที่ทำการตรวจนับ</param>
    ''' <param name="strGenNo">strGenNo</param>
    ''' <param name="strWeekNo">strWeekNo</param>
    ''' <param name="strLCCode">strLCCode</param>
    ''' <param name="strPartNid">strPartNid</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Insert_Tmp_RFPhysicalCount(ByVal strDataBaseName As String, ByVal strTmpTime As String, ByVal strSTfCode As String, ByVal strGenNo As String, ByVal strWeekNo As String, ByVal strLCCode As String, ByVal strPartNid As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("exec Mobile_Insert_Tmp_RFPhysicalCount '{0}','{1}','{2}',{3},'{4}','{5}',{6}", strTmpTime, strSTfCode, strGenNo, strWeekNo, strLCCode, strPartNid, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using
            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Get_Tmp_RFPhysicalCount  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strSTfCode">รหัสพนักงานที่ทำการตรวจนับ</param>
    ''' <param name="strGenNo">strGenNo</param>
    ''' <param name="strWeekNo">strWeekNo</param>
    ''' <param name="strLCCode">strLCCode</param>
    ''' <param name="strPartNid">strPartNid</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Get_Tmp_RFPhysicalCount(ByVal strDataBaseName As String, ByVal strTmpTime As String, ByVal strSTfCode As String, ByVal strGenNo As String, ByVal strWeekNo As String, ByVal strLCCode As String, ByVal strPartNid As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("exec Mobile_Get_Tmp_RFPhysicalCount '{0}','{1}','{2}',{3},'{4}','{5}',{6}", strTmpTime, strSTfCode, strGenNo, strWeekNo, strLCCode, strPartNid, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using
            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' Mobile_Insert_Tmp_RFPartDigitNo  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartDigitNo">strPartDigitno</param>
    ''' <param name="strPartNid">strPartNid</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Insert_Tmp_RFPartDigitNo(ByVal strDataBaseName As String,
                                               ByVal strTmpTime As String,
                                               ByVal strDocNo As String,
                                               ByVal strPartDigitNo As String,
                                               ByVal strPartNid As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Insert_Tmp_RFPartDigitNo '{0}','{1}','{2}','{3}',{4}",
                strTmpTime, strDocNo, strPartDigitNo, strPartNid, "''").AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Confirm_PickPart  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartNid">strPartNid</param>
    ''' <param name="strStfcode">strStfcode</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Confirm_PickPart(ByVal strDataBaseName As String,
                                               ByVal strTmpTime As String,
                                               ByVal strDocNo As String,
                                               ByVal strPartNid As String,
                                               ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Confirm_PickPart '{0}','{1}','{2}','{3}',{4}",
                strTmpTime, strDocNo, strPartNid, strStfcode, "''").AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Confirm_PickPart_Jobtube  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartNid">strPartNid</param>
    ''' <param name="strDigitno">strPartNid</param>
    ''' <param name="strQty">strPartNid</param>
    ''' <param name="strDstbQty">strPartNid</param>
    ''' <param name="strStfcode">strStfcode</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Confirm_PickPart_Jobtube(ByVal strDataBaseName As String,
                                               ByVal strTmpTime As String,
                                               ByVal strDocNo As String,
                                               ByVal strPartNid As String,
                                               ByVal strDigitno As String,
                                               ByVal strQty As String,
                                               ByVal strDstbQty As String,
                                               ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Confirm_PickPart_Jobtube '{0}','{1}','{2}','{3}','{4}',{5},{6},{7}",
                strTmpTime, strDocNo, strPartNid, strDigitno, strQty, strDstbQty, strStfcode, "''").AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' Mobile_Update_Tmp_RFPhysicalCount  
    ''' </summary>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Update_Tmp_RFPhysicalCount(ByVal strDataBaseName As String,
                                           ByVal strTmpTime As String,
                                           ByVal strPartNid As String,
                                           ByVal strQty As String,
                                           ByVal strDF As String,
                                           ByVal strSDM As String,
                                           ByVal strDM As String,
                                           ByVal strCardQty As String,
                                           ByVal strCardDF As String,
                                           ByVal strCardSDM As String,
                                           ByVal strCardDM As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Update_Tmp_RFPhysicalCount '{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                                   strTmpTime, strPartNid, strQty, strDF, strSDM, strDM, strCardQty, strCardDF, strCardSDM, strCardDM, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_Update_PhysicalCount  
    ''' </summary>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Update_PhysicalCount(ByVal strDataBaseName As String,
                                           ByVal strTmpTime As String,
                                           ByVal strPartNid As String,
                                           ByVal strQty As String,
                                           ByVal strDF As String,
                                           ByVal strSDM As String,
                                           ByVal strDM As String,
                                           ByVal strCardQty As String,
                                           ByVal strCardDF As String,
                                           ByVal strCardSDM As String,
                                           ByVal strCardDM As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Update_PhysicalCount '{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                                   strTmpTime, strPartNid, strQty, strDF, strSDM, strDM, strCardQty, strCardDF, strCardSDM, strCardDM, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_DeleteRecord  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTableName">strTableName</param>
    ''' <param name="strCondition">strCondition</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_DeleteRecord(ByVal strDataBaseName As String, ByVal strTableName As String, ByVal strCondition As String) As String
        Dim objResult As New clsResultSQL()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()
                'ให้ทำการลบรายการที่ Scan ค้างไว้ออกด้วย
                If strTableName.ToUpper.Trim = "TMP_RFPHYSICALCOUNT" Then
                    StrBD.Length = 0
                    StrBD.AppendFormat("Delete From Tmp_RFPartDigitNo where {0}", strCondition).AppendLine()
                    objCon.OpenConnection()
                    Try
                        objCon.ExecuteData(StrBD.ToString())
                    Catch ex As Exception
                        objResult.ResultID = "UnSuccess"
                        objResult.ResultMessage = ex.Message
                        Return JsonConvert.SerializeObject(objResult)
                    End Try
                End If

                StrBD.Length = 0
                StrBD.AppendFormat("Delete From {0} where {1}", strTableName, strCondition).AppendLine()
                objCon.OpenConnection()
                Try
                    objCon.ExecuteData(StrBD.ToString())
                Catch ex As Exception
                    objResult.ResultID = "UnSuccess"
                    objResult.ResultMessage = ex.Message
                    Return JsonConvert.SerializeObject(objResult)
                Finally
                    objCon.CloseConnection()
                End Try

                objResult.ResultID = "Success"
                objResult.ResultMessage = "Delete data from: " + strTableName + " Complete"
                Return JsonConvert.SerializeObject(objResult)
            End Using
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function





    ''' <summary>
    ''' Mobile_Crt_Tmp_RFCheckOut_Picking  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo">เลขที่เอกสาร</param>
    ''' <param name="strSTfCode">รหัสพนักงานที่ทำการตรวจนับ</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Crt_Tmp_RFCheckOut_Picking(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strSTfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Crt_Tmp_RFCheckOut_Picking '{0}','{1}',{2}", strDocNo, strSTfCode, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function






    ''' <summary>
    ''' spExecute  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strForExecute">strExecute</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function spExecute(ByVal strDataBaseName As String, ByVal strForExecute As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat(strForExecute).AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' Mobile_INS_Tmp_RFCheckIn  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">strDocNo</param>
    ''' <param name="strStfcode">strSTfCode</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_INS_Tmp_RFCheckIn(ByVal strDataBaseName As String,
                                             ByVal strDocno As String,
                                             ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_INS_Tmp_RFCheckIn '{0}','{1}',{2}", strDocno, strStfcode, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' Mobile_INS_Tmp_RFCheckIn_Parttube  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">strDocNo</param>
    ''' <param name="strStfcode">strSTfCode</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_INS_Tmp_RFCheckIn_Parttube(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_INS_Tmp_RFCheckIn_Parttube '{0}','{1}',{2}", strDocno, strStfcode, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function






    ''' <summary>
    ''' Mobile_INS_Tmp_RFCheckIn_ChkDigit  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">strDocNo</param>
    ''' <param name="strStfcode">strSTfCode</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strParttubestock">strParttubestock</param>
    ''' <param name="strKind">strKind</param>
    ''' <param name="strDigit">strDigit</param>
    ''' <param name="strQty">strQty</param>
    ''' 


    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_INS_Tmp_RFCheckIn_ChkDigit(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strStfcode As String,
                                                      ByVal strPartnid As String, ByVal strParttubestock As String, ByVal strKind As String, ByVal strDigit As String,
                                                      ByVal strQty As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_INS_Tmp_RFCheckIn_ChkDigit '{0}','{1}','{2}','{3}','{4}','{5}',{6},{7}", strDocno, strStfcode, strPartnid, strParttubestock, strKind, strDigit, strQty, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function




    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDocno">strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetReceivePartDetail(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strPartnid As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetReceivePartDetail '{0}','{1}'", Me.objUtilities.CheckQuoteSQL(strDocno), Me.objUtilities.CheckQuoteSQL(strPartnid))
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Mobile_GetReceivePartDetail",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function






    ''' <summary>
    ''' Mobile_Confirm_ReceivePart  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartNid">strPartNid</param>
    ''' <param name="strStfcode">strStfcode</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Confirm_ReceivePart(ByVal strDataBaseName As String,
                                               ByVal strTmpTime As String,
                                               ByVal strDocNo As String,
                                               ByVal strPartNid As String,
                                               ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Confirm_ReceivePart '{0}','{1}','{2}','{3}',{4}",
                strTmpTime, strDocNo, strPartNid, strStfcode, "''").AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using
            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Receive_Hold_Reset  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno"> strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strOption"> strDocNo</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Receive_Hold_Reset(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strPartnid As String, ByVal strOption As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Receive_Hold_Reset '{0}','{1}','{2}','{3}',{4}", strDocno, strPartnid, strOption, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Confirm_StorePart  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strTmpTime">Temtime</param>
    ''' <param name="strDocNo">strDocNo</param>
    ''' <param name="strPartNid">strPartNid</param>
    ''' <param name="strStfcode">strStfcode</param>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Confirm_StorePart(ByVal strDataBaseName As String,
                                               ByVal strTmpTime As String,
                                               ByVal strDocNo As String,
                                               ByVal strPartNid As String,
                                               ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Confirm_StorePart '{0}','{1}','{2}','{3}',{4}",
                strTmpTime, strDocNo, strPartNid, strStfcode, "''").AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDocno">strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetStorePartDetail(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strPartnid As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetStorePartDetail '{0}','{1}'", Me.objUtilities.CheckQuoteSQL(strDocno), Me.objUtilities.CheckQuoteSQL(strPartnid))


                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "UNSUCCESS",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Store_Hold_Reset  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo"> strDocNo</param>
    ''' <param name="strPartnid"> strPartnid</param>
    ''' <param name="strOption"> strDocNo</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Store_Hold_Reset(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strPartnid As String, ByVal strOption As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Store_Hold_Reset '{0}','{1}','{2}','{3}',{4}", strDocNo, strPartnid, strOption, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' สำหรับดึงข้อมูลรายละเอียดสินค้า
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>''' 
    ''' <param name="strDocno">strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_GetStorePartTubeDetail(ByVal strDataBaseName As String, ByVal strDocno As String, ByVal strPartnid As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = strDataBaseName 'clsCurrentVar.DATATEST
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())

                strBD.Length = 0
                strBD.AppendFormat("Mobile_GetStorePartTubeDetail '{0}','{1}'", Me.objUtilities.CheckQuoteSQL(strDocno), Me.objUtilities.CheckQuoteSQL(strPartnid))


                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using


            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "UNSUCCESS",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Store_Parttube_Hold_Reset  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocNo"> strDocNo</param>
    ''' <param name="strOption"> strDocNo</param>
    ''' <param name="strPartnid"> strDocNo</param>
    ''' <param name="strStfCode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Store_Parttube_Hold_Reset(ByVal strDataBaseName As String, ByVal strDocNo As String, ByVal strOption As String, ByVal strPartnid As String, ByVal strStfCode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Store_Parttube_Hold_Reset '{0}','{1}','{2}','{3}',{4}", strDocNo, strOption, strPartnid, strStfCode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function

    ''' <summary>
    ''' Mobile_Clear_Tmp_RFCheckIn  
    ''' </summary>
    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strStatus">RECEIVE Or STORE</param>
    ''' <param name="strDocno">strDocNo</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strStfcode">strStfCode</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Clear_Tmp_RFCheckIn(ByVal strDataBaseName As String, ByVal strStatus As String, ByVal strDocno As String, ByVal strPartnid As String, ByVal strStfcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()


                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Clear_Tmp_RFCheckIn '{0}','{1}','{2}','{3}',{4}", strStatus, strDocno, strPartnid, strStfcode, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            objResult.ResultID = "UNSUCCESS"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function



    ''' <summary>
    ''' Mobile_INS_Tmp_RFCheckIn_ChkDigit_PartTube  
    ''' </summary>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_INS_Tmp_RFCheckIn_ChkDigit_PartTube(ByVal strDataBaseName As String,
                                           ByVal strTmpTime As String,
                                           ByVal strRCVno As String,
                                           ByVal strSTFCode As String,
                                           ByVal strPartnid As String,
                                           ByVal strPartTubeStock As String,
                                           ByVal strKind As String,
                                           ByVal strDigit As String,
                                           ByVal strFullLength As String,
                                           ByVal strDamageLength As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_INS_Tmp_RFCheckIn_ChkDigit_PartTube '{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9}",
                                   strTmpTime, strRCVno, strSTFCode, strPartnid, strPartTubeStock, strKind, strDigit, strFullLength, strDamageLength, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function

    ''' <param name="strDataBaseName">ชื่อ DataBase</param>
    ''' <param name="strDocno">strDocno</param>
    ''' <param name="strPartnid">strPartnid</param>
    ''' <param name="strKind">strKind</param>
    ''' <param name="strDigit">strDigit</param>
    ''' <param name="strQty">strQty</param>
    ''' <param name="strStatus">strStatus</param>
    ''' <param name="strSTFcode">strSTFcode</param>
    ''' <summary>
    ''' Mobile_UPD_Tmp_RFCheckIn  
    ''' </summary>
    ''' 
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_UPD_Tmp_RFCheckIn(ByVal strDataBaseName As String,
                                             ByVal strDocno As String,
                                             ByVal strPartnid As String,
                                             ByVal strKind As String,
                                             ByVal strDigit As String,
                                             ByVal strQty As String,
                                             ByVal strStatus As String,
                                             ByVal strSTFcode As String
                                             ) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_UPD_Tmp_RFCheckIn '{0}','{1}','{2}','{3}',{4},'{5}','{6}',{7}",
                                   strDocno, strPartnid, strKind, strDigit, strQty, strStatus, strSTFcode, "''")
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function


    ''' <summary>
    ''' Mobile_Upd_Tmp_RFCheckIn_PartTube  
    ''' </summary>

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function Mobile_Upd_Tmp_RFCheckIn_PartTube(ByVal strDataBaseName As String,
                                           ByVal strTmpTime As String,
                                           ByVal strRCVno As String,
                                           ByVal strSTFCode As String,
                                           ByVal strPartnid As String,
                                           ByVal strQtyIn As String,
                                           ByVal strQtyDFIn As String,
                                           ByVal strQtyDmIn As String) As String
        Dim objResult As New clsResultSQL()
        Try
            Dim dt As New DataTable
            clsCurrentVar.DataBaseName = strDataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim StrBD As New StringBuilder()

                StrBD.Length = 0
                StrBD.AppendFormat("Mobile_Upd_Tmp_RFCheckIn_PartTube '{0}','{1}','{2}','{3}',{4},{5},{6},{7}",
                                   strTmpTime, strRCVno, strSTFCode, strPartnid, strQtyIn, strQtyDFIn, strQtyDmIn, "''").AppendLine()
                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(StrBD.ToString())
                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function
    ' END for Barcode Systerm



End Class
