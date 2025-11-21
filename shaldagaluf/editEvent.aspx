<%@ Page Title="עריכת אירוע" Language="C#" MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" CodeFile="editEvent.aspx.cs" Inherits="editEvent" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>עריכת אירוע</h2>

    <asp:Panel ID="pnlForm" runat="server">

        <p><strong>כותרת:</strong><br />
            <asp:TextBox ID="txtTitle" runat="server" Width="300"></asp:TextBox>
        </p>

        <p><strong>תאריך:</strong><br />
            <asp:TextBox ID="txtDate" runat="server" Width="150"></asp:TextBox>
        </p>

        <p><strong>שעה:</strong><br />
            <asp:TextBox ID="txtTime" runat="server" Width="150"></asp:TextBox>
        </p>

        <p><strong>הערות:</strong><br />
            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="120" Width="300"></asp:TextBox>
        </p>

        <asp:Button ID="btnSave" runat="server" Text="שמור" OnClick="btnSave_Click" CssClass="btn btn-primary" />
        <asp:HyperLink ID="lnkBack" runat="server" Text="חזור" NavigateUrl="allEvents.aspx" />

    </asp:Panel>

</asp:Content>
