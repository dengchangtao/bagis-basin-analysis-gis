<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PasswordRecovery.aspx.vb" Inherits="webgis.PasswordRecovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        forgot Password
    </h2>
    <br />
    <!-- How to customize email http://www.aspdotnet-suresh.com/2012/01/customize-body-of-email-sent-by.html -->
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
        <MailDefinition BodyFileName="~/EmailTemplate/CustomMail.txt">
        </MailDefinition>
    </asp:PasswordRecovery>
</asp:Content>
