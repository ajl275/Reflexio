using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class ControlMenu
    {
        MenuView menuView;
        public ControlMenu()
        {
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new MenuView((int)((Math.Min(size.X, size.Y) / 2)), (int)((Math.Min(size.X, size.Y) / 3)), 20);
            GameEngine g = GameEngine.Instance;
            menuView.AddMenuItem(0, "Keyboard Controls", g.GetTexture("keyboardOn"), g.GetTexture("keyboardOff"));
            menuView.AddMenuItem(1, "Xbox Controls", g.GetTexture("xboxOn"), g.GetTexture("xboxOff"));
            menuView.AddMenuItem(-1, "Back", g.GetTexture("mainmenuOn"), g.GetTexture("mainmenuOff"));
            menuView.SetBackground(g.GetTexture("mainbkg"), 4, 3, 15);
        }

        public void EnterPressed()
        {
            int selected = menuView.GetCurrent();
            menuView.ResetCurrent();
            if (selected == 0)
            {
                GameEngine.Instance.State = GameEngine.GameState.PLAYING;
                GameEngine.Instance.StartNewLevel(GameEngine.KEYBOARD_CONTROLS_LEVEL_ID);
            }
            else if (selected == 1)
            {
                GameEngine.Instance.StartNewLevel(GameEngine.XBOX_CONTROLS_LEVEL_ID);
                GameEngine.Instance.State = GameEngine.GameState.PLAYING;
            }
            else if (selected == -1)
                GameEngine.Instance.State = GameEngine.GameState.MAIN_MENU;

        }

        public void EscapePressed()
        {
            GameEngine.Instance.State = GameEngine.GameState.MAIN_MENU;
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
