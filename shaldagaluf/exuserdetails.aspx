<%@ Page Title="פרטי משתמש" Language="C#" MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" CodeFile="exuserdetails.aspx.cs" Inherits="exuserdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- פאנל שמציג את פרטי המשתמש -->
    <asp:Panel ID="pnlContent" runat="server" Visible="false">

        <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="exusers.aspx">
            &laquo; חזרה לרשימת המשתמשים
        </asp:HyperLink>

        <h2>פרטי המשתמש</h2>

        <div style="border:1px solid #ccc; border-radius:10px; padding:15px; max-width:400px;">

            <p><strong>שם משתמש:</strong> <asp:Label ID="lblUserName" runat="server" /></p>
            <p><strong>שם פרטי:</strong> <asp:Label ID="lblFirstName" runat="server" /></p>
            <p><strong>שם משפחה:</strong> <asp:Label ID="lblLastName" runat="server" /></p>
            <p><strong>אימייל:</strong> <asp:Label ID="lblEmail" runat="server" /></p>
            <p><strong>טלפון:</strong> <asp:Label ID="lblPhone" runat="server" /></p>
            <p><strong>עיר:</strong> <asp:Label ID="lblCity" runat="server" /></p>

            <p><strong>רמת גישה:</strong>  
                <asp:Label ID="lblAccessLevel" runat="server" />
            </p>

        </div>

    </asp:Panel>

    <!-- פאנל במקרה שהמשתמש לא נמצא -->
    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">

        <asp:HyperLink ID="lnkBack2" runat="server" NavigateUrl="exusers.aspx">
            &laquo; חזרה לרשימת המשתמשים
        </asp:HyperLink>

        <h2>המשתמש לא נמצא</h2>
        <p>לא נמצאו פרטים עבור המשתמש המבוקש.</p>

    </asp:Panel>

</asp:Content>
