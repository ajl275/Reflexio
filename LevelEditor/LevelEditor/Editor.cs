using System;
using System.IO;
using System.Text.RegularExpressions; // For string tokenizing
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LevelEditor
{
    public partial class Editor : Form
    {

        private enum State
        {
            NO_EDITS, //Used when we don't have anything in the level editor
            EDITING, //Used when we have a level loaded.
        }

        private State currentState = State.NO_EDITS;

        public static int NUM_ROWS = 20;
        public static int NUM_COLS = 20;
        public static double GRAVITY = 9.8;
        public static bool BORDER = true;
        public static bool CANREFLECTHORIZONTAL;// = true;
        public static bool CANREFLECTVERTICAL;// = true;
        public static bool CANREFLECTDIAGONAL;// = true;
        public static int WIDTH = 600;
        public static int HEIGHT = 600;
        public static ReflectionOrientation ref_orientation = ReflectionOrientation.NULL;
        public static PhysicsObject dialog_displayer = new PhysicsObject("DialogDisplayer","","","",0,0,0f,0f,0f,false,"");

        public static int COLSCALE = WIDTH/NUM_COLS;
        public static int ROWSCALE = HEIGHT/NUM_ROWS;

        public static string music = "cityA";
        public static string background = "bkgTexture";

        ImageAttributes ia = new ImageAttributes(); 
        ColorMatrix cm = new ColorMatrix(); 

        // added these for setting reflection for level
        public enum ReflectionOrientation
        {
            HORIZONTAL, VERTICAL, DIAGONAL, NULL
        };

        // Used for defining which lines of reflection can be used
        public SortedSet<int> horizontal_lines = new SortedSet<int>();
        public SortedSet<int> vertical_lines = new SortedSet<int>();
        public SortedSet<int> diagonal_lines = new SortedSet<int>();

        private string currentFileName = "";
        
        //private string levelString = "";

        string currdir = CurrDirHack();

        private bool isInsertion;

        private PhysicsObject currentlySelectedObject;

        public List<PhysicsObject> objects;

        //limit to one door and one player
        public PhysicsObject door;
        public PhysicsObject player;

        public int? rPos = null;

        // Stores all the textures
        private static Hashtable textures = new Hashtable();
        // TEXTURE MAPPING FROM NAME TO FILE
        private static string[][] texture_files = 
        {
            new string[] {"bkgTexture", "Images\\Backgrounds\\BlueToast"},
            new string[] {"groundTexture", "Images\\Blocks\\blue"},
            new string[] {"block_green", "Images\\Blocks\\green"},
            new string[] {"block_orange", "Images\\Blocks\\orange"},
            new string[] {"block_purple", "Images\\Blocks\\purple"},
            new string[] {"block_red", "Images\\Blocks\\red"},
            new string[] {"block_yellow", "Images\\Blocks\\yellow"},
            new string[] {"block_blue", "Images\\Blocks\\blue"},
            new string[] {"lineTexture", "Images\\line"},
            new string[] {"playerTexture", "Images\\koalaOld"},
            //new string[] {"collectibleTexture", "Images\\collectible"},
            new string[] {"keyTexture", "Images\\key"},
            new string[] {"openDoorTexture", "Images\\opendoor"},
            new string[] {"closeDoorTexture", "Images\\door"},
            new string[] {"spikesUpTexture", "Images\\spikesUp"},
            new string[] {"spikesRightTexture", "Images\\spikesRight"},
            new string[] {"spikesDownTexture", "Images\\spikesDown"},
            new string[] {"spikesLeftTexture", "Images\\spikesLeft"},
            new string[] {"reflectionStrip", "Images\\reflectionStrip"},
            new string[] {"buddyBlock", "Images\\smallbuddyblock"},
            //new string[] {"reflectionCircle", "Images\\reflectionCircle"},
            new string[] {"switchTexture", "Images\\switch"}
        };


        public Editor()
        {
            InitializeComponent();
            isInsertion = true;
            densityText.Enabled = false;
            restText.Enabled = false;
            frictionText.Enabled = false;
            reflectableText.Enabled = false;
            orientationText.Enabled = false;
            objects = new List<PhysicsObject>();
            foreach (string[] texture in texture_files)
                textures.Add(texture[0], Image.FromFile(currdir + "\\" + texture[1] + ".png"));
            // 1/3 on the top 3 rows and 3 columns 
            cm.Matrix00 = 1 / 3f;
            cm.Matrix01 = 1 / 3f;
            cm.Matrix02 = 1 / 3f;
            cm.Matrix10 = 1 / 3f;
            cm.Matrix11 = 1 / 3f;
            cm.Matrix12 = 1 / 3f;
            cm.Matrix20 = 1 / 3f;
            cm.Matrix21 = 1 / 3f;
            cm.Matrix22 = 1 / 3f;
            ColorMap[] clrmap = new ColorMap[1] { new ColorMap() }; 
            clrmap[0].OldColor = Color.White;
            clrmap[0].NewColor = Color.Gray;

            ia.SetRemapTable(clrmap); 
            ia.SetColorMatrix(cm);

        }

        /// <summary>
        /// Set all tool squares to "unchecked."
        /// </summary>

        private void PlayerButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PlayerButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Player", "playerTexture", "", "", 0, 0, 1.5f, 0f, 0f, true, "U");
                densityText.Enabled = true;
                densityText.Text = "" + currentlySelectedObject.density;
                restText.Enabled = true;
                restText.Text = "" + currentlySelectedObject.restitution;
                frictionText.Enabled = true;
                frictionText.Text = "" + currentlySelectedObject.friction;
                orientationText.Enabled = false;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private void WallButton_CheckedChanged(object sender, EventArgs e)
        {
            if (WallButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Wall", "groundTexture", "", "", 0, 0, 0f, 1f, 0f, true, "U");
                densityText.Enabled = true;
                densityText.Text = "" + currentlySelectedObject.density;
                restText.Enabled = true;
                restText.Text = "" + currentlySelectedObject.restitution;
                frictionText.Enabled = true;
                frictionText.Text = "" + currentlySelectedObject.friction;
                reflectableText.Enabled = true;
                reflectableText.Text = "" + currentlySelectedObject.is_reflectable;
                orientationText.Enabled = false;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private void KeyButton_CheckedChanged(object sender, EventArgs e)
        {
            if (KeyButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Key", "keyTexture", "", "", 0, 0, 0f, 0f, 0f, true, "U");
                densityText.Enabled = true;
                densityText.Text = "" + currentlySelectedObject.density;
                restText.Enabled = true;
                restText.Text = "" + currentlySelectedObject.restitution;
                frictionText.Enabled = true;
                frictionText.Text = "" + currentlySelectedObject.friction;
                reflectableText.Enabled = true;
                reflectableText.Text = "" + currentlySelectedObject.is_reflectable;
                orientationText.Enabled = false;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private void DoorButton_CheckedChanged(object sender, EventArgs e)
        {
            if (DoorButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Door", "closeDoorTexture", "openDoorTexture", "closeDoorTexture", 0, 0, 0f, 0f, 0f, true, "U");
                densityText.Enabled = true;
                densityText.Text = "" + currentlySelectedObject.density;
                restText.Enabled = true;
                restText.Text = "" + currentlySelectedObject.restitution;
                frictionText.Enabled = true;
                frictionText.Text = "" + currentlySelectedObject.friction;
                reflectableText.Enabled = true;
                reflectableText.Text = "" + currentlySelectedObject.is_reflectable;
                orientationText.Enabled = false;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private void SpikeButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SpikeButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Spike", "spikesUpTexture", "", "", 0, 0, 0f, 1f, 0f, true, "U");
                densityText.Enabled = true;
                densityText.Text = "" + currentlySelectedObject.density;
                restText.Enabled = true;
                restText.Text = "" + currentlySelectedObject.restitution;
                frictionText.Enabled = true;
                frictionText.Text = "" + currentlySelectedObject.friction;
                reflectableText.Enabled = true;
                reflectableText.Text = "" + currentlySelectedObject.is_reflectable;
                orientationText.Enabled = true;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private void lineButton_CheckedChanged(object sender, EventArgs e)
        {
            if (lineButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Line", "lineTexture", "", "", 0, 0, 0f, 0f, 0f, true, "H");
                densityText.Enabled = false;
                densityText.Text = "0";
                restText.Enabled = false;
                restText.Text = "0";
                frictionText.Enabled = false;
                frictionText.Text = "0";
                reflectableText.Enabled = false;
                reflectableText.Text = "false";
                orientationText.Enabled = true;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private void BlockButton_CheckedChanged(object sender, EventArgs e)
        {
            if (BlockButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Block", "buddyBlock", "", "", 0, 0, 1f, 5f, 0.25f, true, "U");
                densityText.Enabled = true;
                densityText.Text = currentlySelectedObject.density.ToString();
                restText.Enabled = true;
                restText.Text = currentlySelectedObject.restitution.ToString();
                frictionText.Enabled = true;
                frictionText.Text = currentlySelectedObject.friction.ToString();
                reflectableText.Enabled = true;
                reflectableText.Text = currentlySelectedObject.is_reflectable.ToString();
                orientationText.Enabled = true;
                orientationText.Text = currentlySelectedObject.orientation;
            }
        }

        private void SwitchButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SwitchButton.Checked)
            {
                currentlySelectedObject = new PhysicsObject("Switch", "switchTexture", "", "", 0, 0, 0f, 0f, 0f, true, "U");
                densityText.Enabled = true;
                densityText.Text = "" + currentlySelectedObject.density;
                restText.Enabled = true;
                restText.Text = "" + currentlySelectedObject.restitution;
                frictionText.Enabled = true;
                frictionText.Text = "" + currentlySelectedObject.friction;
                reflectableText.Enabled = true;
                reflectableText.Text = "" + currentlySelectedObject.is_reflectable;
                orientationText.Enabled = true;
                orientationText.Text = "" + currentlySelectedObject.orientation;
            }
        }

        private bool toDrag = false;
        private int y_drag_transform = 0;
        private int x_drag_transform = 0;
        
        //Callback for when the user clicks on the painting area. We need to insert or select an object
        private void pb_Level_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentState != State.NO_EDITS)
            {
                    painting_clicked_insert(e);
            }
        }

        //The callback for when the user presses down the mouse inside the painting area. This means the
        //user wants to start dragging an object.
        private void pb_Level_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && currentlySelectedObject != null)
            {
                x_drag_transform = e.X - currentlySelectedObject.DiscX*COLSCALE;
                y_drag_transform = e.Y - currentlySelectedObject.DiscY*ROWSCALE;

                toDrag = true;
            }
        }

        //The user wants to stop dragging.
        private void pb_Level_MouseUp(object sender, MouseEventArgs e)
        {
            toDrag = false;
        }

        //The user may have dragged the object
        private void pb_Level_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentlySelectedObject != null &&
                !isInsertion)
            {

                if (toDrag)
                {
                    //Update the object's position
                    double new_X = (e.Location.X - x_drag_transform + 0.000001)/COLSCALE;
                    double new_Y = (e.Location.Y - y_drag_transform + 0.000001)/ROWSCALE;

                    currentlySelectedObject.DiscX = (int)(new_X+0.5);
                    currentlySelectedObject.DiscY = (int)(new_Y+0.5);
                    //Refresh the painting area so we can dynamic drawing
                    
                }
                
            }
            
        }
        
        //The distance between two points.
        private double dist(int X1, int Y1, int X2, int Y2)
        {
            return Math.Pow((X1 - X2), 2) + Math.Pow(Math.Abs(Y1 - Y2), 2);
        }


        private void b_ApplyProperties_Click(object sender, EventArgs e)
        {
            currentlySelectedObject.density = float.Parse(densityText.Text);
            currentlySelectedObject.restitution = float.Parse(restText.Text);
            currentlySelectedObject.friction = float.Parse(frictionText.Text);
            currentlySelectedObject.is_reflectable = bool.Parse(reflectableText.Text);
            currentlySelectedObject.orientation = orientationText.Text;
            currentlySelectedObject.hLines = hLineText.Text;
            currentlySelectedObject.dLines =  dLineText.Text;
            currentlySelectedObject.vLines =  vLineText.Text;
        }


        //Used when the user clicks in the painting area while using the insert tool.
        // This will create a new object when the user left-clicks, and delete the object
        // it is currently on when it is right clicked.
        private void painting_clicked_insert(MouseEventArgs e)
        {
            Point mp = pb_Level.PointToClient(MousePosition);
            // Remove the object under the cursor.
            if (e.Button == MouseButtons.Right)
            {
                //XnaPoint mouse = new XnaPoint(pb_Level.PointToClient(MousePosition).X, pb_Level.PointToClient(MousePosition).Y); ;
                foreach (PhysicsObject obj in objects)
                {
                    //XnaRectangle rect = new XnaRectangle(obj.DiscX * ROWSCALE, obj.DiscY * COLSCALE, ROWSCALE, COLSCALE);
                    if (mp.X > (obj.DiscX * COLSCALE) && mp.X < (obj.DiscX * COLSCALE + COLSCALE ) &&
                        mp.Y > (obj.DiscY * ROWSCALE) && mp.Y < (obj.DiscY * ROWSCALE + ROWSCALE))
                    {
                        objects.Remove(obj);
                        break;
                    }
                }
            }

            // Add an object.
            if (e.Button == MouseButtons.Left)
            {
                PhysicsObject temp = null;
                if (WallButton.Checked)
                {
                    temp = new PhysicsObject("Wall", "groundTexture", "", "", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                }
                else if (PlayerButton.Checked)
                {
                    if (player != null)
                    {
                        objects.Remove(player);
                        pb_Level.Invalidate(new Rectangle(player.DiscX * COLSCALE, player.DiscY * ROWSCALE, COLSCALE, ROWSCALE));
                    }
                    temp = new PhysicsObject("Player", "playerTexture", "", "", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                    player = temp;
                }
                else if (SpikeButton.Checked)
                {
                    temp = new PhysicsObject("Spike", "spikesUpTexture", "", "", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                    switch (temp.orientation)
                    {
                        case "U": temp.texture = "spikesUpTexture"; break;
                        case "D": temp.texture = "spikesDownTexture"; break;
                        case "L": temp.texture = "spikesLeftTexture"; break;
                        case "R": temp.texture = "spikesRightTexture"; break;
                    }
                }
                else if (DoorButton.Checked)
                {
                    if (door != null)
                    {
                        objects.Remove(door);
                        pb_Level.Invalidate(new Rectangle(door.DiscX * COLSCALE, door.DiscY * ROWSCALE, COLSCALE, ROWSCALE));
                    }
                    temp = new PhysicsObject("Door", "closeDoorTexture", "openDoorTexture", "closeDoorTexture", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                    door = temp;
                }
                else if (KeyButton.Checked)
                {
                    temp = new PhysicsObject("Key", "keyTexture", "", "", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                }
                else if (BlockButton.Checked)
                {
                    temp = new PhysicsObject("Block", "buddyBlock", "", "", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                }
                else if (SwitchButton.Checked)
                {
                    temp = new PhysicsObject("Switch", "switchTexture", "", "", mp.X / COLSCALE, mp.Y / ROWSCALE, currentlySelectedObject.density, currentlySelectedObject.friction, currentlySelectedObject.restitution, currentlySelectedObject.is_reflectable, currentlySelectedObject.orientation);
                    temp.hLines = currentlySelectedObject.hLines;
                    temp.vLines = currentlySelectedObject.vLines;
                    temp.dLines = currentlySelectedObject.dLines;
                }
                else if (lineButton.Checked)
                {
                    int x = mp.X / COLSCALE;
                    int y = mp.Y / ROWSCALE;
                    switch(currentlySelectedObject.orientation)
                    {
                        case "H":
                            if (CANREFLECTHORIZONTAL && y > 0 && y < NUM_ROWS)
                            {
                                if (!rPos.HasValue)
                                {
                                    ref_orientation = ReflectionOrientation.HORIZONTAL;
                                    rPos = y;
                                }
                                horizontal_lines.Add(y);
                                pb_Level.Invalidate(new Rectangle(0, y * ROWSCALE - 5, WIDTH, 10));
                            }break;
                        case "V":
                            if (CANREFLECTVERTICAL && x > 0 && x < NUM_COLS)
                            {
                                if (!rPos.HasValue)
                                {
                                    ref_orientation = ReflectionOrientation.VERTICAL;
                                    rPos = x;
                                }
                                vertical_lines.Add(x);
                                pb_Level.Invalidate(new Rectangle(x * COLSCALE - 5, 0, 10, HEIGHT));
                            } break;
                        case "D":
                            if (CANREFLECTDIAGONAL)
                            {
                                if ((x < NUM_COLS / 2 && y < NUM_ROWS / 2) || (x > NUM_COLS / 2 && y > NUM_ROWS / 2))
                                {
                                    if (!rPos.HasValue)
                                    {
                                        ref_orientation = ReflectionOrientation.DIAGONAL;
                                        rPos = 1;
                                    }
                                    diagonal_lines.Add(1);
                                }
                                else
                                {
                                    if (!rPos.HasValue)
                                    {
                                        ref_orientation = ReflectionOrientation.DIAGONAL;
                                        rPos = -1;
                                    }
                                    diagonal_lines.Add(-1);
                                }
                                    
                                pb_Level.Invalidate(pb_Level.ClientRectangle);
                                 
                            }break;
                        default :
                            break;
                    }
                }
                if (!lineButton.Checked)
                {
                    objects.Add(temp);
                }
           }
            if (!lineButton.Checked)
            {
                pb_Level.Invalidate(new Rectangle(mp.X / COLSCALE * COLSCALE, mp.Y / ROWSCALE * ROWSCALE, COLSCALE, ROWSCALE));
            }
            //else
            //{
            //    pb_Level.Invalidate(pb_Level.ClientRectangle);
            //}
            pb_Level.Update();
        }

        public class PhysicsObjectComparer : IComparer<PhysicsObject>
        {
            public int Compare(PhysicsObject a, PhysicsObject b)
            {
                if (a == null && b == null)
                {
                    return 0;
                }
                else if (a == null)
                {
                    return -1;
                }
                else if (b == null)
                {
                    return 1;
                }

                if (a.type.Equals(b.type))
                    return a.ToString().CompareTo(b.ToString());
                else
                {
                    if (a.type.Equals("Switch"))
                        return -1;
                    else if (a.type.Equals("Wall") && !b.type.Equals("Switch"))
                        return -1;
                    else if (a.type.Equals("Spike") && (!b.type.Equals("Switch") || !b.type.Equals("Wall")))
                        return -1;
                    else if (a.type.Equals("Door") && (!b.type.Equals("Switch") || !b.type.Equals("Wall") || !b.type.Equals("Spike")))
                        return -1;
                    else if (a.type.Equals("Block") && (!b.type.Equals("Switch") || !b.type.Equals("Wall") || !b.type.Equals("Spike") || !b.type.Equals("Door")))
                        return -1;
                    else if (a.type.Equals("Key") && (!b.type.Equals("Switch") || !b.type.Equals("Wall") || !b.type.Equals("Spike") || !b.type.Equals("Door") || !b.type.Equals("Block")))
                        return -1;
                    else
                        return 1;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Allow the user to choose a name and a location
            SaveFileDialog dialog = new SaveFileDialog();


            dialog.Filter = "XML Files | *.xml";

            dialog.InitialDirectory = ".";
            dialog.Title = "Choose the file to save.";


            DialogResult result = dialog.ShowDialog();
            
            currentFileName = dialog.FileName;

            if (result == DialogResult.OK)
            {
                XmlWriterSettings set = new XmlWriterSettings();
                set.Indent = true;
                XmlWriter writer = XmlWriter.Create(currentFileName, set);
                Serialize(writer);
                writer.Close();
            }

            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringPromptDialog dialog = new StringPromptDialog();

            //initializes the level
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                objects = new List<PhysicsObject>();

                if (BORDER)
                {
                    for (int i = 0; i < NUM_ROWS; i++)
                    {
                        for (int j = 0; j < NUM_COLS; j++)
                        {
                            if (i == 0 || j == 0 || i == (NUM_ROWS - 1) || j == (NUM_COLS - 1))
                            {
                                PhysicsObject wall = new PhysicsObject("Wall", "groundTexture", "", "", i, j, 0f, .1f, 0f, false, "U");
                                //currentlySelectedObject = wall;
                                objects.Add(wall);
                            }
                        }
                    }
                }
                currentState = State.EDITING;

                

                pb_Level.Refresh();
            }
        }



        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Allow the user to choose a name and a location
            OpenFileDialog dialog = new OpenFileDialog();


            dialog.Filter = "XML Files | *.xml";

            dialog.InitialDirectory = ".";
            dialog.Title = "Choose the file to open.";


            DialogResult result = dialog.ShowDialog();
            
            currentFileName = dialog.FileName;

            if (result == DialogResult.OK)
            {
                objects = new List<PhysicsObject>();
                vertical_lines.Clear();
                horizontal_lines.Clear();
                diagonal_lines.Clear();
                door = null;
                player = null;
                LevelCreator.ParseLevelFromFile(currentFileName, this);
                this.currentState = State.EDITING;
                pb_Level.Refresh();
            }
            
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentState != State.NO_EDITS)
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        public static string CurrDirHack()
        {
            return (Directory.GetCurrentDirectory()).Replace("LevelEditor\\LevelEditor\\bin\\Debug", "Reflexio\\Reflexio\\ReflexioContent").Replace("LevelEditor\\LevelEditor\\bin\\Release", "Reflexio\\Reflexio\\ReflexioContent");
        }

        private void drawObject(PhysicsObject obj, PaintEventArgs e)
        {
            if (obj.is_reflectable)
            {
                e.Graphics.DrawImage((Image)textures[obj.texture],
                    obj.DiscX * COLSCALE,
                    obj.DiscY * ROWSCALE,
                    COLSCALE,
                    ROWSCALE);
            }
            else
            {
                 e.Graphics.DrawImage((Image)textures[obj.texture], new Rectangle(obj.DiscX * COLSCALE,
                    obj.DiscY * ROWSCALE,
                    COLSCALE,
                    ROWSCALE), 0, 0, ((Image)textures[obj.texture]).Width, ((Image)textures[obj.texture]).Height,
                    GraphicsUnit.Pixel, ia); 
            }
        }

        //Paints all the objects on the drawing pane.
        private void pb_Level_Paint(object sender, PaintEventArgs e)
        {
            if (currentState == State.EDITING)
            {
                for (int i = 1; i < NUM_COLS; i++)
                {
                    e.Graphics.DrawLine(Pens.Black, i * COLSCALE, 0, i * COLSCALE, 600);
                }

                for (int j = 1; j < NUM_COLS; j++)
                {
                    e.Graphics.DrawLine(Pens.Black, 0,  j * ROWSCALE, 600, j * ROWSCALE);
                }

                foreach (PhysicsObject obj in objects)
                {
                    if (obj != null)
                        drawObject(obj, e);
                }

                foreach (int o in horizontal_lines)
                {
                    e.Graphics.DrawLine(Pens.Yellow, 0, o * ROWSCALE, WIDTH, o * ROWSCALE);
                }
                foreach (int o in vertical_lines)
                {
                    e.Graphics.DrawLine(Pens.Yellow, o * COLSCALE, 0, o * ROWSCALE, HEIGHT) ;
                }
                foreach (int o in diagonal_lines)
                {
                    if (o == 1)
                        e.Graphics.DrawLine(Pens.Yellow, 0, 0, WIDTH, HEIGHT);
                    else
                        e.Graphics.DrawLine(Pens.Yellow, WIDTH, 0, 0, HEIGHT);
                }
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Black, pb_Level.ClientRectangle);
            }
        }

        private void Serialize(XmlWriter writer)
        {
            //if (horizontal_lines.Count == 0)
            //    CANREFLECTHORIZONTAL = false;
            //if (vertical_lines.Count == 0)
            //    CANREFLECTVERTICAL = false;
            //if (diagonal_lines.Count == 0)
            //    CANREFLECTDIAGONAL = false;

            objects.Sort(new PhysicsObjectComparer());

            writer.WriteProcessingInstruction("xml", @"version = ""1.0""");
            writer.WriteStartElement("Level");
            writer.WriteAttributeString("hash", "level");
            writer.WriteElementString("BkgSound", music);
            writer.WriteElementString("BkgImage", background);
            writer.WriteElementString("Width", "13");
            writer.WriteElementString("Height", "13");
            writer.WriteElementString("Gravity", "0" + "," + GRAVITY.ToString());
            writer.WriteElementString("Scale", "50");
            writer.WriteElementString("Dimensions", NUM_ROWS.ToString() + ", " + NUM_COLS.ToString());
            writer.WriteElementString("ReflectionPauseTime", "25");
            writer.WriteElementString("CanReflectHorizontal", CANREFLECTHORIZONTAL.ToString());
            writer.WriteElementString("CanReflectVertical", CANREFLECTVERTICAL.ToString());
            writer.WriteElementString("CanReflectDiagonal", CANREFLECTDIAGONAL.ToString());
            
            foreach (Object o in horizontal_lines)
            {
                writer.WriteElementString("HRLine", o.ToString());
            }
            foreach (Object o in vertical_lines)
            {
                writer.WriteElementString("VRLine", o.ToString());
            }
            foreach (Object o in diagonal_lines)
            {
                writer.WriteElementString("DLine", o.ToString());
            }
            switch (ref_orientation)
            {
                case ReflectionOrientation.DIAGONAL:
                    {
                        writer.WriteElementString("ReflectionOrientation", "D");
                        writer.WriteElementString("ReflectionLinePosition", rPos.ToString());
                    } break;
                case ReflectionOrientation.HORIZONTAL:
                    {
                        writer.WriteElementString("ReflectionOrientation", "H");
                        writer.WriteElementString("ReflectionLinePosition", rPos.ToString());
                    } break;
                case ReflectionOrientation.VERTICAL:
                    {
                        writer.WriteElementString("ReflectionOrientation","V"); 
                        writer.WriteElementString("ReflectionLinePosition", rPos.ToString());
                    } break;
                default :
                    break;
            }
            if (dialog_displayer.Count > 0)
            {
                writer.WriteStartElement("DialogDisplay");
                while (dialog_displayer.Count != 0)
                {
                    Tuple<string, int> foo = dialog_displayer.Dequeue();
                    writer.WriteElementString("Dialog",foo.Item1 + ", " + foo.Item2.ToString());
                }
                writer.WriteElementString("Initialize", "");
                writer.WriteEndElement();
            }

            writer.WriteElementString("Initialize", "");

            foreach (PhysicsObject o in objects)
            {
                if(o != null)
                    o.Serialize(writer);
            }

            writer.WriteEndElement();
        }

        private void dialogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentState == State.EDITING)
            {
                DialogAdder d = new DialogAdder();
                d.ShowDialog();
            }
        }

        private void removeAllReflectionLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            horizontal_lines = new SortedSet<int>();
            vertical_lines = new SortedSet<int>();
            diagonal_lines = new SortedSet<int>();
            ref_orientation = ReflectionOrientation.NULL;
            rPos = null;
            pb_Level.Invalidate(pb_Level.ClientRectangle);
        }

        public void SetWidth(float width)
        {
        }

        public void SetHeight(float height)
        {
        }

        public void SetGravity(float x, float y)
        {
            Editor.GRAVITY = y;
        }

        public void SetScale(float scale)
        {
        }

        public void SetDimensions(int rows, int cols)
        {
            Editor.NUM_ROWS = rows;
            Editor.NUM_COLS = cols;
        }

        public void SetReflectionPauseTime(int time)
        {
        }

        public void SetCanReflectHorizontal(bool val)
        {
            CANREFLECTHORIZONTAL = val;
        }

        public void SetCanReflectVertical(bool val)
        {
            CANREFLECTVERTICAL = val;
        }

        public void SetCanReflectDiagonal(bool val)
        {
            CANREFLECTDIAGONAL = val;
        }

        public void SetReflectionLinePosition(int pos)
        {
            rPos = pos;
        }

        public void SetReflectionOrientation(string orientation)
        {
            if (orientation.Equals("H", StringComparison.OrdinalIgnoreCase))
                ref_orientation = ReflectionOrientation.HORIZONTAL;
            if (orientation.Equals("V", StringComparison.OrdinalIgnoreCase))
                ref_orientation = ReflectionOrientation.VERTICAL;
            if (orientation.Equals("D", StringComparison.OrdinalIgnoreCase))
                ref_orientation = ReflectionOrientation.DIAGONAL;
        }

        public void AddHRLine(int line)
        {
            this.horizontal_lines.Add(line);
        }

        public void AddVRLine(int line)
        {
            this.vertical_lines.Add(line);
        }

        public void AddDLine(int line)
        {
            this.diagonal_lines.Add(line);
        }

        public void AddDialogDisplay(PhysicsObject p)
        {
        }

        public void SetBkgImage(string s)
        {
            background = s;
        }

        public void SetBkgSound(string s)
        {
            music = s;
        }

        public void Initialize()
        {
        }
    }
}
