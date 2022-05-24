Imports Npgsql
Imports System.Data
Imports connection
Imports logs
Imports System.IO

Partial Class _Default

    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_new As String = icon_new
    Public Licon_search As String = icon_search
    Public Licon_user As String = icon_user
    Public Licon_home As String = icon_home

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session("OperatorID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            If Session("DBAccesos_buscar") <> Nothing Then
                buscar_nombre.Text = Session("DBAccesos_buscar")
            End If
            If Session("DBAccesosUserId") = Nothing Then
                FillUsersGrid()
            Else
                inicio()
            End If
        End If
    End Sub

    Protected Sub MakeAccessible(ByVal grid As GridView)
        If grid.Rows.Count > 0 Then
            grid.UseAccessibleHeader = True 'This replaces <td> with <th> and adds the scope attribute
            grid.HeaderRow.TableSection = TableRowSection.TableHeader 'This will add the <thead> and <tbody> elements            
            'grid.FooterRow.TableSection = TableRowSection.TableFooter 'This adds the <tfoot> element. Remove if you don't have a footer row
        End If
    End Sub

    Protected Sub FillUsersGrid()
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario as ID, pw_name as Login, dominio, pw_gecos as Nombre, pais as Pais FROM usuarios_empresas WHERE id_usuario > 0 "
                If buscar_nombre.Text <> "" Then
                    Session("DBAccesos_buscar") = buscar_nombre.Text
                    sql1 = sql1 & " AND (pw_name ILIKE '%" & buscar_nombre.Text & "%' OR pw_gecos ILIKE '%" & buscar_nombre.Text & "%') "
                Else
                    Session("DBAccesos_buscar") = Nothing
                End If
                If inactivo.Checked = True Then
                    sql1 = sql1 & " AND pw_activo = 0 "
                Else
                    sql1 = sql1 & " AND pw_activo = 1 "
                End If
                If pais.Checked = True Then
                    sql1 = sql1 & " AND pais = '" & Session("OperatorCountry") & "' "
                End If
                sql1 = sql1 & " ORDER BY id_usuario"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                users_grid.DataSource = dataread
                users_grid.DataBind()
            End Using
            MakeAccessible(users_grid)
        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try
    End Sub

    Protected Sub buscar_nombre_TextChanged(sender As Object, e As System.EventArgs) Handles buscar_nombre.TextChanged
        FillUsersGrid()
    End Sub

    Protected Sub btn_buscar_Click(sender As Object, e As System.EventArgs) Handles btn_buscar.Click
        FillUsersGrid()
    End Sub

    Protected Sub btn_pais_Click(sender As Object, e As System.EventArgs) Handles btn_pais.Click
        FillUsersGrid()
    End Sub

    Protected Sub btn_inactivo_Click(sender As Object, e As System.EventArgs) Handles btn_inactivo.Click
        FillUsersGrid()
    End Sub

    'Protected Sub btn_nuevo_Click(sender As Object, e As System.EventArgs) Handles btn_nuevo.Click
    '    Session("edit") = "edit"
    '    inicio()
    'End Sub

    Protected Sub inicio()
        Dim temp As String = Session("edit")
        Dim params() As String
        params = selecciona_sistema("usuario", Server).ToString.Split(",")
        If params(0) <> "" Then
            Session("DBAccesos") = params(1)
            Session("DBAccesos_conn") = params(2)
            Session("pestana") = params(3)
            Session("sistema") = params(4)
            If params(5) = "Nothing" Then
                Session("insert") = Nothing
            Else
                Session("insert") = params(5)
            End If
            If params(6) = "Nothing" Then
                Session("edit") = Nothing
            Else
                Session("edit") = params(6)
            End If
            Session("icono") = params(7)
            Session("edit") = temp
            Response.Redirect(params(0))
        Else
            msg = "No encontro sistema '" & "usuario" & "'"
        End If

    End Sub

    'Protected Sub users_grid_SelectedIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles users_grid.SelectedIndexChanging
    '    Dim b As Integer = e.NewSelectedIndex
    '    e.Cancel = True
    '    Session("DBAccesosUserId") = users_grid.Rows(b).Cells(1).Text
    '    Session("DBAccesosLogin") = users_grid.Rows(b).Cells(2).Text
    '    inicio()
    'End Sub

    Protected Sub imageButtonClick(sender As Object, e As System.EventArgs)
        'Dim imageButton As ImageButton = sender
        Dim imageButton As LinkButton = sender
        Dim tableCell As TableCell = imageButton.Parent
        Dim row As GridViewRow = tableCell.Parent
        users_grid.SelectedIndex = row.RowIndex
        Session("DBAccesosUserId") = row.Cells(1).Text
        Session("DBAccesosLogin") = row.Cells(2).Text
        inicio()
    End Sub

    Protected Sub users_grid_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles users_grid.RowDataBound
        Dim row As GridViewRow = e.Row
        If row.RowType = DataControlRowType.DataRow Then
            Dim link As LinkButton = row.FindControl("lnk_user")
            link.ToolTip = "Editar Usuario " & row.Cells(4).Text

            Dim flag As Image = row.FindControl("lnk_flag")
            flag.ToolTip = row.Cells(5).Text

            Dim archivo As String = "~/Content/flags/" & Trim(row.Cells(5).Text).ToLower & "-flag.gif"
            Dim myFilePath As String = Server.MapPath(archivo)
            If File.Exists(myFilePath) Then
                flag.ImageUrl = archivo
            End If

            'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

            '    Dim mod_str As String = ""
            '    conn.Open()
            '    sql1 = "SELECT modulos FROM usuarios_empresas_login WHERE create_id_usuario = @create_id_usuario"
            '    Dim comm1 As New NpgsqlCommand(sql1, conn)
            '    comm1.Parameters.Add("@create_id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("OperatorID")
            '    Dim dataread1 As NpgsqlDataReader = comm1.ExecuteReader
            '    If dataread1.Read Then
            '        If dataread1(0).ToString.Length > 0 Then
            '            mod_str = dataread1(0)
            '        End If
            '    End If

            '    dataread1.Close()
            '    Dim strArr() As String = mod_str.Split(",") 'array en base de datos
            '    Dim count As Integer

            '    Dim Modulos As DropDownList = row.FindControl("lnk_modulos")
            '    Modulos.Items.Add(New ListItem("Modulos", ""))

            '    Dim display As String
            '    sql1 = "SELECT ide, parent FROM usuarios_empresas_menu WHERE childs = 'f' AND status = 't' ORDER BY orden"
            '    Dim comm As New NpgsqlCommand(sql1, conn)
            '    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
            '    While dataread.Read()
            '        For count = 0 To strArr.Length - 1
            '            If dataread(0) = strArr(count) Then
            '                display = dataread(1)
            '                If display <> "" Then
            '                    display = display & " - "
            '                End If
            '                display = display & dataread(0)
            '                Modulos.Items.Add(New ListItem(display, row.Cells(1).Text & "|" & row.Cells(2).Text & "|" & strArr(count)))
            '                Exit For
            '            End If
            '        Next
            '    End While
            '    dataread.Close()
            'End Using


        End If

    End Sub

End Class
