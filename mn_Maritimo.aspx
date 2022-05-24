<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Maritimo.aspx.vb" Inherits="mn_Maritimo" %>

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

            CheckUnCheck('<%=Activo.Checked%>');

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


        /*
        function rdOptionClick(nombre) {
            
            var len = nombre.length;
            var ide = '';
            $("input[type='radio']").each(function () {
                ide = $(this).attr('id');
                if (nombre == ide.substr(0,len)) {
                    if ($(this).context.checked == true) {                        

                        var label = $("label[for='" + $(this).context.id + "']").html()
                        label = label.replace(/"/g, '');
                        label = label.replace(/</g, '[');
                        label = label.replace(/>/g, ']');
                        $('#< % =TextBox7.ClientID % >').val($(this).context.value);
                        $('#< % =TextBox8.ClientID % >').val(label);
                        $('#< % =Button1.ClientID % >').click();

                    }
                }
                console.log(nombre + ')(' + ide.substr(0,len) + ')');
            });            
        }
        */


// ]]>
    </script>

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="panel panel-info">

        <div class="panel-heading">        


        </div>

            <div class="col-sm-offset-6 form-group row boton_bar">

                <% 'If Session("DBAccesos") <> Nothing Then%>                                                                                                                                   
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
                            <%--<asp:LinkButton ID="btn_eliminar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_eliminar()"><%=Licon_off%></asp:LinkButton>             --%>
                        <% Else%>
                            
                        <% End If%>                                                          
                        <asp:LinkButton ID="LinkButton4" runat="server" CssClass="btn btn-sm btn-primary disabled"><%=Licon_off%></asp:LinkButton>
                    <%End If%> 
                <%End If%> 
                <% 'End If%> 

                

            </div>


        <div class="panel-body">

            <!-- aqui va el menu tabs de manifiestos generado desde el codigo-->
            <asp:Label ID="pestana_lbl" runat="server" Text="Label"></asp:Label>

            <div id="catalogo2" class="tabs_panel" >

                <div style="width:33%;display:inline-block;vertical-align:top;">

                    <div class="form-group row">
                        <label for="Login" class="col-sm-3 control-label">Login</label>
                        <div class="col-sm-8">
                            <asp:TextBox class="form-control" id="Login" runat="server" placeholder="Ingrese Login" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                        <div class="col-sm-8">
                            <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                
                    <div class="form-group row">
                        <label for="Email" class="col-sm-3 control-label">Email</label>
                        <div class="col-sm-8">
                            <asp:TextBox class="form-control" id="Email" runat="server"  ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="Password" class="col-sm-3 control-label">Password</label>
                        <div class="col-sm-8">                                                 
                            <asp:TextBox class="form-control" id="Password" runat="server" ReadOnly="True" ></asp:TextBox>                        
                        </div>                                 
                    </div>

                    <div class="form-group row">
                        <label for="Activo" class="col-sm-3 control-label">Activo</label>
                        <div class="col-sm-8">                            
                            <asp:CheckBox ID="Activo" runat="server"></asp:CheckBox>
                        </div>
                    </div>                    

                    <asp:TextBox class="form-control" id="Dominio" runat="server" placeholder="Ingrese Dominio" ReadOnly="True" style="display:none"></asp:TextBox>

                </div>
                <!-- PANEL IZQUIERDO DE BASES DE DATOS -->
                
                <div style="width:33%;display:inline-block;vertical-align:top;height:100%;overflow:auto;">

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
                                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" class="form-control1" AutoPostBack="True" CausesValidation="True">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>                       
                            </div>

                            <br />

                            <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;;border:0px solid blue;">
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


                            
                        <% If Session("DBAccesos") <> Nothing Then%>                                           
                                     
                                <label for="Perfil" class="col-sm-10 control-label">Perfiles <%=Session("pais")%></label>
                                <div class="col-sm-offset-0 col-sm-10 checkbox">
                                    <asp:checkboxlist id="Perfil" runat="server" class="form-control">
                                    </asp:checkboxlist>
                                </div>                                                            

                        <%End If%>
             
                </div>

            </div>

        </div>

    </div>


</asp:Content>

