Imports connection
Imports Npgsql
Imports System.Data
Imports logs
Imports MySql.Data.MySqlClient
Imports System.IO

Partial Class Solicitud
    Inherits System.Web.UI.Page

    Public accordion_open(11) As String
    Public tab_titl(11) As String
    Public tab_cont(11) As String
    Public qry As String = ""

    Public CnnMs As String = ""

    Public msg As String = ""
    Public img As String = icon_err_normal
    Public css As String = "alert-info"

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        If Session("OperatorID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If

        If acordion_field.Text = "" Then
            acordion_field.Text = 1
        End If
        Dim i As Integer
        For i = 1 To accordion_open.Length - 1
            If i = acordion_field.Text Then
                accordion_open(i) = "in"
                tab_titl(i) = "class=active"
                tab_cont(i) = " active"
            Else
                accordion_open(i) = ""
                tab_titl(i) = ""
                tab_cont(i) = ""
            End If
        Next


        If IsPostBack Then
            lee_datos(False)
        Else
            lee_combos()
            lee_datos(True)
        End If

        Empresas() 'llena combo de empresas para customer maritimo
        Ubicaciones()

        Dim nivel1 As TreeNode

        nivel1 = New TreeNode()

        'BawTreeView.Nodes.Add()

    End Sub


    'Protected Function GetConnectionStringFromFile(ByVal xScheme As String, ByVal Server1 As System.Web.HttpServerUtility) As String

    '    Dim xobj As New System.Xml.XmlDocument
    '    Dim xelement As System.Xml.XmlElement


    '    'Server.MapPath(".") returns D:\WebApps\shop\products
    '    'Server.MapPath("..") returns D:\WebApps\shop
    '    'Server.MapPath("~") returns D:\WebApps\shop
    '    'Server.MapPath("/") returns C:\Inetpub\wwwroot
    '    'Server.MapPath("/shop") returns D:\WebApps\shop

    '    Try
    '        Dim tmpStr As String = server1.MapPath("~") & "\App_Code\Connections.xml"
    '        xobj.Load(tmpStr)
    '    Catch
    '        'Err.Raise(Err.Number, "CommonServices", Err.Description)
    '        'Exit Function
    '    End Try



    '    Try
    '        'xelement = xobj.SelectSingleNode("conn/connections[default='demo']/connection[@name='" + xScheme + "']")
    '        xelement = xobj.SelectSingleNode("conn/connections[@default='true']")
    '        'xScheme = xelement.GetAttribute("default")
    '    Catch
    '        'Err.Raise(Err.Number, "CommonServices: GetConnectionString", "Error tratando de leer el elemento xScheme")
    '    End Try



    '    Try
    '        'xelement = xelement.SelectSingleNode("conn/connections/connection[@name='" + xScheme + "']")
    '        xelement = xelement.SelectSingleNode("connection[@name='" + xScheme + "']")
    '    Catch
    '        'Err.Raise(Err.Number, "CommonServices: GetConnectionString", "xScheme [" + xScheme + "] No Valido")
    '    End Try

    '    Dim tmpConn As String = ""
    '    Try
    '        tmpConn = xelement.GetAttribute("connectionString")
    '    Catch
    '        'Err.Raise(Err.Number, "CommonServices: GetConnectionString", "No se encontro el atributo connectionstring")
    '    End Try

    '    Return tmpConn

    'End Function

    Protected Sub lee_datos(ByVal flg As Boolean)
        Try
            CnnMs = GetConnectionStringFromFile("aimar", Server)
            Using conn As New NpgsqlConnection(CnnMs)
                If Session("solicitud") = Nothing Then
                    qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_usr = '" & Session("OperatorID") & "' AND sol_sta < '3' ORDER BY sol_no DESC"
                Else
                    qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Session("solicitud")
                End If
                Dim SolPerm As New SolicitudPermiso()
                Dim comm As New NpgsqlCommand(qry, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    SolPerm = DeserializePermiso(dataread(8))
                    If flg = True Then
                        llena_datos(SolPerm, dataread)
                    End If
                    'llena_ventas(SolPerm)
                End If
                dataread.Close()
            End Using
        Catch ex As Exception
            'MsgBox(ex.Message, , "lee datos")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try
    End Sub


    Protected Sub llena_ventas(ByVal SolPerm As SolicitudPermiso)

        'Try
        '    '//////////////////////////////////// VENTAS / MARITIMO //////////////////////////
        '    For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
        '        For Each li1 As ListItem In vPais.Items
        '            If li.Key = li1.Value Then
        '                li1.Selected = True
        '            End If
        '        Next
        '    Next

        'Catch ex As Exception
        '    MsgBox(ex.Message, , "llena ventas")
        'End Try

    End Sub

    Protected Sub llena_datos(SolPerm As SolicitudPermiso, ByVal dataread As NpgsqlDataReader)

        Solicitud.Text = dataread(0)

        Estatus.SelectedValue = dataread(3)

        Try

            For Each li As KeyValuePair(Of String, String) In SolPerm.Generales
                If (li.Key = "Empleado_id") Then
                    Empleado_id.SelectedValue = li.Value
                End If
                If (li.Key = "Usuario") Then
                    Usuario.Text = li.Value
                End If
                If (li.Key = "Empleado_nombre") Then
                    Empleado_nombre.Text = li.Value
                End If
                If (li.Key = "Dominio") Then
                    Dominio.SelectedValue = li.Value
                End If
                If (li.Key = "Correo") Then
                    Correo.SelectedValue = li.Value
                End If
                If (li.Key = "Chat") Then
                    Chat.SelectedValue = li.Value
                End If
                If (li.Key = "Solicitante") Then
                    Solicitante.Text = li.Value
                End If
                If (li.Key = "Nuevo") Then
                    Nuevo.SelectedValue = li.Value
                End If
                If (li.Key = "Reemplaza") Then
                    Reemplaza.Text = li.Value
                End If
                If (li.Key = "Pais") Then
                    gPais.SelectedValue = li.Value
                End If
                If (li.Key = "Tipo_usuario") Then
                    tipo_usuario.SelectedValue = li.Value
                End If
                If (li.Key = "Ubicacion") Then
                    locode.SelectedValue = li.Value
                End If

                'catalogo
                'cgNivel.SelectedValue = dataread(8)

                If (li.Key = "Empresas") Then

                    Dim empresas_str As String() = li.Value.Split(",")

                    For Each emp As String In empresas_str
                        For Each li1 As ListItem In gEmpresas.Items
                            If emp = li1.Value Then
                                li1.Selected = True
                                Exit For
                            End If
                        Next
                    Next

                    tEmpresas.Items.Clear()
                    Dim temp0 As ListItem
                    For Each li2 As ListItem In gEmpresas.Items
                        If li2.Selected = True Then
                            temp0 = New ListItem(li2.Text, li2.Value)
                            tEmpresas.Items.Add(temp0)
                        End If
                    Next

                End If
                If (li.Key = "Especiales") Then
                    Especiales.Text = li.Value
                End If



            Next
        Catch ex As Exception

        End Try

        Try
            '//////////////////////////////////// AEREO //////////////////////////
            Dim j As Integer = 0
            For j = 0 To SolPerm.Aereo.Pais.Length - 1
                For Each li1 As ListItem In aPais.Items
                    If li1.Value = SolPerm.Aereo.Pais(j) Then
                        li1.Selected = True
                        Exit For
                    End If
                Next
            Next
            aNivel.SelectedValue = SolPerm.Aereo.Nivel

        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// TERRESTRE //////////////////////////                        
            Dim j As Integer = 0
            For j = 0 To SolPerm.Terrestre.Pais.Length - 1
                For Each li1 As ListItem In tPais.Items
                    If li1.Value = SolPerm.Terrestre.Pais(j) Then
                        li1.Selected = True
                        Exit For
                    End If
                Next
            Next
            tNivel.SelectedValue = SolPerm.Terrestre.Nivel

        Catch ex As Exception

        End Try



        Try

            '//////////////////////////////////// CUSTOMER MARITIMO //////////////////////////
            cmNivel.SelectedValue = SolPerm.Customer.Maritimo.Nivel

        Catch ex As Exception

        End Try


        Empresas()
        Ubicaciones()


        Try

            '//////////////////////////////////// CUSTOMER ADUANAS //////////////////////////
            Dim j As Integer = 0
            For j = 0 To SolPerm.Customer.Aduanas.Empresas.Length - 1
                For Each li1 As ListItem In cEmpresas.Items
                    If li1.Value = SolPerm.Customer.Aduanas.Empresas(j) Then
                        li1.Selected = True
                        Exit For
                    End If
                Next
            Next
            caNivel.SelectedValue = SolPerm.Customer.Aduanas.Nivel


        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// CUSTOMER BITACORA //////////////////////////                        
            cbNivel.SelectedValue = SolPerm.Customer.Bitacora.Nivel


        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// CAJA DE AHORRO //////////////////////////                        
            Dim j As Integer = 0
            For j = 0 To SolPerm.Caja.Pais.Length - 1
                For Each li1 As ListItem In cPais.Items
                    If li1.Value = SolPerm.Caja.Pais(j) Then
                        li1.Selected = True
                        Exit For
                    End If
                Next
            Next
            cNivel.SelectedValue = SolPerm.Caja.Nivel

        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// SEGUROS //////////////////////////                            
            sNivel.SelectedValue = SolPerm.Seguros.Nivel

        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// CATALOGOS //////////////////////////                            
            cgNivel.SelectedValue = SolPerm.Catalogos.Nivel

        Catch ex As Exception

        End Try

        Try
            '//////////////////////////////////// WMS //////////////////////////                        
            Dim j As Integer = 0
            For j = 0 To SolPerm.Wms.Bodegas.Length - 1
                For Each li1 As ListItem In Bodega.Items
                    If li1.Value = SolPerm.Wms.Bodegas(j) Then
                        li1.Selected = True
                        Exit For
                    End If
                Next
            Next
            Tipo.SelectedValue = SolPerm.Wms.Tipo
            Grupo.SelectedValue = SolPerm.Wms.Grupo

        Catch ex As Exception

        End Try

        Try
            '//////////////////////////////////// BAW //////////////////////////                        
            For Each li As KeyValuePair(Of String, String) In SolPerm.Baw

                If li.Key.Substring(0, 2) = "01" Then
                    For Each li1 As ListItem In Checkboxlist1.Items
                        If li1.Value = li.Key Then
                            li1.Selected = True
                            Exit For
                        End If
                    Next
                End If

                If li.Key.Substring(0, 2) = "02" Then
                    For Each li1 As ListItem In Checkboxlist2.Items
                        If li1.Value = li.Key Then
                            li1.Selected = True
                            Exit For
                        End If
                    Next
                End If

                If li.Key.Substring(0, 2) = "03" Then
                    For Each li1 As ListItem In Checkboxlist3.Items
                        If li1.Value = li.Key Then
                            li1.Selected = True
                            Exit For
                        End If
                    Next
                End If

                If li.Key.Substring(0, 2) = "04" Then
                    For Each li1 As ListItem In Checkboxlist4.Items
                        If li1.Value = li.Key Then
                            li1.Selected = True
                            Exit For
                        End If
                    Next
                End If
            Next



        Catch ex As Exception

        End Try


        Try

            '//////////////////////////////////// MANIFIESTOS CR / CRLTF //////////////////////////                            
            For Each li As String In SolPerm.Manifiestos.nivelcr
                For Each li1 As ListItem In mcrNivel.Items
                    If li1.Value = li Then
                        li1.Selected = True
                    End If
                Next
            Next

            For Each li As String In SolPerm.Manifiestos.nivelcl
                For Each li1 As ListItem In mclNivel.Items
                    If li1.Value = li Then
                        li1.Selected = True
                    End If
                Next
            Next

        Catch ex As Exception

        End Try



        'Try

        '    '//////////////////////////////////// TIR //////////////////////////     llena_datos leidos de tabla       
        '    For Each li As KeyValuePair(Of String, String) In SolPerm.Tir
        '        For Each li1 As ListItem In Permisos.Items
        '            If li1.Value = li.Key Then
        '                li1.Selected = True
        '                Exit For
        '            End If
        '        Next
        '    Next

        'Catch ex As Exception

        'End Try



        Try
            '//////////////////////////////////// PLANILLAS //////////////////////////
            pNivel.SelectedValue = SolPerm.Planillas.Nivel
        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// BITACORA OD //////////////////////////            
            odNivel.SelectedValue = SolPerm.Bitacora_od.Nivel
        Catch ex As Exception

        End Try

        Try
            '//////////////////////////////////// GRAFICAS ISO //////////////////////////
            iNivel.SelectedValue = SolPerm.Graficas_iso.Nivel
        Catch ex As Exception

        End Try

        Try
            '//////////////////////////////////// E-MANIFIESTOS APL  //////////////////////////
            mNivel.SelectedValue = SolPerm.eManifiestos_apl.Nivel
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub lee_combos()

        Estatus.Items.Clear()
        Dim temp0 As New ListItem("Creada", "1")
        Estatus.Items.Add(temp0)
        'Dim temp1 As New ListItem("Actualizada", "2")
        'Estatus.Items.Add(temp1)
        If Session("OperatorLevel") = 1 Then
            Dim temp2 As New ListItem("Enviada", "3")
            Estatus.Items.Add(temp2)
            Dim temp3 As New ListItem("Aprobada", "4")
            Estatus.Items.Add(temp3)
            Dim temp4 As New ListItem("Rechazada", "5")
            Estatus.Items.Add(temp4)
        End If



        Dim BawMenu As New Dictionary(Of String, String)

        BawMenu.Clear()
        BawMenu.Add("01", "CUENTAS POR COBRAR")
        BawMenu.Add("0101", "Facturacion a Clientes")
        BawMenu.Add("0102", "Facturacion Proforma a Clientes")
        BawMenu.Add("0103", "Recibo a Clientes")
        BawMenu.Add("0104", "Nota de Debito a Clientes")
        BawMenu.Add("0105", "Notas de Credito a Clientes")
        BawMenu.Add("0106", "Nota de Credito Ajuste Contable Clientes")

        BawMenu.Add("02", "CUENTAS POR PAGAR")
        BawMenu.Add("0201", "Provision(Proveedores)")
        BawMenu.Add("0202", "Provision(Agentes)")
        BawMenu.Add("0203", "Provision(Navieras)")
        BawMenu.Add("0204", "Provisiones Lineas Aereas")
        BawMenu.Add("0205", "Caja(Chica)")
        BawMenu.Add("0206", "Elaboracion de Ordenes de Compra")
        BawMenu.Add("0207", "Autorizacion de Ordenes de Compra")
        BawMenu.Add("0208", "Recepcion de Facturas a Proveedores")
        BawMenu.Add("0209", "Generacion de SOA'S")
        BawMenu.Add("0210", "Modificacion de SOA'S")
        BawMenu.Add("0211", "Generar Recibo de SOA")
        BawMenu.Add("0212", "Nota de Debito Proveedor")
        BawMenu.Add("0213", "Nota de Debito Agente")
        BawMenu.Add("0214", "Nota de Debito Naviera")
        BawMenu.Add("0215", "Nota de Debito Linea Aerea")
        BawMenu.Add("0216", "Nota de Credito Proveedor")
        BawMenu.Add("0217", "Nota de Credito Agente")
        BawMenu.Add("0218", "Nota de Credito Naviera")
        BawMenu.Add("0219", "Nota de Credito Linea Aerea")
        BawMenu.Add("0220", "Nota de Credito a Provision")
        BawMenu.Add("0221", "Nota de Credito a Nota de Debito")
        BawMenu.Add("0222", "Cheque a Proveedores")
        BawMenu.Add("0223", "Cheque a Agentes")
        BawMenu.Add("0224", "Cheque a Navieras")
        BawMenu.Add("0225", "Cheque a Lienas Aereas")
        BawMenu.Add("0226", "Cheque devolucion anticipo clientes")
        BawMenu.Add("0227", "Cheque Anticipo Proveedores")
        BawMenu.Add("0228", "Cheque Anticipo Agentes")
        BawMenu.Add("0229", "Cheque Anticipo Navieras")
        BawMenu.Add("0230", "Cheque Anticipo Lineas Aereas")
        BawMenu.Add("0231", "Cheque Anticipo Caja Chica")
        BawMenu.Add("0232", "Transferencia(Proveedores)")
        BawMenu.Add("0233", "Transferencia(Agentes)")
        BawMenu.Add("0234", "Transferencia(Navieras)")
        BawMenu.Add("0235", "Transferencia Lienas Aereas")
        BawMenu.Add("0236", "Transferencia de Anticipo Proveedores")
        BawMenu.Add("0237", "Transferencia de Anticipo Agentes")
        BawMenu.Add("0238", "Transferencia de Anticipo Navieras")
        BawMenu.Add("0239", "Transferencia de Anticipo Lienas Aereas")
        BawMenu.Add("0240", "Elaboracion de Ajuste de Polizas")
        BawMenu.Add("0241", "Ingresar(Depositos)")
        BawMenu.Add("0242", "Asociar(Depositos)")
        BawMenu.Add("0243", "Nota de Credito Bancaria")
        BawMenu.Add("0244", "Nota de Debito Bancaria")
        BawMenu.Add("0245", "Ver Movimiento de Bancos")
        BawMenu.Add("0246", "Ver Movimiento de Cuentas Contables")
        BawMenu.Add("0247", "Ver Movimiento de Documentos")
        BawMenu.Add("0248", "Conciliaciones(Bancarias)")
        BawMenu.Add("0249", "Liquidacion de Anticipos")
        BawMenu.Add("0250", "Comisiones")
        BawMenu.Add("0251", "Pago de Comisiones")
        BawMenu.Add("0252", "Autorizacion de Creditos")
        BawMenu.Add("0253", "Modificacion de Creditos")
        BawMenu.Add("0254", "Anulaciones")
        BawMenu.Add("0255", "Cambio de Fechas")
        BawMenu.Add("0256", "Modificaciones")

        BawMenu.Add("03", "REPORTES")
        BawMenu.Add("0301", "Reporte(Contable)")
        BawMenu.Add("0302", "Reporte(Operativo)")


        BawMenu.Add("04", "COMISIONES")
        BawMenu.Add("0401", "Ver(comisiones)")
        BawMenu.Add("0402", "Generar Corte de Comisiones")
        BawMenu.Add("0403", "Pago de Comisiones")




        Try
            CnnMs = GetConnectionStringFromFile("aimar", Server)
            Using conn As New NpgsqlConnection(CnnMs)

                '///////////////////////////// GENERALES
                qry = "SELECT id_usuario, pw_gecos FROM usuarios_empresas WHERE pw_gecos <> '' AND id_usuario > 0 ORDER BY pw_gecos"
                Dim ds0 As New DataSet()
                Dim cmd0 As New NpgsqlCommand(qry, conn)
                Dim adp0 As New NpgsqlDataAdapter(cmd0)
                adp0.Fill(ds0)
                Empleado_id.Items.Clear()
                Empleado_id.DataSource = ds0
                Empleado_id.DataTextField = "pw_gecos"
                Empleado_id.DataValueField = "id_usuario"
                Empleado_id.DataBind()

                Dim temp As New ListItem("Seleccione", "0")
                Empleado_id.Items.Add(temp)
                Empleado_id.SelectedValue = "0"


                qry = "SELECT DISTINCT dominio FROM usuarios_empresas WHERE dominio <> '' AND pw_activo = 1 AND dominio <> 'aproasa.com' AND dominio <> 'gmail.com' AND dominio <> 'imi.com.pa' AND dominio <> 'outlook.com' ORDER BY dominio"
                Dim ds1 As New DataSet()
                Dim cmd1 As New NpgsqlCommand(qry, conn)
                Dim adp1 As New NpgsqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Dominio.Items.Clear()
                Dominio.DataSource = ds1
                Dominio.DataTextField = "dominio"
                Dominio.DataValueField = "dominio"
                Dominio.DataBind()

                '///////////////////////////// VENTAS MARITIMO
                'qry = "SELECT database, " & _
                '" '<img src=Content/flags/' || lower(pais) || '-flag.gif height=16 title=' || pais || ' /> ' || pais || ' ' || nombre as flag " & _
                '" FROM usuarios_empresas_db WHERE usado_en ILIKE '%maritimo%' AND status = 't' ORDER BY pais"

                'Dim ds2 As New DataSet()
                'Dim cmd2 As New NpgsqlCommand(qry, conn)
                'Dim adp2 As New NpgsqlDataAdapter(cmd2)
                'adp2.Fill(ds2)

                'vPais.Items.Clear()
                'vPais.DataSource = ds2
                'vPais.DataTextField = "flag"
                'vPais.DataValueField = "database"
                'vPais.DataBind()

                'vPaisSel.Items.Clear()
                'vPaisSel.DataSource = ds2
                'vPaisSel.DataTextField = "flag"
                'vPaisSel.DataValueField = "database"
                'vPaisSel.DataBind()

                conn.Open()

                ventas_grid_caller(conn)




                '///////////////////////////// AEREO                    
                'qry = "SELECT pais, '<img src=Content/flags/' || substring(pais for 2) || '-flag.gif height=16 /> ' || REPLACE(nombre, 'LATIN FREIGHT', 'LTF') as nom FROM usuarios_paises WHERE activo = 't' ORDER BY nombre"
                'qry = "SELECT trim(pais) as pais, '<img src=Content/flags/' || substring(pais for 2) || '-flag.gif height=16 /> ' || nombre as nom FROM usuarios_paises WHERE activo = 't' AND char_length(pais) = 2 AND pais <> 'N1' ORDER BY nombre"
                qry = "SELECT pais_iso as pais, '<img src=Content/flags/' || substring(pais_iso for 2) || '-flag.gif height=16 /> ' || nombre_empresa as nombre FROM empresas WHERE activo = 't' AND char_length(pais_iso) = 2 AND pais_iso <> 'N1' ORDER BY nombre_empresa"
                Dim ds As New DataSet()
                Dim cmd As New NpgsqlCommand(qry, conn)
                Dim adp As New NpgsqlDataAdapter(cmd)
                adp.Fill(ds)
                aPais.Items.Clear()
                aPais.DataSource = ds
                aPais.DataTextField = "nombre"
                aPais.DataValueField = "pais"
                aPais.DataBind()

                '///////////////////////////// TERRESTRE
                tPais.Items.Clear()
                tPais.DataSource = ds
                tPais.DataTextField = "nom"
                tPais.DataValueField = "pais"
                tPais.DataBind()

                '///////////////////////////// GENERALES
                gPais.Items.Clear()
                gPais.DataSource = ds
                gPais.DataTextField = "nom"
                gPais.DataValueField = "pais"
                gPais.DataBind()

                conn.Close()

            End Using



            '//////////////////////////////////// CAJA AHORRO //////////////////////////
            'If cNivel.Items.Count = 0 Then

            CnnMs = GetConnectionStringFromFile("caja", Server)
            Using conn As New MySqlConnection(CnnMs)

                qry = "SELECT idpais, CONCAT('<img src=Content/flags/',idpais,'-flag.gif height=16 /> ',pais) as nom FROM paises ORDER BY pais"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(qry, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                cPais.Items.Clear()
                cPais.DataSource = ds
                cPais.DataTextField = "nom"
                cPais.DataValueField = "idpais"
                cPais.DataBind()


                qry = "SELECT rol_id, rango FROM roles ORDER BY rango"
                Dim ds1 As New DataSet()
                Dim cmd1 As New MySqlCommand(qry, conn)
                Dim adp1 As New MySqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                cNivel.Items.Clear()
                cNivel.DataSource = ds1
                cNivel.DataTextField = "rango"
                cNivel.DataValueField = "rol_id"
                cNivel.DataBind()

            End Using

            'End If


            '///////////////////////////// WMS

            CnnMs = GetConnectionStringFromFile("wms", Server)
            Using conn As New MySqlConnection(CnnMs)

                qry = "SELECT * FROM DEF_GROUPS ORDER BY DESCRIPTION"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(qry, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                Grupo.Items.Clear()
                Grupo.DataSource = ds
                Grupo.DataTextField = "DESCRIPTION"
                Grupo.DataValueField = "COD_GROUP"
                Grupo.DataBind()

                qry = "SELECT DOMAINVALUE, MEANING FROM DEF_DOMAINS WHERE DOMAIN = 'USUARIOS_TIPOS' ORDER BY MEANING"
                Dim ds1 As New DataSet()
                Dim cmd1 As New MySqlCommand(qry, conn)
                Dim adp1 As New MySqlDataAdapter(cmd1)
                adp1.Fill(ds1)
                Tipo.Items.Clear()
                Tipo.DataSource = ds1
                Tipo.DataTextField = "MEANING"
                Tipo.DataValueField = "DOMAINVALUE"
                Tipo.DataBind()


                qry = "SELECT  COD_WAREHOUSE, DESCRIPTION FROM DEF_WAREHOUSES"
                Dim ds2 As New DataSet()
                Dim cmd2 As New MySqlCommand(qry, conn)
                Dim adp2 As New MySqlDataAdapter(cmd2)
                adp2.Fill(ds2)
                Bodega.Items.Clear()
                Bodega.DataSource = ds2
                Bodega.DataTextField = "DESCRIPTION"
                Bodega.DataValueField = "COD_WAREHOUSE"
                Bodega.DataBind()


            End Using



            '///////////////////////////// BAW
            Checkboxlist1.Items.Clear()
            Checkboxlist2.Items.Clear()
            Checkboxlist3.Items.Clear()
            Checkboxlist4.Items.Clear()

            For Each li As KeyValuePair(Of String, String) In BawMenu
                If li.Key.Length = 2 And li.Key.Substring(0, 2) = "01" Then
                    lblBaw_1.Text = li.Value
                End If
                If li.Key.Length = 2 And li.Key.Substring(0, 2) = "02" Then
                    lblBaw_2.Text = li.Value
                End If
                If li.Key.Length = 2 And li.Key.Substring(0, 2) = "03" Then
                    lblBaw_3.Text = li.Value
                End If
                If li.Key.Length = 2 And li.Key.Substring(0, 2) = "04" Then
                    lblBaw_4.Text = li.Value
                End If
                If li.Key.Length = 4 And li.Key.Substring(0, 2) = "01" Then
                    Dim temp As New ListItem(li.Value, li.Key)
                    Checkboxlist1.Items.Add(temp)
                End If
                If li.Key.Length = 4 And li.Key.Substring(0, 2) = "02" Then
                    Dim temp As New ListItem(li.Value, li.Key)
                    Checkboxlist2.Items.Add(temp)
                End If
                If li.Key.Length = 4 And li.Key.Substring(0, 2) = "03" Then
                    Dim temp As New ListItem(li.Value, li.Key)
                    Checkboxlist3.Items.Add(temp)
                End If
                If li.Key.Length = 4 And li.Key.Substring(0, 2) = "04" Then
                    Dim temp As New ListItem(li.Value, li.Key)
                    Checkboxlist4.Items.Add(temp)
                End If
            Next



            '///////////////////////////// TIR
            CnnMs = GetConnectionStringFromFile("tir", Server)
            Using conn As New MySqlConnection(CnnMs)

                qry = "SELECT * FROM acceso ORDER BY opcion"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(qry, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                tirPermisos.Items.Clear()
                tirPermisos.DataSource = ds
                tirPermisos.DataTextField = "opcion"
                tirPermisos.DataValueField = "id_opcion"
                tirPermisos.DataBind()

            End Using


        Catch ex As Exception
            'MsgBox(ex.Message, , "lee combos")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try


    End Sub


    Protected Sub ventas_grid_caller(conn As NpgsqlConnection)

        'qry = "SELECT database, pais FROM usuarios_empresas_db WHERE usado_en ILIKE '%maritimo%' AND status = 't' ORDER BY pais"
        qry = "SELECT datname as database, substring(datname from 8 for 2) as pais, " & _
        " '<span><img src=Content/flags/' || lower(substring(datname from 8 for 2)) || '-flag.gif> ' || upper(substring(datname from 8 for 5)) || ' </span>' as flag " & _
        " FROM pg_database WHERE datistemplate = false AND datname ilike 'ventas%' ORDER BY datname;"

        '        " '<img src=" & Chr(34) & "Content/flags/' || lower(substring(datname from 8 for 2)) || '-flag.gif" & Chr(34) & " height=" & Chr(34) & "16" & Chr(34) & " title=" & Chr(34) & "' || upper(substring(datname from 8 for 2)) || '" & Chr(34) & "> ' || upper(substring(datname from 8 for 5)) || ' ' as flag " & _

        '" & Chr(34) & "
        Dim comm As New NpgsqlCommand(qry, conn)
            'conn.Open()
        Dim dataread As NpgsqlDataReader = comm.ExecuteReader
        ventas_grid.DataSource = dataread
        ventas_grid.DataBind()


    End Sub





    Protected Sub gPais_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gPais.SelectedIndexChanged

        cEmpresas.Items.Clear()
        Empresas()

        locode.Items.Clear()
        Ubicaciones()

    End Sub


    'Protected Sub vPaisSel_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles vPaisSel.SelectedIndexChanged

    '    Try

    '        Perfil.Items.Clear()

    '        Using conn_ven As New NpgsqlConnection(conn_db(CnnMs, vPaisSel.SelectedValue, Session("demo")))
    '            qry = "SELECT group_id, name FROM groups WHERE activo = 't' ORDER BY name"
    '            Dim ds As New DataSet()
    '            Dim cmd As New NpgsqlCommand(qry, conn_ven)
    '            Dim adp As New NpgsqlDataAdapter(cmd)
    '            adp.Fill(ds)

    '            Perfil.DataSource = ds
    '            Perfil.DataTextField = "name"
    '            Perfil.DataValueField = "group_id"
    '            Perfil.DataBind()
    '        End Using

    '        Using conn As New NpgsqlConnection(CnnMs)

    '            Dim SolPerm As New SolicitudPermiso()

    '            If Solicitud.Text <> "" Then
    '                qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Solicitud.Text
    '                Dim comm As New NpgsqlCommand(qry, conn)
    '                conn.Open()
    '                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
    '                If dataread.Read() Then
    '                    SolPerm = DeserializePermiso(dataread(8))
    '                End If
    '                dataread.Close()

    '                Try

    '                    For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
    '                        If vPaisSel.SelectedValue = li.Key Then
    '                            Dim perfiles() As String = li.Value
    '                            For Each li2 As String In perfiles
    '                                For Each li1 As ListItem In Perfil.Items
    '                                    If li1.Value = li2 Then
    '                                        li1.Selected = True
    '                                        Exit For
    '                                    End If
    '                                Next
    '                            Next
    '                            Exit For
    '                        End If
    '                    Next

    '                Catch ex As Exception

    '                End Try

    '            End If

    '        End Using

    '    Catch ex As Exception

    '    End Try

    'End Sub



    Protected Sub tEmpresas_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tEmpresas.SelectedIndexChanged

        For Each li As ListItem In tirPermisos.Items
            li.Selected = False
        Next

        tEmpresa.Value = ""

        CnnMs = GetConnectionStringFromFile("aimar", Server)

        Using conn As New NpgsqlConnection(CnnMs)

            Dim SolPerm As New SolicitudPermiso()

            If Solicitud.Text <> "" Then
                qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Solicitud.Text
                Dim comm As New NpgsqlCommand(qry, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    SolPerm = DeserializePermiso(dataread(8))
                End If
                dataread.Close()

                Try
                    tEmpresa.Value = tEmpresas.SelectedValue
                    For Each li As KeyValuePair(Of String, String()) In SolPerm.Tir
                        If tEmpresas.SelectedValue = li.Key Then
                            Dim permisos() As String = li.Value
                            For Each li2 As String In permisos
                                For Each li1 As ListItem In tirPermisos.Items
                                    If li1.Value = li2 Then
                                        li1.Selected = True
                                        Exit For
                                    End If
                                Next
                            Next
                            Exit For
                        End If
                    Next

                Catch ex As Exception

                End Try

            End If

        End Using

    End Sub



    Protected Sub Empresas() 'customer aduanas

        'If Empresa.Items.Count = 0 Or empresa_ant.Text <> gPais.SelectedValue Then

        If cEmpresas.Items.Count = 0 And gPais.SelectedIndex > -1 Then

            empresa_ant.Text = gPais.SelectedValue

            Try

                '///////////////////////////// CUSTOMER
                CnnMs = GetConnectionStringFromFile("customer", Server)
                Using conn As New MySqlConnection(CnnMs)
                    qry = "SELECT id, nombre_empresa FROM empresas_internas WHERE activo = 0 and pais = @pais ORDER BY id"
                    Dim ds As New DataSet()
                    Dim cmd As New MySqlCommand(qry, conn)
                    cmd.Parameters.Add("@pais", MySqlDbType.String).Value = gPais.SelectedValue
                    Dim adp As New MySqlDataAdapter(cmd)
                    adp.Fill(ds)
                    cEmpresas.Items.Clear()
                    cEmpresas.DataSource = ds
                    cEmpresas.DataTextField = "nombre_empresa"
                    cEmpresas.DataValueField = "id"
                    cEmpresas.DataBind()

                End Using


            Catch ex As Exception

            End Try

        End If

    End Sub




    Protected Sub Ubicaciones()

        Try

            If locode.Items.Count = 0 And gPais.SelectedIndex > -1 Then

                locode.Items.Clear()

                CnnMs = GetConnectionStringFromFile("aimar", Server)
                Using conn As New NpgsqlConnection(CnnMs)

                    Dim pais_code As String = gPais.SelectedValue.Substring(0, 2)

                    qry = "SELECT locode, nombre FROM unlocode WHERE pais = @pais ORDER BY nombre"
                    Dim ds1 As New DataSet()
                    Dim cmd1 As New NpgsqlCommand(qry, conn)
                    cmd1.Parameters.Add("@pais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pais_code
                    Dim adp1 As New NpgsqlDataAdapter(cmd1)
                    adp1.Fill(ds1)
                    locode.DataSource = ds1
                    locode.DataTextField = "nombre"
                    locode.DataValueField = "locode"
                    locode.DataBind()

                End Using

            End If

        Catch ex As Exception
            'MsgBox(ex.Message, , "ubicaciones")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

    End Sub



    Protected Sub btnGuardar_Click(sender As Object, e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim json As String = ""
            Dim temp As String = ""
            Dim SolPerm As New SolicitudPermiso()

            CnnMs = GetConnectionStringFromFile("aimar", Server)

            Using conn As New NpgsqlConnection(CnnMs)

                conn.Open()

                If Solicitud.Text <> "" Then
                    qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Solicitud.Text
                    Dim comm As New NpgsqlCommand(qry, conn)

                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        SolPerm = DeserializePermiso(dataread(8))
                    End If
                    dataread.Close()
                End If

                Dim items As New Dictionary(Of String, String)
                items.Clear()
                items.Add("Empleado_id", Empleado_id.SelectedValue)
                items.Add("Usuario", Usuario.Text)
                items.Add("Empleado_nombre", Empleado_nombre.Text)
                items.Add("Dominio", Dominio.Text)
                items.Add("Correo", Correo.SelectedValue)
                items.Add("Chat", Chat.SelectedValue)
                items.Add("Solicitante", Solicitante.Text)
                items.Add("Nuevo", Nuevo.SelectedValue)
                items.Add("Reemplaza", Reemplaza.Text)
                items.Add("Pais", gPais.SelectedValue)
                items.Add("Tipo_usuario", tipo_usuario.SelectedValue)

                temp = ""

                tEmpresas.Items.Clear()
                Dim temp0 As ListItem
                For Each li As ListItem In gEmpresas.Items
                    If li.Selected = True Then
                        temp = temp & li.Value & ","
                        temp0 = New ListItem(li.Text, li.Value)
                        tEmpresas.Items.Add(temp0)
                    End If
                Next

                If tEmpresa.Value <> "" Then
                    tEmpresas.SelectedValue = tEmpresa.Value
                End If
                items.Add("Empresas", temp)
                items.Add("Ubicacion", locode.SelectedValue)
                items.Add("Especiales", Especiales.Text)

                SolPerm.Generales = items


                '//////////////////////////////////// VENTAS / MARITIMO //////////////////////////

                Dim j As Integer = 0
                For Each li As ListItem In Perfil.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next

                Dim perfiles(j - 1) As String
                Dim perfiles_selected As String() = perfiles
                j = 0
                For Each li As ListItem In Perfil.Items
                    If li.Selected = True Then
                        perfiles_selected(j) = li.Value
                        j = j + 1
                    End If
                Next


                Dim ants As Integer = 0

                Dim SolPermTmp As New SolicitudPermiso()
                SolPermTmp.Ventas = New Dictionary(Of String, String())
                If Solicitud.Text <> "" Then

                    Try
                        For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                            If vPaisS.Text <> li.Key Then
                                If li.Key <> "" And li.Value.Length > 0 Then
                                    SolPermTmp.Ventas.Add(li.Key, li.Value)
                                End If
                            Else
                                ants = ants + li.Value.Length
                            End If
                        Next
                    Catch ex As Exception
                        temp = ex.Message
                    End Try

                End If

                SolPerm.Ventas = New Dictionary(Of String, String())
                SolPerm.Ventas = SolPermTmp.Ventas
                'If perfiles_selected.Length > 0 Then
                If vPaisS.Text <> "" Then
                    If ants > 0 Or perfiles_selected.Length > 0 Then
                        SolPerm.Ventas.Add(vPaisS.Text, perfiles_selected)
                    End If
                End If


                '//////////////////////////////////// AEREO //////////////////////////
                SolPerm.Aereo = New PaisNivel
                j = 0
                For Each li As ListItem In aPais.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next
                ReDim SolPerm.Aereo.Pais(j - 1)
                j = 0
                For Each li As ListItem In aPais.Items
                    If li.Selected = True Then
                        SolPerm.Aereo.Pais(j) = li.Value
                        j = j + 1
                    End If
                Next
                SolPerm.Aereo.Nivel = aNivel.SelectedValue


                '//////////////////////////////////// TERRESTRE //////////////////////////
                SolPerm.Terrestre = New PaisNivel
                j = 0
                For Each li As ListItem In tPais.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next
                ReDim SolPerm.Terrestre.Pais(j - 1)
                j = 0
                For Each li As ListItem In tPais.Items
                    If li.Selected = True Then
                        SolPerm.Terrestre.Pais(j) = li.Value
                        j = j + 1
                    End If
                Next
                SolPerm.Terrestre.Nivel = tNivel.SelectedValue


                SolPerm.Customer = New Customer

                '//////////////////////////////////// CUSTOMER MARITIMO //////////////////////////
                SolPerm.Customer.Maritimo = New Maritimo
                SolPerm.Customer.Maritimo.Nivel = cmNivel.SelectedValue

                '//////////////////////////////////// CUSTOMER ADUANAS //////////////////////////
                SolPerm.Customer.Aduanas = New Aduanas
                j = 0
                For Each li As ListItem In cEmpresas.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next
                ReDim SolPerm.Customer.Aduanas.Empresas(j - 1)
                j = 0
                For Each li As ListItem In cEmpresas.Items
                    If li.Selected = True Then
                        SolPerm.Customer.Aduanas.Empresas(j) = li.Value
                        j = j + 1
                    End If
                Next

                If j = 0 Then
                    caNivel.SelectedIndex = -1
                    SolPerm.Customer.Aduanas.Nivel = 0
                Else
                    If caNivel.SelectedValue = "" Then
                        caNivel.SelectedValue = 3
                    End If
                    SolPerm.Customer.Aduanas.Nivel = caNivel.SelectedValue
                End If




                '//////////////////////////////////// CUSTOMER BITACORA //////////////////////////
                SolPerm.Customer.Bitacora = New Bitacora
                SolPerm.Customer.Bitacora.Nivel = cbNivel.SelectedValue


                '//////////////////////////////////// CAJA DE AHORRO //////////////////////////
                SolPerm.Caja = New PaisNivel
                j = 0
                For Each li As ListItem In cPais.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next
                ReDim SolPerm.Caja.Pais(j - 1)
                j = 0
                For Each li As ListItem In cPais.Items
                    If li.Selected = True Then
                        SolPerm.Caja.Pais(j) = li.Value
                        j = j + 1
                    End If
                Next
                SolPerm.Caja.Nivel = cNivel.SelectedValue


                '//////////////////////////////////// SEGUROS //////////////////////////
                SolPerm.Seguros = New Bitacora
                SolPerm.Seguros.Nivel = sNivel.SelectedValue


                '//////////////////////////////////// CATALOGOS //////////////////////////
                SolPerm.Catalogos = New Bitacora
                SolPerm.Catalogos.Nivel = cgNivel.SelectedValue





                '//////////////////////////////////// WMS //////////////////////////
                SolPerm.Wms = New Wms
                j = 0
                For Each li As ListItem In Bodega.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next
                ReDim SolPerm.Wms.Bodegas(j - 1)
                j = 0
                For Each li As ListItem In Bodega.Items
                    If li.Selected = True Then
                        SolPerm.Wms.Bodegas(j) = li.Value
                        j = j + 1
                    End If
                Next
                SolPerm.Wms.Tipo = Tipo.SelectedValue
                SolPerm.Wms.Grupo = Grupo.SelectedValue


                '//////////////////////////////////// BAW //////////////////////////
                SolPerm.Baw = New Dictionary(Of String, String)

                For Each li As ListItem In Checkboxlist1.Items
                    If li.Selected = True Then
                        SolPerm.Baw.Add(li.Value, li.Text)
                    End If
                Next
                For Each li As ListItem In Checkboxlist2.Items
                    If li.Selected = True Then
                        SolPerm.Baw.Add(li.Value, li.Text)
                    End If
                Next
                For Each li As ListItem In Checkboxlist3.Items
                    If li.Selected = True Then
                        SolPerm.Baw.Add(li.Value, li.Text)
                    End If
                Next
                For Each li As ListItem In Checkboxlist4.Items
                    If li.Selected = True Then
                        SolPerm.Baw.Add(li.Value, li.Text)
                    End If
                Next


                'BAW2 este es un ejemplo de 3 niveles
                'SolPerm.BawArr = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))

                'Dim Nivel1 As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))
                'Dim Nivel2 As Dictionary(Of String, Dictionary(Of String, String))
                'Dim Nivel3 As Dictionary(Of String, String)

                ''opcion uno
                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("010101", "uno")
                'Nivel3.Add("010102", "dos")
                'Nivel3.Add("010103", "tres")

                'Nivel2 = New Dictionary(Of String, Dictionary(Of String, String))
                'Nivel2.Add("0101", Nivel3)


                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("010201", "uno")
                'Nivel3.Add("010202", "dos")
                'Nivel3.Add("010203", "tres")

                'Nivel2.Add("0102", Nivel3)

                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("010301", "uno")
                'Nivel3.Add("010302", "dos")
                'Nivel3.Add("010303", "tres")

                'Nivel2.Add("0103", Nivel3)

                'Nivel1 = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))
                'Nivel1.Add("01", Nivel2)


                ''opcion dos
                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("020101", "uno")
                'Nivel3.Add("020102", "dos")
                'Nivel3.Add("020103", "tres")

                'Nivel2 = New Dictionary(Of String, Dictionary(Of String, String))
                'Nivel2.Add("0201", Nivel3)


                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("020201", "uno")
                'Nivel3.Add("020202", "dos")
                'Nivel3.Add("020203", "tres")

                'Nivel2.Add("0202", Nivel3)

                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("020301", "uno")
                'Nivel3.Add("020302", "dos")
                'Nivel3.Add("020303", "tres")

                'Nivel2.Add("0203", Nivel3)

                'Nivel1.Add("02", Nivel2)




                ''opcion tres
                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("030101", "uno")
                'Nivel3.Add("030102", "dos")
                'Nivel3.Add("030103", "tres")

                'Nivel2 = New Dictionary(Of String, Dictionary(Of String, String))
                'Nivel2.Add("0301", Nivel3)

                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("030201", "uno")
                'Nivel3.Add("030202", "dos")
                'Nivel3.Add("030203", "tres")

                'Nivel2.Add("0302", Nivel3)

                'Nivel3 = New Dictionary(Of String, String)
                'Nivel3.Add("030301", "uno")
                'Nivel3.Add("030302", "dos")
                'Nivel3.Add("030303", "tres")

                'Nivel2.Add("0303", Nivel3)

                'Nivel1.Add("03", Nivel2)

                ''opcion 4
                'Nivel3 = New Dictionary(Of String, String)

                'Nivel2 = New Dictionary(Of String, Dictionary(Of String, String))
                'Nivel2.Add("0401", Nivel3)
                'Nivel2.Add("0402", Nivel3)
                'Nivel2.Add("0403", Nivel3)

                'Nivel1.Add("04", Nivel2)


                'SolPerm.BawArr = Nivel1



                '//////////////////////////////////// MANIFIESTOS CR / CRLTF //////////////////////////
                SolPerm.Manifiestos = New Manifiestos
                j = 0
                For Each li As ListItem In mcrNivel.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next

                ReDim SolPerm.Manifiestos.nivelcr(j - 1)

                j = 0
                For Each li As ListItem In mcrNivel.Items
                    If li.Selected = True Then
                        SolPerm.Manifiestos.nivelcr(j) = li.Value
                        j = j + 1
                    End If
                Next


                j = 0
                For Each li As ListItem In mclNivel.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next

                ReDim SolPerm.Manifiestos.nivelcl(j - 1)

                j = 0
                For Each li As ListItem In mclNivel.Items
                    If li.Selected = True Then
                        SolPerm.Manifiestos.nivelcl(j) = li.Value
                        j = j + 1
                    End If
                Next


                '//////////////////////////////////// TIR ////////////////////////// guarda datos

                j = 0
                For Each li As ListItem In tirPermisos.Items
                    If li.Selected = True Then
                        j = j + 1
                    End If
                Next


                If j > 0 And tEmpresa.Value <> "" Then

                    Dim permisos(j - 1) As String
                    Dim permisos_selected As String() = permisos
                    j = 0
                    For Each li As ListItem In tirPermisos.Items
                        If li.Selected = True Then
                            permisos_selected(j) = li.Value
                            j = j + 1
                        End If
                    Next

                    SolPermTmp.Tir = New Dictionary(Of String, String())
                    If Solicitud.Text <> "" Then
                        Try
                            For Each li As KeyValuePair(Of String, String()) In SolPerm.Tir
                                If tEmpresa.Value <> li.Key Then
                                    If li.Key <> "" And li.Value.Length > 0 Then
                                        SolPermTmp.Tir.Add(li.Key, li.Value)
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            temp = ex.Message
                        End Try

                    End If

                    SolPerm.Tir = New Dictionary(Of String, String())
                    SolPerm.Tir = SolPermTmp.Tir
                    If permisos_selected.Length > 0 Then
                        SolPerm.Tir.Add(tEmpresa.Value, permisos_selected)
                    End If


                End If




                '//////////////////////////////////// PLANILLAS //////////////////////////
                SolPerm.Planillas = New Bitacora
                SolPerm.Planillas.Nivel = pNivel.SelectedValue

                '//////////////////////////////////// BITACORA OD //////////////////////////
                SolPerm.Bitacora_od = New Bitacora
                SolPerm.Bitacora_od.Nivel = odNivel.SelectedValue

                '//////////////////////////////////// GRAFICAS ISO //////////////////////////
                SolPerm.Graficas_iso = New Bitacora
                SolPerm.Graficas_iso.Nivel = iNivel.SelectedValue

                '//////////////////////////////////// E-MANIFIESTOS APL  //////////////////////////
                SolPerm.eManifiestos_apl = New Bitacora
                SolPerm.eManifiestos_apl.Nivel = mNivel.SelectedValue


                '////////////////////////////////////////////////////////////////////////////////////
                json = SerializePermiso(SolPerm)
                json = json.Replace("'", "")

                Try
                    'conn.Open()

                    If Solicitud.Text = "" Then
                        qry = "INSERT INTO usuarios_empresas_menu_solicitud (sol_fec, sol_usr, sol_sta, permisos) VALUES (now(), " & Session("OperatorID") & ", 1, '" & json & "') returning sol_no"
                        Dim comm As New NpgsqlCommand(qry, conn)
                        Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                        If dataread.Read() Then
                            Solicitud.Text = dataread(0)
                        End If
                        dataread.Close()
                    Else
                        'sol_sta = " & Estatus.SelectedIndex & ",
                        qry = "UPDATE usuarios_empresas_menu_solicitud SET permisos = '" & json & "' WHERE sol_no = " & Solicitud.Text
                        Dim comm As New NpgsqlCommand(qry, conn)
                        comm.ExecuteNonQuery()
                    End If

                    ventas_grid_caller(conn)
                    conn.Close()

                    msg = "Grabado correctamente sin errores"

                Catch ex As Exception
                    temp = ex.Message
                End Try

            End Using

        Catch ex As Exception
            'MsgBox(ex.Message, , "btnGuardar_Click")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

    End Sub


    Protected Sub btnEnviar_Click(sender As Object, e As System.EventArgs) Handles btnEnviar.Click

        Try

            CnnMs = GetConnectionStringFromFile("aimar", Server)

            Using conn As New NpgsqlConnection(CnnMs)
                conn.Open()
                qry = "UPDATE usuarios_empresas_menu_solicitud SET sol_sta = 3, env_fec = now(), env_usr = " & Session("OperatorID") & " WHERE sol_no = " & Solicitud.Text
                Dim comm As New NpgsqlCommand(qry, conn)
                comm.ExecuteNonQuery()
                limpia_datos()
            End Using

        Catch ex As Exception
            'MsgBox(ex.Message, , "btnEnviar_Click")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

    End Sub



    Protected Sub btnProcesar_Click(sender As Object, e As System.EventArgs) Handles btnProcesar.Click

        Try

            Dim SolPerm As New SolicitudPermiso()
            Dim founded As Boolean = False

            CnnMs = GetConnectionStringFromFile("aimar", Server)

            Using conn As New NpgsqlConnection(CnnMs)
                qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Solicitud.Text
                Dim comm As New NpgsqlCommand(qry, conn)
                conn.Open()
                Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    founded = True
                    SolPerm = DeserializePermiso(dataread(8))
                End If
                dataread.Close()
                conn.Close()

            End Using


            If founded = True Then
                procesa_datos(SolPerm)
            End If

        Catch ex As Exception
            'MsgBox(ex.Message, , "btnProcesar_Click")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
            Exit Sub
        End Try

        'Response.Redirect("Solicitudes.aspx")

    End Sub


    Private Structure PerfilDat
        Public user_id As Integer
        Public stat As String
        Public group As Integer
    End Structure

    Private Structure CustomerDat
        Public numero As Integer
        Public id_empresa As Integer
        Public acceso_aduana As Boolean
        Public stat As String
    End Structure

    Private Structure BodegaDat
        Public bod As String
        Public stat As String
        Public newVal As String
    End Structure

    Protected Sub procesa_datos(SolPerm As SolicitudPermiso)

        CnnMs = GetConnectionStringFromFile("aimar", Server)
        Dim conn_mas As New NpgsqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("aereo", Server)
        Dim conn_aer As New MySqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("terrestre", Server)
        Dim conn_ter As New MySqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("caja", Server)
        Dim conn_caj As New MySqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("customer", Server)
        Dim conn_cus As New MySqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("wms", Server)
        Dim conn_wms As New MySqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("baw", Server)
        Dim conn_baw As New MySqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("ventas", Server) & "cr"
        Dim conn_man_cr As New NpgsqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("ventas", Server) & "crltf"
        Dim conn_man_crltf As New NpgsqlConnection(CnnMs)
        CnnMs = GetConnectionStringFromFile("tir", Server)
        Dim conn_tir As New MySqlConnection(CnnMs)

        Dim trns_mas As NpgsqlTransaction = Nothing
        Dim trns_aer As MySqlTransaction = Nothing
        Dim trns_ter As MySqlTransaction = Nothing
        Dim trns_caj As MySqlTransaction = Nothing
        Dim trns_cus As MySqlTransaction = Nothing
        Dim trns_wms As MySqlTransaction = Nothing
        Dim trns_baw As MySqlTransaction = Nothing
        Dim trns_man_cr As NpgsqlTransaction = Nothing
        Dim trns_man_crltf As NpgsqlTransaction = Nothing
        Dim trns_tir As MySqlTransaction = Nothing

        Dim conn_ven() As NpgsqlConnection
        Dim trns_ven() As NpgsqlTransaction

        Dim i As Integer = 0

        ReDim conn_ven(SolPerm.Ventas.Count)
        ReDim trns_ven(SolPerm.Ventas.Count)

        For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
            Dim db_pais As String = li.Key

            CnnMs = GetConnectionStringFromFile("ventas", Server) & Mid(db_pais, 8)

            conn_ven(i) = New NpgsqlConnection(CnnMs)
            i = i + 1
        Next

        Dim id_usuario_ As Integer = 0
        Dim level_ As Integer = 0
        Dim pw_name_, pw_gecos_, pais_, pw_passwd_, dominio_, tipo_usuario_, pw_correo_, locode_ As String

        pw_name_ = ""
        pw_gecos_ = ""
        pais_ = ""
        pw_passwd_ = ""
        dominio_ = ""
        tipo_usuario_ = ""
        pw_correo_ = ""
        locode_ = ""

        Try

            For Each li As KeyValuePair(Of String, String) In SolPerm.Generales
                If (li.Key = "Empleado_id") Then
                    id_usuario_ = li.Value
                End If
                If (li.Key = "Usuario") Then
                    pw_name_ = li.Value
                End If
                If (li.Key = "Empleado_nombre") Then
                    pw_gecos_ = li.Value
                End If
                If (li.Key = "Dominio") Then
                    dominio_ = li.Value
                End If
                If (li.Key = "Pais") Then
                    pais_ = li.Value
                End If
                If (li.Key = "Tipo_usuario") Then
                    tipo_usuario_ = li.Value
                End If
                If (li.Key = "Ubicacion") Then
                    locode_ = li.Value
                End If
            Next

            conn_mas.Open()
            trns_mas = conn_mas.BeginTransaction()

            Dim comm As NpgsqlCommand
            Dim dataread As NpgsqlDataReader

            If id_usuario_ = 0 Then

                qry = "INSERT INTO usuarios_empresas (pw_name, pw_gecos, pais, dominio, tipo_usuario, locode, level, pw_activo, modificado) VALUES (@pw_name, @pw_gecos, @pais, @dominio, @tipo_usuario, @locode, @level, 1, now()) RETURNING id_usuario"
                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@pw_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_
                comm.Parameters.Add("@pw_gecos", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_gecos_
                comm.Parameters.Add("@pais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pais_
                comm.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = dominio_
                comm.Parameters.Add("@tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = tipo_usuario_
                comm.Parameters.Add("@locode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = locode_
                comm.Parameters.Add("@level", NpgsqlTypes.NpgsqlDbType.Integer).Value = SolPerm.Catalogos.Nivel 'Catalogos
                dataread = comm.ExecuteReader
                If dataread.Read() Then
                    id_usuario_ = dataread(0)
                End If
                dataread.Close()

            Else

                qry = "SELECT pw_name, pw_gecos, pais, pw_passwd, dominio, tipo_usuario, pw_correo, locode, level FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_
                dataread = comm.ExecuteReader
                If dataread.Read() Then

                    If pw_name_ = "" Then
                        pw_name_ = dataread(0)
                    End If

                    If pw_gecos_ = "" Then
                        pw_gecos_ = dataread(1)
                    End If

                    If pais_ = "" Then
                        pais_ = dataread(2)
                    End If

                    pw_passwd_ = dataread(3)

                    If dominio_ = "" Then
                        dominio_ = dataread(4)
                    End If

                    If tipo_usuario_ = "" Then
                        tipo_usuario_ = dataread(5)
                    End If

                    pw_correo_ = dataread(6)

                    If locode_ = "" Then
                        locode_ = dataread(7)
                    End If

                    If level_ = 0 Then
                        level_ = dataread(8)
                    End If
                End If

                dataread.Close()


                qry = "UPDATE usuarios_empresas SET pw_name=@pw_name, pw_gecos=@pw_gecos, pais=@pais, dominio=@dominio, tipo_usuario=@tipo_usuario, locode=@locode, "
                If SolPerm.Catalogos.Nivel <> "" Then
                    qry = qry & " level=@level, "
                End If
                qry = qry & " modificado=now() WHERE id_usuario = @id_usuario"

                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_
                comm.Parameters.Add("@pw_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_
                comm.Parameters.Add("@pw_gecos", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_gecos_
                comm.Parameters.Add("@pais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pais_
                comm.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = dominio_
                comm.Parameters.Add("@tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = tipo_usuario_
                comm.Parameters.Add("@locode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = locode_

                '//////////////////////////////////// CATALOGOS //////////////////////////                            
                If SolPerm.Catalogos.Nivel <> "" Then
                    comm.Parameters.Add("@level", NpgsqlTypes.NpgsqlDbType.Integer).Value = CInt(SolPerm.Catalogos.Nivel)
                End If

                comm.ExecuteNonQuery()

            End If



            '//////////////////////////////////// SEGUROS //////////////////////////                            

            If SolPerm.Seguros.Nivel <> "" Then

                qry = "SELECT id_usuario, id_tipo_usuario FROM detalle_tipos_usuario WHERE id_usuario = @id_usuario ORDER BY id_tipo_usuario DESC"
                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_usuario_

                'Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                dataread = comm.ExecuteReader
                If dataread.Read() Then
                    qry = "UPDATE detalle_tipos_usuario SET id_tipo_usuario=@id_tipo_usuario WHERE id_usuario = @id_usuario"
                Else
                    qry = "INSERT INTO detalle_tipos_usuario VALUES (@id_usuario, @id_tipo_usuario)"
                End If
                dataread.Close()

                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_usuario_
                comm.Parameters.Add("@id_tipo_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = SolPerm.Seguros.Nivel
                comm.ExecuteNonQuery()

            End If


        Catch ex As Exception
            MsgBox(ex.Message, , "procesa datos generales")
            If conn_mas.State = ConnectionState.Open Then
                trns_mas.Rollback()
                conn_mas.Close()
            End If
            Exit Sub
        End Try



        '//////////////////////////////////// VENTAS / MARITIMO //////////////////////////
        Try
            i = 0

            For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas

                Dim db_pais As String = li.Key

                '////////OPEN VENTAS DB                
                Try
                    conn_ven(i).Open()
                    trns_ven(i) = conn_ven(i).BeginTransaction()
                Catch ex As Exception
                    MsgBox(ex.Message, , "ventas / maritimo")
                End Try

                If conn_ven(i).State = ConnectionState.Open Then

                    qry = "UPDATE referencias_usuarios SET activo='f' WHERE id_nuevo=@id_nuevo AND bd=@bd"
                    Dim comm As New NpgsqlCommand(qry, conn_mas)
                    comm.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_usuario_
                    comm.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = db_pais
                    comm.ExecuteNonQuery()


                    Dim ArrPerfil As New List(Of PerfilDat)
                    ArrPerfil.Clear()
                    Dim tmp As New PerfilDat

                    '////////LEER LAS REFERENCIAS 
                    qry = "SELECT id_nuevo, id_anterior, bd, activo FROM referencias_usuarios WHERE id_nuevo = @id_nuevo AND bd = @bd ORDER BY bd"
                    Dim comm_ref As New NpgsqlCommand(qry, conn_mas)
                    comm_ref.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_
                    comm_ref.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = db_pais
                    Dim dataread_ref As NpgsqlDataReader = comm_ref.ExecuteReader
                    If dataread_ref.HasRows Then
                        While dataread_ref.Read()


                            Dim comm_ven As NpgsqlCommand
                            qry = "UPDATE users SET activo='f' WHERE user_id=@user_id"
                            comm_ven = New NpgsqlCommand(qry, conn_ven(i))
                            comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = dataread_ref(1)
                            comm_ven.ExecuteNonQuery()

                            '////////LEER VENTAS
                            'qry = "SELECT user_id, grupo FROM users WHERE user_id = @user_id AND activo = 't' ORDER BY grupo"
                            qry = "SELECT user_id, grupo FROM users WHERE user_id = @user_id ORDER BY grupo"
                            comm_ven = New NpgsqlCommand(qry, conn_ven(i))
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
                    For Each perfil_new As String In li.Value
                        founded = False
                        For Each Perfil_ant As PerfilDat In ArrPerfil
                            If Perfil_ant.group = perfil_new Then
                                founded = True
                                tmp.group = Perfil_ant.group
                                tmp.stat = "U"
                                tmp.user_id = Perfil_ant.user_id
                                ArrPerfil2.Add(tmp)
                                Exit For
                            End If
                        Next

                        If founded = False Then
                            tmp.group = perfil_new
                            tmp.stat = "N"
                            tmp.user_id = 0
                            ArrPerfil2.Add(tmp)
                        End If
                    Next



                    For Each Perfil As PerfilDat In ArrPerfil2

                        If Perfil.stat = "N" Then

                            'ventas
                            qry = "INSERT INTO users (login_name, user_name, password, grupo, activo, email) VALUES (@login_name, @user_name, @password, @grupo, 't', @email) returning user_id"
                            Dim comm_ven As New NpgsqlCommand(qry, conn_ven(i))
                            comm_ven.Parameters.Add("@login_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_
                            comm_ven.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_gecos_
                            comm_ven.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_passwd_
                            comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.group
                            comm_ven.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_ & "@" & dominio_
                            Dim dataread_ven As NpgsqlDataReader = comm_ven.ExecuteReader
                            If dataread_ven.Read() Then
                                Perfil.user_id = dataread_ven(0)
                            End If
                            dataread_ven.Close()

                            'referencias
                            qry = "INSERT INTO referencias_usuarios (id_nuevo, id_anterior, bd, dominio, activo) VALUES (@id_nuevo, @id_anterior, @bd, @dominio, 't')"
                            Dim comm_ref1 As New NpgsqlCommand(qry, conn_mas)
                            comm_ref1.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_usuario_
                            comm_ref1.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                            comm_ref1.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = db_pais
                            comm_ref1.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = dominio_
                            comm_ref1.ExecuteNonQuery()

                        End If

                        If Perfil.stat = "U" Then
                            qry = "UPDATE users SET login_name=@login_name, user_name=@user_name, password=@password, grupo=@grupo, activo='t', email=@email WHERE user_id=@user_id AND grupo=@grupo"
                            Dim comm_ven As New NpgsqlCommand(qry, conn_ven(i))
                            comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                            comm_ven.Parameters.Add("@login_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_
                            comm_ven.Parameters.Add("@user_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_gecos_
                            comm_ven.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_passwd_
                            comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.group
                            comm_ven.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_ & "@" & dominio_
                            comm_ven.ExecuteNonQuery()

                            qry = "UPDATE referencias_usuarios SET dominio=@dominio, activo='t' WHERE id_nuevo=@id_nuevo AND id_anterior=@id_anterior AND bd=@bd"
                            Dim comm_ref1 As New NpgsqlCommand(qry, conn_mas)
                            comm_ref1.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_usuario_
                            comm_ref1.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                            comm_ref1.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = db_pais
                            comm_ref1.Parameters.Add("@dominio", NpgsqlTypes.NpgsqlDbType.Varchar).Value = dominio_
                            comm_ref1.ExecuteNonQuery()
                        End If

                        'If Perfil.stat = "A" Then  no es necesario ya que al principio da de baja a todos
                        '    qry = "UPDATE users SET activo='f' WHERE user_id=@user_id AND grupo=@grupo"
                        '    Dim comm_ven As New NpgsqlCommand(qry, conn_ven(i))
                        '    comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                        '    comm_ven.Parameters.Add("@grupo", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.group
                        '    comm_ven.ExecuteNonQuery()

                        '    qry = "UPDATE referencias_usuarios SET activo='f' WHERE id_nuevo=@id_nuevo AND id_anterior=@id_anterior AND bd=@bd"
                        '    Dim comm_ref1 As New NpgsqlCommand(qry, conn_mas)
                        '    comm_ref1.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Integer).Value = id_usuario_
                        '    comm_ref1.Parameters.Add("@id_anterior", NpgsqlTypes.NpgsqlDbType.Integer).Value = Perfil.user_id
                        '    comm_ref1.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = db_pais
                        '    comm_ref1.ExecuteNonQuery()
                        'End If

                    Next

                End If

                i = i + 1

            Next

        Catch ex As Exception
            MsgBox(ex.Message, , "procesa datos ventas / maritimo")
            If conn_mas.State = ConnectionState.Open Then
                trns_mas.Rollback()
                conn_mas.Close()
            End If

            i = 0
            For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas

                If conn_ven(i).State = ConnectionState.Open Then
                    trns_ven(i).Rollback()
                    conn_ven(i).Close()
                End If

                i = i + 1
            Next

            Exit Sub
        End Try



        Try
            '//////////////////////////////////// AEREO //////////////////////////

            Dim countries As String = ""
            Dim j As Integer = 0
            For j = 0 To SolPerm.Aereo.Pais.Length - 1
                If countries <> "" Then
                    countries = countries & ","
                End If
                countries = countries & Comillas & SolPerm.Aereo.Pais(j) & Comillas
            Next

            If countries <> "" And SolPerm.Aereo.Nivel <> "" Then

                qry = "SELECT  OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate FROM Operators WHERE OperatorID = @OperatorID"
                Dim comm1 As New MySqlCommand(qry, conn_aer)
                comm1.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = id_usuario_

                conn_aer.Open()
                trns_aer = conn_aer.BeginTransaction()

                Dim dataread As MySqlDataReader = comm1.ExecuteReader
                If dataread.Read() Then
                    qry = "UPDATE Operators SET Login=@login, FirstName=@firstname, LastName=@lastname, Email=@email, Phone=@phone, Position=@position, OperatorLevel=@level, Countries=@countries, Sign=@sign WHERE OperatorID = @OperatorID"
                Else
                    qry = "INSERT INTO Operators (OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate, CreatedTime, StartTime, FinishTime) VALUES (@OperatorID, @login, @firstname, @lastname, @email, @phone, @position, @level, @countries, 1, @sign, CURDATE(), CURTIME(), CURTIME(), CURTIME())"
                End If
                dataread.Close()

                Dim comm As New MySqlCommand(qry, conn_aer)
                comm.Parameters.Add("@OperatorID", MySqlDbType.String).Value = id_usuario_
                comm.Parameters.Add("@login", MySqlDbType.String).Value = pw_name_
                comm.Parameters.Add("@firstname", MySqlDbType.String).Value = pw_gecos_
                comm.Parameters.Add("@lastname", MySqlDbType.String).Value = pw_gecos_
                comm.Parameters.Add("@email", MySqlDbType.String).Value = pw_name_ & "@" & dominio_
                comm.Parameters.Add("@phone", MySqlDbType.String).Value = ""
                comm.Parameters.Add("@position", MySqlDbType.String).Value = ""
                comm.Parameters.Add("@level", MySqlDbType.String).Value = SolPerm.Aereo.Nivel
                comm.Parameters.Add("@countries", MySqlDbType.String).Value = countries
                comm.Parameters.Add("@sign", MySqlDbType.String).Value = pw_gecos_
                comm.ExecuteNonQuery()

            End If

        Catch ex As Exception
            MsgBox(ex.Message, , "procesa datos aereo")
            If conn_mas.State = ConnectionState.Open Then
                trns_mas.Rollback()
                conn_mas.Close()
            End If

            i = 0
            For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                If conn_ven(i).State = ConnectionState.Open Then
                    trns_ven(i).Rollback()
                    conn_ven(i).Close()
                End If
                i = i + 1
            Next

            If conn_aer.State = ConnectionState.Open Then
                trns_aer.Rollback()
                conn_aer.Close()
            End If

            Exit Sub

        End Try






        Try
            '//////////////////////////////////// TERRESTRE //////////////////////////                        
            Dim countries As String = ""
            Dim j As Integer = 0
            For j = 0 To SolPerm.Terrestre.Pais.Length - 1
                If countries <> "" Then
                    countries = countries & ","
                End If
                countries = countries & Comillas & SolPerm.Terrestre.Pais(j) & Comillas
            Next

            If countries <> "" And SolPerm.Terrestre.Nivel <> "" Then

                qry = "SELECT  OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate FROM Operators WHERE OperatorID = @OperatorID"
                Dim comm1 As New MySqlCommand(qry, conn_ter)
                comm1.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = id_usuario_

                conn_ter.Open()
                trns_ter = conn_ter.BeginTransaction()

                Dim dataread As MySqlDataReader = comm1.ExecuteReader
                If dataread.Read() Then
                    qry = "UPDATE Operators SET Login=@login, FirstName=@firstname, LastName=@lastname, Email=@email, Phone=@phone, Position=@position, OperatorLevel=@level, Countries=@countries, Sign=@sign WHERE OperatorID = @OperatorID"
                Else
                    qry = "INSERT INTO Operators (OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate, CreatedTime, StartTime, FinishTime) VALUES (@OperatorID, @login, @firstname, @lastname, @email, @phone, @position, @level, @countries, 1, @sign, CURDATE(), CURTIME(), CURTIME(), CURTIME())"
                End If
                dataread.Close()

                Dim comm As New MySqlCommand(qry, conn_ter)
                comm.Parameters.Add("@OperatorID", MySqlDbType.String).Value = id_usuario_
                comm.Parameters.Add("@login", MySqlDbType.String).Value = pw_name_
                comm.Parameters.Add("@firstname", MySqlDbType.String).Value = pw_gecos_
                comm.Parameters.Add("@lastname", MySqlDbType.String).Value = pw_gecos_
                comm.Parameters.Add("@email", MySqlDbType.String).Value = pw_name_ & "@" & dominio_
                comm.Parameters.Add("@phone", MySqlDbType.String).Value = ""
                comm.Parameters.Add("@position", MySqlDbType.String).Value = ""
                comm.Parameters.Add("@level", MySqlDbType.String).Value = SolPerm.Aereo.Nivel
                comm.Parameters.Add("@countries", MySqlDbType.String).Value = countries
                comm.Parameters.Add("@sign", MySqlDbType.String).Value = pw_gecos_
                comm.ExecuteNonQuery()
            End If

        Catch ex As Exception

            MsgBox(ex.Message, , "procesa datos terrestre")

            If conn_mas.State = ConnectionState.Open Then
                trns_mas.Rollback()
                conn_mas.Close()
            End If

            i = 0
            For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                If conn_ven(i).State = ConnectionState.Open Then
                    trns_ven(i).Rollback()
                    conn_ven(i).Close()
                End If
                i = i + 1
            Next

            If conn_aer.State = ConnectionState.Open Then
                trns_aer.Rollback()
                conn_aer.Close()
            End If

            If conn_ter.State = ConnectionState.Open Then
                trns_ter.Rollback()
                conn_ter.Close()
            End If

            Exit Sub

        End Try









        Try

            '//////////////////////////////////// CUSTOMER //////////////////////////

            Dim ArrCustomer As New List(Of CustomerDat)
            ArrCustomer.Clear()
            Dim tmp As New CustomerDat

            Dim acceso_aduana As Boolean = False
            Dim acceso_bitacora As Boolean = False

            Dim puesto_ As String = ""
            Dim ip_ As String = ""
            Dim nivel_aduana As Integer
            Dim nivel_bitacora As Integer
            Dim nivel_maritimo As Integer
            qry = "SELECT numero, id_usuario_empresa, id_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, locode, " & _
            "acceso_aduana, acceso_apl, permisos, fecha_ingreso, fecha_desactiva, modificado, activo, nivel_dua, nivel_bit_apl, psw, acceso_maritimo, locode " & _
            "FROM usuarios WHERE id_usuario_empresa = @id_usuario_empresa AND borrado = 0 ORDER BY numero"

            Dim comm As New MySqlCommand(qry, conn_cus)

            comm.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = id_usuario_
            conn_cus.Open()

            trns_cus = conn_cus.BeginTransaction()

            Dim dataread As MySqlDataReader = comm.ExecuteReader
            While dataread.Read()

                puesto_ = dataread(6)
                ip_ = dataread(7)

                acceso_aduana = dataread(10)
                acceso_bitacora = dataread(11)

                nivel_aduana = dataread(17)
                nivel_bitacora = dataread(18)
                nivel_maritimo = dataread(20)

                'If acceso_aduana = 1 Then
                tmp.numero = dataread(0)
                tmp.id_empresa = dataread(2)
                tmp.acceso_aduana = dataread(10)
                tmp.stat = "A"
                'caNivel.SelectedValue = SolPerm.Customer.Aduanas.Nivel
                ArrCustomer.Add(tmp)
                'End If

            End While

            dataread.Close()



            Dim ArrCustomer2 As New List(Of CustomerDat)
            ArrCustomer2.Clear()

            Dim j As Integer = 0
            For j = 0 To SolPerm.Customer.Aduanas.Empresas.Length - 1 'recorre las empresas seleccionadas en solicitud

                tmp.numero = 0
                tmp.id_empresa = SolPerm.Customer.Aduanas.Empresas(j)
                tmp.acceso_aduana = 1
                tmp.stat = "N"

                For Each Aduanas As CustomerDat In ArrCustomer
                    If Aduanas.id_empresa = SolPerm.Customer.Aduanas.Empresas(j) Then ' si ya existe alguna empresa guardada
                        tmp.numero = Aduanas.numero
                        tmp.acceso_aduana = Aduanas.acceso_aduana
                        tmp.stat = "U"
                        Exit For
                    End If
                Next

                ArrCustomer2.Add(tmp)
            Next

            Dim founded As Boolean
            For Each Aduanas As CustomerDat In ArrCustomer 'recorre las empresas anteriores y las agrega al arreglo de nuevas
                founded = False
                For Each AduanasNew As CustomerDat In ArrCustomer2 'recorre las empresas nuevas para evitar duplicados
                    If Aduanas.numero = AduanasNew.numero Then
                        founded = True
                    End If
                Next

                If founded = False Then
                    tmp.numero = Aduanas.numero
                    tmp.id_empresa = Aduanas.id_empresa
                    tmp.acceso_aduana = Aduanas.acceso_aduana
                    tmp.stat = Aduanas.stat
                    ArrCustomer2.Add(tmp)
                End If

            Next



            If SolPerm.Customer.Aduanas.Nivel = "" Then
                SolPerm.Customer.Aduanas.Nivel = "0"
            End If

            If SolPerm.Customer.Bitacora.Nivel = "" Then
                SolPerm.Customer.Bitacora.Nivel = "0"
            End If

            If SolPerm.Customer.Maritimo.Nivel = "" Then
                SolPerm.Customer.Maritimo.Nivel = "0"
            End If


            For Each Aduanas As CustomerDat In ArrCustomer2 'recorre el arreglo nuevo completo con lo anterior y lon nuevo

                If Aduanas.stat = "A" Then

                    qry = "UPDATE usuarios SET acceso_aduana=0, nivel_dua=0, permisos='', "
                    If SolPerm.Customer.Bitacora.Nivel = "0" And SolPerm.Customer.Maritimo.Nivel = "0" Then
                        qry = qry & " activo=1, fecha_desactiva=NOW(), user_desactiva=@user_desactiva  "
                    Else
                        qry = qry & " id_pais=@id_pais, nombre=@nombre, nombrefull=@nombrefull, puesto=@puesto, direccion_ip=@direccion_ip, email=@email, psw=@psw, " & _
                        " activo=0, nivel=@nivel, acceso_apl=@acceso_apl, nivel_bit_apl=@nivel_bit_apl, acceso_maritimo=@acceso_maritimo, " & _
                        " locode=@locode, borrado=0, user_modifica=@user_desactiva, modificado=now() "
                    End If
                    qry = qry & " WHERE numero=@numero"

                End If

                If Aduanas.stat = "N" Then
                    qry = "INSERT INTO usuarios (numero, id_usuario_empresa, id_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, psw, " & _
                    " fecha_ingreso, activo, nivel, user_input, acceso_aduana, nivel_dua, acceso_apl, nivel_bit_apl, acceso_maritimo, dt_readonly, locode) " & _
                    " VALUES (null, @id_usuario_empresa, @id_empresa, @id_pais, @nombre, @nombrefull, @puesto, @direccion_ip, @email, @psw, " & _
                    " NOW(), 0, @nivel, @user_input, @acceso_aduana, @nivel_dua, @acceso_apl, @nivel_bit_apl, @acceso_maritimo, 0, @locode)"
                End If

                If Aduanas.stat = "U" Then
                    qry = "UPDATE usuarios SET id_pais=@id_pais, nombre=@nombre, nombrefull=@nombrefull, puesto=@puesto, direccion_ip=@direccion_ip, email=@email, psw=@psw, " & _
                    " activo = 0, nivel = @nivel, acceso_aduana=@acceso_aduana, nivel_dua=@nivel_dua, acceso_apl=@acceso_apl, nivel_bit_apl=@nivel_bit_apl, acceso_maritimo=@acceso_maritimo, " & _
                    " locode=@locode, borrado=0, user_modifica=@user_desactiva, modificado=now() WHERE numero=@numero"
                End If

                Dim comm_cus As New MySqlCommand(qry, conn_cus)
                comm_cus.Parameters.Add("@numero", MySqlDbType.Int16).Value = Aduanas.numero
                comm_cus.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = id_usuario_
                comm_cus.Parameters.Add("@id_empresa", MySqlDbType.Int16).Value = Aduanas.id_empresa
                comm_cus.Parameters.Add("@id_pais", MySqlDbType.String).Value = pais_
                comm_cus.Parameters.Add("@nombre", MySqlDbType.String).Value = pw_name_
                comm_cus.Parameters.Add("@nombrefull", MySqlDbType.String).Value = pw_gecos_
                comm_cus.Parameters.Add("@puesto", MySqlDbType.String).Value = puesto_
                comm_cus.Parameters.Add("@direccion_ip", MySqlDbType.String).Value = ip_
                comm_cus.Parameters.Add("@email", MySqlDbType.String).Value = pw_name_ & "@" & dominio_
                comm_cus.Parameters.Add("@psw", MySqlDbType.String).Value = pw_passwd_
                comm_cus.Parameters.Add("@nivel", MySqlDbType.Int16).Value = 6
                comm_cus.Parameters.Add("@user_input", MySqlDbType.Int16).Value = Session("OperatorID")
                comm_cus.Parameters.Add("@locode", MySqlDbType.String).Value = locode_
                comm_cus.Parameters.Add("@user_desactiva", MySqlDbType.Int32).Value = Session("OperatorID")

                '//////////////////////////////////// CUSTOMER ADUANAS //////////////////////////
                If SolPerm.Customer.Aduanas.Nivel = "0" Then
                    comm_cus.Parameters.Add("@acceso_aduana", MySqlDbType.Int16).Value = 0
                Else
                    comm_cus.Parameters.Add("@acceso_aduana", MySqlDbType.Int16).Value = 1
                End If
                comm_cus.Parameters.Add("@nivel_dua", MySqlDbType.Int16).Value = CInt(SolPerm.Customer.Aduanas.Nivel)

                '//////////////////////////////////// CUSTOMER BITACORA //////////////////////////                                        
                If SolPerm.Customer.Bitacora.Nivel = "0" Then
                    comm_cus.Parameters.Add("@acceso_apl", MySqlDbType.Int16).Value = 0
                Else
                    comm_cus.Parameters.Add("@acceso_apl", MySqlDbType.Int16).Value = 1
                End If
                comm_cus.Parameters.Add("@nivel_bit_apl", MySqlDbType.Int16).Value = CInt(SolPerm.Customer.Bitacora.Nivel)

                '//////////////////////////////////// CUSTOMER MARITIMO //////////////////////////                
                comm_cus.Parameters.Add("@acceso_maritimo", MySqlDbType.Int16).Value = CInt(SolPerm.Customer.Maritimo.Nivel)

                comm_cus.ExecuteNonQuery()

            Next

        Catch ex As Exception

            MsgBox(ex.Message, , "procesa datos customer")

            If conn_mas.State = ConnectionState.Open Then
                trns_mas.Rollback()
                conn_mas.Close()
            End If

            i = 0
            For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                If conn_ven(i).State = ConnectionState.Open Then
                    trns_ven(i).Rollback()
                    conn_ven(i).Close()
                End If
                i = i + 1
            Next

            If conn_aer.State = ConnectionState.Open Then
                trns_aer.Rollback()
                conn_aer.Close()
            End If

            If conn_ter.State = ConnectionState.Open Then
                trns_ter.Rollback()
                conn_ter.Close()
            End If

            If conn_cus.State = ConnectionState.Open Then
                trns_cus.Rollback()
                conn_cus.Close()
            End If

            Exit Sub

        End Try








        'Try
        '    '//////////////////////////////////// CAJA DE AHORRO //////////////////////////    tenia problemas la base de datos proba nuevamente  

        '    Dim val_bolean(7) As Integer
        '    Dim j As Integer = 0
        '    For j = 0 To SolPerm.Caja.Pais.Length - 1
        '        Select Case SolPerm.Caja.Pais(j)
        '            Case "BZ"
        '                val_bolean(0) = 1
        '            Case "CR"
        '                val_bolean(1) = 1
        '            Case "SV"
        '                val_bolean(2) = 1
        '            Case "GT"
        '                val_bolean(3) = 1
        '            Case "HN"
        '                val_bolean(4) = 1
        '            Case "NI"
        '                val_bolean(5) = 1
        '            Case "PA"
        '                val_bolean(6) = 1
        '        End Select
        '    Next

        '    'Dim comm_caj As MySqlCommand

        '    qry = "SELECT * FROM usuarios WHERE id_master = @id_master"
        '    Dim comm_caj1 As New MySqlCommand(qry, conn_caj)
        '    comm_caj1 = New MySqlCommand(qry, conn_caj)
        '    comm_caj1.Parameters.Add("@id_master", MySqlDbType.Int32).Value = id_usuario_

        '    conn_caj.Open()
        '    trns_caj = conn_caj.BeginTransaction()

        '    Dim dataread_caj As MySqlDataReader = comm_caj1.ExecuteReader
        '    If dataread_caj.Read() Then
        '        qry = "UPDATE usuarios SET nombres=@nombres, email=@email, username=@username, password=@password, idpais=@idpais, rol_id=@rol_id, gt=@gt, bz=@bz, sv=@sv, hn=@hn, ni=@ni, cr=@cr, pa=@pa, activo=@activo WHERE id_master=@id_master"
        '    Else
        '        If SolPerm.Caja.Pais.Length = 0 Then 'si no selecciono nada no inserta
        '            dataread_caj.Close()
        '            Exit Try
        '        End If
        '        qry = "INSERT INTO usuarios (user_id, nombres, email, username, password, fecha_ingreso, activo, idpais, gt, bz, sv, hn, ni, cr, pa, id_master, rol_id) VALUES (null, @nombres, @email, @username, @password, now(), 1, @idpais, @gt, @bz, @sv, @hn, @ni, @cr, @pa, @id_master, @rol_id)"
        '    End If
        '    dataread_caj.Close()

        '    'comm_caj = New MySqlCommand(qry, conn_caj)
        '    Dim comm_caj As New MySqlCommand(qry, conn_caj)
        '    comm_caj.Parameters.Add("@id_master", MySqlDbType.Int16).Value = id_usuario_
        '    comm_caj.Parameters.Add("@nombres", MySqlDbType.String).Value = pw_gecos_
        '    comm_caj.Parameters.Add("@email", MySqlDbType.String).Value = pw_name_ & "@" & dominio_
        '    comm_caj.Parameters.Add("@username", MySqlDbType.String).Value = pw_name_
        '    comm_caj.Parameters.Add("@password", MySqlDbType.String).Value = pw_passwd_
        '    comm_caj.Parameters.Add("@idpais", MySqlDbType.String).Value = pais_
        '    comm_caj.Parameters.Add("@bz", MySqlDbType.Int16).Value = val_bolean(0)
        '    comm_caj.Parameters.Add("@cr", MySqlDbType.Int16).Value = val_bolean(1)
        '    comm_caj.Parameters.Add("@sv", MySqlDbType.Int16).Value = val_bolean(2)
        '    comm_caj.Parameters.Add("@gt", MySqlDbType.Int16).Value = val_bolean(3)
        '    comm_caj.Parameters.Add("@hn", MySqlDbType.Int16).Value = val_bolean(4)
        '    comm_caj.Parameters.Add("@ni", MySqlDbType.Int16).Value = val_bolean(5)
        '    comm_caj.Parameters.Add("@pa", MySqlDbType.Int16).Value = val_bolean(6)

        '    If SolPerm.Caja.Pais.Length = 0 Then
        '        comm_caj.Parameters.Add("@activo", MySqlDbType.Int16).Value = 0
        '        SolPerm.Caja.Nivel = "0"
        '    Else
        '        comm_caj.Parameters.Add("@activo", MySqlDbType.Int16).Value = 1
        '    End If

        '    comm_caj.Parameters.Add("@rol_id", MySqlDbType.Int16).Value = SolPerm.Caja.Nivel

        '    comm_caj.ExecuteNonQuery()


        'Catch ex As Exception

        '    MsgBox(ex.Message, , "procesa datos caja")

        '    If conn_mas.State = ConnectionState.Open Then
        '        trns_mas.Rollback()
        '        conn_mas.Close()
        '    End If

        '    i = 0
        '    For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
        '        If conn_ven(i).State = ConnectionState.Open Then
        '            trns_ven(i).Rollback()
        '            conn_ven(i).Close()
        '        End If
        '        i = i + 1
        '    Next

        '    If conn_aer.State = ConnectionState.Open Then
        '        trns_aer.Rollback()
        '        conn_aer.Close()
        '    End If

        '    If conn_ter.State = ConnectionState.Open Then
        '        trns_ter.Rollback()
        '        conn_ter.Close()
        '    End If

        '    If conn_cus.State = ConnectionState.Open Then
        '        trns_cus.Rollback()
        '        conn_cus.Close()
        '    End If

        '    If conn_caj.State = ConnectionState.Open Then
        '        trns_caj.Rollback()
        '        conn_caj.Close()
        '    End If

        '    Exit Sub

        'End Try



        '//////////////////////////////////// WMS //////////////////////////                        
        If SolPerm.Wms.Bodegas.Length > 0 Then

            Try


                qry = "SELECT COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE FROM DEF_USERS WHERE COD_USER = @cod_user"
                Dim comm_wms As New MySqlCommand(qry, conn_wms)
                comm_wms.Parameters.Add("@cod_user", MySqlDbType.String).Value = pw_name_

                conn_wms.Open()
                trns_wms = conn_wms.BeginTransaction()

                Dim dataread As MySqlDataReader = comm_wms.ExecuteReader
                If dataread.Read() Then
                    qry = "UPDATE DEF_USERS SET FIRSTNAME=@firstname, LASTNAME=@lastname, COD_GROUP=@cod_group, PASSWORD=@password, USER_TYPE=@user_type, PASSWORD_DATE=NOW(), STATUS=@status WHERE COD_USER=@cod_user"
                Else
                    qry = "INSERT INTO DEF_USERS (COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE, PASSWORD_DATE) VALUES (@cod_user, @firstname, @lastname, @cod_group, @password, @status, @user_type, NOW())"
                End If
                dataread.Close()

                comm_wms = New MySqlCommand(qry, conn_wms)
                comm_wms.Parameters.Add("@cod_user", MySqlDbType.String).Value = pw_name_
                comm_wms.Parameters.Add("@firstname", MySqlDbType.String).Value = pw_gecos_.Split(" ")(0)
                comm_wms.Parameters.Add("@lastname", MySqlDbType.String).Value = pw_gecos_.Split(" ")(1)
                comm_wms.Parameters.Add("@cod_group", MySqlDbType.String).Value = SolPerm.Wms.Grupo
                comm_wms.Parameters.Add("@password", MySqlDbType.String).Value = pw_passwd_
                comm_wms.Parameters.Add("@user_type", MySqlDbType.String).Value = SolPerm.Wms.Tipo
                comm_wms.Parameters.Add("@status", MySqlDbType.String).Value = "1"
                comm_wms.ExecuteNonQuery()

                qry = "DELETE FROM DEF_USERS_WAREHOUSES WHERE COD_USER = @cod_user"
                comm_wms = New MySqlCommand(qry, conn_wms)
                comm_wms.Parameters.Add("@cod_user", MySqlDbType.String).Value = pw_name_
                comm_wms.ExecuteNonQuery()

                Dim j As Integer = 0
                For j = 0 To SolPerm.Wms.Bodegas.Length - 1 'recorre las bodegas seleccionadas                    
                    qry = "INSERT INTO DEF_USERS_WAREHOUSES (COD_USER, COD_WAREHOUSE) VALUES (@cod_user, @bodega)"
                    comm_wms = New MySqlCommand(qry, conn_wms)
                    comm_wms.Parameters.Add("@cod_user", MySqlDbType.String).Value = pw_name_
                    comm_wms.Parameters.Add("@bodega", MySqlDbType.String).Value = SolPerm.Wms.Bodegas(j)
                    comm_wms.ExecuteNonQuery()
                Next

            Catch ex As Exception

                MsgBox(ex.Message, , "procesa datos wms")

                If conn_mas.State = ConnectionState.Open Then
                    trns_mas.Rollback()
                    conn_mas.Close()
                End If

                i = 0
                For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                    If conn_ven(i).State = ConnectionState.Open Then
                        trns_ven(i).Rollback()
                        conn_ven(i).Close()
                    End If
                    i = i + 1
                Next

                If conn_aer.State = ConnectionState.Open Then
                    trns_aer.Rollback()
                    conn_aer.Close()
                End If

                If conn_ter.State = ConnectionState.Open Then
                    trns_ter.Rollback()
                    conn_ter.Close()
                End If

                If conn_cus.State = ConnectionState.Open Then
                    trns_cus.Rollback()
                    conn_cus.Close()
                End If

                If conn_caj.State = ConnectionState.Open Then
                    trns_caj.Rollback()
                    conn_caj.Close()
                End If

                If conn_wms.State = ConnectionState.Open Then
                    trns_wms.Rollback()
                    conn_wms.Close()
                End If

                Exit Sub

            End Try

        End If



        'Try
        '    '//////////////////////////////////// BAW /////////////////////// no se aprueba solo se guarda lo requerido
        '    For Each li As KeyValuePair(Of String, String) In SolPerm.Baw

        '        If li.Key.Substring(0, 2) = "01" Then
        '            For Each li1 As ListItem In Checkboxlist1.Items
        '                If li1.Value = li.Key Then
        '                    li1.Selected = True
        '                    Exit For
        '                End If
        '            Next
        '        End If

        '        If li.Key.Substring(0, 2) = "02" Then
        '            For Each li1 As ListItem In Checkboxlist2.Items
        '                If li1.Value = li.Key Then
        '                    li1.Selected = True
        '                    Exit For
        '                End If
        '            Next
        '        End If

        '        If li.Key.Substring(0, 2) = "03" Then
        '            For Each li1 As ListItem In Checkboxlist3.Items
        '                If li1.Value = li.Key Then
        '                    li1.Selected = True
        '                    Exit For
        '                End If
        '            Next
        '        End If

        '        If li.Key.Substring(0, 2) = "04" Then
        '            For Each li1 As ListItem In Checkboxlist4.Items
        '                If li1.Value = li.Key Then
        '                    li1.Selected = True
        '                    Exit For
        '                End If
        '            Next
        '        End If
        '    Next



        'Catch ex As Exception

        'End Try


        If SolPerm.Manifiestos.nivelcr.Length > 0 Then

            Try

                '//////////////////////////////////// MANIFIESTOS CR //////////////////////////                            

                Dim val_bolean(7) As Boolean


                For Each li As String In SolPerm.Manifiestos.nivelcr
                    Select Case li
                        Case "AE"
                            val_bolean(0) = True
                        Case "TE"
                            val_bolean(1) = True
                        Case "MA"
                            val_bolean(2) = True
                        Case "AD"
                            val_bolean(3) = True
                    End Select
                Next

                qry = "SELECT user_id, id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @id_master"
                Dim comm_man As New NpgsqlCommand(qry, conn_man_cr)
                comm_man.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_

                conn_man_cr.Open()
                trns_man_cr = conn_man_cr.BeginTransaction()

                Dim dataread As NpgsqlDataReader = comm_man.ExecuteReader
                If dataread.Read() Then
                    qry = "UPDATE manifiestos_usuarios SET nombres=@nombres, usuario=@usuario, idpais=@idpais, Aereo=@Aereo, Maritimo=@Maritimo, Terrestre=@Terrestre, Aduanas=@Aduanas, Activo=@Activo WHERE id_master=@id_master"
                Else
                    qry = "INSERT INTO manifiestos_usuarios (id_master, nombres, usuario, idpais, Activo, Aereo, Maritimo, Terrestre, Aduanas) VALUES (@id_master, @nombres, @usuario, @idpais, @Activo, @Aereo, @Maritimo, @Terrestre, @Aduanas)"
                End If
                dataread.Close()

                comm_man = New NpgsqlCommand(qry, conn_man_cr)
                comm_man.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_
                comm_man.Parameters.Add("@nombres", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_gecos_
                comm_man.Parameters.Add("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_
                comm_man.Parameters.Add("@idpais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pais_
                comm_man.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = True
                comm_man.Parameters.Add("@Aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(0)
                comm_man.Parameters.Add("@Terrestre", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(1)
                comm_man.Parameters.Add("@Maritimo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(2)
                comm_man.Parameters.Add("@Aduanas", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(3)
                comm_man.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message, , "procesa datos manifiesto cr")

                If conn_mas.State = ConnectionState.Open Then
                    trns_mas.Rollback()
                    conn_mas.Close()
                End If

                i = 0
                For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                    If conn_ven(i).State = ConnectionState.Open Then
                        trns_ven(i).Rollback()
                        conn_ven(i).Close()
                    End If
                    i = i + 1
                Next

                If conn_aer.State = ConnectionState.Open Then
                    trns_aer.Rollback()
                    conn_aer.Close()
                End If

                If conn_ter.State = ConnectionState.Open Then
                    trns_ter.Rollback()
                    conn_ter.Close()
                End If

                If conn_cus.State = ConnectionState.Open Then
                    trns_cus.Rollback()
                    conn_cus.Close()
                End If

                If conn_caj.State = ConnectionState.Open Then
                    trns_caj.Rollback()
                    conn_caj.Close()
                End If

                If conn_wms.State = ConnectionState.Open Then
                    trns_wms.Rollback()
                    conn_wms.Close()
                End If

                If conn_baw.State = ConnectionState.Open Then
                    trns_baw.Rollback()
                    conn_baw.Close()
                End If

                If conn_man_cr.State = ConnectionState.Open Then
                    trns_man_cr.Rollback()
                    conn_man_cr.Close()
                End If

                Exit Sub

            End Try

        End If


        If SolPerm.Manifiestos.nivelcl.Length > 0 Then

            Try

                '//////////////////////////////////// MANIFIESTOS CRLTF //////////////////////////                            

                Dim val_bolean(7) As Boolean


                For Each li As String In SolPerm.Manifiestos.nivelcl
                    Select Case li
                        Case "AE"
                            val_bolean(0) = True
                        Case "TE"
                            val_bolean(1) = True
                        Case "MA"
                            val_bolean(2) = True
                        Case "AD"
                            val_bolean(3) = True
                    End Select
                Next

                qry = "SELECT user_id, id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @id_master"
                Dim comm_man As New NpgsqlCommand(qry, conn_man_crltf)
                comm_man.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_

                conn_man_crltf.Open()
                trns_man_crltf = conn_man_crltf.BeginTransaction()

                Dim dataread As NpgsqlDataReader = comm_man.ExecuteReader
                If dataread.Read() Then
                    qry = "UPDATE manifiestos_usuarios SET nombres=@nombres, usuario=@usuario, idpais=@idpais, Aereo=@Aereo, Maritimo=@Maritimo, Terrestre=@Terrestre, Aduanas=@Aduanas, Activo=@Activo WHERE id_master=@id_master"
                Else
                    qry = "INSERT INTO manifiestos_usuarios (id_master, nombres, usuario, idpais, Activo, Aereo, Maritimo, Terrestre, Aduanas) VALUES (@id_master, @nombres, @usuario, @idpais, @Activo, @Aereo, @Maritimo, @Terrestre, @Aduanas)"
                End If
                dataread.Close()

                comm_man = New NpgsqlCommand(qry, conn_man_cr)
                comm_man.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = id_usuario_
                comm_man.Parameters.Add("@nombres", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_gecos_
                comm_man.Parameters.Add("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pw_name_
                comm_man.Parameters.Add("@idpais", NpgsqlTypes.NpgsqlDbType.Varchar).Value = pais_
                comm_man.Parameters.Add("@Activo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = True
                comm_man.Parameters.Add("@Aereo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(0)
                comm_man.Parameters.Add("@Terrestre", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(1)
                comm_man.Parameters.Add("@Maritimo", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(2)
                comm_man.Parameters.Add("@Aduanas", NpgsqlTypes.NpgsqlDbType.Boolean).Value = val_bolean(3)
                comm_man.ExecuteNonQuery()


            Catch ex As Exception

                MsgBox(ex.Message, , "procesa datos manifiesto crltf")

                If conn_mas.State = ConnectionState.Open Then
                    trns_mas.Rollback()
                    conn_mas.Close()
                End If

                i = 0
                For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                    If conn_ven(i).State = ConnectionState.Open Then
                        trns_ven(i).Rollback()
                        conn_ven(i).Close()
                    End If
                    i = i + 1
                Next

                If conn_aer.State = ConnectionState.Open Then
                    trns_aer.Rollback()
                    conn_aer.Close()
                End If

                If conn_ter.State = ConnectionState.Open Then
                    trns_ter.Rollback()
                    conn_ter.Close()
                End If

                If conn_cus.State = ConnectionState.Open Then
                    trns_cus.Rollback()
                    conn_cus.Close()
                End If

                If conn_caj.State = ConnectionState.Open Then
                    trns_caj.Rollback()
                    conn_caj.Close()
                End If

                If conn_wms.State = ConnectionState.Open Then
                    trns_wms.Rollback()
                    conn_wms.Close()
                End If

                If conn_baw.State = ConnectionState.Open Then
                    trns_baw.Rollback()
                    conn_baw.Close()
                End If

                If conn_man_cr.State = ConnectionState.Open Then
                    trns_man_cr.Rollback()
                    conn_man_cr.Close()
                End If

                If conn_man_crltf.State = ConnectionState.Open Then
                    trns_man_crltf.Rollback()
                    conn_man_crltf.Close()
                End If

                Exit Sub

            End Try

        End If

        '//////////////////////////////////// TIR //////////////////////////  aprobar 2014-12-19          


        If SolPerm.Tir.Count > 0 Then

            Try
                Dim numero_ As Integer = 0
                qry = "SELECT * FROM usuarios WHERE nombre = @nombre"

                Dim comm As New MySqlCommand(qry, conn_tir)
                comm.Parameters.Add("@nombre", MySqlDbType.String).Value = pw_name_

                conn_tir.Open()
                trns_tir = conn_tir.BeginTransaction()

                Dim dataread As MySqlDataReader = comm.ExecuteReader
                If dataread.Read() Then
                    numero_ = dataread(0)
                    qry = "UPDATE usuarios SET nombrefull=@nombrefull, puesto='', clave=@clave, Permisos='', nivel=2, area='', sucursal=5, activo=0, mac='', status=1, printpreview=1, Version='8.10.57', direccion_ip='', id_empresa=1, modificado=now() WHERE numero=@numero"
                Else
                    qry = "INSERT INTO usuarios (numero, nombre, nombrefull, puesto, clave, Permisos, nivel, area, sucursal, activo, mac, status, printpreview, Version, direccion_ip, id_empresa, modificado) VALUES (@numero, @nombre, @nombrefull, '', @clave, '', 2, '', 5, 0, '', 1, 1, '8.10.57', '', 1, now())"
                End If
                dataread.Close()

                comm = New MySqlCommand(qry, conn_tir)
                comm.Parameters.Add("@numero", MySqlDbType.String).Value = numero_
                comm.Parameters.Add("@nombre", MySqlDbType.String).Value = pw_name_
                comm.Parameters.Add("@nombrefull", MySqlDbType.String).Value = pw_gecos_
                comm.Parameters.Add("@clave", MySqlDbType.String).Value = pw_passwd_
                comm.ExecuteNonQuery()

                If numero_ = 0 Then
                    qry = "SELECT LAST_INSERT_ID()"
                    comm = New MySqlCommand(qry, conn_tir)
                    dataread = comm.ExecuteReader
                    If dataread.Read() Then
                        numero_ = dataread(0)
                    End If
                    dataread.Close()
                End If




                '/////////////////////////////////////////////////PERMISOS//////////////////////////////
                If numero_ > 0 Then

                    Dim binarios As String = ""
                    Dim io As String

                    For Each li As KeyValuePair(Of String, String()) In SolPerm.Tir

                        binarios = ""
                        qry = "SELECT id_opcion FROM acceso ORDER BY opcion"
                        comm = New MySqlCommand(qry, conn_tir)
                        dataread = comm.ExecuteReader
                        If dataread.HasRows Then
                            While dataread.Read()
                                io = "0"
                                For Each li1 As String In li.Value
                                    If dataread(0) = li1 Then
                                        io = "1"
                                        Exit For
                                    End If
                                Next
                                binarios = binarios & io
                            End While
                        End If
                        dataread.Close()

                        Dim numero2_ As Integer = 0
                        qry = "SELECT * FROM permisos WHERE id_usuario=@id_usuario AND id_empresa=@id_empresa"
                        comm = New MySqlCommand(qry, conn_tir)
                        comm.Parameters.Add("@id_usuario", MySqlDbType.String).Value = numero_
                        comm.Parameters.Add("@id_empresa", MySqlDbType.String).Value = li.Key

                        dataread = comm.ExecuteReader
                        If dataread.Read() Then
                            numero2_ = dataread(5)
                            qry = "UPDATE permisos SET id_usuario=@id_usuario, id_empresa=@id_empresa, permisos=@permisos, modificado=now(), borrado=0, nivel_admin=0 WHERE numero=@numero"
                        Else
                            qry = "INSERT INTO permisos (numero, id_usuario, id_empresa, permisos, modificado, borrado, nivel_admin) VALUES (null, @id_usuario, @id_empresa, @permisos, now(), 0, 0)"
                        End If
                        dataread.Close()

                        comm = New MySqlCommand(qry, conn_tir)
                        comm.Parameters.Add("@numero", MySqlDbType.String).Value = numero2_
                        comm.Parameters.Add("@id_usuario", MySqlDbType.String).Value = numero_
                        comm.Parameters.Add("@id_empresa", MySqlDbType.String).Value = li.Key
                        comm.Parameters.Add("@permisos", MySqlDbType.String).Value = binarios
                        comm.ExecuteNonQuery()
                    Next

                End If



            Catch ex As Exception

                MsgBox(ex.Message, , "procesa datos tir")

                If conn_mas.State = ConnectionState.Open Then
                    trns_mas.Rollback()
                    conn_mas.Close()
                End If

                i = 0
                For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                    If conn_ven(i).State = ConnectionState.Open Then
                        trns_ven(i).Rollback()
                        conn_ven(i).Close()
                    End If
                    i = i + 1
                Next

                If conn_aer.State = ConnectionState.Open Then
                    trns_aer.Rollback()
                    conn_aer.Close()
                End If

                If conn_ter.State = ConnectionState.Open Then
                    trns_ter.Rollback()
                    conn_ter.Close()
                End If

                If conn_cus.State = ConnectionState.Open Then
                    trns_cus.Rollback()
                    conn_cus.Close()
                End If

                If conn_caj.State = ConnectionState.Open Then
                    trns_caj.Rollback()
                    conn_caj.Close()
                End If

                If conn_wms.State = ConnectionState.Open Then
                    trns_wms.Rollback()
                    conn_wms.Close()
                End If

                If conn_baw.State = ConnectionState.Open Then
                    trns_baw.Rollback()
                    conn_baw.Close()
                End If

                If conn_man_cr.State = ConnectionState.Open Then
                    trns_man_cr.Rollback()
                    conn_man_cr.Close()
                End If

                If conn_man_crltf.State = ConnectionState.Open Then
                    trns_man_crltf.Rollback()
                    conn_man_crltf.Close()
                End If

                If conn_tir.State = ConnectionState.Open Then
                    trns_tir.Rollback()
                    conn_tir.Close()
                End If

                Exit Sub

            End Try

        End If


        'Try
        '    '//////////////////////////////////// PLANILLAS //////////////////////////
        '    pNivel.SelectedValue = SolPerm.Planillas.Nivel
        'Catch ex As Exception

        'End Try


        'Try
        '    '//////////////////////////////////// BITACORA OD //////////////////////////            
        '    odNivel.SelectedValue = SolPerm.Bitacora_od.Nivel
        'Catch ex As Exception

        'End Try

        'Try
        '    '//////////////////////////////////// GRAFICAS ISO //////////////////////////
        '    iNivel.SelectedValue = SolPerm.Graficas_iso.Nivel
        'Catch ex As Exception

        'End Try

        'Try
        '    '//////////////////////////////////// E-MANIFIESTOS APL  //////////////////////////
        '    mNivel.SelectedValue = SolPerm.eManifiestos_apl.Nivel
        'Catch ex As Exception

        'End Try



        '//////////////////////////////////////////////////////// F I N A L I Z A R   T R A N S A C C I O N E  S ////////////////////////////////////////////////////////
        If conn_aer.State = ConnectionState.Open Then
            trns_aer.Commit()
            conn_aer.Close()
        End If

        If conn_ter.State = ConnectionState.Open Then
            trns_ter.Commit()
            conn_ter.Close()
        End If

        i = 0
        For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
            If conn_ven(i).State = ConnectionState.Open Then
                trns_ven(i).Commit()
                conn_ven(i).Close()
            End If
            i = i + 1
        Next

        If conn_cus.State = ConnectionState.Open Then
            trns_cus.Commit()
            conn_cus.Close()
        End If

        If conn_caj.State = ConnectionState.Open Then
            trns_caj.Commit()
            conn_caj.Close()
        End If

        If conn_wms.State = ConnectionState.Open Then
            trns_wms.Commit()
            conn_wms.Close()
        End If

        If conn_baw.State = ConnectionState.Open Then
            trns_baw.Commit()
            conn_baw.Close()
        End If

        If conn_man_cr.State = ConnectionState.Open Then
            trns_man_cr.Commit()
            conn_man_cr.Close()
        End If

        If conn_man_crltf.State = ConnectionState.Open Then
            trns_man_crltf.Commit()
            conn_man_crltf.Close()
        End If

        If conn_tir.State = ConnectionState.Open Then
            trns_tir.Commit()
            conn_tir.Close()
        End If


        '///////////////////////////////// CAMBIA DE ESTATUS A LA SOLICITUD DESPUES DE REALIZAR EL PROCESO DE APROBACION
        If conn_mas.State = ConnectionState.Open Then
            qry = "UPDATE usuarios_empresas_menu_solicitud SET sol_sta = 4, aut_fec = now(), aut_usr = " & Session("OperatorID") & " WHERE sol_no = " & Solicitud.Text
            Dim comm1 As New NpgsqlCommand(qry, conn_mas)
            'comm1.ExecuteNonQuery()

            Dim json As String = SerializePermiso(SolPerm)

            Dim items As New Dictionary(Of String, String)
            items.Add("MODULOS", json)

            limpia_datos()

            trns_mas.Commit()
            conn_mas.Close()

            log(Server, Session("DBAccesosUserId"), "Aprobar", "", items, "", Session("OperatorID"), Session("Login"), "Solicitud", Session("DBAccesos"), Session("OperatorIP"))


            msg = "Aprobado correctamente sin errores"



            'MsgBox("APROBADO CORRECTAMENTE SIN ERRORES", , "Aprobacion de Solicitud")

        End If




    End Sub


    Protected Sub btnRechazar_Click(sender As Object, e As System.EventArgs) Handles btnRechazar.Click

        Try

            CnnMs = GetConnectionStringFromFile("aimar", Server)
            Using conn As New NpgsqlConnection(CnnMs)

                conn.Open()

                qry = "UPDATE usuarios_empresas_menu_solicitud SET sol_sta = 5 WHERE sol_no = " & Solicitud.Text
                Dim comm As New NpgsqlCommand(qry, conn)
                comm.ExecuteNonQuery()

                limpia_datos()

            End Using

        Catch ex As Exception
            'MsgBox(ex.Message, , "btnRechazar_Click")
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

        Response.Redirect("Solicitudes.aspx")

    End Sub


    Protected Sub limpia_datos()

        acordion_field.Text = ""
        Solicitud.Text = ""
        Estatus.SelectedIndex = -1

        Empleado_id.SelectedIndex = -1
        Usuario.Text = ""
        Empleado_nombre.Text = ""
        Dominio.SelectedIndex = -1
        Correo.SelectedIndex = -1
        Chat.SelectedIndex = -1
        Solicitante.Text = ""
        Nuevo.SelectedIndex = -1
        Reemplaza.Text = ""
        gPais.SelectedIndex = -1
        tipo_usuario.SelectedIndex = -1
        gEmpresas.SelectedIndex = -1
        locode.SelectedIndex = -1

        'vPais.SelectedIndex = -1
        'vPaisSel.SelectedIndex = -1
        Perfil.SelectedIndex = -1

        aPais.SelectedIndex = -1
        aNivel.SelectedIndex = -1

        tPais.SelectedIndex = -1
        tNivel.SelectedIndex = -1

        cmNivel.SelectedIndex = -1
        cEmpresas.SelectedIndex = -1
        caNivel.SelectedIndex = -1
        cbNivel.SelectedIndex = -1

        cNivel.SelectedIndex = -1
        cPais.SelectedIndex = -1
        sNivel.SelectedIndex = -1
        cgNivel.SelectedIndex = -1

        Grupo.SelectedIndex = -1
        Tipo.SelectedIndex = -1
        Bodega.SelectedIndex = -1

        Checkboxlist1.SelectedIndex = -1
        Checkboxlist2.SelectedIndex = -1
        Checkboxlist3.SelectedIndex = -1
        Checkboxlist4.SelectedIndex = -1

        mcrNivel.SelectedIndex = -1
        mclNivel.SelectedIndex = -1

        tirPermisos.SelectedIndex = -1

        pNivel.SelectedIndex = -1
        odNivel.SelectedIndex = -1
        iNivel.SelectedIndex = -1
        mNivel.SelectedIndex = -1

        acordion_field.Text = ""

    End Sub





    '//////////////////////////////////// VENTAS / MARITIMO ////////////////////////// seleccionar base de datos
    Protected Sub imageButtonClick(sender As Object, e As System.EventArgs)
        'Dim imageButton As ImageButton = sender
        Dim imageButton As LinkButton = sender
        Dim tableCell As TableCell = imageButton.Parent
        Dim row As GridViewRow = tableCell.Parent
        ventas_grid.SelectedIndex = row.RowIndex

        Try

            vPaisS.Text = row.Cells(3).Text

            PerfilLabel.Text = "Perfiles <img src=Content/flags/" & row.Cells(4).Text & "-flag.gif height=16 title='" & row.Cells(4).Text & " ' /> " & row.Cells(3).Text

            'PerfilLabel.Text = "Perfiles " & row.Cells(5).Text

            Perfil.Items.Clear()

            CnnMs = GetConnectionStringFromFile("aimar", Server)

            Using conn_mas As New NpgsqlConnection(CnnMs)

                conn_mas.Open()

                CnnMs = GetConnectionStringFromFile("ventas", Server) & row.Cells(4).Text

                Using conn_ven As New NpgsqlConnection(CnnMs)

                    qry = "SELECT group_id, name FROM groups WHERE activo = 't' ORDER BY name"
                    Dim ds As New DataSet()
                    Dim comm_ven As New NpgsqlCommand(qry, conn_ven)
                    Dim adp As New NpgsqlDataAdapter(comm_ven)
                    adp.Fill(ds)
                    Perfil.DataSource = ds
                    Perfil.DataTextField = "name"
                    Perfil.DataValueField = "group_id"
                    Perfil.DataBind()

                    If Empleado_id.SelectedValue <> "0" And Solicitud.Text = "" Then

                        '////////LEER LAS REFERENCIAS 
                        qry = "SELECT id_nuevo, id_anterior, bd, activo FROM referencias_usuarios WHERE id_nuevo = @id_nuevo AND bd = @bd AND activo = 't' ORDER BY bd"
                        Dim comm_ref As New NpgsqlCommand(qry, conn_mas)
                        comm_ref.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Empleado_id.SelectedValue
                        comm_ref.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = vPaisS.Text
                        Dim dataread_ref As NpgsqlDataReader = comm_ref.ExecuteReader
                        If dataread_ref.HasRows Then

                            conn_ven.Open()

                            While dataread_ref.Read()

                                qry = "SELECT user_id, grupo FROM users WHERE user_id = @user_id ORDER BY grupo"
                                comm_ven = New NpgsqlCommand(qry, conn_ven)
                                comm_ven.Parameters.Add("@user_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = dataread_ref(1)
                                Dim dataread_ven As NpgsqlDataReader = comm_ven.ExecuteReader
                                If dataread_ven.Read() Then
                                    For Each li1 As ListItem In Perfil.Items
                                        If li1.Value = dataread_ven(1) Then
                                            li1.Selected = True
                                            Exit For
                                        End If
                                    Next
                                Else
                                End If
                                dataread_ven.Close()

                            End While

                        End If
                        dataread_ref.Close()

                    End If

                    conn_ven.Close()

                End Using






                If Solicitud.Text <> "" Then

                    Dim SolPerm As New SolicitudPermiso()
                    qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Solicitud.Text
                    Dim comm As New NpgsqlCommand(qry, conn_mas)

                    Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                    If dataread.Read() Then
                        SolPerm = DeserializePermiso(dataread(8))
                    End If
                    dataread.Close()

                    For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                        If vPaisS.Text = li.Key Then
                            Dim perfiles() As String = li.Value
                            For Each li2 As String In perfiles
                                For Each li1 As ListItem In Perfil.Items
                                    If li1.Value = li2 Then
                                        li1.Selected = True
                                        Exit For
                                    End If
                                Next
                            Next
                            Exit For
                        End If
                    Next

                End If

                conn_mas.Close()
            End Using



        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try







    End Sub


    '//////////////////////////////////// VENTAS / MARITIMO ////////////////////////// configura gridview base de datos ventas

    Protected Sub ventas_grid_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles ventas_grid.RowDataBound
        Try
            Dim row As GridViewRow = e.Row
            If row.RowType = DataControlRowType.DataRow Then
                'Dim link As LinkButton = row.FindControl("lnk_user")
                Dim icon As String = "glyphicons_153_unchecked.png"

                Dim chk As Image = row.FindControl("lnk_chk")

                chk.Width = 24

                CnnMs = GetConnectionStringFromFile("aimar", Server)

                If Session("solicitud") <> Nothing Then

                    Using conn As New NpgsqlConnection(CnnMs)
                        qry = "SELECT * FROM usuarios_empresas_menu_solicitud WHERE sol_no = " & Session("solicitud")
                        Dim SolPerm As New SolicitudPermiso()
                        Dim comm As New NpgsqlCommand(qry, conn)
                        conn.Open()
                        Dim dataread As NpgsqlDataReader = comm.ExecuteReader
                        If dataread.Read() Then
                            SolPerm = DeserializePermiso(dataread(8))
                        End If
                        dataread.Close()
                        For Each li As KeyValuePair(Of String, String()) In SolPerm.Ventas
                            If li.Key = row.Cells(3).Text Then
                                icon = "glyphicons_152_check.png"
                            End If
                        Next
                    End Using
                End If

                If Empleado_id.SelectedValue <> "0" Then
                    Using conn As New NpgsqlConnection(CnnMs)
                        '////////LEER LAS REFERENCIAS 
                        qry = "SELECT distinct bd FROM referencias_usuarios WHERE id_nuevo = @id_nuevo AND bd = @bd AND activo = 't' ORDER BY bd"
                        Dim comm_ref As New NpgsqlCommand(qry, conn)
                        comm_ref.Parameters.Add("@id_nuevo", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Empleado_id.SelectedValue
                        comm_ref.Parameters.Add("@bd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = row.Cells(3).Text
                        conn.Open()
                        Dim dataread_ref As NpgsqlDataReader = comm_ref.ExecuteReader
                        If dataread_ref.HasRows Then
                            dataread_ref.Read()
                            icon = "glyphicons_152_check.png"
                        End If
                    End Using
                End If

                'link.Text = "<img src=Content/icon/" & icon & " height=16 />"

                chk.ImageUrl = "~/Content/icon/" & icon
                chk.Width = 20

                Dim flag As Image = row.FindControl("lnk_flag")
                flag.ToolTip = row.Cells(4).Text


                row.Cells(5).Text = row.Cells(5).Text.Replace("src=", "src=" & Chr(34))
                row.Cells(5).Text = row.Cells(5).Text.Replace("gif", "gif" & Chr(34))


                Dim archivo As String = "~/Content/flags/" & Trim(row.Cells(4).Text).ToLower & "-flag.gif"
                'Dim myFilePath As String = Server.MapPath(archivo)
                'If File.Exists(myFilePath) Then
                flag.ImageUrl = archivo
                'End If

            End If

        Catch ex As Exception
            msg = ex.Message
            img = icon_err_active
            css = "alert-default"
        End Try

    End Sub



    Protected Sub Empleado_id_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles Empleado_id.SelectedIndexChanged

        Dim comm As NpgsqlCommand
        Dim dataread As NpgsqlDataReader

        Dim comy As MySqlCommand
        Dim dataready As MySqlDataReader

        '//////////////////////////////////// MASTER //////////////////////////                            
        Try

            CnnMs = GetConnectionStringFromFile("aimar", Server)

            Using conn_mas As New NpgsqlConnection(CnnMs)

                conn_mas.Open()

                qry = "SELECT pw_name, pw_gecos, pais, pw_passwd, dominio, tipo_usuario, pw_correo, locode, level, id_usuario FROM usuarios_empresas WHERE id_usuario = @id_usuario"
                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Empleado_id.SelectedValue
                dataread = comm.ExecuteReader
                If dataread.Read() Then
                    'Empleado_id.SelectedValue = dataread(8)
                    Usuario.Text = dataread(0)
                    Empleado_nombre.Text = dataread(1)
                    gPais.SelectedValue = dataread(2)
                    'password  dataread(3)
                    Dominio.SelectedValue = dataread(4)
                    tipo_usuario.SelectedValue = dataread(5)
                    'correo dataread(6)
                    locode.SelectedValue = dataread(7)

                    '//////////////////////////////////// CATALOGOS ////////////////////////// 
                    cgNivel.SelectedValue = dataread(8)

                    'Correo.SelectedValue = li.Value				
                    'Chat.SelectedValue = li.Value				
                    'Solicitante.Text = li.Value				
                    'Nuevo.SelectedValue = li.Value				
                    'Reemplaza.Text = li.Value
                End If
                dataread.Close()

                '//////////////////////////////////// SEGUROS //////////////////////////                            
                qry = "SELECT id_usuario, id_tipo_usuario FROM detalle_tipos_usuario WHERE id_usuario = @id_usuario ORDER BY id_tipo_usuario DESC"
                comm = New NpgsqlCommand(qry, conn_mas)
                comm.Parameters.Add("@id_usuario", NpgsqlTypes.NpgsqlDbType.Integer).Value = Empleado_id.SelectedValue
                dataread = comm.ExecuteReader
                If dataread.Read() Then
                    sNivel.SelectedValue = dataread(1)
                End If
                dataread.Close()

                '//////////////////////////////////// VENTAS / MARITIMO //////////////////////////
                ventas_grid_caller(conn_mas)

                conn_mas.Close()

            End Using
        Catch ex As Exception

        End Try


        Dim j As Integer

        '//////////////////////////////////// AEREO //////////////////////////        
        Try

            CnnMs = GetConnectionStringFromFile("aereo", Server)

            Using conn As New MySqlConnection(CnnMs)
                qry = "SELECT  OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate FROM Operators WHERE OperatorID = @OperatorID"
                comy = New MySqlCommand(qry, conn)
                comy.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = Empleado_id.SelectedValue
                conn.Open()
                dataready = comy.ExecuteReader
                If dataready.Read() Then
                    aNivel.SelectedValue = dataready(7)
                    Dim strArr() As String
                    Dim countries As String = ""
                    For Each li As ListItem In aPais.Items 'set completo de paises
                        strArr = dataready(8).Split(",") 'array en base de datos
                        For j = 0 To strArr.Length - 1
                            If Comillas & Trim(li.Value) & Comillas = strArr(j) Then
                                li.Selected = True
                            End If
                        Next
                    Next
                End If
                dataready.Close()
                conn.Close()
            End Using
        Catch ex As Exception

        End Try

        '//////////////////////////////////// TERRESTRE //////////////////////////                        
        Try
            CnnMs = GetConnectionStringFromFile("terrestre", Server)
            Using conn As New MySqlConnection(CnnMs)
                qry = "SELECT  OperatorID, Login, FirstName, LastName, Email, Phone, Position, OperatorLevel, Countries, Active, Sign, CreatedDate FROM Operators WHERE OperatorID = @OperatorID"
                comy = New MySqlCommand(qry, conn)
                comy.Parameters.Add("@OperatorID", MySqlDbType.Int32).Value = Empleado_id.SelectedValue
                conn.Open()
                dataready = comy.ExecuteReader
                If dataready.Read() Then
                    tNivel.SelectedValue = dataready(7)
                    Dim strArr() As String
                    Dim countries As String = ""
                    For Each li As ListItem In tPais.Items 'set completo de paises
                        strArr = dataready(8).Split(",") 'array en base de datos
                        For j = 0 To strArr.Length - 1
                            If Comillas & Trim(li.Value) & Comillas = strArr(j) Then
                                li.Selected = True
                            End If
                        Next
                    Next
                End If
                dataready.Close()
                conn.Close()
            End Using
        Catch ex As Exception

        End Try

        '//////////////////////////////////// CUSTOMER //////////////////////////
        Try
            CnnMs = GetConnectionStringFromFile("customer", Server)
            Using conn_cus As New MySqlConnection(CnnMs)
                qry = "SELECT numero, id_usuario_empresa, id_empresa, id_pais, nombre, nombrefull, puesto, direccion_ip, email, locode, " & _
                "acceso_aduana, acceso_apl, permisos, fecha_ingreso, fecha_desactiva, modificado, activo, nivel_dua, nivel_bit_apl, psw, acceso_maritimo, locode " & _
                "FROM usuarios WHERE id_usuario_empresa = @id_usuario_empresa AND borrado = 0 ORDER BY numero"
                comy = New MySqlCommand(qry, conn_cus)
                comy.Parameters.Add("@id_usuario_empresa", MySqlDbType.String).Value = Empleado_id.SelectedValue
                conn_cus.Open()
                dataready = comy.ExecuteReader
                While dataready.Read()
                    '//////////////////////////////////// CUSTOMER MARITIMO //////////////////////////
                    cmNivel.SelectedValue = dataready(20)
                    '//////////////////////////////////// CUSTOMER BITACORA //////////////////////////                        
                    cbNivel.SelectedValue = dataready(18)
                    '//////////////////////////////////// CUSTOMER ADUANAS //////////////////////////
                    For Each li As ListItem In cEmpresas.Items
                        If li.Value = dataready(2) Then
                            li.Selected = True
                            Exit For
                        End If
                    Next
                    caNivel.SelectedValue = dataready(17)
                End While
                dataready.Close()
                conn_cus.Close()
            End Using
        Catch ex As Exception

        End Try


        Try
            '//////////////////////////////////// CAJA DE AHORRO //////////////////////////                        
            CnnMs = GetConnectionStringFromFile("caja", Server)
            Using conn As New MySqlConnection(CnnMs)
                qry = "SELECT user_id, nombres, email, username, password, fecha_ingreso, activo, idpais, rol_id, bz, cr, sv, gt, hn, ni, pa, id_master FROM usuarios WHERE id_master = @id_master"
                comy = New MySqlCommand(qry, conn)
                comy.Parameters.Add("@id_master", MySqlDbType.Int32).Value = Empleado_id.SelectedValue
                conn.Open()
                dataready = comy.ExecuteReader
                If dataready.Read() Then
                    cNivel.SelectedValue = dataready(8)
                    j = 8
                    For Each li As ListItem In cPais.Items
                        j = j + 1
                        li.Selected = dataready(j)
                    Next
                End If
                dataready.Close()
                conn.Close()
            End Using
        Catch ex As Exception

        End Try




        '//////////////////////////////////// WMS //////////////////////////     
        Try
            CnnMs = GetConnectionStringFromFile("wms", Server)
            Using conn As New MySqlConnection(CnnMs)
                qry = "SELECT COD_USER, FIRSTNAME, LASTNAME, COD_GROUP, PASSWORD, STATUS, USER_TYPE FROM DEF_USERS WHERE COD_USER = @login"
                comy = New MySqlCommand(qry, conn)
                comy.Parameters.Add("@login", MySqlDbType.String).Value = Usuario.Text
                conn.Open()
                dataready = comy.ExecuteReader
                If dataready.Read() Then
                    Tipo.SelectedValue = dataready(6)
                    Grupo.SelectedValue = dataready(3)
                End If
                dataready.Close()

                'bodegas asignadas
                qry = "SELECT COD_USER, COD_WAREHOUSE FROM DEF_USERS_WAREHOUSES WHERE COD_USER = @login"
                comy = New MySqlCommand(qry, conn)
                comy.Parameters.Add("@login", MySqlDbType.String).Value = Usuario.Text
                dataready = comy.ExecuteReader
                While dataready.Read()
                    For Each li As ListItem In Bodega.Items
                        If li.Value = dataready(1) Then
                            li.Selected = True
                            Exit For
                        End If
                    Next
                End While
                dataready.Close()
                conn.Close()
            End Using
        Catch ex As Exception

        End Try





        '//////////////////////////////////// MANIFIESTOS CR / CRLTF //////////////////////////     
        Try
            CnnMs = GetConnectionStringFromFile("ventas", Server) & "cr"
            Using conn As New NpgsqlConnection(CnnMs)
                qry = "SELECT user_id, id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @codigo"
                comm = New NpgsqlCommand(qry, conn)
                comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Empleado_id.SelectedValue
                conn.Open()
                dataread = comm.ExecuteReader
                If dataread.Read() Then
                    j = 5
                    For Each li As ListItem In mcrNivel.Items
                        j = j + 1
                        If li.Value = dataread(j) Then
                            li.Selected = True
                        End If
                    Next
                End If
                dataread.Close()
            End Using
        Catch ex As Exception

        End Try


        Try
            CnnMs = GetConnectionStringFromFile("ventas", Server) & "crltf"
            Using conn As New NpgsqlConnection(CnnMs)
                qry = "SELECT user_id, id_master, nombres, usuario, idpais, activo, aereo, maritimo, terrestre, aduanas FROM manifiestos_usuarios WHERE id_master = @codigo"
                comm = New NpgsqlCommand(qry, conn)
                comm.Parameters.Add("@id_master", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Empleado_id.SelectedValue
                conn.Open()
                dataread = comm.ExecuteReader
                If dataread.Read() Then
                    j = 5
                    For Each li As ListItem In mcrNivel.Items
                        j = j + 1
                        If li.Value = dataread(j) Then
                            li.Selected = True
                        End If
                    Next
                End If
                dataread.Close()
            End Using
        Catch ex As Exception

        End Try



        '///////////////////////////// TIR
        Try
            CnnMs = GetConnectionStringFromFile("tir", Server)
            Using conn As New MySqlConnection(CnnMs)

                qry = "SELECT * FROM acceso ORDER BY opcion"
                Dim ds As New DataSet()
                Dim cmd As New MySqlCommand(qry, conn)
                Dim adp As New MySqlDataAdapter(cmd)
                adp.Fill(ds)
                tirPermisos.Items.Clear()
                tirPermisos.DataSource = ds
                tirPermisos.DataTextField = "opcion"
                tirPermisos.DataValueField = "id_opcion"
                tirPermisos.DataBind()

            End Using
        Catch ex As Exception

        End Try


        '//////////////////////////////////// TIR //////////////////////////     llena_datos leidos de tabla       
        'For Each li As KeyValuePair(Of String, String) In SolPerm.Tir
        '    For Each li1 As ListItem In Permisos.Items
        '        If li1.Value = li.Key Then
        '            li1.Selected = True
        '            Exit For
        '        End If
        '    Next
        'Next



    End Sub


End Class





'<div class="bs-example">
'    <div class="panel-group" id="accordion">
'        <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne" onclick="accordion_click(1)">1. GENERALES</a>
'                </h4>
'            </div>
'            <div id="collapseOne" class="panel-collapse collapse <%=accordion_open(1)%>">
'                <div class="panel-body">

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />

'                </div>
'            </div>
'        </div>
'        <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" onclick="accordion_click(2)">2. VENTAS / MARITIMO</a>
'                </h4>
'            </div>
'            <div id="collapseTwo" class="panel-collapse collapse <%=accordion_open(2)%>">
'                <div class="panel-body">

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />          

'                </div>
'            </div>
'        </div>
'        <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseThree" onclick="accordion_click(3)">3. AEREO / TERRESTRE</a>
'                </h4>
'            </div>
'            <div id="collapseThree" class="panel-collapse collapse <%=accordion_open(3)%>">
'                <div class="panel-body">                        

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />          

'                    </div>
'            </div>
'        </div>


'        <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseFour" onclick="accordion_click(4)">4. CUSTOMER MARITIMO / CUSTOMER ADUANAS / BITACORA APL </a>
'                </h4>
'            </div>
'            <div id="collapseFour" class="panel-collapse collapse <%=accordion_open(4)%>">
'                <div class="panel-body">                        

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />                       

'                </div>
'            </div>
'        </div>



'        <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseFive" onclick="accordion_click(5)">5. CAJA DE AHORRO / SEGUROS / CATALOGOS</a>
'                </h4>
'            </div>
'            <div id="collapseFive" class="panel-collapse collapse <%=accordion_open(5)%>">
'                <div class="panel-body">                        

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />                       










'                </div>
'            </div>
'        </div>


'       <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseSix" onclick="accordion_click(6)">6. WMS</a>
'                </h4>
'            </div>
'            <div id="collapseSix" class="panel-collapse collapse <%=accordion_open(6)%>">
'                <div class="panel-body">                        

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />                       





'                </div>
'            </div>
'        </div>


'       <div class="panel panel-default">
'            <div class="panel-heading">
'                <h4 class="panel-title">
'                    <a data-toggle="collapse" data-parent="#accordion" href="#collapseSeven" onclick="accordion_click(7)">7. BAW</a>
'                </h4>
'            </div>
'            <div id="collapseSeven" class="panel-collapse collapse <%=accordion_open(7)%>">
'                <div class="panel-body">                        

'                        <br />
'                        <br />
'                        <br />
'                        <br />
'                        <br />                       


'                </div>
'            </div>
'        </div>


'    </div>
'</div>

