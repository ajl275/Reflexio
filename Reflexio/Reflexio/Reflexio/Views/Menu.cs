using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reflexio
{
    public class Menu
    {
        struct MenuItem
        {
            public int id;
            public string name;
            public Texture2D texture;
            public string message;
        };

        AnimationTexture animBkg;
        Texture2D preview;
        List<MenuItem> menu_items_off;
        List<MenuItem> menu_items_on;
        private int current;
        SpriteBatch spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;
        float padding = 150;
        float deltaX = 0;
        float deltaY = 0;

        static SpriteFont Font1 = GameEngine.Font;

        public void Initialize()
        {
            menu_items_off = new List<MenuItem>();
            menu_items_on = new List<MenuItem>();
            current = 0;
        }
        public Menu()
        {
            Initialize();
        }

        public Menu(float deltaX, float deltaY, float padding)
        {
            this.deltaX = deltaX;
            this.deltaY = deltaY;
            this.padding = padding;
            Initialize();
        }

        public void AddMenuItem(int id, string name, Texture2D textureOn, Texture2D textureOff, string message)
        {
            MenuItem mi = new MenuItem();
            mi.id = id;
            mi.name = name;
            mi.texture = textureOff;
            menu_items_off.Add(mi);
            mi.message = message;
            MenuItem mi2 = new MenuItem();
            mi2.id = id;
            mi2.name = name;
            mi2.texture = textureOn;
            mi2.message = message;
            menu_items_on.Add(mi2);
        }

        public void AddMenuItem(int id, string name, Texture2D textureOn, Texture2D textureOff)
        {
            MenuItem mi = new MenuItem();
            mi.id = id;
            mi.name = name;
            mi.texture = textureOff;
            menu_items_off.Add(mi);
            mi.message = null;
            MenuItem mi2 = new MenuItem();
            mi2.id = id;
            mi2.name = name;
            mi2.texture = textureOn;
            mi2.message = null;
            menu_items_on.Add(mi2);
        }

        public void SetBackground(Texture2D texture, int width, int height, int delay)
        {
            animBkg = new AnimationTexture(texture, width, height, delay);
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
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            if (animBkg != null)
                animBkg.DrawWholeScreen(spriteBatch);
            spriteBatch.Begin();
            
            float y = deltaY;
            int cnt = 0;
            foreach (MenuItem mi in menu_items_off)
            {
                Texture2D texture;
                string message = menu_items_on.ToArray<MenuItem>()[current].message; ;
                if (cnt == current)
                {
                    texture = menu_items_on.ToArray<MenuItem>()[current].texture;
                }
                else
                    texture = mi.texture;
                if (texture == null)
                {
                    Vector2 FontPos = new Vector2(deltaX, y);
                    string output = menu_items_off.ToArray<MenuItem>()[cnt].name;
                    Vector2 FontOrigin = Font1.MeasureString(output) / 2;
                    Color c = Color.DarkCyan;
                    if (cnt == current)
                    {
                        c = Color.Blue;
                        if (message != null)
                            spriteBatch.DrawString(Font1, message, new Vector2(deltaX, 20), Color.Black, 0, Font1.MeasureString(message)/2, 0.7f, SpriteEffects.None, 0);
                    }
                    spriteBatch.DrawString(Font1, output, new Vector2(FontPos.X+2, FontPos.Y+2), Color.GhostWhite, 0, FontOrigin, 1.5f, SpriteEffects.None, 0);
                    spriteBatch.DrawString(Font1, output, FontPos, c, 0, FontOrigin, 1.5f, SpriteEffects.None, 0);
                    
                    y += padding + Font1.LineSpacing;
                    cnt++;
                }
                else
                {
                    var origin = new Vector2(texture.Width, texture.Height) / 2;
                    spriteBatch.Draw(texture, new Vector2(deltaX, y)*scale + GameEngine.shiftAmount, null, Color.White, 0, origin, 1 * scale, SpriteEffects.None, 0);
                    if (cnt == current && message != null)
                    {
                        Vector2 FontOrigin = Font1.MeasureString(message) / 2;
                        spriteBatch.DrawString(Font1, message, new Vector2(deltaX, 20)*scale + GameEngine.shiftAmount, Color.Black, 0, FontOrigin, 0.7f * scale, SpriteEffects.None, 0);
                    }
                    y += padding + texture.Height;
                    cnt++;
                }
            }
            spriteBatch.End();
        }
    }
}
