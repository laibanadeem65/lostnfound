using System;
using System.Drawing;
using System.Windows.Forms;
using LostAndFound.Models;
using LostAndFound.BL;

namespace LostAndFound.UI
{
    public class ItemEditForm : Form
    {
        private Item item;
        private TextBox txtItemName, txtDescription, txtCategory, txtLocation;
        private ComboBox cmbItemType, cmbStatus;

        public ItemEditForm(Item itemToEdit)
        {
            this.item = itemToEdit;
            SetupUI();
            LoadItemData();
        }

        private void SetupUI()
        {
            this.Text = "Edit Item";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.Controls.Add(new Label
            {
                Text = "Edit Item Details",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 152, 0),
                AutoSize = false,
                Size = new Size(450, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(25, 20)
            });

            // Item Name
            this.Controls.Add(new Label
            {
                Text = "Item Name:*",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 80),
                AutoSize = true
            });

            txtItemName = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 105),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtItemName);

            // Item Type
            this.Controls.Add(new Label
            {
                Text = "Item Type:*",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 145),
                AutoSize = true
            });

            cmbItemType = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(30, 170),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbItemType.Items.AddRange(new string[] { "Lost", "Found" });
            this.Controls.Add(cmbItemType);

            // Status
            this.Controls.Add(new Label
            {
                Text = "Status:*",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(250, 145),
                AutoSize = true
            });

            cmbStatus = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(250, 170),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new string[] { "Active", "Resolved", "Closed" });
            this.Controls.Add(cmbStatus);

            // Category
            this.Controls.Add(new Label
            {
                Text = "Category:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 210),
                AutoSize = true
            });

            txtCategory = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 235),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtCategory);

            // Location
            this.Controls.Add(new Label
            {
                Text = "Location:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 275),
                AutoSize = true
            });

            txtLocation = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 300),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtLocation);

            // Description
            this.Controls.Add(new Label
            {
                Text = "Description:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 340),
                AutoSize = true
            });

            txtDescription = new TextBox
            {
                Size = new Size(420, 80),
                Location = new Point(30, 365),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtDescription);

            // Save Button
            Button btnSave = new Button
            {
                Text = "Save Changes",
                Size = new Size(200, 45),
                Location = new Point(30, 470),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // Cancel Button
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(200, 45),
                Location = new Point(250, 470),
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

        private void LoadItemData()
        {
            txtItemName.Text = item.ItemName;
            txtDescription.Text = item.Description;
            txtCategory.Text = item.Category;
            txtLocation.Text = item.Location;
            cmbItemType.SelectedItem = item.ItemType;
            cmbStatus.SelectedItem = item.Status;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string itemName = txtItemName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string category = txtCategory.Text.Trim();
            string itemType = cmbItemType.SelectedItem?.ToString();
            string status = cmbStatus.SelectedItem?.ToString();
            string location = txtLocation.Text.Trim();

            string result = ItemBL.UpdateItem(
                SessionManager.CurrentUser,
                item.ItemId,
                itemName,
                description,
                category,
                itemType,
                location,
                status,
                item.OwnerId,
                item.ImagePath
            );

            if (result == "SUCCESS")
            {
                MessageBox.Show("Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}