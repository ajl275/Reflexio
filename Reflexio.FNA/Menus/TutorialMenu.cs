using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class TutorialMenu
    {
        SpecialMenuView menuView;
        int length;
        int lvlnum;
        public TutorialMenu(int j)
        {
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new SpecialMenuView((int)(Math.Min(size.X, size.Y) / 2), (int)(Math.Min(size.X, size.Y) / 3), 10);
            GameEngine g = GameEngine.Instance;
            length = 8;
            lvlnum = j;
            /*foreach (string[] s in GameEngine.level_files)
            {
                if (s[2].Equals("h"))
                    menuView.AddMenuItem(s[1], null, null);
            }
            menuView.AddMenuItem("Back", null, null);*/
            for (int i = j; i < j+8; i++)
            {
                try
                {
                    string[] s = GameEngine.level_files[i];
                    if (GameEngine.Instance.achievement_state.isLevelCompleted(i))
                        menuView.AddMenuItem(i - j, s[1], g.GetTexture(s[1] + "_small_check"), g.GetTexture(s[1] + "_bw_check"));
                    else
                        menuView.AddMenuItem(i-j, s[1], g.GetTexture(s[1]+"_small"), g.GetTexture(s[1]+"_bw"));
                }
                catch (IndexOutOfRangeException)
                {
                    length = i - j;
                    break;
                }
                
            }
            menuView.AddMenuItem(8, "Back", null, null);
            menuView.SetBackground(g.GetTexture("lvlselbkg"));
            Update();
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
            if (selected == 8)
                GameEngine.Instance.State = GameEngine.GameState.LEVEL_MENU;
            else
            {
                GameEngine.Instance.StartNewLevel(lvlnum+selected);
                GameEngine.Instance.State = GameEngine.GameState.PLAYING;
            }
            Update();

        }

        public void EscapePressed()
        {
            GameEngine.Instance.State = GameEngine.GameState.LEVEL_MENU;
        }

        public void UpPressed()
        {
            if (menuView.GetCurrent() < 4)
            {
                Update();
            }
            else if (menuView.GetCurrent() < 8)
            {
                    menuView.Goto(menuView.GetCurrent() - 4);
                    Update();
            }
            else
            {
                menuView.Goto(4);
                Update();
            }
        }

        public void DownPressed()
        {
            if (menuView.GetCurrent() < 4)
            {
                if ((menuView.GetCurrent() + 4) < length)
                {
                    menuView.Goto(menuView.GetCurrent() + 4);
                }

                Update();
            }

            else if (menuView.GetCurrent() < 8)
            {
                menuView.Goto(8);
                Update();
            }
            else
            {
                Update();
            }
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
            menuView.SetPreview(GameEngine.Instance.GetTexture(menuView.GetCurrentName()));
        }

        public void Draw()
        {
            menuView.Draw();
        }
    }
}
