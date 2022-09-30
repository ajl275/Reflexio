using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Reflexio
{
    /// <summary>
    /// Controller for the player.  This contains all of the game logic for the player.
    /// </summary>
    public class PlayerController //: Controller
    {
        public enum XBoxJoyStick
        {
            None,
            Left,
            Right,
            Up,
            Down,
            Diagonal_1,
            Diagonal_neg_1
        };
        private const float DIRECTION_BOUNDARY_1 = 15f;
        private const float DIRECTION_BOUNDARY_2 = 90 - DIRECTION_BOUNDARY_1;
        private bool hasReleasedJoytsick = false;


        private readonly Level level;
        //affects jump height
        public const float JUMP_IMPULSE = -2.2f;

        // Prevent from repeated actions when pressing down the button - MAIN MENU STATE
        //private bool hasReleasedUp = false;
        //private bool hasReleasedDown = false;
        private bool hasReleasedEnter = false;
        private bool hasReleasedEscapeMainMenu = false;

        // Prevent from repeated actions when pressing down the button - PAUSE MENU STATE
        //private bool hasReleasedUp = false;
        //private bool hasReleasedDown = false;
        //private bool hasReleasedEnter = false;

        // Prevent from repeated actions when pressing down the button - GAME OVER MENU STATE
        //private bool hasReleasedUp = false;
        //private bool hasReleasedDown = false;
        //private bool hasReleasedEnter = false;

        // Prevent from repeated actions when pressing down the button
        private bool hasReleasedReflect = false;
        private bool hasReleasedUp = false;
        private bool hasReleasedDown = false;
        private bool hasReleasedLeft = false;
        private bool hasReleasedRight = false;
        private bool hasReleasedDiagonal = false;
        private bool hasReleasedPause = false;
        private bool hasReleasedBlock = false;
        private bool hasReleasedPeek = false;
        // STATIC SINCE RESTART CAN BE CONTINUED TO BE PRESSED ACROSS LEVELS
        private bool hasReleasedRestart = false;

        //private SoundEffectInstance se = GameEngine.Instance.GetMusic("reflectMusic").CreateInstance();

        public PlayerController(Level level)
        {
            this.level = level;
        }

        public void GetInput(GameEngine.GameState state)
        {
            KeyboardState ks = Keyboard.GetState();
            GamePadState gs = GamePad.GetState(PlayerIndex.One);
            
            bool ENTER_PRESSED = ks.IsKeyDown(Keys.Enter) || gs.IsButtonDown(Buttons.A);
            bool UP_PRESSED = ks.IsKeyDown(Keys.Up) || gs.IsButtonDown(Buttons.DPadUp) || gs.IsButtonDown(Buttons.LeftThumbstickUp) || gs.IsButtonDown(Buttons.RightThumbstickUp);
            bool DOWN_PRESSED = ks.IsKeyDown(Keys.Down) || gs.IsButtonDown(Buttons.DPadDown) || gs.IsButtonDown(Buttons.LeftThumbstickDown) || gs.IsButtonDown(Buttons.RightThumbstickDown);
            bool BACK_PRESSED = ks.IsKeyDown(Keys.Escape) || ks.IsKeyDown(Keys.Back) || gs.IsButtonDown(Buttons.Back) || gs.IsButtonDown(Buttons.B);
            bool LEFT_PRESSED = ks.IsKeyDown(Keys.Left) || gs.IsButtonDown(Buttons.DPadLeft) || gs.IsButtonDown(Buttons.LeftThumbstickLeft) || gs.IsButtonDown(Buttons.RightThumbstickLeft);
            bool RIGHT_PRESSED = ks.IsKeyDown(Keys.Right) || gs.IsButtonDown(Buttons.DPadRight) || gs.IsButtonDown(Buttons.LeftThumbstickRight) || gs.IsButtonDown(Buttons.RightThumbstickRight);

            switch (state)
            {
                case GameEngine.GameState.MAIN_MENU:                    
                    #region MAIN_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.mainMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.mainMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.mainMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;
                    
                    break;
                    #endregion

                case GameEngine.GameState.CONTROL_MENU:
                    #region CONTROL_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.controlMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.controlMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.controlMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.controlMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;

                    break;
                    #endregion

                case GameEngine.GameState.LEVEL_MENU:
                    #region LEVEL_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.levelMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.levelMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.levelMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.levelMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;

                    break;
                    #endregion

                case GameEngine.GameState.WORLD1_MENU:
                     #region WORLD1_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldoneMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldoneMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldoneMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldoneMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldoneMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldoneMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion
                case GameEngine.GameState.WORLD2_MENU:
                     #region WORLD2_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldtwoMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldtwoMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldtwoMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldtwoMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldtwoMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldtwoMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion
                case GameEngine.GameState.WORLD3_MENU:
                     #region WORLD3_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldthreeMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldthreeMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldthreeMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldthreeMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldthreeMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldthreeMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion
                case GameEngine.GameState.WORLD4_MENU:
                     #region WORLD4_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldfourMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldfourMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldfourMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldfourMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldfourMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldfourMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion
                case GameEngine.GameState.WORLD5_MENU:
                     #region WORLD5_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldfiveMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldfiveMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldfiveMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldfiveMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldfiveMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldfiveMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion
                case GameEngine.GameState.WORLD6_MENU:
                    #region WORLD6_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldsixMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldsixMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldsixMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldsixMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldsixMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldsixMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion

                case GameEngine.GameState.WORLD7_MENU:
                    #region WORLD7_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.worldsevenMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.worldsevenMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.worldsevenMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.worldsevenMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.worldsevenMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.worldsevenMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion

                case GameEngine.GameState.ACHIEVEMENT_MENU:
                    #region ACHIEVEMENT_MENU
                    if (ENTER_PRESSED)
                    {
                        if (!hasReleasedEnter)
                        {
                            SoundManager.PlaySound("click");
                            hasReleasedEnter = true;
                            GameEngine.Instance.achievementMenu.EnterPressed();
                        }
                    }
                    else
                        hasReleasedEnter = false;

                    if (UP_PRESSED)
                    {
                        if (!hasReleasedUp)
                        {
                            hasReleasedUp = true;
                            GameEngine.Instance.achievementMenu.UpPressed();
                        }
                    }
                    else
                        hasReleasedUp = false;

                    if (DOWN_PRESSED)
                    {
                        if (!hasReleasedDown)
                        {
                            hasReleasedDown = true;
                            GameEngine.Instance.achievementMenu.DownPressed();
                        }
                    }
                    else
                        hasReleasedDown = false;

                    if (LEFT_PRESSED)
                    {
                        if (!hasReleasedLeft)
                        {
                            hasReleasedLeft = true;
                            GameEngine.Instance.achievementMenu.LeftPressed();
                        }
                    }
                    else
                        hasReleasedLeft = false;

                    if (RIGHT_PRESSED)
                    {
                        if (!hasReleasedRight)
                        {
                            hasReleasedRight = true;
                            GameEngine.Instance.achievementMenu.RightPressed();
                        }
                    }
                    else
                        hasReleasedRight = false;

                    if (BACK_PRESSED)
                    {
                        if (!hasReleasedEscapeMainMenu)
                        {
                            hasReleasedEscapeMainMenu = true;
                            GameEngine.Instance.achievementMenu.EscapePressed();
                        }
                    }
                    else
                        hasReleasedEscapeMainMenu = false;
                    break;
                    #endregion
                case GameEngine.GameState.WIN:
                    #region Beat game
                    if (ks.GetPressedKeys().Length > 0 || gs.IsButtonDown(Buttons.Start) || gs.IsButtonDown(Buttons.A) || gs.IsButtonDown(Buttons.Back))
                    {
                        GameEngine.Instance.winMenu.EnterPressed();
                    }
                    break;
                    #endregion

                case GameEngine.GameState.PLAYING:
                    switch (this.level.gamestate)
                    {
                        case Level.GameState.Playing:
                            #region PLAYING
                            /*if (ks.IsKeyDown(Keys.V) || gs.IsButtonDown(Buttons.RightShoulder))
                            {
                                if (!hasReleasedPeek)
                                {
                                    hasReleasedPeek = true;
                                    level.is_peeking = true;
                                    level.Peek();
                                }
                            }
                            else
                            {
                                if (level.is_peeking)
                                {
                                    level.is_peeking = false;
                                    level.Peek();
                                }
                                hasReleasedPeek = false;
                            }*/

                            if (ks.IsKeyDown(Keys.R) || gs.IsButtonDown(Buttons.Back))
                            {
                                if (!hasReleasedRestart)
                                {
                                    hasReleasedRestart = true;
                                    GameEngine.Instance.StartNewLevel(GameEngine.Instance.currentLevelPos);
                                }
                            }
                            else
                                hasReleasedRestart = false;

                            if (gs.ThumbSticks.Right.X == 0 && gs.ThumbSticks.Right.Y == 0)
                                hasReleasedJoytsick = false;

                            XBoxJoyStick Direction = GetDirection(gs.ThumbSticks.Right);
                            bool MOVE_LINE_LEFT = ks.IsKeyDown(Keys.A) || gs.IsButtonDown(Buttons.DPadLeft) || Direction == XBoxJoyStick.Left;
                            bool MOVE_LINE_RIGHT = ks.IsKeyDown(Keys.D) || gs.IsButtonDown(Buttons.DPadRight) || Direction == XBoxJoyStick.Right;
                            bool MOVE_LINE_UP = ks.IsKeyDown(Keys.W) || gs.IsButtonDown(Buttons.DPadUp) || Direction == XBoxJoyStick.Up;
                            bool MOVE_LINE_DOWN = ks.IsKeyDown(Keys.S) || gs.IsButtonDown(Buttons.DPadDown) || Direction == XBoxJoyStick.Down;
                            bool MOVE_LINE_DIAGONAL = ks.IsKeyDown(Keys.X) || gs.IsButtonDown(Buttons.Y) || Direction == XBoxJoyStick.Diagonal_1 || Direction == XBoxJoyStick.Diagonal_neg_1;


                            /* REFLECTION CONTROLS */
                            if (ks.IsKeyDown(Keys.Space) || gs.IsButtonDown(Buttons.LeftTrigger) || gs.IsButtonDown(Buttons.RightTrigger))
                            {
                                if (!hasReleasedReflect) // Prevent from repeated reflections with holding down 'R'
                                {
                                    hasReleasedReflect = true;
                                    SoundManager.PlaySound("reflectMusic");
                                    this.level.Reflect();
                                    GameEngine.Instance.achievement_state.inc_reflection_count(); //Achievement logic - 'Carpal Tunnel'
                                }

                            }
                            else
                                hasReleasedReflect = false;

                            if (MOVE_LINE_LEFT)
                            {
                                if (!hasReleasedLeft && !hasReleasedJoytsick)
                                {
                                    hasReleasedLeft = true;
                                    hasReleasedJoytsick = true;
                                    if (level.vertical_lines.Count != 0)
                                        this.level.MoveReflectionLineLeft();
                                }
                            }
                            else
                                hasReleasedLeft = false;

                            if (MOVE_LINE_RIGHT)
                            {
                                if (!hasReleasedRight && !hasReleasedJoytsick)
                                {
                                    hasReleasedRight = true;
                                    hasReleasedJoytsick = true;
                                    if (level.vertical_lines.Count != 0)
                                        this.level.MoveReflectionLineRight();
                                }
                            }
                            else
                                hasReleasedRight = false;

                            if (MOVE_LINE_UP && !hasReleasedJoytsick)
                            {
                                if (!hasReleasedUp)
                                {
                                    hasReleasedUp = true;
                                    hasReleasedJoytsick = true;
                                    if (level.horizontal_lines.Count != 0)
                                        this.level.MoveReflectionLineUp();
                                }
                            }
                            else
                                hasReleasedUp = false;

                            if (MOVE_LINE_DOWN && !hasReleasedJoytsick)
                            {
                                if (!hasReleasedDown)
                                {
                                    hasReleasedDown = true;
                                    hasReleasedJoytsick = true;
                                    if (level.horizontal_lines.Count != 0)
                                    {
                                        this.level.MoveReflectionLineDown();
                                    }
                                }
                            }
                            else
                                hasReleasedDown = false;

                            if (MOVE_LINE_DIAGONAL && !hasReleasedJoytsick)
                            {
                                if (!hasReleasedDiagonal)
                                {
                                    hasReleasedDiagonal = true;
                                    hasReleasedJoytsick = true;
                                    if (level.diagonal_lines.Count != 0)
                                        this.level.ToggleDiagonalReflection();
                                }
                            }
                            else
                                hasReleasedDiagonal = false;

                            // Release Block
                            if (ks.IsKeyDown(Keys.E) || gs.IsButtonDown(Buttons.X))
                            {
                                if (!hasReleasedBlock)
                                {
                                    hasReleasedBlock = true;
                                    this.level.player.ToggleBlock();
                                }
                            }
                            else
                                hasReleasedBlock = false;

                            // make sure player has been added to world
                            if (level.player.Body != null)
                            {
                                /* MOVEMENT CONTROLS */
                                var moveForce = new Vector2();
                                var jump = false;
                                level.player.is_walking = false;
                                if ((ks.IsKeyDown(Keys.Left) && ks.IsKeyUp(Keys.Right)) || (gs.IsButtonDown(Buttons.LeftThumbstickLeft)))
                                {
                                    hasReleasedLeft = true;
                                    moveForce.X -= Player.DUDE_FORCE;
                                    level.player.is_walking = true;
                                    level.player.has_moved = true;
                                }
                                if ((ks.IsKeyDown(Keys.Right) && ks.IsKeyUp(Keys.Left)) || (gs.IsButtonDown(Buttons.LeftThumbstickRight)))
                                {
                                    hasReleasedRight = true;
                                    moveForce.X += Player.DUDE_FORCE;
                                    level.player.is_walking = true;
                                    level.player.has_moved = true;
                                }
                                if (ks.IsKeyDown(Keys.Up) || gs.IsButtonDown(Buttons.A))
                                {
                                    hasReleasedUp = true;
                                    jump = true;
                                    level.player.has_moved = true;
                                }
                                if (ks.IsKeyDown(Keys.Down))
                                {
                                    hasReleasedDown = true;
                                    level.player.has_moved = true;
                                }
                                if (ks.IsKeyDown(Keys.Right) && ks.IsKeyDown(Keys.Left))
                                    moveForce.X = 0;
                                if (ks.IsKeyDown(Keys.Enter) || ks.IsKeyDown(Keys.Back) || gs.IsButtonDown(Buttons.Back))
                                    level.player.pressed_enter = true;

                                Vector2 vel = level.player.Body.LinearVelocity;

                                // Which way are we facing
                                if (moveForce.X < 0 && level.player.FacingRight)
                                {
                                    level.player.FacingRight = false;
                                    moveForce.X = 0;
                                }
                                else if (moveForce.X > 0 && !level.player.FacingRight)
                                {
                                    level.player.FacingRight = true;
                                    moveForce.X = 0;
                                }

                                // Don't want to be moving - damp out player motion
                                if (moveForce.X == 0f)
                                {
                                    var dampForce = new Vector2(-Player.DUDE_DAMPING * vel.X, 0);
                                    level.player.Body.ApplyForce(dampForce, level.player.Body.Position);
                                }

                                // Velocity too high, clamp it
                                if (Math.Abs(vel.X) >= Player.DUDE_MAXSPEED)
                                {
                                    vel.X = Math.Sign(vel.X) * Player.DUDE_MAXSPEED;
                                    level.player.Body.LinearVelocity = vel;
                                }
                                else
                                {
                                    level.player.Body.ApplyForce(moveForce, level.player.Body.Position);
                                }

                                // Jump!
                                if (level.player.JumpCooldown == 0 && jump && level.player.IsGrounded)
                                {
                                    moveForce.Y = Player.JUMP_FORCE;
                                    /*
                                    var impulse = new Vector2(0, JUMP_IMPULSE);
                                    level.player.Body.ApplyLinearImpulse(impulse, level.player.Body.Position);*/
                                    level.player.Body.ApplyForce(moveForce, level.player.Body.Position);
                                    level.player.JumpCooldown = Player.JUMP_COOLDOWN;
                                    SoundManager.PlaySound("jumpMusic");
                                }
                            }

                            // Pause
                            if (ks.IsKeyDown(Keys.Escape) || ks.IsKeyDown(Keys.P) || gs.IsButtonDown(Buttons.Start))
                            {
                                if (!hasReleasedPause)
                                {
                                    hasReleasedPause = true;
                                    this.level.TogglePaused();
                                }
                            }
                            else
                                hasReleasedPause = false;

                            break;
                            #endregion

                        case Level.GameState.Paused:
                            #region PAUSE MENU
                            if (ks.IsKeyDown(Keys.Escape) || ks.IsKeyDown(Keys.P) || gs.IsButtonDown(Buttons.Start) || gs.IsButtonDown(Buttons.B))
                            {
                                if (!hasReleasedPause)
                                {
                                    hasReleasedPause = true;
                                    this.level.TogglePaused();
                                }
                            }
                            else
                                hasReleasedPause = false;

                            if (ks.IsKeyDown(Keys.Enter) || gs.IsButtonDown(Buttons.A))
                            {
                                if (!hasReleasedEnter)
                                {
                                    SoundManager.PlaySound("click");
                                    hasReleasedEnter = true;
                                    level.pauseMenu.EnterPressed();
                                }
                            }
                            else
                                hasReleasedEnter = false;

                            if (ks.IsKeyDown(Keys.Up) || gs.IsButtonDown(Buttons.DPadUp) || gs.IsButtonDown(Buttons.LeftThumbstickUp) || gs.IsButtonDown(Buttons.RightThumbstickUp))
                            {
                                if (!hasReleasedUp)
                                {
                                    hasReleasedUp = true;
                                    level.pauseMenu.UpPressed();
                                }
                            }
                            else
                                hasReleasedUp = false;

                            if (ks.IsKeyDown(Keys.Down) || gs.IsButtonDown(Buttons.DPadDown) || gs.IsButtonDown(Buttons.LeftThumbstickDown) || gs.IsButtonDown(Buttons.RightThumbstickDown))
                            {
                                if (!hasReleasedDown)
                                {
                                    hasReleasedDown = true;
                                    level.pauseMenu.DownPressed();
                                }
                            }
                            else
                                hasReleasedDown = false;
                            break;
                            #endregion

                        case Level.GameState.Over:
                            #region GAME OVER MENU
                            if (ks.IsKeyDown(Keys.R))
                            {
                                GameEngine.Instance.StartNewLevel(GameEngine.Instance.currentLevelPos);
                            }
                            //if (ks.IsKeyDown(Keys.N))
                            //{
                                //GameEngine.Instance.StartNextLevel();
                            //}

                            if (ks.IsKeyDown(Keys.Enter) || gs.IsButtonDown(Buttons.A))
                            {
                                if (!hasReleasedEnter)
                                {
                                    hasReleasedEnter = true;
                                    level.gameOverMenu.EnterPressed();
                                }
                            }
                            else
                                hasReleasedEnter = false;

                            if (ks.IsKeyDown(Keys.Up) || gs.IsButtonDown(Buttons.DPadUp) || gs.IsButtonDown(Buttons.LeftThumbstickUp) || gs.IsButtonDown(Buttons.RightThumbstickUp))
                            {
                                if (!hasReleasedUp)
                                {
                                    hasReleasedUp = true;
                                    level.gameOverMenu.UpPressed();
                                }
                            }
                            else
                                hasReleasedUp = false;

                            if (ks.IsKeyDown(Keys.Down) || gs.IsButtonDown(Buttons.DPadDown) || gs.IsButtonDown(Buttons.LeftThumbstickDown) || gs.IsButtonDown(Buttons.RightThumbstickDown))
                            {
                                if (!hasReleasedDown)
                                {
                                    hasReleasedDown = true;
                                    level.gameOverMenu.DownPressed();
                                }
                            }
                            else
                                hasReleasedDown = false;
                            break;
                            #endregion
                    }
                break;
            }
        }

        public static XBoxJoyStick GetDirection(Vector2 vec)
        {
            if (vec.LengthSquared() < 0.0625)
                return XBoxJoyStick.None;
            if(Math.Abs(vec.X) < 0.01)
            {
                if(vec.Y > 0)
                    return XBoxJoyStick.Up;
                else
                    return XBoxJoyStick.Down;
            }
            double angle = Math.Atan(vec.Y / vec.X) * 180 / Math.PI;
            if (vec.X > 0)
            {
                if (angle < -DIRECTION_BOUNDARY_2)
                    return XBoxJoyStick.Down;
                if (angle >= -DIRECTION_BOUNDARY_2 && angle <= -DIRECTION_BOUNDARY_1)
                    return XBoxJoyStick.Diagonal_1;
                if (Math.Abs(angle) < DIRECTION_BOUNDARY_1)
                    return XBoxJoyStick.Right;
                if (angle >= DIRECTION_BOUNDARY_1 && angle <= DIRECTION_BOUNDARY_2)
                    return XBoxJoyStick.Diagonal_neg_1;
                if (angle > DIRECTION_BOUNDARY_2)
                    return XBoxJoyStick.Up;
            }
            else
            {
                if (angle < -DIRECTION_BOUNDARY_2)
                    return XBoxJoyStick.Up;
                if (angle >= -DIRECTION_BOUNDARY_2 && angle <= -DIRECTION_BOUNDARY_1)
                    return XBoxJoyStick.Diagonal_1;
                if (Math.Abs(angle) < DIRECTION_BOUNDARY_1)
                    return XBoxJoyStick.Left;
                if (angle >= DIRECTION_BOUNDARY_1 && angle <= DIRECTION_BOUNDARY_2)
                    return XBoxJoyStick.Diagonal_neg_1;
                if (angle > DIRECTION_BOUNDARY_2)
                    return XBoxJoyStick.Down;
            }

            return XBoxJoyStick.None;
        }
    }
}