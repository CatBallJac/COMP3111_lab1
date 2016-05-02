<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Track.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.Track" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Profit or Loss Tracking</h2>
    <asp:Label runat="server" Text="Which type of security to display?"></asp:Label>
    <div>
        <div class="col-md-4">
            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Stock</asp:ListItem>
                <asp:ListItem Value="2">Bond</asp:ListItem>
                <asp:ListItem Value="3">Unit Trust</asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div>
        <asp:GridView ID="gvTrack" runat="server" Visible="True" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="type" HeaderText="Security Type" ReadOnly="True" />
                <asp:BoundField DataField="code" HeaderText="Security Code" ReadOnly="True" />
                <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" />
                <asp:BoundField DataField="shares" HeaderText="Shares" ReadOnly="True" />
                <asp:BoundField DataField="buyingAmount" HeaderText="Buying Amount(HKD)" DataFormatString="{0:n2}" ReadOnly="True" />
                <asp:BoundField DataField="sellingAmount" DataFormatString="{0:n2}" HeaderText="Selling Amount(HKD)" ReadOnly="True" />
                <asp:BoundField DataField="fee" HeaderText="Fee" DataFormatString="{0:n2}" ReadOnly="True" />
                <asp:BoundField DataField="profitLoss" DataFormatString="{0:n2}" HeaderText="Profit or Loss" ReadOnly="True" />

            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:GridView ID="gvAll" runat="server" Visible="True" AutoGenerateColumns="False" OnSelectedIndexChanged="gvAll_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="type" HeaderText="Type" ReadOnly="True" />
                <asp:BoundField DataField="buyingAmount" HeaderText="Buying Amount(HKD)" DataFormatString="{0:n2}" ReadOnly="True" />
                <asp:BoundField DataField="sellingAmount" DataFormatString="{0:n2}" HeaderText="Selling Amount(HKD)" ReadOnly="True" />
                <asp:BoundField DataField="fee" HeaderText="Fee" DataFormatString="{0:n2}" ReadOnly="True" />
                <asp:BoundField DataField="profitLoss" DataFormatString="{0:n2}" HeaderText="Profit or Loss" ReadOnly="True" />

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
