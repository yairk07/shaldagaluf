<%@ Page Title="תוכן" Language="C#" MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" CodeFile="contant.aspx.cs" Inherits="Default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="StyleSheet.css" rel="stylesheet" />

    <style>
        .contant-wrapper {
            width: min(1400px, 95%);
            margin: 40px auto 60px;
            padding: 50px;
            border-radius: 20px;
            background: var(--surface);
            box-shadow: var(--shadow-md);
            border: 1px solid var(--border);
        }

        .content-header {
            text-align: center;
            margin-bottom: 50px;
        }

        .legacy-logo {
            margin-bottom: 25px;
        }

        .legacy-logo img {
            max-width: 200px;
            border-radius: 16px;
            box-shadow: 0 8px 24px rgba(0, 0, 0, 0.3);
        }

        .content-header h2 {
            font-size: 36px;
            font-weight: 700;
            color: var(--heading);
            margin-bottom: 12px;
        }

        .content-header p {
            font-size: 18px;
            color: var(--text);
            opacity: 0.85;
            margin: 0;
        }

        .content-main-img {
            max-width: 600px;
            width: 100%;
            border-radius: 20px;
            box-shadow: var(--shadow-lg);
            margin: 40px auto;
            display: block;
        }

        .weather-section {
            margin: 40px 0;
            text-align: center;
        }

        .cards-section {
            margin-top: 60px;
            margin-bottom: 40px;
            display: flex;
            justify-content: center;
            width: 100%;
        }

        .cards-container {
            display: grid !important;
            grid-template-columns: repeat(4, 1fr);
            gap: 24px;
            max-width: 1200px;
            width: 100%;
        }

        .info-card {
            background: var(--surface);
            padding: 24px;
            border-radius: 16px;
            border: 1px solid var(--border);
            box-shadow: var(--shadow-sm);
            text-align: center;
            cursor: pointer;
            transition: all 0.3s ease;
            display: flex;
            flex-direction: column;
        }

        .info-card:hover {
            transform: translateY(-8px);
            box-shadow: var(--shadow-lg);
            border-color: var(--primary);
        }

        .info-card img {
            width: 100%;
            border-radius: 12px;
            margin-bottom: 16px;
            aspect-ratio: 16/9;
            object-fit: cover;
            transition: transform 0.3s ease;
        }

        .info-card:hover img {
            transform: scale(1.05);
        }

        .info-card p {
            color: var(--text);
            font-size: 17px;
            margin: 0;
            font-weight: 600;
            margin-top: auto;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="contant-wrapper">

        <div class="content-header">
            <div class="legacy-logo">
                <img src="pics/sigma.png" alt="הלוגו הקודם של OptiSched" />
            </div>
            <h2>ברוכים הבאים</h2>
            <p>כאן תוכלו למצוא מידע נוסף, תמונות וקישורים שימושיים.</p>
        </div>

        <img src="pics/הורדה.jpeg" class="content-main-img" alt="תמונת קבוצה" />

        <div class="weather-section">
            <a class="weatherwidget-io"
               href="https://forecast7.com/he/31d0534d85/israel/"
               data-label_1="ISRAEL"
               data-label_2="WEATHER"
               data-theme="original">
               ISRAEL WEATHER
            </a>
        </div>

        <div class="cards-section">
            <asp:DataList ID="dlCards" runat="server"
                          RepeatColumns="3"
                          CssClass="cards-container"
                          RepeatDirection="Horizontal">

                <ItemTemplate>
                    <div class="info-card" onclick="navigateToURL('<%# Eval("url") %>')">
                        <img src='<%# Eval("image") %>' alt='<%# Eval("text") %>' />
                        <p><%# Eval("text") %></p>
                    </div>
                </ItemTemplate>

            </asp:DataList>
        </div>

    </div>

</asp:Content>
