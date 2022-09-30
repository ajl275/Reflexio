using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
namespace Reflexio
{
    public abstract class Collectible : Reflexio.ReflectableObject
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public Collectible(Level level)
            : base(level)
        {
            this.level = level;
        }

        public Collectible(Level level, Texture2D texture, int x, int y, float density, float friction, float restitution, bool is_reflectable)
            : base(level, texture, x, y, density, friction, restitution, is_reflectable)
        {
            this.Initialize();
        }

        public virtual bool CollectedByPlayer()
        {
            if (!this.is_inside_non_reflectable_object)
                this.CompletelyRemove();
            return false;
        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            base.Serialize(writer);
        }
    }
}
