using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class StringPromptDialog : Form
    {
        public StringPromptDialog()
        {
            
            InitializeComponent();
            rowText.Text = "20";
            colText.Text = "20";
            gravText.Text = "9.8";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rowText_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void colText_TextChanged(object sender, EventArgs e)
        {

        }

        private void gravText_TextChanged(object sender, EventArgs e)
        {

        }

        private void verticalBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void horizontalBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void diagonalBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void borderBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        
        private void musicText_TextChanged(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Editor.CANREFLECTDIAGONAL = diagonalBox.Checked;
            Editor.CANREFLECTHORIZONTAL = horizontalBox.Checked;
            Editor.CANREFLECTVERTICAL = verticalBox.Checked;
            Editor.NUM_ROWS = int.Parse(rowText.Text);
            Editor.NUM_COLS = int.Parse(colText.Text);
            Editor.GRAVITY = double.Parse(gravText.Text);
            Editor.BORDER = borderBox.Checked;
            Editor.COLSCALE = Editor.WIDTH / Editor.NUM_COLS;
            Editor.ROWSCALE = Editor.HEIGHT / Editor.NUM_ROWS;

            Editor.music = musicText.Text;
            Editor.background = backgroundText.Text;

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        
    }
}
