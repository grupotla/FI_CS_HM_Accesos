Imports Npgsql
Imports System.Data
Imports connection

Partial Class Solicitudes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        If Session("OperatorID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Session("OperatorLevel") <> 1 Then
            MsgBox("No tiene suficientes permisos")
            Response.Redirect("Default.aspx")
        End If


        Try

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                sql1 = "SELECT sol_no as Solicitud, sol_fec as Fecha, sol_usr as Propietario FROM usuarios_empresas_menu_solicitud WHERE sol_sta = '3' ORDER BY sol_fec DESC"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader

                msg.Text = ""

                If dataread.HasRows Then
                    solicitudes_grid.DataSource = dataread
                    solicitudes_grid.DataBind()
                Else
                    msg.Text = "No hay solicitudes realizadas"
                End If

            End Using

        Catch ex As Exception
            'msg = ex.Message
        End Try

    End Sub


    Protected Sub imageButtonClick(sender As Object, e As System.EventArgs)

        Dim imageButton As LinkButton = sender
        Dim tableCell As TableCell = imageButton.Parent
        Dim row As GridViewRow = tableCell.Parent
        solicitudes_grid.SelectedIndex = row.RowIndex
        Session("solicitud") = row.Cells(1).Text
        Response.Redirect("Solicitud.aspx")
    End Sub

End Class
