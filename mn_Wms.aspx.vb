Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs
'Imports System.Web.Script.Serialization

Partial Class mn_Wms
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

            Grupo.Items.Clear()
            Tipo.Items.Clear()
            Grupo.SelectedIndex = -1
            Tipo.SelectedIndex = -1
            Activo.Checked = False

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "SELECT * FROM DEF_GROUPS ORDER BY DESCRIPTION"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(sql1, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                Grupo.DataSource = ds
                Grupo.DataTextField = "DESCRIPTION"
                Grupo.DataValueField = "COD_GROUP"
                Grupo.DataBind()

                sql1 = "SELECT DOMAINVALUE, MEANING FROM DEF_DOMAINS WHERE DOMAIN = 'USUARIOS_TIPOS' ORDER BY MEANING"
                Dim ds1 As New DataSet()
                Dim cmd1 As New MySqlCommand(sql1, conn)
                Dim adp1 As New MySqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Tipo.DataSource = ds1
                Tipo.DataTextField = "MEANING"
                Tipo.DataValueField = "DOMAINVALUE"
                Tipo.DataBind()


                sql1 = "SELECT  COD_WAREHOUSE, DESCRIPTION FROM DEF_WAREHOUSES"
                Dim ds2 As New DataSet()
                Dim cmd2 As New MySqlCommand(sql1, conn)
                Dim adp2 As New MySqlDataAdapter(cmd2)
                adp2.Fill(ds2)
                Bodega.DataSource = ds2
                Bodega.DataTextField = "DESCRIPTION"
                Bodega.DataValueField = "COD_WAREHOUSE"
                Bodega.DataBind()

                'Checkboxlist1.DataSource = ds2
                'Checkboxlist1.DataTextField = "DESCRIPTION"
                'Checkboxlist1.DataValueField = "COD_WAREHOUSE"
                'Checkboxlist1.DataBind()


                sql1 = "SELECT COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, id_usuario FROM DEF_USERS WHERE COD_USER = @login"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    codigo.Value = dataread(7)
                    Login.Text = dataread(0)
                    Nombre.Text = dataread(1)
                    Apellido.Text = dataread(2)
                    Grupo.SelectedValue = dataread(3)
                    Password.Text = dataread(4)
                    If dataread(5) = 1 Then
                        Activo.Checked = True
                    Else
                        Activo.Checked = False
                    End If
                    Tipo.SelectedValue = dataread(6)
                    Session("insert") = False
                End If
                dataread.Close()


                Dim str_bod As String = ""
                Dim str_chk As String = ""

                'bodegas asignadas
                sql1 = "SELECT COD_USER, COD_WAREHOUSE FROM DEF_USERS_WAREHOUSES WHERE COD_USER = @login"
                Dim comm1 As New MySqlCommand(sql1, conn)
                comm1.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                'conn.Open()
                Dim dataread1 As MySqlDataReader = comm1.ExecuteReader
                While dataread1.Read()

                    str_bod = str_bod & dataread1(1) & ","
                    str_chk = str_chk & dataread1(1) & "," ' en el futuro se agregara un campo estatus 

                End While
                dataread.Close()


                Dim arr_bod() As String = str_bod.Split(",")
                Dim arr_chk() As String = str_chk.Split(",")

                Dim i As Integer = 0
                Dim j As Integer = 0
                For Each li As ListItem In Bodega.Items
                    For i = 0 To arr_bod.Length - 1
                        If arr_bod(i) = li.Value Then
                            'If arr_chk(i) = True Then                            
                            li.Selected = True
                            'End If
                            Exit For
                        End If
                    Next
                    j = j + 1
                Next


            End Using


            If Session("insert") = True Then

                Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                    sql1 = "SELECT pw_name, pw_gecos, pw_passwd, id_usuario FROM usuarios_empresas WHERE id_usuario = '" & Session("DBAccesosUserId") & "'"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        codigo.Value = dataread(3)
                        Login.Text = dataread(0)
                        Nombre.Text = dataread(1)
                        Apellido.Text = dataread(1)
                        Password.Text = dataread(2)
                    End If
                    dataread.Close()

                    Session("insert") = True

                End Using

            End If






        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub







    Protected Sub graba(ByVal operacion As String)
        Try

            Dim items As New Dictionary(Of String, String)
            items.Clear()

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                conn.Open()
                If Session("insert") = True Then
                    sql1 = "INSERT INTO DEF_USERS (COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, PASSWORD_DATE, id_usuario, PASSWORD_EXPIRES, CHANGE_PASSWORD) VALUES (@cod_user, @firstname, @lastname, @cod_group, @password, 1, @user_type, NOW(), @user_id, 0, 0)"
                    msg = "Registro Creado correctamente"
                Else
                    sql1 = "UPDATE DEF_USERS SET FIRSTNAME=@firstname, LASTNAME=@lastname, COD_GROUP=@cod_group, USER_TYPE=@user_type, PASSWORD_DATE=NOW()"
                    msg = "Registro Actualizado correctamente"

                    If operacion = "activar" Then
                        sql1 = sql1 & ", STATUS=@status "
                        msg = "Registro se activo correctamente"
                        items.Add("STATUS", 1)
                    End If

                    sql1 = sql1 & " WHERE COD_USER=@cod_user"

                End If

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@cod_user", MySqlDbType.String).Value = Login.Text
                comm.Parameters.Add("@firstname", MySqlDbType.String).Value = Nombre.Text
                comm.Parameters.Add("@lastname", MySqlDbType.String).Value = Apellido.Text
                comm.Parameters.Add("@cod_group", MySqlDbType.String).Value = Grupo.SelectedValue
                comm.Parameters.Add("@password", MySqlDbType.String).Value = Password.Text
                comm.Parameters.Add("@user_type", MySqlDbType.String).Value = Tipo.SelectedValue
                comm.Parameters.Add("@status", MySqlDbType.String).Value = 1
                comm.Parameters.Add("@user_id", MySqlDbType.String).Value = codigo.value

                comm.ExecuteNonQuery()

                items.Add("COD_USER", Session("DBAccesosLogin"))
                items.Add("FIRSTNAME", Nombre.Text)
                items.Add("LASTNAME", Apellido.Text)
                items.Add("COD_GROUP", Grupo.SelectedValue)
                items.Add("PASSWORD", Password.Text)
                items.Add("USER_TYPE", Tipo.SelectedValue)

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



                For Each li As ListItem In Bodega.Items

                    If li.Selected = True Then

                        sql1 = "SELECT COD_USER, COD_WAREHOUSE FROM DEF_USERS_WAREHOUSES WHERE COD_USER = @login AND COD_WAREHOUSE = @bodega"
                        Dim comm1 As New MySqlCommand(sql1, conn)
                        comm1.Parameters.Add("@login", MySqlDbType.String).Value = Login.Text
                        comm1.Parameters.Add("@bodega", MySqlDbType.String).Value = li.Value

                        Dim dataread As MySqlDataReader = comm1.ExecuteReader
                        If dataread.Read() Then
                            dataread.Close()
                            'sql1 = "UPDATE DEF_USERS_WAREHOUSES SET WHERE COD_USER = @login AND COD_WAREHOUSE = @bodega"
                        Else
                            dataread.Close()
                            sql1 = "INSERT INTO DEF_USERS_WAREHOUSES (COD_USER, COD_WAREHOUSE) VALUES (@login, @bodega)"
                            Dim comm2 As New MySqlCommand(sql1, conn)
                            comm2.Parameters.Add("@login", MySqlDbType.String).Value = Login.Text
                            comm2.Parameters.Add("@bodega", MySqlDbType.String).Value = li.Value
                            comm2.ExecuteNonQuery()
                        End If

                    End If

                Next


            End Using




        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
        End Try

        LeerRegistro()

    End Sub



    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        'activar("1")
        graba("activar")
    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar("2")
    End Sub


    Protected Sub activar(ByVal operacion As String)
        Try


            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                conn.Open()

                sql1 = "UPDATE DEF_USERS SET  STATUS=@STATUS WHERE COD_USER=@cod_user"

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@cod_user", MySqlDbType.String).Value = Login.Text
                comm.Parameters.Add("@STATUS", MySqlDbType.String).Value = operacion
                comm.ExecuteNonQuery()

            End Using


            Dim items As New Dictionary(Of String, String)
            items.Clear()
            items.Add("STATUS", operacion)
            items.Add("USER_TYPE", Tipo.SelectedValue)


            If operacion = "1" Then
                operacion = "activo"
            Else
                operacion = "desactivo"
            End If

            msg = "Registro se " & operacion & " correctamente"

            log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
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
