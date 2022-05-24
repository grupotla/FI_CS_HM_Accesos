<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Solicitud.aspx.vb" Inherits="Solicitud" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



<script language="javascript" type="text/javascript">

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

        });

    function accordion_click(acordion) {
        document.getElementById('<%=acordion_field.ClientID%>').value = acordion;
    }

    function valida_enviar() {
        if (!confirm("Confirme Enviar Esta Solicitud")) {
            return false;
        }
    }

    function valida_procesar() {
        if (!confirm("Confirme Procesar Esta Solicitud")) {
            return false;
        }
    }

    function valida_rechazar() {
        if (!confirm("Confirme Rechazar Esta Solicitud")) {
            return false;
        }
    }

</script>


<asp:TextBox id="acordion_field" runat="server" style="display:none"></asp:TextBox>
<asp:TextBox id="empresa_ant" runat="server" style="display:none"></asp:TextBox>


<style>
.modulos { display:block; margin:10px; padding:10px; height:100%; border:1px solid transparent}
</style>

<div class="container">

<ul class="nav nav-tabs">
  <li <%=tab_titl(1)%> ><a href="#tab_1" data-toggle="tab" onclick="accordion_click(1)" title="Informacion General del Usuario">Generales</a></li>
  <li <%=tab_titl(2)%> ><a href="#tab_2" data-toggle="tab" onclick="accordion_click(2)">Ventas / Maritimo</a></li>
  <li <%=tab_titl(3)%> ><a href="#tab_3" data-toggle="tab" onclick="accordion_click(3)">Aereo / Terrestre</a></li>
  <li <%=tab_titl(4)%> ><a href="#tab_4" data-toggle="tab" onclick="accordion_click(4)" title="Maritimo / Aduanas / Bitacora APL">Customer</a></li>
  <li <%=tab_titl(5)%> ><a href="#tab_5" data-toggle="tab" onclick="accordion_click(5)" title="Caja de Ahorros">Caja</a></li>
  <li <%=tab_titl(6)%> ><a href="#tab_6" data-toggle="tab" onclick="accordion_click(6)">Wms</a></li>
  <li <%=tab_titl(7)%> ><a href="#tab_7" data-toggle="tab" onclick="accordion_click(7)">Baw</a></li>
  <li <%=tab_titl(8)%> ><a href="#tab_8" data-toggle="tab" onclick="accordion_click(8)" title="Manifiestos Costa Rica">Manifiestos</a></li>
  <li <%=tab_titl(9)%> ><a href="#tab_9" data-toggle="tab" onclick="accordion_click(9)" title="Transportes Equitrans">Tir</a></li>
  <li <%=tab_titl(10)%> ><a href="#tab_10" data-toggle="tab" onclick="accordion_click(10)" title="Seguros / Catalogos / Planillas / Bitacora OD / Graficas ISO / Manifiestos E. APL">Varios</a></li>
</ul>


<div class="tab-content" style="border-bottom:1px solid silver;height:65%;overflow-x:hidden">


        <div class="tab-pane<%=tab_cont(1)%>" id="tab_1">          
        <br />

                        <% '/////////////////////////////////////////// GENERALES //////////////// %>
                        <table border="0" width="100%" >
                        <tr>

                        <td width="20%" valign="top" rowspan=2>


                                <div class="form-group row">
                                    <label for="Solicitud" class="col-sm-4 control-label">Solicitud</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox class="form-control" id="Solicitud" runat="server" placeholder="Solcitud Nueva" ReadOnly="True"></asp:TextBox>
                                    </div>                            
                                </div>

                                <div class="form-group row">                          
                                    <label for="Correo" class="col-sm-4 control-label">Correo</label>
                                    <div class="col-sm-6 radio">                                                                              
                                            <asp:RadioButtonList ID="Correo" runat="server"  class="form-control">
                                            <asp:ListItem Text="Nombre" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Generico" Value="G"></asp:ListItem>
                                            </asp:RadioButtonList>                            
                                    </div>
                                </div> 
                                

                                <div class="form-group row">                          
                                    <label for="Chat" class="col-sm-4 control-label">Chat</label>
                                    <div class="col-sm-6 radio">                                                                              
                                            <asp:RadioButtonList ID="Chat" runat="server"  class="form-control">
                                            <asp:ListItem Text="Nombre" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Generico" Value="G"></asp:ListItem>
                                            </asp:RadioButtonList>                            
                                    </div>
                                </div> 
                                

                                <div class="form-group row">                          
                                    <label for="Nuevo" class="col-sm-4 control-label">Personal</label>
                                    <div class="col-sm-6 radio">                                                                              
                                            <asp:RadioButtonList ID="Nuevo" runat="server"  class="form-control">
                                            <asp:ListItem Text="Nuevo" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Reemplazo" Value="R"></asp:ListItem>
                                            </asp:RadioButtonList>                            
                                    </div>
                                </div> 
                                

                               <div class="form-group row">
                                    <label for="tipo_usuario" class="col-sm-4 control-label">Tipo</label>
                                    <div class="col-sm-7">                             
                                        <asp:DropDownList ID="tipo_usuario" runat="server" class="form-control">                        
                                        <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                        <asp:ListItem Value="1">Ventas</asp:ListItem>
                                        <asp:ListItem Value="2">2 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="3">3 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="4">4 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="5">5 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="7">7 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="8">8 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="9">9 pend descrip</asp:ListItem>
                                        <asp:ListItem Value="11">Customer Regional</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group row">                          
                                    <label for="Estatus" class="col-sm-4 control-label">Ubicacion</label>
                                    <div class="col-sm-7">
                                            <asp:DropDownList ID="locode" runat="server" class="form-control" >
                                            </asp:DropDownList>                                                           
                                    </div>
                                </div> 


                                <div class="form-group row">                          
                                    <label for="Estatus" class="col-sm-4 control-label">Estatus</label>
                                    <div class="col-sm-7">
                                            <asp:DropDownList ID="Estatus" runat="server" class="form-control" >
                                            </asp:DropDownList>                                                           
                                    </div>
                                </div> 


                        </td>

                        <td width="50%" rowspan=2>

                                <div class="form-group row">
                                    <label for="Empleado_id" class="col-sm-3 control-label">Empleado Existente</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="Empleado_id" runat="server" class="form-control" AutoPostBack="True">                                                                
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label for="Usuario" class="col-sm-3 control-label">Usuario</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox class="form-control" id="Usuario" runat="server" placeholder="Ingrese usuario" ></asp:TextBox>
                                    </div>                            
                                </div>

                                <div class="form-group row">
                                    <label for="Empleado_nombre" class="col-sm-3 control-label">Nombre de Empleado</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox class="form-control" id="Empleado_nombre" runat="server" placeholder="Ingrese Nombre De Empleado"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label for="Dominio" class="col-sm-3 control-label">Dominio</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="Dominio" runat="server" class="form-control">                                                                
                                        </asp:DropDownList>
                                    </div>
                                </div>



                                <div class="form-group row">
                                    <label for="Solicitante" class="col-sm-3 control-label">Solicitante</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox class="form-control" id="Solicitante" runat="server" placeholder="Ingrese Solicitante"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <label for="Reemplaza" class="col-sm-3 control-label">Reemplaza a</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox class="form-control" id="Reemplaza" runat="server" placeholder="Ingrese Persona a quien reemplaza"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="form-group row">
                                    <label for="Especiales" class="col-sm-3 control-label">Accesos Especiales</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox class="form-control" id="Especiales" runat="server" placeholder="Ingrese Accesos Especiales" TextMode="MultiLine" Rows="2"></asp:TextBox>


                                    </div>
                                </div>

                        </td>

                        <td width="25%" valign="top" height="200px">

                                <div class="form-group row" >
                                    <label for="gPais" class="col-sm-2 control-label">Pais</label>
                                    <div class="col-sm-7 radio">
                                        <asp:RadioButtonList id="gPais" runat="server" class="form-control" 
                                            AutoPostBack="True" CausesValidation="True">
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                   
                        </td>

                        <td width="10%" rowspan=2></td>
                        

                        </tr>


                        <tr>
                            <td valign=top >

                            <div style="position:relative;border:0px solid transparent;width:100%;height:200px;overflow:scroll;">

                              <div class="form-group row" style="width:100%">
                                    <label for="gEmpresas" class="col-sm-3 control-label">Empresas</label>
                                    <div class="col-sm-9 radio">
                                        <asp:checkboxlist id="gEmpresas" runat="server" class="form-control">
                                        <asp:ListItem Text="Aimar" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Latin" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Equitrans" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="IsiSurveyor" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Grhlogistics" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="Frutas Maya" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="APL" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Mayan" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="Aproa" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="Reimar" Value="10"></asp:ListItem>
                                        </asp:checkboxlist>
                                    </div>
                                </div>
                                </div>
                            </td>
                        </tr>
                        </table>


        </div>
        <div class="tab-pane<%=tab_cont(2)%>" id="tab_2">
            

                <table width="90%" border="0">
                <tr>
                <td width="50%">
                    <h3>Ventas / Maritimo</h3>
                </td>
                <td width="50%">
                    <asp:Label ID="PerfilLabel" runat="server" Text="Label">Perfiles</asp:Label>
                </td>
                </tr>
                </table>
                                

                    <% '/////////////////////////////////////////// VENTAS MARITIMO //////////////// %>
                
                <table width="90%" border="0">
                <tr>
                <td width="50%">
                    <div class="form-group row" style="margin-left:20px;border:1px solid transparent;width:90%;height:360px;overflow:scroll">   
                        <center>
                        <asp:GridView ID="ventas_grid" Width="70%" runat="server" CssClass="table table-hover table-striped input-sm text-primary" GridLines="None" AutoGenerateColumns="false"  >                 
                            <Columns>
                                <asp:TemplateField HeaderText="Perfil">
                                    <ItemTemplate>                                                                                                                
                                        <asp:Image ID="lnk_chk" runat="server" Height="16" />     
                                    </ItemTemplate>
                                </asp:TemplateField>       
                                
                                <asp:TemplateField>
	                                <ItemTemplate>
                                        <asp:LinkButton ID="lnk_sel" runat="server" OnClick="imageButtonClick">&nbsp;Seleccionar &nbsp;</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Pais">
	                                <ItemTemplate>
                                        <asp:Image ID="lnk_flag" runat="server" Height="16" />                                                                
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="database" ><ItemStyle ></ItemStyle></asp:BoundField>
                                <asp:BoundField DataField="pais" ><ItemStyle ForeColor="Transparent" Font-Size="0"></ItemStyle></asp:BoundField>
                                <asp:BoundField DataField="flag" Visible=false></asp:BoundField>
                                           
                            </Columns>
                        </asp:GridView>                    

                        </center>
<%                             
    '<asp:RadioButton ID="lnk_radio" runat="server" name="lnk_radios"  AutoPostBack="True" CausesValidation="True" />    
    '<asp:LinkButton ID="lnk_user" runat="server" Height="20" Width="20"><img src="Content/icon/glyphicons_009_magic.png" height="16" /></asp:LinkButton>                            
%>
                                                
                    </div>
                    
                </td>

                <td width="50%" valign="top" align="center">

                    <div class="col-sm-9 checkbox" style="margin:0px;border:1px solid transparent;width:90%;height:360px;overflow:scroll;text-align:center">
                        
                        
                        
                        <asp:checkboxlist id="Perfil" runat="server" class="form-control" >
                        </asp:checkboxlist>

                    </div>    
 
                    <asp:TextBox ID="vPaisS" runat="server" ReadOnly Width="150"  Visible = false ></asp:TextBox>                 

                 </td>
                </tr>
                </table>

 
 <%
     '<div class="form-group row" style="display:none">   
     '        <label for="vPais" class="col-sm-1 control-label" >Paises Asignados</label>
     '        <div class="col-sm-2 checkbox" >
     '            <asp:checkboxlist id="vPais" runat="server" class="form-control" Enabled="False">
     '            </asp:checkboxlist>
     '        </div>
     '        <label for="vPaisSel" class="col-sm-1 control-label">Seleccione Pais</label>
     '        <div class="col-sm-2 radio">
     '            <asp:RadioButtonList ID="vPaisSel" runat="server" class="form-control1" 
     '                AutoPostBack="True" CausesValidation="True">
     '            </asp:RadioButtonList>
     '        </div>
     '</div>
 %>

                        <!--</div>-->

        </div>
        <div class="tab-pane<%=tab_cont(3)%>" id="tab_3">
            
                        <table border="0" width="90%" height="95%">
                        <tr>
                        <td width="50%">
                                <% '/////////////////////////////////////////// AEREO //////////////// %>
                                <div class="modulos">
                                    <h3>Aereo</h3>
                                    <!--<div class="form-group row" style="border:1px solid transparent;display:block;width:95%;height:80%">-->
                                        <label for="aPais" class="col-sm-2 control-label">Pais</label>
                                        <div class="col-sm-5 checkbox">
                                            <asp:checkboxlist id="aPais" runat="server" class="form-control">
                                            </asp:checkboxlist>
                                        </div>

                                        <label for="aNivel" class="col-sm-1 control-label">Nivel</label>
                                        <div class="col-sm-3 radio">                                                                            
                                            <asp:RadioButtonList ID="aNivel" runat="server" class="form-control">                                                                
                                            <asp:ListItem Value="0" Text="Root"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Admin"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Editor"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                </div>
                        </td>
                        <td width="50%">
                                    <% '/////////////////////////////////////////// TERRESTRE //////////////// %>
                                <div class="modulos">
                                    <h3>Terrestre</h3>
                                    <!--<div class="form-group row" style="border:1px solid transparent;display:block;width:95%;height:80%">-->
                                        <label for="tPais" class="col-sm-2 control-label">Pais</label>
                                        <div class="col-sm-5 checkbox">
                                            <asp:checkboxlist id="tPais" runat="server" class="form-control">
                                            </asp:checkboxlist>
                                        </div>

                                        <label for="tNivel" class="col-sm-1 control-label">Nivel</label>
                                        <div class="col-sm-3 radio">                                                                            
                                            <asp:RadioButtonList ID="tNivel" runat="server" class="form-control">                                                                
                                            <asp:ListItem Value="0" Text="Root"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Admin"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Editor"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                </div>
                        </td>
                        </tr>
                        </table>


        </div>
        <div class="tab-pane<%=tab_cont(4)%>" id="tab_4">

                        <table border="0" width="90%" height="95%">
                        <tr>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// MARITIMO //////////////// %>
                                <div class="modulos">
                                    <h3>Maritimo</h3>
                                    <label for="cmNivel" class="col-sm-4 control-label">Nivel</label>
                                    <div class="col-sm-8 radio">                                                       

                                        <asp:RadioButtonList ID="cmNivel" runat="server" class="form-control">
                                        <asp:ListItem Value="5" Text="Operativo"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Consulta"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </div>
                                    </div>
                        </td>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// ADUANAS //////////////// %>
                                <div class="modulos">


                                    <h3>Aduanas</h3>

                                    <div class="form-group row"> 
                                    <label for="caNivel" class="col-sm-3 control-label">Nivel</label>
                                    <div class="col-sm-6 radio">
                                        <asp:RadioButtonList ID="caNivel" runat="server" class="form-control">
                                        <asp:ListItem Value="5" Text="Operativo"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Consultas"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    </div>
                                    
                                    <br />
                                    <br />

                                    <div class="form-group row"> 
                                    <label for="cEmpresas" class="col-sm-3 control-label">Empresas</label>
                                    <div class="col-sm-6 checkbox">
                                        <asp:checkboxlist id="cEmpresas" runat="server" class="form-control">
                                        </asp:checkboxlist>
                                    </div>
                                    </div>
                                </div>
                        </td>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// BITACORA APL //////////////// %>
                                <div class="modulos">
                                    <h3>Bitacora APL</h3>

                                    <label for="cbNivel" class="col-sm-3 control-label">Nivel</label>
                                    <div class="col-sm-9 radio">                             
                                        <asp:RadioButtonList ID="cbNivel" runat="server" class="form-control">
                                        <asp:ListItem Value="5" Text="Operativo"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Consultas"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    </div>
                        </td>                        
                        </tr>
                        </table>

        </div>
        <div class="tab-pane<%=tab_cont(5)%>" id="tab_5">
            

                        <table border="0" width="90%" height="95%">
                        <tr>
                        <td width="60%">
                                    <% '/////////////////////////////////////////// CAJA DE AHORRO //////////////// %>
                                <div class="modulos">
                                    <h3>Caja de Ahorro</h3>
                                    <div class="form-group row">
                                        <label for="cNivel" class="col-sm-2 control-label">Nivel</label>
                                        <div class="col-sm-3 radio">                                
                                            <asp:RadioButtonList ID="cNivel" runat="server" class="form-control">                                                                
                                            </asp:RadioButtonList>    
                                        </div>
        
                                        <label for="cPais" class="col-sm-1 control-label">Pais</label>
                                        <div class="col-sm-4 checkbox">                                
                                            <asp:checkboxlist id="cPais" runat="server" class="form-control">
                                            </asp:checkboxlist>                                
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        </tr>
                        </table>
        </div>
        <div class="tab-pane<%=tab_cont(6)%>" id="tab_6">
            <h3>Wms</h3>
                                    <% '/////////////////////////////////////////// WMS //////////////// %>


                    <div class="form-group row">                                         

                            <label for="Tipo" class="col-sm-1 control-label">Tipo</label>                                     
                            <div class="col-sm-3 select">
                                <asp:DropDownList ID="Tipo" runat="server" class="form-control" >                                                                
                                </asp:DropDownList>                                        
                            </div>
                            
                            <label for="Paises" class="col-sm-1 control-label">Bodegas</label>
                            <div class="col-sm-3 checkbox">
                                <asp:checkboxlist id="Bodega" runat="server" class="form-control">
                                </asp:checkboxlist>                                
                            </div>
                    </div>                                    


                    <div class="form-group row">           
                            <label for="Grupo" class="col-sm-1 control-label">Grupo</label>                                     
                            <div class="col-sm-3 check">
                                <asp:DropDownList ID="Grupo" runat="server" class="form-control" >                                                                
                                </asp:DropDownList>                                        
                            </div>
                    </div>   
        </div>
        <div class="tab-pane<%=tab_cont(7)%>" id="tab_7">
            <h3>Baw</h3>

                                    <% '/////////////////////////////////////////// BAW //////////////// %>
            
            <table width="90%" border=0>
            <tr>
                <td width="38%" rowspan=2 valign=top>

                        <asp:Label ID="lblBaw_1" runat="server" Text="Label" CssClass="col-sm-8 control-label"></asp:Label>
                        <div class="col-sm-10 checkbox">
                            <asp:checkboxlist id="Checkboxlist1" runat="server" class="form-control">
                            </asp:checkboxlist>                                
                        </div>
                </td>
                <td width="38%" rowspan=2>

                        <asp:Label ID="lblBaw_2" runat="server" Text="Label" CssClass="col-sm-8 control-label"></asp:Label>
                        <div class="col-sm-10 checkbox" style="height:330px;overflow:scroll">
                            <asp:checkboxlist id="Checkboxlist2" runat="server" class="form-control">
                            </asp:checkboxlist>                                
                        </div>
                </td>
                <td width="24%" valign=top>
                    
                        <asp:Label ID="lblBaw_3" runat="server" Text="Label" CssClass="col-sm-8 control-label"></asp:Label>                        
                        <div class="col-sm-8 checkbox">
                            <asp:checkboxlist id="Checkboxlist3" runat="server" class="form-control">
                            </asp:checkboxlist>                                
                        </div>
                </td>
            </tr>
            <tr>
                <td width="24%" valign=top>
                        
                        <asp:Label ID="lblBaw_4" runat="server" Text="Label" CssClass="col-sm-offset-0 col-sm-8 control-label"></asp:Label>
                        <div class="col-sm-offset-0 col-sm-10 checkbox">
                            <asp:checkboxlist id="Checkboxlist4" runat="server" class="form-control">
                            </asp:checkboxlist>                                
                        </div>    
                        
                </td>
            </tr>
            
            
            </table>
                

                    <asp:TreeView ID="BawTreeView" runat="server">
                    </asp:TreeView>

        </div>



        <div class="tab-pane<%=tab_cont(8)%>" id="tab_8">
            
                                    <% '/////////////////////////////////////////// MANIFIESTOS CR / CRLTF //////////////// %>

                        <table border="0" width="90%" height="95%">
                        <tr>
                        <td width="50%">
                                                            
                                <div class="modulos">
                                    <h3>Costa Rica</h3>
                                    <div class="form-group row">                         
                                        <label for="mcrNivel" class="col-sm-1 control-label">Nivel</label>
                                        <div class="col-sm-4 checkbox">                                
                                            <asp:checkboxlist ID="mcrNivel" runat="server" class="form-control">                                                                
                                            <asp:ListItem Value="AE" Text="Aereo"></asp:ListItem>
                                            <asp:ListItem Value="TE" Text="Terrestre"></asp:ListItem>
                                            <asp:ListItem Value="MA" Text="Maritimo"></asp:ListItem>
                                            <asp:ListItem Value="AD" Text="Aduana"></asp:ListItem>
                                            </asp:checkboxlist>                          
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        <td width="50%">
                                    
                                <div class="modulos">
                                    <h3>Costa Rica LTF</h3>
                                    <div class="form-group row">
                                        <label for="sNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-4 checkbox">                                                                                             
                                            <asp:checkboxlist ID="mclNivel" runat="server" class="form-control">                                                                
                                            <asp:ListItem Value="AE" Text="Aereo"></asp:ListItem>
                                            <asp:ListItem Value="TE" Text="Terrestre"></asp:ListItem>
                                            <asp:ListItem Value="MA" Text="Maritimo"></asp:ListItem>
                                            <asp:ListItem Value="AD" Text="Aduana"></asp:ListItem>
                                            </asp:checkboxlist>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        </tr>
                        </table>
        </div>


        <div class="tab-pane<%=tab_cont(9)%>" id="tab_9">

                                    <% '/////////////////////////////////////////// TIR //////////////// %>


                        <h3>Modulo Transporte Equitrans</h3>

                            <div class="form-group row">
                                
                                <label for="tEmpresas" class="col-sm-1 control-label">Empresas</label>
                                <div class="col-sm-2 radio">                                                                                             
                                    <asp:RadioButtonList ID="tEmpresas" runat="server" class="form-control" AutoPostBack="True" CausesValidation="True">                                                                                                    
                                    </asp:RadioButtonList>
                                </div>
                                
                                <asp:HiddenField ID="tEmpresa" runat="server" />

                                <label for="tPermisos" class="col-sm-1 control-label">Permisos</label>
                                <div class="col-sm-6 checkbox" style="height:330px;overflow:scroll">                                                                                             
                                    <asp:checkboxlist ID="tirPermisos" runat="server" class="form-control">                                                                
                                    </asp:checkboxlist>
                                </div>
                            </div>
        </div>


        <div class="tab-pane<%=tab_cont(10)%>" id="tab_10">

                        
                        <table border="0" width="90%" height="95%">
                        <tr>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// SEGUROS //////////////// %>
                                <div class="modulos">
                                    <h3>Seguros</h3>
                                    <div class="form-group row">
                                        <label for="sNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-6 radio">                                                                                             
                                            <asp:RadioButtonList ID="sNivel" runat="server" class="form-control">                                                                
                                            <asp:ListItem Value="2" Text="Administrador"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Operativo"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="SAT"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// CATALOGOS //////////////// %>
                                <div class="modulos">
                                    <h3>Catalogos</h3>

                                    <div class="form-group row">
                                        <label for="cgNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-6 radio">                                     
                                            <asp:RadioButtonList ID="cgNivel" runat="server" class="form-control">
                                            <asp:ListItem Value="4" Text="Administrador"></asp:ListItem>                                                
                                            <asp:ListItem Value="3" Text="Supervisor"></asp:ListItem>                        
                                            <asp:ListItem Value="2" Text="Operativo"></asp:ListItem>                        
                                            <asp:ListItem Value="1" Text="Consultas"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// PLANILLAS //////////////// %>
                                <div class="modulos">
                                    <h3>Planillas</h3>

                                    <div class="form-group row">
                                        <label for="pNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-6 radio">                                     
                                            <asp:RadioButtonList ID="pNivel" runat="server" class="form-control">
                                            <asp:ListItem Value="1" Text="Operativo"></asp:ListItem>                                                
                                            <asp:ListItem Value="2" Text="RRHH"></asp:ListItem>                        
                                            <asp:ListItem Value="3" Text="Reportes"></asp:ListItem>              
                                            <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>                                                      
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        </tr>
                        
                        <tr><td>&nbsp;</td>
                        </tr>

                        <tr>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// BITACORA OD //////////////// %>
                                <div class="modulos">
                                    <h3>Bitacora Office Depot</h3>
                                    <div class="form-group row">
                                        <label for="odNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-6 radio">                                                                                             
                                            <asp:RadioButtonList ID="odNivel" runat="server" class="form-control">                                                                
                                            <asp:ListItem Value="1" Text="Operativo"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Reportes"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Cliente Externo"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// GRAFICAS ISO //////////////// %>
                                <div class="modulos">
                                    <h3>Graficas ISO</h3>

                                    <div class="form-group row">
                                        <label for="iNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-6 radio">                                     
                                            <asp:RadioButtonList ID="iNivel" runat="server" class="form-control">
                                            <asp:ListItem Value="1" Text="Operativo"></asp:ListItem>                                                               
                                            <asp:ListItem Value="2" Text="Consultas"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        <td width="33%">
                                    <% '/////////////////////////////////////////// E-MANIFIESTOS APL //////////////// %>
                                <div class="modulos">
                                    <h3>Manifiestos Electronicos APL</h3>

                                    <div class="form-group row">
                                        <label for="mNivel" class="col-sm-3 control-label">Nivel</label>
                                        <div class="col-sm-6 radio">                                     
                                            <asp:RadioButtonList ID="mNivel" runat="server" class="form-control">
                                            <asp:ListItem Value="1" Text="Operativo"></asp:ListItem>                        
                                            <asp:ListItem Value="2" Text="Consultas"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Desactivado"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    </div>
                        </td>
                        </tr>
                        </table>

        </div>



</div><!-- tab content -->
</div><!-- end of container -->



        <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn btn-sm btn-primary" style="position:absolute;top:175px;right:110px;z-index:9999">Guardar</asp:LinkButton>

        <% If Solicitud.Text <> "" And Estatus.SelectedValue < "3" Then%>
        <asp:LinkButton ID="btnEnviar" runat="server" CssClass="btn btn-sm btn-warning" style="position:absolute;top:210px;right:110px;z-index:9999" onclientclick="return valida_enviar()">Enviar</asp:LinkButton>
        <% End If%>

        <% If Session("OperatorLevel") = 1 Then%>
        <% If Solicitud.Text <> "" And Estatus.SelectedValue = "3" Then%>
        <asp:LinkButton ID="btnProcesar" runat="server" CssClass="btn btn-sm btn-success" style="position:absolute;top:210px;right:110px;z-index:9999" onclientclick="return valida_procesar()">Aprobar</asp:LinkButton>
        <asp:LinkButton ID="btnRechazar" runat="server" CssClass="btn btn-sm btn-danger" style="position:absolute;top:250px;right:110px;z-index:9999" onclientclick="return valida_rechazar()">Rechazar</asp:LinkButton>
        <% End If%>
        <% End If%>
           

</asp:Content>












