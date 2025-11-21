<%@ Page Title="רשימת משתמשים" Language="C#" MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" CodeFile="exusers.aspx.cs" Inherits="exusers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>רשימת משתמשים קיימים</h2>

    <!-- חיפוש -->
    <div>
        <asp:TextBox ID="txtSearchemail" runat="server" Placeholder="חפש משתמש..."></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="חפש" OnClick="btnSearch_Click" />
    </div>

    <br />

    <!-- רשימת המשתמשים -->
    <asp:DataList ID="DataListUsers" runat="server"
        RepeatColumns="3"
        RepeatDirection="Horizontal"
        CellPadding="10"
        CellSpacing="10"
        Style="margin-top: 10px">

        <ItemTemplate>
            <div style="border:1px solid #ccc; border-radius:8px; padding:10px; width:250px;">

                <strong>שם משתמש:</strong> <%# Eval("userName") %><br />
                <strong>שם מלא:</strong> <%# Eval("firstName") %> <%# Eval("lastName") %><br />
                <strong>אימייל:</strong> <%# Eval("email") %><br />
                <strong>טלפון:</strong> <%# Eval("phonenum") %><br />
                <strong>עיר:</strong> <%# GetCity(Container.DataItem) %><br /><br />

                <!-- מעבר לדף פרטים לפי אימייל -->
                <asp:HyperLink ID="lnkMoreInfo" runat="server"
    Text="פרטים נוספים"
    NavigateUrl='<%# "exuserdetails.aspx?id=" + Eval("id") %>' />

            </div>
        </ItemTemplate>

    </asp:DataList>

</asp:Content>
