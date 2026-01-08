using System;
using System.Drawing;
using System.Windows.Forms;
using LostAndFound.BL;

namespace LostAndFound.UI
{
    public class ReportItemForm : Form
    {
        private TextBox txtItemName, txtDescription, txtCategory, txtLocation;
        private ComboBox cmbItemType;

        public ReportItemForm()
        {
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Report Lost/Found Item";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Controls.Add(new Label
            {
                Text = "Report a Lost or Found Item",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(67, 160, 71),
                AutoSize = false,
                Size = new Size(450, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(25, 20)
            });

            this.Controls.Add(new Label { Text = "Item Name:*", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(30, 80), AutoSize = true });
            txtItemName = new TextBox { Size = new Size(420, 30), Location = new Point(30, 105), Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtItemName);

            this.Controls.Add(new Label { Text = "Item Type:*", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(30, 145), AutoSize = true });
            cmbItemType = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(30, 170),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbItemType.Items.AddRange(new string[] { "Lost", "Found" });
            cmbItemType.SelectedIndex = 0;
            this.Controls.Add(cmbItemType);

            this.Controls.Add(new Label { Text = "Category:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(30, 210), AutoSize = true });
            txtCategory = new TextBox { Size = new Size(420, 30), Location = new Point(30, 235), Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtCategory);

            this.Controls.Add(new Label { Text = "Location:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(30, 275), AutoSize = true });
            txtLocation = new TextBox { Size = new Size(420, 30), Location = new Point(30, 300), Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtLocation);

            this.Controls.Add(new Label { Text = "Description:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(30, 340), AutoSize = true });
            txtDescription = new TextBox { Size = new Size(420, 70), Location = new Point(30, 365), Font = new Font("Segoe UI", 10), Multiline = true };
            this.Controls.Add(txtDescription);

            Button btnSubmit = new Button
            {
                Text = "Submit Report",
                Size = new Size(200, 45),
                Location = new Point(30, 450),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += (s, e) =>
            {
                string result = ItemBL.AddItem(txtItemName.Text.Trim(), txtDescription.Text.Trim(), txtCategory.Text.Trim(),
                    cmbItemType.SelectedItem?.ToString(), txtLocation.Text.Trim(), SessionManager.CurrentUser.UserId, null);

                if (result == "SUCCESS")
                {
                    MessageBox.Show("Item reported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this.Controls.Add(btnSubmit);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(200, 45),
                Location = new Point(250, 450),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }
    }
}