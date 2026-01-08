using System;
using System.Drawing;
using System.Windows.Forms;

namespace LostAndFound.UI
{
    public class LoginSelectionForm : Form
    {
        public LoginSelectionForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            // ===== Form Settings =====
            this.Text = "Lost and Found - Welcome";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.AutoScaleMode = AutoScaleMode.Font;

            // ===== Title =====
            Label lblTitle = new Label
            {
                Text = "Welcome to Lost and Found",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 118, 210),
                AutoSize = false,
                Size = new Size(800, 70),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 60)
            };
            this.Controls.Add(lblTitle);

            // ===== Subtitle =====
            Label lblSubtitle = new Label
            {
                Text = "Find what's lost, Return what's found",
                Font = new Font("Segoe UI", 14, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(800, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 140)
            };
            this.Controls.Add(lblSubtitle);

            // ===== Sign In Button =====
            Button btnSignIn = new Button
            {
                Text = "Sign In",
                Size = new Size(320, 65),
                Location = new Point(290, 230),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(25, 118, 210),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.Click += (s, e) =>
            {
                new SignInForm().Show();
                this.Hide();
            };
            this.Controls.Add(btnSignIn);

            // ===== Sign Up Button =====
            Button btnSignUp = new Button
            {
                Text = "Sign Up",
                Size = new Size(320, 65),
                Location = new Point(290, 310),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSignUp.FlatAppearance.BorderSize = 0;
            btnSignUp.Click += (s, e) =>
            {
                new SignUpForm().Show();
                this.Hide();
            };
            this.Controls.Add(btnSignUp);

            // ===== Exit Button =====
            Button btnExit = new Button
            {
                Text = "Exit",
                Size = new Size(320, 55),
                Location = new Point(290, 395),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);
        }
    }
}
