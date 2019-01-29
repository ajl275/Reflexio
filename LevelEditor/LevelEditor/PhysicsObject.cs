using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LevelEditor
{
    public class PhysicsObject
    {
        public string type;
        public string openDoorTexture;
        public string closeDoorTexture;
        public string texture;

        // buffer variables for creation.
        public float density, friction, restitution;

        // The discrete position of the wall
        public int DiscX;
        public int DiscY;
        
        // Is this wall reflectable?
        public bool is_reflectable;

        public string orientation;

        public string hLines = "";
        public string vLines = "";
        public string dLines = "";

        private static Queue<Tuple<String, int>> dialog_displayer = new Queue<Tuple<string, int>>();

        public int Count
        {
            get { return dialog_displayer.Count; }
        }

        public PhysicsObject(string type, string texture, string openDoorTexture, string closeDoorTexture, int x, int y, float density, float friction, float restitution, bool is_reflectable, string orientation)
        {
            this.type = type;
            this.texture = texture;
            this.openDoorTexture = openDoorTexture;
            this.closeDoorTexture = closeDoorTexture;
            DiscX = x;
            DiscY = y;
            this.is_reflectable = is_reflectable;
            this.density = density;
            this.friction = friction;
            this.restitution = restitution;
            this.orientation = orientation;
        }
        /*
        public string toString()
        {
            string objtag = "";
            if (type.Equals("wall"))
            {
                objtag = @"<Wall params = ""level"">
                            <Texture>" + "groundTexture" + @"</Texture>
                            <IsReflectable>" + is_reflectable+ @"</IsReflectable>
                            <X>" + DiscX + @"</X>
                            <Y>" + DiscY + @"</Y>
                            <Friction>" + friction + @"</Friction>
                            <Density>" + density + @"</Density>
                            <Restitution>" + restitution + @"</Restitution>
                            <Initialize/>
                        </Wall>";
            }

            if (type.Equals("player"))
            {
                objtag = @"<Player params = ""level"">
                            <Texture>" + "playerTexture" + @"</Texture>
                            <BufferedPosition>" + DiscX + "," + DiscY + @"</BufferedPosition>
                            <Friction>" + friction + @"</Friction>
                            <Density>" + density + @"</Density>
                            <Restitution>" + restitution + @"</Restitution>
                            <Initialize/>
                        </Player>";
            }

            if (type.Equals("spike"))
            {
                if (orientation.Equals("U"))
                {
                    objtag = @"<Spike params = ""level"">
                                <Texture>" + "spikesUpTexture" + @"</Texture>
                                <IsReflectable>" + is_reflectable + @"</IsReflectable>
                                <X>" + DiscX + @"</X>
                                <Y>" + DiscY + @"</Y>
                                <Friction>" + friction + @"</Friction>
                                <Density>" + density + @"</Density>
                                <Restitution>" + restitution + @"</Restitution>
                                <Orientation>" + orientation + @"</Orientation>
                                <Initialize/>
                            </Spike>";
                }
                if (orientation.Equals("D"))
                {
                    objtag = @"<Spike params = ""level"">
                                <Texture>" + "spikesDownTexture" + @"</Texture>
                                <IsReflectable>" + is_reflectable + @"</IsReflectable>
                                <X>" + DiscX + @"</X>
                                <Y>" + DiscY + @"</Y>
                                <Friction>" + friction + @"</Friction>
                                <Density>" + density + @"</Density>
                                <Restitution>" + restitution + @"</Restitution>
                                <Orientation>" + orientation + @"</Orientation>
                                <Initialize/>
                            </Spike>";
                }
                if (orientation.Equals("L"))
                {
                    objtag = @"<Spike params = ""level"">
                                <Texture>" + "spikesLeftTexture" + @"</Texture>
                                <IsReflectable>" + is_reflectable + @"</IsReflectable>
                                <X>" + DiscX + @"</X>
                                <Y>" + DiscY + @"</Y>
                                <Friction>" + friction + @"</Friction>
                                <Density>" + density + @"</Density>
                                <Restitution>" + restitution + @"</Restitution>
                                <Orientation>" + orientation + @"</Orientation>
                                <Initialize/>
                            </Spike>";
                }
                if (orientation.Equals("R"))
                {
                    objtag = @"<Spike params = ""level"">
                                <Texture>" + "spikesRightTexture" + @"</Texture>
                                <IsReflectable>" + is_reflectable + @"</IsReflectable>
                                <X>" + DiscX + @"</X>
                                <Y>" + DiscY + @"</Y>
                                <Friction>" + friction + @"</Friction>
                                <Density>" + density + @"</Density>
                                <Restitution>" + restitution + @"</Restitution>
                                <Orientation>" + orientation + @"</Orientation>
                                <Initialize/>
                            </Spike>";
                }
            }

            if (type.Equals("door"))
            {
                objtag = @"<Door hash= ""door"" params = ""level"">
                            <OpenDoorTexture>openDoorTexture</OpenDoorTexture>
                            <CloseDoorTexture>closeDoorTexture</CloseDoorTexture>
                            <Texture>closeDoorTexture</Texture>
                            <IsReflectable>" + is_reflectable + @"</IsReflectable>
                            <X>" + DiscX + @"</X>
                            <Y>" + DiscY + @"</Y>
                            <Friction>" + friction + @"</Friction>
                            <Density>" + density + @"</Density>
                            <Restitution>" + restitution + @"</Restitution>
                            <Initialize/>
                        </Door>";
            }

            if (type.Equals("key"))
            {
                objtag = @"<Key params = ""level, door"">
                            <Texture>" + "keyTexture" + @"</Texture>
                            <IsReflectable>" + is_reflectable + @"</IsReflectable>
                            <X>" + DiscX + @"</X>
                            <Y>" + DiscY + @"</Y>
                            <Friction>" + friction + @"</Friction>
                            <Density>" + density + @"</Density>
                            <Restitution>" + restitution + @"</Restitution>
                            <Initialize/>
                        </Key>";
            }
            return objtag;
        }*/

        
        public void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement(type);
            writer.WriteAttributeString("params", "level");
            writer.WriteElementString("Texture", texture);
            if (type.Equals("Door"))
            {
                writer.WriteElementString("OpenDoorTexture", openDoorTexture);
                writer.WriteElementString("CloseDoorTexture", closeDoorTexture);
            }

            if (type.Equals("Spike") || type.Equals("Switch"))
            {
                writer.WriteElementString("Orientation", orientation);
            }

            if (type.Equals("Player"))
            {
                writer.WriteElementString("ReflectionTexture", "reflectionStrip");
                writer.WriteElementString("BufferedPosition", DiscX.ToString() + "," + DiscY.ToString());
            }
            else
            {
                writer.WriteElementString("IsReflectable", is_reflectable.ToString());
                writer.WriteElementString("X", DiscX.ToString());
                writer.WriteElementString("Y", DiscY.ToString());
                writer.WriteElementString("ReflectedHorizontal", "False");
                writer.WriteElementString("ReflectedVertical", "False");
            }

            if (type.Equals("Switch"))
            {
                if (!hLines.Equals(""))
                {
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    string[] words = hLines.Split(delimiterChars);
                    foreach (string i in words)
                    {
                        int j = int.Parse(i);
                        writer.WriteElementString("HLine", j.ToString());
                    }
                }
                if (!vLines.Equals(""))
                {
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    string[] words = vLines.Split(delimiterChars);
                    foreach (string i in words)
                    {
                        int j = int.Parse(i);
                        writer.WriteElementString("VLine", j.ToString());
                    }
                    
                }
                if (!dLines.Equals(""))
                {
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    string[] words = dLines.Split(delimiterChars);
                    foreach (string i in words)
                    {
                        int j = int.Parse(i);
                        writer.WriteElementString("DLine", j.ToString());
                    }
                }
            }

            writer.WriteElementString("Friction", friction.ToString());
            writer.WriteElementString("Density", density.ToString());
            writer.WriteElementString("Restitution", restitution.ToString());
            writer.WriteElementString("Initialize", "");

            writer.WriteEndElement();

        }

        public void SetTexture(string s)
        {
            texture = s;
        }

        public void SetOpenDoorTexture(string s)
        {
            openDoorTexture = s;
        }

        public void SetCloseDoorTexture(string s)
        {
            closeDoorTexture = s;
        }

        public void SetOrientation(string s)
        {
            orientation = s;
        }

        public void SetReflectionTexture(string s)
        {
        }

        public void SetDensity(float val)
        {
            this.density = val;
        }

        public void SetFriction(float val)
        {
            this.friction = val;
        }

        public void SetRestitution(float val)
        {
            this.restitution = val;
        }

        public void Initialize()
        {
        }

        public void SetBufferedPosition(int x, int y)
        {
            DiscX = x;
            DiscY = y;
        }

        public void SetX(int x)
        {
            DiscX = x;
        }

        public void SetY(int y)
        {
            DiscY = y;
        }

        public void SetReflectedHorizontal(bool b)
        {
        }

        public void SetReflectedVertical(bool b)
        {
        }

        public void SetIsReflectable(bool is_reflectable)
        {
            this.is_reflectable = is_reflectable;
        }

        public void AddHLine(int s)
        {
            if (hLines.Equals(""))
            {
                hLines += s;
            }
            else
            {
                hLines += "." + s;
            }
        }

        public void AddVLine(int s)
        {
            if (vLines.Equals(""))
            {
                vLines += s;
            }
            else
            {
                vLines += "." + s;
            }
        }

        public void AddDLine(int s)
        {
            if (dLines.Equals(""))
            {
                dLines += s;
            }
            else
            {
                dLines += "." + s;
            }
        }

        public void AddDialog(string s, int frames)
        {
            dialog_displayer.Enqueue(new Tuple<string, int>(s, frames));
        }

        public void Enqueue(Tuple<string, int> tup)
        {
            dialog_displayer.Enqueue(tup);
        }

        public Tuple<string, int> Dequeue()
        {
            return dialog_displayer.Dequeue();
        }
    }
}
