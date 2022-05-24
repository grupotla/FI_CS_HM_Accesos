<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Caja_ahorros.aspx.vb" Inherits="mn_Caja_ahorros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(function () {
            // Document is ready
//            if ($('#<%=Nombre.ClientID%>'))
//                $('#<%=Nombre.ClientID%>').focus();
//            if ('<%=Activo.Checked%>' == 'False') {
                //$('input[type=checkbox]').attr('disabled', 'disabled');
                //$('input[type=radio]').attr('disabled', 'disabled');
//                $('input[type=text]').attr('disabled', 'disabled');
                //$('select').attr('disabled', 'disabled');
//            }
            //$('#<%=Activo.ClientID%>').attr('disabled', 'disabled');
            <% If msg <> "" Then 
                msg = msg.Replace("'", "")
                msg = msg.Replace(vbCrLf, "<br>")
            %>
                $('#modal-texto').html('<%=msg%>');
                $('#modal-img').html('<%=img%>');                
                $('#myAlert').addClass('<%=css%>');                
                $('#myModal').modal();
            <% End If %>  

            $('#<%=Activo.ClientID%>').click(function (event) {
                return false;
            });

            CheckUnCheck('<%=Activo.Checked%>');

            //validar que el operador tiene permisos para asignar permisos a usuario
            $('#<%=Tipo.ClientID%>').click(function (event) {
                <% If Tipo.SelectedIndex > -1 Then%>                
                <% If Session("OperatorLevel") = 3 And Tipo.SelectedValue = 1 Then%>
                    alert('No tiene persimos suficientes');
                    return false;
                <% End If %>  
                <% End If %>                               
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                var checked = parseInt(valor_radio('<%=Tipo.ClientID%>'));
                //alert(OpeLev + ' ' + checked);                
                if (OpeLev == 3 && checked == 1) {
                    alert('No tiene persimos suficientes');
                    return false;
                }
            });

        });

        function valida_eliminar() {
            if (!confirm("Confirme Desactivar Este Registro")) {
                return false;
            }
        }

        function valida_agregar() {
            if (!confirm("Confirme Crear Este Registro")) {
                return false;
            } else {
                return valida_actualizar()
            }
        }

        function valida_activar() {
            if (!confirm("Confirme Activar Este Registro")) {
                return false;
            }
        }

        function valida_actualizar() {               
            //if ($('#<%=Login.ClientID%>').val() == "") { alert("Ingrese Login"); return false; }
            //if ($('#<%=Nombre.ClientID%>').val() == "") { alert("Ingrese Nombre"); return false; }            
            //if ($('#<%=Email.ClientID%>').val() == "") { alert("Ingrese Email"); return false; }
            //if ($('#<%=Password.ClientID%>').val() == "") { alert("Ingrese Password"); return false; }            
            var checked = valor_radio('<%=Tipo.ClientID%>');
            if (checked == 999) { alert("Seleccione Nivel"); return false; }                                         
            checked = valor_checkbox('<%=Paises.ClientID%>');
            if (checked == 999) { alert("Seleccione al menos un Pais"); return false; }                          
            return true;
        }
// ]]>
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="panel panel-info">
            
        <div class="panel-heading"> 

        </div>

            <div class="col-sm-offset-6 form-group row boton_bar">
                <asp:LinkButton ID="btn_cancelar" runat="server" CssClass="btn btn-sm btn-primary" style="display:none"><%=Licon_home%></asp:LinkButton>
                <% If Session("insert") = True Then%>   
                    <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                        <asp:LinkButton ID="btn_agregar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_agregar()"><%=Licon_insert%></asp:LinkButton>
                    <% Else%>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_insert%></asp:LinkButton>
                    <% End If%>                                                          
                <% Else%>
                    <% If Activo.Checked = False Then%>
                        <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                            <asp:LinkButton ID="btn_activar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_activar()"><%=Licon_on%></asp:LinkButton>
                        <% Else%>
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_on%></asp:LinkButton>
                        <% End If%>                                                          
                    <% Else%>                    
                        <% If Session("OperatorLevel") <> Nothing Then%>
                            <asp:LinkButton ID="btn_actualizar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_actualizar()"><%=Licon_update%></asp:LinkButton>
                        <% Else%>
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_update%></asp:LinkButton>
                        <% End If%>                                                          
                        <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                            <asp:LinkButton ID="btn_eliminar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_eliminar()"><%=Licon_off%></asp:LinkButton>             
                        <% Else%>
                            <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_off%></asp:LinkButton>
                        <% End If%>                                                          
                    <%End If%> 
                <%End If%>  
            </div>

        <div class="panel-body">

            <asp:Label ID="pestana_lbl" runat="server" Text=""></asp:Label>
        
            <div id="catalogo2" class="tabs_panel">

                <div style="width:45%;display:inline-block;vertical-align:top">

                        <asp:hiddenfield runat="server" id="codigo" Visible="True"></asp:hiddenfield>

                        <div class="form-group row">
                            <label for="Login" class="col-sm-3 control-label">Login</label>
                            <div class="col-sm-6">                            
                                <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                            <div class="col-sm-6">
                                <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
        
                        <div class="form-group row">
                            <label for="Pais" class="col-sm-3 control-label">Pais</label>
                            <div class="col-sm-6">
                                <asp:TextBox class="form-control" id="Pais" runat="server" placeholder="Ingrese Pais"  ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="Email" class="col-sm-3 control-label">Email</label>
                            <div class="col-sm-6">
                                <asp:TextBox class="form-control" id="Email" runat="server" placeholder="Ingrese Email" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="Password" class="col-sm-3 control-label">Password</label>
                            <div class="col-sm-8">                            
                                <asp:TextBox class="form-control" id="Password" runat="server" placeholder="Ingrese Password" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>

                </div>

                <div style="width:30%;display:inline-block;vertical-align:top">


                        <div class="form-group row">
                            <label for="user_id" class="col-sm-3 control-label">ID</label>
                            <div class="col-sm-6">
                                <asp:TextBox class="form-control" id="user_id" runat="server" placeholder="Registro Nuevo" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">                          
                            <label for="Activo" class="col-sm-3 control-label">Activo</label>
                            <div class="col-sm-6">                          
                                <asp:CheckBox ID="Activo" runat="server"  class="form-control" />                          
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="Tipo" class="col-sm-3 control-label">Nivel</label>
                            <div class="col-sm-6">                                
                                <asp:RadioButtonList ID="Tipo" runat="server" class="form-control">                                                                
                                </asp:RadioButtonList>    
                            </div>
                        </div>


                </div>

                <div style="width:20%;display:inline-block;vertical-align:top">

                        <div class="form-group row">
                            <label for="Paises" class="col-sm-10 control-label">Accesos Paises</label>
                            <div class="col-sm-offset-2 col-sm-10 checkbox">                                
                                <asp:checkboxlist id="Paises" runat="server" class="form-control">
                                </asp:checkboxlist>                                
                            </div>
                        </div>
                </div>

            </div> <!-- catalogo2 -->

        </div>

    </div>

</asp:Content>

