<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationPage.aspx.cs" Inherits="HKeInvestWebApplication.RegistrationPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" EnableViewState="True">
    <h4>Create a new login account</h4>
    <div>
        <asp:Label runat="server" Text="First Name" AssociatedControlID="FirstName" ClientIDMode="Inherit"></asp:Label>
        <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
        <asp:Label runat="server" Text="Last Name" Font-Overline="False" AssociatedControlID="LastName"></asp:Label>
        <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
    </div>

    <div>
        <asp:Label runat="server" Text="Account#" AssociatedControlID="AccountNumber"></asp:Label>
        <asp:TextBox ID="AccountNumber" runat="server"></asp:TextBox>
        <asp:Label runat="server" Text="HKID/Passport#" AssociatedControlID="HKID"></asp:Label>
        <asp:TextBox ID="HKID" runat="server"></asp:TextBox>
    </div>

    <div>
        <asp:Label  runat="server" Text="Date Of Birth" AssociatedControlID="DateOfBirth"></asp:Label>
        <asp:TextBox ID="DateOfBirth" runat="server"></asp:TextBox>
        <asp:Label runat="server" Text="Email" AssociatedControlID="Email"></asp:Label >
        <asp:TextBox ID="Email" runat="server" TextMode="Email"></asp:TextBox>
    </div>

    <div>
        <asp:Label  runat="server" Text="User Name" AssociatedControlID="UserName"></asp:Label>
        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
    </div>

    <div>
        <asp:Label  runat="server" Text="Password" AssociatedControlID="Password"></asp:Label>
        <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
    <asp:Label  runat="server" Text="Confirm Password" AssociatedControlID="ConfirmPassword"></asp:Label>
    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
    </div>

    <div>
        <asp:Button ID="Register" runat="server" Text="Register" />
    </div>
</asp:Content>
