Imports Microsoft.VisualBasic

''' <summary>
''' Class Error เก็บข้อความที่ Error
''' </summary>
''' <remarks></remarks>
Public Class clsResultSQL

    Private _ResultID As String
    ''' <summary>
    ''' หัวข้อ Success หรือ Error
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResultID() As String
        Get
            Return _ResultID
        End Get
        Set(ByVal value As String)
            _ResultID = value
        End Set
    End Property

    Private _ResultMessage As String
    ''' <summary>
    ''' ข้อความ Success หรือ Error
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResultMessage() As String
        Get
            Return _ResultMessage
        End Get
        Set(ByVal value As String)
            _ResultMessage = value
        End Set
    End Property

End Class
