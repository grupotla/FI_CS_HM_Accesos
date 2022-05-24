Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_Pricing

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

            Dim sql1 As String

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("catalogo", Server))

                sql1 = "SELECT pais_iso as pais, '<img src=Content/flags/' || substring(pais_iso for 2) || '-flag.gif height=16 /> ' || nombre_empresa as nombre FROM empresas WHERE activo = 't' ORDER BY nombre_empresa"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                Paises.DataSource = ds
                Paises.DataTextField = "nombre"
                Paises.DataValueField = "pais"
                Paises.DataBind()


                sql1 = "SELECT id_tipo_usuario, nombre_tipo FROM tipos_usuario ORDER BY id_tipo_usuario"
                Dim ds2 As New DataSet()
                Dim cmd2 As New NpgsqlCommand(sql1, conn)
                Dim adp2 As New NpgsqlDataAdapter(cmd2)
                adp2.Fill(ds2)
                accSeguroTipo.DataSource = ds2
                accSeguroTipo.DataTextField = "nombre_tipo"
                accSeguroTipo.DataValueField = "id_tipo_usuario"
                accSeguroTipo.DataBind()

            End Using

            Dim paises_str As String = ""
            Dim strArr() As String
            Dim count As Integer

            accSeguroTipo.SelectedIndex = -1
            Activo.Checked = False
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("pricing", Server))

                sql1 = "SELECT tpu_pk, tpu_user_fk, tpu_tipo_user_fk, tpu_paises_iso, tpu_tps_fk, tpu_insert_user, tpu_insert_date, tpu_update_user, tpu_update_date FROM ti_pricing_user_perfil WHERE tpu_user_fk = @tpu_user_fk"

                Dim comm As New NpgsqlCommand(sql1, conn)

                comm.Parameters.Add("@tpu_user_fk", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")

                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    If dataread(4) = 0 Then
                        Activo.Checked = False
                    Else
                        Activo.Checked = True
                    End If
                    accSeguroTipo.SelectedValue = dataread(2)
                    paises_str = dataread(3)


                    Session("insert") = False
                End If
                dataread.Close()
            End Using


            Dim countries As String = ""
            For Each li As ListItem In Paises.Items 'set completo de paises
                strArr = paises_str.Split(",") 'array en base de datos
                For count = 0 To strArr.Length - 1
                    If Comillas & Trim(li.Value) & Comillas = strArr(count) Then
                        li.Selected = True
                    End If
                Next
            Next


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub


    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        activar(True)
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


    Protected Sub graba(ByVal operacion As String)
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


            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("pricing", Server))


                If Session("insert") = True Then
                    sql1 = "INSERT INTO ti_pricing_user_perfil (tpu_user_fk, tpu_tipo_user_fk, tpu_paises_iso, tpu_tps_fk, tpu_insert_user, tpu_insert_date, tpu_update_user) VALUES (@tpu_user_fk, @tpu_tipo_user_fk, @tpu_paises_iso, 1, @tpu_insert_user, now(), 0)"
                Else
                    sql1 = "UPDATE ti_pricing_user_perfil SET tpu_tipo_user_fk=@tpu_tipo_user_fk, tpu_paises_iso=@tpu_paises_iso, tpu_update_user=@tpu_update_user, tpu_update_date=now() WHERE tpu_user_fk = @tpu_user_fk"
                End If

                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()

                If Session("insert") = True Then
                    comm.Parameters.Add("@tpu_insert_user", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("OperatorID")
                Else
                    comm.Parameters.Add("@tpu_update_user", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("OperatorID")
                End If

                comm.CommandText = sql1

                comm.Parameters.Add("@tpu_paises_iso", NpgsqlTypes.NpgsqlDbType.Varchar).Value = countries

                comm.Parameters.Add("@tpu_user_fk", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")


                Select Case operacion
                    Case "insert"
                        msg = "Registro Creado correctamente"
                        comm.Parameters.Add("@tpu_tipo_user_fk", NpgsqlTypes.NpgsqlDbType.Integer).Value = accSeguroTipo.SelectedValue

                    Case "update"
                        msg = "Registro Actualizado correctamente"
                        comm.Parameters.Add("@tpu_tipo_user_fk", NpgsqlTypes.NpgsqlDbType.Integer).Value = accSeguroTipo.SelectedValue

                    Case "activar"
                        msg = "Registro se activo correctamente"
                        'accSeguroTipo.SelectedValue = 3 'operador
                        comm.Parameters.Add("@tpu_tipo_user_fk", NpgsqlTypes.NpgsqlDbType.Integer).Value = 3

                    Case "desactivar"
                        msg = "Registro se desactivo correctamente"
                        'accSeguroTipo.SelectedIndex = -1
                        comm.Parameters.Add("@tpu_tipo_user_fk", NpgsqlTypes.NpgsqlDbType.Integer).Value = 0

                End Select


                comm.ExecuteNonQuery()

                Dim items As New Dictionary(Of String, String)
                items.Clear()
                items.Add("tpu_user_fk", Session("DBAccesosLogin"))
                items.Add("tpu_tipo_user_fk", accSeguroTipo.SelectedValue)

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "ti_pricing_user_perfil", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

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




    Protected Sub activar(ByVal activo As Boolean)

        Try

            Dim operacion As String

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("pricing", Server))

                conn.Open()

                If activo = True Then
                    operacion = "activo"
                    sql1 = "UPDATE ti_pricing_user_perfil SET tpu_tps_fk=@Activo, tpu_update_user=@tpu_update_user, tpu_update_date=now() WHERE tpu_user_fk=@tpu_user_fk"
                Else
                    operacion = "desactivo"
                    sql1 = "UPDATE ti_pricing_user_perfil SET tpu_tps_fk=@Activo, tpu_update_user=@tpu_update_user, tpu_update_date=now() WHERE tpu_user_fk=@tpu_user_fk"
                End If

                msg = "Registro se " & operacion & " correctamente"

                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@tpu_update_user", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("OperatorID")
                comm.Parameters.Add("@tpu_user_fk", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Integer).Value = IIf(activo, 1, 0)

                comm.ExecuteNonQuery()

            End Using

            Dim items As New Dictionary(Of String, String)
            items.Clear()
            items.Add("tpu_user_fk", Session("DBAccesosUserId"))
            items.Add("Activo", activo)

            log(Server, Session("DBAccesosUserId"), operacion, "", items, "ti_pricing_user_perfil", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try


        LeerRegistro()


    End Sub

End Class
