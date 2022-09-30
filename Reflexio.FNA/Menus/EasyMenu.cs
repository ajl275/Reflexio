using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class EasyMenu
    {
        MenuView menuView;
        public EasyMenu()
        {
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new MenuView((int)(size.X / 2), (int)(size.Y / 3), 10);
            GameEngine g = GameEngine.Instance;
            for (int i = 0; i < GameEngine.level_files.Length; i++)
            {
                string[] s = GameEngine.level_files[i];
                if (s[2].Equals("e"))
                    menuView.AddMenuItem(i, s[1], null, null);
            }
            menuView.AddMenuItem(-1, "Back", null, null);
            menuView.SetBackground(g.GetTexture("mainbkg"), 4, 3, 15);
            menuView.SetPreview(g.GetTexture(menuView.GetCurrentName()));
        }

        public void EnterPressed()
        {
            /*string selected = menuView.GetCurrent();
            for (int i = 0; i < GameEngine.level_files.Length; i++)
            {
                if (selected.Equals(i.ToString()))
                {
                    GameEngine.Instance.StartNewLevel(i);
                    GameEngine.Instance.State = GameEngine.GameState.PLAYING;
                }
            }
            if (selected.Equals("Back"))
                GameEngine.Instance.State = GameEngine.GameState.LEVEL_MENU;*/

            int selected = menuView.GetCurrent();
            menuView.ResetCurrent();
            if(selected == -1)
                GameEngine.Instance.State = GameEngine.GameState.LEVEL_MENU;
            else
            {
                GameEngine.Instance.StartNewLevel(selected);
                GameEngine.Instance.State = GameEngine.GameState.PLAYING;
            }

        }

        public void EscapePressed()
        {
            GameEngine.Instance.State = GameEngine.GameState.LEVEL_MENU;
        }

        public void UpPressed()
        {
            menuView.Prev();
        }

        public void DownPressed()
        {
            menuView.Next();
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
