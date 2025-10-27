<%@ Page Title="Task Calendar" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="tasks.aspx.cs" Inherits="tasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="styles.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlTasks" runat="server" CssClass="tasks-section">
        <h2>הוספת אירועים לתאריך נבחר</h2>

        <!-- לוח שנה -->
        <div class="calendar-wrapper">
            <asp:Calendar ID="calendar" runat="server" Width="100%" Height="300px" CssClass="calendar"
                OnSelectionChanged="calendar_SelectionChanged" OnDayRender="calendar_DayRender" />
        </div>

        <!-- תאריך נבחר -->
        <asp:Label ID="lblSelectedDate" runat="server" CssClass="selected-date" />

        <!-- שדות להוספת אירוע -->
        <div class="form-group">
            <asp:TextBox ID="txtTitle" runat="server" CssClass="inputEvent" placeholder="כותרת האירוע" />
        </div>
        <div class="form-group">
            <asp:TextBox ID="txtTime" runat="server" CssClass="inputEvent" placeholder="שעה (למשל 14:30)" />
        </div>
        <div class="form-group">
            <asp:TextBox ID="txtNote" runat="server" CssClass="inputEvent" TextMode="MultiLine" Rows="3" placeholder="הערות (אופציונלי)" />
        </div>

        <!-- כפתור להוספה -->
        <asp:Button ID="btnAddEvent" runat="server" Text="הוסף אירוע" OnClick="AddEvent" />

        <!-- הצגת האירועים -->
        <div>
            <h3>האירועים שלי:</h3>
            <asp:Label ID="lblEvents" runat="server" CssClass="events-list" />
        </div>
    </asp:Panel>
</asp:Content>
