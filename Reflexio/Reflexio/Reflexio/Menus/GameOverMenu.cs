using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class GameOverMenu
    {
        GameOverView gameOverView;
        Level level;
        public GameOverMenu(Level level)
        {
            gameOverView = new GameOverView();
            this.level = level;
            //gameOverView = new GameOverView(310, 250, 20);
            Vector2 size = GameEngine.Instance.GetWindowSize();
            gameOverView = new GameOverView((int)((Math.Min(size.X, size.Y) / 2)), (int)(Math.Min(size.X, size.Y) * 0.4), 20);
            GameEngine g = GameEngine.Instance;
            gameOverView.AddMenuItem(0, "restart", g.GetTexture("restartOn"), g.GetTexture("restartOff"));
            gameOverView.AddMenuItem(-1, "quit", g.GetTexture("quitOn"), g.GetTexture("quitOff"));
        }

        public void Draw()
        {
            gameOverView.Draw(this.level.Succeeded);
        }

        public void Update()
        {
            
        }

        public void EnterPressed()
        {
            String selected = gameOverView.GetCurrentName();
            gameOverView.ResetCurrent();
            if (selected.Equals("restart"))
                GameEngine.Instance.RestartLevel();
            else if (selected.Equals("quit"))
            {
                level.GoBackToMainMenu = true;
                level.gamestate = Level.GameState.Zip;
                GameEngine.Instance.achievement_state.reset_consecutive_fails(); //Achievement logic - 'stubbornness'
            }
        }

        public void UpPressed()
        {
            gameOverView.Prev();
        }

        public void DownPressed()
        {
            gameOverView.Next();
        }
    }
}
