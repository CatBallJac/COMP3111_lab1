<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientReport.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Report Client</h2>

    <h3>Summary</h3>

    <div>
        <asp:Label runat="server" Text="Account Number:" AssociatedControlID="txtAccountNumber"></asp:Label>
        <asp:Label runat="server" Text="" ID="txtAccountNumber"></asp:Label>
    </div>

    <div>
        <div class="col-md-4">
            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged" >
                <asp:ListItem Value="0">Currency</asp:ListItem>
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <asp:GridView ID="gvSummary" runat="server" Visible="True" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="totalValue" HeaderText="Total Value(HKD)" DataFormatString="{0:n2}" ReadOnly="True" SortExpression="totalValue" />
            <asp:BoundField DataField="convertedValue" DataFormatString="{0:n2}" HeaderText="Value in" ReadOnly="True" Visible="false" SortExpression="convertedValue" />
            <asp:BoundField DataField="freeBalance" HeaderText="Free Balance" DataFormatString="{0:n2}" ReadOnly="True" SortExpression="freeBalance" />
            <asp:BoundField DataField="stockValue" HeaderText="Stock Value(HKD)" DataFormatString="{0:n2}" ReadOnly="True" SortExpression="stockValue" />
            <asp:BoundField DataField="bondValue" HeaderText="Bond Value(HKD)" DataFormatString="{0:n2}" ReadOnly="True" SortExpression="bondValue" />
            <asp:BoundField DataField="unitTrustValue" DataFormatString="{0:n2}" HeaderText="Unit Trust Value(HKD)" ReadOnly="True" SortExpression="unitTrustValue" />
            <asp:BoundField DataField="lastOrderDate" HeaderText="Date of Last Order" ReadOnly="True" SortExpression="lastOrderDate" />
            <asp:BoundField DataField="lastOrderValue" DataFormatString="{0:n2}" HeaderText="Value of Last Order" ReadOnly="True" SortExpression="lastOrderValue" />

        </Columns>
    </asp:GridView>

    <div>
        <div>


            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                <asp:ListItem Value="0">Security type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                <asp:ListItem></asp:ListItem>
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
            
        </div>
        <div>
            <asp:Label ID="lblClientName" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblResultMessage" runat="server" Visible="false"></asp:Label>
        </div>
        <div>
            <asp:GridView ID="gvSecurityHolding" runat="server" Visible="False" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" AllowSorting="true">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" SortExpression="shares" />
                    <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" />
                    <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Price" ReadOnly="True" />
                    <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="convertedValue" DataFormatString="{0:n2}" HeaderText="Value in" ReadOnly="True" SortExpression="convertedValue" />
                </Columns>
                <RowStyle HorizontalAlign="Left" />
            </asp:GridView>
        </div>
        <div>
            <asp:GridView ID="gvOrder" runat="server" Visible="True" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gvOrder_Sorting">
                <Columns>
                    <asp:BoundField DataField="referenceNumber" HeaderText="Reference Number" ReadOnly="True" />
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True" />
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True" />
                    <asp:BoundField DataField="securityCode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="securityName" HeaderText="Security Name" ReadOnly="True" />
                    <asp:BoundField DataField="dateSubmitted" HeaderText="Submitted Date" ReadOnly="True" SortExpression="dateSubmitted" />
                    <asp:BoundField DataField="status" HeaderText="Current Status" ReadOnly="True" />
                    <asp:BoundField DataField="amount" HeaderText="Amount" ReadOnly="True" DataFormatString="{0:n2}" />
                    <asp:BoundField DataField="shares" HeaderText="Quantity of shares" ReadOnly="True" />
                    <asp:BoundField DataField="limitPrice" HeaderText="Limit Price" ReadOnly="True" DataFormatString="{0:n2}" />
                    <asp:BoundField DataField="stopPrice" HeaderText="Stop Price" ReadOnly="True" DataFormatString="{0:n2}" />
                    <asp:BoundField DataField="expiryDay" HeaderText="Expiry Date" ReadOnly="True" />
                </Columns>
                <RowStyle HorizontalAlign="Left" />
            </asp:GridView>
        </div>

        <div>
            <asp:CheckBox ID="cbCalendar" runat="server" AutoPostBack="true" OnCheckedChanged="cbCalendar_CheckedChanged" />
            <asp:Label runat="server" Text="Do you want to set a range of dates?"></asp:Label>

        </div>
        <div>
            <asp:Label runat="server" Text="Start date" Visible="false"></asp:Label>
            <asp:Calendar ID="startDate" runat="server" AutoPostBack="true" Visible="false"></asp:Calendar>

        </div>
        <div>
            <asp:Label runat="server" Text="End date" Visible="false"></asp:Label>
            <asp:Calendar ID="endDate" runat="server" AutoPostBack="true" Visible="false"></asp:Calendar>
            <asp:CustomValidator ID="cvDate" runat="server" Display="Dynamic" CssClass="text-danger" EnableClientScript="false" OnServerValidate="cvDate_ServerValidate" Enabled="false"></asp:CustomValidator>
        </div>
        <asp:Button ID="searchHistory" runat="server" Text="search" OnClick="searchHistory_Click" />
        <div>
            <asp:GridView ID="gvHistory" runat="server" Visible="True" AutoGenerateColumns="False" AllowSorting="true" OnSorting="gvHistory_Sorting">
                <Columns>
                    <asp:BoundField DataField="referenceNumber" HeaderText="Reference Number" ReadOnly="True" />
                    <asp:BoundField DataField="buyOrSell" HeaderText="Buy or Sell" ReadOnly="True" />
                    <asp:BoundField DataField="securityType" HeaderText="Security Type" ReadOnly="True" SortExpression="securityType" />
                    <asp:BoundField DataField="securityCode" HeaderText="Security Code" ReadOnly="True" />
                    <asp:BoundField DataField="securityName" HeaderText="Security Name" ReadOnly="True" SortExpression="securityName" />
                    <asp:BoundField DataField="dateSubmitted" HeaderText="Submitted Date" ReadOnly="True" SortExpression="dateSubmitted" />
                    <asp:BoundField DataField="status" HeaderText="Current Status" ReadOnly="True" SortExpression="status" />
                    <asp:BoundField DataField="shares" HeaderText="Quantity of shares" ReadOnly="True" />
                    <asp:BoundField DataField="executedAmount" HeaderText="Executed amount" ReadOnly="True" />
                    <asp:BoundField DataField="fee" HeaderText="Fee charged" ReadOnly="True" />

                </Columns>
            </asp:GridView>

        </div>
        <asp:Label runat="server" Text="Transaction"></asp:Label>
        <div>
            <asp:GridView ID="gvTransaction" runat="server" Visible="True" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="transactionNumber" HeaderText="Transaction number" ReadOnly="True" />
                    <asp:BoundField DataField="executedDate" HeaderText="Executed date" ReadOnly="True" SortExpression="executedDate" />
                    <asp:BoundField DataField="executedShares" HeaderText="Executed shares" ReadOnly="True" />
                    <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:n2}" ReadOnly="True" />
                </Columns>
            </asp:GridView>

        </div>


    </div>
</asp:Content>
