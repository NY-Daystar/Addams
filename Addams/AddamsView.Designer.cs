namespace Addams
{
    partial class AddamsView
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddamsView));
            titleLabel = new Label();
            versionLabel = new Label();
            pictureBox1 = new PictureBox();
            logRt = new RichTextBox();
            ThemeSwitch = new Addams.Components.Switch();
            tabPage3 = new TabPage();
            label5 = new Label();
            DebugSwitch = new Addams.Components.Switch();
            tryAuthenticateButton = new Button();
            languageCb = new ComboBox();
            label4 = new Label();
            readSettingButton = new Button();
            TokenRtb = new RichTextBox();
            clientSecretTb = new TextBox();
            clientIdTb = new TextBox();
            userTb = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            saveSettingsButton = new Button();
            tabPage2 = new TabPage();
            openFolderButton = new Button();
            listView1 = new ListView();
            name = new ColumnHeader();
            extension = new ColumnHeader();
            size = new ColumnHeader();
            modified_at = new ColumnHeader();
            tabPage1 = new TabPage();
            listView2 = new ListView();
            columnHeader1 = new ColumnHeader();
            exportButton = new Button();
            tabControl1 = new TabControl();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 22F);
            titleLabel.Location = new Point(503, 19);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(116, 41);
            titleLabel.TabIndex = 1;
            titleLabel.Text = "<Title>";
            // 
            // versionLabel
            // 
            versionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            versionLabel.AutoSize = true;
            versionLabel.Location = new Point(960, 27);
            versionLabel.Name = "versionLabel";
            versionLabel.Size = new Size(93, 20);
            versionLabel.TabIndex = 2;
            versionLabel.Text = "Version X.Y.Z";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(456, 19);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(35, 40);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // logRt
            // 
            logRt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logRt.BackColor = Color.PeachPuff;
            logRt.Location = new Point(24, 407);
            logRt.Name = "logRt";
            logRt.ReadOnly = true;
            logRt.Size = new Size(1019, 130);
            logRt.TabIndex = 4;
            logRt.Text = "";
            // 
            // ThemeSwitch
            // 
            ThemeSwitch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ThemeSwitch.Appearance = Appearance.Button;
            ThemeSwitch.AutoSize = true;
            ThemeSwitch.BackColor = Color.LightGray;
            ThemeSwitch.FlatAppearance.BorderSize = 0;
            ThemeSwitch.FlatStyle = FlatStyle.Flat;
            ThemeSwitch.ForeColor = SystemColors.ControlText;
            ThemeSwitch.Location = new Point(975, 50);
            ThemeSwitch.MinimumSize = new Size(30, 15);
            ThemeSwitch.Name = "ThemeSwitch";
            ThemeSwitch.Size = new Size(68, 30);
            ThemeSwitch.TabIndex = 5;
            ThemeSwitch.Text = "switch1";
            ThemeSwitch.UseVisualStyleBackColor = false;
            ThemeSwitch.Click += ThemeSwitch_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label5);
            tabPage3.Controls.Add(DebugSwitch);
            tabPage3.Controls.Add(tryAuthenticateButton);
            tabPage3.Controls.Add(languageCb);
            tabPage3.Controls.Add(label4);
            tabPage3.Controls.Add(readSettingButton);
            tabPage3.Controls.Add(TokenRtb);
            tabPage3.Controls.Add(clientSecretTb);
            tabPage3.Controls.Add(clientIdTb);
            tabPage3.Controls.Add(userTb);
            tabPage3.Controls.Add(label3);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(label1);
            tabPage3.Controls.Add(saveSettingsButton);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1011, 291);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Settings";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(26, 230);
            label5.Name = "label5";
            label5.Size = new Size(97, 20);
            label5.TabIndex = 14;
            label5.Text = "Debug mode";
            // 
            // DebugSwitch
            // 
            DebugSwitch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DebugSwitch.Appearance = Appearance.Button;
            DebugSwitch.AutoSize = true;
            DebugSwitch.BackColor = Color.LightGray;
            DebugSwitch.FlatAppearance.BorderSize = 0;
            DebugSwitch.FlatStyle = FlatStyle.Flat;
            DebugSwitch.ForeColor = SystemColors.HotTrack;
            DebugSwitch.Location = new Point(124, 225);
            DebugSwitch.MinimumSize = new Size(30, 15);
            DebugSwitch.Name = "DebugSwitch";
            DebugSwitch.Size = new Size(68, 30);
            DebugSwitch.TabIndex = 13;
            DebugSwitch.Text = "switch1";
            DebugSwitch.UseVisualStyleBackColor = false;
            // 
            // tryAuthenticateButton
            // 
            tryAuthenticateButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            tryAuthenticateButton.Location = new Point(383, 258);
            tryAuthenticateButton.Name = "tryAuthenticateButton";
            tryAuthenticateButton.Size = new Size(125, 27);
            tryAuthenticateButton.TabIndex = 12;
            tryAuthenticateButton.Text = "Authenticate";
            tryAuthenticateButton.UseVisualStyleBackColor = true;
            // 
            // languageCb
            // 
            languageCb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            languageCb.FormattingEnabled = true;
            languageCb.Location = new Point(124, 177);
            languageCb.Name = "languageCb";
            languageCb.Size = new Size(147, 28);
            languageCb.TabIndex = 11;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(26, 180);
            label4.Name = "label4";
            label4.Size = new Size(74, 20);
            label4.TabIndex = 10;
            label4.Text = "Language";
            // 
            // readSettingButton
            // 
            readSettingButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            readSettingButton.Location = new Point(647, 258);
            readSettingButton.Name = "readSettingButton";
            readSettingButton.Size = new Size(116, 27);
            readSettingButton.TabIndex = 9;
            readSettingButton.Text = "Read Settings";
            readSettingButton.UseVisualStyleBackColor = true;
            // 
            // TokenRtb
            // 
            TokenRtb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TokenRtb.Location = new Point(446, 22);
            TokenRtb.Name = "TokenRtb";
            TokenRtb.Size = new Size(544, 201);
            TokenRtb.TabIndex = 8;
            TokenRtb.Text = "";
            // 
            // clientSecretTb
            // 
            clientSecretTb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            clientSecretTb.Location = new Point(124, 123);
            clientSecretTb.Name = "clientSecretTb";
            clientSecretTb.Size = new Size(292, 27);
            clientSecretTb.TabIndex = 4;
            // 
            // clientIdTb
            // 
            clientIdTb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            clientIdTb.Location = new Point(124, 78);
            clientIdTb.Name = "clientIdTb";
            clientIdTb.Size = new Size(292, 27);
            clientIdTb.TabIndex = 3;
            // 
            // userTb
            // 
            userTb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            userTb.Location = new Point(124, 35);
            userTb.Name = "userTb";
            userTb.Size = new Size(292, 27);
            userTb.TabIndex = 2;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(26, 126);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 7;
            label3.Text = "ClientSecret";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(26, 81);
            label2.Name = "label2";
            label2.Size = new Size(60, 20);
            label2.TabIndex = 6;
            label2.Text = "ClientId";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(26, 39);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 5;
            label1.Text = "Username";
            // 
            // saveSettingsButton
            // 
            saveSettingsButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            saveSettingsButton.Location = new Point(146, 261);
            saveSettingsButton.Name = "saveSettingsButton";
            saveSettingsButton.Size = new Size(125, 27);
            saveSettingsButton.TabIndex = 1;
            saveSettingsButton.Text = "Save Settings";
            saveSettingsButton.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(openFolderButton);
            tabPage2.Controls.Add(listView1);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1011, 291);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Show playlists";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // openFolderButton
            // 
            openFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            openFolderButton.Location = new Point(424, 234);
            openFolderButton.Name = "openFolderButton";
            openFolderButton.Size = new Size(124, 38);
            openFolderButton.TabIndex = 1;
            openFolderButton.Text = "Open folder";
            openFolderButton.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listView1.Columns.AddRange(new ColumnHeader[] { name, extension, size, modified_at });
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new Point(45, 39);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(929, 189);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // name
            // 
            name.Text = "Nam";
            name.Width = 400;
            // 
            // extension
            // 
            extension.Text = "Extension";
            extension.Width = 150;
            // 
            // size
            // 
            size.Text = "Size";
            size.Width = 150;
            // 
            // modified_at
            // 
            modified_at.Text = "Modified at";
            modified_at.Width = 220;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(listView2);
            tabPage1.Controls.Add(exportButton);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1011, 291);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Export playlists";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            listView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listView2.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            listView2.FullRowSelect = true;
            listView2.GridLines = true;
            listView2.Location = new Point(27, 6);
            listView2.Name = "listView2";
            listView2.Size = new Size(945, 211);
            listView2.TabIndex = 1;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = View.Tile;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 800;
            // 
            // exportButton
            // 
            exportButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            exportButton.Location = new Point(418, 233);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(184, 38);
            exportButton.TabIndex = 0;
            exportButton.Text = "Export playlists";
            exportButton.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(24, 77);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1019, 324);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            // 
            // AddamsView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1089, 549);
            Controls.Add(ThemeSwitch);
            Controls.Add(logRt);
            Controls.Add(pictureBox1);
            Controls.Add(versionLabel);
            Controls.Add(titleLabel);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "AddamsView";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label titleLabel;
        private Label versionLabel;
        private PictureBox pictureBox1;
        private RichTextBox logRt;
        public Components.Switch ThemeSwitch;
        private TabPage tabPage3;
        private ComboBox languageCb;
        private Label label4;
        private Button readSettingButton;
        private RichTextBox TokenRtb;
        private TextBox clientSecretTb;
        private TextBox clientIdTb;
        private TextBox userTb;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button saveSettingsButton;
        private TabPage tabPage2;
        private Button openFolderButton;
        private ListView listView1;
        private ColumnHeader name;
        private ColumnHeader extension;
        private ColumnHeader size;
        private ColumnHeader modified_at;
        private TabPage tabPage1;
        private ListView listView2;
        private ColumnHeader columnHeader1;
        private Button exportButton;
        public TabControl tabControl1;
        private Button tryAuthenticateButton;
        private Label label5;
        private Components.Switch DebugSwitch;
    }
}
