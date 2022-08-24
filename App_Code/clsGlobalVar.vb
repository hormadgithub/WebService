Imports Microsoft.VisualBasic

Public Class clsGlobalVar

    '----- เก็บ Modify Add,Edit,Delete,Preview -------
    Public Const Copy As String = "Copy"
    Public Const Add As String = "ADD"
    Public Const Edit As String = "EDIT"
    Public Const Del As String = "DELETE"
    Public Const Preview As String = "PREVIEW"
    Public Const Update As String = "UPDATE"

    'สำหรับเก็บชื่อกลุ่มของพนักงาน (GroupName)
    Public Const GP_AccMgr As String = "GP_AccMgr"
    Public Const GP_BILLING As String = "GP_BILLING"
    Public Const GP_CSMANAGEMENT As String = "GP_CSMANAGEMENT"
    Public Const GP_Delivery As String = "GP_Delivery"
    Public Const GP_Director As String = "GP_Director"
    Public Const GP_DRCHECKER As String = "GP_DRCHECKER"
    Public Const GP_Import As String = "GP_Import"
    Public Const GP_INV As String = "GP_INV"
    Public Const GP_Management As String = "GP_Management"
    Public Const GP_Manager As String = "GP_Manager"
    Public Const GP_Mat As String = "GP_MAT"
    Public Const GP_MatMgr As String = "GP_MatMgr"
    Public Const GP_MD As String = "GP_MD"
    Public Const GP_MIS As String = "GP_MIS"
    Public Const GP_PrintCheque As String = "GP_PrintCheque"
    Public Const GP_Project As String = "GP_Project"
    Public Const GP_Sales As String = "GP_Sales"
    Public Const GP_SalesMgr As String = "GP_SalesMgr"
    Public Const GP_SalesMgrSub As String = "GP_SalesMgrSub"
    Public Const GP_SalesRep As String = "GP_SalesRep"
    Public Const GP_SalesUmgr As String = "GP_SalesUmgr"
    Public Const GP_SEC As String = "GP_SEC"
    Public Const GP_SeniorMgr As String = "GP_SeniorMgr"
    Public Const GP_Services1 As String = "GP_Services1"
    Public Const GP_Services2 As String = "GP_Services2"
    Public Const GP_Support As String = "GP_Support"
    Public Const GP_SupportBranch As String = "GP_SupportBranch"
    Public Const GP_SupportMgr As String = "GP_SupportMgr"
    Public Const GP_SeniorSupport As String = "GP_SeniorSupport"
    Public Const GP_SeniorSupport_Ext As String = "GP_SeniorSupport_Ext"
    Public Const GP_HR As String = "GP_HR"
    Public Const GP_ACE As String = "GP_ACE"


    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เกี่ยวกับ MIS เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย จะรู้ว่าเป็น MIS หรือไม่
    ''' ถ้าเป็น MIS=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupMIS(ByVal GroupName As String()) As Boolean
        GroupMIS = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_MIS) Then
                    GroupMIS = True
                    Exit Function
                End If
            Next
        End If
        Return GroupMIS
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เป็น Sale เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น sales หรือไม่
    ''' ถ้าเป็น Sale GroupSales=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupSales(ByVal GroupName As String()) As Boolean
        GroupSales = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_Sales) OrElse (CompareGroupName = GP_SalesRep) Then
                    GroupSales = True
                    Exit Function
                End If
            Next
        End If
        Return GroupSales
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เป็น Sale เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น sales หรือไม่
    ''' ถ้าเป็น Sale GroupSales=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupSalesRep(ByVal GroupName As String()) As Boolean
        GroupSalesRep = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_SalesRep) Then
                    GroupSalesRep = True
                    Exit Function
                End If
            Next
        End If
        Return GroupSalesRep
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เป็น Unit เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น Unit หรือไม่
    ''' ถ้าเป็น Unit GroupUnitManager=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupUnitManager(ByVal GroupName As String()) As Boolean
        GroupUnitManager = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_SalesUmgr) Then
                    GroupUnitManager = True
                    Exit Function
                End If
            Next
        End If
        Return GroupUnitManager
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เกี่ยวกับ Manager เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น Manager หรือไม่
    ''' ถ้าเป็น GroupManager=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupManager(ByVal GroupName As String()) As Boolean
        GroupManager = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_Manager) Then
                    GroupManager = True
                    Exit Function
                End If
            Next
        End If
        Return GroupManager
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เกี่ยวกับ Sale เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น sales หรือไม่
    ''' ถ้าเป็น Sale GroupSaleAll=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupSaleManager(ByVal GroupName As String()) As Boolean
        GroupSaleManager = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_SalesMgr) Then
                    GroupSaleManager = True
                    Exit Function
                End If
            Next
        End If
        Return GroupSaleManager
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เกี่ยวกับ Sale เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น Sales หรือไม่
    ''' ถ้าเป็น Sale GroupSaleAll=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupSaleManagerSub(ByVal GroupName As String()) As Boolean
        GroupSaleManagerSub = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_SalesMgrSub) Then
                    GroupSaleManagerSub = True
                    Exit Function
                End If
            Next
        End If
        Return GroupSaleManagerSub
    End Function

    ''' <summary>
    ''' สำหรับ Get เฉพาะ Group ที่เป็น Director เท่านั้นใช้บางกรณี
    ''' รวมไว้เพื่อไม่ต้องเขียนเงื่อนไขเยอะให้เรียกใช้ได้เลย ก็รู้แล้วว่าเป็น Director หรือไม่
    ''' ถ้าเป็น Director GroupDirector=True
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GroupDirector(ByVal GroupName As String()) As Boolean
        GroupDirector = False
        If GroupName IsNot Nothing Then 'ถ้ามีชื่อ Group จะไม่เป็น Nothing
            For Each CompareGroupName As String In GroupName 'วน Array ดึงชื่อ Group มาเปรียบเทียบ
                If (CompareGroupName = GP_Director) Then
                    GroupDirector = True
                    Exit Function
                End If
            Next
        End If
        Return GroupDirector
    End Function

End Class
