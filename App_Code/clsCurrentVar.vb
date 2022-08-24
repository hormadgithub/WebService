Imports Microsoft.VisualBasic

Public Class clsCurrentVar

    'Property คำสั่งในการเชื่อมต่อ Server และ DataBase
    Private strConn As String
    Public ReadOnly Property GetConnectionString() As String
        Get
            'Server เดิม MARIA  ip 192.168.2.199
            'If (DataBaseName.Trim().ToUpper() = "DATATEST") Then
            '    Return "Data Source=192.168.2.199;" &
            '      "initial catalog=DataTest;User ID=alluser;password=alluser;Connect Timeout=600;"
            'ElseIf (DataBaseName.Trim().ToUpper() = "PNEUMAXDB") Then
            '    Return "Data Source=192.168.2.199;" &
            '      "initial catalog=PneumaxDB;User ID=alluser;password=alluser;Connect Timeout=600;"
            'Else
            '    Return "Data Source=192.168.2.199;" &
            '      "initial catalog=AnalysisDB;User ID=alluser;password=alluser;Connect Timeout=600;"
            'End If

            '06-01-2022 ทดสอบการ Connect ไปยัง MARIA-2019 ip 192.168.2.195 = DBSERVER
            '14-01-2022 ย้ายมาทำงานบน Server ตัวใหม่
            If (DataBaseName.Trim().ToUpper() = "DATATEST") Then
                Return "Data Source=DBSERVER;" &
                  "initial catalog=DataTest;User ID=alluser;password=alluser;Connect Timeout=600;"
            ElseIf (DataBaseName.Trim().ToUpper() = "PNEUMAXDB") Then
                Return "Data Source=DBSERVER;" &
                  "initial catalog=PneumaxDB;User ID=alluser;password=alluser;Connect Timeout=600;"
            Else
                Return "Data Source=DBSERVER;" &
                  "initial catalog=AnalysisDB;User ID=alluser;password=alluser;Connect Timeout=600;"
            End If
        End Get
    End Property

    Public Const ANALYSISDB As String = "ANALYSISDB"
    Public Const PNEUMAXDB As String = "PNEUMAXDB"
    Public Const DATATEST As String = "DATATEST"
    Public Shared DataBaseName As String = ""

End Class
