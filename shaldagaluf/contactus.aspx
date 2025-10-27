<%@ Page Title="" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="contactus.aspx.cs" Inherits="contactus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="mainform">
        <h1>צור קשר</h1>

        <div>
            <label for="DropDownList1">נושא פנייה</label>
            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="inputEvent">
                <asp:ListItem Text="בעיה באתר" Value="1"></asp:ListItem>
                <asp:ListItem Text="פנייה ליוצר" Value="2"></asp:ListItem>
                <asp:ListItem Text="סתם לכיף" Value="3"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div>
            <label for="TextBox4">שם פרטי</label>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="inputEvent"></asp:TextBox>
        </div>

        <div>
            <label for="TextBox2">שם משפחה</label>
            <asp:TextBox ID="TextBox2" runat="server" CssClass="inputEvent"></asp:TextBox>
        </div>

        <div>
            <label for="TextBoxEmail">אימייל</label>
            <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="inputEvent" TextMode="Email"></asp:TextBox>
        </div>

        <div>
            <label for="TextBox4">תוכן הפנייה</label>
            <asp:TextBox ID="TextBox4" runat="server" CssClass="inputEvent" TextMode="MultiLine" Rows="8"></asp:TextBox>
        </div>
        
<asp:Button ID="btnSubmit" runat="server" Text="שלח" CssClass="addEventBtn" />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="form-message"></asp:Label>
    </div>
</asp:Content>
