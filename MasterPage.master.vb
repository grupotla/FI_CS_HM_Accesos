Imports MySql.Data.MySqlClient
Imports Npgsql
Imports connection

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"
    Public htm As String = ""

    Public Licon_edit As String = icon_edit
    Public Licon_logout As String = icon_logout
    Public Licon_keys As String = icon_keys
    Public Licon_opciones As String = icon_opciones
    Public Licon_home As String = icon_home
    Public Licon_solicitudes As String = icon_solicitudes
    'Public Lmaster_aimar_demo As String = master_aimar_demo

    Public nav As String = "navbar-inverse"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If GetConnectionStringFromFile("aimar", Server) = Nothing Then
            Response.Redirect("Login.aspx")
        End If


        If Session("DBAccesosUserId") = Nothing Then
            nav = "navbar-inverse"
        Else
            nav = "navbar-inverse" 'default"
        End If

        htm = ""

        If Not IsPostBack Then

            Try

                If Session("OperatorID") <> Nothing Then

                    If user_activo(Session("DBAccesosUserId"), Server) = 1 Then

                        Dim activo_arr As New Dictionary(Of String, String)

                        activo_arr.Add("usuario", 1) 'por logica es uno
                        activo_arr.Add("catalogo", user_catalogo(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("exactus", 0)
                        activo_arr.Add("aereo", user_aereo(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("terrestre", user_terrestre(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("maritimo", user_trafico_maritimo(Session("DBAccesosUserId"), Server))

                        Dim cus() As String
                        cus = user_customer(Session("DBAccesosUserId"), Server).ToString.Split(",")
                        activo_arr.Add("aduana", cus(1))
                        activo_arr.Add("apl", cus(2))
                        activo_arr.Add("maritimo_c", cus(3))

                        activo_arr.Add("seguros", user_seguros(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("wms", user_wms(Session("DBAccesosLogin"), Server))
                        activo_arr.Add("baw", user_baw(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("ventas_cr", user_manifiestos_cr(Session("DBAccesosUserId"), Server))
                        'activo_arr.Add("cr_demo", user_manifiestos_cr_demo(Session("DBAccesosUserId")))
                        activo_arr.Add("ventas_crltf", user_manifiestos_crltf(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("manifiestoweb", user_manifiestos_gtweb(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("caja", user_caja(Session("DBAccesosUserId"), Server))
                        activo_arr.Add("pricing", 0)

                        htm = "<ul class='nav navbar-nav' id='menu_modulos'>"
                        htm = htm & menu_gen("", "", True, Session("OperatorID"), Server) 'GENERA EL MENU
                        htm = htm & "</ul>"

                        For Each item As KeyValuePair(Of String, String) In activo_arr
                            If item.Value = 1 Then
                                htm = htm.Replace("#*icono_" & item.Key & "*#", icon_check)
                            Else
                                htm = htm.Replace("#*icono_" & item.Key & "*#", icon_uncheck)
                            End If
                        Next

                    End If

                End If

                menu_db.Text = htm

            Catch ex As Exception
                msg = ex.Message
                img = icon_err_read
                css = "alert-warning"
            End Try

        End If


        'BulletedList1.Items.Clear()
        'If Session("OperatorID") <> Nothing Then
        '    If Session("DBAccesosUserId") <> Nothing Then
        '        BulletedList1.Items.Add("<img src='Content/icon/.png' height='16' />")
        '        BulletedList1.Items.Add(Session("DBAccesosUserId"))
        '        BulletedList1.Items.Add("<span class='badge alert-primary table-bordered'>" & Session("DBAccesosLogin") & "</span>")
        '        BulletedList1.Items.Add(Session("DBAccesosUserId"))
        '        If Session("sistema") <> Session("pestana") Then
        '            BulletedList1.Items.Add(Session("sistema"))
        '        Else
        '            BulletedList1.Items.Add(Session("icono"))
        '        End If
        '    Else
        '        BulletedList1.Items.Add("<img src='Content/icon/.png' height='16' />&nbsp;Modulos")
        '    End If
        'Else
        '    BulletedList1.Items.Add("<img src='Content/icon/.png' height='16' />&nbsp;Login")
        'End If


    End Sub


    Protected Sub lnk_bread_hom_Click(sender As Object, e As System.EventArgs) Handles lnk_bread_hom.Click
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("edit") = Nothing
        Session("insert") = Nothing
        Session("pestana") = Nothing
        Session("icono") = Nothing
        Response.Redirect("Default.aspx")
    End Sub


    Protected Sub opcion_btn_Click(sender As Object, e As System.EventArgs) Handles opcion_btn.Click

        If opcion_txt.Text <> "" Then

            Select Case opcion_txt.Text
                Case "a_home"
                    'If (Session("edit") = "edit" And Session("DBAccesosUserId") = Nothing) Or (Session("edit") = Nothing And Session("DBAccesosUserId") <> Nothing) Then
                    Session("edit") = Nothing
                    Session("sistema") = Nothing
                    Session("DBAccesos") = Nothing
                    Session("DBAccesos_conn") = Nothing
                    Session("DBAccesosUserId") = Nothing
                    Session("DBAccesosLogin") = Nothing
                    Response.Redirect("Default.aspx")
                    'Else
                    'Session("edit") = Nothing
                    'Response.Redirect("mn_Master.aspx")
                    'End If

                Case "a_keys"
                    Session("new") = Nothing
                    Session("insert") = Nothing
                    Session("login_id") = Nothing
                    Response.Redirect("ct_usuarios.aspx")

                Case "a_new_l"
                    Session("new") = True
                    Session("insert") = Nothing
                    Session("login_id") = Nothing
                    Response.Redirect("ct_usuarios.aspx")


                Case "a_opciones"
                    Session("insert") = Nothing
                    Response.Redirect("ct_menu.aspx")

                Case "a_new_m"
                    Session("insert") = 1
                    Response.Redirect("ct_menu.aspx")

                Case Else

                    If opcion_txt.Text = "a_new" Then
                        Session("edit") = "edit"
                        opcion_txt.Text = "usuario"
                    End If


                    Dim strArr() As String = opcion_txt.Text.Split("|")
                    If strArr.Length > 1 Then
                        Session("DBAccesosUserId") = strArr(0)
                        Session("DBAccesosLogin") = strArr(1)
                        opcion_txt.Text = strArr(2)
                    End If

                    Dim params() As String
                    params = selecciona_sistema(opcion_txt.Text, Server).ToString.Split(",")
                    If params(0) <> "" Then
                        If params(1) = "Nothing" Then
                            Session("DBAccesos") = Nothing
                        Else
                            Session("DBAccesos") = params(1)
                        End If
                        If params(2) = "Nothing" Then
                            Session("DBAccesos_conn") = Nothing
                        Else
                            Session("DBAccesos_conn") = params(2)
                        End If
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
                        Response.Redirect(params(0))
                    Else
                        msg = "No encontro sistema '" & opcion_txt.Text & "'. MasterPage:"
                    End If

            End Select

        End If

    End Sub

    Protected Sub lnk_menues_Click(sender As Object, e As System.EventArgs) Handles lnk_menues.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = Nothing
        Session("icono") = Nothing
        Session("login_id") = Nothing
        Response.Redirect("ct_menu.aspx")
    End Sub

    Protected Sub lnk_modulos_Click(sender As Object, e As System.EventArgs) Handles lnk_modulos.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = Nothing
        Session("icono") = Nothing
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub lnk_usuarios_Click(sender As Object, e As System.EventArgs) Handles lnk_usuarios.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = Nothing
        Session("icono") = Nothing
        Session("login_id") = Nothing
        Response.Redirect("ct_usuarios.aspx")
    End Sub

    Protected Sub lnk_usuario_Click(sender As Object, e As System.EventArgs) Handles lnk_usuario.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = "edit"
        Session("icono") = Nothing
        Session("login_id") = Session("OperatorID")
        Response.Redirect("ct_usuarios.aspx")
    End Sub

    Protected Sub lnk_solicitud_Click(sender As Object, e As System.EventArgs) Handles lnk_solicitud.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = "edit"
        Session("icono") = Nothing
        Session("login_id") = Session("OperatorID")
        Session("solicitud") = Nothing
        Response.Redirect("Solicitud.aspx")
    End Sub


    Protected Sub lnk_solicitu1d_Click(sender As Object, e As System.EventArgs) Handles lnk_solicitud1.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = "edit"
        Session("icono") = Nothing
        Session("login_id") = Session("OperatorID")
        Session("solicitud") = Nothing
        Response.Redirect("Solicitud.aspx")
    End Sub


    Protected Sub lnk_solicitudes_Click(sender As Object, e As System.EventArgs) Handles lnk_solicitudes.Click
        Session("sistema") = Nothing
        Session("DBAccesos") = Nothing
        Session("DBAccesos_conn") = Nothing
        Session("DBAccesosUserId") = Nothing
        Session("DBAccesosLogin") = Nothing
        Session("insert") = Nothing
        Session("edit") = "edit"
        Session("icono") = Nothing
        Session("login_id") = Session("OperatorID")
        Session("solicitud") = Nothing
        Response.Redirect("Solicitudes.aspx")
    End Sub

End Class

