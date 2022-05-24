<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_WmsNewPriv.aspx.vb" Inherits="mn_WmsNewPriv" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
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

            <div id="catalogo2" class="tabs_panel" style="border:0px solid silver">

                <div style="width:33%;display:inline-block;vertical-align:top;">

						<div class="form-group row">
							<label for="Login" class="col-sm-3 control-label">Codigo</label>
							<div class="col-sm-8">
								<asp:TextBox class="form-control" id="Codigo" runat="server" placeholder=" " ReadOnly="True"></asp:TextBox>
							</div>
						</div>

						<div class="form-group row">
							<label for="Login" class="col-sm-3 control-label">Login</label>
							<div class="col-sm-8">
								<asp:TextBox class="form-control" id="Login" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>
							</div>
						</div>

						<div class="form-group row">
							<label for="Nombre" class="col-sm-3 control-label">Nombre</label>
							<div class="col-sm-8">
								<asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>
							</div>
						</div>					

						<div class="form-group row">
							<label for="iGrupo" class="col-sm-3 control-label">Perfil</label>
							<div class="col-sm-8">
								<asp:TextBox class="form-control" id="iGrupo" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>
							</div>
						</div>	


						<div class="form-group row">
							<label for="iPais" class="col-sm-3 control-label">Pais Master</label>
							<div class="col-sm-8">
								<asp:TextBox class="form-control" id="iPais" runat="server" placeholder="" ReadOnly="True"></asp:TextBox>
							</div>
						</div>	

                </div>

                <div style="width:33%;display:inline-block;vertical-align:top;height:100%;overflow:auto;">                    
                        <div class="accordion" id="accordion2">
                            <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;border:0px solid red;">
                                <div class="accordion-heading btn btn-default btn-block">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne">
                                        <%=Licon_open%>&nbsp;Perfiles 
                                    </a>
                                </div>
                                <div id="collapseOne" class="accordion-body collapse in">
                                    <div class="accordion-inner">
                                        <div class="col-sm-offset-1 col-sm-102 radio">
                                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" class="form-control1" AutoPostBack="True" CausesValidation="True">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>                       
                            </div>
                        </div>
                </div>


                <!-- PANEL DERECHO DE PERFILES -->
               
                
                <div style="width:33%;display:inline-block;vertical-align:top;text-align:center;height:100%;overflow:auto;">                            
                        <% If Session("DBAccesos") <> Nothing Then%>                                                                                
                        <div class="accordion" id="Div3">
                            <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;border:0px solid red;">
                                <div class="accordion-heading btn btn-default btn-block">
                                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#Div3" href="#collapseOne1">
                                        Privilegios (<%=RadioButtonList1.SelectedValue%>)
                                    </a>
                                </div>
                                <div id="collapseOne1" class="accordion-body collapse in">
                                    <div class="accordion-inner">
                                        <div class="col-sm-offset-1 col-sm-102 radio">                                
                                            <asp:checkboxlist id="Perfil" runat="server" class="form-control1">
                                            </asp:checkboxlist>                                            
                                        </div>
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