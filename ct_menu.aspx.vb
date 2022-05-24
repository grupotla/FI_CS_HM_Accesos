Imports Npgsql
Imports connection
Imports logs
Imports System.IO

Partial Class ct_menu
    Inherits System.Web.UI.Page

    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Public Licon_new As String = icon_new
    Public Licon_opciones As String = icon_opciones
    Public Licon_edit As String = icon_edit
    Public Licon_open As String = icon_open
    Public Licon_dir As String = icon_dir

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session("OperatorID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If Session("sistema") <> Nothing Then
            Session("sistema") = Nothing
            Session("DBAccesos") = Nothing
            Session("DBAccesos_conn") = Nothing
            Session("DBAccesosUserId") = Nothing
            Session("DBAccesosLogin") = Nothing
            Session("insert") = Nothing
            Session("edit") = Nothing
            Session("icono") = Nothing
            Session("login_id") = Nothing ' usuarios
        End If

        If Session("OperatorLevel") <> 1 Then

            If Session("OperatorLevel") <> Nothing Then
                MsgBox("No tiene suficientes permisos")
            End If

            Response.Redirect("Default.aspx")

        End If

        Try

            If Not IsPostBack Then

                If Session("insert") = Nothing Then
                    Dim html As String = ""
                    html = html & "<div class='accordion' id='accordion2'>"
                    html = html & menu_gen1("", "", False, Session("OperatorID"))
                    html = html & "</div>"
                    Label1.Text = html
                End If

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub


    Public Function menu_gen1(ByVal parent As String, ByVal html As String, ByVal icono As Boolean, ByVal user As Integer) As String

        Try

            Dim pointer As String = " onmouseover=this.style.cursor='pointer' "
            Dim boton As String = ""
            Dim icon As String = ""
            Dim title As String = ""
            Dim submen As String = ""
            Dim modo As String = ""
            Dim estilo As String = ""

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                sql1 = "SELECT * FROM usuarios_empresas_menu WHERE parent = '" & parent & "' ORDER BY status desc, orden" 'AND status = 't' 
                Dim cmd As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = cmd.ExecuteReader
                While dataread.Read()

                    If dataread(7) = True Then 'sub menu
                        boton = "primary"
                        icon = Licon_open
                        title = "Ver sub menu"
                        submen = "<a " & pointer & " class='btn btn-sm btn-default' title='Agregar Sub Menu a " & dataread(1) & "' onclick=abre_modal('0','" & dataread(4) & "') >" & Licon_new & "</a>"
                        modo = "data-toggle='collapse' data-parent='#accordion2' href='#collapse_" & dataread(4) & "'"
                        estilo = " accordion-toggle"
                    Else
                        boton = "info"
                        icon = "<img src='" & icon_dir & dataread(3) & "' height='16' />"
                        title = "Editar " & dataread(1)
                        submen = "" '<span style='width:" & Licon_new.ToString.Length & ";display:inline-block'>&nbsp;</span>"
                        modo = "onclick=abre_modal('" & dataread(0) & "','0')"
                        estilo = ""
                    End If

                    If dataread(8) = False Then 'desactivado
                        boton = "default"
                        title = title & " (desactivado)"
                    End If

                    html = html & "<div class='accordion-group'>"
                    html = html & "<div class='accordion-heading'>&nbsp;"
                    html = html & "     <a style='text-align:left;width:200px' class='btn btn-" & boton & estilo & "' title='" & title & "' " & modo & " >"
                    html = html & "         " & icon & "&nbsp;" & dataread(1)
                    html = html & "     </a>"
                    html = html & "     <a " & pointer & " class='btn btn-sm btn-default' title='Editar " & dataread(1) & "' onclick=abre_modal('" & dataread(0) & "','0')>" & Licon_edit & "</a>"
                    html = html & submen
                    html = html & "</div>"
                    html = html & "<div id='collapse_" & dataread(4) & "' class='accordion-body collapse'>"
                    html = html & "     <div class='accordion-inner'>"
                    'html = html & "         <div class='col-sm-offset-0 radio-inline'>"
                    If dataread(7) = True Then
                        html = html & menu_gen1(dataread(4), "", icono, user)
                    End If
                    'html = html & "         </div>"
                    html = html & "     </div>"
                    html = html & "</div>"
                    html = html & "</div>"

                End While
                dataread.Close()
            End Using

        Catch ex As Exception
            html = ex.Message
        End Try

        Return html

    End Function


    Protected Sub btn_abre_Click(sender As Object, e As System.EventArgs) Handles btn_abre.Click

        Dim code As Integer = Val(opcion.Text)

        Codigo.Text = ""
        Nombre.Text = ""
        Link.Text = ""
        Orden.Text = ""
        Menu.Text = ""
        Parent.Text = opcion_parent.Text

        Icono.Text = ""
        Childs.Checked = False
        Status.Checked = False

        LeerRegistro(code)

    End Sub





    Protected Sub LeerRegistro(ByVal opcion As Integer)

        Try

            Session("insert") = 1

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT * FROM usuarios_empresas_menu WHERE id = @opcion"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@opcion", NpgsqlTypes.NpgsqlDbType.Bigint).Value = opcion
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    Codigo.Text = dataread(0)
                    Nombre.Text = dataread(1)
                    Link.Text = dataread(2)
                    Icono.Text = dataread(3)
                    Menu.Text = dataread(4)
                    Parent.Text = dataread(5)
                    Orden.Text = dataread(6)
                    Childs.Checked = dataread(7)
                    Status.Checked = dataread(8)
                    Session("insert") = 2
                End If
            End Using

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub



    Protected Sub graba(ByVal operacion As String)

        Try
            Dim icon_ant As String = Icono.Text

            If FileUpload1.HasFile Then
                Icono.Text = "IC" & FileUpload1.FileName
                'Label1.Text = "File name: " & FileUpload1.PostedFile.FileName & "<br>" & _
                '    "File Size: " & FileUpload1.PostedFile.ContentLength & "<br>" & _
                '    "Content type: " & FileUpload1.PostedFile.ContentType & "<br>" & _
                '    "Location Saved: C:\Uploads\" & FileUpload1.FileName
            End If



            Dim mod_str As String = ""

            If Session("insert") = 1 Then
                sql1 = "INSERT INTO usuarios_empresas_menu (nombre,	link, icon, ide, parent, orden, childs, status) VALUES (@nombre, @link, @icon, @ide, @parent, @orden, @childs, @status)"
            Else
                sql1 = "UPDATE usuarios_empresas_menu SET nombre=@nombre, link=@link, icon=@icon, ide=@ide, parent=@parent, orden=@orden, childs=@childs, status=@status WHERE id = @id"
            End If

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Codigo.Text
                comm.Parameters.Add("@nombre", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                comm.Parameters.Add("@link", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Link.Text
                comm.Parameters.Add("@icon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Icono.Text
                comm.Parameters.Add("@ide", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Menu.Text
                comm.Parameters.Add("@parent", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Parent.Text
                comm.Parameters.Add("@orden", NpgsqlTypes.NpgsqlDbType.Integer).Value = Orden.Text
                comm.Parameters.Add("@childs", NpgsqlTypes.NpgsqlDbType.Boolean).Value = Childs.Checked
                comm.Parameters.Add("@status", NpgsqlTypes.NpgsqlDbType.Boolean).Value = Status.Checked
                conn.Open()
                comm.ExecuteNonQuery()

                If FileUpload1.HasFile Then

                    If icon_ant <> "" Then
                        Dim strPath As String = Server.MapPath("Content/icon/" & icon_ant)
                        System.IO.File.Delete(strPath)
                    End If

                    FileUpload1.SaveAs(Server.MapPath(Path.Combine("~/Content/icon/", Icono.Text)))

                End If

            End Using

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
            Exit Sub
        End Try

        Session("insert") = Nothing

        Response.Redirect("ct_menu.aspx")

    End Sub


    'Protected Sub btn_cancelar_Click(sender As Object, e As System.EventArgs) Handles btn_cancelar.Click
    '    Session("insert") = Nothing
    'End Sub




    'Protected Sub opcion1_btn_Click(sender As Object, e As System.EventArgs) Handles opcion1_btn.Click
    '    'MsgBox(opcion_txt.Text)
    '    Select Case opcion1_txt.Text
    '        Case "a_opciones"
    '            Session("insert") = Nothing

    '        Case "a_new"
    '            Dim code As Integer = 0
    '            Codigo.Text = ""
    '            Nombre.Text = ""
    '            Link.Text = ""
    '            Orden.Text = ""
    '            Menu.Text = ""
    '            Parent.Text = ""
    '            Icono.Text = ""
    '            Childs.Checked = False
    '            Status.Checked = False
    '            LeerRegistro(code)
    '    End Select
    'End Sub

End Class
