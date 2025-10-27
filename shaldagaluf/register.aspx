<%@ Page Title="" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="mainform">
        <h1>הירשם</h1>
        
        <table>
            <tr>
                <td><label for="txtUsername">שם משתמש</label></td>
                <td><asp:TextBox ID="txtUsername" runat="server" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtFirstName">שם פרטי</label></td>
                <td><asp:TextBox ID="txtFirstName" runat="server" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtLastName">שם משפחה</label></td>
                <td><asp:TextBox ID="txtLastName" runat="server" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtEmail">אימייל</label></td>
                <td><asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtPassword">סיסמה</label></td>
                <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtConfirmPassword">הקלדה חוזרת של סיסמה</label></td>
                <td><asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label>מין</label></td>
                <td>
                    <asp:RadioButtonList ID="rblGender" runat="server" CssClass="radioButtonList">
                        <asp:ListItem Text="זכר" Value="1"></asp:ListItem>
                        <asp:ListItem Text="נקבה" Value="2"></asp:ListItem>
                        <asp:ListItem Text="אחר" Value="3"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td><label for="txtPhone">טלפון</label></td>
                <td><asp:TextBox ID="txtPhone" runat="server" TextMode="Phone" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
            <tr>
                <td><label for="txtID">תעודת זהות</label></td>
                <td><asp:TextBox ID="txtID" runat="server" CssClass="inputEvent"></asp:TextBox></td>
            </tr>
           <tr>
    <td><label for="ddlOptions">בחר עיר</label></td>
    <td>
        <asp:DropDownList ID="ddlOptions" runat="server" CssClass="inputEvent">
            <asp:ListItem Text="בחר עיר" Value="" />
            <asp:ListItem Text="תל אביב" Value="1" />
            <asp:ListItem Text="ירושלים" Value="2" />
            <asp:ListItem Text="חיפה" Value="3" />
            <asp:ListItem Text="באר שבע" Value="4" />
            <asp:ListItem Text="ראשון לציון" Value="5" />
            <asp:ListItem Text="נתניה" Value="6" />
        </asp:DropDownList>
    </td>
</tr>

            <tr>
                <tr>
    <td><label for="txtYearOfBirth">שנת לידה</label></td>
    <td><asp:TextBox ID="txtYearOfBirth" runat="server" CssClass="inputEvent"></asp:TextBox></td>
</tr>

                <td colspan="2" style="text-align: center;">
                    <asp:Button ID="btnRegister" runat="server" Text="שלח" OnClick="btnRegister_Click" CssClass="addEventBtn" />
                </td>
            </tr>
        </table>
        
        <br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
