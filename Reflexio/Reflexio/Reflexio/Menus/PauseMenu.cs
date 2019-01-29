using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class PauseMenu
    {
        PauseView pauseView;
        Level level;
        public PauseMenu(Level level)
        {
            pauseView = new PauseView();
            this.level = level;
            //pauseView = new PauseView(310, 250, 20);
            Vector2 size = GameEngine.Instance.GetWindowSize();
            pauseView = new PauseView((int)((Math.Min(size.X, size.Y) / 2)), (int)(Math.Min(size.X, size.Y) * 0.4), 20);
            GameEngine g = GameEngine.Instance;
            pauseView.AddMenuItem(0, "resume", g.GetTexture("resumeOn"), g.GetTexture("resumeOff"));
            pauseView.AddMenuItem(1, "restart", g.GetTexture("restartOn"), g.GetTexture("restartOff"));
            pauseView.AddMenuItem(3, "quit", g.GetTexture("quitOn"), g.GetTexture("quitOff"));
        }

        public void Draw()
        {
            if(!level.is_peeking)
                pauseView.Draw();
        }

        public void Update()
        {
        }

        public void EnterPressed()
        {
            String selected = pauseView.GetCurrentName();
            pauseView.ResetCurrent();
            if (selected.Equals("resume"))
                level.gamestate = Level.GameState.Playing;
            if (selected.Equals("restart"))
                GameEngine.Instance.RestartLevel();
            else if (selected.Equals("quit"))
            {
                level.GoBackToMainMenu = true;
                level.SetGameOver(false);
                level.gamestate = Level.GameState.Zip; 
                GameEngine.Instance.achievement_state.reset_consecutive_fails(); //Achievement logic - 'stubbornness'
            }
        }

        public void UpPressed()
        {
            pauseView.Prev();
        }

        public void DownPressed()
        {
            pauseView.Next();
        }
    }
}
