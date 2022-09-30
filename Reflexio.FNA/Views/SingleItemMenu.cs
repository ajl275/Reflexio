using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reflexio
{
    public class SingleItemMenu
    {
        struct MenuItem
        {
            public int id;
            public string short_name;
            public string full_name;
            public Texture2D texture;
            public Texture2D lock_texture;
            public string wrapped_desc;
            public Texture2D desc_texture;
        };

        Texture2D bkg;
        Texture2D right_arrow;
        Texture2D left_arrow;
        Texture2D main_menu_btn;
        Vector2 MenuOrigin;
        Vector2 arrow_origin;

        List<MenuItem> menu_items;
        private int current;
        SpriteBatch spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;
        int padding = 250;
        int deltaX = 200;
        int deltaY = 200;

        //Width of line-wrap lines
        float LINEWIDTH = 375f;

        //Image Location Coordinates
        private int IMG_X = 325;
        private int IMG_Y = 200;
        private int LEFT_ARR_X = 150;
        private int LEFT_ARR_Y = 250;
        private int RIGHT_ARR_X = 625;
        private int RIGHT_ARR_Y = 250;
        private int MENU_X = 325;
        private int MENU_Y = 575;

        private int NAME_X = 325;
        private int NAME_Y = 350;
        private int DESC_X = 325;
        private int DESC_Y = 425;


        //static SpriteFont Font1 = GameEngine.Font;

        public void Initialize()
        {
            menu_items = new List<MenuItem>();
            current = 0;
            this.left_arrow = GameEngine.Instance.GetTexture("ach_left_arrow");
            this.right_arrow = GameEngine.Instance.GetTexture("ach_right_arrow");
            this.arrow_origin = new Vector2(left_arrow.Width, left_arrow.Height);
            this.main_menu_btn = GameEngine.Instance.GetTexture("mainmenuOn");
            this.MenuOrigin = new Vector2(main_menu_btn.Width, main_menu_btn.Height) / 2;
        }
        public SingleItemMenu()
        {
            Initialize();
        }

        public SingleItemMenu(int deltaX, int deltaY, int padding)
        {
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.padding = padding;
            Initialize();
        }

        public void AddMenuItem(int id, string short_name, string name, Texture2D texture, Texture2D lock_tex, string description)
        {
            MenuItem mi = new MenuItem();
            mi.id = id;
            mi.short_name = short_name;
            mi.full_name = name;
            mi.texture = texture;
            mi.lock_texture = lock_tex;
            //mi.wrapped_desc = wrap(Font1, description, LINEWIDTH);
            string text = short_name + "_desc";
            mi.desc_texture = GameEngine.Instance.GetTexture(short_name + "_desc");
            menu_items.Add(mi);
        }

        public void SetBackground(Texture2D texture)
        {
            bkg = texture;
        }

        public void SetPaddingUp(int up)
        {
            this.padding = up;
        }

        public void SetDeltaX(int x)
        {
            this.deltaX = x;
        }

        public void SetDeltaY(int y)
        {
            this.deltaY = y;
        }

        public int GetCurrent()
        {
            return menu_items.ToArray<MenuItem>()[current].id;
        }

        public string GetCurrentName()
        {
            return menu_items.ToArray<MenuItem>()[current].full_name;
        }

        public string GetCurrentDescription()
        {
            return menu_items.ToArray<MenuItem>()[current].wrapped_desc;
        }

        public void Goto(int pos)
        {
            current = pos;
        }

        public void Next()
        {
            current = (current + 1) % menu_items.ToArray().Length;
        }

        public void ResetCurrent()
        {
            current = 0;
        }

        public void Prev()
        {
            current--;
            if (current < 0)
                current = menu_items.ToArray().Length - 1;
        }

        public string wrap(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
	        StringBuilder sb = new StringBuilder();
	        float lineWidth = 0f;
	        float spaceWidth = spriteFont.MeasureString(" ").X;
            StringBuilder line = new StringBuilder();
	        foreach (string word in words)
	        {
		        Vector2 size = spriteFont.MeasureString(word);
		        if (lineWidth + size.X < maxLineWidth)
		        {
                    line.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
		        }
		        else
		        {
                    //remove trailing space
                    line.Remove(line.Length - 1, 1);
                    lineWidth -= spaceWidth;
                    //center line
                    while (lineWidth < maxLineWidth)
                    {
                        line.Append(" ");
                        line.Insert(0, " ");
                        lineWidth += 2 * spaceWidth;
                    }
                    sb.AppendLine(line.ToString());

                    line.Clear();
                    line.Append(word + " ");
			        lineWidth = size.X + spaceWidth;
		        }
            } 
            //remove trailing space
            line.Remove(line.Length - 1, 1);
            lineWidth -= spaceWidth;
            //center line
            while (lineWidth < maxLineWidth)
            {
                line.Append(" ");
                line.Insert(0, " ");
                lineWidth += 2 * spaceWidth;
            }
            sb.AppendLine(line.ToString());
            return sb.ToString();
        }


        public virtual void Draw()
        {
            
            spriteBatch.Begin();
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650f, h / 650f);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            if (bkg != null)
                spriteBatch.Draw(bkg, new Rectangle(shiftw, shifth, (int)Math.Min(w, h), (int)Math.Min(w, h)), Color.White);

            //Currently visible achievement data
            MenuItem mi = menu_items[current];
            AchievementState ach = GameEngine.Instance.achievement_state;
            Texture2D texture;
            if (ach.is_unlocked(mi.short_name))
                texture = mi.texture;
            else
                texture = mi.lock_texture;

            //Text location vectors
            Vector2 NamePos = new Vector2(NAME_X * scale + shiftw, NAME_Y * scale + shifth);
            //Vector2 NameOrigin = Font1.MeasureString(mi.full_name) / 2;
            Vector2 ImageOrigin = new Vector2(texture.Width, texture.Height) / 2;
            Vector2 DescOrigin = new Vector2(mi.desc_texture.Width, mi.desc_texture.Height) / 2;

            Color c = Color.DarkCyan;

            //Draw Achievement Name
            //spriteBatch.DrawString(Font1, mi.full_name, new Vector2(NamePos.X + 2, NamePos.Y + 2), Color.GhostWhite, 0, NameOrigin, 1.5f * scale, SpriteEffects.None, 0);
            //spriteBatch.DrawString(Font1, mi.full_name, NamePos, c, 0, NameOrigin, 1.5f * scale, SpriteEffects.None, 0);
            //Draw Description
            //spriteBatch.DrawString(Font1, mi.wrapped_desc, new Vector2(DescPos.X + 2, DescPos.Y + 2), Color.GhostWhite, 0, DescOrigin, 1.5f * scale, SpriteEffects.None, 0);
            //spriteBatch.DrawString(Font1, mi.wrapped_desc, DescPos, c, 0, DescOrigin, 1.5f * scale, SpriteEffects.None, 0);
            spriteBatch.Draw(mi.desc_texture, new Vector2(DESC_X * scale + shiftw, DESC_Y * scale + shifth), null, Color.GhostWhite, 0, DescOrigin, scale, SpriteEffects.None,0);
            //Draw images
            spriteBatch.Draw(texture, new Vector2(IMG_X * scale + shiftw, IMG_Y * scale + shifth), null, Color.White, 0, ImageOrigin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(right_arrow, new Vector2(RIGHT_ARR_X * scale + shiftw, RIGHT_ARR_Y * scale + shifth), null, Color.White, 0, arrow_origin, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(left_arrow, new Vector2(LEFT_ARR_X * scale + shiftw, LEFT_ARR_Y * scale + shifth), null, Color.White, 0, arrow_origin, 1, SpriteEffects.None, 0);
            //Draw main menu button
            spriteBatch.Draw(main_menu_btn, new Vector2(MENU_X * scale + shiftw, MENU_Y * scale + shifth), null, Color.White, 0, MenuOrigin, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
