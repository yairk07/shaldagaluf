<%@ Page Title="כל האירועים" Language="C#" 
    MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" 
    CodeFile="allEvents.aspx.cs" 
    Inherits="allEvents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 style="text-align:center;">כל האירועים בכל המשתמשים</h2>
    <div style="width:70%; margin:20px auto; text-align:center;">
    <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" 
                 Placeholder="חפש לפי כותרת / שם משתמש / הערות"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="חפש" CssClass="search-btn"
                OnClick="btnSearch_Click" />
</div>

<asp:Label ID="lblResult" runat="server" CssClass="search-result"></asp:Label>

    <asp:DataList ID="dlEvents" runat="server" 
              RepeatDirection="Vertical"
              RepeatLayout="Table" 
              CssClass="events-table">

    <HeaderTemplate>
        <div class="row header">
            <div class="cell">קוד משתמש</div>
            <div class="cell">שם משתמש</div>
            <div class="cell">כותרת</div>
            <div class="cell">תאריך</div>
            <div class="cell">שעה</div>
            <div class="cell">הערות</div>
            <div class="cell">עריכה</div>
        </div>
    </HeaderTemplate>

    <ItemTemplate>
        <div class="row">
            <div class="cell"><%# Eval("UserId") %></div>
            <div class="cell"><%# Eval("UserName") %></div>
            <div class="cell"><%# Eval("Title") %></div>
            <div class="cell"><%# Eval("EventDate", "{0:dd/MM/yyyy}") %></div>
            <div class="cell"><%# Eval("EventTime") %></div>
            <div class="cell"><%# Eval("Notes") %></div>
            <div class="cell">
                <a href='editEvent.aspx?uid=<%# Eval("UserId") %>' class="edit-link">ערוך</a>
            </div>
        </div>
    </ItemTemplate>

</asp:DataList>


    <style>
      .events-table {
    width: 70%;
    margin: 30px auto;
    font-family: Arial;
    direction: rtl;
}

.row {
    display: flex;
    border-bottom: 1px solid #ddd;
    padding: 6px 0;
    font-size: 15px;
}

.header {
    background: #2b3f5c;
    color: white;
    font-weight: bold;
    border-bottom: 2px solid #1f2d44;
}

.cell {
    flex: 1;
    padding: 4px 8px;
}

.edit-link {
    color: #1f3c88;
    text-decoration: none;
    font-weight: bold;
}

.edit-link:hover {
    text-decoration: underline;
}
.search-box {
    width: 40%;
    padding: 8px;
    border: 1px solid #aaa;
    border-radius: 5px;
    font-size: 14px;
    direction: rtl;
}

.search-btn {
    padding: 8px 15px;
    background: #2b3f5c;
    color: white;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    margin-right: 8px;
}

.search-btn:hover {
    background: #1f2d44;
}

.search-result {
    display: block;
    margin: 10px auto;
    width: 70%;
    text-align: center;
    color: #333;
    font-size: 15px;
}


    </style>

</asp:Content>
