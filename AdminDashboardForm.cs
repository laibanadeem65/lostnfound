using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using LostAndFound.Models;
using LostAndFound.BL;

namespace LostAndFound.UI
{
    public class AdminDashboardForm : Form
    {
        private TabControl tabControl;
        private DataGridView dgvItems, dgvUsers;

        public AdminDashboardForm()
        {
            SetupUI();
            LoadData();
        }

        private void SetupUI()
        {
            this.Text = "Admin Dashboard - Lost and Found";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);

            this.Controls.Add(new Label
            {
                Text = "Admin Dashboard - Welcome, " + SessionManager.CurrentUser.Username,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(213, 0, 0),
                AutoSize = true,
                Location = new Point(20, 15)
            });

            tabControl = new TabControl
            {
                Size = new Size(1040, 520),
                Location = new Point(20, 60),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(tabControl);

            // Items Tab
            TabPage itemsTab = new TabPage("Manage Items");

            dgvItems = new DataGridView
            {
                Size = new Size(1020, 420),
                Location = new Point(10, 50),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            Button btnEditItem = new Button
            {
                Text = "Edit Selected Item",
                Size = new Size(150, 35),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEditItem.FlatAppearance.BorderSize = 0;
            btnEditItem.Click += (s, e) =>
            {
                if (dgvItems.SelectedRows.Count > 0)
                {
                    Item selectedItem = (Item)dgvItems.SelectedRows[0].DataBoundItem;
                    new ItemEditForm(selectedItem).ShowDialog();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Please select an item to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            Button btnDeleteItem = new Button
            {
                Text = "Delete Selected Item",
                Size = new Size(150, 35),
                Location = new Point(170, 10),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDeleteItem.FlatAppearance.BorderSize = 0;
            btnDeleteItem.Click += (s, e) =>
            {
                if (dgvItems.SelectedRows.Count > 0)
                {
                    Item selectedItem = (Item)dgvItems.SelectedRows[0].DataBoundItem;
                    if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string result = ItemBL.DeleteItem(SessionManager.CurrentUser, selectedItem.ItemId, selectedItem.OwnerId);
                        if (result == "SUCCESS")
                        {
                            MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select an item to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            itemsTab.Controls.Add(btnEditItem);
            itemsTab.Controls.Add(btnDeleteItem);
            itemsTab.Controls.Add(dgvItems);
            tabControl.TabPages.Add(itemsTab);

            // Users Tab
            TabPage usersTab = new TabPage("Manage Users");

            dgvUsers = new DataGridView
            {
                Size = new Size(1020, 420),
                Location = new Point(10, 50),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            Button btnToggleRole = new Button
            {
                Text = "Toggle Admin Role",
                Size = new Size(150, 35),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(156, 39, 176),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnToggleRole.FlatAppearance.BorderSize = 0;
            btnToggleRole.Click += (s, e) =>
            {
                if (dgvUsers.SelectedRows.Count > 0)
                {
                    User selectedUser = (User)dgvUsers.SelectedRows[0].DataBoundItem;
                    bool newRole = !selectedUser.IsAdmin;
                    string roleText = newRole ? "Admin" : "Regular User";

                    if (MessageBox.Show($"Change {selectedUser.Username} to {roleText}?", "Confirm Role Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string result = UserBL.ChangeUserRole(SessionManager.CurrentUser, selectedUser.UserId, newRole);
                        if (result == "SUCCESS")
                        {
                            MessageBox.Show("Role changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a user.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            usersTab.Controls.Add(btnToggleRole);
            usersTab.Controls.Add(dgvUsers);
            tabControl.TabPages.Add(usersTab);

            Button btnLogout = new Button
            {
                Text = "Logout",
                Size = new Size(120, 40),
                Location = new Point(940, 600),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => { SessionManager.Logout(); new LoginSelectionForm().Show(); this.Close(); };
            this.Controls.Add(btnLogout);
        }

        private void LoadData()
        {
            dgvItems.DataSource = ItemBL.GetAllItems();
            if (dgvItems.Columns.Contains("ImagePath")) dgvItems.Columns["ImagePath"].Visible = false;

            dgvUsers.DataSource = UserBL.GetAllUsers(SessionManager.CurrentUser);
            if (dgvUsers.Columns.Contains("Password")) dgvUsers.Columns["Password"].Visible = false;
        }
    }
}