Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_Manifiestos

    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off
    Public Licon_del As String = icon_del

    Public Licon_open As String = icon_open
    Public Licon_new As String = icon_new

    Public sql1 As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Session("insert") = False

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

            Session("insert") = False

            user_id.Text = Nothing
            Activo.Checked = False

            'Dim sql1 As String

            Dim paises_str As String = ""

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    id_master.Text = dataread(0)
                    Login.Text = dataread(1)
                    Nombre.Text = dataread(2)
                    Pais.Text = dataread(3)
                End If
                dataread.Close()


                sql1 = "SELECT '' AS country, ' Seleccione Empresa' AS nombre_empresa UNION SELECT country, '<img src=Content/flags/' || substring(country for 2) || '-flag.gif height=16 /> ' || nombre_empresa FROM empresas_parametros WHERE tica_empresa = 't' AND activo = 't' ORDER BY country"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                CheckListEmpresas.DataSource = ds
                CheckListEmpresas.DataValueField = "country"
                CheckListEmpresas.DataTextField = "nombre_empresa"
                CheckListEmpresas.DataBind()

            End Using


            For Each li As ListItem In CheckListEmpresas.Items

                If li.Value <> "" Then

                    Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & li.Value.ToLower)

                        sql1 = "SELECT id_master, nombres, activo FROM manifiestos_usuarios WHERE id_master = " & id_master.Text

                        Dim comm As New NpgsqlCommand(sql1, conn)
                        'comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        conn.Open()
                        Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                        If dataread.Read() Then
                            'If dataread(2) = 1 Then
                            li.Selected = True
                                paises_str = paises_str & Comillas & Trim(li.Value) & Comillas & ","
                            'End If

                        End If
                        dataread.Close()

                    End Using

                End If

            Next

            If paises_str <> "" Then
                paises_str = paises_str.Remove(paises_str.Length - 1, 1)
            Else
                paises_str = "''"
            End If


            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                sql1 = "SELECT '' AS country, ' Seleccione Empresa' AS nombre_empresa UNION SELECT country, '<img src=Content/flags/' || substring(country for 2) || '-flag.gif height=16 /> ' || nombre_empresa FROM empresas_parametros WHERE tica_empresa = 't' AND activo = 't' AND country IN (" & paises_str & ") ORDER BY country"

                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                RadioListEmpAsignada.DataSource = ds
                RadioListEmpAsignada.DataValueField = "country"
                RadioListEmpAsignada.DataTextField = "nombre_empresa"
                RadioListEmpAsignada.DataBind()
            End Using



            Try

                If RadioListEmpAsignada.Items.Count > 0 Then
                    RadioListEmpAsignada.SelectedIndex = 0
                    If Session("pais") <> Nothing Then
                        RadioListEmpAsignada.SelectedValue = Session("pais")
                    Else
                        sel_db(RadioListEmpAsignada.SelectedValue, RadioListEmpAsignada.SelectedItem.Text)
                    End If
                End If

                If RadioButtonList2.Items.Count > 0 Then

                    If RadioListEmpAsignada.Items.Count = 0 Then

                        If Session("pais") = Nothing Then
                            sel_db(RadioListEmpAsignada.SelectedValue, RadioListEmpAsignada.SelectedItem.Text)
                        End If

                    End If

                    If RadioListEmpAsignada.SelectedIndex = 0 Then
                        If Session("pais") <> Nothing Then
                            RadioButtonList2.SelectedValue = Session("pais")
                        End If
                    End If

                End If





            Catch ex As Exception
                'Perfil.Items.Clear()
                Session("pais") = Nothing
            End Try





        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"

        End Try

    End Sub


    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub


    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click

        Try


            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & Session("pais").ToString.ToLower)

                conn.Open()

                sql1 = "DELETE FROM manifiestos_usuarios WHERE id_master=@id_master"

                msg = "Registro se " & " Borrado " & " correctamente"

                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm.ExecuteNonQuery()

            End Using

            Dim items As New Dictionary(Of String, String)
            items.Clear()
            items.Add("id_master", Session("DBAccesosUserId"))


            log(Server, Session("DBAccesosUserId"), " Borrado ", "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

        LeerRegistro()

    End Sub




    'Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
    '    graba("insert")
    'End Sub



    'Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
    '    activar(True)
    '    'graba("activar")
    'End Sub



    'Protected Sub activar(ByVal activo1 As Boolean)
    '    Try

    '        Dim operacion As String

    '        Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & Session("pais").ToString.ToLower)

    '            conn.Open()

    '            If activo1 = True Then
    '                operacion = "activo"
    '                sql1 = "UPDATE manifiestos_usuarios SET Activo=@Activo WHERE id_master=@id_master"
    '            Else
    '                operacion = "desactivo"
    '                'sql1 = "UPDATE manifiestos_usuarios SET Activo=@Activo, Aereo=@Aereo, Maritimo=@Maritimo, Terrestre=@Terrestre, Aduanas=@Aduanas WHERE id_master=@id_master"
    '                sql1 = "UPDATE manifiestos_usuarios SET Activo=@Activo WHERE id_master=@id_master"
    '            End If

    '            msg = "Registro se " & operacion & " correctamente"

    '            Dim comm As New NpgsqlCommand(sql1, conn)
    '            comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
    '            comm.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo1
    '            'comm.Parameters.Add("@Aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
    '            'comm.Parameters.Add("@Maritimo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
    '            'comm.Parameters.Add("@Terrestre", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
    '            'comm.Parameters.Add("@Aduanas", NpgsqlTypes.NpgsqlDbType.Boolean).Value = activo
    '            comm.ExecuteNonQuery()
    '            Activo.Checked = activo1
    '        End Using

    '        Dim items As New Dictionary(Of String, String)
    '        items.Clear()
    '        items.Add("id_master", Session("DBAccesosUserId"))
    '        items.Add("Activo", activo1)

    '        log(Server, Session("DBAccesosUserId"), operacion, "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

    '    Catch ex As Exception
    '        msg = ex.Message
    '        img = icon_err_active
    '        css = "alert-default"
    '    End Try

    '    LeerRegistro()

    'End Sub



    Protected Sub graba(ByVal operacion As String)
        Try

            Dim items As New Dictionary(Of String, String)
            items.Clear()

            'Dim countries As String = ""
            'Dim countries2 As String = ""

            'For Each li As ListItem In CheckListEmpresas.Items()
            '    If li.Selected = True Then
            '        If li.Value <> "" Then
            '            countries = countries & Comillas & Trim(li.Value) & Comillas & ","
            '            countries2 = countries2 & Comillas & Trim(li.Value) & Comillas & ","
            '        End If
            '    End If
            'Next

            'If countries <> "" Then
            '    countries = countries.Remove(countries.Length - 1, 1)
            'Else
            '    countries = "''"
            'End If

            'If countries2 <> "" Then
            '    countries2 = countries2.Remove(countries2.Length - 1, 1)
            'Else
            '    countries2 = "''"
            'End If


            For Each li As ListItem In CheckListEmpresas.Items()
                If li.Selected = True Then
                    If (li.Value <> "" And Session("pais") = Nothing) Or Session("pais") = li.Value Then


                        Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & li.Value.ToLower)

                            sql1 = "SELECT user_id, id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @codigo"
                            Dim comm As New NpgsqlCommand(sql1, conn)
                            comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                            conn.Open()
                            Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                            If dataread.Read() Then

                                sql1 = "UPDATE manifiestos_usuarios SET nombres=@nombres, usuario=@usuario, idpais=@idpais, Aereo=@Aereo, Maritimo=@Maritimo, Terrestre=@Terrestre, Aduanas=@Aduanas, Activo=@Activo"
                                msg = "Registro actualizado correctamente"
                                sql1 = sql1 & " WHERE id_master=@id_master"
                            Else
                                sql1 = "INSERT INTO manifiestos_usuarios (id_master, nombres, usuario, idpais, Activo, Aereo, Maritimo, Terrestre, Aduanas) VALUES (@id_master, @nombres, @usuario, @idpais, @Activo, @Aereo, @Maritimo, @Terrestre, @Aduanas)"
                                msg = "Registro creado correctamente"

                            End If
                            dataread.Close()

                            If msg = "Registro actualizado correctamente" And Session("pais") = Nothing Then


                            Else

                                comm = New NpgsqlCommand(sql1, conn)
                                comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                                comm.Parameters.Add("@nombres", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                                comm.Parameters.Add("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                                comm.Parameters.Add("@idpais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Pais.Text
                                comm.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = Activo.Checked
                                comm.Parameters.Add("@Aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(0).Selected
                                comm.Parameters.Add("@Maritimo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(1).Selected
                                comm.Parameters.Add("@Terrestre", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(2).Selected
                                comm.Parameters.Add("@Aduanas", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(3).Selected
                                comm.ExecuteNonQuery()

                            End If



                        End Using

                        If msg = "Registro actualizado correctamente" And Session("pais") = Nothing Then


                        Else
                            items.Add("id_master", Session("DBAccesosUserId"))
                            items.Add("nombres", Nombre.Text)
                            items.Add("usuario", Login.Text)
                            items.Add("idpais", Pais.Text)
                            items.Add("Aereo", modulos.Items(0).Selected)
                            items.Add("Maritimo", modulos.Items(1).Selected)
                            items.Add("Terrestre", modulos.Items(2).Selected)
                            items.Add("Aduanas", modulos.Items(3).Selected)

                            log(Server, Session("DBAccesosUserId"), operacion, "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))
                        End If

                    End If
                End If
            Next




            'Using conn As New NpgsqlConnection(Session("DBAccesos_conn"))

            '    conn.Open()
            '    If Session("insert") = True Then
            '        'operacion = "insert"
            '        sql1 = "INSERT INTO manifiestos_usuarios (id_master, nombres, usuario, idpais, Activo, Aereo, Maritimo, Terrestre, Aduanas) VALUES (@id_master, @nombres, @usuario, @idpais, @Activo, @Aereo, @Maritimo, @Terrestre, @Aduanas)"
            '        msg = "Registro creado correctamente"
            '        items.Add("Activo", True)
            '    Else
            '        'operacion = "update"
            '        sql1 = "UPDATE manifiestos_usuarios SET nombres=@nombres, usuario=@usuario, idpais=@idpais, Aereo=@Aereo, Maritimo=@Maritimo, Terrestre=@Terrestre, Aduanas=@Aduanas"
            '        msg = "Registro actualizado correctamente"
            '        If operacion = "activar" Then
            '            sql1 = sql1 & ", Activo=@Activo"
            '            msg = "Registro se activo correctamente"
            '            items.Add("Activo", True)
            '        End If

            '        sql1 = sql1 & " WHERE id_master=@id_master"

            '    End If

            '    Dim comm As New NpgsqlCommand(sql1, conn)
            '    comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
            '    comm.Parameters.Add("@nombres", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
            '    comm.Parameters.Add("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
            '    comm.Parameters.Add("@idpais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Pais.Text
            '    comm.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = True
            '    comm.Parameters.Add("@Aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(0).Selected
            '    comm.Parameters.Add("@Maritimo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(1).Selected
            '    comm.Parameters.Add("@Terrestre", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(2).Selected
            '    comm.Parameters.Add("@Aduanas", NpgsqlTypes.NpgsqlDbType.Boolean).Value = modulos.Items(3).Selected
            '    comm.ExecuteNonQuery()
            'End Using

            'items.Add("id_master", Session("DBAccesosUserId"))
            'items.Add("nombres", Nombre.Text)
            'items.Add("usuario", Login.Text)
            'items.Add("idpais", Pais.Text)
            'items.Add("Aereo", modulos.Items(0).Selected)
            'items.Add("Maritimo", modulos.Items(1).Selected)
            'items.Add("Terrestre", modulos.Items(2).Selected)
            'items.Add("Aduanas", modulos.Items(3).Selected)

            'log(Server, Session("DBAccesosUserId"), operacion, "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

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




    Protected Sub sel_db(ByVal dbname As String, ByVal flag As String)
        Session("pais") = dbname
    End Sub



    Protected Sub RadioListEmpAsignada_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioListEmpAsignada.SelectedIndexChanged

        If RadioListEmpAsignada.SelectedIndex > -1 Then
            'RadioButtonList2.SelectedIndex = 0
            sel_db(RadioListEmpAsignada.SelectedValue, RadioListEmpAsignada.SelectedItem.Text)
            FillBodegas()
        End If

    End Sub

    Protected Sub RadioButtonList2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonList2.SelectedIndexChanged

        If RadioButtonList2.SelectedIndex > -1 Then
            RadioListEmpAsignada.SelectedIndex = 0
            sel_db(RadioButtonList2.SelectedValue, RadioButtonList2.SelectedItem.Text)
            FillBodegas()
        End If

    End Sub




    Protected Sub FillBodegas()

        Try
            'Activo.Checked = False
            'Perfil.Items.Clear()

            'Session("insert") = True

            If Session("DBAccesos") <> Nothing And Session("pais") <> Nothing Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & Session("pais").ToString.ToLower)

                    sql1 = "SELECT user_id, id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @codigo"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        user_id.Text = dataread(0)
                        id_master.Text = dataread(1)
                        Nombre.Text = dataread(2)
                        Login.Text = dataread(3)
                        Pais.Text = dataread(4)
                        If dataread(5) = True Then
                            Activo.Checked = True
                        Else
                            Activo.Checked = False
                        End If

                        modulos.Items(0).Selected = dataread(6)
                        modulos.Items(1).Selected = dataread(7)
                        modulos.Items(2).Selected = dataread(8)
                        modulos.Items(3).Selected = dataread(9)

                        'Session("insert") = False
                    End If
                    dataread.Close()

                End Using

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub


End Class

