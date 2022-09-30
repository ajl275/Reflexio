using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Reflexio
{
    public class Key : Collectible
    {
        //private SoundEffectInstance se = GameEngine.Instance.GetMusic("zipperFast").CreateInstance();
        public Key(Level level)
            : base(level)
        {
        }

        public Key(Level level, Texture2D texture, int x, int y, float density, float friction, float restitution, bool is_reflectable)
            : base(level,texture,x,y,density,friction,restitution,is_reflectable)
        {
        }

        public override bool CollectedByPlayer()
        {
            //door.OpenDoor();
            SoundManager.PlaySound("zipperFast");
            level.Keys.Remove(this);
            this.IsDead = true;
            if (level.Keys.Count == 0)
                level.door.OpenDoor();
            return base.CollectedByPlayer();
        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Key");
            writer.WriteStartAttribute("params");
            writer.WriteString("level, door");
            writer.WriteEndAttribute();
            base.Serialize(writer);
            writer.WriteEndElement();
        }
    }
}
