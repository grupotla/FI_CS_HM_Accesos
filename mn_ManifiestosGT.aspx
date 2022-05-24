<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_ManifiestosGT.aspx.vb" Inherits="mn_ManifiestosGT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script language="javascript" type="text/javascript">
// <![CDATA[

        $(function () {
//            // Document is ready
//            if ($('#<%=Nombre.ClientID%>'))
//                $('#<%=Nombre.ClientID%>').focus();
            //$('#<%=Activo.ClientID%>').attr('disabled', 'disabled');
//            if ('<%=Activo.Checked%>' == 'False') {
//                //$('input[type=checkbox]').attr('disabled', 'disabled');
//                $('input[type=text]').attr('disabled', 'disabled');
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
        });

//        function check_activo() {
//            console.log($('#<%=Activo.ClientID%>').attr("checked"));
//            if ($('#<%=Activo.ClientID%>').attr("checked") == undefined) {
//                if ($('#<%=Pais.ClientID%>').val() == "") { alert("Ingrese Pais"); return false; }
//                if ($('#<%=Nombre.ClientID%>').val() == "") { alert("Ingrese Nombre"); return false; }
//                if ($('#<%=Login.ClientID%>').val() == "") { alert("Seleccione Login"); return false; }               
//            }
//            $('#<%=btn_activo.ClientID%>').click();
//        }

        function valida_eliminar() {
            /*if (!confirm("Confirme Desactivar Este Registro")) {
            return false;
            }*/
        }

        function valida_agregar() {
            if (!confirm("Confirme Crear Este Registro")) {
                return false;
            } else {
                return valida_actualizar()
            }
        }

        function valida_activar() {
            /*if (!confirm("Confirme Activar Este Registro")) {
                return false;
            }*/
        }

        function valida_actualizar() {
            var checked = valor_checkbox('<%=modulos.ClientID%>');
            //if (checked == 999) { alert("Seleccione al menos un Modulo"); return false; }              
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
                            <!-- <asp:LinkButton ID="btn_eliminar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_eliminar()"><%=Licon_off%></asp:LinkButton>             -->
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

                <div style="width:49%;display:inline-block;vertical-align:top">

                    <asp:Button ID="btn_activo" runat="server" Text="Button" style="display:none" />
                         
                    


                    <div class="form-group row">
                        <label for="codigo" class="col-sm-3 control-label">Codigo</label>
                        <div class="col-sm-6">                            
                            <asp:TextBox class="form-control" id="codigo" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Login" class="col-sm-3 control-label">Login</label>
                        <div class="col-sm-6">                            
                            <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                        <div class="col-sm-6">
                            <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ReadOnly="True" ></asp:TextBox>
                        </div>
                    </div>                                                                     

                    <div class="form-group row">
                        <label for="Pais" class="col-sm-3 control-label">Pais</label>
                        <div class="col-sm-6">                                                 
                            <asp:TextBox class="form-control" id="Pais" runat="server" placeholder="Ingrese Pais" ReadOnly="true"></asp:TextBox>                                          
                        </div>
                    </div>
                

                    <div class="form-group row">
                        <label for="Login" class="col-sm-3 control-label">ID</label>
                        <div class="col-sm-6">                            
                            <asp:TextBox class="form-control" id="user_id" runat="server" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    
                    <div class="form-group row">                          
                        <label for="Activo" class="col-sm-3 control-label">Activo</label>
                        <div class="col-sm-6">                          
                            <asp:CheckBox ID="Activo" runat="server"  class="form-control"  />                          
                        </div>
                    </div>



                </div>


                <div style="width:49%;display:inline-block;vertical-align:top">



                    <div class="form-group row" style="display:block;margin:10px;height:80px;">

                        <label for="Aereo" class="col-sm-2 control-label">Modulos</label>
                        <div class="col-sm-12">                          
    
                            <asp:CheckBoxList ID="modulos" runat="server" class="form-control"  RepeatDirection="Horizontal">
                            <asp:ListItem Text="Aereo " Value="aereo"></asp:ListItem>
                            <asp:ListItem Text="Maritimo " Value="maritimo"></asp:ListItem>
                            <asp:ListItem Text="Admin " Value="admin"></asp:ListItem>
                            </asp:CheckBoxList>

                        </div>

                    </div>
   
   

   
                    <div class="form-group row" style="display:block;margin:10px;height:140px;">

                    <!--
                        <label for="Aereo" class="col-sm-2 control-label">Empresas</label>
                        <div class="col-sm-8">                          
    -->
    
	 <table class="form-control1" border="1" width=110% >
		<tbody>
		<tr>
        <!--
			<td><input id="ContentPlaceHolder1_Paises_0" name="ctl00$ContentPlaceHolder1$Paises$0" checked="checked" value="1" style="height: 10px;" type="checkbox"><label for="ContentPlaceHolder1_Paises_0" style="text-align: right;">AIMAR GUATEMALA</label></td>
			<td><input id="ContentPlaceHolder1_Paises_1" name="ctl00$ContentPlaceHolder1$Paises$1" value="29" style="height: 10px;" type="checkbox"><label for="ContentPlaceHolder1_Paises_1" style="text-align: right;">APL - GUATEMALA</label></td>
			<td><input id="ContentPlaceHolder1_Paises_2" name="ctl00$ContentPlaceHolder1$Paises$2" checked="checked" value="15" style="height: 10px;" type="checkbox"><label for="ContentPlaceHolder1_Paises_2" style="text-align: right;">LATIN FREIGHT GUATEMALA</label></td>
			<td><input id="ContentPlaceHolder1_Paises_3" name="ctl00$ContentPlaceHolder1$Paises$3" value="40" style="height: 10px;" type="checkbox"><label for="ContentPlaceHolder1_Paises_3" style="text-align: right;">LTM CARRIER</label></td>
			<td><input id="ContentPlaceHolder1_Paises_4" name="ctl00$ContentPlaceHolder1$Paises$4" checked="checked" value="32" style="height: 10px;" type="checkbox"><label for="ContentPlaceHolder1_Paises_4" style="text-align: right;">TLA GUATEMALA </label></td>
            -->
            <td colspan=5 valign=top>
                    <asp:CheckBoxList ID="Paises" runat="server" class="form-control1" RepeatDirection="Horizontal">
                    <asp:ListItem Text="TLA GT" Value="32" style="width:178px;border:1px solid gray;display:block"></asp:ListItem>
					<asp:ListItem Text="AIMAR GT" Value="1" style="width:178px;border:1px solid gray;display:block"></asp:ListItem>
                    <asp:ListItem Text="LATINFREIGHT GT" Value="15" style="width:178px;border:1px solid gray;display:block"></asp:ListItem>
					<asp:ListItem Text="APL GT&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="29" style="display:none" ></asp:ListItem>					
					<asp:ListItem Text="LTM CARRIER&nbsp;&nbsp;" Value="40" style="display:none"></asp:ListItem>
					</asp:CheckBoxList>
            </td>
		</tr>
		
		<tr>
        	<td height=120px valign=top width=180px>				                        
					<asp:CheckBoxList ID="CheckBoxList5" runat="server" class="form-control1">
                    
                    <asp:ListItem Text="Aereo" Value="1" style="display:none"></asp:ListItem>
					<asp:ListItem Text="Maritimo" Value="2" style="display:none"></asp:ListItem>

					<asp:ListItem Text="Maritimo Crear" Value="3"></asp:ListItem>
					<asp:ListItem Text="Maritimo Listado" Value="4"></asp:ListItem>
					<asp:ListItem Text="Aereo Import" Value="6"></asp:ListItem>
					<asp:ListItem Text="Aereo Export" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Aereo Listado" Value="8"></asp:ListItem>
					</asp:CheckBoxList>		
			</td>
			<td height=120px valign=top width=180px>

					<asp:CheckBoxList ID="CheckBoxList1" runat="server" class="form-control1">

                    <asp:ListItem Text="Aereo" Value="1" style="display:none"></asp:ListItem>
					<asp:ListItem Text="Maritimo" Value="2" style="display:none"></asp:ListItem>

					<asp:ListItem Text="Maritimo Crear" Value="3"></asp:ListItem>
					<asp:ListItem Text="Maritimo Listado" Value="4"></asp:ListItem>
					<asp:ListItem Text="Aereo Import" Value="6"></asp:ListItem>
					<asp:ListItem Text="Aereo Export" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Aereo Listado" Value="8"></asp:ListItem>
					</asp:CheckBoxList>

			</td>
            <td height=120px valign=top width=180px>
					<asp:CheckBoxList ID="CheckBoxList3" runat="server" class="form-control1">

                    <asp:ListItem Text="Aereo" Value="1" style="display:none"></asp:ListItem>
					<asp:ListItem Text="Maritimo" Value="2" style="display:none"></asp:ListItem>

					<asp:ListItem Text="Maritimo Crear" Value="3"></asp:ListItem>
					<asp:ListItem Text="Maritimo Listado" Value="4"></asp:ListItem>
					<asp:ListItem Text="Aereo Import" Value="6"></asp:ListItem>
					<asp:ListItem Text="Aereo Export" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Aereo Listado" Value="8"></asp:ListItem>
					</asp:CheckBoxList>
			</td>
			<td height=120px valign=top width=90px style="display:none">

					<asp:CheckBoxList ID="CheckBoxList2" runat="server" class="form-control1">
					<asp:ListItem Text="Maritimo Crear" Value="3"></asp:ListItem>
					<asp:ListItem Text="Maritimo Listado" Value="4"></asp:ListItem>
					<asp:ListItem Text="Aereo Import" Value="6"></asp:ListItem>
					<asp:ListItem Text="Aereo Export" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Aereo Listado" Value="8"></asp:ListItem>
					</asp:CheckBoxList>

			</td>

			<td height=120px valign=top width=110px style="display:none">
			
					<asp:CheckBoxList ID="CheckBoxList4" runat="server" class="form-control1">
					<asp:ListItem Text="Maritimo Crear" Value="3"></asp:ListItem>
					<asp:ListItem Text="Maritimo Listado" Value="4"></asp:ListItem>
					<asp:ListItem Text="Aereo Import" Value="6"></asp:ListItem>
					<asp:ListItem Text="Aereo Export" Value="7"></asp:ListItem>
                    <asp:ListItem Text="Aereo Listado" Value="8"></asp:ListItem>
					</asp:CheckBoxList>
				
			</td>

		</tr>		
	</tbody></table>
    <!--

                        </div>

                    </div>
                    -->

               





                </div>

            </div> <!-- catalogo2 -->

       </div>

    </div>
   
</asp:Content>

