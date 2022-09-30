/*
 * LEVEL CREATOR DETAILS:
 * 
 * INITIALIZABLE PARAMS:
 * SetTexture(string)
 * SetIsReflectable(bool)
 * SetX(int)
 * SetY(int)
 * SetReflectedHorizontal(bool) Default false. false = Normal Direction, true = Reflected
 * SetReflectedVertical(bool) Default false. false = Normal Direction, true = Reflected
 * SetFriction(float)
 * SetDensity(float)
 * SetRestitution(float)
 * SetLevel(Level)
 * SetOrientation(String or) or = U|L|D|R. Default U
 * 
 * CONSTRUCTOR PARAMS
 * Level
 */

// USE THIS WHEN YOU WANT SINUSODIAL MOVEMENT WHILE REFLECTING DIAGONALLY
//#define SINUSODIAL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections;

namespace Reflexio
{
    public class Switch : Reflexio.ReflectableAndOrientable
    {
        //private SoundEffectInstance se1 = GameEngine.Instance.GetMusic("switchActive").CreateInstance();
        SortedSet<int> horizontal_lines = new SortedSet<int>();
        SortedSet<int> vertical_lines = new SortedSet<int>();
        SortedSet<int> diagonal_lines = new SortedSet<int>();

        Texture2D switchOnTexture;
        Texture2D switchOffTexture;
        private bool is_pressed = false;
        private PhysicsObject pressing_object = null;

        public Switch(Level level)
            : base(level)
        {
        }

        public override void Initialize()
        {
            switchOffTexture = this.texture;
            switchOnTexture = GameEngine.Instance.GetTexture("switchOnTexture");
            base.Initialize();
        }

        public Switch(Level level, Texture2D[] textures, int x, int y, float density, float friction, float restitution, bool is_reflectable, Direction direction)
            : base(level, textures, x, y, density, friction, restitution, is_reflectable, direction)
        {
            this.Initialize();
        }

        public override void Update(Level level, float dt)
        {
            if (pressing_object != null)
            {
                Vector2 diff = pressing_object.Body.Position - this.Body.Position;
                if (Math.Abs(diff.X) > this.width || Math.Abs(diff.Y) > this.height)
                    ReleaseSwitch();
                    //Console.WriteLine("HELLO");
            }
            base.Update(level, dt);
        }

        public void PressSwitch(PhysicsObject o)
        {
            if (!is_pressed)
            {
                texture = switchOnTexture;
                SoundManager.PlaySound("switchActive");
                is_pressed = true;
                pressing_object = o;
                foreach (int i in horizontal_lines)
                {
                    level.AddHRLine(i);
                    level.h_switch_lines.Add(i);
                }
                foreach (int i in vertical_lines)
                {
                    level.AddVRLine(i);
                    level.v_switch_lines.Add(i);
                }
                foreach (int i in diagonal_lines)
                {
                    level.AddDLine(i);
                    level.d_switch_lines.Add(i);
                }
            }
        }

        public void ReleaseSwitch()
        {
            if (is_pressed)
            {
                texture = switchOffTexture;
                is_pressed = false;
                pressing_object = null;
                foreach (int i in horizontal_lines)
                {
                    level.RemoveHRLine(i);
                }
                foreach (int i in vertical_lines)
                {
                    level.RemoveVRLine(i);
                }
                foreach (int i in diagonal_lines)
                {
                    level.RemoveDLine(i);
                }
            }
        }

        public override void CollidedWithNonReflectableObject(ReflectableObject ro)
        {
            if (!(ro is Wall))
                this.is_inside_non_reflectable_object = false;
        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Switch");
            writer.WriteAttributeString("params", "level");
            //TODO: textures
            switch (direction)
            {
                case Direction.Up: writer.WriteElementString("Orientation", "U"); break;
                case Direction.Left: writer.WriteElementString("Orientation", "L"); break;
                case Direction.Right: writer.WriteElementString("Orientation", "R"); break;
                case Direction.Down: writer.WriteElementString("Orientation", "D"); break;
            }
            base.Serialize(writer);
            writer.WriteEndElement();
        }

        public void AddHLine(int s)
        {
            horizontal_lines.Add(s);
        }

        public void AddVLine(int s)
        {
            vertical_lines.Add(s);
        }

        public void AddDLine(int s)
        {
            diagonal_lines.Add(s);
        }
    }
}
