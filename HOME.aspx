<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HOME.aspx.cs" Inherits="HKeInvestWebApplication.HOME" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Home Page</h2>

    <div class="form-group">
        <div class="col-md-4">
            <asp:Label ID="type" runat="server" Text="Type"></asp:Label>
            <asp:DropDownList ID="ddlType" runat="server">
                <asp:ListItem>--Select a type of security--</asp:ListItem>
                <asp:ListItem>Bond</asp:ListItem>
                <asp:ListItem>UnitTrust</asp:ListItem>
                <asp:ListItem>Stock</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ControlToValidate="ddlType" InitialValue="--Select a type of security--" runat="server" Display="Dynamic" CssClass="text-danger" EnableClientScript="false" ErrorMessage="Must select a type of security"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-4">
            <asp:CheckBox ID="CheckBoxName" runat="server" OnCheckedChanged="CheckBoxName_CheckedChanged" AutoPostBack="true" />
            <asp:Label ID="LabelName" runat="server" Text="Name"></asp:Label>
            <asp:TextBox ID="TextBoxName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredName" ControlToValidate="TextBoxName" runat="server" ErrorMessage="Name of security is required." Display="Dynamic" CssClass="text-danger" EnableClientScript="false" Enabled="false"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="form-group">
        <div class="col-md-4">
            <asp:CheckBox ID="CheckBoxCode" runat="server" OnCheckedChanged="CheckBoxCode_CheckedChanged" AutoPostBack="true" />
            <asp:Label ID="LabelCode" runat="server" Text="Code"></asp:Label>
            <asp:TextBox ID="TextBoxCode" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredCode" ControlToValidate="TextBoxCode" runat="server" ErrorMessage="Code of security is required." Display="Dynamic" CssClass="text-danger" EnableClientScript="false" Enabled="false"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator runat="server" CssClass="text-danger" Display="Dynamic" ControlToValidate="TextBoxCode" ValidationExpression="^[0-9]{0,}$" EnableClientScript="false" ErrorMessage="Must be digital"></asp:RegularExpressionValidator>
            <asp:CustomValidator ID="CustomCode" ControlToValidate="TextBoxCode" CssClass="text-danger" Display="Dynamic" EnableClientScript="false" runat="server" ErrorMessage="The security code must contain 1-4 effective digits." OnServerValidate="CustomCode_ServerValidate"></asp:CustomValidator>
        </div>
    </div>


    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <asp:Button ID="search" runat="server" Text="Search" Height="34px" CssClass="btn button-default" OnClick="search_Click" />
        </div>
    </div>

    <div>
        <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="false" Visible="false" AllowSorting="true" OnSorting="gvStock_Sorting">
            <Columns>
                <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" SortExpression="code" />
                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" SortExpression="name" />
                <asp:BoundField DataField="close" HeaderText="Recent Price" ReadOnly="true" SortExpression="close" />
                <asp:BoundField DataField="changePercent" HeaderText="Price change in percentage" ReadOnly="true" SortExpression="changePercent" />
                <asp:BoundField DataField="changeDollar" HeaderText="Price change in dollar" ReadOnly="true" SortExpression="changeDollar" />
                <asp:BoundField DataField="volume" HeaderText="Trade Volume" ReadOnly="true" SortExpression="volume" />
                <asp:BoundField DataField="high" HeaderText="High Price" ReadOnly="true" SortExpression="high" />
                <asp:BoundField DataField="low" HeaderText="Low Price" ReadOnly="true" SortExpression="low" />
                <asp:BoundField DataField="peRatio" HeaderText="Price Earning Ratio" ReadOnly="true" SortExpression="peRatio" />
                <asp:BoundField DataField="yield" HeaderText="Yield of Stock" ReadOnly="true" SortExpression="yield" />
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:GridView ID="gvBond" runat="server" AutoGenerateColumns="false" Visible="false" AllowSorting="true" OnSorting="gvBond_Sorting">
            <Columns>
                <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" SortExpression="code" />
                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" SortExpression="name" />
                <asp:BoundField DataField="base" HeaderText="Base Currency" ReadOnly="true" SortExpression="base" />
                <asp:BoundField DataField="size" HeaderText="Size of Bond" ReadOnly="true" SortExpression="size" />
                <asp:BoundField DataField="price" HeaderText="Current Price" ReadOnly="true" SortExpression="price" />
                <asp:BoundField DataField="sixMonths" HeaderText="Compound Annual Growth in 6 months" ReadOnly="true" SortExpression="sixMonths" />
                <asp:BoundField DataField="oneYear" HeaderText="Compound Annual Growth in one year" ReadOnly="true" SortExpression="oneYear" />
                <asp:BoundField DataField="threeYears" HeaderText="Compound Annual Growth in 3 years" ReadOnly="true" SortExpression="threeYears" />
                <asp:BoundField DataField="sinceLaunch" HeaderText="Compound Annual Growth Since Launch" ReadOnly="true" SortExpression="sinceLaunch" />

            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:GridView ID="gvUnitTrust" runat="server" AutoGenerateColumns="false" Visible="false" AllowSorting="true" OnSorting="gvUnitTrust_Sorting">
            <Columns>
                <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="true" SortExpression="code" />
                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" SortExpression="name" />
                <asp:BoundField DataField="base" HeaderText="Base Currency" ReadOnly="true" SortExpression="base" />
                <asp:BoundField DataField="size" HeaderText="Size of Bond" ReadOnly="true" SortExpression="size" />
                <asp:BoundField DataField="price" HeaderText="Current Price" ReadOnly="true" SortExpression="price" />
                <asp:BoundField DataField="riskReturn" HeaderText="Risk Return" ReadOnly="true" SortExpression="riskReturn" />
                <asp:BoundField DataField="sixMonths" HeaderText="Compound Annual Growth in 6 months" ReadOnly="true" SortExpression="sixMonths" />
                <asp:BoundField DataField="oneYear" HeaderText="Compound Annual Growth in one year" ReadOnly="true" SortExpression="oneYear" />
                <asp:BoundField DataField="threeYears" HeaderText="Compound Annual Growth in 3 years" ReadOnly="true" SortExpression="threeYears" />
                <asp:BoundField DataField="sinceLaunch" HeaderText="Compound Annual Growth Since Launch" ReadOnly="true" SortExpression="sinceLaunch" />
            </Columns>
        </asp:GridView>
    </div>


</asp:Content>
