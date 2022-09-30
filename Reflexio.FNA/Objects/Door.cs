using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Reflexio
{
    public class Door : Reflexio.ReflectableObject
    {
        // The textures for open and close door
        protected Texture2D openDoorTexture;
        protected Texture2D closeDoorTexture;
        private bool animating_open = false;

        protected bool is_open = false;

        public Texture2D OpenDoorTexture
        {
            get { return openDoorTexture; }
        }

        public Texture2D CloseDoorTexture
        {
            get { return closeDoorTexture; }
        }

        public void SetOpenDoorTexture(string tex_name)
        {
            this.openDoorTexture = Reflexio.GameEngine.Instance.GetTexture(tex_name);
        }

        public void SetCloseDoorTexture(string tex_name)
        {
            this.closeDoorTexture = Reflexio.GameEngine.Instance.GetTexture(tex_name);
        }

        public override void Initialize()
        {
            base.Initialize();
            texture = closeDoorTexture;
            this.animTexture = new AnimationTexture(GameEngine.Instance.GetTexture("doorEat"), 5, 1, 5);
        }

        public Door(Level level)
            : base(level)
        {
            this.level = level;
        }

        public Door(Level level, Texture2D openDoorTexture, Texture2D closeDoorTexture, int x, int y, float density, float friction, float restitution, bool is_reflectable)
            : base(level, null, x, y, density, friction, restitution, is_reflectable)
        {
            this.openDoorTexture = openDoorTexture;
            this.closeDoorTexture = closeDoorTexture;
            this.Initialize();
        }

        public override void Update(Level level, float dt)
        {
            if (animating_open)
            {
                if (animTexture.num_rotations > 0)
                {
                    this.use_animation_texture = false;
                    animTexture.stop_animating = true;
                    texture = openDoorTexture;
                }
            }
            base.Update(level, dt);
        }

        public void OpenDoor()
        {
            animating_open = true;
            this.animTexture = new AnimationTexture(GameEngine.Instance.GetTexture("openDoorStrip"), 5, 1, 5);
            this.Animate();
            this.is_open = true;
        }

        public void DoorEat()
        {
            this.animTexture = new AnimationTexture(GameEngine.Instance.GetTexture("doorEat"), 5, 1, 5);
            this.Animate();
        }

        public bool IsOpen()
        {
            return this.is_open;
        }

        public override void CollidedWithNonReflectableObject(ReflectableObject ro)
        {
            if (!(ro is Wall))
                this.is_inside_non_reflectable_object = false;
        }


        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Door");
            writer.WriteAttributeString("hash", "door");
            writer.WriteAttributeString("params", "level");
            writer.WriteElementString("OpenDoorTexture", openDoorTexture.Name);
            writer.WriteElementString("CloseDoorTexture", closeDoorTexture.Name);
            base.Serialize(writer);
            writer.WriteEndElement();
        }
    }
}
