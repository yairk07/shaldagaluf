<%@ Page Title="רשימת משתמשים" Language="C#" MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" CodeFile="exusers.aspx.cs" Inherits="exusers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>רשימת משתמשים קיימים</h2>

    <div>
        <asp:TextBox ID="txtSearchemail" runat="server" Placeholder="חפש משתמש..."></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="חפש" OnClick="btnSearch_Click" />
    </div>

    <asp:DataList ID="DataListUsers" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
        CellPadding="10" CellSpacing="10" style="margin-top: 10px">
        <ItemTemplate>
            <div style="border:1px solid #ccc; border-radius:8px; padding:10px; width:250px;">
                <strong>שם משתמש:</strong> <%# Eval("userName") %><br />
                <strong>שם מלא:</strong> <%# Eval("firstName") %> <%# Eval("lastName") %><br />
                <strong>אימייל:</strong> <%# Eval("email") %><br />
                <strong>טלפון:</strong> <%# Eval("phonenum") %><br />
                <strong>עיר:</strong> <%# Eval("CityName") %><br /><br />

                <asp:Button ID="btnMoreInfo" runat="server"
                    Text="פרטים נוספים"
                    OnClientClick="return false;"
                    CssClass="btn-more" />
            </div>
        </ItemTemplate>
    </asp:DataList>

</asp:Content>
