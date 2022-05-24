Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports connection
Imports logs

Partial Class mn_CustomerAduana
    Inherits System.Web.UI.Page
    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"
    Public active As Boolean

    Public Licon_home As String = icon_home
    Public Licon_insert As String = icon_insert
    Public Licon_on As String = icon_on
    Public Licon_update As String = icon_update
    Public Licon_off As String = icon_off

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'acc_aduana.Attributes.Add("onclick", "return AduanaClick()") 'ejecuta un click 
        'acc_Apl.Attributes.Add("onclick", "return AplClick()") 'ejecuta un click
        'acc_Maritimo.Attributes.Add("onclick", "return MaritimoClick()") 'ejecuta un click
        'Activo.Attributes.Add("onclick", "return ActivoClick()") 'ejecuta un click


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
            msg = ""
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

            Empresa.Items.Clear()            

            niveldua.SelectedIndex = -1
            nivelbitapl.SelectedIndex = -1

            codigo.Value = Nothing


            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario, pw_name, pw_gecos, pais, dominio, pw_passwd, locode FROM usuarios_empresas WHERE id_usuario = '" & Session("DBAccesosUserId") & "'"
                Dim comm As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    Login.Text = dataread(1)
                    Nombre.Text = dataread(2)
                    Pais.Text = dataread(3)
                    Email.Text = dataread(4)
                    Password.Text = dataread(5)
                    Locode.Text = dataread(3) & dataread(6)
                End If
            End Using




            Using conn_cus As New MySqlConnection(GetConnectionStringFromFile("customer", Server))



                sql1 = "SELECT id, nombre_empresa FROM empresas_internas WHERE activo = 0 and pais = @pais ORDER BY id"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(sql1, conn_cus)
                cmd.Parameters.Add("@pais", MySqlDbType.String).Value = Pais.Text
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                Empresa.DataSource = ds
                Empresa.DataTextField = "nombre_empresa"
                Empresa.DataValueField = "id"
                Empresa.DataBind()


                conn_cus.Open()


                sql1 = "SELECT numero, id_usuario_empresa, id_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, locode, " & _
                "acceso_aduana, acceso_apl, permisos, fecha_ingreso, fecha_desactiva, modificado, activo, nivel_dua, nivel_bit_apl, psw, acceso_maritimo, locode " & _
                "FROM usuarios WHERE id_usuario_empresa = @id_usuario_empresa AND borrado = 0 ORDER BY numero"
                Dim comm As New MySqlCommand(sql1, conn_cus)
                comm.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = Session("DBAccesosUserId")

                Dim dataread As MySqlDataReader = comm.ExecuteReader
                While dataread.Read()

                    Session("insert") = False

                    codigo.Value = dataread(0)
                    Pais.Text = dataread(3)
                    Login.Text = dataread(4)
                    Nombre.Text = dataread(5)
                    Puesto.Text = dataread(6)
                    IP.Text = dataread(7)
                    Email.Text = dataread(8)
                    Password.Text = dataread(19)
                    Locode.Text = dataread(21)

                    niveldua.SelectedValue = dataread(17)
                    nivelbitapl.SelectedValue = dataread(18)


                    'If dataread(11) = 0 And dataread(20) = 0 And dataread(16) = True Then  'apl - maritimo - aduana  
                    'esta desabilitado
                    'Dim a As String = "test"

                    'Else

                    accMaritimo.Checked = False
                    accApl.Checked = dataread(11)

                    If dataread(20) = 5 Then '2014-12-02 este campo acceso_maritimo q ahora guarda el valor entero 0 3 5 
                        Opciones.Items(0).Selected = True
                        accMaritimo.Checked = True
                    End If

                    If dataread(20) = 3 Then '2014-12-02
                        Opciones.Items(1).Selected = True
                        accMaritimo.Checked = True
                    End If

                    Activo.Checked = Not dataread(16) '1 inactivo  0 activo

                    For Each li As ListItem In Empresa.Items
                        If li.Value = dataread(2) And dataread(10) = 1 Then
                            li.Selected = True
                            Exit For
                        End If
                    Next

                    'End If



                End While

                dataread.Close()


            End Using








            Select Case Session("pestana")
                Case "aduana"
                    active = Activo.Checked
                Case "apl"
                    active = accApl.Checked
                Case "maritimo_c"
                    active = accMaritimo.Checked
            End Select


        Catch ex As Exception
            msg = ex.Message
            img = icon_err_read
            css = "alert-warning"
        End Try

    End Sub





    Private Structure CustomerDat
        Public numero As Integer
        Public id_empresa As Integer
        Public acceso_aduana As Integer
        Public stat As String
    End Structure



    Protected Function graba(ByVal operacion As String) As String

        Dim ArrCustomer As New List(Of CustomerDat)
        ArrCustomer.Clear()
        Dim tmp As New CustomerDat

        Using conn_cus As New MySqlConnection(GetConnectionStringFromFile("customer", Server))

            conn_cus.Open()

            sql1 = "SELECT numero, id_usuario_empresa, id_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, locode, " & _
            "acceso_aduana, acceso_apl, permisos, fecha_ingreso, fecha_desactiva, modificado, activo, nivel_dua, nivel_bit_apl, psw, acceso_maritimo, locode " & _
            "FROM usuarios WHERE id_usuario_empresa = @id_usuario_empresa AND borrado = 0 ORDER BY numero"            
            Dim comm As New MySqlCommand(sql1, conn_cus)
            comm.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = Session("DBAccesosUserId")

            Dim dataread As MySqlDataReader = comm.ExecuteReader
            While dataread.Read()
                tmp.numero = dataread(0)
                tmp.id_empresa = dataread(2)
                tmp.acceso_aduana = dataread(10)
                tmp.stat = "A"
                ArrCustomer.Add(tmp)
            End While
            dataread.Close()

            If tmp.stat <> "A" Then

                'sql1 = "INSERT INTO usuarios (numero, id_usuario_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, psw, " & _
                '" fecha_ingreso, activo, nivel, user_input, dt_readonly, locode) " & _
                '" VALUES (null, @id_usuario_empresa, @id_empresa, @id_pais, @nombre, @nombrefull, @puesto, @direccion_ip, @email, @psw, " & _
                '" NOW(), 0, 6, @user_input, 0, @locode)"

                'Dim comm_cus As New MySqlCommand(sql1, conn_cus)                
                'comm_cus.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = Session("DBAccesosUserId")                
                'comm_cus.Parameters.Add("@id_pais", MySqlDbType.String).Value = Pais.Text
                'comm_cus.Parameters.Add("@nombre", MySqlDbType.String).Value = Login.Text
                'comm_cus.Parameters.Add("@nombrefull", MySqlDbType.String).Value = Nombre.Text
                'comm_cus.Parameters.Add("@puesto", MySqlDbType.String).Value = Puesto.Text
                'comm_cus.Parameters.Add("@direccion_ip", MySqlDbType.String).Value = IP.Text
                'comm_cus.Parameters.Add("@email", MySqlDbType.String).Value = Email.Text
                'comm_cus.Parameters.Add("@psw", MySqlDbType.String).Value = Password.Text
                'comm_cus.Parameters.Add("@user_input", MySqlDbType.Int16).Value = Session("OperatorID")
                'comm_cus.Parameters.Add("@locode", MySqlDbType.String).Value = Locode.Text
                'comm_cus.Parameters.Add("@user_desactiva", MySqlDbType.Int32).Value = Session("OperatorID")


                'sql1 = "INSERT INTO usuarios (numero, id_usuario_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, psw, " & _
                '" fecha_ingreso, activo, nivel, user_input, dt_readonly, locode) " & _
                '" VALUES (numero, '" & Session("DBAccesosUserId") & "', '" & Pais.Text & "', '" & Login.Text & "', '" & Nombre.Text & "', '" & Puesto.Text & "', '" & IP.Text & "', '" & Email.Text & "', '" & Password.Text & "', NOW(), 1, 6, '" & Session("OperatorID") & "', 0, '" & Locode.Text & "')"

                'Dim comm_cus As New MySqlCommand(sql1, conn_cus)
                'comm_cus.ExecuteNonQuery()


       

                Dim arr_where As New Dictionary(Of String, String)
                Dim arr_data As New Dictionary(Of String, String)
                arr_data.Add("numero", "null")
                arr_data.Add("id_usuario_empresa", Session("DBAccesosUserId"))
                arr_data.Add("id_pais", Pais.Text)
                arr_data.Add("nombre", Login.Text)
                arr_data.Add("nombrefull", Nombre.Text)
                arr_data.Add("puesto", Puesto.Text)
                arr_data.Add("direccion_ip", IP.Text)
                arr_data.Add("email",Email.Text) 
                arr_data.Add("psw", Password.Text)
                arr_data.Add("fecha_ingreso", "now()")
                arr_data.Add("activo", 1)
                arr_data.Add("nivel", 6)
                arr_data.Add("user_input", Session("OperatorID"))
                arr_data.Add("dt_readonly", 0)
                arr_data.Add("locode", Locode.Text)

                Dim arr_result As New Dictionary(Of String, String)
                arr_result = db_oper("insert", "usuarios", "mysql", arr_data, conn_cus, arr_where)


                Select Case Session("pestana")
                    Case "aduana"

                    Case "apl"

                    Case "maritimo_c"

                End Select

                Return ""

                Exit Function

            End If

            Dim ArrCustomer2 As New List(Of CustomerDat)
            ArrCustomer2.Clear()

            Dim founded As Boolean = False

            For Each perfil_new As ListItem In Empresa.Items

                If perfil_new.Selected = True Then

                    founded = False
                    For Each Perfil_ant As CustomerDat In ArrCustomer
                        If Perfil_ant.id_empresa = perfil_new.Value Then
                            founded = True
                            tmp.numero = Perfil_ant.numero
                            tmp.id_empresa = Perfil_ant.id_empresa
                            tmp.stat = "U"
                            tmp.acceso_aduana = 1
                            ArrCustomer2.Add(tmp)
                            Exit For
                        End If
                    Next

                    If founded = False Then
                        tmp.numero = 0
                        tmp.id_empresa = perfil_new.Value
                        tmp.stat = "N"
                        tmp.acceso_aduana = 1
                        ArrCustomer2.Add(tmp)
                    End If
                End If
            Next

            For Each Perfil_ant As CustomerDat In ArrCustomer
                founded = False
                For Each Perfil_new As CustomerDat In ArrCustomer2
                    If Perfil_ant.id_empresa = Perfil_new.id_empresa Then
                        founded = True
                        Exit For
                    End If
                Next
                If founded = False Then
                    tmp.numero = Perfil_ant.numero
                    tmp.id_empresa = Perfil_ant.id_empresa
                    tmp.stat = "A"
                    tmp.acceso_aduana = 0
                    ArrCustomer2.Add(tmp)
                End If
            Next



            Dim acceso_aduana_str As String = ""
            Dim numero_str As String = ""
            Dim id_empresa_str As String = ""

            For Each Aduanas As CustomerDat In ArrCustomer2 'recorre el arreglo nuevo completo con lo anterior y lon nuevo



                Dim arr_where As New Dictionary(Of String, String)
                Dim arr_data As New Dictionary(Of String, String)

                arr_data.Add("id_usuario_empresa", Session("DBAccesosUserId"))
                arr_data.Add("id_empresa", Aduanas.id_empresa)
                arr_data.Add("id_pais", Pais.Text)
                arr_data.Add("nombre", Login.Text)
                arr_data.Add("nombrefull", Nombre.Text)
                arr_data.Add("puesto", Puesto.Text)
                arr_data.Add("direccion_ip", IP.Text)
                arr_data.Add("email", Email.Text)
                arr_data.Add("psw", Password.Text)
                arr_data.Add("fecha_ingreso", "now()")
                arr_data.Add("activo", 0)
                arr_data.Add("nivel", 6)
                arr_data.Add("user_input", Session("OperatorID"))

                arr_data.Add("acceso_aduana", Aduanas.acceso_aduana)
                arr_data.Add("nivel_dua", niveldua.SelectedValue)

                If accApl.Checked Then
                    arr_data.Add("acceso_apl", 1)
                Else                    
                    arr_data.Add("acceso_apl", 0)
                End If
                arr_data.Add("nivel_bit_apl", nivelbitapl.SelectedValue)

                arr_data.Add("acceso_maritimo", Opciones.SelectedValue)

                arr_data.Add("dt_readonly", 0)
                arr_data.Add("locode", Locode.Text)

                Dim arr_result As New Dictionary(Of String, String)

                If Aduanas.stat = "N" Then
                    arr_data.Add("numero", "null")
                    arr_result = db_oper("insert", "usuarios", "mysql", arr_data, conn_cus, arr_where)
                End If

                If Aduanas.stat = "U" Or Aduanas.stat = "A" Then
                    arr_where.Add("numero", Aduanas.numero)
                    arr_result = db_oper("update", "usuarios", "mysql", arr_data, conn_cus, arr_where)
                End If

                acceso_aduana_str = acceso_aduana_str & Aduanas.acceso_aduana & ","
                numero_str = numero_str & Aduanas.id_empresa & ","
                id_empresa_str = id_empresa_str & Aduanas.id_empresa & ","

            Next

            Dim items As New Dictionary(Of String, String)
            items.Add("id_usuario_empresa", Session("DBAccesosUserId"))
            items.Add("id_pais", Pais.Text)
            items.Add("nombre", Login.Text)
            items.Add("nombrefull", Nombre.Text)
            items.Add("puesto", Puesto.Text)
            items.Add("direccion_ip", IP.Text)
            items.Add("email", Email.Text)
            items.Add("psw", Password.Text)
            items.Add("locode", Locode.Text)

            items.Add("nivel_dua", niveldua.SelectedValue)
            items.Add("acceso_apl", accApl.Checked)
            items.Add("nivel_bit_apl", nivelbitapl.SelectedValue)

            items.Add("acceso_aduana", acceso_aduana_str)
            items.Add("numero", numero_str)
            items.Add("id_empresa", id_empresa_str)

            log(Server, Session("DBAccesosUserId"), operacion, "", items, "Operators", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))



        End Using

        '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        Select Case Session("pestana")
            Case "aduana"
                active = Activo.Checked
            Case "apl"
                active = accApl.Checked
            Case "maritimo_c"
                active = accMaritimo.Checked
        End Select


        Return ""

    End Function

    Protected Sub btn_agregar_Click(sender As Object, e As System.EventArgs) Handles btn_agregar.Click
        'save_click("insert")
        graba("insert")
    End Sub

    Protected Sub btn_actualizar_Click(sender As Object, e As System.EventArgs) Handles btn_actualizar.Click
        'save_click("update")
        graba("update")
    End Sub

    Protected Sub btn_activar_Click(sender As Object, e As System.EventArgs) Handles btn_activar.Click
        activar("activar") 'aqui es alreves la cosa del false y true
        'save_click("update")
    End Sub

    Protected Sub btn_eliminar_Click(sender As Object, e As System.EventArgs) Handles btn_eliminar.Click
        activar("desactivar")
    End Sub


    Protected Sub activar(ByVal operacion As String)
        Try

            Using conn As New MySqlConnection(Session("DBAccesos_conn"))

                Dim items As New Dictionary(Of String, String)
                items.Add("id_usuario_empresa", Session("DBAccesosUserId"))

                If operacion = "desactivar" Then

                    Select Case Session("pestana")
                        Case "aduana"
                            sql1 = "UPDATE usuarios SET activo=1, fecha_desactiva=NOW(), user_desactiva=@user_desactiva WHERE id_usuario_empresa = @codigo"
                        Case "apl"
                            sql1 = "UPDATE usuarios SET acceso_apl=0, fecha_desactiva=NOW(), user_desactiva=@user_desactiva WHERE id_usuario_empresa = @codigo"
                        Case "maritimo_c"
                            sql1 = "UPDATE usuarios SET acceso_maritimo=0, fecha_desactiva=NOW(), user_desactiva=@user_desactiva WHERE id_usuario_empresa = @codigo"
                    End Select

                    msg = "Registro se desactivo correctamente"
                    'items.Add("activo", 1)
                Else

                    Select Case Session("pestana")
                        Case "aduana"
                            sql1 = "UPDATE usuarios SET activo=0, user_modifica=@user_modifica WHERE id_usuario_empresa = @codigo and activo=1"
                        Case "apl"
                            sql1 = "UPDATE usuarios SET acceso_apl=1, user_modifica=@user_modifica WHERE id_usuario_empresa = @codigo and acceso_apl=0"
                        Case "maritimo_c"

                            If Opciones.SelectedIndex = -1 Then
                                Opciones.SelectedValue = 3
                            End If

                            sql1 = "UPDATE usuarios SET acceso_maritimo=@acceso_maritimo, user_modifica=@user_modifica WHERE id_usuario_empresa = @codigo"
                    End Select

                    msg = "Registro se activo correctamente"
                    'items.Add("activo", 0)
                End If

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.Int32).Value = Session("DBAccesosUserId")
                comm.Parameters.Add("@user_modifica", MySqlDbType.Int32).Value = Session("OperatorID")
                comm.Parameters.Add("@user_desactiva", MySqlDbType.Int32).Value = Session("OperatorID")
                comm.Parameters.Add("@acceso_maritimo", MySqlDbType.Int32).Value = Opciones.SelectedValue
                conn.Open()
                comm.ExecuteNonQuery()

                log(Server, Session("DBAccesosUserId"), operacion, "", items, "users", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))

            End Using

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
