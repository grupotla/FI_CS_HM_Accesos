Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_Seguros

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

            accSeguroTipo.SelectedIndex = -1
            Activo.Checked = False
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario, id_tipo_usuario FROM detalle_tipos_usuario WHERE id_usuario = '" & Session("DBAccesosUserId") & "' ORDER BY id_tipo_usuario DESC"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    If dataread(1) = 0 Then
                        Activo.Checked = False
                    Else
                        Activo.Checked = True
                    End If
                    accSeguroTipo.SelectedValue = dataread(1)
                    Session("insert") = False
                End If
                dataread.Close()
            End Using

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub


    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        graba("activar")
    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        graba("desactivar")
    End Sub


    Protected Sub graba(ByVal operacion As String)
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                If Session("insert") = True Then
                    sql1 = "INSERT INTO detalle_tipos_usuario VALUES (@id_usuario, @id_tipo_usuario)"
                Else
                    sql1 = "UPDATE detalle_tipos_usuario SET id_tipo_usuario=@id_tipo_usuario WHERE id_usuario = @id_usuario"
                End If

                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.CommandText = sql1
                conn.Open()
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")

                Select Case operacion
                    Case "insert"
                        msg = "Registro Creado correctamente"
                        comm.Parameters.Add("@id_tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = accSeguroTipo.SelectedValue

                    Case "update"
                        msg = "Registro Actualizado correctamente"
                        comm.Parameters.Add("@id_tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = accSeguroTipo.SelectedValue

                    Case "activar"
                        msg = "Registro se activo correctamente"
                        'accSeguroTipo.SelectedValue = 3 'operador
                        comm.Parameters.Add("@id_tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = 3

                    Case "desactivar"
                        msg = "Registro se desactivo correctamente"
                        'accSeguroTipo.SelectedIndex = -1
                        comm.Parameters.Add("@id_tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = 0

                End Select


                comm.ExecuteNonQuery()

                Dim items As New Dictionary(Of String, String)
                items.Clear()
                items.Add("id_usuario", Session("DBAccesosLogin"))
                items.Add("id_tipo_usuario", accSeguroTipo.SelectedValue)

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

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
