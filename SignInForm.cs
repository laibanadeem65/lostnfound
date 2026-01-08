using System;
using System.Drawing;
using System.Windows.Forms;
using LostAndFound.BL;
using LostAndFound.Models;

namespace LostAndFound.UI
{
    public class SignInForm : Form
    {
        private TextBox txtUsername, txtPassword;

        public SignInForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Sign In";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Controls.Add(new Label
            {
                Text = "Sign In to Your Account",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 118, 210),
                AutoSize = false,
                Size = new Size(380, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(35, 30)
            });

            this.Controls.Add(new Label { Text = "Username:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 100), AutoSize = true });
            txtUsername = new TextBox { Size = new Size(340, 30), Location = new Point(50, 125), Font = new Font("Segoe UI", 11) };
            this.Controls.Add(txtUsername);

            this.Controls.Add(new Label { Text = "Password:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 170), AutoSize = true });
            txtPassword = new TextBox { Size = new Size(340, 30), Location = new Point(50, 195), Font = new Font("Segoe UI", 11), PasswordChar = '*' };
            this.Controls.Add(txtPassword);

            Button btnSignIn = new Button
            {
                Text = "Sign In",
                Size = new Size(160, 45),
                Location = new Point(50, 260),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(25, 118, 210),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.Click += BtnSignIn_Click;
            this.Controls.Add(btnSignIn);

            Button btnBack = new Button
            {
                Text = "Back",
                Size = new Size(160, 45),
                Location = new Point(230, 260),
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

        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User user = UserBL.LoginUser(txtUsername.Text.Trim(), txtPassword.Text);
            if (user != null)
            {
                SessionManager.CurrentUser = user;
                MessageBox.Show("Login successful! Welcome " + user.Username, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (user.IsAdmin)
                    new AdminDashboardForm().Show();
                else
                    new UserDashboardForm().Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}