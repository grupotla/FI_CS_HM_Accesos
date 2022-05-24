Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_WmsBodegas
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

            inicia()

        End If



    End Sub








    Protected Sub LeerRegistro()

        Try

            Dim htm As String = "<ul id='pestana' class='nav nav-tabs'>"
            htm = htm & menu_gen(Session("sistema"), "", False, Session("OperatorID"), Server)


            htm = htm & "<li id='li_" & "wm3" & "' class='active'>"
            htm = htm & "<a onmouseover=this.style.cursor='pointer' id='" & "wms3" & "' name='lnk_' title='" & "Paises & Bodegas" & "'>"
            htm = htm & "Paises & Bodegas" & "</a></li>"

            htm = htm & "<li id='li_" & "wm2" & "'>"
            htm = htm & "<a onmouseover=this.style.cursor='pointer' id='" & "wms2" & "' name='lnk_' title='" & "Privilegios" & "'>"
            htm = htm & "Privilegios" & "</a></li>"

            htm = htm & "</ul>"
            pestana_lbl.Text = htm

            RadioListEmpAsignada.Items.Clear()
            RadioButtonList2.Items.Clear()

            Dim paises_str As String = "''"


            Session("insert") = False

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                sql1 = "SELECT COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, id_usuario, COUNTRIES, PASSWORD_EXPIRES, CHANGE_PASSWORD, MODULES FROM DEF_USERS WHERE COD_USER = @COD_USER"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@COD_USER", MySqlDbType.String).Value = Session("DBAccesosLogin")
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    Codigo.Text = dataread(7)
                    Login.Text = dataread(0)
                    Nombre.Text = dataread(1)
 
                    If Not dataread.IsDBNull(8) Then
                        paises_str = dataread(8)
                    End If

                    Session("insert") = False
                End If

            End Using


            If paises_str = "" Then
                paises_str = "''"
            End If

            Dim countries As String = ""



            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT '' AS pais_iso, ' Seleccione Pais' AS nombre_empresa UNION SELECT pais_iso, '<img src=Content/flags/' || substring(pais_iso for 2) || '-flag.gif height=16 /> ' || nombre_empresa as nombre_empresa FROM empresas WHERE activo = 't'  ORDER BY nombre_empresa" 'AND pais_iso NOT IN (" & paises_str & ")
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                CheckListEmpresas.DataSource = ds
                CheckListEmpresas.DataValueField = "pais_iso"
                CheckListEmpresas.DataTextField = "nombre_empresa"
                CheckListEmpresas.DataBind()


                sql1 = "SELECT DISTINCT pais_iso FROM empresas WHERE pais_iso IN (" & paises_str & ")"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                'If dataread.Read() Then
                While dataread.Read()
                    For Each li As ListItem In CheckListEmpresas.Items
                        If dataread(0) = li.Value Then
                            li.Selected = True
                            'countries = countries & Comillas & Trim(li.Value).Substring(0, 2) & Comillas & ","

                            countries = countries & Comillas & Trim(li.Value) & Comillas & ","
                        End If
                    Next
                End While
                'End If
                dataread.Close()


            End Using


            If countries <> "" Then
                countries = countries.Remove(countries.Length - 1, 1)
            Else
                countries = "''"
            End If

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                'sql1 = "SELECT '' AS pais_iso, ' Seleccione Pais' AS nombre_empresa UNION SELECT pais_iso, '<img src=Content/flags/' || substring(pais_iso for 2) || '-flag.gif height=16 /> ' || nombre_empresa as nombre_empresa FROM empresas WHERE activo = 't' AND pais_iso IN (" & paises_str & ") ORDER BY nombre_empresa"
                sql1 = "SELECT '' AS pais_iso, ' Seleccione Pais' AS nombre_empresa UNION SELECT codigo AS pais_iso, '<img src=Content/flags/' || substring(codigo for 2) || '-flag.gif height=16 /> ' || descripcion AS nombre_empresa FROM paises WHERE codigo IN (" & countries & ") " ' CAST(countrie AS text) = 'true' AND CAST(oficina_aimar AS text) = 'true' AND  ORDER BY descripcion"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(sql1, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                RadioListEmpAsignada.DataSource = ds
                RadioListEmpAsignada.DataValueField = "pais_iso"
                RadioListEmpAsignada.DataTextField = "nombre_empresa"
                RadioListEmpAsignada.DataBind()
            End Using





            'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
            ''sql1 = "SELECT '' AS pais_iso, ' Seleccione Pais' AS nombre_empresa UNION SELECT pais_iso, '<img src=Content/flags/' || substring(pais_iso for 2) || '-flag.gif height=16 /> ' || nombre_empresa as nombre_empresa FROM empresas WHERE activo = 't' AND pais_iso NOT IN (" & paises_str & ") ORDER BY nombre_empresa"
            'sql1 = "SELECT '' AS pais_iso, ' Seleccione Pais' AS nombre_empresa UNION SELECT codigo AS pais_iso, '<img src=Content/flags/' || substring(codigo for 2) || '-flag.gif height=16 /> ' || descripcion AS nombre_empresa FROM paises WHERE CAST(countrie AS text) = 'true' AND CAST(oficina_aimar AS text) = 'true' AND codigo IN (" & countries & ") " 'ORDER BY descripcion"
            'Dim ds As New DataSet()
            'Dim cmd As New NpgsqlCommand(sql1, conn)
            'Dim adp As New NpgsqlDataAdapter(cmd)
            'adp.Fill(ds)
            'RadioButtonList2.DataSource = ds
            'RadioButtonList2.DataValueField = "pais_iso"
            'RadioButtonList2.DataTextField = "nombre_empresa"
            'RadioButtonList2.DataBind()
            'End Using



            Try

                If RadioListEmpAsignada.Items.Count > 0 Then
                    RadioListEmpAsignada.SelectedIndex = 0
                    If Session("pais") <> Nothing Then
                        RadioListEmpAsignada.SelectedValue = Session("pais")
                    Else
                        sel_db(RadioListEmpAsignada.SelectedValue, RadioListEmpAsignada.SelectedItem.Text)
                    End If
                End If

                If RadioButtonList2.Items.Count > 0 Then

                    If RadioListEmpAsignada.Items.Count = 0 Then

                        If Session("pais") = Nothing Then
                            sel_db(RadioListEmpAsignada.SelectedValue, RadioListEmpAsignada.SelectedItem.Text)
                        End If

                    End If

                    If RadioListEmpAsignada.SelectedIndex = 0 Then
                        If Session("pais") <> Nothing Then
                            RadioButtonList2.SelectedValue = Session("pais")
                        End If
                    End If

                End If





            Catch ex As Exception
                Perfil.Items.Clear()
                Session("pais") = Nothing
            End Try


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub

    Protected Sub FillBodegas()

        Try
            'Activo.Checked = False
            Perfil.Items.Clear()

            'Session("insert") = True

            If Session("DBAccesos") <> Nothing And Session("pais") <> Nothing Then

                Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                    'COUNTRY = @country
                    sql1 = "SELECT COD_WAREHOUSE, DESCRIPTION FROM DEF_WAREHOUSES WHERE COUNTRY LIKE '%""" + Session("pais") + """%'"

                    sql1 = "SELECT a.COD_WAREHOUSE, a.DESCRIPTION, a.COD_DISTRIBUTION, a.WAREHOUSE_TYPE " & _
                    "FROM DEF_WAREHOUSES a, DEF_DISTRIBUTION_CENTERS c " & _
                    "WHERE b.COD_WAREHOUSE = a.COD_WAREHOUSE  " & _
                    "AND COD_DISTRIBUTION = COD_DC AND (VIEW_NAME = @country OR (VIEW_NAME = '' AND @country = 'GT'))"

                    sql1 = "SELECT a.COD_WAREHOUSE, a.DESCRIPTION, a.COD_DISTRIBUTION, a.WAREHOUSE_TYPE, VIEW_NAME " & _
                    "FROM DEF_WAREHOUSES a " & _
                    "INNER JOIN DEF_DISTRIBUTION_CENTERS c ON COD_DISTRIBUTION = COD_DC " & _
                    "WHERE VIEW_NAME = @country OR (VIEW_NAME = '' AND 'GT' = @country)"

                    Dim ds2 As New DataSet()
                    Dim cmd2 As New MySqlCommand(sql1, conn)
                    cmd2.Parameters.Add("@country", MySqlDbType.String).Value = Session("pais")
                    Dim adp2 As New MySqlDataAdapter(cmd2)
                    adp2.Fill(ds2)
                    Perfil.DataSource = ds2
                    Perfil.DataTextField = "DESCRIPTION"
                    Perfil.DataValueField = "COD_WAREHOUSE"
                    Perfil.DataBind()


                    'bodegas asignadas
                    'sql1 = "SELECT COD_USER, COD_WAREHOUSE FROM DEF_USERS_WAREHOUSES WHERE COD_USER = @login"
                    'sql1 = "SELECT a.COD_WAREHOUSE, a.DESCRIPTION, a.COD_DISTRIBUTION, a.WAREHOUSE_TYPE, a.COUNTRY FROM DEF_WAREHOUSES a, DEF_USERS_WAREHOUSES b WHERE b.COD_WAREHOUSE = a.COD_WAREHOUSE AND b.COD_USER = @login AND b.COUNTRY = @country"

                    sql1 = "SELECT a.COD_WAREHOUSE, a.DESCRIPTION, a.COD_DISTRIBUTION, a.WAREHOUSE_TYPE " & _
"FROM DEF_WAREHOUSES a, DEF_USERS_WAREHOUSES b, DEF_DISTRIBUTION_CENTERS c " & _
"WHERE b.COD_WAREHOUSE = a.COD_WAREHOUSE AND b.COD_USER = @login  " & _
"AND COD_DISTRIBUTION = COD_DC AND (VIEW_NAME = @country OR (VIEW_NAME = '' AND @country = 'GT'))"


                    sql1 = " SELECT " & _
                        "a.COD_WAREHOUSE, a.DESCRIPTION, a.COD_DISTRIBUTION, a.WAREHOUSE_TYPE, VIEW_NAME  " & _
                    "FROM " & _
                       " DEF_WAREHOUSES a " & _
                       " INNER JOIN DEF_USERS_WAREHOUSES b ON b.COD_WAREHOUSE = a.COD_WAREHOUSE AND b.COD_USER = @login " & _
                       " INNER JOIN DEF_DISTRIBUTION_CENTERS c ON COD_DISTRIBUTION = COD_DC " & _
                   "WHERE VIEW_NAME = @country OR (VIEW_NAME = '' AND 'GT' = @country) "



                    Dim comm1 As New MySqlCommand(sql1, conn)
                    comm1.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                    comm1.Parameters.Add("@country", MySqlDbType.String).Value = Session("pais")
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

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub



    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub








    Protected Sub graba(ByVal operacion As String)


        Dim trans As MySqlTransaction = Nothing

        Try

            Dim items As New Dictionary(Of String, String)
            items.Clear()


            Dim countries As String = ""
            Dim countries2 As String = ""
            'For Each li As ListItem In RadioListEmpAsignada.Items()
            'If li.Value <> Session("pais") And li.Value <> "" Then
            'countries = countries & Comillas & Trim(li.Value) & Comillas & ","
            'End If
            'Next

            'For Each li As ListItem In RadioButtonList2.Items()
            'If li.Selected = True Then
            'If li.Value <> Session("pais") And li.Value <> "" Then
            'countries = countries & Comillas & Trim(li.Value) & Comillas & ","
            'End If
            'End If
            'Next


            For Each li As ListItem In CheckListEmpresas.Items()
                If li.Selected = True Then
                    If li.Value <> "" Then
                        countries = countries & Comillas & Trim(li.Value) & Comillas & ","
                        'countries2 = countries2 & Comillas & Trim(li.Value).ToString().Substring(0, 2) & Comillas & ","
                        countries2 = countries2 & Comillas & Trim(li.Value) & Comillas & ","
                    End If
                End If
            Next

            If countries <> "" Then
                countries = countries.Remove(countries.Length - 1, 1)
            Else
                countries = "''"
            End If

            If countries2 <> "" Then
                countries2 = countries2.Remove(countries2.Length - 1, 1)
            Else
                countries2 = "''"
            End If


            Dim guardo As Boolean = False
            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                conn.Open()

                trans = conn.BeginTransaction

                'sql1 = "DELETE b FROM DEF_USERS_WAREHOUSES b INNER JOIN DEF_WAREHOUSES a ON b.COD_WAREHOUSE = a.COD_WAREHOUSE AND b.COUNTRY = @country WHERE b.COD_USER = @login"
                Dim comm0 As New MySqlCommand()

                sql1 = " DELETE b " & _
                          "  FROM " & _
                           "     DEF_WAREHOUSES a " & _
                            "    INNER JOIN DEF_USERS_WAREHOUSES b ON b.COD_WAREHOUSE = a.COD_WAREHOUSE AND b.COD_USER = @login " & _
                            "    INNER JOIN DEF_DISTRIBUTION_CENTERS c ON COD_DISTRIBUTION = COD_DC " & _
                           "WHERE VIEW_NAME = @country OR (VIEW_NAME = '' AND 'GT' = @country) "

                Dim str As String = ""
                For Each li As ListItem In CheckListEmpresas.Items()

                    If li.Selected = True Then

                    Else
                        If li.Value <> "" Then

                            'str = li.Value.ToString().Substring(0, 2)
                            str = li.Value.ToString()

                            If countries2.IndexOf(str) = -1 Then

                                comm0.Parameters.Clear()
                                comm0 = New MySqlCommand(sql1, conn)
                                comm0.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                                'comm0.Parameters.Add("@country", MySqlDbType.String).Value = li.Value.ToString().Substring(0, 2)
                                comm0.Parameters.Add("@country", MySqlDbType.String).Value = li.Value
                                comm0.ExecuteNonQuery()
                            End If

                        End If
                    End If
                Next

                comm0.Parameters.Clear()
                comm0 = New MySqlCommand(sql1, conn)
                comm0.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                comm0.Parameters.Add("@country", MySqlDbType.String).Value = Session("pais")
                comm0.ExecuteNonQuery()




                For Each li As ListItem In CheckListEmpresas.Items()
                    If li.Selected = True Then

                        'If li.Value.ToString().Substring(0, 2) = Session("pais") Then
                        If li.Value.ToString() = Session("pais") Then
                            guardo = True
                        End If

                    End If
                Next

                If guardo Then
                    For Each li As ListItem In Perfil.Items

                        If li.Selected = True Then
                            sql1 = "INSERT INTO DEF_USERS_WAREHOUSES (COD_USER, COD_WAREHOUSE) VALUES (@login, @bodega)"
                            Dim comm2 As New MySqlCommand(sql1, conn)
                            comm2.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin") 'Login.Text
                            comm2.Parameters.Add("@bodega", MySqlDbType.String).Value = li.Value
                            comm2.Parameters.Add("@country", MySqlDbType.String).Value = Session("pais")
                            comm2.ExecuteNonQuery()
                            'guardo = True
                        End If

                    Next

                End If


                If guardo Then
                    'countries = countries & Comillas & Session("pais") & Comillas & ","
                End If



                sql1 = "UPDATE DEF_USERS SET COUNTRIES=@country WHERE COD_USER=@cod_user"
                msg = "Registro Actualizado correctamente"

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@cod_user", MySqlDbType.String).Value = Session("DBAccesosLogin") ' Login.Text
                comm.Parameters.Add("@country", MySqlDbType.String).Value = countries
                comm.ExecuteNonQuery()

                items.Add("COD_USER", Session("DBAccesosLogin"))
                items.Add("COUNTRIES", countries)


                trans.Commit()

                Dim pais_iso As String = connection.getEmpresaUser(Codigo.Text, Server)
                connection.setEmpresaUser("wms", Codigo.Text, pais_iso, pais_iso, Server)


                log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            End Using

        Catch ex As Exception
            'trans.Rollback()
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
        End Try

        'LeerRegistro()
        inicia()

    End Sub




    Protected Sub sel_db(ByVal dbname As String, ByVal flag As String)
        Session("pais") = dbname
    End Sub



    Protected Sub RadioListEmpAsignada_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioListEmpAsignada.SelectedIndexChanged

        If RadioListEmpAsignada.SelectedIndex > -1 Then
            'RadioButtonList2.SelectedIndex = 0
            sel_db(RadioListEmpAsignada.SelectedValue, RadioListEmpAsignada.SelectedItem.Text)
            FillBodegas()
        End If

    End Sub

    Protected Sub RadioButtonList2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles RadioButtonList2.SelectedIndexChanged

        If RadioButtonList2.SelectedIndex > -1 Then
            RadioListEmpAsignada.SelectedIndex = 0
            sel_db(RadioButtonList2.SelectedValue, RadioButtonList2.SelectedItem.Text)
            FillBodegas()
        End If

    End Sub

    Protected Sub inicia()
        LeerRegistro()
        FillBodegas()
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

