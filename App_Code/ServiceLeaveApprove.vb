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
Imports System.Net.Mail

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
'<WebService(Namespace:="http://tempuri.org/")> _
<WebService(Namespace:="http://58.181.171.24/Webservice/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.ComponentModel.ToolboxItem(False)>
<System.Web.Script.Services.ScriptService()>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class ServiceLeaveApprove
    Inherits System.Web.Services.WebService

    Private objCurrentVar As clsCurrentVar
    Private objUtilities As clsUtilities
    Private objDBUtilities As clsDBUtilities


    ''' <summary>
    ''' สำหรับ Check Login
    ''' </summary>
    ''' <param name="UserName">UserName</param>
    ''' <param name="Password">Password</param>
    ''' <param name="VersionApp">Version ของ Application</param>
    ''' <param name="MacAddressdevice">MacAddress ของ Device</param>
    ''' <param name="OSDevice">ชื่อระบบปฏิบัติการ ของ Device</param>
    ''' <param name="ModelDevice">Model ของ Device</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getLogin(ByVal UserName As String, ByVal Password As String, ByVal VersionApp As String,
                                ByVal MacAddressdevice As String, ByVal OSdevice As String, ByVal Branddevice As String, ByVal Modeldevice As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Dim strBD As New StringBuilder


            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                strBD.AppendFormat("select	Top 1 t1.STFcode,t1.STFtitle,t1.DPcode,t2.DPname,t4.PSTdes_Eng as PSTdesEng,t1.PSTCode,").AppendLine()
                strBD.AppendFormat("        t1.STFfname,t1.STFlname,t1.STFfullname,t5.BRcode,t5.BRdesc_T as BRdescThai,t1.STFstart").AppendLine()
                strBD.AppendFormat("from	Staff t1").AppendLine()
                strBD.AppendFormat("        inner join Department t2 on t1.DPcode=t2.DPcode").AppendLine()
                strBD.AppendFormat("        inner join Position t4 on t1.PSTcode=t4.PSTcode").AppendLine()
                strBD.AppendFormat("        left outer join Branch t5 on t1.BRcode=t5.BRcode").AppendLine()
                strBD.AppendFormat("where   t1.STFactive='Y'").AppendLine()
                strBD.AppendFormat("and     LEFT(t1.STFcode,2)<>'09' and t1.STFcode<>'1754'").AppendLine()
                strBD.AppendFormat("and		(t1.PSTcode in('03','05','10','11','12','13') Or t1.DPcode='MIS' Or t1.STFcode='2447')").AppendLine()
                strBD.AppendFormat("and     USER_ID='{0}'", Me.objUtilities.CheckQuoteSQL(UserName)).AppendLine()
                strBD.AppendFormat("and     User_pwd='{0}'", Me.objUtilities.CheckQuoteSQL(Password))
                strBD.AppendFormat("order by t1.STFcode asc ").AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                    If (dt IsNot Nothing AndAlso dt.Rows.Count > 0) Then
                        Dim _STFcode As String = dt.Rows(0)("STFcode")
                        strBD.Length = 0
                        strBD.AppendFormat("select  *").AppendLine()
                        strBD.AppendFormat("from    MobileAppPermission").AppendLine()
                        strBD.AppendFormat("where   AppID='02'").AppendLine()
                        strBD.AppendFormat("and     AppMacAddress='{0}'", MacAddressdevice).AppendLine()
                        Dim dtII As New DataTable
                        dtII = objCon.GetDataTable(strBD.ToString())
                        If (dtII IsNot Nothing AndAlso dtII.Rows.Count > 0) Then
                            'ถ้ามีข้อมูล Device แล้ว ให้ตรวจสอบว่าใช้งานได้หรือไม่ โดยใช้ MacAddress ตรวจสอบ
                            If dtII.Rows(0)("AppActive").Trim() <> "Y" Then
                                'ไม่มีสิทธิ์ใช้งานโปรแกรมแล้ว บุคคลอื่นอาจจะใช้ Login ของคนที่ใช้งานก็จะไม่ได้
                                dt.Rows(0)("STFcode") = "UnSuccess"
                            Else
                                'Update Version Application กลับไปให้
                                strBD.Length = 0
                                strBD.AppendFormat("Update  MobileAppPermission").AppendLine()
                                strBD.AppendFormat("set     AppVersion='{0}',", VersionApp).AppendLine()
                                strBD.AppendFormat("        LastUpdate=getdate(),").AppendLine()
                                strBD.AppendFormat("        LastUser='{0}'", _STFcode).AppendLine()
                                strBD.AppendFormat("where   AppID='02'").AppendLine()
                                strBD.AppendFormat("and     STFcode='{0}'", _STFcode).AppendLine()
                                strBD.AppendFormat("and     AppMacAddress='{0}'", MacAddressdevice.Trim())
                                objCon.ExecuteData(strBD.ToString())
                            End If
                        Else
                            'ถ้าไม่มีข้อมูลให้บันทึก ข้อมูล Device ก่อน 
                            strBD.Length = 0
                            strBD.AppendFormat("insert into MobileAppPermission(AppID,STFcode,AppMacAddress,AppOS,AppBrand,AppModel,AppVersion,AppActive,CrtUser,LastUser)").AppendLine()
                            strBD.AppendFormat("values('02','{0}','{1}','{2}','{3}','{4}','{5}','Y','{0}','{0}')",
                                               _STFcode, MacAddressdevice.Trim(), OSdevice.ToUpper().Trim(), Branddevice.Trim(), Modeldevice, VersionApp).AppendLine()
                            objCon.ExecuteData(strBD.ToString())
                        End If
                    End If
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            'Return serializer.Serialize(rows)
            Return JsonConvert.SerializeObject(dt)

        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getLogin",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับ Check Version Pneumax Approve
    ''' </summary>
    ''' <param name="CheckVersion">CheckVersion</param>
    ''' <returns>json</returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getCheckVersion(ByVal CheckVersion As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objUtilities = New clsUtilities
            Me.objCurrentVar = New clsCurrentVar
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                strBD.AppendFormat("select	CVValue").AppendLine()
                strBD.AppendFormat("from	ConstVar t1").AppendLine()
                strBD.AppendFormat("where   CVcode='PNEUMAXAPPROVE'").AppendLine()
                strBD.AppendFormat("and     CVValue<='{0}'", CheckVersion.Trim()).AppendLine()

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Dim objResult As New clsResultSQL()
            If (dt IsNot Nothing) AndAlso (dt.Rows.Count > 0) Then
                objResult.ResultID = "Success"
                objResult.ResultMessage = "Version OK"
            Else
                objResult.ResultID = "UnSuccess"
                objResult.ResultMessage = "กรุณา Update Pneumax Approve ด้วย เนื่องจาก Version ไม่ตรงกัน !!!"
            End If

            'Return serializer.Serialize(objResult)
            Return JsonConvert.SerializeObject(objResult)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getCheckVersion",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get ข้อมูล ผู้บริหารใส่ใน Dropdown
    ''' </summary>
    ''' <param name="STFcode">รหัสพนักงาน</param>
    ''' <param name="DPcode">รหัสแผนก</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getEmployeeManagerDropDown(ByVal STFcode As String, ByVal DPcode As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                'Dim GroupName As String()
                'GroupName = Nothing

                objCon.OpenConnection()
                Try
                    'dt = objCon.GetDataTable(String.Format("spGroupOfUser '{0}','{1}'", STFcode, DPcode))

                    'If dt.Rows.Count > 0 Then
                    '    Dim _GroupName As String = ""
                    '    For i As Integer = 0 To dt.Rows.Count - 1
                    '        If _GroupName = "" Then
                    '            _GroupName = dt.Rows(0)("GPName")
                    '        Else
                    '            _GroupName = String.Format("{0},{1}", _GroupName, dt.Rows(i)("GPName"))
                    '        End If
                    '        GroupName = Split(_GroupName, ",")
                    '    Next
                    'End If

                    If (DPcode.ToUpper() = "MIS") Then
                        strBD.AppendFormat("select	replace(t1.STFcode,' ','') as STFcode,t1.STFfullname as STFname").AppendLine()
                        strBD.AppendFormat("from	Staff t1").AppendLine()
                        strBD.AppendFormat("        inner join Department t2 on t1.DPcode=t2.DPcode").AppendLine()
                        strBD.AppendFormat("        inner join Position t4 on t1.PSTcode=t4.PSTcode").AppendLine()
                        strBD.AppendFormat("        left outer join Branch t5 on t1.BRcode=t5.BRcode").AppendLine()
                        strBD.AppendFormat("where   t1.STFactive='Y'").AppendLine()
                        strBD.AppendFormat("and     LEFT(t1.STFcode,2)<>'09' and t1.STFcode<>'1754'").AppendLine()
                        strBD.AppendFormat("and		(t1.PSTcode in('03','05','10','11','12','13') Or t1.DPcode='MIS' Or t1.STFcode='2447')").AppendLine()
                        strBD.AppendFormat("order by t1.STFcode ").AppendLine()
                    Else
                        strBD.AppendFormat("select	replace(t1.STFcode,' ','') as STFcode,t1.STFfullname as STFname").AppendLine()
                        strBD.AppendFormat("from	Staff t1").AppendLine()
                        strBD.AppendFormat("        inner join Department t2 on t1.DPcode=t2.DPcode").AppendLine()
                        strBD.AppendFormat("        inner join Position t4 on t1.PSTcode=t4.PSTcode").AppendLine()
                        strBD.AppendFormat("        left outer join Branch t5 on t1.BRcode=t5.BRcode").AppendLine()
                        strBD.AppendFormat("where   t1.STFactive='Y'").AppendLine()
                        strBD.AppendFormat("and     LEFT(t1.STFcode,2)<>'09' and t1.STFcode<>'1754'").AppendLine()
                        strBD.AppendFormat("and		(t1.PSTcode in('03','05','10','11','12','13') Or t1.DPcode='MIS' Or t1.STFcode='2447')").AppendLine()
                        strBD.AppendFormat("and     t1.STFcode='{0}' ", STFcode).AppendLine()
                        strBD.AppendFormat("order by t1.STFcode ").AppendLine()
                    End If

                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getEmployeeManagerDropDown",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get แผนกใส่ใน DropDown
    ''' </summary>
    ''' <param name="STFcode">รหัสพนักงาน</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getDepartmentDropDown(ByVal STFcode As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                strBD.AppendFormat("select	distinct replace(t1.DPcode,' ','') DPcode,DPname").AppendLine()
                strBD.AppendFormat("From	Staff t1 ").AppendLine()
                strBD.AppendFormat("		inner join Department t2 on t1.DPcode=t2.DPcode ").AppendLine()
                strBD.AppendFormat("where	t1.STFcode='{0}'", STFcode)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getDepartmentDropDown",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get Manager ระบุรหัสพนักงาน แค่ 1 รายเท่านั้น
    ''' </summary>
    ''' <param name="STFcode">รหัสพนักงานขาย</param>
    ''' <param name="DPcode">รหัสแผนก</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getManagerOneRow(ByVal STFcode As String, ByVal DPcode As String) As String
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                strBD.AppendFormat("select	t1.STFcode,t1.DPcode,t2.DPname,").AppendLine()
                strBD.AppendFormat("        t1.PSTcode,t1.STFfullname,t1.BRcode").AppendLine()
                strBD.AppendFormat("from	Staff t1").AppendLine()
                strBD.AppendFormat("        inner join Department t2 on t1.DPcode=t2.DPcode").AppendLine()
                strBD.AppendFormat("where	t1.STFactive='Y' ").AppendLine()
                strBD.AppendFormat("and		t1.STFcode='{0}'", STFcode).AppendLine()
                strBD.AppendFormat("and		t1.DPcode='{0}'", DPcode)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getManagerOneRow",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get ข้อมูลแผนก ที่ใช้ในการกรองเงื่อนไข
    ''' </summary>
    ''' <param name="STFcode">รหัสพนักงาน</param>
    ''' <param name="DPcode">รหัสแผนก</param>
    ''' <param name="BRcode">รหัสสาขา</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getDepartmentDropDownCriteria(ByVal STFcode As String, ByVal DPcode As String, ByVal BRcode As String) As String
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                Dim GroupName As String()
                GroupName = Nothing

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(String.Format("spGroupOfUser '{0}','{1}'", STFcode, DPcode))

                    If dt.Rows.Count > 0 Then
                        Dim _GroupName As String = ""
                        For i As Integer = 0 To dt.Rows.Count - 1
                            If _GroupName = "" Then
                                _GroupName = dt.Rows(0)("GPName")
                            Else
                                _GroupName = String.Format("{0},{1}", _GroupName, dt.Rows(i)("GPName"))
                            End If
                            GroupName = Split(_GroupName, ",")
                        Next
                    End If

                    strBD.AppendFormat("Select *").AppendLine()
                    strBD.AppendFormat("from  (").AppendLine()
                    If (clsGlobalVar.GroupDirector(GroupName) OrElse clsGlobalVar.GroupMIS(GroupName) OrElse
                        ((clsGlobalVar.GroupManager(GroupName) OrElse clsGlobalVar.GroupSaleManager(GroupName)) AndAlso BRcode.Trim() <> "" AndAlso BRcode.Trim() <> "BKK")) Then
                        strBD.AppendFormat("          Select  'ALL' as DPcode,'ALL' as DPname").AppendLine()
                        strBD.AppendFormat("          union all").AppendLine()
                        strBD.AppendFormat("          Select  DPcode,(rtrim(DPcode)+' ('+DPname+')') as DPname").AppendLine()
                        strBD.AppendFormat("          From    Department").AppendLine()
                        strBD.AppendFormat("          Where   DPSale='Y'").AppendLine()
                        strBD.AppendFormat("          And     DPactive = 1").AppendLine()
                        strBD.AppendFormat("          And	  DPBranch<>'Y'").AppendLine()
                    Else
                        strBD.AppendFormat("          Select  DPcode,(rtrim(DPcode)+' ('+DPname+')') as DPname").AppendLine()
                        strBD.AppendFormat("          From    Department").AppendLine()
                        strBD.AppendFormat("          Where   DPSale='Y'").AppendLine()
                        strBD.AppendFormat("          And     DPactive = 1").AppendLine()
                        strBD.AppendFormat("          And       DPBranch<>'Y'").AppendLine()
                        strBD.AppendFormat("          And		DPcode='{0}'", DPcode).AppendLine()
                    End If
                    strBD.AppendFormat("      )M1").AppendLine()
                    Dim _StrSQL As String = strBD.ToString()
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getDepartmentDropDownCriteria",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get ข้อมูล สาขา ที่ใช้ในการกรองเงื่อนไข
    ''' </summary>
    ''' <param name="STFcode">รหัสพนักงาน</param>
    ''' <param name="DPcode">รหัสแผนก</param>
    ''' <param name="BRcode">รหัสสาขา</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getBranchDropDownCriteria(ByVal STFcode As String, ByVal DPcode As String, ByVal BRcode As String) As String
        Try
            clsCurrentVar.DataBaseName = clsCurrentVar.PNEUMAXDB
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                Dim GroupName As String()
                GroupName = Nothing

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(String.Format("spGroupOfUser '{0}','{1}'", STFcode, DPcode))

                    If dt.Rows.Count > 0 Then
                        Dim _GroupName As String = ""
                        For i As Integer = 0 To dt.Rows.Count - 1
                            If _GroupName = "" Then
                                _GroupName = dt.Rows(0)("GPName")
                            Else
                                _GroupName = String.Format("{0},{1}", _GroupName, dt.Rows(i)("GPName"))
                            End If
                            GroupName = Split(_GroupName, ",")
                        Next
                    End If

                    strBD.AppendFormat("Select *").AppendLine()
                    strBD.AppendFormat("from  (").AppendLine()
                    If (clsGlobalVar.GroupDirector(GroupName) OrElse clsGlobalVar.GroupMIS(GroupName) OrElse
                        ((clsGlobalVar.GroupManager(GroupName) OrElse clsGlobalVar.GroupSaleManager(GroupName)) AndAlso BRcode.Trim() <> "" AndAlso BRcode.Trim() <> "BKK")) Then
                        strBD.AppendFormat("          Select    'ALL' as BRcode,'ALL' as BRname,1 as BRgroup").AppendLine()
                        strBD.AppendFormat("          union all").AppendLine()
                        strBD.AppendFormat("          select    BRcode,(rtrim(BRcode)+' ('+BRdesc_T+')') as BRname,BRgroup").AppendLine()
                        strBD.AppendFormat("          from	    Branch").AppendLine()
                        If ((clsGlobalVar.GroupManager(GroupName) OrElse clsGlobalVar.GroupSaleManager(GroupName)) AndAlso BRcode.Trim() <> "" AndAlso BRcode.Trim() <> "BKK") Then
                            strBD.AppendFormat("          where	    (MGRCode='{0}' or MGRCode2='{0}')", STFcode).AppendLine()
                        End If
                    Else
                        strBD.AppendFormat("          select	BRcode,BRdesc_T as BRname,BRgroup").AppendLine()
                        strBD.AppendFormat("          from	    Branch").AppendLine()
                    End If
                    strBD.AppendFormat("      )M1").AppendLine()
                    strBD.AppendFormat("Order by BRgroup,BRcode").AppendLine()
                    Dim SqlStr As String = strBD.ToString()
                    dt = objCon.GetDataTable(SqlStr)
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getBranchDropDownCriteria",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get ข้อมูล Leave Approve ใส่ใน Grid
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="STFcode">รหัสพนักงาน ของผู้ approve</param>
    ''' <param name="DPcode">รหัสแผนก ของผู้ approve</param>
    ''' <param name="BRcode">รหัสสาขา ของผู้ approve</param>
    ''' <param name="DPcodeCriteria">รหัสแผนกที่ใช้กรอง Approve</param>
    ''' <param name="BRcodeCriteria">รหัสสาขาที่ใช้กรอง Approve</param>
    ''' <param name="STFNameSearch">ชื่อพนักงานที่ลา ใช้ในการค้นหา</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getLeaveApproveGrid(ByVal DataBaseName As String, ByVal STFcode As String, ByVal DPcode As String, ByVal BRcode As String,
                                              ByVal DPcodeCriteria As String, ByVal BRcodeCriteria As String, ByVal STFNameSearch As String) As String
        Try
            clsCurrentVar.DataBaseName = DataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                Dim GroupName As String()
                GroupName = Nothing

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(String.Format("spGroupOfUser '{0}','{1}'", STFcode, DPcode))

                    If dt.Rows.Count > 0 Then
                        Dim _GroupName As String = ""
                        For i As Integer = 0 To dt.Rows.Count - 1
                            If _GroupName = "" Then
                                _GroupName = dt.Rows(0)("GPName")
                            Else
                                _GroupName = String.Format("{0},{1}", _GroupName, dt.Rows(i)("GPName"))
                            End If
                            GroupName = Split(_GroupName, ",")
                        Next
                    End If

                    strBD.AppendFormat("Select    isnull(Convert(varchar(20),WorkDate,120),'') as WorkDate,Origin,STFcode,convert(Varchar(8000),STFfullname) as STFname,t1.BRcode,DPcode,").AppendLine()
                    strBD.AppendFormat("		  CheckCode,ApprCode1,ApprCode2,OrgCode,TransNo,MGRCode,MGRCode2").AppendLine()
                    strBD.AppendFormat("from	  VW_Fox_Leave_Appr t1").AppendLine()
                    strBD.AppendFormat("          inner join Branch t2 on t1.BRcode=t2.BRcode").AppendLine()
                    strBD.AppendFormat("where      (OrgCode<>'17' and OrgCode<>'18')").AppendLine()
                    If (DPcodeCriteria.Trim().ToUpper() <> "ALL") Then
                        strBD.AppendFormat("and       t1.DPcode='{0}'", DPcodeCriteria).AppendLine()
                    End If
                    If (BRcodeCriteria.Trim().ToUpper() <> "ALL") Then
                        strBD.AppendFormat("and       t2.BRcode='{0}'", BRcodeCriteria).AppendLine()
                    Else
                        If ((clsGlobalVar.GroupManager(GroupName) OrElse clsGlobalVar.GroupSaleManager(GroupName)) AndAlso
                            BRcode.Trim() <> "" AndAlso BRcode.Trim() <> "BKK") Then
                            strBD.AppendFormat("          and 	(MGRCode='{0}' or MGRCode2='{0}')", STFcode).AppendLine()
                        End If
                    End If
                    If (clsGlobalVar.GroupManager(GroupName) OrElse clsGlobalVar.GroupSaleManager(GroupName)) Then
                        strBD.AppendFormat("          and 	(STFcode<>'{0}')", STFcode).AppendLine()
                    End If

                    If (STFNameSearch.Trim() <> "") Then
                        strBD.AppendFormat("and		STFfullname like '{0}%'", Me.objUtilities.CheckQuoteSQL(STFNameSearch)).AppendLine()
                    End If
                    strBD.AppendFormat("Order by WorkDate, STFfullname").AppendLine()
                    Dim SqlStr As String = strBD.ToString()
                    dt = Nothing
                    dt = objCon.GetDataTable(SqlStr)
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object)
            For Each dr As DataRow In dt.Rows
                row = New Dictionary(Of String, Object)()
                For Each col As DataColumn In dt.Columns
                    row.Add(col.ColumnName, dr(col))
                Next
                rows.Add(row)
            Next
            'ตั้งชื่อ Group ของข้อมูล
            Dim resultString As String = ""
            'resultString = serializer.Serialize(rows)
            resultString = JsonConvert.SerializeObject(dt)
            resultString = "{""leaveapprovegrids"":" & resultString & "}"
            Return resultString
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getLeaveApproveGrid",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับ get ข้อมูลการนัดหมายใส่ใน GridView
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="DPcode">รหัสแผนก</param>
    ''' <param name="STFcode">รหัสพนักงาน</param>
    ''' <param name="AppDate">วันที่นัดหมาย 2017-08-30</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getAppointmentGrid(ByVal DataBaseName As String, ByVal DPcode As String, ByVal STFcode As String, ByVal AppDate As String) As String
        'Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Try
            clsCurrentVar.DataBaseName = DataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                AppDate = Me.objUtilities.DateyyyyMMdd(AppDate)
                Dim strBD As New StringBuilder
                strBD.AppendFormat("select	ROW_NUMBER() over(order by appdate desc) as RowNo,").AppendLine()
                strBD.AppendFormat("        isnull(Convert(varchar(20),AppDate,120),'') as AppDate, AppStartTime,  ").AppendLine()
                strBD.AppendFormat("        CSCode,convert(Varchar(8000),CSthiname) as CSthiname").AppendLine()
                strBD.AppendFormat("from    vwFunnel_ShowGridAppointment t1").AppendLine()
                strBD.AppendFormat("where	DPcode='{0}' ", DPcode).AppendLine()
                strBD.AppendFormat("and		left(SAcode,4)='{0}'", STFcode).AppendLine()
                strBD.AppendFormat("and		convert(varchar(8),t1.appDate,112)=CONVERT(varchar(8),'{0}',112)", AppDate)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object)
            For Each dr As DataRow In dt.Rows
                row = New Dictionary(Of String, Object)()
                For Each col As DataColumn In dt.Columns
                    row.Add(col.ColumnName, dr(col))
                Next
                rows.Add(row)
            Next
            'ตั้งชื่อ Group ของข้อมูล
            Dim resultString As String = ""
            resultString = JsonConvert.SerializeObject(dt)
            resultString = "{""appointmentgrids"":" & resultString & "}"
            Return resultString
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getAppointmentGrid",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' สำหรับ get ข้อมูลการลาใส่ใน Textbox
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="TransNo">Transaction Number</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getLeaveApprove(ByVal DataBaseName As String, ByVal TransNo As String) As String
        Try
            clsCurrentVar.DataBaseName = DataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                Dim strBD As New StringBuilder
                strBD.AppendFormat("select	TransNo,STFcode,convert(Varchar(8000),STFfullname) as STFname,").AppendLine()
                strBD.AppendFormat("        isnull(Convert(varchar(20),WorkDate,103),'') as WorkDate,").AppendLine()
                strBD.AppendFormat("		Origin,OrgCode,t1.BRcode,DPcode,CheckCode,ApprCode1,ApprCode2,CancelCode,").AppendLine()
                strBD.AppendFormat("		(isnull(Convert(varchar(20),CrtDate,103),'')+' '+isnull(Convert(varchar(5),CrtDate,108),'')) as CrtDate,convert(Varchar(8000),LeaveRemark) as LeaveRemark,HaveMedicalCertificate").AppendLine()
                strBD.AppendFormat("from    VW_Fox_Leave_Appr t1").AppendLine()
                strBD.AppendFormat("where	TransNo='{0}' ", TransNo)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Return JsonConvert.SerializeObject(dt)
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getLeaveApprove",
                .ResultMessage = ex.Message}}
            'Return serializer.Serialize(objerror)
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function


    ''' <summary>
    ''' Update Approve ใบลา
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="OptionApprove">Y=Approve,N หรือว่างๆ=ไม่อนุมัติใบลา</param>
    ''' <param name="TransNoCond">เงื่อนไข Transaction Number</param>
    ''' <param name="ApprCode">รหัสพนักงานที่ทำการ Approve</param>
    ''' <param name="CurrentUserSTFcode">รหัสพนักงาน ที่ทำการแก้ไขล่าสุด</param>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function UpdateLeaveApprove(ByVal DataBaseName As String, ByVal OptionApprove As String, ByVal TransNoCond As String,
                                       ByVal ApprCode As String, ByVal CurrentUserSTFcode As String) As String
        Dim objResult As New clsResultSQL()
        Try
            clsCurrentVar.DataBaseName = DataBaseName
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                objCon.OpenConnection()

                Dim StrBD As New StringBuilder()
                StrBD.AppendFormat("select  t1.STFcode,WorkDate,Origin,t2.STFInternalMail,t2.STFfullname").AppendLine()
                StrBD.AppendFormat("from    Fox_Leave t1").AppendLine()
                StrBD.AppendFormat("        inner join Staff t2 on t1.STFcode=t2.STFcode").AppendLine()
                StrBD.AppendFormat("where   TransNo='{0}'", TransNoCond).AppendLine()
                StrBD.AppendFormat("and     CheckCode<>''").AppendLine()
                If (OptionApprove.Trim() = "Y") Then 'Approve
                    StrBD.AppendFormat("and     LeaveUnApprCode='' and CancelCode=''").AppendLine()
                Else 'ไม่อนุมัติ
                    StrBD.AppendFormat("and     ApprCode2='' and CancelCode=''").AppendLine()
                End If

                Dim dtII As New DataTable
                Dim SqlStrII As String = StrBD.ToString()
                dtII = objCon.GetDataTable(StrBD.ToString())
                'ตรวจสอบว่า มีข้อมูลให้ Approve จริงๆ
                If (dtII IsNot Nothing AndAlso dtII.Rows.Count > 0) Then
                    Try
                        StrBD.Length = 0
                        If (OptionApprove.Trim() = "Y") Then 'Approve
                            StrBD.AppendFormat("Update  Fox_Leave").AppendLine()
                            StrBD.AppendFormat("set     ApprCode1='{0}',", ApprCode).AppendLine()
                            StrBD.AppendFormat("        ApprDate1=getdate(),").AppendLine()
                            StrBD.AppendFormat("        ApprCode2='{0}',", ApprCode).AppendLine()
                            StrBD.AppendFormat("        ApprDate2=getdate(),").AppendLine()
                            StrBD.AppendFormat("        LeaveComplete='Y',").AppendLine()
                            StrBD.AppendFormat("        LeaveCompleteDate=getdate()").AppendLine()
                            StrBD.AppendFormat("where   TransNo='{0}'", TransNoCond).AppendLine()
                            StrBD.AppendFormat("and     CheckCode<>''").AppendLine()
                            StrBD.AppendFormat("and     LeaveUnApprCode=''").AppendLine()
                            StrBD.AppendFormat("and     CancelCode=''")
                            objCon.ExecuteData(StrBD.ToString())

                            'ทำการ Approve จนครบ
                            'Insert ข้อมูลการลา เข้าใน Fox_Extra 
                            If Me.Insert_Fox_Extra(DataBaseName, TransNoCond, ApprCode) Then

                            End If
                        Else 'ไม่อนุมัติ
                            StrBD.AppendFormat("Update  Fox_Leave").AppendLine()
                            StrBD.AppendFormat("set     LeaveUnApprCode='{0}',", ApprCode).AppendLine()
                            StrBD.AppendFormat("        LeaveUnApprDate=getdate()").AppendLine()
                            StrBD.AppendFormat("where   TransNo='{0}'", TransNoCond).AppendLine()
                            StrBD.AppendFormat("and     CheckCode<>'' and ApprCode2=''").AppendLine()
                            StrBD.AppendFormat("and     LeaveUnApprCode=''").AppendLine()
                            StrBD.AppendFormat("and     CancelCode=''")
                            objCon.ExecuteData(StrBD.ToString())

                            'สำหรับ SendMail กรณี ไม่อนุมัติ
                            Dim _STFthifullnameApprcode As String = ""
                            Dim _STFcode As String = ""
                            Dim _STFInternalMail As String = ""
                            Dim _STFfullnameLeave As String = ""
                            Dim _Origin As String = ""
                            Dim _WorkDate As String = ""
                            If (dtII.Rows(0)("STFcode") IsNot Nothing) Then _STFcode = dtII.Rows(0)("STFcode")
                            If (dtII.Rows(0)("WorkDate") IsNot Nothing AndAlso IsDate(dtII.Rows(0)("WorkDate"))) Then _WorkDate = Me.objUtilities.Date_ddMMyyyy(dtII.Rows(0)("WorkDate"))
                            If (dtII.Rows(0)("Origin") IsNot Nothing) Then _Origin = dtII.Rows(0)("Origin")
                            If (dtII.Rows(0)("STFInternalMail") IsNot Nothing) Then _STFInternalMail = dtII.Rows(0)("STFInternalMail")
                            If (dtII.Rows(0)("STFfullname") IsNot Nothing) Then _STFfullnameLeave = dtII.Rows(0)("STFfullname")

                            Dim StrBD5 As New StringBuilder()
                            StrBD5.AppendFormat("select  t1.STFcode,STFfullname").AppendLine()
                            StrBD5.AppendFormat("from    Staff t1").AppendLine()
                            StrBD5.AppendFormat("where   STFcode='{0}'", ApprCode).AppendLine()
                            Dim dt5 As New DataTable
                            dt5 = objCon.GetDataTable(StrBD5.ToString())
                            If (dt5 IsNot Nothing AndAlso dt5.Rows.Count > 0) Then
                                If (dt5.Rows(0)("STFfullname") IsNot Nothing) Then _STFthifullnameApprcode = dt5.Rows(0)("STFfullname")
                            End If

                            If (_STFInternalMail.Trim() <> "") Then
                                Me.SendMail_UnApprove(DataBaseName, _STFcode, _STFfullnameLeave, _STFInternalMail, _Origin, _WorkDate, _STFthifullnameApprcode, CurrentUserSTFcode)
                            End If
                        End If

                        objResult.ResultID = "Success"
                        objResult.ResultMessage = "UpdateLeaveApprove"

                        Return JsonConvert.SerializeObject(objResult)
                    Catch ex As Exception
                        objResult.ResultID = "UnSuccess"
                        objResult.ResultMessage = ex.Message
                        Return JsonConvert.SerializeObject(objResult)
                    Finally
                        objCon.CloseConnection()
                    End Try
                Else
                    objResult.ResultID = "UnSuccess"
                    objResult.ResultMessage = "ไม่มีการ Approve"
                    Return JsonConvert.SerializeObject(objResult)
                End If
            End Using
        Catch ex As Exception
            objResult.ResultID = "UnSuccess"
            objResult.ResultMessage = ex.Message
            Return JsonConvert.SerializeObject(objResult)
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ Insert,Update Data LeaveManagement
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="TransNoCond">เงื่อนไข Transaction Number</param>
    ''' <param name="CurrentUserSTFcode">รหัสพนักงาน ที่ทำการแก้ไขล่าสุด</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Insert_Fox_Extra(ByVal DataBaseName As String, ByVal TransNoCond As String, ByVal CurrentUserSTFcode As String) As Boolean
        Insert_Fox_Extra = False
        Try
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString)
                Dim StrBD As New StringBuilder()
                Me.objDBUtilities = New clsDBUtilities(Me.objCurrentVar)

                Dim _STFcode As String = ""
                Dim _WorkDate As String = ""
                Dim _Origin As String = ""
                Dim _OrgCode As String = ""
                Dim StrBD2 As New StringBuilder()
                StrBD2.AppendFormat("Select  STFcode, WorkDate, Origin, OrgCode").AppendLine()
                StrBD2.AppendFormat("from    Fox_Leave").AppendLine()
                StrBD2.AppendFormat("where   TransNo='{0}'", TransNoCond)
                                Dim dtII As New DataTable
                dtII = objCon.GetDataTable(StrBD2.ToString())
                If (dtII IsNot Nothing AndAlso dtII.Rows.Count > 0) Then
                    _STFcode = dtII.Rows(0)("STFcode")
                    _WorkDate = dtII.Rows(0)("WorkDate")
                    _Origin = dtII.Rows(0)("Origin")
                    _OrgCode = dtII.Rows(0)("OrgCode")

                    Dim _KeySearch As String = String.Format("{0}{1}{2}", _STFcode.Trim(), Me.objUtilities.Dateyyyy_MM_dd(CDate(_WorkDate)), _Origin.Trim())

                    'ตรวจสอบก่อนว่า มีข้อมูลใน Fox_Extra แล้วหรือยัง
                    Dim StrBD3 As New StringBuilder()
                    StrBD3.AppendFormat("select  KeySearch").AppendLine()
                    StrBD3.AppendFormat("from    FOX_EXTRA").AppendLine()
                    StrBD3.AppendFormat("where   KeySearch='{0}'", _KeySearch.Trim())
                    Dim dt3 As New DataTable
                    dt3 = objCon.GetDataTable(StrBD3.ToString())
                    If (dt3 IsNot Nothing AndAlso dt3.Rows.Count > 0) Then
                        'ถ้า Approve แล้วมีข้อมูลใน  Fox_Extra แล้ว แสดงว่า HR คีย์ใบลาให้  จะต้องทำการถอย Approve ทั้งหมด
                        StrBD.Length = 0
                        StrBD.AppendFormat("Update  Fox_Leave").AppendLine()
                        StrBD.AppendFormat("set     ApprCode1='',").AppendLine()
                        StrBD.AppendFormat("        ApprDate1=NULL,").AppendLine()
                        StrBD.AppendFormat("        ApprCode2='',").AppendLine()
                        StrBD.AppendFormat("        ApprDate2=NULL,").AppendLine()
                        StrBD.AppendFormat("        LeaveComplete='',").AppendLine()
                        StrBD.AppendFormat("        LeaveCompleteDate=NULL").AppendLine()
                        StrBD.AppendFormat("Where   TransNo='{0}'", TransNoCond).AppendLine()
                        objCon.ExecuteData(StrBD.ToString())
                        'MessageBox.Show(String.Format("ไม่สามารถ Approve ได้{0}เนื่องจากฝ่าย HR บันทึกใบลา {1} ให้แล้ว  !!!", vbCrLf, objLeaveManagementCurrent.Origin), "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Insert_Fox_Extra = False
                    Else
                        StrBD.Length = 0
                        StrBD.AppendFormat("Insert into {0}(StfCode,WorkDate,Origin,OrgCode,LastUser,KeySearch,ToWorkDate,RefTransNo) values('{1}','{2}','{3}','{4}','{5}','{6}','{2}','{7}')",
                                       "FOX_EXTRA", _STFcode.Trim(), Me.objUtilities.Date_ddMMyyyy(_WorkDate), _Origin.Trim(), _OrgCode,
                                       CurrentUserSTFcode, _KeySearch, TransNoCond)
                        'execute
                        If objCon.ExecuteData(StrBD.ToString()) = "" Then 'ถ้า Return ="" แสดงว่า Complete
                            Insert_Fox_Extra = True
                        Else
                            Insert_Fox_Extra = False
                        End If
                    End If
                Else
                    Insert_Fox_Extra = False
                End If
            End Using

            Return Insert_Fox_Extra
        Catch ex As Exception
            Insert_Fox_Extra = False
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ SendMail กรณี ไม่อนุมัติ
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="STFcodeLeave">รหัสพนักงานที่ลา</param>
    ''' <param name="STFthifullnameLeave">ชื่อพนักงานที่ลา</param>
    ''' <param name="STFInternalMailLeave">Email ของคนที่ลา</param>
    ''' <param name="OriginLeave">ประเภทการลา</param>
    ''' <param name="WorkDateLeave">วันที่ลา</param>
    ''' <param name="STFthifullnameUnApprove">ชื่อพนักงานที่ไม่อนุมัติ</param>
    ''' <param name="CurrentUserSTFcode">รหัสพนักงาน ที่ทำการแก้ไขล่าสุด</param>
    ''' <returns></returns>
    Private Function SendMail_UnApprove(ByVal DataBaseName As String, ByVal STFcodeLeave As String, ByVal STFthifullnameLeave As String,
                                        ByVal STFInternalMailLeave As String, ByVal OriginLeave As String,
                                        ByVal WorkDateLeave As String, ByVal STFthifullnameUnApprove As String, ByVal CurrentUserSTFcode As String) As Boolean
        SendMail_UnApprove = False
        Try
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString)
                If (STFInternalMailLeave.Trim() <> "") Then
                    'แจ้งเตือน Email ไม่อนุมัติ ใบลา
                    Dim SC As SmtpClient = New System.Net.Mail.SmtpClient("192.168.2.146", 25)
                    Dim _getSTFemailReceive As String = ""
                    'ต้องแนบ Email ของพนักงานด้วย
                    If (STFInternalMailLeave IsNot Nothing AndAlso STFInternalMailLeave.Trim() <> "") Then
                        If (DataBaseName.ToUpper() = "DATATEST") Then
                            _getSTFemailReceive = String.Format("Test{0}", STFInternalMailLeave.Trim())
                        Else
                            _getSTFemailReceive = String.Format("{0}", STFInternalMailLeave.Trim())
                        End If
                    End If

                    Dim EM As MailMessage
                    EM = Nothing
                    EM = New MailMessage("AppLeaveApprove@pneumax.co.th", _getSTFemailReceive)
                    If (DataBaseName.ToUpper() = "DATATEST") Then
                        EM.Bcc.Add("sitrach.rue@pneumax.co.th,petcharatana.dua@pneumax.co.th")
                    End If
                    EM.Subject = String.Format("คุณ{0} ไม่อนุมัติ {1} วันที่ {2}", STFthifullnameUnApprove, OriginLeave, WorkDateLeave)
                    Dim StrBD As New StringBuilder
                    Dim body As String
                    StrBD.AppendFormat("<head>").AppendLine()
                    StrBD.AppendFormat("<title></title>").AppendLine()
                    StrBD.AppendFormat("<style type='text/css'>").AppendLine()
                    StrBD.AppendFormat("</style>").AppendLine()
                    StrBD.AppendFormat("</head>").AppendLine()
                    StrBD.AppendFormat("<body>").AppendLine()
                    StrBD.AppendFormat("<span style='font-size: 14pt; font-family: Tahoma; color: #FF3300; font-style: Bold;'>").AppendLine()
                    StrBD.AppendFormat("คุณ{0}   ไม่อนุมัติ {1} วันที่ {2} ของคุณ{3}<br><br>", STFthifullnameUnApprove, OriginLeave, WorkDateLeave, STFthifullnameLeave).AppendLine()
                    StrBD.AppendFormat("").AppendLine()
                    StrBD.AppendFormat("</span>").AppendLine()
                    StrBD.AppendFormat("</body>").AppendLine()
                    body = StrBD.ToString()
                    EM.Body = body.Replace(vbCrLf, "")
                    EM.IsBodyHtml = True
                    Try
                        SC.Send(EM)
                        SendMail_UnApprove = True
                        Return SendMail_UnApprove
                    Catch ex As Exception
                        Exit Function
                    End Try
                    EM.Dispose()
                End If
            End Using

        Catch ex As Exception
            SendMail_UnApprove = False
        End Try
    End Function

    ''' <summary>
    ''' สำหรับ get ข้อมูลการนัดหมายใส่ใน GridView
    ''' </summary>
    ''' <param name="DataBaseName">ชื่อ DataBase</param>
    ''' <param name="STFcode">รหัสพนักงาน</param>
    ''' <param name="WorkDate">วันที่นัดหมาย 2017-08-30</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Function getFoxExtraHistory(ByVal DataBaseName As String, ByVal STFcode As String, ByVal WorkDate As String) As String
        Try
            clsCurrentVar.DataBaseName = DataBaseName
            Dim dt As New DataTable
            Me.objCurrentVar = New clsCurrentVar
            Me.objUtilities = New clsUtilities
            Using objCon As clsConnection = clsConnection.GetConnection(Me.objCurrentVar.GetConnectionString())
                'WorkDate = Me.objUtilities.DateyyyyMMdd(WorkDate)
                WorkDate = Me.objUtilities.ConvertStrToDateEng(WorkDate)
                Dim strBD As New StringBuilder
                strBD.AppendFormat("exec sp_Show_Extra_History '{0}','{1}'", STFcode, WorkDate)

                objCon.OpenConnection()
                Try
                    dt = objCon.GetDataTable(strBD.ToString())
                Catch ex As Exception

                Finally
                    objCon.CloseConnection()
                End Try
            End Using

            Dim rows As New List(Of Dictionary(Of String, Object))()
            Dim row As Dictionary(Of String, Object)
            For Each dr As DataRow In dt.Rows
                row = New Dictionary(Of String, Object)()
                For Each col As DataColumn In dt.Columns
                    row.Add(col.ColumnName, dr(col))
                Next
                rows.Add(row)
            Next
            'ตั้งชื่อ Group ของข้อมูล
            Dim resultString As String = ""
            resultString = JsonConvert.SerializeObject(dt)
            resultString = "{""foxextrahistorygrids"":" & resultString & "}"
            Return resultString
        Catch ex As Exception
            'ส่ง Error ออกไปแบบ json เพราะว่าปลายทางจะได้ข้อมูล Error ออกไปใช้ตรวจสอบ หรือแสดงได้
            Dim objerror As clsResultSQL() = New clsResultSQL() _
            {New clsResultSQL() With {
                .ResultID = "Error_Function_getFoxExtraHistory",
                .ResultMessage = ex.Message}}
            Return JsonConvert.SerializeObject(objerror)
        End Try
    End Function
End Class