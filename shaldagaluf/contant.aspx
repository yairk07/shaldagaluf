<%@ Page Title="" Language="C#" MasterPageFile="~/danimaster.master" AutoEventWireup="true" CodeFile="contant.aspx.cs" Inherits="Default3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- Link to the external JavaScript file -->
    <script src="javaScript.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-section" style="text-align: center; margin: 20px;">
        <h2>ברוכים הבאים</h2>
        <p>כאן תוכלו למצוא מידע נוסף, תמונות וקישורים שימושיים.</p>

        <!-- Image Section -->
        <div style="margin: 15px;">
            <img src="pics/הורדה.jpeg" alt="תמונה של הצוות" style="width: 300px; border-radius: 10px; box-shadow: 2px 2px 10px gray;" />
        </div>

        <!-- Weather Widget Integration -->
        <div style="margin-top: 20px;">
            <a class="weatherwidget-io" href="https://forecast7.com/he/31d0534d85/israel/" data-label_1="ISRAEL" data-label_2="WEATHER" data-theme="original">ISRAEL WEATHER</a>
        </div>

        <!-- Links and Info Boxes -->
        <div style="display: flex; justify-content: center; gap: 20px; margin-top: 30px;">
            
            <!-- Box 1: Homework Help -->
            <div class="info-box" style="width: 250px; padding: 15px; border: 1px solid #ccc; border-radius: 10px; box-shadow: 2px 2px 10px gray; text-align: center;">
                <img src="pics/math.jpg" alt="math" style="width: 100%; border-radius: 10px;" onclick="navigateToURL('https://tiktek.com/il/heb-index.htm')" />
                <p>נתקעתה עם שיעורי בית ? ואתה רוצה לצאת עם חברים</p>
            </div>

            <!-- Box 2: Scouts Action Reminder -->
            <div class="info-box" style="width: 250px; padding: 15px; border: 1px solid #ccc; border-radius: 10px; box-shadow: 2px 2px 10px gray; text-align: center;">
                <img src="pics/scouts.png" alt="scouts" style="width: 100%; border-radius: 10px;" onclick="navigateToURL('https://prod-hamasa.zofim.org.il/he/login/')" />
                <p>נשאר יום אחד להגיש פעולה ולא התחלתה.</p>
            </div>

            <!-- Box 3: Chat GPT -->
            <div class="info-box" style="width: 250px; padding: 15px; border: 1px solid #ccc; border-radius: 10px; box-shadow: 2px 2px 10px gray; text-align: center;">
                <img src="pics/chat.png" alt="chat" style="width: 100%; border-radius: 10px;" onclick="navigateToURL('https://chatgpt.com/')" />
                <p>כל דבר אחר</p>
            </div>
        </div>
    </div>
</asp:Content>
