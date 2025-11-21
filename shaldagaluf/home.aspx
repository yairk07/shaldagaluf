<%@ Page Title="" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="styles.css" />
    <script src="JavaScript.js" defer></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="pnlAbout" runat="server" CssClass="about-section home-shell">

        <section class="home-hero">
            <div class="hero-text">
                <span class="hero-eyebrow">מרכז השליטה שלך</span>
                <asp:Label ID="lblWelcome" runat="server" CssClass="welcome-label hero-title" />
                <p class="hero-description">
                    נטר אירועים קרובים, קבע משימות חדשות והישאר מעודכן בלוח השנה של היחידה – הכל ממסך אחד מותאם לכל מכשיר.
                </p>
                <div class="hero-actions">
                    <a class="btn-pill primary" href="tasks.aspx">טופס משימות</a>
                    <a class="btn-pill ghost" href="allEvents.aspx">כל האירועים</a>
                </div>
            </div>

            <div class="hero-summary">
                <div class="stat-chip">
                    <span class="chip-label">תאריך נבחר</span>
                    <asp:Label ID="lblSelectedDate" runat="server" CssClass="chip-value" Text="בחרו יום בלוח כדי להתחיל" />
                </div>
                <div class="stat-chip">
                    <span class="chip-label">זמן אמת</span>
                    <span class="chip-value muted">האירועים מתעדכנים אוטומטית לאחר בחירת תאריך</span>
                </div>
            </div>
        </section>

        <section class="home-grid">
            <article class="home-card calendar-card">
                <div class="card-header">
                    <div>
                        <h3>לוח פעילות</h3>
                        <p class="card-subtitle">בחר תאריך כדי לצפות בכל מה שמתוכנן</p>
                    </div>
                    <a class="link-inline" href="editEvent.aspx">ניהול אירועים</a>
                </div>

                <div class="calendar-wrapper">
                    <asp:Calendar ID="calendar" runat="server"
                          Width="100%" Height="300px"
                          CssClass="calendar"
                          OnSelectionChanged="calendar_SelectionChanged"
                          OnDayRender="calendar_DayRender" />
                </div>
            </article>

            <article class="home-card events-card">
                <div class="card-header">
                    <div>
                        <h3>האירועים שלך</h3>
                        <p class="card-subtitle">תוצאה יומית לפי התאריך שבחרת</p>
                    </div>
                    <a class="link-inline" href="allEvents.aspx">צפייה בכל האירועים</a>
                </div>
                <asp:Label ID="lblEvents" runat="server" CssClass="events-feed events-list" />
            </article>
        </section>

        <section class="insights-grid">
            <article class="home-card quote-card">
                <h3>השראה לדרך</h3>
                <p class="quote">
                    "Guns, like any tool, are neither inherently good nor evil—they merely reflect the hands that wield them..."
                </p>
            </article>

            <article class="home-card gallery-card">
                <div class="card-header">
                    <div>
                        <h3>גלריית יחידה</h3>
                        <p class="card-subtitle">גלול בין התמונות האהובות</p>
                    </div>
                    <div class="gallery-controls">
                        <button id="prevBtn" type="button" onclick="changeImage(-1)" aria-label="תמונה קודמת">&#10094;</button>
                        <button id="nextBtn" type="button" onclick="changeImage(1)" aria-label="תמונה הבאה">&#10095;</button>
                    </div>
                </div>
                <img id="galleryImage" src="pics/gun1.jpg" alt="Gallery Image" class="gallery-img" />
            </article>

            <article class="home-card roulette-card">
                <h3>משחק רולטה רוסית</h3>
                <p class="card-subtitle">בחר שני אירועים ותן למזל לבחור</p>
                <div class="roulette-inputs">
                    <input type="text" id="word1" class="word-input" placeholder="אירוע ראשון" />
                    <input type="text" id="word2" class="word-input" placeholder="אירוע שני" />
                </div>
                <button type="button" onclick="playRoulette()" class="roulette-btn">ירי!</button>

                <div id="rouletteResult" class="roulette-result"></div>

                <div id="lossSection" class="loss-section" style="display: none;">
                    <div class="loss-wrapper">
                        <img id="lossImage" src="pics/gunauto.gif" alt="הפסד" class="loss-gif" />
                        <div id="loserName" class="loser-name-overlay"></div>
                    </div>
                </div>
            </article>

            <article class="home-card gif-card">
                <h3>אנימציית מוטיבציה</h3>
                <img src="pics/revolver.gif" alt="Russian Roulette Animation" class="roulette-gif" />
            </article>
        </section>
    </asp:Panel>
</asp:Content>
