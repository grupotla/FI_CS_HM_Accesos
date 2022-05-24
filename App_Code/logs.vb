Imports Npgsql
Imports System.Data
Imports connection
'Imports System.Runtime.Serialization
'Imports System.Runtime.Serialization.Json
Imports System.IO


Public Class logs

    Inherits System.Web.UI.Page

    Public Shared Sub log(ByVal Server As System.Web.HttpServerUtility, ByVal id As Integer, ByVal accion As String, ByVal before As String, ByVal after As Dictionary(Of String, String), ByVal tabla As String, ByVal OperatorID As String, ByVal OperatorLogin As String, ByVal sistema As String, ByVal DBAccesos As String, ByVal ip As String)
        Dim msg As String
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                conn.Open()

                Dim after_txt As String = Serialize(after)
                'Dim objeto As Dictionary(Of String, String) = Deserialize(after_txt)
                'sql1 = "INSERT INTO usuarios_empresas_log (user_id, user_name, fecha, sistema, db, accion, after_txt, ip, tiempo, tabla) VALUES (@user_id, @user_name, CURRENT_TIMESTAMP, @sistema, @db, @accion, @after, @ip, CURRENT_TIMESTAMP, @tabla)"
                sql1 = "INSERT INTO usuarios_empresas_log (user_id, user_name, sistema, db, accion, before_txt, after_txt, ip, tabla, tiempo) VALUES (@user_id, @user_name, @sistema, @db, @accion, @before, @after, @ip, @tabla, @tiempo)"
                Dim comm As New NpgsqlCommand(sql1, conn)
                'comm.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = 0
                comm.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = OperatorID
                comm.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Text).Value = OperatorLogin
                comm.Parameters.Add("@sistema", NpgsqlTypes.NpgsqlDbType.Text).Value = sistema
                comm.Parameters.Add("@db", NpgsqlTypes.NpgsqlDbType.Text).Value = DBAccesos
                comm.Parameters.Add("@accion", NpgsqlTypes.NpgsqlDbType.Text).Value = accion
                comm.Parameters.Add("@before", NpgsqlTypes.NpgsqlDbType.Text).Value = before
                comm.Parameters.Add("@after", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Serialize(after)
                comm.Parameters.Add("@ip", NpgsqlTypes.NpgsqlDbType.Text).Value = ip
                comm.Parameters.Add("@tabla", NpgsqlTypes.NpgsqlDbType.Text).Value = tabla
                comm.Parameters.Add("@tiempo", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = DateTime.Now
                comm.ExecuteNonQuery()

                'sql1 = "INSERT INTO usuarios_empresas_log (user_id, user_name, sistema, db, accion, after_txt, ip, tabla) VALUES (" & OperatorID & ", '" & OperatorLogin & "', '" & sistema & "', '" & DBAccesos & "', '" & accion & "', '" & after_txt & "', '" & ip & "', '" & tabla & "')"
                'Dim comm1 As New NpgsqlCommand(sql1, conn)
                'comm1.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            msg = ex.Message
        End Try

    End Sub


    Public Shared Function Serialize(ByVal obj As Dictionary(Of String, String)) As String
        'Dim serializer As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType)
        Dim ms As MemoryStream = New MemoryStream
        'serializer.WriteObject(ms, obj)
        Dim retVal As String = Encoding.Default.GetString(ms.ToArray)
        ms.Dispose()
        Return retVal
    End Function

    Public Shared Function Deserialize(ByVal json As String) As Dictionary(Of String, String)
        Dim obj As New Dictionary(Of String, String) ' = Activator.CreateInstance
        Dim ms As MemoryStream = New MemoryStream(Encoding.Unicode.GetBytes(json))
        'Dim serializer As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType)
        'obj = CType(serializer.ReadObject(ms), Dictionary(Of String, String))
        ms.Close()
        ms.Dispose()
        Return obj
    End Function


    Public Shared Function SerializePermiso(ByVal obj As SolicitudPermiso) As String
        'Dim serializer As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType)
        Dim ms As MemoryStream = New MemoryStream
        'serializer.WriteObject(ms, obj)
        Dim retVal As String = Encoding.Default.GetString(ms.ToArray)
        ms.Dispose()
        Return retVal
    End Function

    Public Shared Function DeserializePermiso(ByVal json As String) As SolicitudPermiso
        Dim obj As New SolicitudPermiso
        Dim ms As MemoryStream = New MemoryStream(Encoding.Unicode.GetBytes(json))
        'Dim serializer As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType)
        'obj = CType(serializer.ReadObject(ms), SolicitudPermiso)
        ms.Close()
        ms.Dispose()
        Return obj
    End Function

End Class
