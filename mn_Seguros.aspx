<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Seguros.aspx.vb" Inherits="mn_Seguros" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(function () {
//            if ($('#<%=Activo.ClientID%>'))
//                $('#<%=Activo.ClientID%>').attr('disabled', 'disabled');
//            if ('<%=Activo.Checked%>' == 'False') {
//                $('input[type=checkbox]').attr('disabled', 'disabled');
//                $('input[type=text]').attr('disabled', 'disabled');
//                $('select').attr('disabled', 'disabled');
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
            $('#<%=accSeguroTipo.ClientID%>').click(function (event) {
                <% If accSeguroTipo.SelectedIndex > -1 Then%>                
                <% If Session("OperatorLevel") = 3 And accSeguroTipo.SelectedValue = 2 Then%>
                    alert('No tiene persimos suficientes');
                    return false;
                <% End If %>  
                <% End If %>                               
                var OpeLev = parseInt('<%=Session("OperatorLevel")%>');
                var checked = parseInt(valor_radio('<%=accSeguroTipo.ClientID%>'));
                //alert(OpeLev + ' ' + checked);                
                if (OpeLev == 3 && checked == 2) {
                    alert('No tiene permisos suficientes');
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
            var checked = valor_radio('<%=accSeguroTipo.ClientID%>');
            if (checked == 999) { alert("Seleccione Nivel"); return false; }              
            return true;
        }

// ]]>
    </script>

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
                                      
                <div class="form-group row">
                    <label for="accSeguroTipo" class="col-sm-3 control-label">Nivel</label>
                    <div class="col-sm-4">                                                                                             
                        <asp:RadioButtonList ID="accSeguroTipo" runat="server" class="form-control">                                                                
                        <asp:ListItem Value="2" Text="Administrador"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Operativo"></asp:ListItem>
                        <asp:ListItem Value="5" Text="SAT"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <br />
                <br />
                <br />

                <div class="form-group row">
                    <label for="Activo" class="col-sm-3 control-label">Activo</label>
                    <div class="col-sm-4">                          
                        <asp:CheckBox ID="Activo" runat="server"  class="form-control" />
                    </div>
                </div>

            </div>
                       
        </div>

    </div>  


</asp:Content>



