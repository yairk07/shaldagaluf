<%@ Page Title="" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="exusers.aspx.cs" Inherits="exusers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <title>רשימת משתמשים קיימים</title>
</head>
<body>
        <div>
            <asp:TextBox ID="txtSearchemail" runat="server" Placeholder="הכנס שם משתמש"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="btnSearch" runat="server" Text="חפש"  />
        </div>

     <div>
                 <asp:GridView ID="Gridregisterdusers" runat="server"></asp:GridView>

</asp:Content>

