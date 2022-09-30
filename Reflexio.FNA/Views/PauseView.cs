using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reflexio
{
    class PauseView : Menu
    {
        public PauseView() : base()
        {
        }

        public PauseView(int deltaX, int deltaY, int padding)
            : base(deltaX, deltaY, padding)
        {
        }
        
        // TODO: make an actual menu
        public override void Draw()
        {
            //OldDraw();
            base.Draw();
        }

        SpriteBatch spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;
        //SpriteFont Font1 = GameEngine.Instance.Content.Load<SpriteFont>("Fonts\\font");
        Vector2 FontPos = new Vector2(GameEngine.Instance.GraphicsDevice.Viewport.Width / 2,
                GameEngine.Instance.GraphicsDevice.Viewport.Height / 2);
        public void OldDraw()
        {
            spriteBatch.Begin();
            string output = "Paused";
            //Vector2 FontOrigin = Font1.MeasureString(output) / 2;
            //spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
        }
    }
}
