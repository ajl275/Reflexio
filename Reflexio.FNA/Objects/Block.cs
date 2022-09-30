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
using Color = Microsoft.Xna.Framework.Color;

namespace Reflexio
{
    public class Block : Reflexio.ReflectableObject
    {
        private Texture2D normal_texture;
        //private SoundEffectInstance se = GameEngine.Instance.GetMusic("destroy").CreateInstance();
        public static List<Block> gameBlocks = new List<Block>();
        public bool is_orginally_reflectable;
        float prev_y_vel;
        private int buddyFlinchTime;

        private const int BLOCK_FLINCH_TIME = 30;
        public void SetBufferedPosition(int x, int y)
        {
            Vector2 world_pos = level.DiscreteToContinuous(x, y);
            this.BufferedPosition = new Vector2(world_pos.X + level.ROW_SCALE / 2, world_pos.Y + level.COL_SCALE / 2);
        }

        public new void Initialize()
        {
            width = level.ROW_SCALE * 0.93f;
            height = level.COL_SCALE * 0.93f;
            Vector2 world_pos = level.DiscreteToContinuous(DiscX, DiscY);
            this.BufferedPosition = new Vector2(world_pos.X + level.ROW_SCALE / 2, world_pos.Y + level.COL_SCALE / 2);
            is_orginally_reflectable = is_reflectable;
            gameBlocks.Add(this);
            if (!is_reflectable)
                ReflectableObject.non_reflectable_objects.Add(this);
            normal_texture = texture;
            prev_y_vel = 0;
            buddyFlinchTime = 0;
        }

        public static void FlushGameBlocks()
        {
            gameBlocks.Clear();
        }

        public Block(Level level)
            : base(level)
        {
            this.level = level;
        }

        public Block(Level level, Texture2D texture, int x, int y, float density, float friction, float restitution, bool is_reflectable)
            : base(level, texture, x, y, density, friction, restitution, is_reflectable)
        {
            Initialize();
        }

        protected override void CreateJoints()
        {
        }

        public override void Reflect(int start, int end, Reflexio.Level.ReflectionOrientation ref_or) 
        {
            int[] new_pos = {-1, -1};
            try
            {
                new_pos = level.ContinuousToDiscrete(this.Body.Position);
            }
            catch (NullReferenceException)
            {
            };
            DiscX = new_pos[0];
            DiscY = new_pos[1];
            base.Reflect(start, end, ref_or);
            try
            {
                this.Body.ApplyForce(new Vector2(0, 0.1f), Body.Position);
            }
            catch (NullReferenceException)
            { };
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
                gameBlocks.Remove((Block)ro);
                /*if (se.State == SoundState.Playing)
                    se.Stop();*/
                SoundManager.PlaySound("destroy");
                level.Buddydeath();
            }

        }

        protected override Body CreatePhysics()
        {

            var newBody = BodyFactory.CreateBody(World, new Vector2());
            newBody.BodyType = BodyType.Dynamic;
            newBody.FixedRotation = true;
            var rectangleVertices = PolygonTools.CreateEllipse(width / 2, height / 2, 20);
            //var rectangleVertices = PolygonTools.CreateRectangle(width / 2, height / 2);
            var rectangleShape = new PolygonShape(rectangleVertices, density);
            var fixture = newBody.CreateFixture(rectangleShape);
            newBody.FixedRotation = true;
            fixture.Friction = friction;
            fixture.Restitution = restitution;
            
            return newBody;
        }

        public override void Update(Level world, float dt)
        {
            texture = normal_texture;
            if (buddyFlinchTime > 0)
            {
                texture = GameEngine.Instance.GetTexture("buddyBlockFlinch");
                buddyFlinchTime--;
            }
            else
            {
                if (prev_y_vel > 0 && this.Body.LinearVelocity.Y < 0 && prev_y_vel - this.Body.LinearVelocity.Y > 10)
                {
                    buddyFlinchTime = BLOCK_FLINCH_TIME;
                }
                if (this.Body.LinearVelocity.Y > 4.0f)
                {
                    texture = GameEngine.Instance.GetTexture("buddyBlockFall");
                }
            }

            prev_y_vel = this.Body.LinearVelocity.Y;
            base.Update(world, dt);

            if (this.Body.Position.Y - width / 2 - 2 > Level.HEIGHT)
            {
                level.Buddydeath();
            }

        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Block");
            writer.WriteAttributeString("params", "level");
            base.Serialize(writer);
            writer.WriteEndElement();
        }
    }
}
