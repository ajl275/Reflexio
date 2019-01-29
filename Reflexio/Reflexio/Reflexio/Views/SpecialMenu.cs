using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reflexio
{
    public class SpecialMenu
    {
        struct MenuItem
        {
            public int id;
            public string name;
            public Texture2D texture;
        };

        Texture2D bkg;
        Texture2D preview;
        List<MenuItem> menu_items_off;
        List<MenuItem> menu_items_on;
        private int current;
        SpriteBatch spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;
        int padding = 250;
        float deltaX = 200;
        float deltaY = 200;

        static SpriteFont Font1 = GameEngine.Font;

        public void Initialize()
        {
            menu_items_off = new List<MenuItem>();
            menu_items_on = new List<MenuItem>();
            current = 0;
        }
        public SpecialMenu()
        {
            Initialize();
        }

        public SpecialMenu(int deltaX, int deltaY, int padding)
        {
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.padding = padding;
            Initialize();
        }

        public void AddMenuItem(int id, string name, Texture2D textureOn, Texture2D textureOff)
        {
            MenuItem mi = new MenuItem();
            mi.id = id;
            mi.name = name;
            mi.texture = textureOff;
            menu_items_off.Add(mi);
            MenuItem mi2 = new MenuItem();
            mi2.id = id;
            mi2.name = name;
            mi2.texture = textureOn;
            menu_items_on.Add(mi2);
        }

        public void SetBackground(Texture2D texture)
        {
           bkg = texture;
        }

        public void SetPreview(Texture2D texture)
        {
            preview = texture;
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
            return menu_items_off.ToArray<MenuItem>()[current].id;
        }

        public string GetCurrentName()
        {
            return menu_items_off.ToArray<MenuItem>()[current].name;
        }

        public void Goto(int pos)
        {
            current = pos;
        }

        public void Next()
        {
            current = (current + 1) % menu_items_off.ToArray().Length;
        }

        public void ResetCurrent()
        {
            current = 0;
        }

        public void Prev()
        {
            current = current - 1;
            if (current < 0)
                current = menu_items_off.ToArray().Length - 1;
        }

        public virtual void Draw()
        {
            
            spriteBatch.Begin();
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w/650, h/650);
            int shiftw = (int)(w/2-Math.Min(w,h)/2);
            int shifth = (int)(h/2-Math.Min(w,h)/2);
            if (bkg != null)
                spriteBatch.Draw(bkg, new Rectangle(shiftw, shifth, (int)Math.Min(w,h), (int)Math.Min(w,h)), Color.White);
            if (preview != null)
            {
                spriteBatch.Draw(preview, new Vector2(175 * scale + shiftw, 25 * scale + shifth), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            int y = 425;
            int x = 150;
            int cnt = 0;
            foreach (MenuItem mi in menu_items_off)
            {
                Texture2D texture;
                if (cnt == current)
                    texture = menu_items_on.ToArray<MenuItem>()[current].texture;
                else
                    texture = mi.texture;
                if (texture == null)
                {
                    Vector2 FontPos = new Vector2(325*scale + shiftw, (y+190)*scale + shifth);
                    string output = menu_items_off.ToArray<MenuItem>()[cnt].name;
                    Vector2 FontOrigin = Font1.MeasureString(output) / 2;
                    Color c = Color.DarkCyan;
                    if (cnt == current)
                        c = Color.Blue;
                    spriteBatch.DrawString(Font1, output, new Vector2(FontPos.X + 2, FontPos.Y + 2), Color.GhostWhite, 0, FontOrigin, 1.5f*scale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Font1, output, FontPos, c, 0, FontOrigin, 1.5f*scale, SpriteEffects.None, 0);

                    y += padding + Font1.LineSpacing;
                    cnt++;
                }
                else
                {
                    Vector2 FontPos = new Vector2(325*scale + shiftw, 350*scale + shifth);
                    string output = menu_items_off.ToArray<MenuItem>()[cnt].name;
                    Vector2 FontOrigin = Font1.MeasureString(output) / 2;
                    Color c = Color.DarkCyan;
                    var origin = new Vector2(texture.Width, texture.Height) / 2;
                    if (cnt < 4)
                        spriteBatch.Draw(texture, new Vector2(x*scale + shiftw, y*scale + shifth), null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
                    else
                        spriteBatch.Draw(texture, new Vector2((x - 4 *(padding+texture.Height))*scale + shiftw, (y+110)*scale + shifth), null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
                    x += padding + texture.Height;
                    if (cnt == current)
                    {
                        Texture2D title = GameEngine.Instance.GetTexture(output + "_text");
                        Vector2 orig = new Vector2(title.Width / 2, title.Height / 2);
                        spriteBatch.Draw(title, FontPos, null, Color.White, 0, orig, scale, SpriteEffects.None, 0);
                        //spriteBatch.DrawString(Font1, output, FontPos, c, 0, FontOrigin, 1.5f, SpriteEffects.None, 0);
                    }
                    cnt++;
                }
            }
            spriteBatch.End();
        }
    }
}
