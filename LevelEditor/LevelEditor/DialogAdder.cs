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
    public partial class DialogAdder : Form
    {
        public DialogAdder()
        {
            InitializeComponent();
            text.Text = "Message";
            frames.Text = "60";
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            Editor.dialog_displayer.Enqueue(new Tuple<string, int>(text.Text, int.Parse(frames.Text)));
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DialogAdder_Load(object sender, EventArgs e)
        {

        }
    }
}
