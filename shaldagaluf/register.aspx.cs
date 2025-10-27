using System;
using System.Web.UI;

public partial class register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string firstName = txtFirstName.Text.Trim();
        string lastName = txtLastName.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;
        string phone = txtPhone.Text.Trim();
        string id = txtID.Text.Trim();
        string genderStr = rblGender.SelectedValue;
        string cityStr = ddlOptions.SelectedValue;
        string yearofbirth = txtYearOfBirth.Text;

        // לא שדה בטופס: נדרש להוסיף ב־.aspx

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) ||
            string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) ||
            string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(id) ||
            string.IsNullOrEmpty(genderStr)  || string.IsNullOrEmpty(yearofbirth))
        {
            lblMessage.Text = "אנא מלא את כל השדות.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (password != confirmPassword)
        {
            lblMessage.Text = "הסיסמה ואימות הסיסמה אינם תואמים.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            lblMessage.Text = "אנא הכנס כתובת אימייל תקינה.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        int gender, city, yearOfBirth;
        if (!int.TryParse(genderStr, out gender) ||
            !int.TryParse(cityStr, out city) ||
            !int.TryParse(yearofbirth, out yearOfBirth))
        {
            lblMessage.Text = "וודא שציינת מספרים תקינים עבור מין, עיר ושנת לידה.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        User user = new User
        {
            Username = username,
            Firstname = firstName,
            Lastname = lastName,
            Email = email,
            Password = password,
            Gender = gender,
            YearOfBirth = yearOfBirth,
            UserId = id,
            PhoneNum = phone,
            City = city
        };

        user.insertintodb();

        lblMessage.Text = "הרישום בוצע בהצלחה!";
        lblMessage.ForeColor = System.Drawing.Color.Green;
    }
}
