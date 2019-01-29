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
    public partial class Form1 : Form
    {
        private string currentFileName = "";
        private bool isInsertion;

        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set all tool squares to "unchecked."
        /// </summary>
        private void ClearToolMenu()
        {
            toolInsertion.Checked = false;
            toolSelection.Checked = false;
        }


        private void toolInsertion_Click(object sender, EventArgs e)
        {
            ClearToolMenu();
            toolInsertion.Checked = true;

            isInsertion = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolSelection_Click(object sender, EventArgs e)
        {
            ClearToolMenu();
            toolSelection.Checked = true;

            isInsertion = false;
        }


        private void mi_Save_As_Click(object sender, EventArgs e)
        {

                //Allow the user to choose a name and a location
                SaveFileDialog dialog = new SaveFileDialog();


                dialog.Filter = "XML Files | *.xml";

                dialog.InitialDirectory = ".";
                dialog.Title = "Choose the file to save.";


                DialogResult result = dialog.ShowDialog();
                currentFileName = dialog.FileName;
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentFileName == "")
            {
                mi_Save_As_Click(sender, e);
            }
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PlayerButton_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
