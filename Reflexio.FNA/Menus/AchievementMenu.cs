using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class AchievementMenu
    {
        SingleItemMenuView menuView;
        public AchievementMenu()
        {
            int j = 0;
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new SingleItemMenuView((int)((Math.Min(size.X, size.Y) / 2)), (int)((Math.Min(size.X, size.Y) / 3)), 10);
            GameEngine g = GameEngine.Instance;
            int i = 0;
            foreach (string[] s in AchievementState.achievements)
            {
                menuView.AddMenuItem(i, s[0], s[1], g.GetTexture(s[3]), g.GetTexture("ach_locked"), s[2]);
            }
            //menuView.AddMenuItem(8, "Back", null, null);
            menuView.SetBackground(g.GetTexture("lvlselbkg"));
            Update();
        }

        public void EnterPressed()
        {
            GameEngine.Instance.State = GameEngine.GameState.MAIN_MENU;
            Update();
        }

        public void EscapePressed()
        {
            GameEngine.Instance.State = GameEngine.GameState.MAIN_MENU;
        }

        public void UpPressed()
        {
            menuView.Next();
            Update();
        }

        public void DownPressed()
        {
            menuView.Prev();
            Update();
        }

        public void LeftPressed()
        {
            menuView.Prev();
            Update();
        }

        public void RightPressed()
        {
            menuView.Next();
            Update();
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
