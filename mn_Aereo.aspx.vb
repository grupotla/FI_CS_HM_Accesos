Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_Aereo
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load



        If Session("OperatorID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If valida_pagina(Request.ServerVariables("SCRIPT_NAME"), GetConnectionStringFromFile("aimar", Server), Session("OperatorID"), Session("sistema")) = False Then
            Response.Redirect("Default.aspx")
        End If

        If Session("DBAccesosUserId") = Nothing Then
            Response.Redirect("mn_Master.aspx")
        End If

        If Not IsPostBack Then

            LeerRegistro()

        End If

    End Sub

    Protected Sub LeerRegistro()

        Try

            Dim htm As String = "<ul id='pestana' class='nav nav-tabs'>"
            htm = htm & menu_gen(Session("sistema"), "", False, Session("OperatorID"), Server)
            htm = htm & "</ul>"

            pestana_lbl.Text = htm



            Session("insert") = True

            Paises.Items.Clear()

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                'sql1 = "SELECT pais, '<img src=Content/flags/' || substring(pais for 2) || '-flag.gif height=16 /> ' || REPLACE(nombre, 'LATIN FREIGHT', 'LTF') as nom FROM usuarios_paises WHERE activo = 't' ORDER BY nombre"
                'sql1 = "SELECT pais, '<img src=Content/flags/' || substring(pais for 2) || '-flag.gif height=16 /> ' || nombre as nom FROM usuarios_paises WHERE activo = 't' AND char_length(pais) = 2 AND pais <> 'N1' ORDER BY nombre"
                'sql1 = "SELECT pais, '<img src=Content/flags/' || substring(pais for 2) || '-flag.gif height=16 /> ' || nombre as nom FROM usuarios_paises WHERE activo = 't' ORDER BY nombre"
                sql1 = "SELECT pais_iso as pais, '<img src=Content/flags/' || substring(pais_iso for 2) || '-flag.gif height=16 /> ' || nombre_empresa as nombre FROM empresas WHERE activo = 't' ORDER BY nombre_empresa"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                Paises.DataSource = ds
                Paises.DataTextField = "nombre"
                Paises.DataValueField = "pais"
                Paises.DataBind()
            End Using


            Dim filtro As String = ""


            If Session("DBAccesos") = "terrestre" Then
                filtro = ", PerfilColgate"
            End If


            Dim paises_str As String
            Dim strArr() As String
            Dim count As Integer

            Tipo.SelectedIndex = -1

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "SELECT  OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate" + filtro + " FROM Operators WHERE OperatorID = @OperatorID"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then

                    codigo.Value = dataread(0)
                    Login.Text = dataread(1)
                    Tipo.SelectedValue = dataread(7)
                    Nombre.Text = dataread(2)
                    Apellido.Text = dataread(3)
                    Email.Text = dataread(4)
                    Telefono.Text = dataread(5)
                    Puesto.Text = dataread(6)
                    Firma.Text = dataread(10)
                    paises_str = dataread(8)
                    Activo.Checked = dataread(9)

                    If Session("DBAccesos") = "terrestre" Then
                        PerfilColgate.Checked = dataread("PerfilColgate")
                    End If

                    Dim countries As String = ""
                    For Each li As ListItem In Paises.Items 'set completo de paises
                        strArr = paises_str.Split(",") 'array en base de datos
                        For count = 0 To strArr.Length - 1
                            If Comillas & Trim(li.Value) & Comillas = strArr(count) Then
                                li.Selected = True
                            End If
                        Next
                    Next

                    Session("insert") = False

                End If

            End Using


            If Session("insert") = True Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    sql1 = "SELECT * FROM usuarios_empresas WHERE id_usuario = @codigo"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        codigo.Value = dataread(0)
                        Login.Text = dataread(1)
                        Nombre.Text = dataread(5)
                        Email.Text = dataread(1) & "@" & dataread(10)
                        Firma.Text = dataread(5)

                        'Tipo.SelectedValue = dataread(8)
                        'strArr = dataread(5).ToString.Split(" ")
                        'Nombre.Text = strArr(0)
                        'If strArr.Length > 1 Then
                        '    Apellido.Text = strArr(1)
                        'Else
                        '    strArr = dataread(5).ToString.Split("-")
                        '    Nombre.Text = strArr(0)
                        '    If strArr.Length > 1 Then
                        '        Apellido.Text = strArr(1)
                        '    End If
                        'End If

                    End If

                End Using

            End If



        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try





    End Sub





    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar(False)
    End Sub

    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        activar(True)
    End Sub

    Private Sub activar(ByVal active As Boolean)
        Try
            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "UPDATE Operators SET Active=@active WHERE OperatorID = @codigo"

                Dim operacion As String

                If active = True Then
                    operacion = "activo"
                    'sql1 = "UPDATE Operators SET Active=@active WHERE OperatorID = @codigo"
                Else
                    operacion = "desactivo"
                    'sql1 = "UPDATE Operators SET Active=@active, Countries=@countries, OperatorLevel=@level WHERE OperatorID = @codigo"
                End If

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.String).Value = codigo.Value
                comm.Parameters.Add("@active", MySqlDbType.Int16).Value = active
                comm.Parameters.Add("@countries", MySqlDbType.VarString).Value = ""
                comm.Parameters.Add("@level", MySqlDbType.Int16).Value = -1
                conn.Open()
                comm.ExecuteNonQuery()

                Dim items As New Dictionary(Of String, String)
                items.Add("OperatorID", codigo.Value)
                items.Add("active", active)

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "users", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

                msg = "Registro se " & operacion & " correctamente"

            End Using


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

        LeerRegistro()

    End Sub


    Protected Sub graba(operacion As String)

        Try

            Dim countries As String = ""
            For Each li As ListItem In Paises.Items
                If li.Selected = True Then
                    If countries <> "" Then
                        countries = countries & ","
                    End If
                    countries = countries & Comillas & Trim(li.Value) & Comillas
                End If
            Next

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                Dim filtro As String = ""

                If Session("DBAccesos") = "terrestre" Then
                    filtro = ", PerfilColgate=@PerfilColgate"
                End If


                If operacion = "update" Then
                    msg = "Registro Actualizado correctamente"
                    sql1 = "UPDATE Operators SET Login=@login, FirstName=@firstname, LastName=@lastname, Email=@email, Phone=@phone, Position=@position, OperatorLevel=@level, Countries=@countries, Sign=@sign" + filtro + " WHERE OperatorID = @OperatorID"
                Else
                    msg = "Registrado Creado correctamente"
                    sql1 = "INSERT INTO Operators (OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate, CreatedTime, StartTime, FinishTime) VALUES (@OperatorID, @login, @firstname, @lastname, @email, @phone, @position, @level, @countries, 1, @sign, CURDATE(), CURTIME(), CURTIME(), CURTIME())"
                End If

                Dim comm As New MySqlCommand(sql1, conn)

                If Session("DBAccesos") = "terrestre" And operacion = "update" Then
                    comm.Parameters.Add("@PerfilColgate", MySqlDbType.Int32).Value = PerfilColgate.Checked
                End If

                comm.Parameters.Add("@OperatorID", MySqlDbType.String).Value = codigo.Value
                comm.Parameters.Add("@login", MySqlDbType.String).Value = Login.Text
                comm.Parameters.Add("@firstname", MySqlDbType.String).Value = Nombre.Text
                comm.Parameters.Add("@lastname", MySqlDbType.String).Value = Apellido.Text
                comm.Parameters.Add("@email", MySqlDbType.String).Value = Email.Text
                comm.Parameters.Add("@phone", MySqlDbType.String).Value = Telefono.Text
                comm.Parameters.Add("@position", MySqlDbType.String).Value = Puesto.Text
                comm.Parameters.Add("@level", MySqlDbType.String).Value = Tipo.SelectedValue
                comm.Parameters.Add("@countries", MySqlDbType.String).Value = countries
                comm.Parameters.Add("@sign", MySqlDbType.String).Value = Firma.Text
                conn.Open()
                comm.ExecuteNonQuery()

                Dim items As New Dictionary(Of String, String)
                items.Add("OperatorID", codigo.Value)
                items.Add("Login", Login.Text)
                items.Add("FirstName", Nombre.Text)
                items.Add("LastName", Apellido.Text)
                items.Add("Email", Email.Text)
                items.Add("Phone", Telefono.Text)
                items.Add("Position", Puesto.Text)
                items.Add("OperatorLevel", Tipo.SelectedValue)
                items.Add("Countries", countries)
                items.Add("Active", Activo.Checked)
                items.Add("Sign", Firma.Text)

                log(Server, codigo.Value, operacion, "", items, "Operators", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


                Dim sistema As String = Session("DBAccesos_conn")
                Dim k As Int16 = sistema.IndexOf("aereo")
                If k > 0 Then
                    sistema = "aereo"
                Else

                    k = sistema.IndexOf("terrestre")
                    If k > 0 Then
                        sistema = "terrestre"

                    End If


                End If


                Dim pais_iso As String = connection.getEmpresaUser(codigo.Value, Server)
                connection.setEmpresaUser(sistema, codigo.Value, pais_iso, pais_iso, Server)


            End Using

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
        End Try

        LeerRegistro()

    End Sub




    Protected Sub btn_cancelar_Click(sender As Object, e As System.EventArgs) Handles btn_cancelar.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Response.Redirect("Default.aspx")
    End Sub


End Class
