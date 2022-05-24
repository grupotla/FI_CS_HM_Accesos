Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs


Partial Class mn_WmsNew
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Public Licon_open As String = icon_open
    Public Licon_new As String = icon_new

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

            inicia()

        End If



    End Sub








    Protected Sub LeerRegistro()

        Try

            Dim htm As String = "<ul id='pestana' class='nav nav-tabs'>"
            htm = htm & menu_gen(Session("sistema"), "", False, Session("OperatorID"), Server)




            Dim modules As String = ""

            Session("insert") = True

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))


                sql1 = "SELECT DOMAINVALUE, MEANING FROM DEF_DOMAINS b WHERE b.DOMAIN = 'PRIVILEGIOS' AND MPC02 = '1' ORDER BY MEANING"
                Dim ds2 As New DataSet()
                Dim cmd2 As New MySqlCommand(sql1, conn)
                Dim adp2 As New MySqlDataAdapter(cmd2)
                adp2.Fill(ds2)
                Especiales.DataSource = ds2
                Especiales.DataValueField = "DOMAINVALUE"
                Especiales.DataTextField = "MEANING"
                Especiales.DataBind()




                sql1 = "SELECT DOMAINVALUE, MEANING FROM DEF_DOMAINS WHERE DOMAIN = 'USUARIOS_TIPOS' ORDER BY MEANING"
                Dim ds1 As New DataSet()
                Dim cmd1 As New MySqlCommand(sql1, conn)
                Dim adp1 As New MySqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Tipo.DataSource = ds1
                Tipo.DataTextField = "MEANING"
                Tipo.DataValueField = "DOMAINVALUE"
                Tipo.DataBind()


                sql1 = "SELECT PRIVILEGIO FROM DEF_USERS_PRIVILEGIOS WHERE COD_USER = '" & Session("DBAccesosLogin") & "' AND STAT = 1"
                Dim comm1 As New MySqlCommand(sql1, conn)
                conn.Open()
                Dim dataread1 As MySqlDataReader = comm1.ExecuteReader
                While dataread1.Read()
                    For Each li As ListItem In Especiales.Items
                        If dataread1(0) = li.Value Then
                            li.Selected = True
                        End If
                    Next
                End While
                dataread1.Close()

                sql1 = "SELECT COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, id_usuario, COUNTRIES, PASSWORD_EXPIRES, CHANGE_PASSWORD, MODULES FROM DEF_USERS WHERE COD_USER = @COD_USER"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@COD_USER", MySqlDbType.String).Value = Session("DBAccesosLogin")
                'conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    Codigo.Text = dataread(7)
                    Login.Text = dataread(0)
                    Nombre.Text = dataread(1)
                    Apellido.Text = dataread(2)
                    Grupo.Value = dataread(3)
                    Password.Text = dataread(4)
                    If dataread(5) = 1 Then
                        Activo.Checked = True
                    Else
                        Activo.Checked = False
                    End If

                    If dataread(9) = 1 Then
                        CheckBox1.Checked = True
                    Else
                        CheckBox1.Checked = False
                    End If

                    If dataread(10) = 1 Then
                        CheckBox2.Checked = True
                    Else
                        CheckBox2.Checked = False
                    End If


                    If Not dataread.IsDBNull(11) Then
                        modules = dataread(11)
                    End If

                    Tipo.SelectedValue = dataread(6)

                    Session("insert") = False
                End If

            End Using



            If modules = "" Then modules = "''"

            Checkboxlist1.Items.Clear()
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT DISTINCT sistema, sistema FROM empresas_plantillas_docs ORDER BY sistema"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                Checkboxlist1.DataSource = ds
                Checkboxlist1.DataValueField = "sistema"
                Checkboxlist1.DataTextField = "sistema"
                Checkboxlist1.DataBind()


                sql1 = "SELECT DISTINCT sistema, sistema FROM empresas_plantillas_docs WHERE sistema IN (" & modules & ") ORDER BY sistema"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                'If dataread.Read() Then
                While dataread.Read()
                    For Each li As ListItem In Checkboxlist1.Items
                        If dataread(0) = li.Value Then
                            li.Selected = True
                        End If
                    Next
                End While
                'End If
                dataread.Close()


            End Using


            If Session("insert") = True Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    sql1 = "SELECT pw_name, pw_gecos, pw_passwd, id_usuario FROM usuarios_empresas WHERE id_usuario = '" & Session("DBAccesosUserId") & "'"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        Codigo.Text = dataread(3)
                        Login.Text = dataread(0)
                        Nombre.Text = dataread(1)
                        Apellido.Text = dataread(1)
                        Password.Text = dataread(2)
                    End If
                    dataread.Close()

                    Session("insert") = True

                End Using

            End If

            If Session("insert") = False Then

                htm = htm & "<li id='li_" & "wm3" & "'>"
                htm = htm & "<a onmouseover=this.style.cursor='pointer' id='" & "wms3" & "' name='lnk_' title='" & "Paises & Bodegas" & "'>"
                htm = htm & "Paises & Bodegas" & "</a></li>"

                htm = htm & "<li id='li_" & "wm2" & "'>"
                htm = htm & "<a onmouseover=this.style.cursor='pointer' id='" & "wms2" & "' name='lnk_' title='" & "Privilegios" & "'>"
                htm = htm & "Privilegios" & "</a></li>"

            End If

            htm = htm & "</ul>"
            pestana_lbl.Text = htm




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
        graba("update")
    End Sub



    Private Structure PerfilDat
        Public user_id As Integer
        Public stat As String
        Public group As Integer
    End Structure




    Protected Sub graba(ByVal operacion As String)
        Try

            Dim items As New Dictionary(Of String, String)
            items.Clear()
            Dim sql1 As String = ""

            Dim guardo As Boolean = False
            Using conn As New MySqlConnection(Session("DBAccesos_conn"))



                Dim modules As String = ""
                'If Session("insert") = False Then

                For Each li As ListItem In Checkboxlist1.Items()
                    If li.Selected = True Then
                        'If Session("pais") <> li.Value And li.Value <> "" Then
                        modules = modules & Comillas & Trim(li.Value) & Comillas & ","
                    End If
                Next

                modules = modules.Remove(modules.Length - 1, 1)

                If modules = "" Then modules = "''"



                conn.Open()

                If Session("insert") = True Then
                    sql1 = "INSERT INTO DEF_USERS (COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, PASSWORD_DATE, id_usuario, PASSWORD_EXPIRES, CHANGE_PASSWORD, MODULES) VALUES (@cod_user, @firstname, @lastname, @cod_group, @password, 1, @user_type, NOW(), @user_id, @expires, @change, @mod)"
                    msg = "Registro Creado correctamente"

                    'sql1 = "INSERT INTO DEF_USERS (COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, PASSWORD_DATE, id_usuario, PASSWORD_EXPIRES, CHANGE_PASSWORD, MODULES) "
                    'sql1 = sql1 + "VALUES ('" + Session("DBAccesosLogin") + "', '" + Nombre.Text + "', '" + Apellido.Text + "', '" + Grupo.Value + "', '" + Password.Text + "', 1, " + Tipo.SelectedValue + ", NOW(), " + Session("DBAccesosUserId") + ", 0, 0, " + modules + ")"

                Else
                    sql1 = "UPDATE DEF_USERS SET FIRSTNAME=@firstname, LASTNAME=@lastname, COD_GROUP=@cod_group, USER_TYPE=@user_type, PASSWORD_DATE=NOW(), PASSWORD_EXPIRES=@expires, CHANGE_PASSWORD=@change, MODULES=@mod"
                    msg = "Registro Actualizado correctamente"

                    If operacion = "activar" Then
                        sql1 = sql1 & ", STATUS=@status "
                        msg = "Registro se activo correctamente"
                        items.Add("STATUS", 1)
                    End If

                    sql1 = sql1 & " WHERE COD_USER=@cod_user"

                End If

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@cod_user", MySqlDbType.String).Value = Session("DBAccesosLogin") 'Login.Text
                comm.Parameters.Add("@firstname", MySqlDbType.String).Value = Nombre.Text
                comm.Parameters.Add("@lastname", MySqlDbType.String).Value = Apellido.Text
                comm.Parameters.Add("@cod_group", MySqlDbType.String).Value = Grupo.Value
                comm.Parameters.Add("@password", MySqlDbType.String).Value = Password.Text
                comm.Parameters.Add("@user_type", MySqlDbType.String).Value = Tipo.SelectedValue
                comm.Parameters.Add("@status", MySqlDbType.String).Value = 1
                comm.Parameters.Add("@user_id", MySqlDbType.String).Value = Session("DBAccesosUserId") 'codigo.Value
                comm.Parameters.Add("@expires", MySqlDbType.Int16).Value = 0
                comm.Parameters.Add("@change", MySqlDbType.Int16).Value = 0
                comm.Parameters.Add("@mod", MySqlDbType.String).Value = modules

                comm.ExecuteNonQuery()

                items.Add("COD_USER", Session("DBAccesosLogin"))
                items.Add("FIRSTNAME", Nombre.Text)
                items.Add("LASTNAME", Apellido.Text)
                items.Add("COD_GROUP", Grupo.Value)
                items.Add("PASSWORD", Password.Text)
                items.Add("USER_TYPE", Tipo.SelectedValue)
                items.Add("id_usuario", Session("DBAccesosUserId"))

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



                For Each li As ListItem In Especiales.Items

                    sql1 = "SELECT PRIVILEGIO FROM DEF_USERS_PRIVILEGIOS WHERE COD_USER = @login AND PRIVILEGIO = @priv"
                    Dim comm1 As New MySqlCommand(sql1, conn)
                    comm1.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin") 'Login.Text
                    comm1.Parameters.Add("@priv", MySqlDbType.String).Value = li.Value

                    Dim dataread As MySqlDataReader = comm1.ExecuteReader

                    If li.Selected = True Then

                        If dataread.Read() Then
                            dataread.Close()
                        Else
                            dataread.Close()

                            sql1 = "INSERT INTO DEF_USERS_PRIVILEGIOS (COD_USER, STAT, PRIVILEGIO, DATETIME, REGISTRA) VALUES (@login, 1, @priv, now(), @reg)"
                            Dim comm2 As New MySqlCommand(sql1, conn)
                            comm2.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin") ' Login.Text
                            comm2.Parameters.Add("@priv", MySqlDbType.String).Value = li.Value
                            comm2.Parameters.Add("@reg", MySqlDbType.String).Value = Session("OperatorID")
                            comm2.ExecuteNonQuery()
                        End If

                    Else

                        If dataread.Read() Then

                            dataread.Close()

                            sql1 = "DELETE FROM DEF_USERS_PRIVILEGIOS WHERE COD_USER = @login AND PRIVILEGIO = @priv"
                            Dim comm2 As New MySqlCommand(sql1, conn)
                            comm2.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin") ' Login.Text
                            comm2.Parameters.Add("@priv", MySqlDbType.String).Value = li.Value
                            comm2.ExecuteNonQuery()
                        Else
                            dataread.Close()

                        End If

                    End If

                Next

            End Using

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
        End Try

        inicia()

    End Sub


    Protected Sub sel_db(ByVal dbname As String, ByVal flag As String)
        Session("pais") = dbname
    End Sub


    Protected Sub inicia()
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

