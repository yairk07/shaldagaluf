<%@ Page Title="תוכן" Language="C#" MasterPageFile="~/danimaster.master"
    AutoEventWireup="true" CodeFile="contant.aspx.cs" Inherits="Default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="StyleSheet.css" rel="stylesheet" />

    <style>
        .contant-wrapper {
            max-width: 900px;
            margin: 140px auto 50px;
            padding: 30px;
            border-radius: 16px;
            background: #fff;
            box-shadow: 0 6px 20px rgba(0,0,0,.08);
            text-align: center;
        }

        .contant-main-img {
            width: 300px;
            border-radius: 12px;
            box-shadow: 2px 2px 12px rgba(0,0,0,.2);
            margin: 20px 0;
        }

        .cards-container {
            display: flex !important;
            flex-wrap: wrap;
            justify-content: center;
            gap: 25px;
            margin-top: 30px;
        }

        .info-card {
            width: 260px;
            background: #ffffff;
            padding: 15px;
            border-radius: 12px;
            border: 1px solid #ccc;
            box-shadow: 0 4px 14px rgba(0,0,0,.12);
            text-align: center;
            cursor: pointer;
            transition: 0.25s;
        }

        .info-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 6px 18px rgba(0,0,0,.18);
        }

        .info-card img {
            width: 100%;
            border-radius: 10px;
            margin-bottom: 12px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="contant-wrapper">

        <h2>ברוכים הבאים</h2>
        <p>כאן תוכלו למצוא מידע נוסף, תמונות וקישורים שימושיים.</p>

        <img src="pics/הורדה.jpeg" class="contant-main-img" />

        <div style="margin-top: 20px;">
            <a class="weatherwidget-io"
               href="https://forecast7.com/he/31d0534d85/israel/"
               data-label_1="ISRAEL"
               data-label_2="WEATHER"
               data-theme="original">
               ISRAEL WEATHER
            </a>
        </div>

        <asp:DataList ID="dlCards" runat="server"
                      RepeatColumns="3"
                      CssClass="cards-container"
                      RepeatDirection="Horizontal">

            <ItemTemplate>
                <div class="info-card" onclick="navigateToURL('<%# Eval("url") %>')">
                    <img src='<%# Eval("image") %>' />
                    <p><%# Eval("text") %></p>
                </div>
            </ItemTemplate>

        </asp:DataList>

    </div>

</asp:Content>
