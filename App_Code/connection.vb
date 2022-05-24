Imports MySql.Data.MySqlClient
Imports Npgsql
Imports System.Data
Imports System.IO


Public Class connection


    Public Shared sql1 As String
    Public Shared Comillas As String = "'"

    Public Shared icon_dir As String = "Content/icon/"

    Public Shared icon_err_read As String = "<img src=" & icon_dir & "glyphicons_196_circle_exclamation_mark.png height=16 />"
    Public Shared icon_err_update As String = "<img src=" & icon_dir & "glyphicons_078_warning_sign.png height=16 />"
    Public Shared icon_err_active As String = "<img src=" & icon_dir & "glyphicons_360_bug.png height=16 />"
    Public Shared icon_err_normal As String = "<img src=" & icon_dir & "glyphicons_195_circle_info.png height=16 />"

    Public Shared icon_check As String = "<img src=" & icon_dir & "glyphicons_152_check.png height=16 />"
    Public Shared icon_uncheck As String = "<img src=" & icon_dir & "glyphicons_153_unchecked.png height=16 />"

    Public Shared icon_home As String = "<img src=" & icon_dir & "glyphicons_020_home.png height=16 title='Listado Usuarios'  />&nbsp;Home"
    Public Shared icon_insert As String = "<img src=" & icon_dir & "glyphicons_414_disk_save.png height=16 title='Crear Registro' />&nbsp;Crear Registro"
    Public Shared icon_on As String = "<img src=" & icon_dir & "glyphicons_193_circle_ok.png height=16 title='Activar Registro' />&nbsp;Activar"
    Public Shared icon_update As String = "<img src=" & icon_dir & "glyphicons_416_disk_saved.png height=16 title='Actualizar Registro' />&nbsp;Actualizar"
    Public Shared icon_off As String = "<img src=" & icon_dir & "glyphicons_192_circle_remove.png height=16 title='Desactivar Registro' />&nbsp;Desactivar"

    Public Shared icon_edit As String = "<img src=" & icon_dir & "glyphicons_030_pencil.png height=16 title='Editar Campos de Registro' />&nbsp;Editar"
    Public Shared icon_cancel As String = "<img src=" & icon_dir & "glyphicons_199_ban.png height=16 title='Bloquear Campos de Registro' />&nbsp;Cancelar"
    Public Shared icon_new As String = "<img src=" & icon_dir & "glyphicons_190_circle_plus.png height=16 title='Ingreso Nuevo' />&nbsp;Nuevo"

    Public Shared icon_del As String = "<img src=" & icon_dir & "glyphicons_199_ban.png height=16 title='Borrar Registro' />&nbsp;Borrar"

    'default
    Public Shared icon_search As String = "<img src=" & icon_dir & "glyphicons_027_search.png height=16 title='Busqueda de usuarios' />"
    Public Shared icon_user As String = "<img src=" & icon_dir & "glyphicons_003_user.png height=16 />"

    'ct_usuarios
    Public Shared icon_keys As String = "<img src=" & icon_dir & "glyphicons_044_keys.png height=16 />&nbsp;Usuarios Login"

    Public Shared icon_pasgen As String = "<img src=" & icon_dir & "glyphicons_009_magic.png height=16 />"
    Public Shared icon_pascan As String = "<img src=" & icon_dir & "glyphicons_221_unshare.png height=16 />"
    Public Shared icon_pasenc As String = "<img src=" & icon_dir & "glyphicons_258_qrcode.png height=16 />"

    Public Shared icon_login As String = "<img src=" & icon_dir & "glyphicons_386_log_in.png height=16 title='Ingresar Credenciales' />"
    Public Shared icon_logout As String = "<img src=" & icon_dir & "glyphicons_387_log_out.png height=16 title='Cerrar Session' />"
    Public Shared icon_opciones As String = "<img src=" & icon_dir & "glyphicons_156_show_thumbnails.png height=16 />&nbsp;Opciones de Menu"

    Public Shared icon_solicitudes As String = "<img src=" & icon_dir & "glyphicons_156_show_thumbnails.png height=16 />&nbsp;Solicitudes"

    'ventas maritimo & menu
    Public Shared icon_open As String = "<img src=" & icon_dir & "glyphicons_144_folder_open.png height=16 />"

    Public Shared icon_clonar As String = "<img src=" & icon_dir & "glyphicons_154_more_windows.png height=16 />&nbsp;Clonar"



    Public Shared Function getEmpresaUser(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As String
        Dim pais_iso As String = ""
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT pais FROM usuarios_empresas WHERE id_usuario = @codigo"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    pais_iso = dataread(0)
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_activo")
        End Try
        Return pais_iso
    End Function



    Public Shared Function setEmpresaUser(ByVal traffic As String, ByVal user As Integer, ByVal pais_iso As String, ByVal pais_iso_ant As String, ByVal Server As System.Web.HttpServerUtility) As String
        Dim countries As String = ""
        Dim countries_new As String = ""

        Dim strArr() As String
        Dim count As Integer = 0
        Try
            Using conn As New MySqlConnection(GetConnectionStringFromFile(traffic, Server))

                If traffic = "wms" Then
                    sql1 = "SELECT COUNTRIES FROM DEF_USERS WHERE id_usuario = @OperatorID AND STATUS = 1"
                Else
                    sql1 = "SELECT Countries FROM Operators WHERE OperatorID = @OperatorID AND Active = 1"
                End If

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = user
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    countries_new = Comillas & pais_iso & Comillas
                    countries = dataread(0)
                    strArr = countries.Split(",") 'array en base de datos
                    For count = 0 To strArr.Length - 1
                        If Comillas & pais_iso & Comillas = strArr(count) Or Comillas & pais_iso_ant & Comillas = strArr(count) Then

                        Else
                            If strArr(count) <> "" Then
                                countries_new = countries_new & "," & strArr(count)
                            End If
                        End If
                    Next
                End If
                conn.Close()

                If countries_new <> "" Then
                    If traffic = "wms" Then
                        sql1 = "UPDATE DEF_USERS SET COUNTRIES = @countries WHERE id_usuario = @OperatorID AND STATUS = 1"
                    Else
                        sql1 = "UPDATE Operators SET Countries = @countries WHERE OperatorID = @OperatorID AND Active = 1"
                    End If
                    comm = New MySqlCommand(sql1, conn)
                    comm.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = user
                    comm.Parameters.Add("@countries", MySqlDbType.VarString).Value = countries_new
                    conn.Open()
                    comm.ExecuteNonQuery()
                Else
                    countries_new = countries_new
                End If

            End Using

        Catch ex As Exception
            MsgBox(ex.Message, , "user_activo")
        End Try
        Return ""
    End Function


    Public Shared Function user_activo(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT pw_activo FROM usuarios_empresas WHERE id_usuario = @codigo AND pw_activo = 1"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = dataread(0)
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_activo")
        End Try
        Return activo
    End Function


    Public Shared Function user_catalogo(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT level FROM usuarios_empresas WHERE id_usuario = @codigo AND level <> 0" ' AND pw_activo = 1"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_catalogo")
        End Try
        Return activo
    End Function

    Public Shared Function user_aereo(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New MySqlConnection(GetConnectionStringFromFile("aereo", Server))
                sql1 = "SELECT  OperatorID FROM Operators WHERE OperatorID = @codigo AND Active = 1"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.Int32).Value = user
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_aereo")
        End Try
        Return activo
    End Function

    Public Shared Function user_terrestre(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New MySqlConnection(GetConnectionStringFromFile("terrestre", Server))
                sql1 = "SELECT  OperatorID FROM Operators WHERE OperatorID = @codigo AND Active = 1"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.Int32).Value = user
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_terrestre")
        End Try
        Return activo
    End Function

    Public Shared Function user_trafico_maritimo(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "select id_anterior from referencias_usuarios where id_nuevo=@codigo and activo = 't'"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_trafico_maritimo")
        End Try
        Return activo
    End Function

    Public Shared Function user_customer(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As String
        Dim activo As String = "0,0,0,0,"
        Try
            Using conn As New MySqlConnection(GetConnectionStringFromFile("customer", Server))
                'sql1 = "SELECT activo, acceso_aduana, acceso_apl, acceso_maritimo FROM usuarios WHERE id_empresa <> 0 AND id_usuario_empresa = @codigo order by numero desc"

                sql1 = "SELECT distinct activo, acceso_aduana, acceso_apl, acceso_maritimo FROM usuarios WHERE id_empresa <> 0 AND id_usuario_empresa = @codigo order by acceso_aduana"

                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", MySqlDbType.Int32).Value = user
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = ""

                    If dataread(0) = True Then
                        activo = activo & "0,"
                    Else
                        activo = activo & "1,"
                    End If
                    activo = activo & dataread(1) & ","
                    activo = activo & dataread(2) & ","
                    activo = activo & dataread(3) & ","
                    'If dataread(0) = 0 Then
                    '    activo = activo & "1,"
                    'Else
                    '    activo = activo & "0,"
                    'End If
                    'If dataread(1) = 0 Then
                    '    activo = activo & "1,"
                    'Else
                    '    activo = activo & "0,"
                    'End If
                    'If dataread(2) = 0 Then
                    '    activo = activo & "1,"
                    'Else
                    '    activo = activo & "0,"
                    'End If
                    'If dataread(3) = 0 Then
                    '    activo = activo & "1,"
                    'Else
                    '    activo = activo & "0,"
                    'End If
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_customer")
        End Try
        Return activo
    End Function

    Public Shared Function user_seguros(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT id_usuario FROM detalle_tipos_usuario WHERE id_usuario = @codigo AND id_tipo_usuario <> 0"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_seguros")
        End Try
        Return activo
    End Function

    Public Shared Function user_wms(ByVal user As String, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New MySqlConnection(GetConnectionStringFromFile("wms", Server))
                sql1 = "SELECT COD_USER FROM DEF_USERS WHERE COD_USER = @login AND STATUS = 1"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@login", MySqlDbType.String).Value = user
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_wms")
        End Try
        Return activo
    End Function

    Public Shared Function user_manifiestos_cr(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & "cr")
                sql1 = "SELECT user_id FROM manifiestos_usuarios WHERE id_master = @codigo AND activo='t'"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_manifiestos_cr")
        End Try
        Return activo
    End Function

    'Public Shared Function user_manifiestos_cr_demo(ByVal user As Integer) As Integer
    '    Dim activo As Integer = 0
    '    Try
    '        Using conn As New NpgsqlConnection(conn_db(conn_str, "ventas_cr_demo", demo))
    '            sql1 = "SELECT user_id FROM manifiestos_usuarios WHERE id_master = @codigo AND activo='t'"
    '            Dim comm As New NpgsqlCommand(sql1, conn)
    '            comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
    '            conn.Open()
    '            Dim dataread As NpgsqlDataReader = comm.ExecuteReader
    '            If dataread.Read() Then
    '                activo = 1
    '            End If
    '            dataread.Close()
    '        End Using
    '    Catch ex As Exception
    '        msgbox(ex.Message)
    '    End Try
    '    Return activo
    'End Function

    Public Shared Function user_manifiestos_crltf(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("ventas", Server) & "crltf")
                sql1 = "SELECT user_id FROM manifiestos_usuarios WHERE id_master = @codigo AND activo='t'"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_manifiestos_crltf")
        End Try
        Return activo
    End Function


    Public Shared Function user_manifiestos_gtweb(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                sql1 = "SELECT userid FROM aausers WHERE id_usuario = @codigo"
                Dim comm As New NpgsqlCommand(sql1, conn)
                comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_manifiestos_gtweb")
        End Try
        Return activo
    End Function


    Public Shared Function user_baw(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try
            'Using conn As New NpgsqlConnection(conn_db(conn_str, "baw", demo))
            '    sql1 = "SELECT user_id FROM manifiestos_usuarios WHERE id_master = @codigo AND activo='t'"
            '    Dim comm As New NpgsqlCommand(sql1, conn)
            '    comm.Parameters.Add("@codigo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = user
            '    conn.Open()
            '    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
            '    If dataread.Read() Then
            activo = 0
            '    End If
            '    dataread.Close()
            'End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_baw")
        End Try
        Return activo
    End Function

    Public Shared Function user_caja(ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As Integer
        Dim activo As Integer = 0
        Try

            Using conn As New MySqlConnection(GetConnectionStringFromFile("caja", Server))
                sql1 = "SELECT id_master FROM usuarios WHERE id_master = @id_master AND activo = 1"
                Dim comm As New MySqlCommand(sql1, conn)
                comm.Parameters.Add("@id_master", MySqlDbType.Int32).Value = user
                conn.Open()
                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    activo = 1
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "user_caja")
        End Try
        Return activo
    End Function




    Public Shared Function GetConnectionStringFromFile(ByVal xScheme As String, ByVal Server As System.Web.HttpServerUtility) As String

        Dim xobj As New System.Xml.XmlDocument
        Dim xelement As System.Xml.XmlElement

        'Server.MapPath(".") returns D:\WebApps\shop\products
        'Server.MapPath("..") returns D:\WebApps\shop
        'Server.MapPath("~") returns D:\WebApps\shop
        'Server.MapPath("/") returns C:\Inetpub\wwwroot
        'Server.MapPath("/shop") returns D:\WebApps\shop

        If xScheme = "apl" Or xScheme = "maritimo_c" Or xScheme = "aduana" Then
            xScheme = "customer"
        End If

        If xScheme = "usuario" Or xScheme = "catalogo" Or xScheme = "seguros" Then
            xScheme = "aimar"
        End If

        If xScheme = "pricing" Then
            xScheme = "pricing"
        End If


        Dim tmpConn As String = ""

        Try
            Dim tmpStr As String = Server.MapPath("~") & "\App_Code\Connections.xml"
            xobj.Load(tmpStr)
            xelement = xobj.SelectSingleNode("conn/connections[@default='true']")
            xelement = xelement.SelectSingleNode("connection[@name='" + xScheme + "']")
            tmpConn = xelement.GetAttribute("connectionString")
        Catch ex As Exception
            'MsgBox(ex.Message, , "GetConnectionStringFromFile " & xScheme)
            'esto da error en el server
        End Try

        Return tmpConn

    End Function




    Public Shared Function selecciona_sistema(ByVal sistema As String, ByVal Server As System.Web.HttpServerUtility) As String
        Dim respuesta As String = ",,,,,,,,"
        Try
            Dim link As String = ""
            sql1 = "SELECT * FROM usuarios_empresas_menu WHERE ide = '" & sistema & "' ORDER BY orden"
            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))
                Dim cmd As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = cmd.ExecuteReader
                If dataread.Read() Then
                    respuesta = ""
                    respuesta = respuesta & dataread(2) & "," 'link
                    If dataread(4) = "maritimo" Then
                        respuesta = respuesta & "Nothing," 'DBAccesos
                        respuesta = respuesta & "Nothing," 'DBAccesos_conn
                    Else
                        respuesta = respuesta & dataread(4) & "," 'DBAccesos


                        If dataread(4) = "ventas_cr" Or dataread(4) = "ventas_crltf" Then
                            respuesta = respuesta & GetConnectionStringFromFile("ventas", Server) & Mid(dataread(4), 8) & ","
                        Else
                            respuesta = respuesta & GetConnectionStringFromFile(dataread(4), Server) & ","
                        End If


                    End If
                    respuesta = respuesta & dataread(4) & "," 'pestaña
                    If dataread(5) = "" Then
                        respuesta = respuesta & dataread(4) & "," 'sistema = ide
                    Else
                        respuesta = respuesta & dataread(5) & "," 'sistema = parent
                    End If
                    respuesta = respuesta & "Nothing," 'insert
                    respuesta = respuesta & "Nothing," 'edit
                    respuesta = respuesta & "<img src='Content/icon/" & dataread(3) & "' height='16px'> " & dataread(1) & "," 'icon
                    '1 nombre   '2 link '3 icon '4 ide  '5 parent   '6 orden    '7 childs
                End If
            End Using

        Catch ex As Exception
            MsgBox(ex.Message, , "selecciona_sistema")
        End Try
        Return respuesta
    End Function


    Public Shared Function menu_gen(ByVal parent As String, ByVal html As String, ByVal icono As Boolean, ByVal user As Integer, ByVal Server As System.Web.HttpServerUtility) As String
        Try

            Using conn As New NpgsqlConnection(GetConnectionStringFromFile("aimar", Server))

                Dim mod_str As String = ""
                sql1 = "SELECT modulos FROM usuarios_empresas_login WHERE create_id_usuario = '" & user & "'"
                Dim cmd As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread1 As NpgsqlDataReader = cmd.ExecuteReader
                If dataread1.Read() Then
                    If dataread1(0).ToString.Length > 0 Then
                        mod_str = dataread1(0)
                    End If
                End If
                dataread1.Close()

                Dim strArr() As String
                strArr = mod_str.Split(",")
                mod_str = ""
                Dim count As Integer
                For count = 0 To strArr.Length - 1
                    If count > 0 Then
                        mod_str = mod_str & ","
                    End If
                    mod_str = mod_str & "'" & strArr(count) & "'"
                Next

                If icono = False Then
                    html = html & "<li id='li1_home'><a id='a_home' name='lnk_' style='cursor: pointer;'>" & icon_home & "</a></li>" 'home para pestañas
                End If

                sql1 = "SELECT id, nombre, link, icon, ide, parent, orden, childs, status FROM usuarios_empresas_menu WHERE parent = '" & parent & "' AND ide IN (" & mod_str & ") AND status = 't' ORDER BY orden"
                cmd.CommandText = sql1
                Dim dataread2 As NpgsqlDataReader = cmd.ExecuteReader
                If Not dataread2.Read() Then
                    dataread2.Close()
                    sql1 = "SELECT id, nombre, link, icon, ide, parent, orden, childs, status FROM usuarios_empresas_menu WHERE ide = '" & parent & "' AND ide IN (" & mod_str & ") AND status = 't' ORDER BY orden"
                    cmd.CommandText = sql1
                Else
                    dataread2.Close()
                End If

                Dim dataread As NpgsqlDataReader = cmd.ExecuteReader
                While dataread.Read()
                    If dataread(7) = False Then
                        If icono = True Then
                            html = html & "<li id='li_" & dataread(4) & "'>"
                        Else
                            html = html & "<li id='li1_" & dataread(4) & "'>" ' cuando son tabs con distinto id para no duplicar con el menu principal
                        End If
                        html = html & "<a onmouseover=this.style.cursor='pointer' id='" & dataread(4) & "' name='lnk_' title='" & dataread(1) & "'>"
                        If icono = True Then
                            html = html & "#*icono_" & dataread(4) & "*#&nbsp;" 'icono check uncheck menu principal
                        Else
                            html = html & "<img src='" & icon_dir & dataread(3) & "' height='16' />&nbsp;" 'icono propio de sistema pestañas
                        End If
                        html = html & dataread(1) & "</a></li>"
                    Else
                        html = html & "<li id='li_" & dataread(4) & "' class='dropdown'>"
                        html = html & "<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" & dataread(1) & "<b class='caret'></b></a>"
                        html = html & "<ul class='dropdown-menu'>"
                        html = html & menu_gen(dataread(4), "", icono, user, Server)
                        html = html & "</ul>"
                        html = html & "</li>"
                    End If
                End While
                dataread.Close()
            End Using

        Catch ex As Exception
            MsgBox(ex.Message, , "menu_gen")
        End Try

        Return html
    End Function



    '========================================================== 
    Public Shared Function PwdAleatorio(ByVal Longitud As Integer, ByVal Repetir As Boolean) As String

        '---------------------------------------------------------- 
        ' por Carlos de la Orden Dijs, Abril 2001 - 100% gratis! ;-) 
        '---------------------------------------------------------- 
        ' Devuelve una cadena con una secuencia de caracteres aleatoria, de longitud especificada. 
        ' Si Repetir = True la secuencia puede contener caracteres repetidos. Si Repetir = False, todos los caracteres son únicos. 
        ' Para añadir más caracteres posibles, añadirlos al vector vCaracteres simplemente separando como comas, como los que están ya escritos, y la función los escogerá. 
        '----------------------------------------------------------
        Dim vPass() As Char
        Dim I, J As Integer
        ' nuestro vector y dos contadores 
        Dim vNumeros() As Integer
        ' vector para guardar lo que llevamos 
        Dim n As Integer
        Dim bRep As Boolean
        ' vector donde están los posibles caract. 
        Dim vCaracteres As String() = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
        ' Establezco la longitud del vector 
        ReDim vPass(Longitud - 1)
        ' Y del vector auxiliar que guarda los caracteres ya escogidos 
        ReDim vNumeros(Longitud - 1)
        I = 0
        ' Inicializo los nºs aleatorios Randomize Hasta que encuentre todos los caracteres 

        Randomize()
        ' Initializes the random-number generator, otherwise each time you run your program, the sequence of numbers will be the same

        Do Until I = Longitud
            ' Hallo un número aleatorio entre 0 y el máximo indice del vector de caracteres. 
            n = Int(Rnd() * UBound(vCaracteres))

            ' Si no puedo repetir... 
            If Not Repetir Then
                bRep = False
                ' Busco el numero entre los ya elegidos 
                For J = 0 To UBound(vNumeros)

                    'Dim ch1 As String = vCaracteres(n)
                    'Dim ch2 As String = vCaracteres(vNumeros(J))

                    If n = vNumeros(J) Then
                        'If ch1 = ch2 Then
                        ' Si esta, indico que ya estaba 
                        bRep = True
                    End If
                Next
                ' Si ya estaba, tengo que repetir este caracter así que resto 1 a I y volvemos sobre la misma posición. 
                If bRep = True Then
                    I = I - 1
                Else
                    vNumeros(I) = n
                    vPass(I) = vCaracteres(n)
                End If
            Else
                ' Me da igual que esté o no repetido 
                vNumeros(I) = n
                vPass(I) = vCaracteres(n)
            End If

            ' Siguiente carácter! 
            I = I + 1
        Loop

        'Devuelvo la cadena. Join une los elementos de un vector utilizando como separador el segundo parámetro: en este 'caso, nada -> "". 
        'PwdAleatorio = String.Join("", vPass)
        PwdAleatorio = ""
        For Each item As Char In vPass
            PwdAleatorio = PwdAleatorio & item
        Next

        'Dim items As New Dictionary(Of String, String)
        'log(0, PwdAleatorio, "", items, "", Session("OperatorID"), Session("Login"), Session("sistema"), Session("DBAccesos"), Session("OperatorIP"))


    End Function
    'PwdAleatorio 
    '==========================================================



    Public Shared Function valida_pagina(ByVal url As String, ByVal master_str As String, ByVal user As Integer, ByVal sistema As String) As Boolean

        Dim res As Boolean = False

        Try

            'valida_pagina = False
            Dim paginas() As String = url.Split("/")
            Dim a As Integer = paginas.Length
            Dim pagina As String = paginas(a - 1)

            Using conn As New NpgsqlConnection(master_str)

                Dim mod_str As String = ""
                sql1 = "SELECT modulos FROM usuarios_empresas_login WHERE create_id_usuario = '" & user & "'"
                Dim cmd As New NpgsqlCommand(sql1, conn)
                conn.Open()
                Dim dataread1 As NpgsqlDataReader = cmd.ExecuteReader
                If dataread1.Read() Then
                    If dataread1(0).ToString.Length > 0 Then
                        mod_str = dataread1(0)
                    End If
                End If
                dataread1.Close()

                Dim strArr() As String
                strArr = mod_str.Split(",")
                mod_str = ""
                Dim count As Integer
                For count = 0 To strArr.Length - 1
                    If count > 0 Then
                        mod_str = mod_str & ","
                    End If
                    mod_str = mod_str & "'" & strArr(count) & "'"
                Next

                Dim ide As String = ""
                sql1 = "SELECT * FROM usuarios_empresas_menu WHERE link = '" & pagina & "' AND ide IN (" & mod_str & ") AND status = 't' ORDER BY orden"
                Dim comm As New NpgsqlCommand(sql1, conn)
                'conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    res = True

                    If dataread(5) = "" Then
                        ide = dataread(4)
                    Else
                        ide = dataread(5)
                    End If

                End If
                dataread.Close()

                If sistema <> ide Then
                    res = False
                End If

            End Using
        Catch ex As Exception
            MsgBox(ex.Message, , "valida_pagina")
        End Try

        Return res 'valida_pagina

    End Function

    Public Shared Function db_oper(ByVal tipo As String, ByVal table As String, ByVal tipo_db As String, ByVal arr_dato As Dictionary(Of String, String), ByVal conn As Object, ByVal arr_where As Dictionary(Of String, String)) As Dictionary(Of String, String)
        tipo = tipo.ToUpper
        tipo_db = tipo_db.ToUpper

        Dim keys As String = ""
        Dim values As String = ""
        For Each li As KeyValuePair(Of String, String) In arr_dato
            Select Case tipo
                Case "SELECT", "INSERT"
                    If keys <> "" Then
                        keys = keys & ","
                    End If
                    If values <> "" Then
                        values = values & ","
                    End If
                    keys = keys & li.Key
                    values = values & "'" & li.Value & "'"
                Case "UPDATE"
                    If values <> "" Then
                        values = values & ","
                    End If
                    values = values & li.Key & "='" & li.Value & "'"
            End Select
        Next

        Dim where As String = ""
        For Each li As KeyValuePair(Of String, String) In arr_where
            Select Case tipo
                Case "DELETE", "UPDATE"
                    If where <> "" Then
                        where = where & ","
                    End If
                    where = where & li.Key & "='" & li.Value & "'"
            End Select
        Next

        Dim sql As String = ""
        Select Case tipo
            Case "INSERT"
                sql = "INSERT INTO " & table & " (" & keys & ") VALUES (" & values & ")"

            Case "SELECT"
                If keys = "" Then
                    keys = "*"
                End If
                sql = "SELECT " & keys & " FROM " & table & " " & where

            Case "DELETE"
                sql = "DELETE FROM " & table & " WHERE " & where

            Case "UPDATE"
                sql = "UPDATE " & table & " SET " & values & " WHERE " & where

        End Select

        Dim arr_result As New Dictionary(Of String, String)
        arr_result.Add("sql", sql)

        Try


            Select Case tipo_db
                Case "MYSQL"

                    Dim comm As New MySqlCommand(sql, conn)

                    Select Case tipo
                        Case "SELECT"
                            Dim dataread As MySqlDataReader = comm.ExecuteReader
                            'db_oper.Add("data", dataread)

                        Case "DELETE", "UPDATE", "INSERT"
                            comm.ExecuteNonQuery()
                    End Select

                    arr_result.Add("affected", comm.UpdatedRowSource)
                    arr_result.Add("last_id", comm.LastInsertedId)

                Case "POSTGRES"

                    Dim comm As New NpgsqlCommand(sql, conn)

                    Select Case tipo
                        Case "SELECT"

                        Case "DELETE", "UPDATE", "INSERT"
                            comm.ExecuteNonQuery()
                    End Select

                    arr_result.Add("affected", comm.UpdatedRowSource)
                    'arr_result.Add("last_id", comm.LastInsertedId)

            End Select


        Catch ex As Exception
            arr_result.Add("error", ex.Message)
        End Try

        Return arr_result

    End Function


    'function db_oper($tipo,$table,$display,$arr_dato=array(),$conn,$arr_where=array()){

    '        function mayusculas(&$texto) {
    '            for ($i=0;$i<strlen($texto);$i++)
    '                if (strpos("*ABCDEFGHIJKLMNOPQRSTUVWXYZ",$texto[$i])) {
    '                    $texto = "\"$texto\"";
    '                    break;
    '                }
    '        }

    '        $sql = strtoupper($tipo);

    '        mayusculas(&$table);

    '        switch ($tipo) {
    '        case "insert":$sql .= " INTO $table ";  break;
    '        case "update":$sql .= " $table SET ";   break;
    '        case "delete":$sql .= " FROM $table ";  break;
    '        }

    '        $keys = "";    
    '        $values = "";    

    '        foreach ($arr_dato as $key => $value) {        

    '            mayusculas(&$key);

    '            if (strpos($value,"||") == -1)
    '                $value = "'$value'";
    '                Else
    '                $value = substr($value,2,strlen($value));

    '            switch ($tipo) {
    '            case "insert":
    '                if (!empty($keys)) {
    '                    $keys .= ", ";
    '                    $values .= ", ";
    '                }
    '                $keys .= $key;
    '                $values .= $value;
    '                break;
    '            case "update":
    '                if (!empty($values)) {
    '                    $values .= ", ";
    '                }
    '                $values .= "$key=$value";
    '                break;
    '            }        
    '        }



    '        $where = "";        
    '        foreach ($arr_where as $key => $value) {        
    '            mayusculas(&$key);
    '            switch ($tipo) {
    '            case "update":
    '            case "delete":
    '                if (!empty($where))
    '                    $where .= " AND ";
    '                $where .= "$key = '$value'";
    '                break;            
    '            }        
    '        }

    '        switch ($tipo) {
    '        case "insert":$sql .= " ($keys) VALUES ($values);"; break;
    '        case "update":$sql .= " $values WHERE $where;";     break;
    '        case "delete":$sql .= " WHERE $where;";             break;
    '        }

    '        $conn->Execute($sql);

    '        $result['error'] = $conn->ErrorMsg();	
    '        $result['affected'] = $conn->Affected_Rows();
    '        $result['sql'] = $sql;

    '        if ($display) {        
    '            echo "<pre>";
    '            print_r($result);
    '            echo "</pre>";        
    '        }

    '        return $result;
    '}	




End Class
