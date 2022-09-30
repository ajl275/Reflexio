/*
 * LEVEL CREATOR DETAILS:
 * 
 * INITIALIZABLE PARAMS:
 * SetTexture(string)
 * SetIsReflectable(bool) Default true.
 * SetX(int)
 * SetY(int)
 * SetReflectedHorizontal(bool) Default false. false = Normal Direction, true = Reflected
 * SetReflectedVertical(bool) Default false. false = Normal Direction, true = Reflected
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
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

// USE THIS WHEN YOU WANT SINUSODIAL MOVEMENT WHILE REFLECTING DIAGONALLY
//#define SINUSODIAL

namespace Reflexio
{
    public abstract class ReflectableObject : Reflexio.PhysicsObject
    {
        #region MEMBER VARIABLES
        // The texture
        protected Texture2D texture;

        public bool use_animation_texture;
        public AnimationTexture animTexture;

        // buffer variables for creation.
        protected float density, friction, restitution;
        public float width, height;

        public float Density
        {
            get { return density; }
        }

        // The discrete position of the wall
        protected int DiscX;
        protected int DiscY;

        // Start position of reflection
        protected int startX;
        protected int startY;
        protected Vector2 reflectionVelocity;
        protected Vector2 reflectionStartPosition;
        // Is the wall being reflected. For animation
        protected bool is_being_reflected = false;
        #if SINUSODIAL
            bool is_being_reflected_straight = false;
            bool is_being_reflected_diagonally = false;
        #endif

        // The level the wall belongs to
        protected Level level;

        // Is this wall reflectable?
        protected bool is_reflectable = true;
        // Direction currently facing
        protected bool reflected_horizontal = false;
        protected bool reflected_vertical = false;

        // Checks for all collision between non reflectable and reflectable objects
        protected static HashSet<ReflectableObject> non_reflectable_objects = new HashSet<ReflectableObject>();
        public bool is_inside_non_reflectable_object = false;

        // Use SpriteEffects and Negative Scaling while drawing
        protected bool use_sprite_effects = true;
        #endregion

        #region GETTERS AND SETTERS
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
            this.reflected_horizontal = b;
        }

        public void SetReflectedVertical(bool b)
        {
            this.reflected_vertical = b;
        }

        public void SetIsReflectable(bool is_reflectable)
        {
            this.is_reflectable = is_reflectable;
        }

        public bool IsReflectable()
        {
            return is_reflectable;
        }

        public void SetLevel(Level level)
        {
            this.level = level;
        }

        public void SetTexture(string tex_name)
        {
            this.texture = Reflexio.GameEngine.Instance.GetTexture(tex_name);
        }

        public int GetDiscX()
        {
            return this.DiscX;
        }

        public int GetDiscY()
        {
            return this.DiscY;
        }

        public void SetAnimationTexture(Texture2D texture, int width, int height, int delay)
        {
            animTexture = new AnimationTexture(texture, width, height, delay);
        }
        #endregion

        #region INITIALIZE AND CONSTRUCTORS
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public virtual void Initialize()
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            width = level.ROW_SCALE;
            height = level.COL_SCALE;
            Vector2 world_pos = level.DiscreteToContinuous(DiscX, DiscY);
            this.BufferedPosition = new Vector2(world_pos.X + width / 2, world_pos.Y + height / 2);
            if (!is_reflectable)
                ReflectableObject.non_reflectable_objects.Add(this);
        }

        public ReflectableObject(Level level)
            : base(level.World)
        {
            this.level = level;
        }

        public ReflectableObject(Level level, Texture2D texture, int x, int y, float density, float friction, float restitution, bool is_reflectable)
            : base(level.World)
        {
            this.texture = texture;
            this.level = level;
            DiscX = x;
            DiscY = y;
            this.is_reflectable = is_reflectable;
            this.density = density;
            this.friction = friction;
            this.restitution = restitution;
        }

        public static void FlushNonReflectableObjects()
        {
            ReflectableObject.non_reflectable_objects.Clear();
        }
        #endregion

        #region JOINTS AND PHYSICS
        protected override void CreateJoints()
        {
        }

        protected override Body CreatePhysics()
        {
            var newBody = BodyFactory.CreateBody(World, new Vector2());
            if (density != 0)
            {
                newBody.BodyType = BodyType.Dynamic;
            }
            var rectangleVertices = PolygonTools.CreateRectangle(width / 2, height / 2);
            var rectangleShape = new PolygonShape(rectangleVertices, density);
            var fixture = newBody.CreateFixture(rectangleShape);
            fixture.Friction = friction;
            fixture.Restitution = restitution;

            return newBody;
        }
        #endregion

        #region REFLECTION
        public override void Reflect(int start, int end, Reflexio.Level.ReflectionOrientation ref_or)
        {
            if (is_reflectable)
            {
                if (ref_or == Level.ReflectionOrientation.HORIZONTAL)
                {
                    if (DiscY >= start && DiscY <= end)
                    {
                        startX = DiscX;
                        startY = DiscY;
                        DiscY = (start + end) - DiscY;

                        reflectionVelocity = new Vector2((DiscX - startX) * level.ROW_SCALE, (DiscY - startY) * level.COL_SCALE) * Level.SCALE / Level.REFLECTION_PAUSE_TIME;
                        reflectionStartPosition = this.Body.Position * Level.SCALE;
                        is_being_reflected = true;
                        #if SINUSODIAL
                            is_being_reflected_straight = true;
                            is_being_reflected_diagonally = false;
                        #endif
                        this.reflected_horizontal = !this.reflected_horizontal;
                        this.Body.SetTransform(level.DiscreteToContinuousMidPoint(DiscX, DiscY, width, height), this.Body.Rotation);
                    }
                }
                else if (ref_or == Level.ReflectionOrientation.VERTICAL)
                {
                    if (DiscX >= start && DiscX <= end)
                    {
                        startX = DiscX;
                        startY = DiscY;
                        DiscX = (start + end) - DiscX;

                        reflectionVelocity = new Vector2((DiscX - startX) * level.ROW_SCALE, (DiscY - startY) * level.COL_SCALE) * Level.SCALE / Level.REFLECTION_PAUSE_TIME;
                        reflectionStartPosition = this.Body.Position * Level.SCALE;
                        is_being_reflected = true;
                        #if SINUSODIAL
                            is_being_reflected_straight = true;
                            is_being_reflected_diagonally = false;
                        #endif

                        this.reflected_vertical = !this.reflected_vertical;
                        this.Body.SetTransform(level.DiscreteToContinuousMidPoint(DiscX, DiscY, width, height), this.Body.Rotation);
                    }
                }
                else if (ref_or == Level.ReflectionOrientation.DIAGONAL)
                {
                    startX = DiscX;
                    startY = DiscY;

                    if (start == 1)
                    {

                        DiscX = DiscY;
                        DiscY = startX;
                    }
                    if (start == -1)
                    {
                        DiscX = level.NUM_COLS - DiscY - 1;
                        DiscY = level.NUM_ROWS - startX - 1;
                    }

                    reflectionVelocity = new Vector2((DiscX - startX) * level.ROW_SCALE, (DiscY - startY) * level.COL_SCALE) * Level.SCALE / Level.REFLECTION_PAUSE_TIME;
                    reflectionStartPosition = this.Body.Position * Level.SCALE;
                    is_being_reflected = true;
                    #if SINUSODIAL
                        is_being_reflected_straight = false;
                        is_being_reflected_diagonally = true;
                    #endif

                    this.reflected_horizontal = !this.reflected_horizontal;
                    this.reflected_vertical = !this.reflected_vertical;
                    this.Body.SetTransform(level.DiscreteToContinuousMidPoint(DiscX, DiscY, width, height), this.Body.Rotation);
                }

                // Check for collisions between reflectable and non-reflectable objects
                CheckForCollisionWithNonReflectableObject();
            }
        }

        public virtual void CheckForCollisionWithNonReflectableObject()
        {
            List<ReflectableObject> removeList = new List<ReflectableObject>();
            this.is_inside_non_reflectable_object = false;
            foreach (ReflectableObject ro in non_reflectable_objects)
            {
                if (ro.DiscX == this.DiscX && ro.DiscY == this.DiscY)
                {
                    this.is_inside_non_reflectable_object = true;
                    this.CollidedWithNonReflectableObject(ro);
                    removeList.Add(ro);
                }
            }
            foreach (ReflectableObject ro in removeList)
            {
                non_reflectable_objects.Remove(ro);
            }
        }

        public virtual void CollidedWithNonReflectableObject(ReflectableObject ro)
        {
        }
        #endregion

        #region DRAW
        public override void Draw()
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            base.Draw();
            Color color;
            if (is_reflectable)
                color = Color.White;
            else
                color = Color.Gray;

            var origin = new Vector2(texture.Width, texture.Height) / 2;
            var spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;

            if (level.reflection_pause_remaining_time <= 0)
            {
                #if SINUSODIAL
                    is_being_reflected_straight = false;
                    is_being_reflected_diagonally = false;
                #endif
                is_being_reflected = false;
            }

            // XNA weird thing, can't do a nonuniform negative scale (reflection through ONE axis), but can
            // do a uniform negative scale (reflection through origin)
            // SpriteEffects can reflect through one axis but can't do reflection thorugh origin
            // So if just reflected once use SpriteEffects, if rotated both on both axis use uniform negatove scale.
            SpriteEffects rotation = SpriteEffects.None;
            int reflection_scale = 1;
            if (use_sprite_effects)
            {
                if (this.reflected_horizontal && !this.reflected_vertical)
                    rotation = SpriteEffects.FlipVertically;
                else if (!this.reflected_horizontal && this.reflected_vertical)
                    rotation = SpriteEffects.FlipHorizontally;
                else if (this.reflected_horizontal && this.reflected_vertical)
                    reflection_scale = -1;
            }

            if (level.reflection_pause_remaining_time > 0 && is_being_reflected)
            {
                #if SINUSODIAL
                    Vector2 position = new Vector2();
                    if(is_being_reflected_straight)
                        position = reflectionStartPosition + reflectionVelocity * (Level.REFLECTION_PAUSE_TIME - level.reflection_pause_remaining_time);
                    else if (is_being_reflected_diagonally)
                    {
                        double angle = ((double)Level.REFLECTION_PAUSE_TIME - level.reflection_pause_remaining_time) / Level.REFLECTION_PAUSE_TIME * Math.PI / 2;
                        position = new Vector2( (DiscX - (float)Math.Cos(angle) * (DiscX - startX)) * level.ROW_SCALE, (startY - (float)Math.Sin(angle) * (startY - DiscY)) * level.COL_SCALE) * Level.SCALE; 
                    }
                #else
                    Vector2 position = reflectionStartPosition + reflectionVelocity * (Level.REFLECTION_PAUSE_TIME - level.reflection_pause_remaining_time);
                #endif
                if (use_animation_texture)
                {
                    animTexture.Draw(spriteBatch, position, color, Body.Rotation, origin,
                        reflection_scale * new Vector2(width * Level.SCALE, height * Level.SCALE),
                        rotation);
                }
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(texture, GameEngine.shiftAmount + position * scale, null, color, Body.Rotation, origin,
                                reflection_scale * new Vector2(width * Level.SCALE / texture.Width, height / texture.Height * Level.SCALE) * scale,                                
                                rotation, 0);
                    spriteBatch.End();
                }
            }
            else
            {
                if (Body != null)
                {
                    if (use_animation_texture)
                    {
                        animTexture.Draw(spriteBatch, Reflexio.Level.SCALE * Body.Position, color, Body.Rotation, origin,
                            reflection_scale * new Vector2(width * Level.SCALE, height * Level.SCALE),
                            rotation);
                    }
                    else
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, GameEngine.shiftAmount + Reflexio.Level.SCALE * Body.Position * scale, null, color, Body.Rotation, origin,
                                         reflection_scale * new Vector2(width * Level.SCALE / texture.Width, height / texture.Height * Level.SCALE) * scale,
                                         rotation, 0);
                        spriteBatch.End();
                    }
                }
            }
        }

        public void Animate()
        {
            this.use_animation_texture = true;
            animTexture.ResetCurrentFrame();
        }

        public void StopAnimate()
        {
            this.use_animation_texture = false;
        }
        #endregion

        public override void Serialize(System.Xml.XmlWriter writer)
        {
            if (texture != null)
                writer.WriteElementString("Texture", texture.Name);
            writer.WriteElementString("IsReflectable", IsReflectable().ToString());
            writer.WriteElementString("X", DiscX.ToString());
            writer.WriteElementString("Y", DiscY.ToString());
            writer.WriteElementString("ReflectedHorizontal", reflected_horizontal.ToString());
            writer.WriteElementString("ReflectedVertical", reflected_vertical.ToString());
            writer.WriteElementString("Friction", friction.ToString());
            writer.WriteElementString("Density", density.ToString());
            writer.WriteElementString("Restitution", restitution.ToString());
            writer.WriteElementString("Initialize", "");
        }
    }
}
