<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
// <![CDATA[
        function valida() {
            if ($('#<%=user_txt.ClientID%>').val() == "") { alert("Ingrese Usuario"); $('#<%=user_txt.ClientID%>').focus(); return false; }
            if ($('#<%=pass_txt.ClientID%>').val() == "") { alert("Ingrese Contraseña"); $('#<%=pass_txt.ClientID%>').focus(); return false; }
            return true;
       }       

       $(function () {
           // Document is ready
           $('#<%=user_txt.ClientID%>').focus();

            <% If msg <> "" Then 
                msg = msg.Replace("'", "")
                msg = msg.Replace(vbCrLf, "<br>")
            %>
                $('#modal-texto').html('<%=msg%>');
                $('#modal-img').html('<%=img%>');                
                $('#myAlert').addClass('<%=css%>');                
                $('#myModal').modal();
            <% End If %> 

            $('#<%=user_txt.ClientID%>,#<%=pass_txt.ClientID%>').keypress(function(e) {
                if (e.which == 13 || e.keyCode == 13) { 
                    //return valida();
                }
            });

            $('#<%=btn_login.ClientID%>').click(function() {            
                return valida();
            });

       });       

// ]]>
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   

    <%-- 
    <div class="panel panel-info">         
            <div class="row" style="padding:10px;">                            
                <div class="col-sm-3">
                    <asp:TextBox ID="user_txt" runat="server" MaxLength="20" class="form-control" placeholder="Usuario" ></asp:TextBox>
                </div>                
                <div class="col-sm-3">
                    <asp:TextBox ID="pass_txt" runat="server" MaxLength="20" class="form-control" TextMode="Password" placeholder="Contraseña" ></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:LinkButton ID="btn_login" runat="server" CssClass="btn btn-sm btn-primary"><%=Licon_login% >&nbsp;Entrar</asp:LinkButton>
                </div>
            </div>

    </div>
            
            --%>

        <div class="col-sm-offset-0 row">
          
          <div class="col-lg-3">
            <div class="input-group">
                <span class="input-group-addon btn-primary"><%=Licon_user%></span>
                <asp:TextBox ID="user_txt" runat="server" MaxLength="20" class="form-control" placeholder="Usuario" ></asp:TextBox>
            </div>            
          </div>

          <div class="col-lg-4">
            <div class="input-group">
              <span class="input-group-addon btn-primary"><%=Licon_keys%></span>
              <asp:TextBox ID="pass_txt" runat="server" MaxLength="50" class="form-control" TextMode="Password" placeholder="Contraseña" ></asp:TextBox>             
            </div><!-- /input-group -->
          </div><!-- /.col-lg-6 -->


          <div class="col-lg-1">
            <div class="input-group">
                <!--
              <span class="input-group-addon btn-primary"><%=Licon_login%></span>
              <asp:LinkButton ID="btn_login" runat="server" CssClass="btn btn-sm">Entrar</asp:LinkButton>
              -->              

              <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-sm btn-primary"><%=Licon_login%>&nbsp;&nbsp;Entrar</asp:LinkButton>

            </div><!-- /input-group -->
          </div><!-- /.col-lg-6 -->

        </div>




</asp:Content>
