<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(function () {
            $('#<%=buscar_nombre.ClientID%>').focus();      

            $('#<%=users_grid.ClientID%>').removeClass('text-primary');
            $('#<%=users_grid.ClientID%>').removeClass('text-danger');
            if ($('#<%=inactivo.ClientID%>').attr("checked") == "checked") {             
                $('#<%=users_grid.ClientID%>').addClass('text-warning');
            } else {
                $('#<%=users_grid.ClientID%>').addClass('text-primary');
            }
            
            $("#<%=inactivo.ClientID%>").click(function () {             
                $("#<%=btn_pais.ClientID%>").click();
            });

            $("#<%=pais.ClientID%>").click(function () {             
                $("#<%=btn_pais.ClientID%>").click();
            });

            <% If msg <> "" Then 
                msg = msg.Replace("'", "")
                msg = msg.Replace(vbCrLf, "<br>")
            %>
                $('#modal-texto').html('<%=msg%>');
                $('#modal-img').html('<%=img%>');                
                $('#myAlert').addClass('<%=css%>');                
                $('#myModal').modal();
            <% End If %> 


//            $("a").click(function (event) {//funciona para todos los menu
//                console.log('evento=' + event.currentTarget.name);
//                if (event.currentTarget.name == 'lnk1_') {
//                    $('#< %=opcion1_txt.ClientID%>').removeAttr('disabled');
//                    $('#< %=opcion1_txt.ClientID%>').val(event.currentTarget.id);
//                    $('#< %=opcion1_btn.ClientID%>').click();
//                }
//            });

        });

        function calling(){
            alert('#<%=btn_buscar.ClientID%>');
            $('#<%=btn_buscar.ClientID%>').click();
        }

// ]]>
    </script>

    <style type="text/css">                         
        #<%=users_grid.ClientID%> thead { display:block; width:100%; background:#D9EDF7; border-bottom:1px solid #428BCA; }
        #<%=users_grid.ClientID%> tbody { display:block; width:100%; overflow:auto; height:350px; }        
        #<%=users_grid.ClientID%> th { height:20px; width:150px; text-align:left; border:0px; }
        #<%=users_grid.ClientID%> td { height:20px; width:150px; text-align:left; /*white-space: nowrap; */}        
    </style>

</asp:Content>

<%--

//            $('#<%=users_grid.ClientID%> thead, #<%=users_grid.ClientID%> tbody').css('width','100%');
//            $('#<%=users_grid.ClientID%> thead, #<%=users_grid.ClientID%> tbody').css('display','block');
//            $('#<%=users_grid.ClientID%> thead').css('background','#D9EDF7');
//            $('#<%=users_grid.ClientID%> tbody').css('overflow','auto');
//            $('#<%=users_grid.ClientID%> tbody').css('height','370px');

//            $('#<%=users_grid.ClientID%> th, #<%=users_grid.ClientID%> td').css('height','20px');
//            $('#<%=users_grid.ClientID%> th, #<%=users_grid.ClientID%> td').css('width','150px');
//            $('#<%=users_grid.ClientID%> th, #<%=users_grid.ClientID%> td').css('text-align','left');
//            $('#<%=users_grid.ClientID%> th').css('border','0');--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">




<% If Session("DBAccesosUserId") = Nothing Then%>

    <div class="panel panel-info">

        <div class="panel-heading">        
        </div>


<%-- 

        <div class="col-sm-offset-3 form-group row boton_bar">

            <asp:TextBox ID="buscar_nombre" class="form-control-feedback text-primary" AutoComplete="False" placeholder="Buscar Usuario" runat="server" MaxLength="20" Width="150px" AutoPostBack="True"></asp:TextBox>
            <asp:LinkButton ID="btn_buscar" runat="server" CssClass="btn btn-sm btn-default" ><%=Licon_search%> </asp:LinkButton>                    


<%--            <% If Session("OperatorLevel") = 1 Then%>
                <asp:LinkButton ID="btn_nuevo" runat="server" CssClass="btn btn-sm btn-default"><%=Licon_new%></asp:LinkButton> 
            <% Else%>
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-default disabled"><%=Licon_new%></asp:LinkButton> 
            <% End If%>--%>
<%-- 
            &nbsp;&nbsp;&nbsp;   
            <img src="Content/flags/<%=Session("OperatorCountry")%>-flag.gif" height="28" alt="<%=Session("OperatorCountry")%>" />
       
            <% If Session("OperatorLevel") = 1 Then%>
                &nbsp;<asp:CheckBox ID="pais" runat="server" />
                &nbsp;&nbsp;&nbsp;
                <label for="inactivo" class="control-label">Inactivos</label>
                &nbsp;<asp:CheckBox ID="inactivo" runat="server" />      
                <asp:Button ID="btn_inactivo" runat="server" Text="Button" Visible="True" style="display:none"/>
                <asp:Button ID="btn_pais" runat="server" Text="Button" Visible="True" style="display:none"/>
                <span class="pull-right">
                </span>      
            <% Else%>
                <span class="pull-right">
                </span>      
            <% End If%>
        </div>
--%>            

        <div class="col-sm-offset-3 form-group row boton_bar">
          
            

  <div class="col-lg-3">
    <div class="input-group">        
      <asp:TextBox ID="buscar_nombre" class="form-control" AutoComplete="False" placeholder="Buscar Usuario" runat="server" MaxLength="20" AutoPostBack="True"></asp:TextBox>      

      <span class="input-group-addon btn btn-primary">
        <asp:LinkButton ID="btn_buscar" runat="server" CssClass="" ><%=Licon_search%></asp:LinkButton>
      </span>      

    </div><!-- /input-group -->
  </div><!-- /.col-lg-6 -->




              
              <% If Session("OperatorLevel") = 1 Then%>
                

  <div class="col-lg-2">
    <div class="input-group">
      <span class="input-group-addon btn btn-primary">
        <asp:CheckBox ID="pais" runat="server" />
      </span>      
      <asp:TextBox class="form-control" ><img src="Content/flags/<%=Session("OperatorCountry")%>-flag.gif" height="20" alt="<%=Session("OperatorCountry")%>" /></asp:TextBox>      
    </div><!-- /input-group -->
  </div><!-- /.col-lg-6 -->


              
  <div class="col-lg-2">
    <div class="input-group">
      <span class="input-group-addon btn btn-primary">
        <asp:CheckBox ID="inactivo" runat="server" />   
      </span>        
      <asp:TextBox class="form-control">Inactivos</asp:TextBox>      
    </div><!-- /input-group -->
  </div><!-- /.col-lg-6 -->



                   
                <asp:Button ID="btn_inactivo" runat="server" Text="Button" Visible="True" style="display:none"/>
                <asp:Button ID="btn_pais" runat="server" Text="Button" Visible="True" style="display:none"/>
              <% Else%>

  <div class="col-lg-2">
    <div class="input-group input-group-sm">
      <span class="input-group-addon">
        <img src="Content/flags/<%=Session("OperatorCountry")%>-flag.gif" height="20" alt="<%=Session("OperatorCountry")%>" />
      </span>
    </div><!-- /input-group -->
  </div><!-- /.col-lg-6 -->

              <% End If%>


        </div>


            

    

        <div class="panel-body">
                                                    

            <ul id="pestana" class="nav nav-tabs">            
            <li id="li1_home" class="active"><a id="a_home" name="lnk_" onmouseover="this.style.cursor='pointer'" style="cursor: pointer;"><%=Licon_home%></a></li>            
            <% If Session("OperatorLevel") = 1 Then%>
                <li id="li1_new"><a id="a_new" name="lnk_"  onmouseover="this.style.cursor='pointer';"><%=Licon_new%></a></li>
            <% Else%>
                <li id="" class="disabled"><a id="" name=""><%=Licon_new%></a></li>
            <% End If%>
            </ul>
        
         

         <div class="tabs_panel" style="padding:10px">



            <!-- AutoGenerateSelectButton="True" -->
            <asp:GridView ID="users_grid" runat="server" CssClass="table table-hover table-striped input-sm text-primary" GridLines="None"> 
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>                                                                        
                            <asp:LinkButton ID="lnk_user" runat="server" OnClick="imageButtonClick" Height="20" Width="20"><%=Licon_user%></asp:LinkButton>                        
                            <asp:Image ID="lnk_flag" runat="server" Height="20" />                        
                            <asp:DropDownList ID="lnk_modulos" runat="server" OnChange="OnSelectionChange(this);" Width="80"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>                   
                </Columns>
            </asp:GridView>
        </div>
    <%--        
    <asp:ImageButton runat="server" ID="imageButton" ImageUrl='.png' CommandName="image" CommandArgument='' OnClick="imageButtonClick"   />
    <asp:PlaceHolder ID="MyFlag" runat="server"></asp:PlaceHolder>
    --%>

        </div>

    </div>

<% Else%>

                <div id="myAlert" class="alert-info" >
                    <div class="form-group row">
                        <div id="modal-img" class="col-sm-1 control-label"><%=img%></div>
                        <div id="modal-texto">Usuario Operador no tiene permisos para modulo principal</div>                                
                    </div>                
                </div>


<% End If%>

</asp:Content>



