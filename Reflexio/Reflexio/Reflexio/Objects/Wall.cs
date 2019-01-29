/**
 * CHANGES
 * 8 oct - Devansh - Changed the reflection logic for reflection about any line
 * 8 oct - Devansh - Also added another constructor and Initialize() and Setters.
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Reflexio
{
    public class Wall : Reflexio.ReflectableObject
    {
        //private SoundEffectInstance se = GameEngine.Instance.GetMusic("destroy").CreateInstance();
        public override void Initialize()
        {
            base.Initialize();
        }

        public Wall(Level level)
            : base(level)
        {
        }

        public Wall(Level level, Texture2D texture, int x, int y, float density, float friction, float restitution, bool is_reflectable)
            : base(level, texture, x, y, density, friction, restitution, is_reflectable)
        {
            this.Initialize();   
        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Wall");
            writer.WriteAttributeString("params", "level");
            base.Serialize(writer);
            writer.WriteEndElement();
        }

        public override void CollidedWithNonReflectableObject(ReflectableObject ro)
        {
            if (ro is Wall)
            {
                ro.CompletelyRemove();
                /*if (se.State == SoundState.Playing)
                    se.Stop();*/
                SoundManager.PlaySound("destroy");
            }

            if (ro is Block)
            {
                ro.CompletelyRemove();
                Block.gameBlocks.Remove((Block)ro);
                /*if (se.State == SoundState.Playing)
                    se.Stop();*/
                SoundManager.PlaySound("destroy");
                level.Buddydeath();
            }
        }
    }
}
