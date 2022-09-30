/*
 * Animation Texture automatically displays the correct animation frame from a strip given
 * a texture and the number of frames
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;


namespace Reflexio
{
    public class AnimationTexture
    {
        public Texture2D animTexture;
        int current_frame = 0;
        int width = 0;
        int height = 0;
        int delay = 0;
        public int time = 0;
        public int num_rotations = 0;
        public bool stop_animating = false;
        public Rectangle sector;
        Vector2 origin;
        Vector2 midScreen = new Vector2(Level.WIDTH * Level.SCALE / 2, Level.HEIGHT * Level.SCALE / 2);

        public AnimationTexture(Texture2D animTexture, int width, int height, int delay)
        {
            this.animTexture = animTexture;
            this.width = width;
            this.height = height;
            this.delay = delay;
            sector = new Rectangle();
            sector.Width = animTexture.Width / width;
            sector.Height = animTexture.Height / height;
            origin = new Vector2(sector.Width / 2, sector.Height / 2);
        }

        public void ResetCurrentFrame()
        {
            current_frame = 0;
            time = 0;
            num_rotations = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color color, float rot, Vector2 scale, SpriteEffects effects)
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale2 = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            sector.X = (current_frame % width) * sector.Width;
            sector.Y = (current_frame / width) * sector.Height;
            scale.X /= sector.Width;
            scale.Y /= sector.Height;
            spriteBatch.Begin();
            spriteBatch.Draw(animTexture, GameEngine.shiftAmount + pos*scale2, sector, color, rot, origin, scale * scale2, effects, 0);
            spriteBatch.End();
            time++;
            if (!stop_animating && (delay == 0 || time % delay == 0))
            {
                current_frame = (current_frame + 1) % (width * height);
                if (current_frame == 0)
                    num_rotations++;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, Color color, float rot, Vector2 origin, Vector2 scale, SpriteEffects effects)
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale2 = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            sector.X = (current_frame % width) * sector.Width;
            sector.Y = (current_frame / width) * sector.Height;
            scale.X /= sector.Width;
            scale.Y /= sector.Height;
            spriteBatch.Begin();
            spriteBatch.Draw(animTexture, GameEngine.shiftAmount + pos*scale2, sector, color, rot, origin, scale*scale2, effects, 0);
            spriteBatch.End();
            time++;
            if (!stop_animating && (delay == 0 || time % delay == 0))
            {
                current_frame = (current_frame + 1) % (width * height);
                if (current_frame == 0)
                    num_rotations++;
            }
        }

        public void SetToLastFrame()
        {
            current_frame = (width * height) - 1;
        }

        public void DrawWholeScreen(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(Level.WIDTH * Level.SCALE, Level.HEIGHT * Level.SCALE);
            Draw(spriteBatch, midScreen, Color.White, 0, scale, SpriteEffects.None);
        }
    }
}
