Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_WmsNewPriv
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

        'If valida_pagina(Request.ServerVariables("SCRIPT_NAME"), GetConnectionStringFromFile("aimar", Server), Session("OperatorID"), Session("sistema")) = False Then
        'Response.Redirect("Default.aspx")
        'End If

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


            htm = htm & "<li id='li_" & "wm3" & "'>"
            htm = htm & "<a onmouseover=this.style.cursor='pointer' id='" & "wms3" & "' name='lnk_' title='" & "Paises & Bodegas" & "'>"
            htm = htm & "Paises & Bodegas" & "</a></li>"

            htm = htm & "<li id='li_" & "wm2" & "' class='active'>"
            htm = htm & "<a onmouseover=this.style.cursor='pointer' id='" & "wms2" & "' name='lnk_' title='" & "Privilegios" & "'>"
            htm = htm & "Privilegios" & "</a></li>"

            htm = htm & "</ul>"
            pestana_lbl.Text = htm





            RadioButtonList1.Items.Clear()


            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "SELECT '' AS COD_GROUP, '-- Seleccione Perfil --' AS DESCRIPTION UNION SELECT DISTINCT a.COD_GROUP, a.DESCRIPTION  FROM DEF_GROUPS a, DEF_PRIV_X_GROUP b WHERE a.COD_GROUP = b.COD_GROUP ORDER BY DESCRIPTION"

                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(sql1, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                RadioButtonList1.DataSource = ds
                RadioButtonList1.DataValueField = "COD_GROUP"
                RadioButtonList1.DataTextField = "DESCRIPTION"
                RadioButtonList1.DataBind()


                sql1 = "SELECT COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, id_usuario, COUNTRIES, PASSWORD_EXPIRES, CHANGE_PASSWORD, MODULES FROM DEF_USERS WHERE COD_USER = @COD_USER"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@COD_USER", MySqlDbType.String).Value = Session("DBAccesosLogin")
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    Codigo.Text = dataread(7)
                    Login.Text = dataread(0)
                    Nombre.Text = dataread(1)

                    iGrupo.Text = dataread(3)

                    RadioButtonList1.SelectedIndex = 0

                    If RadioButtonList1.Items.Count > 0 Then
                        If iGrupo.Text <> "" Then
                            RadioButtonList1.SelectedValue = iGrupo.Text
                            LeePerfiles(iGrupo.Text)
                        End If

                    End If


                    Session("insert") = False
                End If




            End Using



            Dim pais As String = ""
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT pais FROM usuarios_empresas WHERE id_usuario = '" & Session("DBAccesosUserId") & "'"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    pais = dataread(0)
                End If
                dataread.Close()

            End Using


            iPais.Text = pais


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub


    Protected Sub LeePerfiles(ByRef grupo As String)

        Try

            'Dim pais As String = ""
            'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
            '    sql1 = "SELECT pais FROM usuarios_empresas WHERE id_usuario = '" & Session("DBAccesosUserId") & "'"
            '    Dim comm As New NpgsqlCommand(sql1, conn)
            '    conn.Open()
            '    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
            '    If dataread.Read() Then
            '        pais = dataread(0)
            '    End If
            '    dataread.Close()
            'End Using

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "SELECT DISTINCT DOMAINVALUE, CONCAT(MPC03,' - ',MEANING) AS MEANING, MPC02 FROM DEF_PRIV_X_GROUP a, DEF_DOMAINS b WHERE a.COD_GROUP = '" & grupo & "' AND b.DOMAIN = 'PRIVILEGIOS' AND a.PRIVILEGE = b.DOMAINVALUE AND CAST(MPC02 AS SIGNED) > 1 AND (MPC03 NOT LIKE 'Reportes Aduana%' AND MPC03 NOT LIKE 'ON HAND%') " & _
                " UNION " & _
                "       SELECT DISTINCT DOMAINVALUE, CONCAT(MPC03,' - ',MEANING) AS MEANING, MPC02 FROM DEF_PRIV_X_GROUP a, DEF_DOMAINS b WHERE a.COD_GROUP = '" & grupo & "' AND b.DOMAIN = 'PRIVILEGIOS' AND a.PRIVILEGE = b.DOMAINVALUE AND CAST(MPC02 AS SIGNED) > 1 AND (MPC03 LIKE 'Reportes Aduana " + iPais.Text + "' OR MPC03 LIKE 'ON HAND " + iPais.Text + "') " & _
                " ORDER BY CAST(MPC02 AS SIGNED), MEANING "


                Dim ds2 As New DataSet()
                Dim cmd2 As New MySqlCommand(sql1, conn)
                Dim adp2 As New MySqlDataAdapter(cmd2)
                adp2.Fill(ds2)
                Perfil.DataSource = ds2
                Perfil.DataValueField = "DOMAINVALUE"
                Perfil.DataTextField = "MEANING"
                Perfil.DataBind()


                sql1 = "SELECT PRIVILEGIO FROM DEF_USERS_PRIVILEGIOS WHERE COD_USER = '" & Session("DBAccesosLogin") & "' AND STAT = 1"
                Dim comm1 As New MySqlCommand(sql1, conn)
                conn.Open()
                Dim dataread1 As MySqlDataReader = comm1.ExecuteReader
                While dataread1.Read()
                    For Each li As ListItem In Perfil.Items
                        If dataread1(0) = li.Value Then
                            li.Selected = True
                        End If
                    Next
                End While

            End Using


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub



    Protected Sub RadioButtonList1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged

        If RadioButtonList1.SelectedIndex > -1 Then
            LeePerfiles(RadioButtonList1.SelectedValue)
        End If

    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub



    Protected Sub graba(ByVal operacion As String)
        Try

            Dim items As New Dictionary(Of String, String)
            items.Clear()

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                conn.Open()

                sql1 = "UPDATE DEF_USERS SET COD_GROUP=@cod_group WHERE COD_USER=@cod_user"
                msg = "Registro Actualizado correctamente"

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@cod_user", MySqlDbType.String).Value = Session("DBAccesosLogin") 'Login.Text
                comm.Parameters.Add("@cod_group", MySqlDbType.String).Value = RadioButtonList1.SelectedValue

                comm.ExecuteNonQuery()

                items.Add("COD_USER", Session("DBAccesosLogin"))
                items.Add("COD_GROUP", RadioButtonList1.SelectedValue)

                'log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



                iGrupo.Text = RadioButtonList1.SelectedValue

                sql1 = "DELETE FROM DEF_USERS_PRIVILEGIOS WHERE COD_USER = @login AND PRIVILEGIO IN ( " & _
"SELECT DISTINCT DOMAINVALUE FROM DEF_PRIV_X_GROUP a, DEF_DOMAINS b WHERE a.COD_GROUP = '" & iGrupo.Text & "' AND b.DOMAIN = 'PRIVILEGIOS' AND a.PRIVILEGE = b.DOMAINVALUE AND CAST(MPC02 AS SIGNED) > 1 AND (MPC03 NOT LIKE 'Reportes Aduana%' AND MPC03 NOT LIKE 'ON HAND%') " & _
" UNION " & _
"SELECT DISTINCT DOMAINVALUE FROM DEF_PRIV_X_GROUP a, DEF_DOMAINS b WHERE a.COD_GROUP = '" & iGrupo.Text & "' AND b.DOMAIN = 'PRIVILEGIOS' AND a.PRIVILEGE = b.DOMAINVALUE AND CAST(MPC02 AS SIGNED) > 1 AND (MPC03 LIKE 'Reportes Aduana " + iPais.Text + "' OR MPC03 LIKE 'ON HAND " + iPais.Text + "') )"

                Dim comm2 As New MySqlCommand(sql1, conn)
                comm2.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin") ' Login.Text
                'comm2.Parameters.Add("@priv", MySqlDbType.String).Value = li.Value
                comm2.ExecuteNonQuery()

                comm2.Parameters.Clear()


                For Each li As ListItem In Perfil.Items
                    If li.Selected = True Then

                        sql1 = "INSERT INTO DEF_USERS_PRIVILEGIOS (COD_USER, STAT, PRIVILEGIO, DATETIME, REGISTRA) VALUES (@login, 1, @priv, now(), @reg)"
                        comm2 = New MySqlCommand(sql1, conn)
                        comm2.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin") ' Login.Text
                        comm2.Parameters.Add("@priv", MySqlDbType.String).Value = li.Value
                        comm2.Parameters.Add("@reg", MySqlDbType.String).Value = Session("OperatorID")
                        comm2.ExecuteNonQuery()
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

End Class
