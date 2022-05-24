<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Wms.aspx.vb" Inherits="mn_Wms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script language="javascript" type="text/javascript">
// <![CDATA[

       $(function () {
            $('#<%=Tipo.ClientID%>').focus();
           //$('#<%=Activo.ClientID%>').attr('disabled', 'disabled');
           //if ('<%=Activo.Checked%>' == 'False') {
               //$('input[type=text]').attr('disabled', 'disabled');
               //$('input[type=checkbox]').attr('disabled', 'disabled');
               //$('select').attr('disabled', 'disabled');
           //}
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
            var checked = valor_checkbox('<%=Bodega.ClientID%>');
            if (checked == 999) { alert("Seleccione al menos una Bodega"); return false; }                        
           return true;
       }
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

            <asp:Label ID="pestana_lbl" runat="server" Text=""></asp:Label>

            <div id="catalogo2"  class="tabs_panel">
                
                <div style="width:45%;display:inline-block;vertical-align:top">
                                        
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

    <%--                <div class="form-group row">
                        <label for="Apellido" class="col-sm-3 control-label">Apellido</label>
                        <div class="col-sm-6">
                        </div>
                    </div>    
    --%>


                    <div class="form-group row">
                        <label for="Password" class="col-sm-3 control-label">Password</label>
                        <div class="col-sm-9">                                                 
                            <asp:TextBox class="form-control" id="Password" runat="server" placeholder="Ingrese Password" ReadOnly="true"></asp:TextBox>                                          
                        </div>
                    </div>


                            <asp:TextBox class="form-control" id="Apellido" runat="server" placeholder="Ingrese Apellido" style="Display:none"></asp:TextBox>

<%--                </div>

                <div style="width:32%;display:inline-block;vertical-align:top;">--%>
                
                   <div class="form-group row">                                         
                        <label for="Tipo" class="col-sm-3 control-label">Tipo</label>                                     
                        <div class="col-sm-7">
                            <asp:DropDownList ID="Tipo" runat="server" class="form-control" >                                                                
                            </asp:DropDownList>                                        
                        </div>
                    </div>

                    <div class="form-group row">                                         
                        <label for="Grupo" class="col-sm-3 control-label">Grupo</label>                                     
                        <div class="col-sm-7">
                            <asp:DropDownList ID="Grupo" runat="server" class="form-control" >                                                                
                            </asp:DropDownList>                                        
                        </div>
                    </div>                                    

                    <div class="form-group row">                          
                        <label for="Estatus" class="col-sm-3 control-label">Activo</label>
                        <div class="col-sm-7">                          
                            <asp:CheckBox ID="Activo" runat="server"  class="form-control" />                          
                        </div>
                    </div>
                
                </div>



                <div style="width:48%;display:inline-block;vertical-align:top;height:100%;overflow:auto;">
                      
                        
                            <label for="Paises" class="col-sm-8 control-label">Accesos Bodegas</label><br />
                            <div class="col-sm-10 checkbox">
                                <asp:checkboxlist id="Bodega" runat="server" class="form-control">
                                </asp:checkboxlist>                                
                            </div>
                        

                </div>

            </div> <!-- catalogo2 -->

       </div>

    </div>
   


</asp:Content>

