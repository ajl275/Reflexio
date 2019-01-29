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
    public abstract class ReflectableAndOrientable: Reflexio.ReflectableObject
    {
        // The textures: Up, Right, Down, Left
        protected Texture2D[] textures;

        public enum Direction
        {
            Up, Right, Down, Left
        }

        public Direction direction = Direction.Up;

        public void SetOrientation(String or)
        {
            if (or.Equals("U"))
                this.direction = Direction.Up;
            else if (or.Equals("L"))
                this.direction = Direction.Left;
            else if (or.Equals("D"))
                this.direction = Direction.Down;
            else if (or.Equals("R"))
                this.direction = Direction.Right;
        }

        public ReflectableAndOrientable(Level level)
            : base(level)
        {
        }

        public override void Initialize()
        {
            SetTextureAccordingToDirection();
            // Dont want to use sprite effects to reflect orientable objects - very weird behaviour.
            // Better to use the logic defined below in UpdateDirection and separate
            // sprites.
            use_sprite_effects = false;
            base.Initialize();
        }

        public ReflectableAndOrientable(Level level, Texture2D[] textures, int x, int y, float density, float friction, float restitution, bool is_reflectable, Direction direction)
            : base(level, null, x, y, density, friction, restitution, is_reflectable)
        {
            this.direction = direction;
            this.textures = textures;
            this.Initialize();
        }

        #region REFLECT
        public override void Reflect(int start, int end, Level.ReflectionOrientation ref_or)
        {
            base.Reflect(start, end, ref_or);
            if ( is_reflectable && ((ref_or == Level.ReflectionOrientation.HORIZONTAL && DiscY >= start && DiscY <= end) ||
                (ref_or == Level.ReflectionOrientation.VERTICAL && DiscX >= start && DiscX <= end) ||
                ref_or == Level.ReflectionOrientation.DIAGONAL))
            {
                UpdateDirection(ref_or, start);
                SetTextureAccordingToDirection();
            }
        }

        public void UpdateDirection(Level.ReflectionOrientation ref_or, int diagonal_line_pos)
        {
            switch (this.direction)
            {
                case Direction.Up:
                    if (ref_or == Level.ReflectionOrientation.HORIZONTAL)
                        this.direction = Direction.Down;
                    else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == 1)
                        this.direction = Direction.Left;
                    else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == -1)
                        this.direction = Direction.Right;
                        break;
                case Direction.Right:
                    if (ref_or == Level.ReflectionOrientation.VERTICAL)
                            this.direction = Direction.Left;
                        else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == 1)
                            this.direction = Direction.Down;
                        else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == -1)
                            this.direction = Direction.Up;
                        break;
                case Direction.Down:
                    if (ref_or == Level.ReflectionOrientation.HORIZONTAL)
                        this.direction = Direction.Up;
                    else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == 1)
                        this.direction = Direction.Right;
                    else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == -1)
                        this.direction = Direction.Left;
                        break;
                case Direction.Left:
                        if (ref_or == Level.ReflectionOrientation.VERTICAL)
                            this.direction = Direction.Right;
                        else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == 1)
                            this.direction = Direction.Up;
                        else if (ref_or == Level.ReflectionOrientation.DIAGONAL && diagonal_line_pos == -1)
                            this.direction = Direction.Down;
                        break;
            }
        }

        public void SetTextureAccordingToDirection()
        {
            if (textures == null)
                return;
            switch (this.direction)
            {
                case Direction.Up:
                    this.texture = textures[0];
                    break;
                case Direction.Right:
                    this.texture = textures[1];
                    break;
                case Direction.Down:
                    this.texture = textures[2];
                    break;
                case Direction.Left:
                    this.texture = textures[3];
                    break;
            }
        }
        #endregion       
    }
}
