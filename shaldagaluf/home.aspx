<%@ Page Title="" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="styles.css" />
    <script src="JavaScript.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlAbout" runat="server" CssClass="about-section">
        <asp:Label ID="lblWelcome" runat="server" CssClass="welcome-label" />

        <!-- Calendar -->
        <div class="calendar-wrapper">
            <asp:Calendar ID="calendar" runat="server"
                          Width="100%" Height="300px"
                          CssClass="calendar"
                          OnSelectionChanged="calendar_SelectionChanged"
                          OnDayRender="calendar_DayRender" />
        </div>
        <asp:Label ID="lblSelectedDate" runat="server" CssClass="selected-date" />

        <!-- Events -->
        <div>
            <h3>האירועים שלך:</h3>
            <asp:Label ID="lblEvents" runat="server" CssClass="events-list" />
        </div>
       
        <!-- Quote -->
        <div class="quote-section">
            <p class="quote">
                "Guns, like any tool, are neither inherently good nor evil—they merely reflect the hands that wield them..."
            </p>
        </div>

        <!-- Gallery -->
        <div class="gallery">
            <button id="prevBtn" type="button" onclick="changeImage(-1)">&#10094;</button>
            <img id="galleryImage" src="pics/gun1.jpg" alt="Gallery Image" class="gallery-img" />
            <button id="nextBtn" type="button" onclick="changeImage(1)">&#10095;</button>
        </div>

        <!-- Russian Roulette Game -->
        <div class="roulette-game">
            <h3>משחק רולטה רוסית</h3>
            <input type="text" id="word1" class="word-input" placeholder="אירוע ראשון" />
            <input type="text" id="word2" class="word-input" placeholder="אירוע שני" />
            <button type="button" onclick="playRoulette()" class="roulette-btn">ירי!</button>

            <div id="rouletteResult" class="roulette-result"></div>

            <!-- הפסד -->
            <div id="lossSection" class="loss-section" style="display: none;">
                <div class="loss-wrapper">
                    <img id="lossImage" src="pics/gunauto.gif" alt="הפסד" class="loss-gif" />
                    <div id="loserName" class="loser-name-overlay"></div>
                </div>
            </div>
        </div>

        <!-- Decorative GIF -->
        <div class="roulette-animation">
            <h3>רולטה רוסית:</h3>
            <img src="pics/revolver.gif" alt="Russian Roulette Animation" class="roulette-gif" />
        </div>
    </asp:Panel>
</asp:Content>
