Imports Npgsql
Imports connection
Imports logs
Imports System.Data
Imports cMd5Hash
Imports System.IO

Partial Class ct_usuarios
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Public Licon_keys As String = icon_keys
    Public Licon_new As String = icon_new
    Public Licon_user As String = icon_user
    Public Licon_pasgen As String = icon_pasgen
    Public Licon_pascan As String = icon_pascan
    Public Licon_pasenc As String = icon_pasenc
    Public Licon_search As String = icon_search

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session("OperatorLevel") <> 1 Then

            If Session("edit") <> Nothing And Session("login_id") <> Nothing Then 'viene de master link editar datos de usuario login

            Else

                If Session("OperatorLevel") <> Nothing Then
                    MsgBox("No tiene suficientes permisos")
                End If

                Response.Redirect("Default.aspx")

            End If

        End If


        If Session("sistema") <> Nothing Or Session("DBAccesosLogin") <> Nothing Then
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



        If Not IsPostBack Then

            If Session("new") = True Then
                FillUsersGrid()
            Else

                If Session("insert") = Nothing And Session("login_id") = Nothing Then
                    LeerGrid()
                Else
                    LeerRegistro()
                End If

            End If

        End If

    End Sub



    Protected Sub LeerRegistro()

        Try

            codigo.Text = ""
            Session("insert") = True
            Activo.Checked = False

            Pais.Items.Clear()

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                'sql1 = "SELECT trim(pais) pais, nombre FROM usuarios_paises WHERE activo = 't' ORDER BY nombre"
                sql1 = "SELECT trim(pais_iso) as pais, nombre_empresa as nombre FROM empresas WHERE activo = 't' ORDER BY nombre_empresa"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                Pais.DataSource = ds
                Pais.DataTextField = "nombre"
                Pais.DataValueField = "pais"
                Pais.DataBind()
                'End Using

                Modulos.Items.Clear()
                CheckBoxList1.Items.Clear()

                'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT ide, '<img src=" & icon_dir & "' || icon || ' height=12 />' || ' ' ||   parent || ' ' || nombre as opcion, parent FROM usuarios_empresas_menu WHERE childs = 'f' AND status = 't' ORDER BY orden"
                Dim ds1 As New DataSet()
                Dim cmd1 As New NpgsqlCommand(sql1, conn)
                Dim adp1 As New NpgsqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Modulos.DataSource = ds1
                Modulos.DataTextField = "opcion"
                Modulos.DataValueField = "ide"
                Modulos.DataBind()

                CheckBoxList1.DataSource = ds1
                CheckBoxList1.DataTextField = "parent"
                CheckBoxList1.DataValueField = "ide"
                CheckBoxList1.DataBind()
                'End Using

                'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, level, pw_activo, pw_passwd, modulos, demo, create_id_usuario FROM usuarios_empresas_login WHERE create_id_usuario = @create_id_usuario"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@create_id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("login_id")
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    codigo.Text = dataread(0)
                    Login.Text = dataread(1)
                    Nombre.Text = dataread(2)
                    Pais.SelectedValue = dataread(3)
                    Nivel.SelectedValue = dataread(4)
                    Activo.Checked = dataread(5)
                    Password.Text = dataread(6)
                    Demo.Checked = dataread(8)
                    aimar_code.Text = dataread(9)

                    Dim mod_str As String = ""
                    If dataread(7).ToString.Length > 0 Then
                        mod_str = dataread(7)
                    End If
                    Dim strArr() As String
                    Dim count As Integer

                    For Each li As ListItem In Modulos.Items
                        strArr = mod_str.Split(",") 'array en base de datos
                        For count = 0 To strArr.Length - 1
                            If li.Value = strArr(count) Then
                                li.Selected = True
                            End If
                        Next
                    Next
                    Session("insert") = False
                End If


                If Session("insert") = True Then

                    sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio, tipo_usuario, level, pw_activo, pw_correo, pw_passwd, locode FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                    Dim comm1 As New NpgsqlCommand(sql1, conn)
                    comm1.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("login_id")
                    'conn.Open()
                    Dim dataread1 As NpgsqlDataReader = comm1.ExecuteReader
                    If dataread1.Read() Then
                        'codigo.Text = dataread1(0)
                        aimar_code.Text = dataread1(0)
                        Login.Text = dataread1(1)
                        Nombre.Text = dataread1(2)
                        'Pais.Text = dataread1(3)
                        For Each li As ListItem In Pais.Items
                            If Trim(li.Value) = dataread1(3) Then
                                li.Selected = True
                            End If
                        Next
                        'Dominio.Text = dataread1(4)
                        'tipo_usuario.SelectedValue = dataread1(5)
                        Nivel.SelectedValue = dataread1(6)
                        'Activo.Checked = dataread1(7)
                        'Correo.SelectedValue = dataread1(8)
                        Password.Text = Trim(dataread1(9))
                        'Email.Text = dataread1(1) & "@" & dataread1(4)

                    End If

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

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar(False)
    End Sub

    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        activar(True)
    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub graba(ByVal operacion As String)


        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio, tipo_usuario, level, pw_activo, pw_correo, pw_passwd, locode FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                Dim comm1 As New NpgsqlCommand(sql1, conn)
                comm1.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = aimar_code.Text
                conn.Open()
                Dim dataread1 As NpgsqlDataReader = comm1.ExecuteReader
                If Not dataread1.Read() Then
                    msg = "Codigo Master no existente en Aimar"
                    Exit Sub
                End If
            End Using

            Dim mod_str As String = "" '

            If Session("insert") = True Then
                sql1 = "INSERT INTO usuarios_empresas_login (pw_name, pw_gecos, pais, pw_passwd, level, pw_activo, demo, create_id_usuario) VALUES (@pw_name, @pw_gecos, @pais, @pw_passwd, @level, true, @demo, @create_id_usuario)"
            Else
                sql1 = "UPDATE usuarios_empresas_login SET pw_name=@pw_name, pw_gecos=@pw_gecos, pw_passwd=@pw_passwd, pais=@pais, level=@level, modulos=@modulos, demo=@demo WHERE create_id_usuario = @create_id_usuario"

                Dim i As Integer = 0
                For Each li As ListItem In Modulos.Items
                    If li.Selected = True Then
                        mod_str = mod_str & li.Value & ","
                        If CheckBoxList1.Items(Modulos.SelectedIndex).Text <> "" Then
                            mod_str = mod_str & CheckBoxList1.Items(i).Text & ","
                        End If
                    End If
                    i = i + 1
                Next
            End If


            If Password.Text.Length = 10 Then
                Dim HashCode As cMd5Hash
                HashCode = New cMd5Hash()
                Password.Text = HashCode.Md5FromString(Password.Text)
            End If


            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = codigo.Text
                comm.Parameters.Add("@pw_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                comm.Parameters.Add("@pw_gecos", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                comm.Parameters.Add("@pw_passwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Password.Text
                comm.Parameters.Add("@pais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Pais.SelectedValue
                comm.Parameters.Add("@level", NpgsqlTypes.NpgsqlDbType.Integer).Value = Nivel.SelectedValue
                comm.Parameters.Add("@modulos", NpgsqlTypes.NpgsqlDbType.Varchar).Value = mod_str
                comm.Parameters.Add("@demo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = Demo.Checked
                comm.Parameters.Add("@create_id_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = aimar_code.Text
                conn.Open()
                comm.ExecuteNonQuery()

                If Session("insert") = True Then
                    sql1 = "SELECT LASTVAL()"
                    comm.CommandText = sql1
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        codigo.Text = dataread(0)
                        'Session("login_id") = dataread(0)
                    End If
                    dataread.Close()
                End If

            End Using

            Dim items As New Dictionary(Of String, String)
            items.Add("id_usuario", codigo.Text)
            items.Add("pw_name", Login.Text)
            items.Add("pw_gecos", Nombre.Text)
            items.Add("pw_passwd", Password.Text)
            items.Add("pais", Pais.SelectedValue)
            items.Add("level", Nivel.SelectedValue)
            items.Add("modulos", mod_str)
            items.Add("create_id_usuario", aimar_code.Text)

            log(Server, codigo.Text, operacion, "", items, "usuarios_empresas_login", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
            Exit Sub
        End Try

        If Session("insert") = True Then
            Session("insert") = False
            Activo.Checked = True
            msg = "Registro Creado correctamente"
            LeerRegistro()
        Else
            msg = "Registro Actualizado correctamente"


            Dim continua As Boolean = True
            If operacion = "update" Then
                If Session("insert") = Nothing And Session("login_id") = Nothing Then
                Else
                    If Session("OperatorLevel") <> 1 Then
                        continua = False
                    End If
                End If
            End If

            If continua = True Then
                LeerGrid()
            Else

            End If



        End If

    End Sub

    Protected Sub activar(ByVal operacion As Boolean)

        Try
            sql1 = "UPDATE usuarios_empresas_login SET pw_activo=@activo WHERE create_id_usuario = @codigo" ', modulos=@modulos
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = aimar_code.Text
                comm.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = operacion
                'comm.Parameters.Add("@modulos", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
                conn.Open()
                comm.ExecuteNonQuery()
            End Using

            Dim items As New Dictionary(Of String, String)
            items.Add("id_usuario", codigo.Text)
            items.Add("activo", operacion)

            log(Server, codigo.Text, operacion, "", items, "usuarios_empresas_login", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
            Exit Sub
        End Try

        If operacion = True Then
            msg = "Registro se activo correctamente"
            LeerRegistro()
        Else
            msg = "Registro se desactivo correctamente"

            If Session("OperatorLevel") = 1 Then
                LeerGrid()
            Else
                LeerRegistro()
            End If
        End If

    End Sub

    'Protected Sub btn_list_Click(sender As Object, e As System.EventArgs) Handles btn_list.Click
    '    LeerGrid()
    'End Sub

    Protected Sub LeerGrid()
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT create_id_usuario as id, pw_name as login, pw_gecos as nombre, pais, level as nivel, pw_activo as activo FROM usuarios_empresas_login ORDER BY create_id_usuario"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                login_grid.DataSource = dataread
                login_grid.DataBind()
                Session("insert") = Nothing
                Session("login_id") = Nothing
                If login_grid.Rows.Count = 0 Then
                    Session("insert") = True
                    Session("login_id") = 0
                End If
            End Using
        Catch ex As Exception
            msg = ex.Message
        End Try
    End Sub


    Protected Sub login_grid_SelectedIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles login_grid.SelectedIndexChanging
        Session("login_id") = login_grid.Rows(e.NewSelectedIndex).Cells(1).Text
        Session("insert") = False
        e.Cancel = True
        LeerRegistro()
    End Sub


    Protected Sub lnk_check_Click(sender As Object, e As System.EventArgs) Handles lnk_check.Click

        Dim checked As Boolean = False
        Dim unchecked As Boolean = False

        If lnk_check.Text = "Modulos" Then
            checked = True
            lnk_check.Text = "Modulos " & icon_check
        Else

            If lnk_check.Text = "Modulos " & icon_check Then
                checked = False
                lnk_check.Text = "Modulos " & icon_uncheck
            Else
                checked = True
                lnk_check.Text = "Modulos " & icon_check
            End If
        End If

        For Each li As ListItem In Modulos.Items
            li.Selected = checked
        Next
    End Sub



    Protected Sub imageButtonClick(sender As Object, e As System.EventArgs)

        'Dim imageButton As ImageButton = sender
        Dim imageButton As LinkButton = sender
        Dim tableCell As TableCell = imageButton.Parent
        Dim row As GridViewRow = tableCell.Parent
        login_grid.SelectedIndex = row.RowIndex

        Session("login_id") = row.Cells(1).Text
        Session("insert") = False
        LeerRegistro()

    End Sub


    Protected Sub login_grid_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles login_grid.RowDataBound

        Dim row As GridViewRow = e.Row

        If row.RowType = DataControlRowType.DataRow Then

            Dim link As LinkButton = row.FindControl("lnk_user")
            link.ToolTip = "Editar Usuario " & row.Cells(3).Text

            Dim flag As Image = row.FindControl("lnk_flag")
            flag.ImageUrl = "~/Content/flags/" & Trim(row.Cells(4).Text).ToLower & "-flag.gif"
            flag.ToolTip = row.Cells(4).Text

        End If

    End Sub

    Protected Sub gen_pass_Click(sender As Object, e As System.EventArgs) Handles gen_pass.Click
        Password.Text = PwdAleatorio(10, False)
    End Sub

    'Protected Sub enc_pass_Click(sender As Object, e As System.EventArgs) Handles enc_pass.Click
    '    Password.Text = Trim(Password.Text)
    '    If Password.Text <> "" Then
    '        Dim HashCode As cMd5Hash
    '        HashCode = New cMd5Hash()
    '        Password.Text = HashCode.Md5FromString(Password.Text)
    '    End If
    'End Sub

    Protected Sub can_pass_Click(sender As Object, e As System.EventArgs) Handles can_pass.Click
        LeerRegistro()
    End Sub












    '////////////////////////////////////////////////////////////////////////// M A S T E R //////////////////////////////////////////////////////////



    Protected Sub imageButtonClick_master(sender As Object, e As System.EventArgs)
        'Dim imageButton As ImageButton = sender
        Dim imageButton As LinkButton = sender
        Dim tableCell As TableCell = imageButton.Parent
        Dim row As GridViewRow = tableCell.Parent
        users_grid.SelectedIndex = row.RowIndex
        Session("login_id") = row.Cells(1).Text
        Session("insert") = True
        Session("new") = Nothing
        Response.Redirect("ct_usuarios.aspx")
    End Sub

    Protected Sub users_grid_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles users_grid.RowDataBound
        Dim row As GridViewRow = e.Row
        If row.RowType = DataControlRowType.DataRow Then
            Dim link As LinkButton = row.FindControl("lnk_user1")
            link.ToolTip = "Seleccionar Usuario " & row.Cells(4).Text

            Dim flag As Image = row.FindControl("lnk_flag1")

            Dim archivo As String = "~/Content/flags/" & Trim(row.Cells(5).Text).ToLower & "-flag.gif"

            Dim myFilePath As String = Server.MapPath(archivo)
            If File.Exists(myFilePath) Then
                'do my work here'
                'flag.ImageUrl = "~/Content/flags/" & Trim(row.Cells(5).Text).ToLower & "-flag.gif"
                flag.ImageUrl = archivo
            End If

            flag.ToolTip = row.Cells(5).Text
        End If
    End Sub

    'Protected Sub btn_abre_Click(sender As Object, e As System.EventArgs) Handles btn_abre.Click
    '    Session("new") = True
    '    FillUsersGrid()
    'End Sub

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

                'If inactivo.Checked = True Then
                '    sql1 = sql1 & " AND pw_activo = 0 "
                'Else
                sql1 = sql1 & " AND pw_activo = 1 "
                'End If

                'If pais.Checked = True Then
                '    sql1 = sql1 & " AND pais = '" & Session("OperatorCountry") & "' "
                'End If

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

    Protected Sub MakeAccessible(ByVal grid As GridView)
        If grid.Rows.Count > 0 Then
            'This replaces <td> with <th> and adds the scope attribute
            grid.UseAccessibleHeader = True
            'This will add the <thead> and <tbody> elements
            grid.HeaderRow.TableSection = TableRowSection.TableHeader
            'This adds the <tfoot> element. Remove if you don't have a footer row
            'grid.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub


    Protected Sub buscar_nombre_TextChanged(sender As Object, e As System.EventArgs) Handles buscar_nombre.TextChanged
        FillUsersGrid()
    End Sub

    Protected Sub btn_buscar_Click(sender As Object, e As System.EventArgs) Handles btn_buscar.Click
        FillUsersGrid()
    End Sub



    'Protected Sub opcion1_btn_Click(sender As Object, e As System.EventArgs) Handles opcion1_btn.Click


    '    'Select Case opcion1_txt.Text
    '    '    Case "a_keys"
    '    '        Session("new") = Nothing
    '    '        LeerGrid()

    '    '    Case "a_home"

    '    '    Case "a_login"

    '    '    Case "a_new"
    '    '        Session("new") = True
    '    '        FillUsersGrid()
    '    'End Select



    'End Sub

End Class
