<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_CustomerAduana.aspx.vb" Inherits="mn_CustomerAduana" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(function () {
            // Document is ready				

            <% If msg <> "" Then 
                msg = msg.Replace("'", "")
                msg = msg.Replace(vbCrLf, "<br>")
            %>
                $('#modal-texto').html('<%=msg%>');
                $('#modal-img').html('<%=img%>');                
                $('#myAlert').addClass('<%=css%>');                
                $('#myModal').modal();
            <% End If %>  
                 

            $('.pestanas_customer').hide();
            $('#<%=Session("pestana")%>2').show();
            
            CheckUnCheck('<%=active%>');



            $('#<%=Activo.ClientID%>').click(function (event) {
                return false;
            });            
            $('#<%=Empresa.ClientID%>').click(function (event) {
                if ('<%=Activo.Checked%>' == 'False') 
                    return false;
            });
            $('#<%=niveldua.ClientID%>').click(function (event) {
                if ('<%=Activo.Checked%>' == 'False') 
                    return false;
            });
            $('#<%=Puesto.ClientID%>').keypress(function (event) {
                if ('<%=Activo.Checked%>' == 'False') 
                    return false;
            });

            //$('<%=Puesto.ClientID%>').attr('disabled', 'disabled');




            $('#<%=accMaritimo.ClientID%>').click(function (event) {
                return false;
            });

            $('#<%=Opciones.ClientID%>').click(function (event) {
                if ('<%=accMaritimo.Checked%>' == 'False') 
                    return false;
            });
            


            $('#<%=accApl.ClientID%>').click(function (event) {
                return false;
            });

            $('#<%=nivelbitapl.ClientID%>').click(function (event) {
                if ('<%=accApl.Checked%>' == 'False') 
                    return false;
            });

            



	        //validar que el operador tiene permisos para asignar permisos a usuario
            $('#<%=niveldua.ClientID%>').click(function (event) {
                <% If niveldua.SelectedIndex > -1 Then%>
                <% If Session("OperatorLevel") = 3 And niveldua.SelectedValue = 5 Then%>
                    alert('No tiene persimos suficientes');
                    return false;
                <% End If %>  
                <% End If %>                               
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                var checked = parseInt(valor_radio('<%=niveldua.ClientID%>'));
                //alert(OpeLev + ' ' + checked);
                if (OpeLev == 3 && checked == 5) {
                    alert('No tiene persimos suficientes');
                    return false;
                }
            });


            
            $('#<%=nivelbitapl.ClientID%>').click(function (event) {
                <% If nivelbitapl.SelectedIndex > -1 Then%>
                <% If Session("OperatorLevel") = 3 And nivelbitapl.SelectedValue = 5 Then%>
                    alert('No tiene persimos suficientes');
                    return false;
                <% End If %>  
                <% End If %>                               
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                var checked = parseInt(valor_radio('<%=nivelbitapl.ClientID%>'));
                //alert(OpeLev + ' ' + checked);
                if (OpeLev == 3 && checked == 5) {
                    alert('No tiene persimos suficientes');
                    return false;
                }
            });



            $('#<%=Opciones.ClientID%>').click(function (event) {
                <% If Opciones.SelectedIndex > -1 Then%>
                <% If Session("OperatorLevel") = 3 And Opciones.SelectedValue = "O" Then%>
                    alert('No tiene persimos suficientes');
                    return false;
                <% End If %>  
                <% End If %>                               
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                var checked = valor_checkbox('<%=Opciones.ClientID%>');
                //alert(OpeLev + ' ' + checked);
                if (OpeLev == 3 && checked == 'O') {
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
            /*
            if ('<%=Session("pestana")%>' == "aduana") {
                //if ($('#<%=Login.ClientID%>').val() == "") { alert("Seleccione Login"); return false; }
                //if ($('#<%=Nombre.ClientID%>').val() == "") { alert("Ingrese Nombre"); return false; }
                var checked = valor_radio('<%=niveldua.ClientID%>');
                if (checked == 999) { alert("Seleccione Nivel Dua"); return false; }     
                checked = valor_checkbox('<%=Empresa.ClientID%>');
                if (checked == 999) { alert("Seleccione al menos una Empresa"); return false; }   
            }
            if ('<%=Session("pestana")%>' == "apl") {
                var checked = valor_radio('<%=nivelbitapl.ClientID%>');
                if (checked == 999) { alert("Seleccione Nivel Bitacora APL"); return false; }                     
            }
            if ('<%=Session("pestana")%>' == "maritimo_c") {
                var checked = valor_radio('<%=Opciones.ClientID%>');
                if (checked == 999) { alert("Seleccione Nivel Maritimo"); return false; }   
            }
            */
            return true;
        }


// ]]>
    </script>
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="panel panel-info">
    
        <div class="panel-heading">        

            <asp:hiddenfield runat="server" id="codigo"  Visible="True"></asp:hiddenfield>

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

                    <% If active = False Then%>
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

            <div id="catalogo2" class="tabs_panel">

                    

                   <div id="aduana2" class="pestanas_customer">

                        <div style="width:35%;display:inline-block;vertical-align:top">


                           <div class="form-group row">
                                <label for="Login" class="col-sm-3 control-label">Login</label>
                                <div class="col-sm-6">                            
                                    <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login" ReadOnly="True"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                                <div class="col-sm-6">
                                    <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre"  ReadOnly="True"></asp:TextBox>
                                </div>                                
                            </div>  


                            <div class="form-group row">
                                <label for="Nombre" class="col-sm-3 control-label">Pais</label>
                                <div class="col-sm-6">
                                    <asp:TextBox class="form-control" id="Pais" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>         
                                </div>                                
                            </div>                          


                           <div class="form-group row">
                              <label for="email" class="col-sm-3 control-label">Dominio</label>
                              <div class="col-sm-6">
                                 <asp:TextBox class="form-control" id="Email" runat="server" placeholder="Ingrese Email" ReadOnly="True"></asp:TextBox>
                              </div>
                           </div>

                           <div class="form-group row">
                              <label for="Password" class="col-sm-3 control-label">Password</label>
                              <div class="col-sm-9">
                                 <asp:TextBox class="form-control" id="Password" runat="server" placeholder="" ReadOnly="True" ></asp:TextBox>
                              </div>
                           </div>

                            <div class="form-group row">
                                <label for="Nombre" class="col-sm-3 control-label">Locode</label>
                                <div class="col-sm-6">                                    
                                    <asp:TextBox class="form-control" id="Locode" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>        
                                </div>                                
                            </div>
                 
                        </div>

                        <div style="width:28%;display:inline-block;vertical-align:top">

                            <div class="form-group row">
                                <label for="nivelbitapl" class="col-sm-4 control-label">Nivel Dua</label>
                                <div class="col-sm-7">

                                    <asp:RadioButtonList ID="niveldua" runat="server" class="form-control">
                                    <asp:ListItem Value="5" Text="Operativo"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Consultas"></asp:ListItem>                                    
                                    </asp:RadioButtonList>

                                </div>                                
                            </div>

                            <br />
                            <br />


                           <div class="form-group row">
                              <label for="Puesto" class="col-sm-4 control-label">Puesto</label>
                              <div class="col-sm-7">
                                 <asp:TextBox class="form-control" id="Puesto" runat="server" placeholder="Ingrese Puesto"></asp:TextBox>
                              </div>
                           </div>

                           <div class="form-group row">
                              <label for="IP" class="col-sm-4 control-label">IP</label>
                              <div class="col-sm-7">
                                 <asp:TextBox class="form-control" id="IP" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>
                              </div>
                           </div>

                            <div class="form-group row">                          
                                <label for="Activo" class="col-sm-4 control-label">Activo</label>
                                <div class="col-sm-7">                                                              
                                    <asp:CheckBox ID="Activo" runat="server"  class="form-control" />
                                </div>                                
                            </div>


                        </div>

                        <div style="width:35%;display:inline-block;vertical-align:top">
                      
                            <div class="form-group row" style="width:90%">
                                <label for="Paises" class="col-sm-7 control-label">Accesos Empresas</label>
                                <div class="col-sm-10 checkbox">
                                    <asp:checkboxlist id="Empresa" runat="server" class="form-control">
                                    </asp:checkboxlist>
                                </div>
                            </div>

                            

                        </div>

                   </div>



                   <div id="apl2" class="pestanas_customer">
                                       
                        <div class="form-group row">
                            <label for="nivelbitapl" class="col-sm-3 control-label">Nivel</label>
                            <div class="col-sm-4">                             

                                <asp:RadioButtonList ID="nivelbitapl" runat="server" class="form-control">
                                <asp:ListItem Value="5" Text="Operativo"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Consultas"></asp:ListItem>                                    
                                </asp:RadioButtonList>

                            </div>
                        </div>
                        <br />
                        <br />
                 
                        <div class="form-group row">                          
                            <label for="accApl" class="col-sm-3 control-label">Activo</label>
                            <div class="col-sm-4">                          
                                <asp:CheckBox ID="accApl" runat="server"  class="form-control" />                          
                            </div>
                        </div>

                   </div>

                    <div id="maritimo_c2" class="pestanas_customer">
    
                            <div class="form-group row">
                                <label for="Opciones" class="col-sm-3 control-label">Nivel</label>
                                <div class="col-sm-4">       
                                                   
                                    <asp:RadioButtonList ID="Opciones" runat="server" class="form-control">
                                    <asp:ListItem Text="Operativo" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Consultas" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>

                                </div>
                            </div>
                            <br />
                            <br />

                            <div class="form-group row">
                                <label for="accMaritimo" class="col-sm-3 control-label">Activo</label>
                                <div class="col-sm-4">                          
                                    <asp:CheckBox ID="accMaritimo" runat="server"  class="form-control" />                          
                                </div>
                            </div>
                   
                    </div>

            </div> <!-- catalogo2 -->

        </div>

    </div>



</asp:Content>

