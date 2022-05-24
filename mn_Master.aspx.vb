Imports Npgsql
Imports MySql.Data.MySqlClient
Imports System.Data
Imports connection
Imports logs
Imports cMd5Hash

Partial Class mn_Master
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"
    Public htm As String = ""

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Public Licon_cancel As String = icon_cancel
    Public Licon_edit As String = icon_edit

    Public Licon_pasgen As String = icon_pasgen
    Public Licon_pascan As String = icon_pascan
    Public Licon_pasenc As String = icon_pasenc
    Public Licon_clonar As String = icon_clonar


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session("OperatorID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If valida_pagina(Request.ServerVariables("SCRIPT_NAME"), GetConnectionStringFromFile("aimar", Server), Session("OperatorID"), Session("sistema")) = False Then
            Response.Redirect("Default.aspx")
        End If

        If Session("OperatorLevel") <> 1 Then
            msg = "No tiene suficientes permisos"
        End If

        If Not IsPostBack Then

            Dim htm As String = "<ul id='pestana' class='nav nav-tabs'>"
            htm = htm & menu_gen(Session("sistema"), "", False, Session("OperatorID"), Server)
            htm = htm & "</ul>"

            pestana_lbl.Text = htm

            LeerRegistro()

        End If

    End Sub



    Protected Sub LeerRegistro()

        Try

            Pais.Items.Clear()

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                sql1 = "SELECT '' as pais, '-- Seleccione --' as nombre UNION (SELECT trim(pais_iso) as pais, nombre_empresa as nombre FROM empresas WHERE activo = 't') ORDER BY nombre"
                Dim ds3 As New DataSet()
                Dim cmd3 As New NpgsqlCommand(sql1, conn)
                Dim adp3 As New NpgsqlDataAdapter(cmd3)
                adp3.Fill(ds3)
                PaisesContactos.DataSource = ds3
                PaisesContactos.DataTextField = "nombre"
                PaisesContactos.DataValueField = "pais"
                PaisesContactos.DataBind()


                conn.Open()

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

                sql1 = "SELECT id_def_usuario, descripcion_usuario FROM definicion_usuario ORDER BY descripcion_usuario"
                Dim ds2 As New DataSet()
                Dim cmd2 As New NpgsqlCommand(sql1, conn)
                Dim adp2 As New NpgsqlDataAdapter(cmd2)
                adp2.Fill(ds2)
                tipo_usuario.DataSource = ds2
                tipo_usuario.DataTextField = "descripcion_usuario"
                tipo_usuario.DataValueField = "id_def_usuario"
                tipo_usuario.DataBind()


                '"SELECT 'aimargroup.com' as dominio UNION SELECT 'equitrans.net' as dominio UNION SELECT 'grhlogistics.com' as dominio UNION SELECT 'isisurveyor.com' as dominio UNION SELECT 'latinfreightneutral.com' as dominio UNION SELECT 'latinneutral.com' as dominio UNION SELECT 'mayanlogistics.com' as dominio UNION SELECT 'grupotla.com' as dominio" & _
                sql1 = "SELECT DISTINCT ide, dominio FROM (SELECT '' as ide, '-- Seleccione --' as dominio UNION " & _
                " SELECT DISTINCT dominio as ide, dominio FROM usuarios_empresas WHERE dominio <> '' AND pw_activo = 1 AND dominio <> 'aproasa.com' AND dominio <> 'gmail.com' AND dominio <> 'imi.com.pa' AND dominio <> 'outlook.com'" & _
                " UNION " & _
                " SELECT DISTINCT dominio as ide, dominio FROM empresas_parametros WHERE dominio <> '' AND activo = 't') X ORDER BY dominio"
                Dim ds1 As New DataSet()
                Dim cmd1 As New NpgsqlCommand(sql1, conn)
                Dim adp1 As New NpgsqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Dominio.DataSource = ds1
                Dominio.DataTextField = "dominio"
                Dominio.DataValueField = "dominio"
                Dominio.DataBind()




                If Session("DBAccesosUserId") <> Nothing Then
                    'Using conn As New NpgsqlConnection(Session("DBAccesos_conn"))
                    sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio, tipo_usuario, level, pw_activo, pw_correo, pw_passwd, locode, exactus_pagos FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                    Dim comm As New NpgsqlCommand(sql1, conn)
                    comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    'conn.Open()
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        codigo.Value = dataread(0)
                        Login.Text = dataread(1)
                        Nombre.Text = dataread(2)
                        For Each li As ListItem In Pais.Items
                            If Trim(li.Value) = dataread(3) Then
                                li.Selected = True
                            End If
                        Next
                        Dominio.SelectedValue = dataread(4)
                        tipo_usuario.SelectedValue = dataread(5)
                        Nivel.SelectedValue = dataread(6)
                        Activo.Checked = dataread(7)
                        Correo.SelectedValue = dataread(8)
                        Password.Text = Trim(dataread(9))
                        Email.Text = dataread(1) & "@" & dataread(4)

                        Pais.SelectedValue = Trim(dataread(3))

                        bk_login.Value = dataread(1)
                        bk_nombre.Value = dataread(2)
                        bk_pais.Value = Trim(dataread(3))

                        If Not IsDBNull(dataread(10)) Then
                            bk_locode.Value = dataread(10)
                        End If

                        If Not IsDBNull(dataread(11)) Then
                            exactus_pagos.SelectedValue = dataread(11)
                        End If


                        bk_dominio.Value = dataread(4)
                        bk_tipo.Value = dataread(5)
                        bk_level.Value = dataread(6)
                        bk_correo.Value = dataread(8)
                        bk_password.Value = Trim(dataread(9))
                        bk_email.Value = dataread(1) & "@" & dataread(4)

                    End If

                    dataread.Close()



                    '''''''''''''''''''''''''''''' REPORTES CONTABLES

                    sql1 = "SELECT id, id_usuario, id_estatus FROM usuarios_empresas_contabilidad WHERE id_usuario = " & Session("DBAccesosUserId")
                    'comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                    comm = New NpgsqlCommand(sql1, conn)
                    dataread = comm.ExecuteReader
                    If dataread.Read() Then
                        If dataread(2) = 1 Then
                            RadioButtonListRepCont.SelectedValue = 1
                        Else
                            RadioButtonListRepCont.SelectedValue = 2
                        End If
                    End If
                    dataread.Close()

                    bk_reporte.Value = RadioButtonListRepCont.SelectedValue

                Else

                    For Each li As ListItem In Pais.Items
                        If li.Value = Session("OperatorCountry") Then
                            li.Selected = True
                        End If
                    Next

                End If



                fillUbicacion()

                Try
                    If locode.SelectedIndex > -1 And bk_locode.Value <> "" Then
                        locode.SelectedValue = bk_locode.Value
                    End If

                    If RadioButtonListRepCont.SelectedIndex > -1 And bk_reporte.Value <> "" Then
                        RadioButtonListRepCont.SelectedValue = bk_reporte.Value
                    End If

                Catch ex As Exception
                    msg = ex.Message
                    msg = ""
                End Try


                gen_pass.Visible = False
                'enc_pass.Visible = False
                can_pass.Visible = False

                If Correo.SelectedIndex = -1 Then

                Else
                    If Correo.SelectedValue = 0 Then
                        gen_pass.Visible = True
                        'enc_pass.Visible = True
                        can_pass.Visible = True
                    End If
                End If



                Try

                    sql1 = "SELECT id as ""ID"", pais_iso AS ""PAIS ISO"", " & _
                        " CASE WHEN correo_cc_bc = 0 THEN '--' WHEN correo_cc_bc = 1 THEN 'TO' WHEN correo_cc_bc = 2 THEN 'CC' WHEN correo_cc_bc = 3 THEN 'BC' END as ""TIPO"", " & _
                        "  CASE WHEN accion = 1 THEN 'EXCEL' WHEN accion = 2 THEN 'ALERTA' WHEN accion = 3 THEN 'NOTIFICAION' ELSE '--' END as ""ACCION"", " & _
                        " activo as ""ACTIVO"" FROM usuarios_empresas_exactus_correo WHERE id_usuario = " & Session("DBAccesosUserId") & " ORDER BY pais_iso" 'activo = 't' "
                    Dim comm1 As New NpgsqlCommand(sql1, conn)
                    Dim dataread1 As NpgsqlDataReader = comm1.ExecuteReader
                    users_grid.DataSource = dataread1
                    users_grid.DataBind()

                Catch ex As Exception

                End Try


            End Using



        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try



    End Sub


    Protected Sub btn_cancelar_Click(sender As Object, e As System.EventArgs) Handles btn_cancelar.Click
        inicio()
    End Sub

    Protected Sub fillUbicacion()

        Try

            locode.Items.Clear()

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))


                Dim pais_code As String = Pais.SelectedValue.Substring(0, 2)

                'If pais_code = "N1" Then
                'pais_code = "NI"
                'End If

                sql1 = "SELECT locode, nombre FROM unlocode WHERE pais = @pais ORDER BY nombre"
                Dim ds1 As New DataSet()
                Dim cmd1 As New NpgsqlCommand(sql1, conn)
                cmd1.Parameters.Add("@pais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = IIf(pais_code = "N1", "NI", pais_code)
                Dim adp1 As New NpgsqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                locode.DataSource = ds1
                locode.DataTextField = "nombre"
                locode.DataValueField = "locode"
                locode.DataBind()

            End Using

        Catch ex As Exception
            'msg = ex.Message
            'img = icon_err_active
            'css = "alert-default"
            MsgBox(ex.Message, , "fill_ubicacion")
        End Try



    End Sub

    Protected Sub inicio()

        If (Session("edit") = "edit" And Session("DBAccesosUserId") = Nothing) Or (Session("edit") = Nothing And Session("DBAccesosUserId") <> Nothing) Then
            Session("edit") = Nothing
            Session("sistema") = Nothing
            Session("DBAccesos") = Nothing
            Session("DBAccesos_conn") = Nothing
            Session("DBAccesosUserId") = Nothing
            Session("DBAccesosLogin") = Nothing
            Response.Redirect("Default.aspx")
        Else
            Session("edit") = Nothing
            Response.Redirect("mn_Master.aspx")
        End If

    End Sub

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        graba("insert")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        graba("update")
    End Sub

    Protected Sub btn_editar_Click(sender As Object, e As System.EventArgs) Handles btn_editar.Click
        Session("edit") = "edit"
        LeerRegistro()
        'Response.Redirect("mn_Master.aspx")
    End Sub

    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        activar(True)
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar(False)
    End Sub




    Protected Sub imageButtonClick(sender As Object, e As System.EventArgs)
        'Dim imageButton As ImageButton = sender
        Dim imageButton As LinkButton = sender
        Dim tableCell As TableCell = imageButton.Parent
        Dim row As GridViewRow = tableCell.Parent
        users_grid.SelectedIndex = row.RowIndex
        'Session("DBAccesosUserId") = row.Cells(1).Text
        'Session("DBAccesosLogin") = row.Cells(2).Text
        'inicio()

        Using conn_mas As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

            Session("edit") = "edit"
            LeerRegistro()

            conn_mas.Open()
            ID = -1
            sql1 = "SELECT id, pais_iso, correo_cc_bc, substr(CAST(activo AS text),1,1), accion FROM usuarios_empresas_exactus_correo WHERE id = " & row.Cells(1).Text
            Dim comm1 As New NpgsqlCommand(sql1, conn_mas)
            Dim dataread1 As NpgsqlDataReader = comm1.ExecuteReader
            If dataread1.Read() Then
                exactus_correo_id.Text = dataread1(0)
                PaisesContactos.SelectedValue = dataread1(1)
                TipoContactos.SelectedValue = dataread1(2)
                EstadoContactos.Checked = IIf(dataread1(3) = "t", True, False)

                Accion.SelectedValue = dataread1(4)

                PaisesContactos_bk.Value = dataread1(1)
                TipoContactos_bk.Value = dataread1(2)
                EstadoContactos_bk.Value = dataread1(3)
                Accion_bk.Value = dataread1(4)
            End If
            dataread1.Close()

        End Using



    End Sub


    Protected Sub ReportesContables(conn_mas As NpgsqlConnection)

        Dim id As Integer = -1
        Dim sql1 As String

        Try

            If bk_reporte.Value <> RadioButtonListRepCont.SelectedValue Then


                sql1 = "SELECT id, id_usuario, id_estatus FROM usuarios_empresas_contabilidad WHERE id_usuario = " & Session("DBAccesosUserId")
                'comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                Dim comm As New NpgsqlCommand(sql1, conn_mas)
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    id = dataread(0)
                End If
                dataread.Close()

                If RadioButtonListRepCont.SelectedValue = "" Then
                    RadioButtonListRepCont.SelectedValue = "2"
                End If

                If id = -1 Then
                    msg = "Registró Perfil Reporte Contable"
                    sql1 = "INSERT INTO usuarios_empresas_contabilidad (id_usuario, id_estatus) VALUES (" & Session("DBAccesosUserId") & "," & RadioButtonListRepCont.SelectedValue & ")"
                Else
                    msg = "Actualizó Perfil Reporte Contable"
                    sql1 = "UPDATE usuarios_empresas_contabilidad SET id_estatus = " & RadioButtonListRepCont.SelectedValue & " WHERE id = " & id
                End If

                comm = New NpgsqlCommand(sql1, conn_mas)
                comm.ExecuteNonQuery()

            End If


        Catch ex As Exception
            msg = ex.Message
        End Try

    End Sub





    Protected Sub ExactusCorreos(conn_mas As NpgsqlConnection)
        Dim id As Integer = -1
        Dim sql1 As String

        '///////////////////////////////////////////////////////////////////////////// usuarios_empresas_exactus_correo

        Try

            If exactus_correo_id.Text = "" Then
                exactus_correo_id.Text = "0"
            End If

            If PaisesContactos_bk.Value <> PaisesContactos.SelectedValue Or TipoContactos_bk.Value <> TipoContactos.SelectedValue Or (EstadoContactos_bk.Value = "t" And EstadoContactos.Checked = False Or EstadoContactos_bk.Value = "f" And EstadoContactos.Checked = True) Or Accion_bk.Value <> Accion.SelectedValue Then


                If PaisesContactos.SelectedValue = "" Then
                    msg = "Por favor seleccione Pais"
                    Exit Sub
                End If

                If TipoContactos.SelectedValue = "" Then
                    msg = "Por favor seleccione Tipo Contacto"
                    Exit Sub
                End If

                If Accion.SelectedValue = "" Then
                    msg = "Por favor seleccione Accion"
                    Exit Sub
                End If

                'If EstadoContactos.SelectedValue = "" Then
                'msg = "Por favor seleccione Estado"
                'End If

                id = -1

                sql1 = "SELECT id, pais_iso, correo_cc_bc, activo, accion FROM usuarios_empresas_exactus_correo WHERE "

                If exactus_correo_id.Text = "0" Then
                    sql1 = sql1 & "pais_iso = '" & PaisesContactos.SelectedValue & "' AND id_usuario = " & Session("DBAccesosUserId") & " AND correo_cc_bc = " & TipoContactos.SelectedValue & " AND accion = " & Accion.SelectedValue
                Else
                    sql1 = sql1 & "id = " & exactus_correo_id.Text
                End If

                Dim comm As New NpgsqlCommand(sql1, conn_mas)
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    id = dataread(0)
                End If
                dataread.Close()




                If id = -1 Then
                    msg = "Registró Perfil de Correo"
                    sql1 = "INSERT INTO usuarios_empresas_exactus_correo (id_usuario, pais_iso, correo_cc_bc, activo, accion) VALUES (" & Session("DBAccesosUserId") & ", '" & PaisesContactos.SelectedValue & "', " & TipoContactos.SelectedValue & ", " & IIf(EstadoContactos.Checked = True, "true", "false") & ", '" & Accion.SelectedValue & "')"
                Else

                    If RadioButtonList1.Checked = True Then
                        msg = "Borró Perfil de Correo"
                        sql1 = "DELETE FROM usuarios_empresas_exactus_correo WHERE id = " & id
                    Else
                        msg = "Actualizó Perfil de Correo"
                        sql1 = "UPDATE usuarios_empresas_exactus_correo SET pais_iso = '" & PaisesContactos.SelectedValue & "', correo_cc_bc = " & TipoContactos.SelectedValue & ", activo = '" & IIf(EstadoContactos.Checked = True, "true", "false") & "', accion = '" & Accion.SelectedValue & "' WHERE id = " & id
                    End If

                End If

                comm = New NpgsqlCommand(sql1, conn_mas)
                comm.ExecuteNonQuery()


            Else
                If RadioButtonList1.Checked = True And exactus_correo_id.Text <> "0" Then
                    msg = "Borró Perfil de Correo"
                    sql1 = "DELETE FROM usuarios_empresas_exactus_correo WHERE id = " & exactus_correo_id.Text

                    Dim comm As New NpgsqlCommand(sql1, conn_mas)
                    comm.ExecuteNonQuery()
                End If

            End If


            PaisesContactos.SelectedValue = ""
            TipoContactos.SelectedValue = ""
            Accion.SelectedValue = ""
            EstadoContactos.Checked = False
            exactus_correo_id.Text = ""
            RadioButtonList1.Checked = False


        Catch ex As Exception
            msg = ex.Message
        End Try

    End Sub



    Protected Sub graba(operacion As String)

        Dim randomized1 As String = ""
        Dim finaliza As Boolean = True

        Try

            Dim cambios As Boolean = False
            Login.Text = Trim(Login.Text)
            Nombre.Text = Trim(Nombre.Text)
            Password.Text = Trim(Password.Text)
            'Dominio.Text = Trim(Dominio.Text)
            Email.Text = Login.Text & "@" & Dominio.SelectedValue

            If Login.Text <> bk_login.Value Then
                cambios = True
            End If
            If Nombre.Text <> bk_nombre.Value Then
                cambios = True
            End If
            If Pais.SelectedValue <> bk_pais.Value Then
                cambios = True
            End If
            If Dominio.SelectedValue <> bk_dominio.Value Then
                cambios = True
            End If
            If Nivel.SelectedValue <> bk_level.Value Then
                cambios = True
            End If
            If Password.Text <> bk_password.Value Then
                cambios = True
            End If
            If Email.Text <> bk_email.Value Then
                cambios = True
            End If
            If Correo.SelectedValue <> bk_correo.Value Then
                cambios = True
            End If
            If tipo_usuario.SelectedValue <> bk_tipo.Value Then
                cambios = True
            End If
            If bk_locode.Value <> locode.SelectedValue Then
                cambios = True
            End If

            Dim str As String = Session("DBAccesos_conn")

            If str = "" Then

                Session("DBAccesos_conn") = GetConnectionStringFromFile("aimar", Server)

            End If

            Dim conn_mas As New NpgsqlConnection(Session("DBAccesos_conn"))


            conn_mas.Open()

            If Session("OperatorID") = 1237 Then
                If FechaVencida.Checked Then
                    cambios = True
                End If
            End If


            If cambios = False Then

                ReportesContables(conn_mas)

                ExactusCorreos(conn_mas)

                'Session("edit") = Nothing
                'LeerRegistro()
                finaliza = False
                Exit Try

            End If


            randomized1 = Password.Text
            If Correo.SelectedValue = 0 And randomized1.Length = 10 Then
                Dim HashCode As cMd5Hash
                HashCode = New cMd5Hash()
                Password.Text = HashCode.Md5FromString(Password.Text)
            End If



            Dim items As New Dictionary(Of String, String)


            Dim trns_mas As NpgsqlTransaction
            'conn_mas.Open()
            trns_mas = conn_mas.BeginTransaction()

            If Session("DBAccesosUserId") = Nothing Then
                sql1 = "INSERT INTO usuarios_empresas (pw_name, pw_gecos, pais, pw_passwd, dominio, tipo_usuario, pw_activo, pw_correo, locode, exactus_pagos) VALUES (@pw_name, @pw_gecos, @pais, @pw_passwd, @dominio, @tipo_usuario, 1, @pw_correo, @locode, @exactus_pagos) RETURNING id_usuario"
            Else
                'sql1 = "UPDATE usuarios_empresas SET pw_name=@pw_name, pw_gecos=@pw_gecos, pw_passwd=@pw_passwd, pais=@pais, dominio=@dominio, tipo_usuario=@tipo_usuario, level=@level, pw_correo=@pw_correo, locode=@locode WHERE id_usuario = @codigo"

                sql1 = "UPDATE usuarios_empresas SET "
                sql1 = sql1 & " pw_name=@pw_name, "
                sql1 = sql1 & " pw_gecos=@pw_gecos, "
                sql1 = sql1 & " pw_passwd=@pw_passwd, "
                sql1 = sql1 & " pais=@pais, "
                sql1 = sql1 & " dominio=@dominio, "
                sql1 = sql1 & " tipo_usuario=@tipo_usuario, exactus_pagos=@exactus_pagos,"

                If Nivel.SelectedValue <> "" Then
                    sql1 = sql1 & " level=@level, "
                End If

                If Session("OperatorID") = 1237 Then
                    If FechaVencida.Checked Then
                        sql1 = sql1 & " pw_passwd_fecha = now(), "
                    End If
                End If

                sql1 = sql1 & " pw_correo=@pw_correo, "
                sql1 = sql1 & " locode=@locode "
                sql1 = sql1 & " WHERE id_usuario = @codigo"

            End If

            If Nivel.SelectedValue = "" Then
                Nivel.SelectedValue = "0"
            End If

            If exactus_pagos.SelectedValue = "" Then
                exactus_pagos.SelectedValue = "0"
            End If

            Dim comm As New NpgsqlCommand(sql1, conn_mas)
            comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
            comm.Parameters.Add("@pw_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
            comm.Parameters.Add("@pw_gecos", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
            comm.Parameters.Add("@pw_passwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Password.Text
            comm.Parameters.Add("@pais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Pais.SelectedValue
            comm.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Dominio.SelectedValue
            comm.Parameters.Add("@tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = tipo_usuario.SelectedValue
            comm.Parameters.Add("@level", NpgsqlTypes.NpgsqlDbType.Integer).Value = Nivel.SelectedValue
            'comm.Parameters.Add("@pw_activo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Activo.Checked
            comm.Parameters.Add("@pw_correo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Correo.SelectedValue
            comm.Parameters.Add("@locode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = locode.SelectedValue
            comm.Parameters.Add("@exactus_pagos", NpgsqlTypes.NpgsqlDbType.Integer).Value = exactus_pagos.SelectedValue

            comm.ExecuteNonQuery()

            If operacion = "insert" Then

                sql1 = "SELECT LASTVAL()"
                comm.CommandText = sql1
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    Session("DBAccesosUserId") = dataread(0)
                End If
                dataread.Close()

                ReportesContables(conn_mas)

                ExactusCorreos(conn_mas)


            Else

                ReportesContables(conn_mas)

                ExactusCorreos(conn_mas)

                ''''''''''''''''''''''''''''''''''''''''''''''''''''' U P D A T E '''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Dim conn_aer As New MySqlConnection(GetConnectionStringFromFile("aereo", Server))
                Dim conn_ter As New MySqlConnection(GetConnectionStringFromFile("terrestre", Server))
                Dim conn_cus As New MySqlConnection(GetConnectionStringFromFile("customer", Server))
                Dim conn_wms As New MySqlConnection(GetConnectionStringFromFile("wms", Server))
                Dim conn_baw As New MySqlConnection(GetConnectionStringFromFile("baw", Server))
                Dim conn_man_cr As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & "cr")
                Dim conn_man_crltf As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & "crltf")
                Dim conn_caj As New MySqlConnection(GetConnectionStringFromFile("caja", Server))

                Dim trns_aer As MySqlTransaction = Nothing
                Dim trns_ter As MySqlTransaction = Nothing
                Dim trns_cus As MySqlTransaction = Nothing
                Dim trns_wms As MySqlTransaction = Nothing
                Dim trns_baw As MySqlTransaction = Nothing
                Dim trns_man_cr As NpgsqlTransaction = Nothing
                Dim trns_man_crltf As NpgsqlTransaction = Nothing
                Dim trns_caj As MySqlTransaction = Nothing

                Dim user_aer As Integer = 0
                Dim user_ter As Integer = 0
                Dim user_tra As Integer = 0
                Dim user_cus As Integer = 0
                Dim user_seg As Integer = 0
                Dim user_wm1 As Integer = 0
                Dim user_ba1 As Integer = 0
                Dim user_man_cr As Integer = 0
                Dim user_man_crltf As Integer = 0
                Dim user_caj As Integer = 0


                Dim arr_conn_object() As NpgsqlConnection
                Dim arr_trns_object() As NpgsqlTransaction
                Dim ob As Integer = 0
                Dim i As Integer = 0

                user_aer = user_aereo(Session("DBAccesosUserId"), Server)
                user_ter = user_terrestre(Session("DBAccesosUserId"), Server)
                user_tra = user_trafico_maritimo(Session("DBAccesosUserId"), Server)

                'user_cus = user_customer(Session("DBAccesosUserId"))

                Dim cus() As String = user_customer(Session("DBAccesosUserId"), Server).ToString.Split(",")
                user_cus = cus(0)


                user_seg = user_seguros(Session("DBAccesosUserId"), Server)
                user_wm1 = user_wms(Session("DBAccesosLogin"), Server)
                user_ba1 = user_baw(Session("DBAccesosUserId"), Server)
                'user_man_cr_demo = user_manifiestos_cr_demo(Session("DBAccesosUserId"))
                user_man_cr = user_manifiestos_cr(Session("DBAccesosUserId"), Server)
                user_man_crltf = user_manifiestos_crltf(Session("DBAccesosUserId"), Server)
                user_caj = user_caja(Session("DBAccesosUserId"), Server)


                Try
                    '''''''''''''''''''''''''''''''''''''''''''T R A F I C O   A E R E O ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Nivel.SelectedValue <> bk_level.Value Then
                        cambios = True
                    End If
                    If Email.Text <> bk_email.Value Then
                        cambios = True
                    End If


                    If cambios = True Then

                        conn_aer.Open()
                        trns_aer = conn_aer.BeginTransaction()

                        sql1 = "UPDATE Operators SET Login=@login, FirstName=@firstname, LastName=@lastname, Email=@email WHERE OperatorID = @codigo" ', OperatorLevel=@level
                        Dim comm_aer As New MySqlCommand(sql1, conn_aer)
                        comm_aer.Parameters.Add("@codigo", MySqlDbType.Int32).Value = codigo.Value
                        comm_aer.Parameters.Add("@login", MySqlDbType.String).Value = Login.Text
                        comm_aer.Parameters.Add("@firstname", MySqlDbType.String).Value = Nombre.Text
                        comm_aer.Parameters.Add("@lastname", MySqlDbType.String).Value = Nombre.Text
                        comm_aer.Parameters.Add("@email", MySqlDbType.String).Value = Email.Text
                        'comm_aer.Parameters.Add("@level", MySqlDbType.Int32).Value = tipo_usuario.SelectedValue
                        comm_aer.ExecuteNonQuery()

                        items.Clear()
                        items.Add("OperatorID", codigo.Value)
                        items.Add("Login", Login.Text)
                        items.Add("FirstName", Nombre.Text)
                        items.Add("LastName", Nombre.Text)
                        items.Add("Email", Email.Text)
                        'items.Add("OperatorLevel", tipo_usuario.SelectedValue)
                        'log_txt = js.Serialize(items)
                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "Operators", Session("OperatorID"), Session("Login"), "aereo", "aereo", Session("OperatorIP"))

                    End If

                    If user_aer = 1 Then

                        If Pais.SelectedValue <> bk_pais.Value Then
                            cambios = True
                            connection.setEmpresaUser("aereo", codigo.Value, Pais.SelectedValue, bk_pais.Value, Server)
                        End If

                    End If

                    If cambios = False Then
                        user_aer = 0
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''T R A F I C O   T E R R E S T R E ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Nivel.SelectedValue <> bk_level.Value Then
                        cambios = True
                    End If
                    If Email.Text <> bk_email.Value Then
                        cambios = True
                    End If

                    If cambios = True Then

                        conn_ter.Open()
                        trns_ter = conn_ter.BeginTransaction()

                        sql1 = "UPDATE Operators SET Login=@login, FirstName=@firstname, LastName=@lastname, Email=@email WHERE OperatorID = @codigo"  ', OperatorLevel=@level
                        Dim comm_ter As New MySqlCommand(sql1, conn_ter)
                        comm_ter.Parameters.Add("@codigo", MySqlDbType.Int32).Value = codigo.Value
                        comm_ter.Parameters.Add("@login", MySqlDbType.String).Value = Login.Text
                        comm_ter.Parameters.Add("@firstname", MySqlDbType.String).Value = Nombre.Text
                        comm_ter.Parameters.Add("@lastname", MySqlDbType.String).Value = Nombre.Text
                        comm_ter.Parameters.Add("@email", MySqlDbType.String).Value = Email.Text
                        'comm_ter.Parameters.Add("@level", MySqlDbType.Int32).Value = tipo_usuario.SelectedValue
                        comm_ter.ExecuteNonQuery()

                        items.Clear()
                        items.Add("OperatorID", codigo.Value)
                        items.Add("Login", Login.Text)
                        items.Add("FirstName", Nombre.Text)
                        items.Add("LastName", Nombre.Text)
                        items.Add("Email", Email.Text)
                        items.Add("OperatorLevel", tipo_usuario.SelectedValue)
                        'log_txt = js.Serialize(items)
                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "Operators", Session("OperatorID"), Session("Login"), "terrestre", "terrestre", Session("OperatorIP"))
                    End If

                    If user_ter = 1 Then
                        If Pais.SelectedValue <> bk_pais.Value Then
                            connection.setEmpresaUser("terrestre", codigo.Value, Pais.SelectedValue, bk_pais.Value, Server)
                            cambios = True
                        End If
                    End If

                    If cambios = False Then
                        user_ter = 0
                    End If

                    ' '''''''''''''''''''''''''''''''''''''''''''T R A F I C O   M A R I T I M O ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    'If Password.Text <> bk_password.Value Then
                    'cambios = True
                    'End If
                    If Email.Text <> bk_email.Value Then
                        cambios = True
                    End If
                    If cambios = False Then
                        user_tra = 0
                    End If

                    If user_tra = 1 Then

                        If Dominio.Text <> bk_dominio.Value Then
                            sql1 = "UPDATE referencias_usuarios SET dominio=@dominio WHERE id_nuevo = @codigo and activo = 't'"
                            Dim comm_mas1 As New NpgsqlCommand(sql1, conn_mas)
                            comm_mas1.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Dominio.Text
                            comm_mas1.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                            comm_mas1.ExecuteNonQuery()
                        End If

                        Dim DBAccesos_conn As String


                        Dim str_num As String = ""
                        sql1 = "SELECT DISTINCT bd FROM referencias_usuarios WHERE id_nuevo=@object_id AND activo='t' ORDER BY bd"
                        Dim comm_mas3 As New NpgsqlCommand(sql1, conn_mas)
                        comm_mas3.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        Dim dataread3 As NpgsqlDataReader = comm_mas3.ExecuteReader
                        While dataread3.Read()
                            str_num = str_num & dataread3(0) & ","
                        End While
                        dataread3.Close()
                        Dim arr_num1() As String = str_num.Split(",")
                        ReDim arr_conn_object(arr_num1.Length)
                        ReDim arr_trns_object(arr_num1.Length)
                        For i = 0 To arr_num1.Length - 1
                            DBAccesos_conn = GetConnectionStringFromFile(arr_num1(i), Server)
                            If (DBAccesos_conn <> "") Then
                                ob = ob + 1
                                arr_conn_object(ob) = New NpgsqlConnection(DBAccesos_conn)
                                arr_conn_object(ob).Open()
                                arr_trns_object(ob) = arr_conn_object(ob).BeginTransaction

                                sql1 = "UPDATE users SET login_name=@login_name, user_name=@user_name, password=@password, email=@email WHERE login_name = @object_login and activo = 't'"
                                Dim comm_ven As New NpgsqlCommand(sql1, arr_conn_object(ob))
                                comm_ven.Parameters.Add("@login_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                                comm_ven.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                                comm_ven.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
                                comm_ven.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Email.Text
                                comm_ven.Parameters.Add("@object_login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesosLogin")
                                comm_ven.ExecuteNonQuery()
                            End If
                        Next

                    End If


                    '''''''''''''''''''''''''''''''''''''''''''C U S T O M E R ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Pais.SelectedValue <> bk_pais.Value Then
                        cambios = True
                    End If
                    If Password.Text <> bk_password.Value Then
                        cambios = True
                    End If
                    If Email.Text <> bk_email.Value Then
                        cambios = True
                    End If

                    If bk_locode.Value <> locode.SelectedValue Then
                        cambios = True
                    End If

                    If cambios = False Then
                        user_cus = 0
                    End If
                    If user_cus = 1 Then
                        conn_cus.Open()
                        trns_cus = conn_cus.BeginTransaction()

                        sql1 = "UPDATE usuarios SET id_pais=@id_pais, nombre=@nombre, nombrefull=@nombrefull, email=@email, psw=@psw, user_modifica=@user_modifica, locode=@locode "

                        If bk_locode.Value <> locode.SelectedValue Then
                            sql1 = sql1 & ", acceso_aduana=0 "
                        End If

                        sql1 = sql1 & " WHERE id_usuario_empresa=@id_usuario_empresa and borrado=0"

                        Dim comm_cus As New MySqlCommand(sql1, conn_cus)
                        comm_cus.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = Session("DBAccesosUserId")
                        comm_cus.Parameters.Add("@id_pais", MySqlDbType.String).Value = Trim(Pais.SelectedValue)
                        comm_cus.Parameters.Add("@nombre", MySqlDbType.String).Value = Trim(Login.Text)
                        comm_cus.Parameters.Add("@nombrefull", MySqlDbType.String).Value = Trim(Nombre.Text)
                        'comm.Parameters.Add("@puesto", MySqlDbType.String).Value = Puesto.Text                        
                        comm_cus.Parameters.Add("@email", MySqlDbType.String).Value = Dominio.SelectedValue
                        comm_cus.Parameters.Add("@psw", MySqlDbType.String).Value = Trim(Password.Text)
                        comm_cus.Parameters.Add("@locode", MySqlDbType.String).Value = Pais.SelectedValue & locode.SelectedValue
                        comm_cus.Parameters.Add("@user_modifica", MySqlDbType.String).Value = Session("OperatorID")
                        comm_cus.ExecuteNonQuery()




                        items.Clear()
                        items.Add("id_usuario_empresa", Session("DBAccesosUserId"))
                        items.Add("id_pais", Pais.SelectedValue)
                        items.Add("nombre", Login.Text)
                        items.Add("nombrefull", Nombre.Text)
                        items.Add("email", Dominio.SelectedValue) '2014-04-14 cambio
                        items.Add("psw", Password.Text)
                        items.Add("locode", Pais.SelectedValue & locode.SelectedValue) '2014-04-14 se agrego
                        items.Add("user_modifica", Session("OperatorID"))

                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "usuarios", Session("OperatorID"), Session("Login"), "customer", "customer", Session("OperatorIP"))

                        Dim after_txt As String = Serialize(items)

                        Dim arr_where As New Dictionary(Of String, String)
                        Dim arr_data As New Dictionary(Of String, String)
                        arr_data.Add("user_id", Session("OperatorID"))
                        arr_data.Add("user_name", Session("Login"))
                        arr_data.Add("sistema", "customer")
                        arr_data.Add("db", "customer")
                        arr_data.Add("accion", "update master")
                        arr_data.Add("after_txt", after_txt)
                        arr_data.Add("ip", Session("OperatorIP"))
                        arr_data.Add("tabla", "usuarios")

                        'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                        'conn.Open()
                        'Dim arr_result As New Dictionary(Of String, String)
                        'arr_result = db_oper("insert", "usuarios_empresas_log", "postgres", arr_data, conn_mas, arr_where)
                        'conn.Close()
                        'End Using





                    End If


                    'If user_seg = 1 Then
                    'conn_mas.Open()
                    'End If

                    '''''''''''''''''''''''''''''''''''''''''''W M S '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Password.Text <> bk_password.Value Then
                        cambios = True
                    End If



                    If cambios = True Then
                        conn_wms.Open()
                        trns_wms = conn_wms.BeginTransaction()

                        'sql1 = "UPDATE DEF_USERS SET COD_USER=@cod_user, FIRSTNAME=@firstname, LASTNAME=@lastname, PASSWORD=@password, PASSWORD_DATE=NOW() WHERE COD_USER=@cod_user1"
                        sql1 = "UPDATE DEF_USERS SET COD_USER=@cod_user, FIRSTNAME=@firstname, LASTNAME=@lastname, PASSWORD=@password, PASSWORD_DATE=NOW() WHERE id_usuario=@id_usuario"
                        Dim comm_wms As New MySqlCommand(sql1, conn_wms)
                        comm_wms.Parameters.Add("@cod_user", MySqlDbType.String).Value = Login.Text
                        'comm_wms.Parameters.Add("@cod_user1", MySqlDbType.String).Value = Session("DBAccesosLogin")
                        comm_wms.Parameters.Add("@firstname", MySqlDbType.String).Value = Nombre.Text
                        comm_wms.Parameters.Add("@lastname", MySqlDbType.String).Value = Nombre.Text
                        comm_wms.Parameters.Add("@password", MySqlDbType.String).Value = Password.Text
                        comm_wms.Parameters.Add("@id_usuario", MySqlDbType.Int32).Value = codigo.Value
                        comm_wms.ExecuteNonQuery()

                        items.Clear()
                        items.Add("COD_USER", Login.Text) ' Session("DBAccesosLogin"))
                        items.Add("FIRSTNAME", Nombre.Text)
                        items.Add("LASTNAME", Nombre.Text)
                        'items.Add("PASSWORD", Password.Text)
                        'log_txt = js.Serialize(items)
                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "DEF_USERS", Session("OperatorID"), Session("Login"), "wms", "wms", Session("OperatorIP"))


                        If Login.Text <> bk_login.Value Then

                            sql1 = "UPDATE DEF_USERS_WAREHOUSES SET COD_USER=@cod_user WHERE COD_USER=@cod_user1"
                            Dim comm_wms1 As New MySqlCommand(sql1, conn_wms)
                            comm_wms1.Parameters.Add("@cod_user", MySqlDbType.String).Value = Login.Text
                            comm_wms1.Parameters.Add("@cod_user1", MySqlDbType.String).Value = Session("DBAccesosLogin")
                            comm_wms1.ExecuteNonQuery()

                            items.Clear()
                            items.Add("COD_USER", Login.Text) ' Session("DBAccesosLogin"))
                            log(Server, Session("DBAccesosUserId"), "update master", "", items, "DEF_USERS_WAREHOUSES", Session("OperatorID"), Session("Login"), "wms", "wms", Session("OperatorIP"))

                        End If


                    End If

                    If user_wm1 = 1 Then

                        If Pais.SelectedValue <> bk_pais.Value Then
                            connection.setEmpresaUser("wms", codigo.Value, Pais.SelectedValue, bk_pais.Value, Server)
                            cambios = True
                        End If

                    End If

                    If cambios = False Then
                        user_wm1 = 0
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''B A W '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Pais.SelectedValue <> bk_pais.Value Then
                        cambios = True
                    End If
                    If Dominio.SelectedValue <> bk_dominio.Value Then
                        cambios = True
                    End If
                    If Nivel.SelectedValue <> bk_level.Value Then
                        cambios = True
                    End If
                    If Password.Text <> bk_password.Value Then
                        cambios = True
                    End If
                    If Email.Text <> bk_email.Value Then
                        cambios = True
                    End If
                    If cambios = False Then
                        user_ba1 = 0
                    End If
                    If user_ba1 = 1 Then
                        'conn_baw.Open()
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''M A N I F I E S T O S   C R '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Pais.SelectedValue <> bk_pais.Value Then
                        cambios = True
                    End If
                    If cambios = False Then
                        user_man_cr = 0
                    End If
                    If user_man_cr = 1 Then

                        conn_man_cr.Open()
                        trns_man_cr = conn_man_cr.BeginTransaction()

                        sql1 = "UPDATE manifiestos_usuarios SET nombres=@nombres, usuario=@usuario, idpais=@idpais WHERE id_master=@id_master"
                        Dim comm_man_cr As New NpgsqlCommand(sql1, conn_man_cr)
                        comm_man_cr.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        comm_man_cr.Parameters.Add("@nombres", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                        comm_man_cr.Parameters.Add("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                        comm_man_cr.Parameters.Add("@idpais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Pais.SelectedValue
                        comm_man_cr.ExecuteNonQuery()

                        items.Clear()
                        items.Add("id_master", Session("DBAccesosLogin"))
                        items.Add("nombres", Nombre.Text)
                        items.Add("usuario", Login.Text)
                        items.Add("idpais", Pais.SelectedValue)
                        'log_txt = js.Serialize(items)
                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "ventas_cr.manifiestos_usuarios", Session("OperatorID"), Session("Login"), "manifiestos cr", "ventas_cr", Session("OperatorIP"))

                    End If

                    '''''''''''''''''''''''''''''''''''''''''''M A N I F I E S T O S   C R LTF'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Pais.SelectedValue <> bk_pais.Value Then
                        cambios = True
                    End If
                    If cambios = False Then
                        user_man_crltf = 0
                    End If
                    If user_man_crltf = 1 Then

                        conn_man_crltf.Open()
                        trns_man_crltf = conn_man_crltf.BeginTransaction()

                        sql1 = "UPDATE manifiestos_usuarios SET nombres=@nombres, usuario=@usuario, idpais=@idpais WHERE id_master=@id_master"
                        Dim comm_man_crltf As New NpgsqlCommand(sql1, conn_man_crltf)
                        comm_man_crltf.CommandText = sql1
                        comm_man_crltf.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                        comm_man_crltf.Parameters.Add("@nombres", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Nombre.Text
                        comm_man_crltf.Parameters.Add("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Login.Text
                        comm_man_crltf.Parameters.Add("@idpais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Pais.SelectedValue
                        comm_man_crltf.ExecuteNonQuery()

                        items.Clear()
                        items.Add("id_master", Session("DBAccesosLogin"))
                        items.Add("nombres", Nombre.Text)
                        items.Add("usuario", Login.Text)
                        items.Add("idpais", Pais.SelectedValue)
                        'log_txt = js.Serialize(items)
                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "ventas_crltf.manifiestos_usuarios", Session("OperatorID"), Session("Login"), "manifiestos crltf", "ventas_crltf", Session("OperatorIP"))


                    End If



                    '''''''''''''''''''''''''''''''''''''''''''C A J A   D E   A H O R R O S'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    cambios = False
                    If Login.Text <> bk_login.Value Then
                        cambios = True
                    End If
                    If Nombre.Text <> bk_nombre.Value Then
                        cambios = True
                    End If
                    If Pais.SelectedValue <> bk_pais.Value Then
                        cambios = True
                    End If
                    If Password.Text <> bk_password.Value Then
                        cambios = True
                    End If
                    If Email.Text <> bk_email.Value Then
                        cambios = True
                    End If
                    If cambios = False Then
                        user_caj = 0
                    End If
                    If user_caj = 1 Then

                        conn_caj.Open()
                        trns_caj = conn_caj.BeginTransaction()

                        sql1 = "UPDATE usuarios SET nombres=@nombres, email=@email, username=@username, password=@password, idpais=@idpais WHERE id_master=@id_master"
                        Dim comm_caj As New MySqlCommand(sql1, conn_caj)
                        comm_caj.Parameters.Add("@id_master", MySqlDbType.String).Value = Session("DBAccesosUserId")
                        comm_caj.Parameters.Add("@nombres", MySqlDbType.String).Value = Nombre.Text
                        comm_caj.Parameters.Add("@email", MySqlDbType.String).Value = Email.Text
                        comm_caj.Parameters.Add("@username", MySqlDbType.String).Value = Login.Text
                        comm_caj.Parameters.Add("@password", MySqlDbType.String).Value = Password.Text
                        comm_caj.Parameters.Add("@idpais", MySqlDbType.String).Value = Pais.SelectedValue
                        comm_caj.ExecuteNonQuery()
                        items.Clear()
                        items.Add("id_master", Session("DBAccesosUserId"))
                        items.Add("nombres", Nombre.Text)
                        items.Add("email", Email.Text)
                        items.Add("username", Login.Text)
                        items.Add("password", Password.Text)
                        items.Add("idpais", Pais.SelectedValue)
                        'log_txt = js.Serialize(items)
                        log(Server, Session("DBAccesosUserId"), "update master", "", items, "Operators", Session("OperatorID"), Session("Login"), "caja de ahorros", "caja_regional", Session("OperatorIP"))


                    End If

                Catch ex As Exception

                    msg = ex.Message

                    finaliza = False

                End Try




                '''''''''''''''''''''resultados''''''''''''''''''''''''
                If user_aer = 1 And conn_aer.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_aer.Commit()
                    Else
                        trns_aer.Rollback()
                    End If
                    conn_aer.Close()
                End If

                If user_ter = 1 And conn_ter.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_ter.Commit()
                    Else
                        trns_ter.Rollback()
                    End If
                    conn_ter.Close()
                End If

                If user_tra = 1 And ob > 0 Then
                    For i = 1 To ob
                        If arr_conn_object(i).State = ConnectionState.Open Then
                            If finaliza = True Then
                                arr_trns_object(i).Commit()
                            Else
                                arr_trns_object(i).Rollback()
                            End If
                            arr_conn_object(i).Close()
                        End If
                    Next
                End If

                If user_cus = 1 And conn_cus.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_cus.Commit()
                    Else
                        trns_cus.Rollback()
                    End If
                    conn_cus.Close()
                End If


                'If user_seg = 1 And conn_mas.State = ConnectionState.Open  Then
                'trns_mas.Commit()
                'conn_mas.Close()
                'End If

                If user_wm1 = 1 And conn_wms.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_wms.Commit()
                    Else
                        trns_wms.Rollback()
                    End If
                    conn_wms.Close()
                End If

                If user_ba1 = 1 And conn_baw.State = ConnectionState.Open Then
                    'trns_baw.Commit()
                    'conn_baw.Close()
                End If

                If user_man_cr = 1 And conn_man_cr.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_man_cr.Commit()
                    Else
                        trns_man_cr.Rollback()
                    End If
                    conn_man_cr.Close()
                End If

                If user_man_crltf = 1 And conn_man_crltf.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_man_crltf.Commit()
                    Else
                        trns_man_crltf.Commit()
                    End If
                    conn_man_crltf.Close()
                End If

                If user_caj = 1 And conn_caj.State = ConnectionState.Open Then
                    If finaliza = True Then
                        trns_caj.Commit()
                    Else
                        trns_caj.Rollback()
                    End If
                    conn_caj.Close()
                End If

            End If 'DEL IF INSERT








            If finaliza = True Then

                items.Clear()
                items.Add("id_usuario", Session("DBAccesosUserId"))
                items.Add("login_name", Login.Text)
                items.Add("user_name", Nombre.Text)
                If Password.Text <> bk_password.Value Then
                    If operacion = "update" Then
                        items.Add("ant_password_md5", bk_password.Value)
                    End If
                    If Correo.SelectedValue = 0 Then
                        items.Add("new_password_plain", randomized.Text)
                    End If
                    items.Add("new_password_md5", Password.Text)
                Else
                    items.Add("password", Password.Text)
                End If
                items.Add("pais", Pais.SelectedValue)
                items.Add("correo", Correo.SelectedValue)
                items.Add("email", Email.Text)
                items.Add("nivel", Nivel.SelectedValue)
                items.Add("tipo", tipo_usuario.SelectedValue)
                items.Add("dominio", Dominio.SelectedValue)

                'log(Server, Session("DBAccesosUserId"), operacion, "", items, "usuarios_empresas", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))





                'Dim after_txt As String = Serialize(items)

                Dim after_txt As String
                Dim before_txt As String

                Dim arr_where As New Dictionary(Of String, String)
                Dim arr_data As New Dictionary(Of String, String)
                arr_data.Add("user_id", Session("OperatorID"))
                arr_data.Add("user_name", Session("Login"))
                arr_data.Add("sistema", Session("sistema"))
                arr_data.Add("db", Session("DBAccesos"))
                arr_data.Add("accion", operacion)


                before_txt = "|Login:" & bk_login.Value & "|Nombre:" & bk_nombre.Value & "|Pais:" & bk_pais.Value & "|Dominio:" & bk_dominio.Value & "|Nivel:" & bk_level.Value & "|" & bk_password.Value & "|Email:" & bk_email.Value & "|Correo:" & bk_correo.Value & "|TipoUsuario:" & bk_tipo.Value & "|Locode:" & bk_locode.Value & "|"
                arr_data.Add("before_txt", before_txt)

                after_txt = "|Login:" & Login.Text & "|Nombre:" & Nombre.Text & "|Pais:" & Pais.SelectedValue & "|Dominio:" & Dominio.SelectedValue & "|Nivel:" & Nivel.SelectedValue & "|" & Password.Text & "|Email:" & Email.Text & "|Correo:" & Correo.SelectedValue & "|TipoUsuario:" & tipo_usuario.SelectedValue & "|Locode:" & locode.SelectedValue & "|"
                arr_data.Add("after_txt", after_txt)

                arr_data.Add("ip", Session("OperatorIP"))
                arr_data.Add("tabla", "usuarios_empresas")
                arr_data.Add("tiempo", DateTime.Now.ToLongDateString())



                log(Server, Session("DBAccesosUserId"), operacion, before_txt, arr_data, "usuarios_empresas", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


                'Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                'conn.Open()
                'Dim arr_result As New Dictionary(Of String, String)
                'arr_result = db_oper("insert", "usuarios_empresas_log", "postgres", arr_data, conn_mas, arr_where)
                'conn.Close()
                'End Using











                If operacion = "update" Then
                    msg = "Registro actualizado correctamente"
                Else
                    msg = "Registro creado correctamente"
                End If

                If Login.Text <> Session("DBAccesosLogin") Then
                    Session("DBAccesosLogin") = Login.Text
                End If

            End If


            If conn_mas.State = ConnectionState.Open Then
                If finaliza = True Then
                    trns_mas.Commit()
                Else
                    trns_mas.Rollback()
                End If
                conn_mas.Close()
            End If



        Catch ex As Exception
            msg = ex.Message
            img = icon_err_update
            css = "alert-danger"
            finaliza = False
        End Try


        If finaliza = True Then
            Session("edit") = Nothing
            If operacion = "update" Then
                LeerRegistro()
            Else
                Response.Redirect("mn_Master.aspx")
            End If
        Else
            LeerRegistro()
        End If


    End Sub







    Protected Sub activar(ByVal activo As Boolean)
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "UPDATE usuarios_empresas SET pw_activo=@pw_activo WHERE id_usuario = @codigo"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Session("DBAccesosUserId")
                comm.Parameters.Add("@pw_activo", NpgsqlTypes.NpgsqlDbType.Integer).Value = activo
                conn.Open()
                comm.ExecuteNonQuery()
                Dim items As New Dictionary(Of String, String)
                items.Add("id_usuario", Session("DBAccesosUserId"))
                items.Add("pw_activo", activo)
                Dim operacion As String
                If activo = True Then
                    operacion = "activar"
                    msg = "Registro Activado"
                Else
                    operacion = "desactivar"
                    msg = "Registro Desactivado"
                    Session("edit") = Nothing
                End If
                log(Server, Session("DBAccesosUserId"), operacion, "", items, "usuarios_empresas", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))
            End Using
        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

        'LeerRegistro()
        Response.Redirect("mn_Master.aspx")
    End Sub



    Protected Sub correo_click_Click(sender As Object, e As System.EventArgs) Handles correo_click.Click

        If Correo.SelectedIndex <> -1 Then

            If Correo.SelectedValue = 1 Then
                Password.Text = bk_password.Value
                Password.ReadOnly = False
                gen_pass.Visible = False
                can_pass.Visible = False
            End If

            If Correo.SelectedValue = 0 Then
                Password.Text = ""
                Password.ReadOnly = True
                gen_pass.Visible = True
                can_pass.Visible = True
            End If

        End If

    End Sub

    Protected Sub gen_pass_Click(sender As Object, e As System.EventArgs) Handles gen_pass.Click

        If Correo.SelectedValue = "0" Then
            Password.Text = PwdAleatorio(10, False)
            randomized.Text = Password.Text
            'enc_pass.Visible = True
        End If

    End Sub

    'Protected Sub enc_pass_Click(sender As Object, e As System.EventArgs) Handles enc_pass.Click
    '    Password.Text = Trim(Password.Text)
    '    If Password.Text <> "" Then
    '        Dim HashCode As cMd5Hash
    '        HashCode = New cMd5Hash()
    '        Password.Text = HashCode.Md5FromString(Password.Text)
    '        'enc_pass.Visible = False
    '    End If
    'End Sub

    Protected Sub can_pass_Click(sender As Object, e As System.EventArgs) Handles can_pass.Click
        randomized.Text = ""
        LeerRegistro()
    End Sub

    Protected Sub Pais_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles Pais.SelectedIndexChanged
        fillUbicacion()
    End Sub

    Protected Sub btn_clonar_Click(sender As Object, e As System.EventArgs) Handles btn_clonar.Click

        Dim finaliza As Boolean = True
        Dim object_id As Integer = 0
        Dim object_login As String = ""
        Dim copia As Boolean = False
        Dim operacion As String = "Clonacion (" & Session("demo") & ")"

        If opcion_txt.Text = "" Then
            object_id = 0
            object_login = PwdAleatorio(10, False)
            msg = " crear usuario nuevo " & object_login
        Else
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio, tipo_usuario, level, pw_activo, pw_correo, pw_passwd, locode FROM usuarios_empresas WHERE pw_name = @login"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = opcion_txt.Text
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    object_id = dataread(0)
                    object_login = dataread(1)
                    copia = True
                    msg = " clonar a usuario " & object_login
                End If
                dataread.Close()
            End Using
        End If


        If opcion_txt.Text <> "" And copia = False Then
            msg = "Usuario " & opcion_txt.Text & " no encontrado para generar clone"
            LeerRegistro()
            Exit Sub
        End If


        If MsgBox("Esta seguro de proceder a " & msg, MsgBoxStyle.Question + MsgBoxStyle.YesNo, "CLONAR USUARIO") = MsgBoxResult.No Then
            msg = "Clonacion cancelada"
            LeerRegistro()
            Exit Sub
        End If

        'Dim conn_mas As New NpgsqlConnection(Session("DBAccesos_conn"))
        Dim conn_mas As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
        Dim trns_mas As NpgsqlTransaction = Nothing
        Dim conn_aer As New MySqlConnection(GetConnectionStringFromFile("aereo", Server))
        Dim trns_aer As MySqlTransaction = Nothing
        Dim conn_ter As New MySqlConnection(GetConnectionStringFromFile("terrestre", Server))
        Dim trns_ter As MySqlTransaction = Nothing
        Dim conn_cus As New MySqlConnection(GetConnectionStringFromFile("customer", Server))
        Dim trns_cus As MySqlTransaction = Nothing
        Dim conn_wms As New MySqlConnection(GetConnectionStringFromFile("wms", Server))
        Dim trns_wms As MySqlTransaction = Nothing
        Dim conn_man_cr As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & "cr")
        Dim trns_man_cr As NpgsqlTransaction = Nothing
        Dim conn_man_crltf As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & "crltf")
        Dim trns_man_crltf As NpgsqlTransaction = Nothing
        Dim conn_caj As New MySqlConnection(GetConnectionStringFromFile("caja", Server) & "crltf")
        Dim trns_caj As MySqlTransaction = Nothing

        Dim arr_conn_object() As NpgsqlConnection
        Dim arr_trns_object() As NpgsqlTransaction

        Dim arr_conn_source() As NpgsqlConnection
        Dim arr_trns_source() As NpgsqlTransaction

        Dim ob As Integer = 0
        Dim so As Integer = 0
        Dim i As Integer = 0

        Dim items As New Dictionary(Of String, String)

        Try

            Dim res As Integer = 0

            'MASTER
            conn_mas.Open()
            trns_mas = conn_mas.BeginTransaction()

            If copia = True Then

                sql1 = "UPDATE usuarios_empresas as uno SET pw_gecos=b.pw_gecos, pais=b.pais, pw_passwd=b.pw_passwd, dominio=b.dominio, tipo_usuario=b.tipo_usuario, pw_activo=b.pw_activo, pw_correo=b.pw_correo, locode=b.locode, level=b.level FROM usuarios_empresas as b WHERE uno.id_usuario = @object_id AND b.id_usuario = @source_id"
                Dim comm_mas1 As New NpgsqlCommand(sql1, conn_mas)
                comm_mas1.Parameters.Add("@source_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                res = comm_mas1.ExecuteNonQuery()

            Else

                sql1 = "INSERT INTO usuarios_empresas (pw_name, pw_gecos, pais, pw_passwd, dominio, tipo_usuario, pw_activo, pw_correo, locode, level) "
                sql1 = sql1 & "SELECT @object_login, pw_gecos, pais, pw_passwd, dominio, tipo_usuario, pw_activo, pw_correo, locode, level FROM usuarios_empresas WHERE id_usuario = @codigo"
                Dim comm As New NpgsqlCommand(sql1, conn_mas)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm.Parameters.Add("@object_login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = object_login
                res = comm.ExecuteNonQuery()

                If res > 0 Then
                    sql1 = "SELECT LASTVAL()"
                    comm.CommandText = sql1
                    res = 0
                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        object_id = dataread(0)
                        res = dataread(0)
                    End If
                    dataread.Close()
                End If

            End If

            If res = 0 Then
                msg = "No fue clonado el Usuario " & object_login & " !"
                finaliza = False
                Exit Try
            Else
                items.Clear()
                items.Add("sistema", "master")
                items.Add("id_usuario s", Session("DBAccesosUserId"))
                items.Add("login_name s", Session("DBAccesosLogin"))
                items.Add("id_usuario o", object_id)
                items.Add("login_name o", object_login)
                log(Server, Session("DBAccesosUserId"), operacion, "", items, "usuarios_empresas", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))
            End If





            'MARITIMO
            items.Clear()
            items.Add("sistema", "maritimo")
            Dim str_num As String = ""
            Dim DBAccesos_conn As String = ""

            If copia = True Then 'desactiva permisos anteriores de usuario object
                str_num = ""
                res = 0
                sql1 = "SELECT DISTINCT bd FROM referencias_usuarios WHERE id_nuevo=@object_id AND activo = 't' ORDER BY bd"
                Dim comm_mas3 As New NpgsqlCommand(sql1, conn_mas)
                comm_mas3.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                Dim dataread3 As NpgsqlDataReader = comm_mas3.ExecuteReader
                While dataread3.Read()
                    str_num = str_num & dataread3(0) & ","
                End While
                dataread3.Close()
                Dim arr_num1() As String = str_num.Split(",")
                ReDim arr_conn_object(arr_num1.Length)
                ReDim arr_trns_object(arr_num1.Length)
                For i = 0 To arr_num1.Length - 1
                    DBAccesos_conn = GetConnectionStringFromFile(arr_num1(i), Server)
                    If (DBAccesos_conn <> "") Then
                        ob = ob + 1
                        arr_conn_object(ob) = New NpgsqlConnection(DBAccesos_conn)
                        arr_conn_object(ob).Open()
                        arr_trns_object(ob) = arr_conn_object(ob).BeginTransaction
                        sql1 = "UPDATE users SET activo=@activo WHERE login_name = @object_login and activo = 't'"
                        Dim comm_ven As New NpgsqlCommand(sql1, arr_conn_object(ob))
                        comm_ven.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = False
                        comm_ven.Parameters.Add("@object_login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = object_login
                        comm_ven.ExecuteNonQuery()
                    End If
                Next
                sql1 = "UPDATE referencias_usuarios SET activo=@activo WHERE id_nuevo = @object_id and activo = 't'"
                Dim comm_mas1 As New NpgsqlCommand(sql1, conn_mas)
                comm_mas1.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = False
                comm_mas1.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("permisos anteriores", res)
            End If





            res = 0
            str_num = ""
            sql1 = "SELECT DISTINCT bd FROM referencias_usuarios WHERE id_nuevo=@source_id AND activo = 't' ORDER BY bd"
            Dim comm_mas2 As New NpgsqlCommand(sql1, conn_mas)
            comm_mas2.Parameters.Add("@source_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
            Dim dataread2 As NpgsqlDataReader = comm_mas2.ExecuteReader
            While dataread2.Read()
                str_num = str_num & dataread2(0) & ","
            End While
            dataread2.Close()
            Dim arr_num2() As String = str_num.Split(",")
            ReDim arr_conn_source(arr_num2.Length)
            ReDim arr_trns_source(arr_num2.Length)
            For i = 0 To arr_num2.Length - 1
                DBAccesos_conn = GetConnectionStringFromFile(arr_num2(i), Server)
                If (DBAccesos_conn <> "") Then
                    so = so + 1
                    arr_conn_source(so) = New NpgsqlConnection(DBAccesos_conn)
                    arr_conn_source(so).Open()
                    arr_trns_source(so) = arr_conn_source(so).BeginTransaction
                    sql1 = "INSERT INTO users(login_name, user_name, password, grupo, activo, ext, celular, email, puesto) "
                    sql1 = sql1 & "SELECT @object_login, user_name, password, grupo, activo, ext, celular, email, puesto FROM users WHERE login_name = @login AND activo = 't' "
                    sql1 = sql1 & "returning user_id, login_name"
                    Dim comm_ven As New NpgsqlCommand(sql1, arr_conn_source(so))
                    comm_ven.Parameters.Add("@object_login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = object_login
                    comm_ven.Parameters.Add("@login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = Session("DBAccesosLogin")
                    Dim dataread3 As NpgsqlDataReader = comm_ven.ExecuteReader
                    While dataread3.Read()
                        sql1 = "INSERT INTO referencias_usuarios (id_nuevo, id_anterior, bd, dominio, activo) VALUES (@object_id, @ide, @bd, @dominio, @activo)"
                        Dim comm_mas3 As New NpgsqlCommand(sql1, conn_mas)
                        comm_mas3.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                        comm_mas3.Parameters.Add("@ide", NpgsqlTypes.NpgsqlDbType.Bigint).Value = dataread3(0)
                        comm_mas3.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = arr_num2(i)
                        comm_mas3.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = ""
                        comm_mas3.Parameters.Add("@activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = True
                        res = res + comm_mas3.ExecuteNonQuery()
                    End While
                End If
            Next
            items.Add("permisos nuevos", res)
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "referencias_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



            'SEGUROS
            items.Clear()
            items.Add("sistema", "seguros")
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object
                sql1 = "UPDATE detalle_tipos_usuario as uno SET id_tipo_usuario = b.id_tipo_usuario FROM detalle_tipos_usuario as b WHERE uno.id_usuario = @object_id AND b.id_usuario = @source_id"
                Dim comm_mas1 As New NpgsqlCommand(sql1, conn_mas)
                comm_mas1.Parameters.Add("@source_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO detalle_tipos_usuario (id_usuario, id_tipo_usuario) "
                sql1 = sql1 & "SELECT @object_id, id_tipo_usuario FROM detalle_tipos_usuario WHERE id_usuario = @codigo"
                Dim comm1 As New NpgsqlCommand(sql1, conn_mas)
                comm1.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm1.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                res = comm1.ExecuteNonQuery()
                items.Add("permisos nuevos", res)
            End If
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "detalle_tipos_usuario", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


            'AEREO
            items.Clear()
            items.Add("sistema", "aereo")
            conn_aer.Open()
            trns_aer = conn_aer.BeginTransaction()
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object
                sql1 = "UPDATE Operators a, Operators b SET a.FirstName=b.FirstName, a.LastName=b.LastName, a.Email=b.Email, a.Phone=b.Phone, a.Position=b.Position, a.OperatorLevel=b.OperatorLevel, a.Active=b.Active, a.Countries=b.Countries, a.Sign=b.Sign WHERE a.OperatorID = @object_id AND b.OperatorID = @source_id AND b.Active = 1"
                Dim comm_mas1 As New MySqlCommand(sql1, conn_aer)
                comm_mas1.Parameters.Add("@source_id", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO Operators (OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Active, CreatedDate, CreatedTime, StartTime, FinishTime, Countries, Sign) "
                sql1 = sql1 & "SELECT @object_id, @object_login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Active, CreatedDate, CreatedTime, StartTime, FinishTime, Countries, Sign FROM Operators WHERE OperatorID = @codigo AND Active = 1"
                Dim comm_aer As New MySqlCommand(sql1, conn_aer)
                comm_aer.Parameters.Add("@codigo", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm_aer.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                comm_aer.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
                res = comm_aer.ExecuteNonQuery()
                items.Add("permisos nuevos", res)
            End If
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "Operators", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            'TERRESTRE
            items.Clear()
            items.Add("sistema", "terrestre")
            conn_ter.Open()
            trns_ter = conn_ter.BeginTransaction()
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object                
                sql1 = "UPDATE Operators a, Operators b SET a.FirstName=b.FirstName, a.LastName=b.LastName, a.Email=b.Email, a.Phone=b.Phone, a.Position=b.Position, a.OperatorLevel=b.OperatorLevel, a.Active=b.Active, a.Countries=b.Countries, a.Sign=b.Sign WHERE a.OperatorID = @object_id AND b.OperatorID = @source_id AND b.Active = 1"
                Dim comm_mas1 As New MySqlCommand(sql1, conn_ter)
                comm_mas1.Parameters.Add("@source_id", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO Operators (OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Active, CreatedDate, CreatedTime, StartTime, FinishTime, Countries, Sign) "
                sql1 = sql1 & "SELECT @object_id, @object_login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Active, CreatedDate, CreatedTime, StartTime, FinishTime, Countries, Sign FROM Operators WHERE OperatorID = @codigo AND Active = 1"
                Dim comm_ter As New MySqlCommand(sql1, conn_ter)
                comm_ter.Parameters.Add("@codigo", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm_ter.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                comm_ter.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
                res = comm_ter.ExecuteNonQuery()
                items.Add("permisos nuevos", res)
            End If
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "Operators", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            'CUSTOMER
            items.Clear()
            items.Add("sistema", "customer")
            conn_cus.Open()
            trns_cus = conn_cus.BeginTransaction()
            res = 0
            If copia = True Then 'desactiva permisos anteriores de usuario objeto
                sql1 = "UPDATE usuarios SET borrado=1 WHERE id_usuario_empresa = @object_id AND borrado = 0"
                Dim comm_mas1 As New MySqlCommand(sql1, conn_cus)
                comm_mas1.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("permisos anteriores", res)
            End If

            sql1 = "INSERT INTO usuarios (id_usuario_empresa, id_empresa, id_pais, nombre, nombrefull, puesto, clave, psw, nivel, area, sucursal, permisos, activo, mac, status, printpreview, version, direccion_ip, modificado, borrado, acceso_aduana, activation_code, nivel_dua, email, nivel_bit_apl, locode, acceso_apl, dt_readonly, fecha_ingreso, user_input, fecha_desactiva, user_desactiva, user_modifica, acceso_maritimo) "
            sql1 = sql1 & "SELECT @object_id, id_empresa, id_pais, @object_login, nombrefull, puesto, clave, psw, nivel, area, sucursal, permisos, activo, mac, status, printpreview, version, direccion_ip, modificado, borrado, acceso_aduana, activation_code, nivel_dua, email, nivel_bit_apl, locode, acceso_apl, dt_readonly, fecha_ingreso, user_input, fecha_desactiva, user_desactiva, user_modifica, acceso_maritimo FROM usuarios WHERE id_usuario_empresa = @codigo AND activo = '0' AND borrado = '0'"
            Dim comm_cus As New MySqlCommand(sql1, conn_cus)
            comm_cus.Parameters.Add("@codigo", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
            comm_cus.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
            comm_cus.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
            res = comm_cus.ExecuteNonQuery()
            items.Add("permisos nuevos", res)
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


            'WMS
            items.Clear()
            items.Add("sistema", "wms")
            conn_wms.Open()
            trns_wms = conn_wms.BeginTransaction()
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object
                sql1 = "UPDATE DEF_USERS a, DEF_USERS b SET a.FIRSTNAME=b.FIRSTNAME, a.LASTNAME=b.LASTNAME, a.COD_GROUP=b.COD_GROUP, a.PASSWORD=b.PASSWORD, a.STATUS=b.STATUS, a.PASSWORD_EXPIRES=b.PASSWORD_EXPIRES, a.CHANGE_PASSWORD=b.CHANGE_PASSWORD, a.PASSWORD_DATE=b.PASSWORD_DATE, a.ID_CARD=b.ID_CARD, a.BLOOD_TYPE=b.BLOOD_TYPE, a.COMMENTS=b.COMMENTS, a.USER_TYPE=b.USER_TYPE, a.EXTERNAL_USER=b.EXTERNAL_USER WHERE a.COD_USER = @object_login AND b.COD_USER = @source_login AND b.STATUS = 1"
                Dim comm_mas1 As New MySqlCommand(sql1, conn_wms)
                comm_mas1.Parameters.Add("@source_login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                comm_mas1.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos H", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO DEF_USERS (COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, PASSWORD_EXPIRES, CHANGE_PASSWORD, PASSWORD_DATE, ID_CARD, BLOOD_TYPE, COMMENTS, USER_TYPE, EXTERNAL_USER) "
                sql1 = sql1 & "SELECT @object_login, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, PASSWORD_EXPIRES, CHANGE_PASSWORD, PASSWORD_DATE, ID_CARD, BLOOD_TYPE, COMMENTS, USER_TYPE, EXTERNAL_USER FROM DEF_USERS WHERE COD_USER = @login AND STATUS = @status"
                Dim comm_wms As New MySqlCommand(sql1, conn_wms)
                comm_wms.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
                comm_wms.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
                comm_wms.Parameters.Add("@status", MySqlDbType.String).Value = "1"
                res = comm_wms.ExecuteNonQuery()
                items.Add("permisos nuevos H", res)
            End If

            res = 0
            If copia = True Then 'desactiva permisos anteriores de usuario objet
                sql1 = "UPDATE DEF_USERS_WAREHOUSES SET COD_USER = '', COD_WAREHOUSE='' WHERE COD_USER = @object_login"
                Dim comm_mas1 As New MySqlCommand(sql1, conn_wms)
                comm_mas1.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
                res = comm_mas1.ExecuteNonQuery()
                items.Add("permisos anteriores D", res)
            End If

            sql1 = "INSERT INTO DEF_USERS_WAREHOUSES (COD_USER, COD_WAREHOUSE) SELECT @object_login, COD_WAREHOUSE FROM DEF_USERS_WAREHOUSES WHERE COD_USER = @login"
            Dim comm_wms1 As New MySqlCommand(sql1, conn_wms)
            comm_wms1.Parameters.Add("@login", MySqlDbType.String).Value = Session("DBAccesosLogin")
            comm_wms1.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
            res = comm_wms1.ExecuteNonQuery()
            items.Add("permisos nuevos D", res)
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "DEF_USERS / DEF_USERS_WAREHOUSES", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


            'BAW
            'Dim conn_baw As New MySqlConnection(GetConnectionStringFromFile("baw", Server))
            'Dim trns_baw As MySqlTransaction

            'MANIFIESTOS CR
            items.Clear()
            items.Add("sistema", "manifiestos cr")
            conn_man_cr.Open()
            trns_man_cr = conn_man_cr.BeginTransaction()
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object
                sql1 = "UPDATE manifiestos_usuarios as uno SET nombres=b.nombres, idpais=b.idpais, activo=b.activo, aereo=b.aereo, maritimo=b.maritimo, terrestre=b.terrestre, aduanas=b.aduanas FROM manifiestos_usuarios as b WHERE uno.id_master = @object_id AND b.id_master = @source_id AND b.activo = 't'"
                Dim comm_mas1 As New NpgsqlCommand(sql1, conn_man_cr)
                comm_mas1.Parameters.Add("@source_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO manifiestos_usuarios (id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas) "
                sql1 = sql1 & "SELECT @object_id, nombres, @object_login, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @codigo AND activo = 't'"
                Dim comm_man_cr As New NpgsqlCommand(sql1, conn_man_cr)
                comm_man_cr.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm_man_cr.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                comm_man_cr.Parameters.Add("@object_login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = object_login
                res = comm_man_cr.ExecuteNonQuery()
                items.Add("permisos nuevos", res)
            End If
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            'MANIFIESTOS CRLTF
            items.Clear()
            items.Add("sistema", "manifiestos crltf")
            conn_man_crltf.Open()
            trns_man_crltf = conn_man_crltf.BeginTransaction()
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object
                sql1 = "UPDATE manifiestos_usuarios as uno SET nombres=b.nombres, idpais=b.idpais, activo=b.activo, aereo=b.aereo, maritimo=b.maritimo, terrestre=b.terrestre, aduanas=b.aduanas FROM manifiestos_usuarios as b WHERE uno.id_master = @object_id AND b.id_master = @source_id AND b.activo = 't'"
                Dim comm_mas1 As New NpgsqlCommand(sql1, conn_man_crltf)
                comm_mas1.Parameters.Add("@source_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO manifiestos_usuarios (id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas) "
                sql1 = sql1 & "SELECT @object_id, nombres, @object_login, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @codigo AND activo = 't'"
                Dim comm_man_crltf As New NpgsqlCommand(sql1, conn_man_crltf)
                comm_man_crltf.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Session("DBAccesosUserId")
                comm_man_crltf.Parameters.Add("@object_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = object_id
                comm_man_crltf.Parameters.Add("@object_login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = object_login
                res = comm_man_crltf.ExecuteNonQuery()
                items.Add("permisos nuevos", res)
            End If
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "manifiestos_usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            'CAJA AHORRO
            items.Clear()
            items.Add("sistema", "caja")
            conn_caj.Open()
            trns_caj = conn_caj.BeginTransaction()
            res = 0
            If copia = True Then 'copia permisos de usuario source a usuario object
                sql1 = "UPDATE usuarios a, usuarios b SET a.nombres=b.nombres, a.email=b.email, a.password=b.password, a.fecha_ingreso=b.fecha_ingreso, a.activo=b.activo, a.idpais=b.idpais, a.rol_id=b.rol_id, a.gt=b.gt, a.bz=b.bz, a.sv=b.sv, a.hn=b.hn, a.ni=b.ni, a.cr=b.cr, a.pa=b.pa WHERE a.id_master = @object_id AND b.id_master = @source_id AND b.activo = 1"
                Dim comm_mas1 As New MySqlCommand(sql1, conn_caj)
                comm_mas1.Parameters.Add("@source_id", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm_mas1.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                res = comm_mas1.ExecuteNonQuery()
                items.Add("copia permisos", res)
            End If
            If res = 0 Then
                sql1 = "INSERT INTO usuarios (nombres, email, username, password, fecha_ingreso, activo, idpais, rol_id, gt, bz, sv, hn, ni, cr, pa, id_master) "
                sql1 = sql1 & "SELECT nombres, email, @object_login, password, fecha_ingreso, activo, idpais, rol_id, gt, bz, sv, hn, ni, cr, pa, @object_id FROM usuarios WHERE id_master = @source_id AND activo = 1"
                Dim comm_caj As New MySqlCommand(sql1, conn_caj)
                comm_caj.Parameters.Add("@source_id", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm_caj.Parameters.Add("@object_id", MySqlDbType.Int32).Value = object_id
                comm_caj.Parameters.Add("@object_login", MySqlDbType.String).Value = object_login
                res = comm_caj.ExecuteNonQuery()
                items.Add("permisos nuevos", res)
            End If
            log(Server, Session("DBAccesosUserId"), operacion, "", items, "usuarios", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))




            msg = "Usuario " & object_id & " " & object_login
            If copia = True Then
                msg = msg & ""
            Else
                msg = msg & " (nuevo) "
            End If
            msg = msg & " fue CLONADO correctamente "

        Catch ex As Exception

            msg = ex.Message

            finaliza = False

        End Try



        ''''''''''''''''''''''resultados''''''''''''''''''''''''
        For i = 1 To ob
            If arr_conn_object(i).State = ConnectionState.Open Then
                If finaliza = True Then
                    arr_trns_object(i).Commit()
                Else
                    arr_trns_object(i).Rollback()
                End If
                arr_conn_object(i).Close()
            End If
        Next



        For i = 1 To so
            If arr_conn_source(i).State = ConnectionState.Open Then
                If finaliza = True Then
                    arr_trns_source(i).Commit()
                Else
                    arr_trns_source(i).Rollback()
                End If
                arr_conn_source(i).Close()
            End If
        Next


        If conn_aer.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_aer.Commit()
            Else
                trns_aer.Rollback()
            End If
            conn_aer.Close()
        End If

        If conn_ter.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_ter.Commit()
            Else
                trns_ter.Rollback()
            End If
            conn_ter.Close()
        End If

        If conn_cus.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_cus.Commit()
            Else
                trns_cus.Rollback()
            End If
            conn_cus.Close()
        End If

        If conn_wms.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_wms.Commit()
            Else
                trns_wms.Rollback()
            End If
            conn_wms.Close()
        End If

        'If conn_bawr.State = ConnectionState.Open Then
        'If finaliza = True Then
        'trns_baw.Commit()
        'Else
        'trns_baw.Rollback()
        'End If
        'conn_baw.Close()
        'End If

        If conn_man_cr.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_man_cr.Commit()
            Else
                trns_man_cr.Rollback()
            End If
            conn_man_cr.Close()
        End If

        If conn_man_crltf.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_man_crltf.Commit()
            Else
                trns_man_crltf.Rollback()
            End If
            conn_man_crltf.Close()
        End If

        If conn_caj.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_caj.Commit()
            Else
                trns_caj.Rollback()
            End If
            conn_caj.Close()
        End If

        If conn_mas.State = ConnectionState.Open Then
            If finaliza = True Then
                trns_mas.Commit()
            Else
                trns_mas.Rollback()
            End If
            conn_mas.Close()
        End If

        LeerRegistro()

    End Sub

End Class

