using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using LostAndFound.Models;
using LostAndFound.BL;

namespace LostAndFound.UI
{
    public class ProfileForm : Form
    {
        private Label lblUsername, lblLostCount, lblFoundCount;
        private TextBox txtEmail, txtPhone, txtNewPassword, txtConfirmPassword;
        private DataGridView dgvMyItems;

        public ProfileForm()
        {
            SetupUI();
            LoadProfileData();
        }

        private void SetupUI()
        {
            this.Text = "My Profile";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Controls.Add(new Label
            {
                Text = "My Profile",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                AutoSize = false,
                Size = new Size(700, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(50, 15)
            });

            this.Controls.Add(new Label { Text = "Username:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 70), AutoSize = true });
            lblUsername = new Label
            {
                Text = SessionManager.CurrentUser.Username,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(25, 118, 210),
                Location = new Point(50, 95),
                AutoSize = true
            };
            this.Controls.Add(lblUsername);

            this.Controls.Add(new Label { Text = "Email:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 130), AutoSize = true });
            txtEmail = new TextBox { Size = new Size(300, 25), Location = new Point(50, 155), Font = new Font("Segoe UI", 10), Text = SessionManager.CurrentUser.Email };
            this.Controls.Add(txtEmail);

            this.Controls.Add(new Label { Text = "Phone:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(400, 130), AutoSize = true });
            txtPhone = new TextBox { Size = new Size(300, 25), Location = new Point(400, 155), Font = new Font("Segoe UI", 10), Text = SessionManager.CurrentUser.PhoneNumber };
            this.Controls.Add(txtPhone);

            this.Controls.Add(new Label { Text = "New Password (leave blank to keep current):", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(50, 195), AutoSize = true });
            txtNewPassword = new TextBox { Size = new Size(300, 25), Location = new Point(50, 220), Font = new Font("Segoe UI", 10), PasswordChar = '*' };
            this.Controls.Add(txtNewPassword);

            this.Controls.Add(new Label { Text = "Confirm Password:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(400, 195), AutoSize = true });
            txtConfirmPassword = new TextBox { Size = new Size(300, 25), Location = new Point(400, 220), Font = new Font("Segoe UI", 10), PasswordChar = '*' };
            this.Controls.Add(txtConfirmPassword);

            Button btnUpdate = new Button
            {
                Text = "Update Profile",
                Size = new Size(150, 40),
                Location = new Point(50, 270),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Click += (s, e) =>
            {
                string newPassword = string.IsNullOrWhiteSpace(txtNewPassword.Text) ? SessionManager.CurrentUser.Password : txtNewPassword.Text;
                string confirmPassword = string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ? SessionManager.CurrentUser.Password : txtConfirmPassword.Text;

                string result = UserBL.UpdateUserProfile(SessionManager.CurrentUser.UserId, newPassword, confirmPassword, txtEmail.Text.Trim(), txtPhone.Text.Trim());

                if (result == "SUCCESS")
                {
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SessionManager.CurrentUser.Email = txtEmail.Text.Trim();
                    SessionManager.CurrentUser.PhoneNumber = txtPhone.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                        SessionManager.CurrentUser.Password = newPassword;
                    txtNewPassword.Clear();
                    txtConfirmPassword.Clear();
                }
                else
                {
                    MessageBox.Show(result, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Controls.Add(btnUpdate);

            Button btnClose = new Button
            {
                Text = "Close",
                Size = new Size(120, 35),
                Location = new Point(630, 270),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            this.Controls.Add(new Label { Text = "My Statistics", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(50, 330), AutoSize = true });

            lblLostCount = new Label { Font = new Font("Segoe UI", 10), Location = new Point(50, 360), AutoSize = true };
            this.Controls.Add(lblLostCount);

            lblFoundCount = new Label { Font = new Font("Segoe UI", 10), Location = new Point(250, 360), AutoSize = true };
            this.Controls.Add(lblFoundCount);

            this.Controls.Add(new Label { Text = "My Posted Items", Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(50, 395), AutoSize = true });

            dgvMyItems = new DataGridView
            {
                Size = new Size(700, 200),
                Location = new Point(50, 425),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvMyItems);
        }

        private void LoadProfileData()
        {
            List<Item> myItems = ItemBL.GetUserItems(SessionManager.CurrentUser.UserId);
            dgvMyItems.DataSource = myItems;

            if (dgvMyItems.Columns.Contains("OwnerId")) dgvMyItems.Columns["OwnerId"].Visible = false;
            if (dgvMyItems.Columns.Contains("ImagePath")) dgvMyItems.Columns["ImagePath"].Visible = false;
            if (dgvMyItems.Columns.Contains("OwnerUsername")) dgvMyItems.Columns["OwnerUsername"].Visible = false;

            Dictionary<string, int> stats = ItemBL.GetUserItemStats(SessionManager.CurrentUser.UserId);
            lblLostCount.Text = $"Lost Items: {stats["Lost"]}";
            lblFoundCount.Text = $"Found Items: {stats["Found"]}";
        }
    }
}