<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ct_usuarios.aspx.vb" Inherits="ct_usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script language="javascript" type="text/javascript">
// <![CDATA[

        function valida_eliminar() {
            if (!confirm("Confirme Desactivar Este Registro")) {
                return false;
            }
        }
        
        function valida() {
            var err = "";
            var campo = "";
            if ($('#<%=Password.ClientID%>').val() == "") { err = "Ingrese Password"; campo = '#<%=Password.ClientID%>'; }
            if ($('#<%=Nombre.ClientID%>').val() == "") { err = "Ingrese Nombre"; campo = '#<%=Nombre.ClientID%>'; }
            if ($('#<%=Login.ClientID%>').val() == "") { err = "Ingrese Login"; campo = '#<%=Login.ClientID%>'; }
            if ($('#<%=Nivel.ClientID%>').val() == "") { err = "Seleccione Nivel"; campo = '#<%=Nivel.ClientID%>'; }
            if ($('#<%=aimar_code.ClientID%>').val() == "") { err = "No hay codigo de la master"; campo = '#<%=aimar_code.ClientID%>'; }
            if (err != "") {
                alert(err);
                $(campo).focus();
                return false;
            } else {
                return true;
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
            if ($('#<%=codigo.ClientID%>').val() == "") { alert("No puede actualizar"); return false; }
            var res = valida();
            if (res == true) {
                //console.log('si entro');
                $('#modal-texto').html('Espere mientras se guadan cambios');
                $('#myModal').modal();
                return true;            
            } else {            
                return false;            
            }
        }

        $(function () {
            $('#<%=Nivel.ClientID%>').focus();
            <% If Session("insert") = Nothing And Session("login_id") = Nothing Then%>
            $('.breadcrumb').append('<li><%=Licon_keys%></li>');
            <% Else%>
            //$('.breadcrumb').append('<li><a class="text-primary"><%=Licon_keys%></a></li>');
            $('.breadcrumb').append('<li><%=Licon_keys%></li>');
            <% End If%>
            
            <% If msg <> "" Then 
                msg = msg.Replace("'", "")
                msg = msg.Replace(vbCrLf, "<br>")
            %>
                $('#modal-texto').html('<%=msg%>');
                $('#modal-img').html('<%=img%>');                
                $('#myAlert').addClass('<%=css%>');                
                $('#myModal').modal();
            <% End If %>  

            $('#<%=Activo.ClientID%>').click(function(){
                return false;
            });

            if ('<%=Nivel.SelectedValue%>' == 3) {
                $('#<%=Demo.ClientID%>').click(function(){
                    return false;
                });            
            }

//            <% 'If Activo.Checked = false And Session("login_id") > 0 Then%>            
//                $('input[type=text]').attr('disabled', 'disabled');
//                $('input[type=checkbox]').attr('disabled', 'disabled');                
//                $('select').attr('disabled', 'disabled');
//                $('#lnk_check').attr('readonly', 'readonly');            
//            <% 'End If %> 

            var tmp = 0;
            $('#<%=Nivel.ClientID%>').click(function (event) {
                tmp = event.currentTarget.selectedIndex;
            });

            //validar que el operador tiene permisos para asignar permisos a usuario
            $('#<%=Nivel.ClientID%>').change(function (event) {                
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                //console.log(OpeLev + ' ' + event.currentTarget.selectedIndex + ' ' + event.currentTarget.value );
                if (OpeLev > event.currentTarget.value) {                    
                    event.currentTarget.selectedIndex = tmp;                   
                }                                             
            });

//            $("#tab_keys").click(function (event) {//funciona para todos los menu                
//                $('#< %=btn_cancelar.ClientID%>').submit();
//            });

//            $("a").click(function (event) {//funciona para todos los menu
//                alert('evento lnk1=' + event.currentTarget.name);
//                if (event.currentTarget.name == 'lnk1_') {
//                alert('test');
//                    $('#< %=opcion1_txt.ClientID%>').removeAttr('disabled');
//                    $('#< %=opcion1_txt.ClientID%>').val(event.currentTarget.id);
//                    $('#< %=opcion1_btn.ClientID%>').click();
//                }
//            });
        });                      

//        function abre_modal(){
//            $('#< %=btn_abre.ClientID%>').click();
//            //$('#myModalMenu').modal();
//        }

// ]]>
    </script>


    <style type="text/css">                  
        #<%=users_grid.ClientID%> thead { display:block; width:100%; background:#D9EDF7; border-bottom:1px solid #428BCA; }
        #<%=users_grid.ClientID%> tbody { display:block; width:100%; overflow:auto; height:350px; }        
        #<%=users_grid.ClientID%> th { height:20px; width:150px; text-align:left; border:0px; }
        #<%=users_grid.ClientID%> td { height:20px; width:150px; text-align:left; /*white-space: nowrap; */}        
    </style>


</asp:Content>





<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%--
        <asp:Button ID="opcion1_btn" runat="server" Text="Button" style="display:none"></asp:Button>
        <asp:TextBox ID="opcion1_txt" runat="server" style="display:none"></asp:TextBox>
--%>

   <div class="panel panel-success">

        <div class="panel-heading">      
            <%--<asp:Button ID="btn_abre" runat="server" Text="Button" style="display:none"/>--%>
        </div>

        <div class="col-sm-offset-5 form-group row boton_bar">
            <% If Session("new") = True Then%>   
                <%-- <asp:LinkButton ID="btn_cancelar" runat="server" CssClass="btn btn-sm btn-primary" style="display:none"><%=Licon_keys%></asp:LinkButton>&nbsp;  --%>
                
                <asp:TextBox ID="buscar_nombre" class="form-control-feedback text-primary" AutoComplete="False"  runat="server" placeholder="Buscar Usuario" MaxLength="20" Width="150px" ></asp:TextBox>
                <asp:LinkButton ID="btn_buscar" runat="server" CssClass="btn-sm"><%=Licon_search%></asp:LinkButton>                    
            <% Else%>                                           

                <% If Session("insert") = Nothing And Session("login_id") = Nothing Then%>
<%--                   
                    <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                        <a onmouseover=this.style.cursor='pointer' class='btn btn-sm btn-default' title='Agregar Usuario de la Master' onclick="abre_modal()" ><%=Licon_new%></a>
                    <% Else%>                    
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-success disabled"><%=Licon_new%></asp:LinkButton>
                    <% End If%>
--%>
                    <span style="height:29px;display:block;" />&nbsp;</span>

                <% Else%>
                    
<%--                
                    <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                        <asp:LinkButton ID="btn_list" runat="server" style="display:none"></asp:LinkButton>    
                    <% Else%>                    
                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-sm btn-success disabled"><%=Licon_keys%></asp:LinkButton>
                    <% End If%>
--%>
                                        
                    <% If Session("insert") = True Then%>
                        <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                            <asp:LinkButton ID="btn_agregar" runat="server" CssClass="btn btn-sm btn-success" onclientclick="return valida_agregar()"><%=Licon_insert%></asp:LinkButton>
                        <% Else%>                    
                            <asp:LinkButton ID="LinkButton6" runat="server" CssClass="btn btn-sm btn-success disabled"><%=Licon_insert%></asp:LinkButton>
                        <% End If%>
			        <% Else%>			            
                        <% If Activo.Checked = True Then%>
                            <% If Session("OperatorLevel") <> Nothing Then%>
                                <asp:LinkButton ID="btn_actualizar" runat="server" CssClass="btn btn-sm btn-success" onclientclick="return valida_actualizar()"><%=Licon_update%></asp:LinkButton>
                            <% Else%>                    
                                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-sm btn-success disabled"><%=Licon_update%></asp:LinkButton>
                            <% End If%>
                            <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                                <asp:LinkButton ID="btn_eliminar" runat="server" CssClass="btn btn-sm btn-success" onclientclick="return valida_eliminar()"><%=Licon_off%></asp:LinkButton>
                            <% Else%>                    
                                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-sm btn-success disabled"><%=Licon_off%></asp:LinkButton>
                            <% End If%>
                        <% Else%>
                            <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                                <asp:LinkButton ID="btn_activar" runat="server" CssClass="btn btn-sm btn-success" onclientclick="return valida_activar()"><%=Licon_on%></asp:LinkButton>
                            <% Else%>                    
                                <asp:LinkButton ID="LinkButton5" runat="server" CssClass="btn btn-sm btn-success disabled"><%=Licon_on%></asp:LinkButton>
                            <% End If%>
                        <% End If%>                                
    			    <% End If%>                                            
                <% End If%>
            <% End If%>
        </div>

        
        
        <div class="panel-body">

            

        <ul id="pestana" class="nav nav-tabs">       
        
        <% If Session("OperatorLevel") = 1 Then%>
            <li id="li1_keys"
            <% If Session("new") = True Then%>                       
            <% Else%>                            
                <% If Session("insert") = Nothing And Session("login_id") = Nothing Then%>            
                         class="active"
                <% Else%>                
                <% End If%>                            
            <% End If%>                   
            >
            <a id="a_keys" name="lnk_" onmouseover="this.style.cursor='pointer'" style="cursor: pointer;"><%=Licon_keys%></a></li>
        <% Else%>
            <li id="Li1" class="disabled"><a id="A1" name=""><%=Licon_keys%></a></li>
        <% End If%>
                     
        
                     
                     
        <% If Session("OperatorLevel") = 1 Then%>
        <% 'If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>                        
            <li id="li1_new_l"
                <% If Session("new") = True Then%>                       
                    class="active"
                <% End If%>                   
            >
            <a id="a_new_l" name="lnk_" onmouseover="this.style.cursor='pointer'" style="cursor: pointer;"><%=Licon_new%></a>
            </li>
        <% Else%>
            <li id="" class="disabled"><a id="" name=""><%=Licon_new%></a></li>
        <% End If%>
        
            
                                            

        <% If Session("new") = True Then%>
                    
            </ul>
       
            <div class="tabs_panel" style="padding:10px">
                <asp:GridView ID="users_grid" runat="server"  CssClass="table table-hover table-striped input-sm text-primary" GridLines="None">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>                                                                        
                                <asp:LinkButton ID="lnk_user1" runat="server" OnClick="imageButtonClick_master" Height="20" Width="20"><%=Licon_user%></asp:LinkButton>                        
                                <asp:Image ID="lnk_flag1" runat="server" Height="20" />
                            </ItemTemplate>
                        </asp:TemplateField>                               
                    </Columns>
                </asp:GridView>
           </div>

        <% Else%>                    

            <% If Session("insert") = Nothing And Session("login_id") = Nothing Then%>
                
                </ul>

                <div class="tabs_panel" style="padding:10px">
                    <asp:GridView ID="login_grid" runat="server"  CssClass="table table-hover table-striped input-sm text-primary" GridLines="None" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>                                
                                    <asp:LinkButton ID="lnk_user" runat="server" OnClick="imageButtonClick" Height="20" Width="20"><%=Licon_user%></asp:LinkButton>
                                    <asp:Image ID="lnk_flag" runat="server" Height="20" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
               </div>

            <% Else%>
            
                <li id="li1_login" class="active"><a id="a_login" name="lnk1_" onmouseover="this.style.cursor='pointer'"><%=Login.Text%></a></li>
                </ul>


                <div class="tabs_panel">

                    <div style="width:35%;display:inline-block;vertical-align:top">

                        <div class="form-group row">
                            <label for="codigo" class="col-sm-3 control-label">ID</label>
                            <div class="col-sm-7">
                                <asp:TextBox class="form-control" id="codigo" runat="server" placeholder="Registro Nuevo" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    
                        <div class="form-group row">
                            <label for="codigo" class="col-sm-3 control-label">Master</label>
                            <div class="col-sm-7">
                                <asp:TextBox class="form-control" id="aimar_code" runat="server" ReadOnly="True"></asp:TextBox>
                            </div>
                        </div>
                    
                        <div class="form-group row">
                            <label for="Nivel" class="col-sm-3 control-label">Nivel</label>
                            <div class="col-sm-7">                             
                                <asp:DropDownList ID="Nivel" runat="server" class="form-control">                        
                                <asp:ListItem Value="">Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">Root</asp:ListItem>
                                <asp:ListItem Value="2">Admin</asp:ListItem>
                                <asp:ListItem Value="3">Editor</asp:ListItem>                        
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="Login" class="col-sm-3 control-label">Login</label>
                            <div class="col-sm-7">
                                <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login (usuario)"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                            <div class="col-sm-7">
                                <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre Completo"></asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div style="width:35%;display:inline-block;vertical-align:top">

                        <div class="form-group row">                                         
                            <label for="Pais" class="col-sm-2 control-label">Pais</label>                                     
                            <div class="col-sm-7">
                                <asp:DropDownList ID="Pais" runat="server" class="form-control">                                                                
                                </asp:DropDownList>
                            </div>
                        </div>

    

                        <div class="form-group row">
                            <label for="Password" class="col-sm-2 control-label">Password </label>
                            <div class="col-sm-9">                                                 
                    

                            <asp:TextBox class="form-control" id="Password" runat="server" placeholder="Ingrese Password"></asp:TextBox>                                       

                                <asp:LinkButton ID="gen_pass" runat="server" CssClass="btn btn-sm btn-default" title="Generar Password"><%=Licon_pasgen%>&nbsp;Genera</asp:LinkButton>
        <%--                    <asp:LinkButton ID="enc_pass" runat="server" CssClass="btn btn-sm btn-default" title="Encrypta Password"><%=Licon_pasenc%>&nbsp;Encrypta</asp:LinkButton> --%>                        
                                <asp:LinkButton ID="can_pass" runat="server" CssClass="btn btn-sm btn-default" title="Cancela Password"><%=Licon_pascan%>&nbsp;Cancela</asp:LinkButton>

                            </div>                                        
                        </div>

                        <div class="form-group row">                          
                            <label for="Demo" class="col-sm-2 control-label">Demo</label>
                            <div class="col-sm-7">                               
                                <asp:CheckBox ID="Demo" runat="server"  class="form-control" />                        
                            </div>
                        </div>

                        <div class="form-group row">                          
                            <label for="Activo" class="col-sm-2 control-label">Activo</label>
                            <div class="col-sm-7">                               
                                <asp:CheckBox ID="Activo" runat="server"  class="form-control" />                        
                            </div>
                        </div>

                    </div>

                    <div style="width:25%;display:inline-block;vertical-align:top">

                        <% If Activo.Checked = True Then%>

                            <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>

                                <asp:LinkButton ID="lnk_check" runat="server">Modulos</asp:LinkButton>

                                <div class="checkbox">                        
                                    <asp:checkboxlist id="Modulos" runat="server" ></asp:checkboxlist>
                                </div>

                                <asp:CheckBoxList ID="CheckBoxList1" runat="server" style="display:none">                
                                </asp:CheckBoxList>

                            <% End If%>

                        <% End If%>

                    </div>
                </div>


            <% End If%>
   

        <% End If%>


        
 

        </div>

    </div>








</asp:Content>

