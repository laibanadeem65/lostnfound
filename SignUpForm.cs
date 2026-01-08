using System;
using System.Drawing;
using System.Windows.Forms;
using LostAndFound.BL;

namespace LostAndFound.UI
{
    public class SignUpForm : Form
    {
        private TextBox txtUsername, txtPassword, txtConfirmPassword, txtEmail, txtPhone;

        public SignUpForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Sign Up - Create Account";
            this.Size = new Size(500, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Controls.Add(new Label
            {
                Text = "Create New Account",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 118, 210),
                AutoSize = false,
                Size = new Size(400, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 20)
            });

            this.Controls.Add(new Label { Text = "Username:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 80), AutoSize = true });
            txtUsername = new TextBox { Size = new Size(380, 30), Location = new Point(50, 105), Font = new Font("Segoe UI", 11) };
            this.Controls.Add(txtUsername);

            this.Controls.Add(new Label { Text = "Password:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 150), AutoSize = true });
            txtPassword = new TextBox { Size = new Size(380, 30), Location = new Point(50, 175), Font = new Font("Segoe UI", 11), PasswordChar = '*' };
            this.Controls.Add(txtPassword);

            this.Controls.Add(new Label { Text = "Confirm Password:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 220), AutoSize = true });
            txtConfirmPassword = new TextBox { Size = new Size(380, 30), Location = new Point(50, 245), Font = new Font("Segoe UI", 11), PasswordChar = '*' };
            this.Controls.Add(txtConfirmPassword);

            this.Controls.Add(new Label { Text = "Email:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 290), AutoSize = true });
            txtEmail = new TextBox { Size = new Size(380, 30), Location = new Point(50, 315), Font = new Font("Segoe UI", 11) };
            this.Controls.Add(txtEmail);

            this.Controls.Add(new Label { Text = "Phone Number (Optional):", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 360), AutoSize = true });
            txtPhone = new TextBox { Size = new Size(380, 30), Location = new Point(50, 385), Font = new Font("Segoe UI", 11) };
            this.Controls.Add(txtPhone);

            Button btnSignUp = new Button
            {
                Text = "Sign Up",
                Size = new Size(180, 45),
                Location = new Point(50, 450),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.Click += BtnSignUp_Click;
            this.Controls.Add(btnSignUp);

            Button btnBack = new Button
            {
                Text = "Back to Login",
                Size = new Size(180, 45),
                Location = new Point(250, 450),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += (s, e) => { new LoginSelectionForm().Show(); this.Close(); };
            this.Controls.Add(btnBack);
        }

        private void BtnSignUp_Click(object sender, EventArgs e)
        {
            string result = UserBL.RegisterUser(txtUsername.Text.Trim(), txtPassword.Text, txtConfirmPassword.Text, txtEmail.Text.Trim(), txtPhone.Text.Trim());
            if (result == "SUCCESS")
            {
                MessageBox.Show("Registration successful! You can now sign in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new SignInForm().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(result, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}