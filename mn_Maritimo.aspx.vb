Imports Npgsql
Imports System.Data
Imports connection
Imports logs


Partial Class mn_Maritimo
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    'Public input_data As Boolean
    'Public insert_data As Boolean
    'Public update_data As Boolean

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Public Licon_open As String = icon_open
    Public Licon_new As String = icon_new

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'RadioButtonList1.Attributes.Add("onclick", "return rdOptionClick('" & RadioButtonList1.ClientID & "')")
        'RadioButtonList2.Attributes.Add("onclick", "return rdOptionClick('" & RadioButtonList2.ClientID & "')")

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

            inicia()

        End If



    End Sub








    Protected Sub FillPaisesDB()
        Try

            Dim htm As String = "<ul id='pestana' class='nav nav-tabs'>"
            htm = htm & menu_gen(Session("sistema"), "", False, Session("OperatorID"), Server)
            htm = htm & "</ul>"

            pestana_lbl.Text = htm

            RadioButtonList1.Items.Clear()
            RadioButtonList2.Items.Clear()

            Dim dbs As String = ""
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT DISTINCT bd FROM referencias_usuarios WHERE id_nuevo='" & Session("DBAccesosUserId") & "' AND activo = 't' ORDER BY bd"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                While dataread.Read()
                    If dbs <> "" Then
                        dbs = dbs & ","
                    End If
                    dbs = dbs & Comillas & dataread(0) & Comillas
                End While
                dataread.Close()
            End Using


            'Dim selec As String = "SELECT database, " & _
            '" '<img src=Content/flags/' || lower(pais) || '-flag.gif height=16 title=' || pais || ' /> ' || pais || ' ' || nombre as flag " & _
            '" FROM usuarios_empresas_db WHERE usado_en ILIKE '%maritimo%' AND status = 't' "

            Dim selec As String = "SELECT datname as database, " & _
            " '<img src=Content/flags/' || lower(substring(datname from 8 for 2)) || '-flag.gif height=16 title=' || upper(substring(datname from 8 for 2)) || ' /> ' || upper(datname) || ' ' as flag " & _
            " FROM pg_database WHERE datistemplate = false AND datname ilike 'ventas%'"

            'If Session("demo") = False Then
            'selec = selec & " AND demo = 'f' "
            'End If

            If dbs <> "" Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    'sql1 = selec & " AND database IN (" & dbs & ") ORDER BY database"
                    sql1 = selec & " AND datname IN (" & dbs & ") ORDER BY datname;"
                    Dim ds As New DataSet()
                    Dim cmd As New NpgsqlCommand(sql1, conn)
                    Dim adp As New NpgsqlDataAdapter(cmd)
                    adp.Fill(ds)
                    RadioButtonList1.DataSource = ds
                    RadioButtonList1.DataValueField = "database"
                    RadioButtonList1.DataTextField = "flag"
                    RadioButtonList1.DataBind()
                End Using

                If RadioButtonList1.Items.Count > 0 Then

                    RadioButtonList1.SelectedIndex = 0

                    If Session("DBAccesos") = Nothing Then
                        'TextBox7.Text = RadioButtonList1.Items(0).Value
                        'Dim label As String = RadioButtonList1.Items(0).Text
                        'label = label.Replace("<", "[")
                        'label = label.Replace(">", "]")
                        'TextBox8.Text = label                        
                        sel_db(RadioButtonList1.SelectedValue, RadioButtonList1.SelectedItem.Text)
                    End If

                    For Each li As ListItem In RadioButtonList1.Items
                        If li.Value = Session("DBAccesos") Then
                            li.Selected = True
                        End If
                    Next

                End If

                'sql1 = selec & " AND database NOT IN (" & dbs & ") ORDER BY database"

                sql1 = selec & " AND datname NOT IN (" & dbs & ") ORDER BY datname;"

            Else

                'sql1 = selec & " ORDER BY database"

                sql1 = selec & " ORDER BY datname;"

                'msg = "Usuario " & Session("DBAccesosUserId") & " No esta registrado en ninguna base de datos"

            End If

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                RadioButtonList2.DataSource = ds
                RadioButtonList2.DataValueField = "database"
                RadioButtonList2.DataTextField = "flag"
                RadioButtonList2.DataBind()
            End Using


            If RadioButtonList2.Items.Count > 0 Then

                If Session("DBAccesos") = Nothing Then
                    RadioButtonList2.SelectedIndex = 0
                    'TextBox7.Text = RadioButtonList2.Items(0).Value
                    'Dim label As String = RadioButtonList2.Items(0).Text
                    'label = label.Replace("<", "[")
                    'label = label.Replace(">", "]")
                    'TextBox8.Text = label
                    sel_db(RadioButtonList2.SelectedValue, RadioButtonList2.SelectedItem.Text)
                End If

                For Each li As ListItem In RadioButtonList2.Items
                    If li.Value = Session("DBAccesos") Then
                        li.Selected = True
                    End If
                Next

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub

    Protected Sub FillPerfiles()

        Try
            Activo.Checked = False
            Perfil.Items.Clear()

            Session("insert") = True

            If Session("DBAccesos") <> Nothing Then

                Using conn_mas As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))


                    Session("DBAccesos_conn") = GetConnectionStringFromFile("ventas", Server) & Mid(Session("DBAccesos"), 8)

                    Using conn_ven As New NpgsqlConnection(Session("DBAccesos_conn"))

                        sql1 = "SELECT group_id, name FROM groups WHERE activo = 't' ORDER BY name"
                        Dim ds As New DataSet()
                        Dim comm_ven As New NpgsqlCommand(sql1, conn_ven)
                        Dim adp As New NpgsqlDataAdapter(comm_ven)
                        adp.Fill(ds)
                        Perfil.DataSource = ds
                        Perfil.DataTextField = "name"
                        Perfil.DataValueField = "group_id"
                        Perfil.DataBind()

                        conn_mas.Open()

                        '////////LEER LAS REFERENCIAS 
                        sql1 = "SELECT id_nuevo, id_anterior, bd, activo FROM referencias_usuarios WHERE id_nuevo = @id_nuevo AND bd = @bd AND activo = 't' ORDER BY bd"
                        Dim comm_ref As New NpgsqlCommand(sql1, conn_mas)
                        comm_ref.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        comm_ref.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
                        Dim dataread_ref As NpgsqlDataReader = comm_ref.ExecuteReader
                        If dataread_ref.HasRows Then

                            conn_ven.Open()
                            While dataread_ref.Read()
                                sql1 = "SELECT user_id, grupo, activo, login_name, user_name, password, email FROM users WHERE user_id = @user_id ORDER BY grupo"
                                comm_ven = New NpgsqlCommand(sql1, conn_ven)
                                comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = dataread_ref(1)
                                Dim dataread_ven As NpgsqlDataReader = comm_ven.ExecuteReader
                                If dataread_ven.Read() Then
                                    Login.Text = dataread_ven(3)
                                    Nombre.Text = dataread_ven(4)
                                    Email.Text = dataread_ven(6)
                                    Password.Text = dataread_ven(5)
                                    For Each li As ListItem In Perfil.Items
                                        If li.Value = dataread_ven(1) Then
                                            Session("insert") = False
                                            Activo.Checked = True
                                            li.Selected = True
                                            Exit For
                                        End If
                                    Next
                                End If
                                dataread_ven.Close()

                            End While

                            conn_ven.Close()

                        End If
                        dataread_ref.Close()



                    End Using


                    If Session("insert") = True Then

                        sql1 = "SELECT * FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                        Dim comm_mas As New NpgsqlCommand(sql1, conn_mas)

                        comm_mas.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        Dim dataread As NpgsqlDataReader = comm_mas.ExecuteReader
                        If dataread.Read() Then
                            Login.Text = dataread(1)
                            Nombre.Text = dataread(5)
                            Email.Text = dataread(1) & "@" & dataread(10)
                            Password.Text = dataread(2)
                            Dominio.Text = dataread(10)
                        End If
                        dataread.Close()

                    End If

                    conn_mas.Close()

                End Using

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub

    'Protected Function ides_perfil() As String
    '    Dim ides As String = ""
    '    Try
    '        'obtiene ides de perfiles en referencias del master
    '        Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
    '            sql1 = "SELECT id_anterior, activo FROM referencias_usuarios WHERE id_nuevo=" & Session("DBAccesosUserId") & " AND bd='" & Session("DBAccesos") & "' AND activo = 't' ORDER BY id_anterior"
    '            Dim comm As New NpgsqlCommand(sql1, conn)
    '            conn.Open()
    '            Dim dataread As NpgsqlDataReader = comm.ExecuteReader
    '            While dataread.Read()

    '                If ides <> "" Then
    '                    ides = ides & ","
    '                End If
    '                ides = ides & dataread(0)

    '            End While
    '        End Using
    '    Catch ex As Exception
    '        msg = ex.Message
    '        img = icon_err_read
    '        css = "alert-warning"
    '    End Try

    '    Return ides
    'End Function



    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub


    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        graba("update")
    End Sub

    'Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
    '    Dim conn_ven As New NpgsqlConnection(Session("DBAccesos_conn"))
    '    Dim conn_mas As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
    '    conn_mas.Open()
    '    conn_ven.Open()
    '    Dim finaliza As Boolean = True
    '    Dim trns_ven As NpgsqlTransaction
    '    Dim trns_mas As NpgsqlTransaction
    '    trns_ven = conn_ven.BeginTransaction()
    '    trns_mas = conn_mas.BeginTransaction()
    '    Dim items As New Dictionary(Of String, String)
    '    Try
    '        Dim ides As String = ides_perfil()
    '        If ides <> "" Then
    '            'INABILITA USERS
    '            sql1 = "UPDATE users SET activo=@activo WHERE user_id in (" & ides & ") and login_name = '" & Session("DBAccesosLogin") & "'"
    '            Dim comm_ven As New NpgsqlCommand(sql1, conn_ven)
    '            comm_ven.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = False
    '            comm_ven.ExecuteNonQuery()
    '            'LOG      
    '            items.Clear()
    '            items.Add("user_id", ides)
    '            items.Add("activo", False)
    '            log(Server,Session("DBAccesosUserId"), "desactivar", sql1, items, "users", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))
    '            'INABILITA(REFERENCIAS)
    '            sql1 = "UPDATE referencias_usuarios SET activo=@activo WHERE id_anterior in (" & ides & ")"
    '            Dim comm_mas1 As New NpgsqlCommand(sql1, conn_mas)
    '            comm_mas1.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = False
    '            comm_mas1.ExecuteNonQuery()
    '            'LOG
    '            items.Clear()
    '            items.Add("id_nuevo", Session("DBAccesosUserId"))
    '            items.Add("id_anterior", ides)
    '            items.Add("activo", False)
    '            log(Server,Session("DBAccesosUserId"), "desactivar", sql1, items, "referencias_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))
    '            msg = "Registro se desactivo correctamente"
    '        End If
    '    Catch ex As Exception
    '        msg = ex.Message
    '        img = icon_err_active
    '        css = "alert-default"
    '        finaliza = False
    '    End Try
    '    If finaliza = True Then
    '        trns_mas.Commit()
    '        trns_ven.Commit()
    '    Else
    '        trns_mas.Rollback()
    '        trns_ven.Rollback()
    '    End If
    '    conn_mas.Close()
    '    conn_ven.Close()
    '    inicia()
    'End Sub


    Private Structure PerfilDat
        Public user_id As Integer
        Public stat As String
        Public group As Integer
    End Structure


    Protected Sub graba(ByVal operacion As String)

        Try

            Dim conn_mas As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
            conn_mas.Open()

            Dim conn_ven As New NpgsqlConnection(Session("DBAccesos_conn"))
            conn_ven.Open()

            'Dim opero As Boolean = False
            Dim finaliza As Boolean = False
            Dim trns_ven As NpgsqlTransaction
            Dim trns_mas As NpgsqlTransaction

            trns_ven = conn_ven.BeginTransaction()
            trns_mas = conn_mas.BeginTransaction()



            sql1 = "UPDATE referencias_usuarios SET activo='f' WHERE id_nuevo=@id_nuevo AND bd=@bd"
            Dim comm As New NpgsqlCommand(sql1, conn_mas)
            comm.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")
            comm.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
            comm.ExecuteNonQuery()


            Dim ArrPerfil As New List(Of PerfilDat)
            ArrPerfil.Clear()
            Dim tmp As New PerfilDat

            '////////LEER LAS REFERENCIAS 
            sql1 = "SELECT id_nuevo, id_anterior, bd, activo FROM referencias_usuarios WHERE id_nuevo = @id_nuevo AND bd = @bd ORDER BY id_anterior"
            Dim comm_ref As New NpgsqlCommand(sql1, conn_mas)
            comm_ref.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
            comm_ref.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
            Dim dataread_ref As NpgsqlDataReader = comm_ref.ExecuteReader
            If dataread_ref.HasRows Then
                While dataread_ref.Read()

                    Dim comm_ven As NpgsqlCommand
                    sql1 = "UPDATE users SET activo='f' WHERE user_id=@user_id"
                    comm_ven = New NpgsqlCommand(sql1, conn_ven)
                    comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = dataread_ref(1)
                    comm_ven.ExecuteNonQuery()

                    '////////LEER VENTAS
                    'sql1 = "SELECT user_id, grupo FROM users WHERE user_id = @user_id AND activo = 't' ORDER BY grupo"
                    sql1 = "SELECT user_id, grupo FROM users WHERE user_id = @user_id ORDER BY grupo"
                    comm_ven = New NpgsqlCommand(sql1, conn_ven)
                    comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = dataread_ref(1)
                    Dim dataread_ven As NpgsqlDataReader = comm_ven.ExecuteReader
                    If dataread_ven.Read() Then
                        tmp.group = dataread_ven(1)
                        tmp.stat = "A"
                        tmp.user_id = dataread_ven(0)
                        ArrPerfil.Add(tmp)
                    Else
                    End If
                    dataread_ven.Close()

                End While
            End If
            dataread_ref.Close()






            Dim ArrPerfil2 As New List(Of PerfilDat)
            ArrPerfil2.Clear()

            Dim founded As Boolean = False

            'agrega los perfiles nuevos
            For Each perfil_new As ListItem In Perfil.Items

                If perfil_new.Selected = True Then

                    founded = False
                    For Each Perfil_ant As PerfilDat In ArrPerfil
                        If Perfil_ant.group = perfil_new.Value Then
                            founded = True
                            tmp.group = Perfil_ant.group
                            tmp.stat = "U"
                            tmp.user_id = Perfil_ant.user_id
                            ArrPerfil2.Add(tmp)
                            Exit For
                        End If
                    Next

                    If founded = False Then
                        tmp.group = perfil_new.Value
                        tmp.stat = "N"
                        tmp.user_id = 0
                        ArrPerfil2.Add(tmp)
                    End If

                End If

            Next


            Try

                Dim stat_str As String = ""
                Dim user_id_str As String = ""
                Dim group_str As String = ""

                For Each Perfil As PerfilDat In ArrPerfil2

                    If Perfil.stat = "N" Then

                        'ventas
                        sql1 = "INSERT INTO users (login_name, user_name, password, grupo, activo, email) VALUES (@login_name, @user_name, @password, @grupo, 't', @email) returning user_id"
                        Dim comm_ven As New NpgsqlCommand(sql1, conn_ven)
                        comm_ven.Parameters.Add("@login_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                        comm_ven.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                        comm_ven.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
                        comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.group
                        comm_ven.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Email.Text 'pw_name_ & "@" & Dominio.Text
                        Dim dataread_ven As NpgsqlDataReader = comm_ven.ExecuteReader
                        If dataread_ven.Read() Then
                            Perfil.user_id = dataread_ven(0)
                        End If
                        dataread_ven.Close()

                        'referencias
                        sql1 = "INSERT INTO referencias_usuarios (id_nuevo, id_anterior, bd, dominio, activo) VALUES (@id_nuevo, @id_anterior, @bd, @dominio, 't')"
                        Dim comm_ref1 As New NpgsqlCommand(sql1, conn_mas)
                        comm_ref1.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")
                        comm_ref1.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                        comm_ref1.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
                        comm_ref1.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Dominio.Text
                        comm_ref1.ExecuteNonQuery()

                    End If

                    If Perfil.stat = "U" Then
                        sql1 = "UPDATE users SET login_name=@login_name, user_name=@user_name, password=@password, grupo=@grupo, activo='t', email=@email WHERE user_id=@user_id AND grupo=@grupo"
                        Dim comm_ven As New NpgsqlCommand(sql1, conn_ven)
                        comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                        comm_ven.Parameters.Add("@login_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                        comm_ven.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                        comm_ven.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
                        comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.group
                        comm_ven.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Email.Text 'pw_name_ & "@" & Dominio.Text
                        comm_ven.ExecuteNonQuery()

                        sql1 = "UPDATE referencias_usuarios SET dominio=@dominio, activo='t' WHERE id_nuevo=@id_nuevo AND id_anterior=@id_anterior AND bd=@bd"
                        Dim comm_ref1 As New NpgsqlCommand(sql1, conn_mas)
                        comm_ref1.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")
                        comm_ref1.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                        comm_ref1.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
                        comm_ref1.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Dominio.Text
                        comm_ref1.ExecuteNonQuery()
                    End If

                    'If Perfil.stat = "A" Then
                    '    sql1 = "UPDATE users SET activo='f' WHERE user_id=@user_id AND grupo=@grupo"
                    '    Dim comm_ven As New NpgsqlCommand(sql1, conn_ven)
                    '    comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                    '    comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.group
                    '    comm_ven.ExecuteNonQuery()

                    '    sql1 = "UPDATE referencias_usuarios SET activo='f' WHERE id_nuevo=@id_nuevo AND id_anterior=@id_anterior AND bd=@bd"
                    '    Dim comm_ref1 As New NpgsqlCommand(sql1, conn_mas)
                    '    comm_ref1.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")
                    '    comm_ref1.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                    '    comm_ref1.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
                    '    comm_ref1.ExecuteNonQuery()
                    'End If

                    stat_str = stat_str & Perfil.stat & ","
                    user_id_str = user_id_str & Perfil.user_id & ","
                    group_str = group_str & Perfil.group & ","

                Next

                Dim items As New Dictionary(Of String, String)
                Items.Add("login_name", Login.Text)
                Items.Add("user_name", Nombre.Text)
                Items.Add("password", "")                
                Items.Add("grupo", group_str)
                Items.Add("email", Email.Text)
                Items.Add("id_nuevo", Session("DBAccesosUserId"))
                Items.Add("id_anterior", user_id_str)
                Items.Add("bd", Session("DBAccesos"))
                Items.Add("dominio", Dominio.Text)
                log(Server, Session("DBAccesosUserId"), operacion, "", Items, "Operators", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

                finaliza = True

            Catch ex As Exception
                finaliza = False
                msg = ex.Message
                img = icon_err_update
                css = "alert-danger"
            End Try



            'Dim items As New Dictionary(Of String, String)

            'Activo.Checked = False
            'Dim id_num As Integer
            'Dim j As Integer = 0

            'For Each li As ListItem In Perfil.Items

            '    id_num = 0
            '    sql1 = ""

            '    If IsNumeric(Checkboxlist1.Items(j).Text) Then
            '        id_num = Checkboxlist1.Items(j).Text
            '        If li.Selected = True Then
            '            Activo.Checked = True
            '        End If
            '        operacion = "update"
            '        sql1 = "UPDATE users SET login_name=@login_name, user_name=@user_name, password=@password, grupo=@grupo, activo=@activo, ext=@ext, celular=@celular, email=@email, puesto=@puesto WHERE user_id=@user_id"
            '    Else
            '        If li.Selected = True Then
            '            Activo.Checked = True
            '            operacion = "insert"
            '            sql1 = "INSERT INTO users (login_name, user_name, password, grupo, activo, ext, celular, email, puesto) VALUES (@login_name, @user_name, @password, @grupo, @activo, @ext, @celular, @email, @puesto)"
            '        End If
            '    End If



            '    If sql1 <> "" Then

            '        Dim comm_ven As New NpgsqlCommand(sql1, conn_ven)
            '        'comm_ven.Transaction = trns_ven
            '        comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_num
            '        comm_ven.Parameters.Add("@login_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
            '        comm_ven.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
            '        comm_ven.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
            '        comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = li.Value
            '        comm_ven.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = li.Selected
            '        comm_ven.Parameters.Add("@ext", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
            '        comm_ven.Parameters.Add("@celular", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
            '        comm_ven.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Email.Text
            '        comm_ven.Parameters.Add("@puesto", NpgsqlTypes.NpgsqlDbType.Varchar).Value = li.Text
            '        Try
            '            comm_ven.ExecuteNonQuery()
            '        Catch ex As Exception
            '            msg = ex.Message
            '            img = icon_err_update
            '            css = "alert-danger"
            '            finaliza = False
            '            Exit For
            '        End Try


            '        'LOG
            '        items.Clear()
            '        items.Add("user_id", id_num)
            '        items.Add("grupo", li.Value)
            '        items.Add("activo", li.Selected)

            '        'log_txt = js.Serialize(items)
            '        log(Server, Session("DBAccesosUserId"), operacion, "", items, "users", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


            '        If operacion = "insert" Then
            '            sql1 = "SELECT LASTVAL()"
            '            comm_ven.CommandText = sql1
            '            Dim dataread1 As NpgsqlDataReader = comm_ven.ExecuteReader
            '            If dataread1.Read() Then
            '                id_num = dataread1(0)
            '            End If
            '            dataread1.Close()
            '        End If


            '        If operacion = "insert" Then
            '            sql1 = "INSERT INTO referencias_usuarios (id_nuevo, id_anterior, bd, dominio, activo) VALUES (@id_nuevo, @id_anterior, @bd, @dominio, @activo)"
            '        Else
            '            sql1 = "UPDATE referencias_usuarios SET dominio=@dominio, activo=@activo WHERE id_nuevo=@id_nuevo AND id_anterior=@id_anterior AND bd=@bd"
            '        End If

            '        Dim comm As New NpgsqlCommand(sql1, conn_mas)
            '        'comm.Transaction = trns_mas
            '        comm.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")
            '        comm.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_num
            '        comm.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesos")
            '        comm.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Dominio.Text
            '        comm.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = li.Selected
            '        Try
            '            comm.ExecuteNonQuery()
            '        Catch ex As Exception
            '            msg = ex.Message
            '            img = icon_err_update
            '            css = "alert-danger"
            '            finaliza = False
            '            Exit For
            '        End Try

            '        'LOG
            '        items.Clear()
            '        items.Add("id_nuevo", Session("DBAccesosUserId"))
            '        items.Add("id_anterior", id_num)
            '        items.Add("db", id_num)
            '        items.Add("dominio", Dominio.Text)
            '        items.Add("activo", li.Selected)

            '        log(Server, Session("DBAccesosUserId"), operacion, "", items, "referencias_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            '        opero = True
            '    End If

            '    j = j + 1
            'Next




            If finaliza = True Then ' And opero = True Then
                trns_mas.Commit()
                trns_ven.Commit()

                If operacion = "insert" Then
                    msg = "Registro Creado correctamente"
                Else
                    msg = "Registro Actualizado correctamente"
                End If
            Else
                'If opero = False Then
                msg = ""
                'End If
                trns_mas.Rollback()
                trns_ven.Rollback()
            End If

            conn_mas.Close()
            conn_ven.Close()

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
        End Try

        inicia()

    End Sub


    Protected Sub sel_db(ByVal dbname As String, ByVal flag As String)
        Session("DBAccesos") = dbname
        Session("DBAccesos_conn") = Nothing
        'Dim flag As String = TextBox8.Text
        'flag = flag.Replace("[", "<")
        'flag = flag.Replace("]", ">")
        Session("pais") = flag
    End Sub

    'Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

    '    sel_db()
    '    FillPerfiles()

    'End Sub


    Protected Sub RadioButtonList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged

        If RadioButtonList1.SelectedIndex > -1 Then
            sel_db(RadioButtonList1.SelectedValue, RadioButtonList1.SelectedItem.Text)
            FillPerfiles()
        End If

    End Sub

    Protected Sub RadioButtonList2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonList2.SelectedIndexChanged

        If RadioButtonList2.SelectedIndex > -1 Then
            sel_db(RadioButtonList2.SelectedValue, RadioButtonList2.SelectedItem.Text)
            FillPerfiles()
        End If

    End Sub

    Protected Sub inicia()
        FillPaisesDB()
        FillPerfiles()
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

