Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_Baw
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

            If Session("insert") = True Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    sql1 = "SELECT * FROM usuarios_empresas WHERE id_usuario = @codigo"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then

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

        Catch ex As Exception

        End Try

    End Sub



End Class
