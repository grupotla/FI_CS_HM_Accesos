Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs


Partial Class mn_Caja_ahorros
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

            Dim count As Integer
            Paises.Items.Clear()
            Tipo.Items.Clear()
            Tipo.SelectedIndex = -1

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "SELECT idpais, CONCAT('<img src=Content/flags/',idpais,'-flag.gif height=16 /> ',pais) as nom FROM paises ORDER BY pais"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(sql1, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                Paises.DataSource = ds
                Paises.DataTextField = "nom"
                Paises.DataValueField = "idpais"
                Paises.DataBind()

                sql1 = "SELECT rol_id, rango FROM roles ORDER BY rango"
                Dim ds1 As New DataSet()
                Dim cmd1 As New MySqlCommand(sql1, conn)
                Dim adp1 As New MySqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Tipo.DataSource = ds1
                Tipo.DataTextField = "rango"
                Tipo.DataValueField = "rol_id"
                Tipo.DataBind()

                sql1 = "SELECT user_id, nombres, email, username, password, fecha_ingreso, activo, idpais, rol_id, bz, cr, sv, gt, hn, ni, pa, id_master FROM usuarios WHERE id_master = @codigo"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then

                    user_id.Text = dataread(0)
                    codigo.Value = dataread(16)
                    Nombre.Text = dataread(1)
                    Email.Text = dataread(2)
                    Login.Text = dataread(3)
                    Password.Text = dataread(4)
                    Activo.Checked = dataread(6)
                    Pais.Text = dataread(7)
                    Tipo.SelectedValue = dataread(8)

                    count = 8
                    For Each li As ListItem In Paises.Items
                        count = count + 1
                        li.Selected = dataread(count)
                    Next

                    Session("insert") = False

                End If

            End Using

            If Session("insert") = True Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    sql1 = "SELECT id_usuario, pw_name, pw_passwd, pw_uid, pw_gid, pw_gecos, pw_dir, pw_shell, tipo_usuario, pais, dominio, level, pw_activo, pw_codigo_tributario, pw_correo, id_usuario_reg, modificado FROM usuarios_empresas WHERE id_usuario=@codigo"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        codigo.Value = Nothing
                        Nombre.Text = dataread(5)
                        Email.Text = dataread(1) & "@" & dataread(10)
                        Login.Text = dataread(1)
                        Password.Text = dataread(2)
                        'Activo.Checked = True
                        Pais.Text = dataread(9)
                        'Tipo.SelectedValue = Nothing
                    End If

                End Using

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub




    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        'activar(True)
        graba("activar")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar(False)
    End Sub

    Protected Sub activar(ByVal activo As Boolean)
        Try
            Using conn As New MySqlConnection(Session("DBAccesos_conn"))
                'sql1 = "UPDATE usuarios SET activo=@activo, gt=@gt, bz=@bz, sv=@sv, hn=@hn, ni=@ni, cr=@cr, pa=@pa WHERE id_master = @codigo"
                sql1 = "UPDATE usuarios SET activo=@activo WHERE id_master = @codigo"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.String).Value = Session("DBAccesosUserId")
                comm.Parameters.Add("@activo", MySqlDbType.Int16).Value = activo
                'comm.Parameters.Add("@bz", MySqlDbType.Int16).Value = False
                'comm.Parameters.Add("@cr", MySqlDbType.Int16).Value = False
                'comm.Parameters.Add("@sv", MySqlDbType.Int16).Value = False
                'comm.Parameters.Add("@gt", MySqlDbType.Int16).Value = False
                'comm.Parameters.Add("@hn", MySqlDbType.Int16).Value = False
                'comm.Parameters.Add("@ni", MySqlDbType.Int16).Value = False
                'comm.Parameters.Add("@pa", MySqlDbType.Int16).Value = False

                conn.Open()
                comm.ExecuteNonQuery()

                Dim items As New Dictionary(Of String, String)
                items.Add("id_master", Session("DBAccesosUserId"))
                items.Add("user_id", user_id.Text)
                items.Add("activo", activo)
                log(Server, Session("DBAccesosUserId"), "desactivar", sql1, items, "usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))
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


            Dim items As New Dictionary(Of String, String)


            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                If operacion = "insert" Then
                    msg = "Registro Creado correctamente"
                    sql1 = "INSERT INTO usuarios (user_id, nombres, email, username, password, fecha_ingreso, activo, idpais, gt, bz, sv, hn, ni, cr, pa, id_master, rol_id) VALUES (null, @nombres, @email, @username, @password, now(), 1, @idpais, @gt, @bz, @sv, @hn, @ni, @cr, @pa, @id_master, @rol_id)"
                    items.Add("Active", 1)
                Else
                    msg = "Registro Actualizado correctamente"
                    sql1 = "UPDATE usuarios SET nombres=@nombres, email=@email, username=@username, password=@password, idpais=@idpais, rol_id=@rol_id, gt=@gt, bz=@bz, sv=@sv, hn=@hn, ni=@ni, cr=@cr, pa=@pa"

                    If operacion = "activar" Then
                        msg = "Registro se activo correctamente"
                        sql1 = sql1 & ", activo=@activo"
                        items.Add("Active", 1)
                    End If

                    sql1 = sql1 & " WHERE id_master=@id_master"
                End If

                Dim comm As New MySqlCommand(sql1, conn)

                comm.Parameters.Add("@user_id", MySqlDbType.Int32).Value = user_id.Text
                comm.Parameters.Add("@nombres", MySqlDbType.String).Value = Nombre.Text
                comm.Parameters.Add("@email", MySqlDbType.String).Value = Email.Text
                comm.Parameters.Add("@username", MySqlDbType.String).Value = Login.Text
                comm.Parameters.Add("@password", MySqlDbType.String).Value = Password.Text
                comm.Parameters.Add("@idpais", MySqlDbType.String).Value = Pais.Text
                comm.Parameters.Add("@rol_id", MySqlDbType.Int16).Value = Tipo.SelectedValue

                comm.Parameters.Add("@bz", MySqlDbType.Int16).Value = Paises.Items(0).Selected
                comm.Parameters.Add("@cr", MySqlDbType.Int16).Value = Paises.Items(1).Selected
                comm.Parameters.Add("@sv", MySqlDbType.Int16).Value = Paises.Items(2).Selected
                comm.Parameters.Add("@gt", MySqlDbType.Int16).Value = Paises.Items(3).Selected
                comm.Parameters.Add("@hn", MySqlDbType.Int16).Value = Paises.Items(4).Selected
                comm.Parameters.Add("@ni", MySqlDbType.Int16).Value = Paises.Items(5).Selected
                comm.Parameters.Add("@pa", MySqlDbType.Int16).Value = Paises.Items(6).Selected
                comm.Parameters.Add("@id_master", MySqlDbType.String).Value = Session("DBAccesosUserId")

                comm.Parameters.Add("@activo", MySqlDbType.Int16).Value = 1

                conn.Open()
                comm.ExecuteNonQuery()

                Items.Add("user_id", user_id.Text)
                items.Add("nombres", Nombre.Text)
                items.Add("username", Login.Text)
                items.Add("password", Password.Text)
                items.Add("idpais", Pais.Text)
                items.Add("rol_id", Tipo.SelectedValue)
                items.Add("bz", Paises.Items(0).Selected)
                items.Add("cr", Paises.Items(1).Selected)
                items.Add("sv", Paises.Items(2).Selected)
                items.Add("gt", Paises.Items(3).Selected)
                items.Add("hn", Paises.Items(4).Selected)
                items.Add("ni", Paises.Items(5).Selected)
                items.Add("pa", Paises.Items(6).Selected)
                items.Add("id_master", Session("DBAccesosUserId"))

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "Operators", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

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
