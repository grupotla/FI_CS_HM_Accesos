<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_WmsBodegas.aspx.vb" Inherits="mn_WmsBodegas" %>

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



            $("label").click(function (event) {
                //$("input[name='radio_group']").removeAttr("checked");
                if (event.currentTarget.htmlFor) {
                    //console.log($('#' + event.currentTarget.htmlFor).attr('type'));
                    if ($('#' + event.currentTarget.htmlFor).attr('type') == 'radio') {
                        $('#' + event.currentTarget.htmlFor).attr("checked", "checked");
                    }
                    return true;
                } else {
                    //console.log(event);
                }
                return false;
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
                return valida_actualizar();
            }
        }

        function valida_activar() {
            if (!confirm("Confirme Activar Este Registro")) {
                return false;
            } else {
                return valida_actualizar();
            }
        }

        function valida_actualizar() {
            var checked = valor_checkbox('<%=Perfil.ClientID%>');
            //if (checked == 999) { alert("Seleccione Perfil"); return false; }            
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
                <% If Session("insert") = False Then%>   
                    <% If Session("OperatorLevel") <> 3 And Session("OperatorLevel") <> Nothing Then%>
                        <asp:LinkButton ID="btn_actualizar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_actualizar()"><%=Licon_update%></asp:LinkButton>
                    <% Else%>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_update%></asp:LinkButton>
                    <% End If%>                                                          
                <%End If%> 

            </div>


        <div class="panel-body">

            <!-- aqui va el menu tabs de manifiestos generado desde el codigo-->
            <asp:Label ID="pestana_lbl" runat="server" Text="Label">Generales</asp:Label>

            <div id="catalogo2" class="tabs_panel" >

                <div style="width:33%;display:inline-block;vertical-align:top;">


						<div class="form-group row">
							<label for="Login" class="col-sm-3 control-label">Codigo</label>
							<div class="col-sm-8">
								<asp:TextBox class="form-control" id="Codigo" runat="server" placeholder=" " ReadOnly="True"></asp:TextBox>
							</div>
						</div>
                        
                              
                            <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;;border:0px solid blue;">
                                <div class="accordion-heading btn btn-default btn-block">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#Div1">
                                        Empresas 
                                    </a>
                                </div>
                                <div id="Div2" class="accordion-body collapse in">
                                    <div class="accordion-inner">
                                        <div class="col-sm-offset-1 col-sm-121 radio">
                                            <asp:checkboxlist id="CheckListEmpresas" runat="server" class="form-control1">
                                            </asp:checkboxlist>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            



                </div>
                <!-- PANEL IZQUIERDO DE BASES DE DATOS -->
                
                <div style="width:33%;display:inline-block;vertical-align:top;height:100%;overflow:auto;">


                
                    <div class="form-group row">
                        <label for="Login" class="col-sm-3 control-label">Login</label>
                        <div class="col-sm-8">
                            <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>



                        <div class="accordion" id="accordion2">
                            <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;border:0px solid red;">
                                <div class="accordion-heading btn btn-default btn-block">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne">
                                        <%=Licon_open%>&nbsp;Usuario asignado en :
                                    </a>
                                </div>
                                <div id="collapseOne" class="accordion-body collapse in">
                                    <div class="accordion-inner">
                                        <div class="col-sm-offset-1 col-sm-102 radio">
                                            <asp:RadioButtonList ID="RadioListEmpAsignada" runat="server" class="form-control1" AutoPostBack="True" CausesValidation="True">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>                       
                            </div>

                            <br />

                            <div class="accordion-group table-bordered" style="display:none;background:rgb(240,240,240);padding:5px;;border:0px solid blue;">
                                <div class="accordion-heading btn btn-default btn-block">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseTwo">
                                        <%=Licon_new%> Asignar Usuario en :
                                    </a>
                                </div>
                                <div id="collapseTwo" class="accordion-body collapse in">
                                    <div class="accordion-inner">
                                        <div class="col-sm-offset-1 col-sm-121 radio">
                                            <asp:RadioButtonList ID="RadioButtonList2" runat="server" class="form-control1" AutoPostBack="True" CausesValidation="True">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                </div>





                <!-- PANEL DERECHO DE PERFILES -->
               
                
                <div style="width:33%;display:inline-block;vertical-align:top;text-align:center;height:100%;overflow:auto;">

                                    <div class="form-group row">
                        <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                        <div class="col-sm-8">
                            <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
             

                    <asp:TextBox class="form-control" id="Dominio" runat="server" placeholder="Ingrese Dominio" ReadOnly="True" style="display:none"></asp:TextBox>

                            
                        <% If Session("DBAccesos") <> Nothing Then%>          
                        
                        
                            <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;;border:0px solid blue;">
                                <div class="accordion-heading btn btn-default btn-block">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#Div1">
                                        Bodegas <%=Session("pais")%>
                                    </a>
                                </div>
                                <div id="Div1" class="accordion-body collapse in">
                                    <div class="accordion-inner">
                                        <div class="col-sm-offset-1 col-sm-121 radio">
                                            <asp:checkboxlist id="Perfil" runat="server" class="form-control1">
                                            </asp:checkboxlist>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                                                                                     
                                     
                                                       

                        <%End If%>
             
                </div>

            </div>

        </div>

    </div>


</asp:Content>

