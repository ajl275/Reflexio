using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class LevelMenu
    {
        MenuView menuView;
        public LevelMenu()
        {
            Vector2 size = GameEngine.Instance.GetWindowSize();
            menuView = new MenuView((int)((Math.Min(size.X, size.Y) / 2)), (int)((Math.Min(size.X, size.Y) / 3)), 20);
            GameEngine g = GameEngine.Instance;
            if (GameEngine.Instance.achievement_state.isWorldUnlocked(1))
                menuView.AddMenuItem(0, "World 1", g.GetTexture("world1On"), g.GetTexture("world1Off"), "Go to Levels of World 1");

            if (GameEngine.Instance.achievement_state.isWorldUnlocked(2))
                menuView.AddMenuItem(1, "World 2", g.GetTexture("world2On"), g.GetTexture("world2Off"), "Go to Levels of World 2");
            else
                menuView.AddMenuItem(1, "World 2", g.GetTexture("world2OnLocked"), g.GetTexture("world2OffLocked"), "World 2 is Locked. Complete at least 6 levels in World 1 to Unlock");

            if (GameEngine.Instance.achievement_state.isWorldUnlocked(3))
                menuView.AddMenuItem(2, "World 3", g.GetTexture("world3On"), g.GetTexture("world3Off"), "Go to Levels of World 3");
            else
                menuView.AddMenuItem(2, "World 3", g.GetTexture("world3OnLocked"), g.GetTexture("world3OffLocked"), "World 3 is Locked. Complete at least 6 levels in World 2 to Unlock");

            if (GameEngine.Instance.achievement_state.isWorldUnlocked(4))
                menuView.AddMenuItem(3, "World 4", g.GetTexture("world4On"), g.GetTexture("world4Off"), "Go to Levels of World 4");
            else
                menuView.AddMenuItem(3, "World 4", g.GetTexture("world4OnLocked"), g.GetTexture("world4OffLocked"), "World 4 is Locked. Complete at least 6 levels in World 3 to Unlock");

            if (GameEngine.Instance.achievement_state.isWorldUnlocked(5))
                menuView.AddMenuItem(4, "World 5", g.GetTexture("world5On"), g.GetTexture("world5Off"), "Go to Levels of World 5");
            else
                menuView.AddMenuItem(4, "World 5", g.GetTexture("world5OnLocked"), g.GetTexture("world5OffLocked"), "World 5 is Locked. Complete at least 6 levels in World 4 to Unlock");

            if (GameEngine.Instance.achievement_state.isWorldUnlocked(6))
                menuView.AddMenuItem(5, "World 6", g.GetTexture("world6On"), g.GetTexture("world6Off"), "Go to Levels of World 6");
            else
                menuView.AddMenuItem(5, "World 6", g.GetTexture("world6OnLocked"), g.GetTexture("world6OffLocked"), "World 6 is Locked. Complete at least 6 levels in World 5 to Unlock");

            if (GameEngine.Instance.achievement_state.isWorldUnlocked(7))
                menuView.AddMenuItem(5, "World 7", g.GetTexture("world7On"), g.GetTexture("world7Off"), "Go to Levels of World 7");
            else
                menuView.AddMenuItem(5, "World 7", g.GetTexture("world7OnLocked"), g.GetTexture("world7OffLocked"), "World 7 is Locked. Complete at least 6 levels in World 6 to Unlock");

            menuView.AddMenuItem(6, "Back", g.GetTexture("mainmenuOn"), g.GetTexture("mainmenuOff"), "Go back to Main Menu");
            menuView.SetBackground(g.GetTexture("mainbkg"), 4, 3, 15);
        }

        public void EnterPressed()
        {
            String selected = menuView.GetCurrentName();
            menuView.ResetCurrent();
            if (selected.Equals("World 1"))
                GameEngine.Instance.State = GameEngine.GameState.WORLD1_MENU;
            else if (selected.Equals("World 2") && GameEngine.Instance.achievement_state.isWorldUnlocked(2))
                GameEngine.Instance.State = GameEngine.GameState.WORLD2_MENU;
            else if (selected.Equals("World 3") && GameEngine.Instance.achievement_state.isWorldUnlocked(3))
                GameEngine.Instance.State = GameEngine.GameState.WORLD3_MENU;
            else if (selected.Equals("World 4") && GameEngine.Instance.achievement_state.isWorldUnlocked(4))
                GameEngine.Instance.State = GameEngine.GameState.WORLD4_MENU;
            else if (selected.Equals("World 5") && GameEngine.Instance.achievement_state.isWorldUnlocked(5))
                GameEngine.Instance.State = GameEngine.GameState.WORLD5_MENU;
            else if (selected.Equals("World 6") && GameEngine.Instance.achievement_state.isWorldUnlocked(6))
                GameEngine.Instance.State = GameEngine.GameState.WORLD6_MENU;
            else if (selected.Equals("World 7") && GameEngine.Instance.achievement_state.isWorldUnlocked(7))
                GameEngine.Instance.State = GameEngine.GameState.WORLD7_MENU;
            else if (selected.Equals("Back"))
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
