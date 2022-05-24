<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ct_menu.aspx.vb" Inherits="ct_menu" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script language="javascript" type="text/javascript">
// <![CDATA[

        function valida_eliminar() {
            if (!confirm("Confirme Desactivar Este Registro")) {
                return false;
            }
        }

        function validar() {
            if ($('#<%=Nombre.ClientID%>').val() == "") { alert("Ingrese Nombre"); return false; }
            if ($('#<%=Menu.ClientID%>').val() == "") { alert("Ingrese Menu"); return false; }
            if ($('#<%=Orden.ClientID%>').val() == "") { alert("Ingrese Orden"); return false; }            
            return true;
        }

        function valida_agregar() {
            //if (document.getElementById("<%=Status.ClientID%>").checked == false) { alert(""); return false; }

            document.getElementById("<%=Status.ClientID%>").checked = true;
            return validar();
        }

        function valida_actualizar() {
            return validar();
        }

        $(function () {
            // Document is ready
            if ($('#<%=Nombre.ClientID%>'))
                $('#<%=Nombre.ClientID%>').focus();
            //$('.breadcrumb').hide();
            //$('.breadcrumb').html();
            //$('.breadcrumb').append('<li><%=Licon_opciones%></li>'); 2020-02-28

            <% If Session("insert") <> Nothing Then%>
            $('.breadcrumb').append('<li><%=Parent.text%></li>');
            $('.breadcrumb').append('<li><%=Nombre.text%></li>');
            <% End IF%>

//            $("a").click(function (event) {//funciona para todos los menu
//                console.log('evento=' + event.currentTarget.name);
//                if (event.currentTarget.name == 'lnk1_') {
//                    $('#< %=opcion1_txt.ClientID%>').removeAttr('disabled');
//                    $('#< %=opcion1_txt.ClientID%>').val(event.currentTarget.id);
//                    $('#< %=opcion1_btn.ClientID%>').click();
//                }
//            });
        });

        function abre_modal(opcion, parent){
            $('#<%=opcion.ClientID%>').val(opcion);
            $('#<%=opcion_parent.ClientID%>').val(parent);
            $('#<%=btn_abre.ClientID%>').click();
            //$('#myModalMenu').modal();
        }

// ]]>
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<%--        <asp:Button ID="opcion1_btn" runat="server" Text="Button" style="display:none"></asp:Button>
        <asp:TextBox ID="opcion1_txt" runat="server" style="display:none"></asp:TextBox>
--%>

   <div class="panel panel-info">
 
        <div class="panel-heading">                        
    
        </div>


            <div class="col-sm-offset-4 form-group row boton_bar">                               
                <% If Session("insert") = Nothing Then%>                    
<%--                    <% If Session("OperatorLevel") = 1 Then%>
                        <a class="btn btn-sm btn-info" OnClick="abre_modal('0','')" title="Agregar Menu a Root"><%=Licon_new%></a>
                    <% Else%>
                        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_new%></asp:LinkButton>
                    <% End If%>   --%>                     
                    <span style="height:29px;display:block;" />&nbsp;</span>                                                                                       
                <% Else%>
                    <%--<asp:LinkButton ID="btn_cancelar" runat="server" CssClass="btn btn-sm btn-primary" style="display:none"><%=Licon_opciones%></asp:LinkButton>--%>
                    <% If Session("insert") = 1 Then%>
                            <% If Session("OperatorLevel") = 1 Then%>
                                <asp:LinkButton ID="btn_agregar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_agregar()"><%=Licon_insert%></asp:LinkButton>
                            <% Else%>
                                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_insert%></asp:LinkButton>
                            <% End If%>                                                                                                               
                    <% Else%>
                            <% If Session("OperatorLevel") = 1 Then%>
                                <asp:LinkButton ID="btn_actualizar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_actualizar()"><%=Licon_update%></asp:LinkButton>
                            <% Else%>
                                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_update%></asp:LinkButton>
                            <% End If%>                                                         
                    <% End If%>
                <% End If%>
            </div>


        <div class="panel-body" >

            

            <ul id="pestana" class="nav nav-tabs">
        
            <li id="li1_opciones"
                <% If Session("insert") = nothing Then%>
                    class="active"
                <% Else%>
                <% End If%>
            >            
            <!--
            <a id="a_opciones" name="lnk_" onmouseover="this.style.cursor='pointer'" style="cursor: pointer;"><%=Licon_opciones%></a></li>  2020-02-28
            -->
            <% If Session("insert") = Nothing Then%>
                    <li id="li1_new_m">

                    <% If Session("OperatorLevel") = 1 Then%>
                        <a id="a_new_m" name="lnk_" onmouseover="this.style.cursor='pointer'" style="cursor: pointer;"><%=Licon_new%></a>
                    <% Else%>
                        <%=Licon_new%>
                    <% End If%>
                    </li>        

            <% Else%>
                <li id="li1_opcion" class="active"><a id="a_opcion" name="lnk_" onmouseover="this.style.cursor='pointer'" style="cursor: pointer;">
                <% If Session("insert") = 1 Then%>    
                    <%=Licon_new%> 
                <% Else%>                    
                    <%="<img src='" & Licon_dir & Icono.Text & "' height='16' />" & " " & Nombre.Text%>
                <% End If%>
                
            </a></li>        
            <% End If%>

            </ul>

            

            <div class="tabs_panel">

            <% If Session("insert") = Nothing Then%>

                <asp:Button ID="btn_abre" runat="server" Text="Button" style="display:none"/>
                <asp:TextBox id="opcion" runat="server" style="display:none"></asp:TextBox>
                <asp:TextBox id="opcion_parent" runat="server" style="display:none"></asp:TextBox>    
                <div class="col-sm-offset-1">
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>                
                </div>

            <% Else%>


                <div style="width:54%;display:inline-block;vertical-align:top">                                    

                            <div class="form-group row">
                                <label for="Id" class="col-sm-4 control-label">Id</label>
                                <div class="col-sm-8">
                                    <asp:TextBox class="form-control" id="Codigo" runat="server" placeholder="Registro Nuevo" ReadOnly="True" ></asp:TextBox>
                                </div>
                            </div>
                        
                            <div class="form-group row">
                                <label for="Menu" class="col-sm-4 control-label">Menu ID</label>
                                <div class="col-sm-8">
                                    <asp:TextBox class="form-control" id="Menu" runat="server" placeholder="Ingrese Menu" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="Nombre" class="col-sm-4 control-label">Desc</label>
                                <div class="col-sm-8">
                                    <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="Link" class="col-sm-4 control-label">Link</label>
                                <div class="col-sm-8">
                                    <asp:TextBox class="form-control" id="Link" runat="server" placeholder="Ingrese Link" ></asp:TextBox>
                                </div>
                            </div>
                        
                            <div class="form-group row">
                                <label for="Icono" class="col-sm-4 control-label">Icono</label>
                                <div class="col-sm-8">
                                    <asp:TextBox class="form-control" id="Icono" runat="server" placeholder="Ingrese Icono" ></asp:TextBox>
                                </div>
                            </div>
                                              
                </div>


                <div style="width:45%;display:inline-block;vertical-align:top">

                            <div class="form-group row">
                                <label for="Parent" class="col-sm-5 control-label">Parent</label>
                                <div class="col-sm-6">
                                    <asp:TextBox class="form-control" id="Parent" runat="server" placeholder="Root" ReadOnly="True" ></asp:TextBox>
                                </div>
                            </div>
                                                        
                            <div class="form-group row">
                                <label for="Orden" class="col-sm-5 control-label">Orden</label>
                                <div class="col-sm-6">
                                    <asp:TextBox class="form-control" id="Orden" runat="server" placeholder="Ingrese Orden" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="Childs" class="col-sm-5 control-label">Sub-Menu</label>
                                <div class="col-sm-6">                                                                            
                                    <asp:CheckBox ID="Childs" runat="server" />
                                </div>                                    
                            </div>

                            <div class="form-group row">
                                <label for="Status" class="col-sm-5 control-label">Estado</label>
                                <div class="col-sm-6">                                                                            
                                    <asp:CheckBox ID="Status" runat="server" />
                                </div>
                            </div>

                </div>
                                                 


                <div class="form-group row">
                    <label for="Icono" class="col-sm-2 control-label">File</label>
                    <div class="col-sm-9">
                        <asp:FileUpload ID="FileUpload1" Name="FileUpload1" runat="server" class="form-control"  />
                        <asp:Button ID="btn_upload" runat="server" Text="" style="display:none" />
                    </div>
                </div>


            <% End If%>

                            
           </div>

       </div>

    </div>
    
 
  




</asp:Content>

