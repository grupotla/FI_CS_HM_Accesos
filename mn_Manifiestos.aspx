<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="mn_Manifiestos.aspx.vb" Inherits="mn_Manifiestos" %>

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
                //return false;
            });

            //CheckUnCheck('<%=Activo.Checked%>');



            $('input[type=checkbox]').each(function () {
                if (this.checked) {

                    console.log(this);

                    var s = $(this).hasClass("saved");

                    $(this).addClass("saved");

                }
            });


            $('input[type=radio]').change(function () {

                var id = $(this).attr('id');

                //alert('Radio ' + id);

                var y = id.indexOf("RadioListEmpAsignada");

                //alert(y);

                if (y > -1) {

                    $('input[type=checkbox]').each(function () {

                        var id = $(this).attr('id');

                        //alert('Radio - Check ' + id);

                        var x = id.indexOf("CheckListEmpresas");

                        if (x > -1) {

                            var s = $(this).hasClass("saved");

                            if (this.checked && s == false) {

                                var id1 = document.getElementById(id);

                                id1.checked = !id1.checked;

                            }
                        }


                    });


                }

            });




            $('input[type=checkbox]').change(function () {

                var id = $(this).attr('id');

                var x = id.indexOf("CheckListEmpresas");

                var s = id.slice(-1);

                if (x > -1 && s != "0") {

                    var id1 = document.getElementById(id);
                    var c = id1.checked;
                    var s = $(this).hasClass("saved");

                    if (s) {

                        id1.checked = !id1.checked;

                        return false;

                    } else {

                        if (c == true) {

                            $("#ctl00_ContentPlaceHolder1_RadioListEmpAsignada_0").click();

                        }
                    }

                }

            });

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
            if (!confirm("Confirme Eliminar Este Registro")) {
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

            <% If Session("DBAccesos") <> Nothing And Session("pais") <> Nothing Then %>          

                var checked = valor_checkbox('<%=modulos.ClientID%>');

                if (checked == 999) { alert("Seleccione al menos un Modulo"); return false; }              

            <% End If %>          


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
                
                <% If Session("OperatorLevel") <> Nothing Then%>
                    <asp:LinkButton ID="btn_actualizar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_actualizar()"><%=Licon_update%></asp:LinkButton>
                <% End If%>
                
                <% If Session("OperatorLevel") <> Nothing And Session("pais") <> Nothing Then%>
                    <asp:LinkButton ID="btn_eliminar" runat="server" CssClass="btn btn-sm btn-primary" onclientclick="return valida_eliminar()"><%=Licon_del%></asp:LinkButton>             
                <% End If%>
 
            </div>

        
        
        
        
        <!-- ///////////////////////////////////////// NUEVO CODIGO //////////////////////////////////////////////// -->


        <div class="panel-body">

            
            <!-- aqui va el menu tabs de manifiestos generado desde el codigo-->
            <asp:Label ID="pestana_lbl" runat="server" Text="Label"></asp:Label>


            <div id="catalogo1" class="tabs_panel" >

           
                   

                 <!-- PANEL IZQUIERDO LISTADO DE EMPRESAS -->
                
                <div style="width:31%;display:inline-block;vertical-align:top;">

				    <div class="form-group row">
					    <label for="user_id" class="col-sm-3 control-label">Codigo</label>
					    <div class="col-sm-8">
						    <asp:TextBox class="form-control" id="id_master" runat="server" placeholder=" " ReadOnly="True"></asp:TextBox>
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



                <!-- PANEL CENTRAL DE EMPRESAS YA ALMACENADAS AL USUARIO -->
                
                <div style="width:31%;display:inline-block;vertical-align:top;height:100%;overflow:auto;">

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
               
                
                <div style="width:31%;display:inline-block;vertical-align:top;text-align:center;height:100%;overflow:auto;">

                    <asp:Button ID="btn_activo" runat="server" Text="Button" style="display:none" />
                        

                    <div class="form-group row">
                        <label for="Nombre" class="col-sm-3 control-label">Nombre</label>
                        <div class="col-sm-8">
                            <asp:TextBox class="form-control" id="Nombre" runat="server" placeholder="Ingrese Nombre" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
             

                            
                    <% If Session("DBAccesos") <> Nothing And Session("pais") <> Nothing Then%>          
                        
                        
                        <div class="accordion-group table-bordered" style="background:rgb(240,240,240);padding:5px;;border:0px solid blue;">
                            <div class="accordion-heading btn btn-default btn-block">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#Div1">
                                    Perfil <%=Session("pais")%>
                                </a>
                            </div>
                            <div id="Div1" class="accordion-body collapse in">
                                <div class="accordion-inner">

                                                                        <div class="form-group row">                          
                                        <label for="Activo" class="col-sm-3 control-label">Codigo</label>
                                        <div class="col-sm-6">                          
                    	                <asp:TextBox class="form-control" id="user_id" runat="server" placeholder=" " ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </div>




                                    <div class="form-group row">

                                        <label for="modulos" class="col-sm-3 control-label">Modulos</label>
                                        <div class="col-sm-6">                          
    
                                            <asp:CheckBoxList ID="modulos" runat="server" class="form-control">
                                            <asp:ListItem Text="Aereo" Value="aereo"></asp:ListItem>
                                            <asp:ListItem Text="Maritimo" Value="maritimo"></asp:ListItem>
                                            <asp:ListItem Text="Terrestre" Value="terrestre"></asp:ListItem>
                                            <asp:ListItem Text="Aduana" Value="aduana"></asp:ListItem>
                                            </asp:CheckBoxList>

                                        </div>

                                    </div> 

                                    <br /><br /><br /><br />


                                    <div class="form-group row">                          
                                        <label for="Activo" class="col-sm-3 control-label">Activo</label>
                                        <div class="col-sm-6">                          
                                            <asp:CheckBox ID="Activo" runat="server"  class="form-control"  />                          
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="Pais" class="col-sm-3 control-label">Pais</label>
                                        <div class="col-sm-6">                                                 
                                            <asp:TextBox class="form-control" id="Pais" runat="server" placeholder="Ingrese Pais" ReadOnly="true"></asp:TextBox>                                          
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

