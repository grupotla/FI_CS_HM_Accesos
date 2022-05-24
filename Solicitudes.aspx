<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Solicitudes.aspx.vb" Inherits="Solicitudes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

            <asp:Label ID="msg" runat="server" Text="Label"></asp:Label>

            <asp:GridView ID="solicitudes_grid" runat="server" CssClass="table table-hover table-striped input-sm text-primary" GridLines="None" > 
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>                                                                        
                            <asp:LinkButton ID="lnk_user" runat="server" OnClick="imageButtonClick" Height="20" Width="20">Abrir</asp:LinkButton>                                                                                
                        </ItemTemplate>
                    </asp:TemplateField>                   
                </Columns>
            </asp:GridView>

</asp:Content>

