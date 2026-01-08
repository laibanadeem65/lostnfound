using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using LostAndFound.Models;
using LostAndFound.BL;

namespace LostAndFound.UI
{
    public class UserDashboardForm : Form
    {
        private DataGridView dgvItems;
        private TextBox txtSearch;
        private ComboBox cmbFilter;

        public UserDashboardForm()
        {
            SetupUI();
            LoadItems();
        }

        private void SetupUI()
        {
            this.Text = "User Dashboard - Lost and Found";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);

            this.Controls.Add(new Label
            {
                Text = "Welcome, " + SessionManager.CurrentUser.Username + "!",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 118, 210),
                AutoSize = true,
                Location = new Point(20, 15)
            });

            this.Controls.Add(new Label { Text = "Search:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(20, 60), AutoSize = true });

            txtSearch = new TextBox { Size = new Size(300, 25), Location = new Point(20, 85), Font = new Font("Segoe UI", 10) };
            this.Controls.Add(txtSearch);

            Button btnSearch = new Button
            {
                Text = "Search",
                Size = new Size(100, 30),
                Location = new Point(330, 83),
                BackColor = Color.FromArgb(25, 118, 210),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += (s, e) =>
            {
                string keyword = txtSearch.Text.Trim();
                LoadItems(string.IsNullOrWhiteSpace(keyword) ? null : keyword);
            };
            this.Controls.Add(btnSearch);

            this.Controls.Add(new Label { Text = "Filter:", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(450, 60), AutoSize = true });

            cmbFilter = new ComboBox
            {
                Size = new Size(150, 25),
                Location = new Point(450, 85),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFilter.Items.AddRange(new string[] { "All", "Lost", "Found", "Active", "Resolved" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) =>
            {
                string filter = cmbFilter.SelectedItem.ToString();
                if (filter == "All")
                    LoadItems();
                else if (filter == "Lost" || filter == "Found")
                {
                    List<Item> items = ItemBL.FilterItemsByType(filter);
                    DisplayItemsWithHiddenDescription(items);
                }
                else
                {
                    List<Item> items = ItemBL.FilterItemsByStatus(filter);
                    DisplayItemsWithHiddenDescription(items);
                }
            };
            this.Controls.Add(cmbFilter);

            dgvItems = new DataGridView
            {
                Size = new Size(1040, 380),
                Location = new Point(20, 130),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvItems);

            Button btnClaimItem = new Button
            {
                Text = "Claim This Item",
                Size = new Size(180, 40),
                Location = new Point(20, 530),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClaimItem.FlatAppearance.BorderSize = 0;
            btnClaimItem.Click += BtnClaimItem_Click;
            this.Controls.Add(btnClaimItem);

            Button btnReportItem = new Button
            {
                Text = "Report Lost/Found Item",
                Size = new Size(180, 40),
                Location = new Point(220, 530),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReportItem.FlatAppearance.BorderSize = 0;
            btnReportItem.Click += (s, e) => { new ReportItemForm().ShowDialog(); LoadItems(); };
            this.Controls.Add(btnReportItem);

            Button btnProfile = new Button
            {
                Text = "My Profile",
                Size = new Size(150, 40),
                Location = new Point(420, 530),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Click += (s, e) => new ProfileForm().ShowDialog();
            this.Controls.Add(btnProfile);

            Button btnLogout = new Button
            {
                Text = "Logout",
                Size = new Size(120, 40),
                Location = new Point(940, 530),
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

        private void LoadItems(string searchKeyword = null)
        {
            List<Item> items;
            if (searchKeyword != null)
                items = ItemBL.SearchItems(searchKeyword);
            else
                items = ItemBL.GetAllItems();

            DisplayItemsWithHiddenDescription(items);
        }

        private void DisplayItemsWithHiddenDescription(List<Item> items)
        {
            foreach (Item item in items)
            {
                if (item.OwnerId != SessionManager.CurrentUser.UserId)
                {
                    item.Description = "[Hidden - Claim to view]";
                }
            }

            dgvItems.DataSource = null;
            dgvItems.DataSource = items;

            if (dgvItems.Columns.Contains("OwnerId")) dgvItems.Columns["OwnerId"].Visible = false;
            if (dgvItems.Columns.Contains("ImagePath")) dgvItems.Columns["ImagePath"].Visible = false;
        }

        private void BtnClaimItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to claim.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Item selectedItem = (Item)dgvItems.SelectedRows[0].DataBoundItem;

            if (selectedItem.OwnerId == SessionManager.CurrentUser.UserId)
            {
                MessageBox.Show("You cannot claim your own item!", "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ClaimItemDialog claimDialog = new ClaimItemDialog(selectedItem);
            claimDialog.ShowDialog();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UserDashboardForm
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "UserDashboardForm";
            this.Load += new System.EventHandler(this.UserDashboardForm_Load);
            this.ResumeLayout(false);

        }

        private void UserDashboardForm_Load(object sender, EventArgs e)
        {

        }
    }

    public class ClaimItemDialog : Form
    {
        private Item item;
        private Item originalItem;
        private TextBox txtYourDescription;
        private Label lblResult;
        private Label lblMatchInfo;
        private Panel pnlContactInfo;

        public ClaimItemDialog(Item itemToClaim)
        {
            this.item = itemToClaim;

            List<Item> allItems = ItemBL.GetAllItems();
            this.originalItem = allItems.Find(i => i.ItemId == itemToClaim.ItemId);

            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Claim Item - Verification";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.Add(new Label
            {
                Text = "Claim: " + item.ItemName,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 152, 0),
                AutoSize = false,
                Size = new Size(550, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(25, 20)
            });

            this.Controls.Add(new Label
            {
                Text = "To verify this is your item, please describe it:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = false,
                Size = new Size(550, 30),
                Location = new Point(30, 80)
            });

            Label lblHint = new Label
            {
                Text = "Hint: Describe color, brand, size, unique features, etc.",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(550, 20),
                Location = new Point(30, 105)
            };
            this.Controls.Add(lblHint);

            this.Controls.Add(new Label
            {
                Text = "Your Description:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(30, 130),
                AutoSize = true
            });

            txtYourDescription = new TextBox
            {
                Size = new Size(520, 100),
                Location = new Point(30, 155),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtYourDescription);

            Button btnVerify = new Button
            {
                Text = "Verify & Claim",
                Size = new Size(200, 45),
                Location = new Point(30, 275),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(67, 160, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.Click += BtnVerify_Click;
            this.Controls.Add(btnVerify);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(150, 45),
                Location = new Point(250, 275),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);

            lblMatchInfo = new Label
            {
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Blue,
                AutoSize = false,
                Size = new Size(520, 30),
                Location = new Point(30, 335),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };
            this.Controls.Add(lblMatchInfo);

            lblResult = new Label
            {
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(520, 30),
                Location = new Point(30, 365),
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };
            this.Controls.Add(lblResult);

            pnlContactInfo = new Panel
            {
                Size = new Size(520, 150),
                Location = new Point(30, 405),
                BackColor = Color.FromArgb(230, 247, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                AutoScroll = true
            };
            this.Controls.Add(pnlContactInfo);
        }

        private void BtnVerify_Click(object sender, EventArgs e)
        {
            string userDescription = txtYourDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(userDescription))
            {
                MessageBox.Show("Please enter a description to verify the item.", "Empty Description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (userDescription.Length < 5)
            {
                MessageBox.Show("Please provide a more detailed description (at least 5 characters).", "Too Short", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string actualDescription = originalItem.Description ?? "";
            var matchResult = FindMatchingWords(userDescription.ToLower(), actualDescription.ToLower());

            if (matchResult.Item1 > 0)
            {
                lblMatchInfo.Text = $"Match found! {matchResult.Item1} word(s) matched: {matchResult.Item2}";
                lblMatchInfo.ForeColor = Color.Green;
                lblMatchInfo.Visible = true;

                lblResult.Text = "✓ Verification Successful! Here are the contact details:";
                lblResult.ForeColor = Color.Green;
                lblResult.Visible = true;

                ShowContactInformation();
            }
            else
            {
                lblMatchInfo.Text = "No matching words found in the description.";
                lblMatchInfo.ForeColor = Color.Orange;
                lblMatchInfo.Visible = true;

                lblResult.Text = "✗ Description doesn't match. Please try again with more details.";
                lblResult.ForeColor = Color.Red;
                lblResult.Visible = true;
                pnlContactInfo.Visible = false;
            }
        }

        private void ShowContactInformation()
        {
            pnlContactInfo.Controls.Clear();

            // FIXED: Use GetUserById instead of GetAllUsers
            User owner = UserBL.GetUserById(originalItem.OwnerId);

            if (owner == null)
            {
                MessageBox.Show("Unable to retrieve owner information. Owner ID: " + originalItem.OwnerId, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int yPosition = 10;

            Label lblTitle = new Label
            {
                Text = "📞 Owner Contact Information:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 118, 210),
                AutoSize = true,
                Location = new Point(10, yPosition)
            };
            pnlContactInfo.Controls.Add(lblTitle);
            yPosition += 35;

            Label lblOwner = new Label
            {
                Text = "Owner: " + owner.Username,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, yPosition)
            };
            pnlContactInfo.Controls.Add(lblOwner);
            yPosition += 30;

            Label lblEmail = new Label
            {
                Text = "📧 Email: " + owner.Email,
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(10, yPosition)
            };
            pnlContactInfo.Controls.Add(lblEmail);
            yPosition += 25;

            if (!string.IsNullOrWhiteSpace(owner.PhoneNumber))
            {
                Label lblPhone = new Label
                {
                    Text = "📱 Phone: " + owner.PhoneNumber,
                    Font = new Font("Segoe UI", 10),
                    AutoSize = true,
                    Location = new Point(10, yPosition)
                };
                pnlContactInfo.Controls.Add(lblPhone);
                yPosition += 25;
            }

            Label lblInstructions = new Label
            {
                Text = "Please contact the owner to arrange item return.",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(480, 30),
                Location = new Point(10, yPosition)
            };
            pnlContactInfo.Controls.Add(lblInstructions);

            pnlContactInfo.Visible = true;
        }

        private Tuple<int, string> FindMatchingWords(string userDesc, string actualDesc)
        {
            if (string.IsNullOrEmpty(userDesc) || string.IsNullOrEmpty(actualDesc))
                return new Tuple<int, string>(0, "");

            string[] stopWords = { "the", "a", "an", "is", "are", "was", "were", "of", "in", "on", "at", "to", "for", "it", "its", "i", "my", "me" };

            string[] userWords = userDesc.Split(new[] { ' ', ',', '.', '!', '?', '-', '_', ':', ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] actualWords = actualDesc.Split(new[] { ' ', ',', '.', '!', '?', '-', '_', ':', ';' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> matchedWords = new List<string>();
            int matchCount = 0;

            foreach (string userWord in userWords)
            {
                if (userWord.Length <= 2)
                    continue;

                bool isStopWord = false;
                foreach (string stopWord in stopWords)
                {
                    if (userWord == stopWord)
                    {
                        isStopWord = true;
                        break;
                    }
                }
                if (isStopWord)
                    continue;

                foreach (string actualWord in actualWords)
                {
                    if (actualWord.Length <= 2)
                        continue;

                    if (userWord == actualWord || actualWord.Contains(userWord) || userWord.Contains(actualWord))
                    {
                        matchCount++;
                        if (!matchedWords.Contains(userWord))
                        {
                            matchedWords.Add(userWord);
                        }
                        break;
                    }
                }
            }

            string matchedWordsStr = matchedWords.Count > 0 ? string.Join(", ", matchedWords) : "";
            return new Tuple<int, string>(matchCount, matchedWordsStr);
        }
    }
}