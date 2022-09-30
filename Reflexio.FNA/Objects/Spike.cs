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

namespace Reflexio
{
    public class Spike : Reflexio.ReflectableAndOrientable
    {

        public Spike(Level level)
            : base(level)
        {
        }

        public override void Initialize()
        {
            if (textures == null)
                textures = new Texture2D[4]
                    {
                        GameEngine.Instance.GetTexture("spikesUpTexture"),
                        GameEngine.Instance.GetTexture("spikesRightTexture"),
                        GameEngine.Instance.GetTexture("spikesDownTexture"),
                        GameEngine.Instance.GetTexture("spikesLeftTexture")
                    };
            base.Initialize();
        }

        public Spike(Level level, Texture2D[] textures, int x, int y, float density, float friction, float restitution, bool is_reflectable, Direction direction)
            : base (level, textures, x, y, density, friction, restitution, is_reflectable, direction)
        {
            this.Initialize();
        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Spike");
            writer.WriteAttributeString("params", "level");
            //writer.WriteElementString("Textures", textures.ToString());
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

        public override void CollidedWithNonReflectableObject(ReflectableObject ro)
        {
            if (!(ro is Wall))
                this.is_inside_non_reflectable_object = false;
        }
    }
}
