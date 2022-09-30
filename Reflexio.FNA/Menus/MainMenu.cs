using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class MainMenu
    {
        MenuView menuView;
        public MainMenu()
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new MenuView((int)((Math.Min(size.X, size.Y) / 2)), (int)((Math.Min(size.X, size.Y) / 3)), 20);
            GameEngine g = GameEngine.Instance;
            if(GameEngine.Instance.achievement_state.game_progress_exists)
                menuView.AddMenuItem(0, "start", g.GetTexture("continueOn"), g.GetTexture("continueOff"));
            else
                menuView.AddMenuItem(0,"start", g.GetTexture("startOn"), g.GetTexture("startOff"));
            menuView.AddMenuItem(1,"levelselect", g.GetTexture("levelselectOn"), g.GetTexture("levelselectOff"));
            menuView.AddMenuItem(3, "Controls", g.GetTexture("controlsOn"), g.GetTexture("controlsOff"));
            menuView.AddMenuItem(4, "Achievements", g.GetTexture("achievementsOn"), g.GetTexture("achievementsOff"));
            menuView.AddMenuItem(5, "Credits", g.GetTexture("creditsOn"), g.GetTexture("creditsOff"));
            menuView.AddMenuItem(-1,"exit", g.GetTexture("exitOn"), g.GetTexture("exitOff"));
            menuView.SetBackground(g.GetTexture("mainbkg"), 4, 3, 15);
        }

        public void EnterPressed()
        {
            String selected = menuView.GetCurrentName();
            menuView.ResetCurrent();
            if (selected.Equals("start"))
            {
                if (GameEngine.Instance.achievement_state.get_starting_level() >= 0)
                {
                    GameEngine.Instance.State = GameEngine.GameState.PLAYING;
                    GameEngine.Instance.StartNewLevel(GameEngine.Instance.achievement_state.get_starting_level());
                }
                else
                {
                    GameEngine.Instance.refreshMenus();
                    GameEngine.Instance.State = GameEngine.Instance.achievement_state.get_menu_world_state();
                }
            }
            else if (selected.Equals("levelselect"))
                GameEngine.Instance.State = GameEngine.GameState.LEVEL_MENU;
            else if (selected.Equals("Controls"))
                GameEngine.Instance.State = GameEngine.GameState.CONTROL_MENU;
            else if (selected.Equals("Achievements"))
                GameEngine.Instance.State = GameEngine.GameState.ACHIEVEMENT_MENU;
            else if (selected.Equals("Credits"))
            {
                GameEngine.Instance.StartNewLevel(GameEngine.CREDITS_LEVEL_ID);
                GameEngine.Instance.State = GameEngine.GameState.PLAYING;
            }
            else if (selected.Equals("exit"))
            {
                AchievementState.toSaveFile(GameEngine.Instance.achievement_state.toSaveState());
                GameEngine.Instance.Exit();
            }

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
