/*
 * LEVEL CREATOR DETAILS:
 * 
 * INITIALIZABLE PARAMS:
 * SetTexture(string)
 * SetBufferedPosition(int, int)
 * SetFriction(float)
 * SetDensity(float)
 * SetRestitution(float)
 * SetLevel(Level)
 * 
 * CONSTRUCTOR PARAMS
 * Level
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Input;

namespace Reflexio
{
    public class Player : Reflexio.PhysicsObject
    {

        /// <summary>
        /// How much force to apply to get the dude moving
        /// </summary>
        public const float DUDE_FORCE = 20.0f;

        public const float JUMP_FORCE = -300.0f;

        /// <summary>
        /// How hard the brakes are applied to get a dude to stop moving
        /// </summary>
        public const float DUDE_DAMPING = 20.0f; // 

        /// <summary>
        /// Upper limit on dude left-right movement.  Does NOT apply to vertical movement.
        /// </summary>
        public const float DUDE_MAXSPEED = 2.0f;

        public const int JUMP_COOLDOWN = 30;

        // The player texture
        protected Texture2D texture;
        protected Texture2D jumpUp;
        protected Texture2D jumpDown;

        // buffer variables for creation.
        private float density, friction, restitution;
        public float width, height;

        private Level level;

        private float sensorWidth;
        private Vector2 sensorCenter;
        public HashSet<Fixture> sensorFixtures = new HashSet<Fixture>();
        public Fixture SensorFixture { get; private set; }
        private string sensorName;


        // Variables for controlling splitting
        public Vector2? prev_velocity;
        public Vector2? prev_position;
        public float diffY = 0; // Difference in Y position
        public float PLAYER_MAX_X_VEL = 30;
        public float PLAYER_MAX_Y_VEL = 5;

        // Variables for joining block
        Joint block_joint = null;
        Block joined_block = null;

        public bool IsGrounded { get; set; }

        public int JumpCooldown { get; set; }

        // Lets us know which direction we're facing
        public bool FacingRight { get; set; }

        // For animation
        Texture2D idle_texture;
        Texture2D reflectionTexture;
        AnimationTexture reflection_strip;
        AnimationTexture walk_strip;
        AnimationTexture idle_strip;
        int num_of_frames_reflection = 10;
        int num_of_frames_walk = 6;
        int num_of_frames_idle = 7;
        public bool is_walking = false;
        public bool has_moved = false;
        public bool pressed_enter = false;
        int time_since_changed = 0;
        int MINIMUM_AMT_OF_TIME = 5;

        public Texture2D Texture
        {
            get { return texture; }
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

        public void SetLevel(Level level)
        {
            this.level = level;
        }

        public void SetTexture(string tex_name)
        {
            this.texture = Reflexio.GameEngine.Instance.GetTexture(tex_name);
        }

        public void SetReflectionTexture(string tex_name)
        {
            this.reflectionTexture = Reflexio.GameEngine.Instance.GetTexture(tex_name);
        }

        public void SetBufferedPosition(int x, int y)
        {
            Vector2 world_pos = level.DiscreteToContinuous(x, y);
            this.BufferedPosition = new Vector2(world_pos.X + level.ROW_SCALE / 2, world_pos.Y + level.COL_SCALE / 2);
        }

        public new void Initialize()
        {
            width = level.ROW_SCALE * 0.9f;
            height = level.COL_SCALE * 0.9f;
            IsGrounded = false;
            sensorWidth = width * 0.85f;
            sensorCenter = new Vector2(0, height / 2.0f);
            sensorName = Level.dudeSensorName;
            reflection_strip = new AnimationTexture(reflectionTexture, num_of_frames_reflection, 1, 0);
            walk_strip = new AnimationTexture(this.texture, num_of_frames_walk, 1, 5);
            if (idle_texture == null)
                idle_texture = GameEngine.Instance.GetTexture("idleStrip");
            idle_strip = new AnimationTexture(this.idle_texture, num_of_frames_idle, 1, 10);
            jumpDown = GameEngine.Instance.GetTexture("jumpDown");
            jumpUp = GameEngine.Instance.GetTexture("jumpUp");
        }

        public Player(Level level)
            : base(level.World)
        {
            this.level = level;
        }

        public Player(Level level, Texture2D texture, Texture2D idle_texture, Texture2D reflectionTexture, int x, int y, float density, float friction, float restitution)
            : base(level.World)
        {
            this.texture = texture;
            this.reflectionTexture = reflectionTexture;
            this.idle_texture = idle_texture;
            this.level = level;            
            Vector2 world_pos = level.DiscreteToContinuous(x, y);
            this.BufferedPosition = new Vector2(world_pos.X + level.ROW_SCALE / 2, world_pos.Y + level.COL_SCALE/2);
            this.density = density;
            this.friction = friction;
            this.restitution = restitution;
            Initialize();
        }

        protected override void CreateJoints()
        {
        }

        public override void Reflect(int start, int end, Reflexio.Level.ReflectionOrientation ref_or) { }

        protected override Body CreatePhysics()
        {

            var newBody = BodyFactory.CreateBody(World, new Vector2());
            if (density != 0)
            {
                newBody.BodyType = BodyType.Dynamic;
            }
            var rectangleVertices = PolygonTools.CreateEllipse(width / 2, height / 2, 20);
            //var rectangleVertices = PolygonTools.CreateRectangle(width / 2, height / 2);
            var rectangleShape = new PolygonShape(rectangleVertices, density);
            var fixture = newBody.CreateFixture(rectangleShape);
            newBody.FixedRotation = true;
            fixture.Friction = friction;
            fixture.Restitution = restitution;
            
            SensorFixture = FixtureFactory.AttachRectangle(sensorWidth, 0.05f, 1f, sensorCenter, newBody, sensorName);
            SensorFixture.IsSensor = true;
            
            return newBody;
        }

        public override void Update(Level world, float dt)
        {
            // Apply cooldowns
            JumpCooldown = Math.Max(0, JumpCooldown - 1);

            if (prev_velocity.HasValue)
            {
                if (this.Body.LinearVelocity.Y < -this.PLAYER_MAX_Y_VEL)
                    this.Body.LinearVelocity = new Vector2(this.Body.LinearVelocity.X, -this.PLAYER_MAX_Y_VEL);
                Vector2 diff = this.Body.LinearVelocity - (Vector2)this.prev_velocity;
                if(System.Math.Abs(diff.X) > this.PLAYER_MAX_X_VEL)
                    this.Body.LinearVelocity = new Vector2(0, this.Body.LinearVelocity.Y);
                prev_velocity = this.Body.LinearVelocity;
            }
            else
                prev_velocity = new Vector2(this.Body.LinearVelocity.X, this.Body.LinearVelocity.Y);

            if (prev_position.HasValue)
            {
                Vector2 pp = (Vector2)prev_position;
                float diffX = this.Body.Position.X - pp.X;
                diffY = this.Body.Position.Y - pp.Y;
                if (System.Math.Abs(diffX) > level.ROW_SCALE/2)
                    this.Body.SetTransform(new Vector2(pp.X, pp.Y), this.Body.Rotation);
                /*if (diffX < 0 && Keyboard.GetState().IsKeyDown(Keys.Left)
                    this.Body.SetTransform(new Vector2(pp.X, pp.Y), this.Body.Rotation);*/
                prev_position = this.Body.Position;
            }
            else
                prev_position = new Vector2(this.Body.Position.X, this.Body.Position.Y);

            if ((Reflexio.GameEngine.Instance.currentLevel.reflection_pause_remaining_time <= 0) && (diffY > 0.01 || diffY < -0.01))
                time_since_changed++;
            else
                time_since_changed = 0;

            base.Update(world, dt);

            if (this.Body.Position.Y - width / 2 - 2 > Level.HEIGHT)
            {
                level.SetGameOver(false);
                GameEngine.Instance.achievement_state.death_by_fall(); //Achievement Logic - 'failure is an option'
            }
        }

        public override void Draw()
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            if (level.gamestate == Level.GameState.Buffer && level.hidePlayer)
                return;
            base.Draw();

            var flip = FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            var spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;

            if (Reflexio.GameEngine.Instance.currentLevel.reflection_pause_remaining_time > 0)
            {
                walk_strip.ResetCurrentFrame();
                idle_strip.ResetCurrentFrame();
                reflection_strip.Draw(spriteBatch, Level.SCALE * Body.Position, Color.White, Body.Rotation,
                                new Vector2(level.ROW_SCALE , level.COL_SCALE) * Level.SCALE, flip);
            }
            else if (diffY > 0.01 && time_since_changed > MINIMUM_AMT_OF_TIME)
            {
                reflection_strip.ResetCurrentFrame();
                idle_strip.ResetCurrentFrame();
                reflection_strip.ResetCurrentFrame();
                Vector2 origin = new Vector2(jumpDown.Width, jumpDown.Height) / 2;
                spriteBatch.Begin();
                spriteBatch.Draw(jumpDown, GameEngine.shiftAmount + Level.SCALE * Body.Position * scale, null, Color.White, Body.Rotation, origin, new Vector2(level.ROW_SCALE / jumpDown.Width, level.COL_SCALE / jumpDown.Height) * Level.SCALE * scale, flip, 0);
                spriteBatch.End();
            }
            else if (diffY < -0.01 && time_since_changed > MINIMUM_AMT_OF_TIME)
            {
                reflection_strip.ResetCurrentFrame();
                idle_strip.ResetCurrentFrame();
                reflection_strip.ResetCurrentFrame();
                Vector2 origin = new Vector2(jumpUp.Width, jumpUp.Height) / 2;
                spriteBatch.Begin();
                spriteBatch.Draw(jumpUp, GameEngine.shiftAmount + Level.SCALE * Body.Position * scale, null, Color.White, Body.Rotation, origin, new Vector2(level.ROW_SCALE / jumpUp.Width, level.COL_SCALE / jumpUp.Height) * Level.SCALE * scale, flip, 0);
                spriteBatch.End();
            }
            else if (is_walking)
            {
                reflection_strip.ResetCurrentFrame();
                idle_strip.ResetCurrentFrame();
                walk_strip.Draw(spriteBatch, Level.SCALE * Body.Position, Color.White, Body.Rotation,
                                new Vector2(level.ROW_SCALE, level.COL_SCALE) * Level.SCALE, flip);
            }
            else
            {
                walk_strip.ResetCurrentFrame();
                reflection_strip.ResetCurrentFrame();
                idle_strip.Draw(spriteBatch, Level.SCALE * Body.Position, Color.White, Body.Rotation,
                                new Vector2(level.ROW_SCALE, level.COL_SCALE) * Level.SCALE, flip);
            }
        }

        public void ToggleBlock()
        {
            if (joined_block == null)
            {
                Block[] blocks = Block.gameBlocks.ToArray<Block>();
                Block closest_block = null;
                float closest_dist = float.MaxValue;
                foreach(Block block in blocks)
                {
                    float dist = (block.Body.Position - this.Body.Position).LengthSquared();
                    if(closest_block == null)
                    {
                        if(dist < 1.0f)
                        {
                            closest_block = block;
                            closest_dist = dist;
                        }
                    }
                    else
                    {
                        if (dist < closest_dist)
                        {
                            closest_block = block;
                            closest_dist = dist;
                        }
                    }
                }
                if (closest_block != null)
                {
                    joined_block = closest_block;
                    block_joint = JointFactory.CreateSliderJoint(level.World, Body, joined_block.Body, Vector2.Zero, Vector2.Zero, level.COL_SCALE, level.COL_SCALE);
                    joined_block.SetIsReflectable(false);
                }
            }
            else
            {
                level.World.RemoveJoint(block_joint);
                joined_block.SetIsReflectable(joined_block.is_orginally_reflectable);
                joined_block = null;
            }
        }

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Player");
            writer.WriteAttributeString("params", "level");
            writer.WriteElementString("Texture", texture.Name);
            writer.WriteElementString("ReflectionTexture", reflectionTexture.Name);
            writer.WriteElementString("BufferedPosition", ((int)BufferedPosition.Value.X).ToString() + "," + ((int)BufferedPosition.Value.Y).ToString());
            writer.WriteElementString("Friction", friction.ToString());
            writer.WriteElementString("Density", density.ToString());
            writer.WriteElementString("Restitution", restitution.ToString());
            writer.WriteElementString("Initialize", "");
            writer.WriteEndElement();
        }
    }
}
