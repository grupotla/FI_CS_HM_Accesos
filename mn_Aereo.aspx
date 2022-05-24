<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Aereo.aspx.vb" Inherits="mn_Aereo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
// <![CDATA[
    
        $(function () {
            // Document is ready
//            if ($('#<%=Nombre.ClientID%>'))
//                $('#<%=Nombre.ClientID%>').focus();
//            $('#<%=Activo.ClientID%>').attr('disabled', 'disabled');
//            if ('<%=Activo.Checked%>' == 'False') {
                //$('input[type=text]').attr('disabled', 'disabled');
                //$('input[type=checkbox]').attr('disabled', 'disabled');
                //$('input[type=radio]').attr('disabled', 'disabled');
                //$('textarea').attr('disabled', 'disabled');                
//            }


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
                <% If Session("OperatorLevel") > Tipo.SelectedValue + 1 Then%>
                    alert('No tiene persimos suficientes');
                    return false;
                <% End If %>  
                <% End If %>                               
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                var checked = parseInt(valor_radio('<%=Tipo.ClientID%>')) + 1;
                //alert(OpeLev + ' ' + checked);
                if (OpeLev > checked) {
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

        function valida_activar() {                                  
            if (!confirm("Confirme Activar Este Registro")) {
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
         
        function valida_actualizar() {
            var checked = valor_radio('<%=Tipo.ClientID%>');
            if (checked == 999) { alert("Seleccione Nivel"); return false; }            
            if ($('#<%=Login.ClientID%>').val() == "") { alert("Seleccione Login"); return false; }
            if ($('#<%=Nombre.ClientID%>').val() == "") { alert("Ingrese Nombre"); return false; }
            if ($('#<%=Firma.ClientID%>').val() == "") { alert("Ingrese Firma"); return false; }                                        
            var checked = valor_checkbox('<%=Paises.ClientID%>');
            if (checked == 999) { alert("Seleccione al menos un Pais"); return false; }                        
            return true;
        }





//        function toggleChecked(status) {
//            $("input[type=checkbox]").each(function () {
//                $(this).attr("checked", status);
//            })            
//        }

//        function check_activo() {
//            console.log($('#ctl00_ContentPlaceHolder1_Activo').attr("checked"));
//            if ($('#ctl00_ContentPlaceHolder1_Activo').attr("checked") == undefined) {
//                if ($('#ctl00_ContentPlaceHolder1_Firma').val() == "") { alert("Ingrese Firma"); return false; }
//                if ($('#ctl00_ContentPlaceHolder1_Email').val() == "") { alert("Ingrese Email"); return false; }
//                if ($('#ctl00_ContentPlaceHolder1_Apellido').val() == "") { alert("Ingrese Apellido"); return false; }
//                if ($('#ctl00_ContentPlaceHolder1_Nombre').val() == "") { alert("Ingrese Nombre"); return false; }
//                if ($('#ctl00_ContentPlaceHolder1_Login').val() == "0") { alert("Seleccione Login"); return false; }
//            }
//            //$('#ctl00_ContentPlaceHolder1_btn_activo').click();
//        }

// ]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="panel panel-info">

        <div class="panel-heading">          

            <asp:hiddenfield runat="server" id="codigo" Visible="True"></asp:hiddenfield>
         
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

            <!-- aqui va el menu tabs de manifiestos generado desde el codigo-->
            <asp:Label ID="pestana_lbl" runat="server" Text="Label"></asp:Label>

            <div id="catalogo2"  class="tabs_panel">

                <div style="width:44%;display:inline-block;vertical-align:top">

                    <div class="form-group row">
                        <label for="Login" class="col-sm-4 control-label">Login</label>
                        <div class="col-sm-7">                            
                            <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Nombre" class="col-sm-4 control-label">Nombre</label>
                        <div class="col-sm-7">
                            <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ReadOnly  ></asp:TextBox>
                        </div>
                    </div>

        
                    <div class="form-group row">
                        <label for="Email" class="col-sm-4 control-label">Email</label>
                        <div class="col-sm-7">
                            <asp:TextBox class="form-control" id="Email" runat="server" placeholder="Ingrese Email" ReadOnly="true"  ></asp:TextBox>
                        </div>
                    </div>
    <%--        
                    <div class="form-group row">
                        <label for="Apellido" class="col-sm-4 control-label">* Apellido</label>
                        <div class="col-sm-7">
                        
                        </div>
                    </div>
    --%>

                    <div class="form-group row">
                        <label for="Telefono" class="col-sm-4 control-label">Telefono</label>
                        <div class="col-sm-7">
                            <asp:TextBox class="form-control" id="Telefono" runat="server" placeholder="Ingrese Telefono" ></asp:TextBox>
                        </div>
                    </div>


                    <asp:TextBox class="form-control" id="Apellido" runat="server" placeholder="Ingrese Apellido" style="display:none"></asp:TextBox>

                    <div class="form-group row">
                        <label for="Puesto" class="col-sm-4 control-label">Puesto</label>
                        <div class="col-sm-7">
                            <asp:TextBox class="form-control" id="Puesto" runat="server" placeholder="Ingrese Puesto" ></asp:TextBox>
                        </div>
                    </div>


                </div>

                <div style="width:30%;display:inline-block;vertical-align:top">




                    <div class="form-group row">
                        <label for="Tipo" class="col-sm-3 control-label">Nivel</label>
                        <div class="col-sm-8">                                                                            

                                <asp:RadioButtonList ID="Tipo" runat="server" class="form-control">                                                                
                                <asp:ListItem Value="0" Text="Root"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Admin"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Editor"></asp:ListItem>
                                </asp:RadioButtonList>

                        </div>
                    </div>
                    <br />
                    <br />
                    <br />


                    <div class="form-group row">
                        <label for="Firma" class="col-sm-3 control-label">Firma</label>
                        <div class="col-sm-8">                     
                            <asp:TextBox class="form-control" ID="Firma" runat="server" TextMode="MultiLine" ></asp:TextBox>
                        </div>
                    </div>


                    <div class="form-group row">                          
                        <label for="Activo" class="col-sm-3 control-label">Activo</label>
                        <div class="col-sm-8">                          
                            <asp:CheckBox ID="Activo" runat="server"  class="form-control" />                          
                        </div>
                    </div>

                    <% If Session("DBAccesos") = "terrestre" Then %>

                    <div class="form-group row">                          
                        <label for="PerfilColgate" class="col-sm-3 control-label">Perfil Colgate</label>
                        <div class="col-sm-8">                          
                            <asp:CheckBox ID="PerfilColgate" runat="server"  class="form-control" />                          
                        </div>
                    </div>

                    <% End If %>


                </div>


                <div style="width:25%;display:inline-block;vertical-align:top">


                            
                                <label for="Paises" class="col-sm-8 control-label">Accesos Paises</label>
                                <div class="col-sm-10 checkbox">
                                    <asp:checkboxlist id="Paises" runat="server" class="form-control">
                                    </asp:checkboxlist>
                                </div>
                            


                </div>
                   
            </div>
 
        </div>

    </div>


   


</asp:Content>

