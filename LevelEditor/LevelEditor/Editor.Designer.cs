namespace LevelEditor
{
    partial class Editor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllReflectionLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ObjectsPage = new System.Windows.Forms.TabPage();
            this.lineButton = new System.Windows.Forms.RadioButton();
            this.BlockButton = new System.Windows.Forms.RadioButton();
            this.SwitchButton = new System.Windows.Forms.RadioButton();
            this.SpikeButton = new System.Windows.Forms.RadioButton();
            this.DoorButton = new System.Windows.Forms.RadioButton();
            this.KeyButton = new System.Windows.Forms.RadioButton();
            this.WallButton = new System.Windows.Forms.RadioButton();
            this.PlayerButton = new System.Windows.Forms.RadioButton();
            this.PropertiesPage = new System.Windows.Forms.TabPage();
            this.orientationText = new System.Windows.Forms.ComboBox();
            this.reflectableText = new System.Windows.Forms.ComboBox();
            this.hLineText = new System.Windows.Forms.TextBox();
            this.vLineText = new System.Windows.Forms.TextBox();
            this.dLineText = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.b_ApplyProperties = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.frictionText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.restText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.densityText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pb_Level = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.ObjectsPage.SuspendLayout();
            this.PropertiesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Level)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.insertToolStripMenuItem,
            this.removeAllReflectionLinesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(808, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.newToolStripMenuItem.Text = "New Level";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.openToolStripMenuItem.Text = "Open Level...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // insertToolStripMenuItem
            // 
            this.insertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dialogToolStripMenuItem});
            this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            this.insertToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.insertToolStripMenuItem.Text = "Insert";
            // 
            // dialogToolStripMenuItem
            // 
            this.dialogToolStripMenuItem.Name = "dialogToolStripMenuItem";
            this.dialogToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.dialogToolStripMenuItem.Text = "Dialog";
            this.dialogToolStripMenuItem.Click += new System.EventHandler(this.dialogToolStripMenuItem_Click);
            // 
            // removeAllReflectionLinesToolStripMenuItem
            // 
            this.removeAllReflectionLinesToolStripMenuItem.Name = "removeAllReflectionLinesToolStripMenuItem";
            this.removeAllReflectionLinesToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
            this.removeAllReflectionLinesToolStripMenuItem.Text = "Remove All Reflection Lines";
            this.removeAllReflectionLinesToolStripMenuItem.Click += new System.EventHandler(this.removeAllReflectionLinesToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.ObjectsPage);
            this.tabControl1.Controls.Add(this.PropertiesPage);
            this.tabControl1.Location = new System.Drawing.Point(606, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 521);
            this.tabControl1.TabIndex = 3;
            // 
            // ObjectsPage
            // 
            this.ObjectsPage.Controls.Add(this.lineButton);
            this.ObjectsPage.Controls.Add(this.BlockButton);
            this.ObjectsPage.Controls.Add(this.SwitchButton);
            this.ObjectsPage.Controls.Add(this.SpikeButton);
            this.ObjectsPage.Controls.Add(this.DoorButton);
            this.ObjectsPage.Controls.Add(this.KeyButton);
            this.ObjectsPage.Controls.Add(this.WallButton);
            this.ObjectsPage.Controls.Add(this.PlayerButton);
            this.ObjectsPage.Location = new System.Drawing.Point(4, 22);
            this.ObjectsPage.Name = "ObjectsPage";
            this.ObjectsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ObjectsPage.Size = new System.Drawing.Size(192, 495);
            this.ObjectsPage.TabIndex = 0;
            this.ObjectsPage.Text = "Physics Objects";
            this.ObjectsPage.UseVisualStyleBackColor = true;
            // 
            // lineButton
            // 
            this.lineButton.AutoSize = true;
            this.lineButton.Location = new System.Drawing.Point(34, 198);
            this.lineButton.Name = "lineButton";
            this.lineButton.Size = new System.Drawing.Size(96, 17);
            this.lineButton.TabIndex = 7;
            this.lineButton.Text = "Reflection Line";
            this.lineButton.UseVisualStyleBackColor = true;
            this.lineButton.CheckedChanged += new System.EventHandler(this.lineButton_CheckedChanged);
            // 
            // BlockButton
            // 
            this.BlockButton.AutoSize = true;
            this.BlockButton.Location = new System.Drawing.Point(34, 175);
            this.BlockButton.Name = "BlockButton";
            this.BlockButton.Size = new System.Drawing.Size(85, 17);
            this.BlockButton.TabIndex = 6;
            this.BlockButton.Text = "Buddy Block";
            this.BlockButton.UseVisualStyleBackColor = true;
            this.BlockButton.CheckedChanged += new System.EventHandler(this.BlockButton_CheckedChanged);
            // 
            // SwitchButton
            // 
            this.SwitchButton.AutoSize = true;
            this.SwitchButton.Location = new System.Drawing.Point(34, 152);
            this.SwitchButton.Name = "SwitchButton";
            this.SwitchButton.Size = new System.Drawing.Size(57, 17);
            this.SwitchButton.TabIndex = 5;
            this.SwitchButton.Text = "Switch";
            this.SwitchButton.UseVisualStyleBackColor = true;
            this.SwitchButton.CheckedChanged += new System.EventHandler(this.SwitchButton_CheckedChanged);
            // 
            // SpikeButton
            // 
            this.SpikeButton.AutoSize = true;
            this.SpikeButton.Location = new System.Drawing.Point(34, 106);
            this.SpikeButton.Name = "SpikeButton";
            this.SpikeButton.Size = new System.Drawing.Size(52, 17);
            this.SpikeButton.TabIndex = 4;
            this.SpikeButton.Text = "Spike";
            this.SpikeButton.UseVisualStyleBackColor = true;
            this.SpikeButton.CheckedChanged += new System.EventHandler(this.SpikeButton_CheckedChanged);
            // 
            // DoorButton
            // 
            this.DoorButton.AutoSize = true;
            this.DoorButton.Location = new System.Drawing.Point(34, 83);
            this.DoorButton.Name = "DoorButton";
            this.DoorButton.Size = new System.Drawing.Size(48, 17);
            this.DoorButton.TabIndex = 3;
            this.DoorButton.Text = "Door";
            this.DoorButton.UseVisualStyleBackColor = true;
            this.DoorButton.CheckedChanged += new System.EventHandler(this.DoorButton_CheckedChanged);
            // 
            // KeyButton
            // 
            this.KeyButton.AutoSize = true;
            this.KeyButton.Location = new System.Drawing.Point(34, 129);
            this.KeyButton.Name = "KeyButton";
            this.KeyButton.Size = new System.Drawing.Size(43, 17);
            this.KeyButton.TabIndex = 2;
            this.KeyButton.Text = "Key";
            this.KeyButton.UseVisualStyleBackColor = true;
            this.KeyButton.CheckedChanged += new System.EventHandler(this.KeyButton_CheckedChanged);
            // 
            // WallButton
            // 
            this.WallButton.AutoSize = true;
            this.WallButton.Location = new System.Drawing.Point(34, 60);
            this.WallButton.Name = "WallButton";
            this.WallButton.Size = new System.Drawing.Size(46, 17);
            this.WallButton.TabIndex = 1;
            this.WallButton.Text = "Wall";
            this.WallButton.UseVisualStyleBackColor = true;
            this.WallButton.CheckedChanged += new System.EventHandler(this.WallButton_CheckedChanged);
            // 
            // PlayerButton
            // 
            this.PlayerButton.AutoSize = true;
            this.PlayerButton.Checked = true;
            this.PlayerButton.Location = new System.Drawing.Point(34, 36);
            this.PlayerButton.Name = "PlayerButton";
            this.PlayerButton.Size = new System.Drawing.Size(54, 17);
            this.PlayerButton.TabIndex = 0;
            this.PlayerButton.TabStop = true;
            this.PlayerButton.Text = "Player";
            this.PlayerButton.UseVisualStyleBackColor = true;
            this.PlayerButton.CheckedChanged += new System.EventHandler(this.PlayerButton_CheckedChanged);
            // 
            // PropertiesPage
            // 
            this.PropertiesPage.Controls.Add(this.orientationText);
            this.PropertiesPage.Controls.Add(this.reflectableText);
            this.PropertiesPage.Controls.Add(this.hLineText);
            this.PropertiesPage.Controls.Add(this.vLineText);
            this.PropertiesPage.Controls.Add(this.dLineText);
            this.PropertiesPage.Controls.Add(this.label8);
            this.PropertiesPage.Controls.Add(this.label2);
            this.PropertiesPage.Controls.Add(this.label1);
            this.PropertiesPage.Controls.Add(this.b_ApplyProperties);
            this.PropertiesPage.Controls.Add(this.label7);
            this.PropertiesPage.Controls.Add(this.label6);
            this.PropertiesPage.Controls.Add(this.frictionText);
            this.PropertiesPage.Controls.Add(this.label5);
            this.PropertiesPage.Controls.Add(this.restText);
            this.PropertiesPage.Controls.Add(this.label4);
            this.PropertiesPage.Controls.Add(this.densityText);
            this.PropertiesPage.Controls.Add(this.label3);
            this.PropertiesPage.Location = new System.Drawing.Point(4, 22);
            this.PropertiesPage.Name = "PropertiesPage";
            this.PropertiesPage.Padding = new System.Windows.Forms.Padding(3);
            this.PropertiesPage.Size = new System.Drawing.Size(192, 492);
            this.PropertiesPage.TabIndex = 1;
            this.PropertiesPage.Text = "Properties";
            this.PropertiesPage.UseVisualStyleBackColor = true;
            // 
            // orientationText
            // 
            this.orientationText.FormattingEnabled = true;
            this.orientationText.Items.AddRange(new object[] {
            "U",
            "D",
            "L",
            "R",
            "H",
            "V",
            "D"});
            this.orientationText.Location = new System.Drawing.Point(127, 170);
            this.orientationText.Name = "orientationText";
            this.orientationText.Size = new System.Drawing.Size(47, 21);
            this.orientationText.TabIndex = 22;
            // 
            // reflectableText
            // 
            this.reflectableText.FormattingEnabled = true;
            this.reflectableText.Items.AddRange(new object[] {
            "True",
            "False"});
            this.reflectableText.Location = new System.Drawing.Point(127, 145);
            this.reflectableText.Name = "reflectableText";
            this.reflectableText.Size = new System.Drawing.Size(47, 21);
            this.reflectableText.TabIndex = 21;
            // 
            // hLineText
            // 
            this.hLineText.Location = new System.Drawing.Point(127, 201);
            this.hLineText.Name = "hLineText";
            this.hLineText.Size = new System.Drawing.Size(47, 20);
            this.hLineText.TabIndex = 20;
            // 
            // vLineText
            // 
            this.vLineText.Location = new System.Drawing.Point(127, 228);
            this.vLineText.Name = "vLineText";
            this.vLineText.Size = new System.Drawing.Size(47, 20);
            this.vLineText.TabIndex = 19;
            // 
            // dLineText
            // 
            this.dLineText.Location = new System.Drawing.Point(127, 251);
            this.dLineText.Name = "dLineText";
            this.dLineText.Size = new System.Drawing.Size(47, 20);
            this.dLineText.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Vertical Add";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Diagonal Add";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Horizontal Add";
            // 
            // b_ApplyProperties
            // 
            this.b_ApplyProperties.Location = new System.Drawing.Point(69, 303);
            this.b_ApplyProperties.Name = "b_ApplyProperties";
            this.b_ApplyProperties.Size = new System.Drawing.Size(76, 33);
            this.b_ApplyProperties.TabIndex = 14;
            this.b_ApplyProperties.Text = "Apply";
            this.b_ApplyProperties.UseVisualStyleBackColor = true;
            this.b_ApplyProperties.Click += new System.EventHandler(this.b_ApplyProperties_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Orientation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Is Reflectable";
            // 
            // frictionText
            // 
            this.frictionText.Location = new System.Drawing.Point(127, 120);
            this.frictionText.Name = "frictionText";
            this.frictionText.Size = new System.Drawing.Size(47, 20);
            this.frictionText.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Friction";
            // 
            // restText
            // 
            this.restText.Location = new System.Drawing.Point(127, 94);
            this.restText.Name = "restText";
            this.restText.Size = new System.Drawing.Size(47, 20);
            this.restText.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Restitution";
            // 
            // densityText
            // 
            this.densityText.Location = new System.Drawing.Point(127, 68);
            this.densityText.Name = "densityText";
            this.densityText.Size = new System.Drawing.Size(47, 20);
            this.densityText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Density";
            // 
            // pb_Level
            // 
            this.pb_Level.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pb_Level.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Level.Location = new System.Drawing.Point(0, 24);
            this.pb_Level.MaximumSize = new System.Drawing.Size(600, 600);
            this.pb_Level.MinimumSize = new System.Drawing.Size(600, 600);
            this.pb_Level.Name = "pb_Level";
            this.pb_Level.Size = new System.Drawing.Size(600, 600);
            this.pb_Level.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb_Level.TabIndex = 0;
            this.pb_Level.TabStop = false;
            this.pb_Level.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_Level_Paint);
            this.pb_Level.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pb_Level_MouseClick);
            this.pb_Level.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_Level_MouseDown);
            this.pb_Level.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_Level_MouseMove);
            this.pb_Level.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_Level_MouseUp);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(808, 636);
            this.Controls.Add(this.pb_Level);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Editor";
            this.Text = "Level Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ObjectsPage.ResumeLayout(false);
            this.ObjectsPage.PerformLayout();
            this.PropertiesPage.ResumeLayout(false);
            this.PropertiesPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Level)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ObjectsPage;
        private System.Windows.Forms.TabPage PropertiesPage;
        private System.Windows.Forms.RadioButton PlayerButton;
        private System.Windows.Forms.RadioButton WallButton;
        private System.Windows.Forms.TextBox densityText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox frictionText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox restText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton SpikeButton;
        private System.Windows.Forms.RadioButton DoorButton;
        private System.Windows.Forms.RadioButton KeyButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton SwitchButton;
        private System.Windows.Forms.PictureBox pb_Level;
        private System.Windows.Forms.Button b_ApplyProperties;
        private System.Windows.Forms.RadioButton BlockButton;
        private System.Windows.Forms.ToolStripMenuItem insertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dialogToolStripMenuItem;
        private System.Windows.Forms.RadioButton lineButton;
        private System.Windows.Forms.TextBox hLineText;
        private System.Windows.Forms.TextBox vLineText;
        private System.Windows.Forms.TextBox dLineText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox reflectableText;
        private System.Windows.Forms.ToolStripMenuItem removeAllReflectionLinesToolStripMenuItem;
        private System.Windows.Forms.ComboBox orientationText;
    }
}

