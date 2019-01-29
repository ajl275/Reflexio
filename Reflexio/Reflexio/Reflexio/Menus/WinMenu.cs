using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class WinMenu
    {
        MenuView menuView;
        public WinMenu()
        {
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new MenuView((int) ((Math.Min(size.X, size.Y) / 2)), (int)((Math.Min(size.X, size.Y) / 3)), 20);
            GameEngine g = GameEngine.Instance;
            menuView.AddMenuItem(0, "Congratulations!", null, null);
            menuView.AddMenuItem(1, "You Saved Joey from his kidnappers,", null, null);
            menuView.AddMenuItem(1, "the evil Jelly Aliens", null, null);
            //menuView.SetBackground(g.GetTexture("mainbkg"), 4, 3, 15);
        }

        public void EnterPressed()
        {
            GameEngine.Instance.State = GameEngine.GameState.MAIN_MENU;
        }

        public void Update()
        {
        }

        public void Draw()
        {
            menuView.Draw();
        }
    }
}
