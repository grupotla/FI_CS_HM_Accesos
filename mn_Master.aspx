<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Master.aspx.vb" Inherits="mn_Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(function () {

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

            //$('#<%=tipo_usuario.ClientID%>').focus();
            if ('<%=Session("edit")%>' == '' && '<%=Session("DBAccesosUserId")%>' != '') {                
                $('input[type=text]').attr('disabled', 'disabled');
                $('select').attr('disabled', 'disabled');
                $('input[type=checkbox]').attr('disabled', 'disabled');
                $('input[type=radio]').attr('disabled', 'disabled');
                $('#<%=gen_pass.ClientID%>').hide();
                //$('#< %=enc_pass.ClientID%>').hide();
                $('#<%=can_pass.ClientID%>').hide();
            }

            $('#<%=Correo.ClientID%>').click(function (event) {
                var checked = valor_radio($(this).attr('id'));
                //console.log('Edit=' + '<%=Session("edit")%>');
                //console.log('Correo=' + '<%=Correo.SelectedValue%>');
                //console.log('checked=' + checked);

                //< % If Session("edit") <> Nothing And Session("DBAccesosUserId") <> Nothing Then%>   

                //if ('<%=Session("edit")%>' == '' || '<%=Correo.SelectedValue %>' == checked || checked == 999){                    
                    //return false;
                //}                
                $('#<%=correo_click.ClientID%>').click();                
            });

            //$('#ctl00_ContentPlaceHolder1_Activo').attr('disabled', 'disabled');

            $('#usuario2').hide();
            $('#catalogo2').hide();
            $('#exactus2').hide();

            if ('<%=Session("pestana")%>' == "usuario") {
                $('#usuario2').show();
            }

            if ('<%=Session("pestana")%>' == "catalogo") {
                $('#catalogo2').show();
            }

            if ('<%=Session("pestana")%>' == "exactus") {
                $('#exactus2').show();
            }

            if ('<%=Session("DBAccesosUserId")%>' == "") { //al insertar registro desabilita la pestaña de catalogo 
                $('#catalogo').hide();
            }
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
                return valida()
            }
        }

        function valida_activar() {
            if (!confirm("Confirme Activar Este Registro")) {
                return false;
            }
        }

        function valida_actualizar() {
            //if ($('#<%=codigo.ClientID%>').val() == "") { alert("No puede actualizar"); return false; }
            if (valida() == true) {                
                $('#modal-texto').html('Espere mientras se guardan los cambios');
                $('#myModal').modal();
                return true;            
            } else {            
                return false;            
            }
        }

        function valida() {                       
            if ($('#<%=Login.ClientID%>').val() == "") { alert("Ingrese Login"); return false; }
            if ($('#<%=Nombre.ClientID%>').val() == "") { alert("Ingrese Nombre"); return false; }
            if ($('#<%=Dominio.ClientID%>').val() == "") { alert("Ingrese Dominio"); return false; }
            var checked = valor_radio('<%=Correo.ClientID%>');            
            if (checked == 999) { alert("Tiene Correo ?"); return false; }
            if ($('#<%=Password.ClientID%>').val() == "") { if (checked == 0) { alert("Genere Password"); } else  { alert("Ingrese Password"); } return false; }    
            //if (checked == 0)             
                //alert('Antes de Guardar tome nota del password generado ' + $('#<%=Password.ClientID%>').val());            
            //if ('<%=locode.SelectedIndex%>' == '-1') { alert("Seleccione Ubicacion"); return false; }
            return true;
        }

        function valida_clonar() {
            if (confirm("Confirme Clonar Este Registro")) {

                if (!confirm("Clonar a un usuario nuevo ? ")) {

                    if (confirm("Clonar a un usuario con login existente en el sistema ?")) {
                              
                        var usuario = prompt('Ingrese el Login del usuario', 'pepitoperez');
                        var temp = '('+usuario+')';
                        if (temp == '(null)') {

                            return false;

                        } else {

                            $('#<%=opcion_txt.ClientID%>').removeAttr('disabled');                            
                            $('#<%=opcion_txt.ClientID%>').val(usuario);                                                                                    
                            return true;
                        } 

                        return false;                        

                    } else {
                        return false;
                    }          
                              
                } else {
                    $('#<%=opcion_txt.ClientID%>').val('');
                    return true;
                }
                
            } else {
                return false;
            }
        }         

// ]]>
    </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    

    
    <asp:TextBox ID="opcion_txt" runat="server" style="display:none"></asp:TextBox> <!-- se usa para obtener el valor del login a clonar -->

    <asp:Button ID="correo_click" runat="server" Text="Button" style="display:none" UseSubmitBehavior="False" />

    <div class="panel panel-info">
        
        <div class="panel-heading">  

            <asp:hiddenfield runat="server" id="codigo" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_login" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_nombre" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_password" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_dominio" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_level" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_pais" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_email" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_correo" Visible="True"></asp:hiddenfield>
            <asp:hiddenfield runat="server" id="bk_tipo" Visible="True"></asp:hiddenfield>            
            <asp:hiddenfield runat="server" id="bk_locode" Visible="True"></asp:hiddenfield>            
            <asp:hiddenfield runat="server" id="bk_reporte" Visible="True"></asp:hiddenfield>  

            <asp:hiddenfield runat="server" id="PaisesContactos_bk" Visible="True"></asp:hiddenfield>            
            <asp:hiddenfield runat="server" id="TipoContactos_bk" Visible="True"></asp:hiddenfield>            
            <asp:hiddenfield runat="server" id="Accion_bk" Visible="True"></asp:hiddenfield>  
            <asp:hiddenfield runat="server" id="EstadoContactos_bk" Visible="True"></asp:hiddenfield>  

        </div>

            <div class="col-sm-offset-7 form-group row boton_bar">        
            
            <%'=Session("edit") & ")(" & Session("DBAccesosUserId")%>
            
                    
                    <% If Session("edit") = "edit" And Session("DBAccesosUserId") <> Nothing Then%>
                        
                        
                    <% Else%>
                    <% End If%>                                                    
                    
                       
                    <%'=Session("edit") & ")(" & Session("DBAccesosUserId")%>

                    <% If Session("edit") <> Nothing And Session("DBAccesosUserId") <> Nothing Then%>   

                            <% If Activo.Checked = True Then%>
                        
                                    <asp:LinkButton ID="btn_cancelar" runat="server" CssClass="btn btn-sm btn-primary"><%=Licon_cancel%></asp:LinkButton>
                                
                                    <% If Session("OperatorLevel") = 1 Then%>
                                        <asp:LinkButton ID="btn_actualizar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_actualizar()"><%=Licon_update%></asp:LinkButton>
                                    <% Else%>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_update%></asp:LinkButton>
                                    <% End If%>                                                         

                                    <% If Session("pestana") = "usuario" Then%>
                                        <% If Session("OperatorLevel") = 1 Then%>
                                            <asp:LinkButton ID="btn_eliminar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_eliminar()"><%=Licon_off%></asp:LinkButton>
                                        <% Else%>
                                            <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_off%></asp:LinkButton>
                                        <% End If%>
                                    <% End If%>
  
                             <% Else%>

                            <% End If%>   

                    <% Else%>

                         <% If Activo.Checked = True Then%>                         

                                <% If Session("DBAccesosUserId") = Nothing Then%>
              
                                 <% Else%>			                                                                              
                                             
                                    <% If Session("OperatorLevel") = 1 Then%>
                                        <asp:LinkButton ID="btn_editar" runat="server" CssClass="btn btn-sm btn-primary"><%=Licon_edit%></asp:LinkButton>
                                        <% If Session("pestana") = "usuario" Then%>
                                            <asp:LinkButton ID="btn_clonar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_clonar()"><%=Licon_clonar%></asp:LinkButton>
                                        <% End If%>                                                                                         
                                    <% Else%>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_edit%></asp:LinkButton>                                
                                        <% If Session("pestana") = "usuario" Then%>                                    
                                            <asp:LinkButton ID="LinkButton8" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_clonar%></asp:LinkButton>
                                        <% End If%>                                                         
                                    <% End If%>   
                                                                                                                                                         
			                     <% End If%>    
								 
                        <% Else%>   
  
                                <% If Session("DBAccesosUserId") = Nothing Then%>
                                                
                                    <% If Session("OperatorLevel") = 1 Then%>
                                        <asp:LinkButton ID="btn_agregar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_agregar()"><%=Licon_insert%></asp:LinkButton>
                                    <% Else%>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_insert%></asp:LinkButton>
                                    <% End If%>   

                                 <% Else%>		

                                    <% If Session("pestana") = "usuario" Then%>
                                        <% If Session("OperatorLevel") = 1 Then%>
                                            <asp:LinkButton ID="btn_activar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_activar()"><%=Licon_on%></asp:LinkButton>
                                        <% Else%>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_on%></asp:LinkButton>
                                        <% End If%>                            
			                        <% End If%>  

                                 <% End If%>

                         <% End If%>                          

                    <% End If%>   



                                           
            </div>


        <div class="panel-body">    

            <!-- aqui va el menu tabs de manifiestos generado desde el codigo-->
            <asp:Label ID="pestana_lbl" runat="server" Text="Label"></asp:Label>

            <div id="catalogo2" class="tabs_panel">

                    <div class="form-group row">
                        <label for="Nivel" class="col-sm-3 control-label">Nivel</label>
                        <div class="col-sm-3">                                     
                            <asp:RadioButtonList ID="Nivel" runat="server" class="form-control">
                            <asp:ListItem Value="4" Text="Administrador"></asp:ListItem>                                                
                            <asp:ListItem Value="3" Text="Supervisor"></asp:ListItem>                        
                            <asp:ListItem Value="2" Text="Operativo"></asp:ListItem>                        
                            <asp:ListItem Value="1" Text="Consultas"></asp:ListItem>
                            <asp:ListItem Value="0" Text="--"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

            </div>

            <div id="usuario2" class="tabs_panel">
                    
                <div style="width:48%;display:inline-block;vertical-align:top">

                    <div class="form-group row">
                        <label for="Login" class="col-sm-3 control-label">Login</label>
                        <div class="col-sm-6">
                            <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                        <div class="col-sm-6">
                            <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">                                         
                        <label for="Pais" class="col-sm-3 control-label">Pais</label>                                     
                        <div class="col-sm-6">
                            <asp:DropDownList ID="Pais" runat="server" class="form-control" AutoPostBack="True">                                                                
                            </asp:DropDownList>
                        </div>
                    </div>
                
                    <div class="form-group row">
                        <label for="Email" class="col-sm-3 control-label">Email</label>
                        <div class="col-sm-6">
                            <asp:TextBox class="form-control" id="Email" runat="server"  ReadOnly="true" ></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Password" class="col-sm-3 control-label">Password</label>
                        <div class="col-sm-7">                                                 
                            <asp:TextBox class="form-control" id="Password" runat="server" placeholder="Genere o Ingrese Password"></asp:TextBox>                        
                        </div>                                 
                    </div>

                    <% If Password.Text.Length = 10 and Session("Edit") <> Nothing Then%>
                        <div class="col-sm-10 col-sm-offset-3" style="color:red">
                            Anotar el password no encriptado antes de guardar
                        </div>                                 
                    <% End If%>


                    <div class="form-group row">                          
                        <label for="Correo" class="col-sm-3 control-label">Reportes Contables</label>
                        <div class="col-sm-6">                                                                              
                                <asp:RadioButtonList ID="RadioButtonListRepCont" runat="server"  class="form-control">
                                <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>                            
                        </div>
                    </div>                 
      
                    <% If Session("OperatorID") = 1237 Then %>
                    <div class="form-group row">                          
                        <label for="FechaVencida" class="col-sm-3 control-label">Fecha Vencida</label>
                        <div class="col-sm-6">                               
                            <asp:CheckBox ID="FechaVencida" runat="server"  class="form-control" />                        
                        </div>
                    </div>
                    <% End If%>


                    

                    <div class="form-group row">
                        <label for="Password" class="col-sm-3 control-label"></label>
                        <asp:TextBox id="randomized" runat="server" style="display:none"></asp:TextBox>    
                        <asp:LinkButton ID="can_pass" runat="server" CssClass="btn btn-sm btn-default" title="Cancela Password"><%=Licon_pascan%>&nbsp;Cancela</asp:LinkButton>
                        <asp:LinkButton ID="gen_pass" runat="server" CssClass="btn btn-sm btn-default" title="Generar Password"><%=Licon_pasgen%>&nbsp;Genera</asp:LinkButton>
<%--
                        <asp:LinkButton ID="enc_pass" runat="server" CssClass="btn btn-sm btn-default" title="Encrypta Password"><%=Licon_pasenc%>&nbsp;Encrypta</asp:LinkButton>
--%>                    
                    </div>


                </div>

                <div style="width:48%;display:inline-block;vertical-align:top">

                   <div class="form-group row">
                        <label for="tipo_usuario" class="col-sm-3 control-label">Definicion</label>
                        <div class="col-sm-6">                             
                            <asp:DropDownList ID="tipo_usuario" runat="server" class="form-control">                        
                            </asp:DropDownList>
                        </div>
                    </div>

                            <!-- <asp:ListItem Value="0">Seleccione</asp:ListItem>
                            <asp:ListItem Value="1">Ventas</asp:ListItem>
                            <asp:ListItem Value="2">2 pend descrip</asp:ListItem>
                            <asp:ListItem Value="3">3 pend descrip</asp:ListItem>
                            <asp:ListItem Value="4">4 pend descrip</asp:ListItem>
                            <asp:ListItem Value="5">5 pend descrip</asp:ListItem>
                            <asp:ListItem Value="7">7 pend descrip</asp:ListItem>
                            <asp:ListItem Value="8">8 pend descrip</asp:ListItem>
                            <asp:ListItem Value="9">9 pend descrip</asp:ListItem>
                            <asp:ListItem Value="11">Customer Regional</asp:ListItem> -->
                                          
                    <div class="form-group row">
                        <label for="Dominio" class="col-sm-3 control-label">Dominio</label>
                        <div class="col-sm-6">

                            <asp:DropDownList ID="Dominio" runat="server" class="form-control">                                                                
                            </asp:DropDownList>

                        </div>
                    </div>

                    <div class="form-group row">                          
                        <label for="Correo" class="col-sm-3 control-label">Obtener Password desde Correo ?</label>
                        <div class="col-sm-6">                                                                              
                                <asp:RadioButtonList ID="Correo" runat="server"  class="form-control">
                                <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>                            
                        </div>
                    </div>                 
                    <br />
                    <br />


                    <div class="form-group row">                                         
                        <label for="locode" class="col-sm-3 control-label">Ubicacion</label>

<%--                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>--%>

<%--                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>--%>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="locode" runat="server" class="form-control">                                                                
                                        </asp:DropDownList>
                                    </div>
<%--                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Pais" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
--%>                
                    </div>

                    <div class="form-group row">                          
                        <label for="Activo" class="col-sm-3 control-label">Activo</label>
                        <div class="col-sm-6">                               
                            <asp:CheckBox ID="Activo" runat="server"  class="form-control" />                        
                        </div>
                    </div>



                    <div class="form-group row">                          
                        <label for="Correo" class="col-sm-3 control-label">Apertura Costos</label>
                        <div class="col-sm-6">                                                                              
                                <asp:RadioButtonList ID="exactus_pagos" runat="server"  class="form-control">
                                <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                </asp:RadioButtonList>                            
                        </div>
                    </div>   


                </div>

            </div>




            <div id="exactus2" class="tabs_panel">
    
                <table border="0">
                <tr>
                    <td>&nbsp;&nbsp;</td>
                    <td width="5%" valign="bottom">
                        <label for="PaisesContactos" class="col-sm-0 control-label">Id</label>    
                       <div class="col-sm-0">
                            <asp:TextBox class="form-control" id="exactus_correo_id" runat="server"  ReadOnly="true" ></asp:TextBox>
                        </div>
                    </td>
                    <td>&nbsp;&nbsp;</td>
                    <td width="30%" valign="bottom">                        
                        <label for="PaisesContactos" class="col-sm-0 control-label">Pais</label>                                     
                        <div class="col-sm-0">
                            <asp:DropDownList ID="PaisesContactos" runat="server" class="form-control" AutoPostBack="True">                                                                
                            </asp:DropDownList>
                        </div></td>
                    <td>&nbsp;&nbsp;</td>
                    <td width="25%" valign="bottom">                        
                        <label for="TipoContactos" class="col-sm-0 control-label">Tipo Contacto</label>
                        <div class="col-sm-0">
                            <asp:DropDownList ID="TipoContactos" runat="server" class="form-control" AutoPostBack="True">                                                                
                            <asp:ListItem Value="" Text="-- Seleccione --"></asp:ListItem>                                                
                            <asp:ListItem Value="0" Text="No recibe emails"></asp:ListItem>                                                
                            <asp:ListItem Value="1" Text="Send (to)"></asp:ListItem>                                                
                            <asp:ListItem Value="2" Text="Carbon Copy (cc)"></asp:ListItem>                        
                            <asp:ListItem Value="3" Text="Blind Copy (bc)"></asp:ListItem>    
                            </asp:DropDownList>
                        </div>

                    </td>
                    <td>&nbsp;&nbsp;</td>
                    <td width="20%" valign="bottom">

                        <label for="TipoContactos" class="col-sm-0 control-label" style="width:25px;">Accion</label>
                        <div class="col-sm-0">
                            <asp:DropDownList ID="Accion" runat="server" class="form-control" AutoPostBack="True">                                                                
                            <asp:ListItem Value="" Text="-- Seleccione --"></asp:ListItem>                                                
                            <asp:ListItem Value="1" Text="Excel"></asp:ListItem>                                                
                            <asp:ListItem Value="2" Text="Alerta"></asp:ListItem>                                                
                            <asp:ListItem Value="3" Text="Notificacion"></asp:ListItem>                           
                            </asp:DropDownList>
                        </div>

                    </td>
                    <td width="5%" valign="bottom">
                          <div class="col-lg-2">
                            <div class="input-group">
                              <asp:TextBox class="form-control">Activo</asp:TextBox>      
                              <span class="input-group-addon btn btn-primary">
                                <asp:CheckBox ID="EstadoContactos" runat="server" />   
                              </span>        
                            </div><!-- /input-group -->
                          </div><!-- /.col-lg-6 -->

                    </td>
                    <td width="5%" valign="bottom">
                       
                          <div class="col-lg-2">
                            <div class="input-group">
                              <asp:TextBox class="form-control">Borrar</asp:TextBox>      
                              <span class="input-group-addon btn btn-primary">
                                <asp:CheckBox ID="RadioButtonList1" runat="server" />   
                              </span>        
                            </div><!-- /input-group -->
                          </div><!-- /.col-lg-6 -->
                    </td>
     
                </tr>
                </table>

                    <div class="form-group row">                                
                               

                          
       

          

<!--           
                        <label for="EstadoContactos" class="col-sm-1 control-label">Estado</label>
                        -->




 

                    </div>

                </table>

    
                <div style="width:100%;display:inline-block;vertical-align:top">

                    <asp:GridView ID="users_grid" runat="server" CssClass="table table-hover table-striped input-sm text-primary" GridLines="None"> 
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>                                                                        
                                    <asp:LinkButton ID="lnk_user" runat="server" OnClick="imageButtonClick" Height="20" Width="40"><%=Licon_edit%></asp:LinkButton>                        
                                </ItemTemplate>
                            </asp:TemplateField>                   
                        </Columns>
                    </asp:GridView>

                </div>

            </div>




        </div>

    </div>


</asp:Content>


