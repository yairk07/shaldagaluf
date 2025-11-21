## Shaldagaluf – פורטל ניהול אירועים ASP.NET

יישום WebForms (‎.NET Framework 4.7.2‎) המאפשר ניהול אירועים, משתמשים ומשימות עבור קהילת "ש״ל דגאלוף". המערכת כוללת רישום והתחברות, חיווי אירועים בלוח שנה, תצוגות אדמין, וטפסי תקשורת.

### יכולות עיקריות
- **ניהול משתמשים**: רישום (`register.aspx`), התחברות (`login.aspx`) וטבלת משתמשים מנהלתית (`exusers.aspx`, `exuserdetails.aspx`) שנשענות על `UsersService`.
- **יומן ואירועים**: דף הבית מציג לוח שנה אינטראקטיבי, רשימת אירועים אישיים ומשחקון ״רולטה״; שירות `EventService` מספק איסוף אירועים מטבלת `calnder`.
- **משימות ועמודי תפעול**: דפי `tasks.aspx`, `allEvents.aspx` ו-`editEvent.aspx` מאפשרים צפייה ותחזוקת אירועים ומשימות.
- **תוכן שיווקי ותקשורת**: עמודי `contactus.aspx`, `contant.aspx`, `termofservice.aspx` מספקים מידע סטטי וטפסי יצירת קשר.
- **מיתוג ונכסי UI**: הקבצים `StyleSheet.css`, `JavaScript.js` ותיקיית `pics/` מטפלים במראה, באנימציות ובגלריות.

### מבנה הפרויקט
```
project aluf/
├── shaldagaluf.sln                  # פתרון VS 2019 (פורמט 12.00)
├── shaldagaluf/                     # אתר WebForms
│   ├── App_Code/                    # שכבת לוגיקה (DAL/BL)
│   │   ├── Connect.cs               # מחבר ל-Access דרך OleDb
│   │   ├── UsersService.cs          # CRUD למשתמשים + אימות
│   │   └── EventService.cs          # שליפת אירועים
│   ├── App_Data/calnder.db1.accdb.mdb  # בסיס נתונים Access
│   ├── *.aspx / *.aspx.cs           # עמודי התצוגה וקוד הבק-אנד
│   ├── danimaster.master            # Master Page מרכזי
│   ├── StyleSheet.css, JavaScript.js
│   └── pics/                        # משאבים סטטיים
└── packages/                        # חבילות NuGet משוחזרות (Roslyn)
```

### דרישות מוקדמות
- Windows עם ‎.NET Framework 4.7.2‎.
- Visual Studio 2019 (או חדש יותר שמסוגל לטעון פרויקטי WebForms).
- ‏[Access Database Engine](https://www.microsoft.com/en-us/download/details.aspx?id=13255) ‎64bit/32bit‎ – נדרש ל-`Microsoft.ACE.OLEDB.12.0`.
- Git משמש לניהול גרסאות (כבר מוגדר מרחוק אל `https://github.com/yairk07/shaldagaluf`).

### הוראות הפעלה מקומית
1. שיבט את הריפו:  
   ```bash
   git clone https://github.com/yairk07/shaldagaluf.git
   ```
2. פתח את `shaldagaluf.sln` ב-Visual Studio.
3. ודא שהפרויקט מוגדר כ-Start Project (קליק ימני → *Set as StartUp Project*).
4. בדפדפן `Web.config` אין מחרוזות ייחודיות; חיבור ה-DB נקבע ב-`Connect.GetConnectionString()` ומצביע אל `~/App_Data/calnder.db1.accdb.mdb`. ודא שהקובץ לא חסום ע"י אנטי-וירוס ושהאותנטיקציה שלך מאפשרת קריאה/כתיבה.
5. הרץ עם `F5`. Visual Studio ישיק IIS Express על הפורט שהוגדר בקובץ ‎`.sln` (ברירת מחדל ‎55820‎).

### עבודה עם בסיס הנתונים
- הקובץ `App_Data/calnder.db1.accdb.mdb` מכיל את טבלאות `Users`, `Citys`, `calnder` ועוד.
- הקוד משתמש במחברי `OleDbParameter` ולכן אין צורך בעדכון ידני של מחרוזת החיבור, אך אם מזיזים את הקובץ – יש לעדכן את הקבוע `calnder` ב-`Connect.cs`.
- לגיבוי/שחזור: סגור את IIS Express ו-Visual Studio, העתק את הקובץ, החזר במקרה הצורך והרץ שוב.

### שחזור חבילות
הפרויקט כולל `packages.config` ותקיית `packages/`. בפתיחה ב-VS, NuGet ישחזר אוטומטית את `Microsoft.CodeDom.Providers.DotNetCompilerPlatform`. אם חסרה, הרץ:
```powershell
Update-Package -reinstall
```

### שיפורים מוצעים
- הוספת ‎`.gitignore`‎ כדי למנוע גרסאות של `Bin/` ו-`packages/`.
- רענון UI ומיתוג אחיד (למשל העברת מחרוזות קשיחות לקובץ משאבים).
- לוג רישום והקשחת אימות (hash לסיסמאות, reCAPTCHA).
- בדיקות יחידה עבור `UsersService` ו-`EventService` באמצעות שכבת DAL מופשטת.

---
לשאלות או הפניות נוספות אפשר ליצור קשר דרך `contactus.aspx` או לפתוח Issue במאגר.

