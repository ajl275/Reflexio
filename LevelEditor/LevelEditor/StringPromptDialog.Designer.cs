namespace LevelEditor
{
    partial class StringPromptDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.rowText = new System.Windows.Forms.TextBox();
            this.colText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gravText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.verticalBox = new System.Windows.Forms.CheckBox();
            this.horizontalBox = new System.Windows.Forms.CheckBox();
            this.diagonalBox = new System.Windows.Forms.CheckBox();
            this.borderBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.backgroundText = new System.Windows.Forms.ComboBox();
            this.musicText = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Rows:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // rowText
            // 
            this.rowText.Location = new System.Drawing.Point(198, 39);
            this.rowText.Name = "rowText";
            this.rowText.Size = new System.Drawing.Size(154, 20);
            this.rowText.TabIndex = 1;
            this.rowText.TextChanged += new System.EventHandler(this.rowText_TextChanged);
            // 
            // colText
            // 
            this.colText.Location = new System.Drawing.Point(198, 63);
            this.colText.Name = "colText";
            this.colText.Size = new System.Drawing.Size(154, 20);
            this.colText.TabIndex = 3;
            this.colText.TextChanged += new System.EventHandler(this.colText_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of Columns:";
            // 
            // gravText
            // 
            this.gravText.Location = new System.Drawing.Point(198, 86);
            this.gravText.Name = "gravText";
            this.gravText.Size = new System.Drawing.Size(154, 20);
            this.gravText.TabIndex = 5;
            this.gravText.TextChanged += new System.EventHandler(this.gravText_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value of Gravity (m/(s^2)):";
            // 
            // verticalBox
            // 
            this.verticalBox.AutoSize = true;
            this.verticalBox.Location = new System.Drawing.Point(21, 116);
            this.verticalBox.Name = "verticalBox";
            this.verticalBox.Size = new System.Drawing.Size(127, 17);
            this.verticalBox.TabIndex = 7;
            this.verticalBox.Text = "Can Reflect Vertically";
            this.verticalBox.UseVisualStyleBackColor = true;
            this.verticalBox.CheckedChanged += new System.EventHandler(this.verticalBox_CheckedChanged);
            // 
            // horizontalBox
            // 
            this.horizontalBox.AutoSize = true;
            this.horizontalBox.Location = new System.Drawing.Point(21, 139);
            this.horizontalBox.Name = "horizontalBox";
            this.horizontalBox.Size = new System.Drawing.Size(139, 17);
            this.horizontalBox.TabIndex = 8;
            this.horizontalBox.Text = "Can Reflect Horizontally";
            this.horizontalBox.UseVisualStyleBackColor = true;
            this.horizontalBox.CheckedChanged += new System.EventHandler(this.horizontalBox_CheckedChanged);
            // 
            // diagonalBox
            // 
            this.diagonalBox.AutoSize = true;
            this.diagonalBox.Location = new System.Drawing.Point(21, 162);
            this.diagonalBox.Name = "diagonalBox";
            this.diagonalBox.Size = new System.Drawing.Size(134, 17);
            this.diagonalBox.TabIndex = 9;
            this.diagonalBox.Text = "Can Reflect Diagonally";
            this.diagonalBox.UseVisualStyleBackColor = true;
            this.diagonalBox.CheckedChanged += new System.EventHandler(this.diagonalBox_CheckedChanged);
            // 
            // borderBox
            // 
            this.borderBox.AutoSize = true;
            this.borderBox.Location = new System.Drawing.Point(21, 185);
            this.borderBox.Name = "borderBox";
            this.borderBox.Size = new System.Drawing.Size(158, 17);
            this.borderBox.TabIndex = 10;
            this.borderBox.Text = "Auto Generate Border Walls";
            this.borderBox.UseVisualStyleBackColor = true;
            this.borderBox.CheckedChanged += new System.EventHandler(this.borderBox_CheckedChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(63, 261);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(119, 36);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(198, 261);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(119, 36);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Background Music";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Background Image";
            // 
            // backgroundText
            // 
            this.backgroundText.FormattingEnabled = true;
            this.backgroundText.Items.AddRange(new object[] {
            "bkgTexture",
            "bkg_movement",
            "bkg_horizontal",
            "bkg_hor_mult",
            "bkg_vert_mult",
            "groundTexture",
            "lineTexture",
            "playerTexture",
            "jumpUp",
            "jumpDown",
            "deathStrip",
            "collectibleTexture",
            "keyTexture",
            "openDoorTexture",
            "closeDoorTexture",
            "spikesUpTexture",
            "spikesRightTexture",
            "spikesDownTexture",
            "spikesLeftTexture",
            "idleStrip",
            "reflectionStrip",
            "buddyBlock",
            "reflectionCircle",
            "switchTexture",
            "zip",
            "unzip",
            "startOn",
            "startOff",
            "exitOn",
            "exitOff",
            "optionsOn",
            "levelselectOn",
            "levelselectOff",
            "mainbkg",
            "resumeOn",
            "resumeOff",
            "quitOn",
            "quitOff",
            "pauseOptionsOn",
            "pauseOptionsOff",
            "restartOn",
            "restartOff",
            "zippedbkg",
            "doorEat",
            "pauseOptionsOn",
            "pauseOptionsOff"});
            this.backgroundText.Location = new System.Drawing.Point(198, 231);
            this.backgroundText.Name = "backgroundText";
            this.backgroundText.Size = new System.Drawing.Size(154, 21);
            this.backgroundText.TabIndex = 17;
            // 
            // musicText
            // 
            this.musicText.FormattingEnabled = true;
            this.musicText.Items.AddRange(new object[] {
            "cityA",
            "cityB",
            "cityC",
            "meadows",
            "teardrop",
            "winter",
            "jumpMusic",
            "reflectMusic",
            "splatSound",
            "switchActive",
            "zipperSlow",
            "zipperFast",
            "dunDunDun",
            "destroy",
            "click"});
            this.musicText.Location = new System.Drawing.Point(198, 206);
            this.musicText.Name = "musicText";
            this.musicText.Size = new System.Drawing.Size(154, 21);
            this.musicText.TabIndex = 18;
            // 
            // StringPromptDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 299);
            this.Controls.Add(this.musicText);
            this.Controls.Add(this.backgroundText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.borderBox);
            this.Controls.Add(this.diagonalBox);
            this.Controls.Add(this.horizontalBox);
            this.Controls.Add(this.verticalBox);
            this.Controls.Add(this.gravText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.colText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rowText);
            this.Controls.Add(this.label1);
            this.Name = "StringPromptDialog";
            this.Text = "Level Initialization";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox rowText;
        private System.Windows.Forms.TextBox colText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox gravText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox verticalBox;
        private System.Windows.Forms.CheckBox horizontalBox;
        private System.Windows.Forms.CheckBox diagonalBox;
        private System.Windows.Forms.CheckBox borderBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox backgroundText;
        private System.Windows.Forms.ComboBox musicText;
    }
}