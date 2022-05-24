Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_ManifiestosGT

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

            user_id.Text = Nothing
            Activo.Checked = False


            'Paises.Items.Clear()
            'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
            'sql1 = "SELECT id_empresa as pais, nombre_empresa as nombre FROM empresas WHERE activo = 't' AND pais_iso ILIKE 'GT%' ORDER BY nombre_empresa"
            'Dim ds As New DataSet()
            'Dim cmd As New NpgsqlCommand(sql1, conn)
            'Dim adp As New NpgsqlDataAdapter(cmd)
            'adp.Fill(ds)
            'Paises.DataSource = ds
            'Paises.DataTextField = "nombre"
            'Paises.DataValueField = "pais"
            'Paises.DataBind()
            'End Using

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server)) ' Session("DBAccesos_conn"))

                sql1 = "SELECT userid, username, password, firstname, lastname, admin, aereo, marino, id_usuario FROM aausers WHERE id_usuario = @codigo"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    user_id.Text = dataread(0)
                    codigo.Text = dataread(8)
                    'Nombre.Text = dataread(2)
                    Login.Text = dataread(1)
                    'Pais.Text = dataread(4)
                    If dataread(5) = True Or dataread(6) = True Or dataread(7) = True Then
                        Activo.Checked = True
                    Else
                        Activo.Checked = False
                    End If

                    modulos.Items(0).Selected = dataread(5) 'admin
                    modulos.Items(1).Selected = dataread(6) 'aereo
                    modulos.Items(2).Selected = dataread(7) 'maritimo

                    Session("insert") = False
                End If
                dataread.Close()


                sql1 = "SELECT * FROM usuarios_x_empresa WHERE id_usuario=@codigo"
                comm = New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                dataread = comm.ExecuteReader
                While dataread.Read()
                    For Each li As ListItem In Paises.Items
                        If li.Value = dataread(2) Then
                            li.Selected = True
                        End If
                    Next
                End While
                dataread.Close()

                For Each li As ListItem In CheckBoxList1.Items
                    li.Selected = False
                Next

                For Each li As ListItem In CheckBoxList3.Items
                    li.Selected = False
                Next

                For Each li As ListItem In CheckBoxList5.Items
                    li.Selected = False
                Next


                sql1 = "SELECT * FROM usuarios_permisos_manifiestos WHERE id_usuario=@codigo"
                comm = New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                dataread = comm.ExecuteReader
                While dataread.Read()

                    If dataread(2) = 32 Then 'tla
                        For Each li As ListItem In CheckBoxList5.Items
                            If li.Value = dataread(3) Then
                                li.Selected = True
                            End If
                        Next
                    End If

                    If dataread(2) = 1 Then 'aimar
                        For Each li As ListItem In CheckBoxList1.Items
                            If li.Value = dataread(3) Then
                                li.Selected = True
                            End If
                        Next
                    End If

                    If dataread(2) = 15 Then 'latin
                        For Each li As ListItem In CheckBoxList3.Items
                            If li.Value = dataread(3) Then
                                li.Selected = True
                            End If
                        Next
                    End If

                    If dataread(2) = 29 Then
                        For Each li As ListItem In CheckBoxList2.Items
                            If li.Value = dataread(3) Then
                                li.Selected = True
                            End If
                        Next
                    End If



                    If dataread(2) = 40 Then
                        For Each li As ListItem In CheckBoxList4.Items
                            If li.Value = dataread(3) Then
                                li.Selected = True
                            End If
                        Next
                    End If



                End While
                dataread.Close()

            End Using


            If Session("insert") = True Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio FROM usuarios_empresas WHERE id_usuario = @codigo"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        codigo.Text = dataread(0)
                        Login.Text = dataread(1)
                        'Nombre.Text = dataread(2)
                        'Pais.Text = dataread(3)
                    End If

                    Session("insert") = True

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


    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        'activar(True)
        graba("activar")
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar(False)
    End Sub


    Protected Sub activar(ByVal activo As Boolean)

        If 1 = 2 Then

            Try

                Dim operacion As String

                Using conn As New NpgsqlConnection(Session("DBAccesos_conn"))

                    conn.Open()

                    If activo = True Then
                        operacion = "activo"
                        sql1 = "UPDATE manifiestos_usuarios SET Activo=@Activo WHERE id_master=@id_master"
                    Else
                        operacion = "desactivo"
                        'sql1 = "UPDATE manifiestos_usuarios SET Activo=@Activo, Aereo=@Aereo, Maritimo=@Maritimo, Terrestre=@Terrestre, Aduanas=@Aduanas WHERE id_master=@id_master"
                        sql1 = "UPDATE manifiestos_usuarios SET Activo=@Activo WHERE id_master=@id_master"
                    End If

                    msg = "Registro se " & operacion & " correctamente"

                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    comm.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
                    'comm.Parameters.Add("@Aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
                    'comm.Parameters.Add("@Maritimo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
                    'comm.Parameters.Add("@Terrestre", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
                    'comm.Parameters.Add("@Aduanas", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
                    comm.ExecuteNonQuery()

                End Using

                Dim items As New Dictionary(Of String, String)
                items.Clear()
                items.Add("id_master", Session("DBAccesosUserId"))
                items.Add("Activo", activo)

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            Catch ex As Exception
                msg = ex.Message
                img = icon_err_active
                css = "alert-default"
            End Try

            LeerRegistro()
        End If

        LeerRegistro()


    End Sub



    Protected Sub graba(ByVal operacion As String)

        Dim trns As NpgsqlTransaction = Nothing

        Try

            Dim items As New Dictionary(Of String, String)
            items.Clear()

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server)) 'Session("DBAccesos_conn"))

                conn.Open()
                If Session("insert") = True Then
                    'operacion = "insert"
                    sql1 = "SELECT userid, username, password, firstname, lastname, admin, aereo, marino, id_usuario FROM v WHERE id_usuario = @codigo"

                    sql1 = "INSERT INTO aausers (username, password, firstname, lastname, admin, aereo, marino, id_usuario) VALUES (@username, @password, @firstname, @lastname, @admin, @aereo, @marino, @id_usuario)"
                    msg = "Registro creado correctamente"
                    items.Add("Activo", True)
                Else
                    'operacion = "update" userid=@userid, 
                    sql1 = "UPDATE aausers SET username=@username, password=@password, firstname=@firstname, lastname=@lastname, admin=@admin, aereo=@aereo, marino=@marino " ', id_usuario=@id_usuario"
                    msg = "Registro actualizado correctamente"
                    If operacion = "activar" Then
                        'sql1 = sql1 & ", Activo=@Activo"
                        msg = "Registro se actualizado correctamente"
                        items.Add("Activo", True)
                    End If

                    sql1 = sql1 & " WHERE id_usuario=@id_usuario"

                End If

                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                comm.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "" 'Pais.Text
                comm.Parameters.Add("@firstname", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                comm.Parameters.Add("@lastname", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
                comm.Parameters.Add("@admin", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(0).Selected
                comm.Parameters.Add("@aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(1).Selected
                comm.Parameters.Add("@marino", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(2).Selected
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                'comm.Parameters.Add("@userid", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user_id.Text
                comm.ExecuteNonQuery()
            End Using

            items.Add("id_usuario", Session("DBAccesosUserId"))
            items.Add("firstname", Nombre.Text)
            items.Add("username", Login.Text)
            'items.Add("idpais", Pais.Text)
            items.Add("admin", modulos.Items(0).Selected)
            items.Add("aereo", modulos.Items(1).Selected)
            items.Add("marino", modulos.Items(2).Selected)
            'items.Add("Aduanas", modulos.Items(3).Selected)

            log(Server, Session("DBAccesosUserId"), operacion, "", items, "aausers", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server)) 'Session("DBAccesos_conn"))
                conn.Open()

                trns = conn.BeginTransaction()

                sql1 = "DELETE FROM usuarios_x_empresa WHERE id_usuario = @login"
                Dim comm0 As New NpgsqlCommand(sql1, conn)
                comm0.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")  'Session("DBAccesosLogin")                
                comm0.ExecuteNonQuery()

                sql1 = "DELETE FROM usuarios_permisos_manifiestos WHERE id_usuario = @login"
                comm0 = New NpgsqlCommand(sql1, conn)
                comm0.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")  'Session("DBAccesosLogin")                
                comm0.ExecuteNonQuery()

                Dim go As Boolean
                For Each li As ListItem In Paises.Items

                    If li.Selected = True Then

                        sql1 = "INSERT INTO usuarios_x_empresa (id_usuario, id_empresa, fecha_creado, id_menu) VALUES (@login, @empresa, CURRENT_DATE, 0)"
                        'sql1 = "INSERT INTO usuarios_x_empresa (id_usuario, id_empresa) VALUES (@login, @empresa)"
                        Dim comm2 As New NpgsqlCommand(sql1, conn)
                        comm2.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        comm2.Parameters.Add("@empresa", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li.Value
                        comm2.ExecuteNonQuery()

                        If li.Value = 1 Then

                            CheckBoxList1.Items(0).Selected = modulos.Items(0).Selected 'aereo
                            CheckBoxList1.Items(1).Selected = modulos.Items(1).Selected 'maritimo

                            For Each li2 As ListItem In CheckBoxList1.Items
                                If li2.Selected = True Then

                                    go = False

                                    If modulos.Items(0).Selected And (li2.Value = 1 Or li2.Value = 6 Or li2.Value = 7 Or li2.Value = 8) Then
                                        go = True
                                    End If

                                    If modulos.Items(1).Selected And (li2.Value = 2 Or li2.Value = 3 Or li2.Value = 4) Then
                                        go = True
                                    End If

                                    If go Then
                                        sql1 = "INSERT INTO usuarios_permisos_manifiestos (id_usuario, id_empresa, id_menu, fecha_creado) VALUES (@login, @empresa, @menu, now())"
                                        comm2 = New NpgsqlCommand(sql1, conn)
                                        comm2.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                                        comm2.Parameters.Add("@empresa", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li.Value
                                        comm2.Parameters.Add("@menu", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li2.Value
                                        comm2.ExecuteNonQuery()
                                    End If

                                End If

                            Next
                        End If

                        If li.Value = 29 Then
                            For Each li2 As ListItem In CheckBoxList2.Items
                                If li2.Selected = True Then

                                    sql1 = "INSERT INTO usuarios_permisos_manifiestos (id_usuario, id_empresa, id_menu, fecha_creado) VALUES (@login, @empresa, @menu, now())"
                                    comm2 = New NpgsqlCommand(sql1, conn)
                                    comm2.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                                    comm2.Parameters.Add("@empresa", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li.Value
                                    comm2.Parameters.Add("@menu", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li2.Value
                                    comm2.ExecuteNonQuery()

                                End If

                            Next
                        End If

                        If li.Value = 15 Then

                            CheckBoxList3.Items(0).Selected = modulos.Items(0).Selected 'aereo
                            CheckBoxList3.Items(1).Selected = modulos.Items(1).Selected 'maritimo

                            For Each li2 As ListItem In CheckBoxList3.Items
                                If li2.Selected = True Then
                                    go = False

                                    If modulos.Items(0).Selected And (li2.Value = 1 Or li2.Value = 6 Or li2.Value = 7 Or li2.Value = 8) Then
                                        go = True
                                    End If

                                    If modulos.Items(1).Selected And (li2.Value = 2 Or li2.Value = 3 Or li2.Value = 4) Then
                                        go = True
                                    End If

                                    If go Then

                                        sql1 = "INSERT INTO usuarios_permisos_manifiestos (id_usuario, id_empresa, id_menu, fecha_creado) VALUES (@login, @empresa, @menu, now())"
                                        comm2 = New NpgsqlCommand(sql1, conn)
                                        comm2.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                                        comm2.Parameters.Add("@empresa", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li.Value
                                        comm2.Parameters.Add("@menu", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li2.Value
                                        comm2.ExecuteNonQuery()
                                    End If

                                End If

                            Next
                        End If

                        If li.Value = 40 Then



                            For Each li2 As ListItem In CheckBoxList4.Items
                                If li2.Selected = True Then

                                    sql1 = "INSERT INTO usuarios_permisos_manifiestos (id_usuario, id_empresa, id_menu, fecha_creado) VALUES (@login, @empresa, @menu, now())"
                                    comm2 = New NpgsqlCommand(sql1, conn)
                                    comm2.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                                    comm2.Parameters.Add("@empresa", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li.Value
                                    comm2.Parameters.Add("@menu", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li2.Value
                                    comm2.ExecuteNonQuery()

                                End If

                            Next

                        End If

                        If li.Value = 32 Then

                            CheckBoxList5.Items(0).Selected = modulos.Items(0).Selected 'aereo
                            CheckBoxList5.Items(1).Selected = modulos.Items(1).Selected 'maritimo

                            For Each li2 As ListItem In CheckBoxList5.Items
                                If li2.Selected = True Then

                                    go = False

                                    If modulos.Items(0).Selected And (li2.Value = 1 Or li2.Value = 6 Or li2.Value = 7 Or li2.Value = 8) Then
                                        go = True
                                    End If

                                    If modulos.Items(1).Selected And (li2.Value = 2 Or li2.Value = 3 Or li2.Value = 4) Then
                                        go = True
                                    End If

                                    If go Then
                                        sql1 = "INSERT INTO usuarios_permisos_manifiestos (id_usuario, id_empresa, id_menu, fecha_creado) VALUES (@login, @empresa, @menu, now())"
                                        comm2 = New NpgsqlCommand(sql1, conn)
                                        comm2.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                                        comm2.Parameters.Add("@empresa", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li.Value
                                        comm2.Parameters.Add("@menu", NpgsqlTypes.NpgsqlDbType.Bigint).Value = li2.Value
                                        comm2.ExecuteNonQuery()
                                    End If

                                End If

                            Next
                        End If


                    End If

                Next

                trns.Commit()


            End Using

        Catch ex As Exception
            'trns.Rollback()

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
