Imports Npgsql
Imports connection
Imports cMd5Hash
Imports logs

Partial Class Login

    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_login As String = icon_login
    Public Licon_user As String = icon_user
    Public Licon_keys As String = icon_keys

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load





        'Dim hay_usuarios As Boolean = True 2014-04-15 no verificar si hay usuarios, esto debe ser manual
        'Try
        '    Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
        '        sql1 = "SELECT id_usuario, pw_name, pais, level FROM usuarios_empresas_login"
        '        Dim comm As New NpgsqlCommand(sql1, conn)
        '        conn.Open()
        '        Dim dataread As NpgsqlDataReader = comm.ExecuteReader
        '        If Not dataread.Read() Then
        '            hay_usuarios = False
        '        End If
        '        dataread.Close()
        '    End Using
        'Catch ex As Exception
        '    msg = ex.Message
        '    img = icon_err_read
        '    css = "alert-warning"
        'End Try


        'If hay_usuarios = False Then
        '    MsgBox("No hay usuarios debe crear el admin")
        '    Response.Redirect("ct_usuarios.aspx")
        'End If
        Try

            If Not IsPostBack Then

                If Session("OperatorID") <> Nothing Then

                    Dim items As New Dictionary(Of String, String)

                    log(Server, 0, "logout", "", items, "", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

                    Session.RemoveAll()

                End If

            End If


            If 1 = 2 Then 'PROCESO PARA SETEAR PAIS DEFAULT EN TRAFICOS 2020-03-20
                Dim conn_mas As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario, pais FROM usuarios_empresas WHERE pw_activo = 1 ORDER BY id_usuario"
                Dim comm_mas3 As New NpgsqlCommand(sql1, conn_mas)
                comm_mas3.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                conn_mas.Open()
                Dim dataread3 As NpgsqlDataReader = comm_mas3.ExecuteReader
                While dataread3.Read()
                    'connection.setEmpresaUser("aereo", dataread3(0), dataread3(1), "", Server)
                    'connection.setEmpresaUser("terrestre", dataread3(0), dataread3(1), "", Server)
                    'connection.setEmpresaUser("wms", dataread3(0), dataread3(1), "", Server)
                End While
            End If

        Catch ex As Exception
            msg = ex.Message
        End Try


    End Sub


    Protected Sub btn_login_Click(sender As Object, e As System.EventArgs) Handles btn_login.Click
        login_sp()
    End Sub

    Protected Sub pass_txt_TextChanged(sender As Object, e As System.EventArgs) Handles pass_txt.TextChanged
        login_sp()
    End Sub

    Protected Sub user_txt_TextChanged(sender As Object, e As System.EventArgs) Handles user_txt.TextChanged
        login_sp()
    End Sub

    Protected Sub login_sp()

        Try

            Session("OperatorID") = Nothing
            Session("demo") = False

            Dim password As String = Trim(pass_txt.Text)

            If user_txt.Text <> "" And password <> "" Then
                Try
                    Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                        Dim HashCode As cMd5Hash
                        HashCode = New cMd5Hash()
                        password = HashCode.Md5FromString(password)

                        Dim visitorIPAddress As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
                        If visitorIPAddress = Nothing Then
                            visitorIPAddress = Request.ServerVariables("REMOTE_ADDR")
                        End If
                        If visitorIPAddress = Nothing Then
                            visitorIPAddress = Request.UserHostAddress
                        End If
                        'id_usuario
                        'sql1 = "SELECT create_id_usuario, pw_name, pais, level, demo, pw_gecos, pw_activo FROM usuarios_empresas_login WHERE pw_name=@user AND (pw_passwd=@pass1 OR pw_passwd=@pass2)"
                        sql1 = "SELECT create_id_usuario, pw_name, pais, level, demo, pw_gecos, pw_activo FROM usuarios_empresas_login WHERE pw_name = '" + Trim(user_txt.Text) + "' AND (pw_passwd = '" + Trim(pass_txt.Text) + "' OR pw_passwd = MD5('" + Trim(pass_txt.Text) + "'))"
                        Dim comm As New NpgsqlCommand(sql1, conn)
                        'comm.Parameters.Add("@user", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Trim(user_txt.Text)
                        'comm.Parameters.Add("@pass1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Trim(pass_txt.Text)
                        'comm.Parameters.Add("@pass2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = password
                        conn.Open()
                        Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                        If dataread.Read() Then

                            If dataread(6) = True Then
                                Session("OperatorID") = dataread(0)
                                Session("Login") = dataread(1)
                                Session("OperatorCountry") = dataread(2)
                                Session("OperatorLevel") = dataread(3)
                                Session("OperatorIP") = visitorIPAddress

                                If dataread(4) = True Then
                                    Session("Login") = dataread(5) & " Demo"
                                    Session("demo") = True
                                Else
                                    Session("Login") = dataread(5)
                                    Session("demo") = False
                                End If

                            Else
                                msg = "Usuario desactivado! No puede acceder a sistema."
                            End If

                        Else
                            msg = "Usuario o contraseña invalido!"
                        End If

                        dataread.Close()

                        Dim items As New Dictionary(Of String, String)
                        items.Add("pw_name", user_txt.Text)
                        log(Server, Session("OperatorID"), "login", "", items, "usuarios_empresas_login", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), visitorIPAddress)

                    End Using

                Catch ex As Exception
                    msg = ex.Message
                    img = icon_err_read
                    css = "alert-warning"
                End Try

            Else
                msg = "Ingrese Usuario y contraseña!"
            End If



            If Session("OperatorID") <> Nothing Then
                Response.Redirect("Default.aspx")
            End If

        Catch ex As Exception
            msg = ex.Message
        End Try


    End Sub

End Class

