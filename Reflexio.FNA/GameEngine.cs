using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Reflexio
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameEngine : Microsoft.Xna.Framework.Game
    {
        public static Vector2 shiftAmount = Vector2.Zero;

        GraphicsDeviceManager graphics;

        // The game's main state it is either in the menu, paused, or it is being played.
        public enum GameState
        {
            BOX_JELLYFISH,
            MAIN_MENU,
            CONTROL_MENU,
            LEVEL_MENU,
            WORLD1_MENU,
            WORLD2_MENU,
            WORLD3_MENU,
            WORLD4_MENU,
            WORLD5_MENU,
            WORLD6_MENU,
            WORLD7_MENU,
            ACHIEVEMENT_MENU,
            PLAYING,
            WIN
        }

        public const int KEYBOARD_CONTROLS_LEVEL_ID = -10;
        public const int XBOX_CONTROLS_LEVEL_ID = -20;
        public const int CREDITS_LEVEL_ID = -30;

        Texture2D box_jellyfish;
        int box_jellyfish_time = 0;
        float box_jellyfish_fade_current = 0;
        float box_jellyfish_fade_slope = 0;
        int BOX_JELLYFISH_FADE_START = 0;
        int BOX_JELLYFISH_FADE_END = 1;
        int BOX_JELLYFISH_FADE_IN_TIME = 60;        
        int BOX_JELLYFISH_STAY_TIME = 180;
        int BOX_JELLYFISH_FADE_OUT_TIME = 120;
        int BOX_JELLYFISH_WAIT_TIME = 60;
        String BOX_JELLYFISH_MUSIC = "box_jellyfish_music";

        private GameState state;

        public GameState State
        {
            get { return state; }
            set { state = value; }
        }
        
        public SpriteBatch spriteBatch;
        /**
         * Gets a sprite batch for drawing
         */
        public SpriteBatch SpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }

        /**
         * Gets the unique instance of GameEngine
         */
        private static GameEngine instance;
        public static GameEngine Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameEngine();
                return instance;
            }
        }

        //private static SpriteFont font;

       /* public static SpriteFont Font
        {
            get { return font; }
        }*/

        // The current level being played.
        public Level currentLevel;
        public int currentLevelPos = 0;
        public Stopwatch currentLevelStopwatch = new Stopwatch();

        // Controller
        public PlayerController control;

        // Menus
        public MainMenu mainMenu;

        public LevelMenu levelMenu;

        public TutorialMenu worldoneMenu;

        public TutorialMenu worldtwoMenu;
        
        public TutorialMenu worldthreeMenu;

        public TutorialMenu worldfourMenu;
        
        public TutorialMenu worldfiveMenu;

        public TutorialMenu worldsixMenu;

        public TutorialMenu worldsevenMenu;

        public AchievementMenu achievementMenu;

        public ControlMenu controlMenu;

        public WinMenu winMenu;
        
        // PLay the game in full screen?
        public bool full_screen = false;

        public AchievementState achievement_state;

        // Stores all the textures
        private static Dictionary<String, Texture2D> textures = new Dictionary<String, Texture2D>();
        // TEXTURE MAPPING FROM NAME TO FILE
        private static string[][] texture_files = 
        {
            //Backgrounds
            new string[] {"lvlselbkg", "Images\\Backgrounds\\LevelSelect"},
            new string[] {"bkgTexture", "Images\\Backgrounds\\BlueToast"},
            new string[] {"bkgTexture_toast", "Images\\Backgrounds\\BlueToast"},
            new string[] {"bkgTexture_happyheart", "Images\\Backgrounds\\HappyHeart"},
            new string[] {"bkgTexture_toaster", "Images\\Backgrounds\\FlyingToasters"},
            new string[] {"bkgTexture_gray", "Images\\Backgrounds\\GraySimple"},
            new string[] {"bkgTexture_yellow", "Images\\Backgrounds\\YellowSimple"},
            new string[] {"bkgTexture_soap", "Images\\Backgrounds\\Soap"},
            new string[] {"bkgTexture_greendrops", "Images\\Backgrounds\\GreenDrops"},
            new string[] {"Back", "Images\\background_small"},

            //Achievement-Related Icons
            new string[] {"ach_noob", "Images\\Achievements\\noob_img"},
            new string[] {"ach_joey", "Images\\Achievements\\still_just_a_joey"},
            new string[] {"ach_mirror", "Images\\Achievements\\mirror_master"},
            new string[] {"ach_umbrella", "Images\\Achievements\\umbrella_wizard"},
            new string[] {"ach_master", "Images\\Achievements\\master_reflecter"},
            new string[] {"ach_bear", "Images\\Achievements\\what_a_bear"},
            new string[] {"ach_tunnel", "Images\\Achievements\\carpal_tunnel"},
            new string[] {"ach_endurance", "Images\\Achievements\\endurance_img"},
            new string[] {"ach_stubbornness", "Images\\Achievements\\stubbornness_img"},
            new string[] {"ach_aplus", "Images\\Achievements\\aplus_img"},
            new string[] {"ach_eureka", "Images\\Achievements\\eureka_img"},
            new string[] {"ach_speed", "Images\\Achievements\\speed_run"},
            new string[] {"ach_failure", "Images\\Achievements\\failure_is_an_option"},
            new string[] {"ach_locked", "Images\\Achievements\\locked"},
            new string[] {"ach_left_arrow", "Images\\Achievements\\left_arrow"},
            new string[] {"ach_right_arrow", "Images\\Achievements\\right_arrow"},

            new string[] {"n00b_desc", "Images\\Achievements\\noob"},
            new string[] {"joey_desc", "Images\\Achievements\\joey"},
            new string[] {"mirror_desc", "Images\\Achievements\\mirror"},
            new string[] {"umbrella_desc", "Images\\Achievements\\umbrella"},
            new string[] {"master_desc", "Images\\Achievements\\master"},
            new string[] {"bear_desc", "Images\\Achievements\\bear"},
            new string[] {"tunnel_desc", "Images\\Achievements\\tunnel"},
            new string[] {"endurance_desc", "Images\\Achievements\\endurance"},
            new string[] {"stubbornness_desc", "Images\\Achievements\\stubbornness"},
            new string[] {"failure_desc", "Images\\Achievements\\failure"},
            new string[] {"aplus_desc", "Images\\Achievements\\aplus"},
            new string[] {"eureka_desc", "Images\\Achievements\\eureka"},
            new string[] {"speed_desc", "Images\\Achievements\\speed"},

            //Blocks
            new string[] {"block_green", "Images\\Blocks\\green"},
            new string[] {"block_orange", "Images\\Blocks\\orange"},
            new string[] {"block_purple", "Images\\Blocks\\purple"},
            new string[] {"block_red", "Images\\Blocks\\red"},
            new string[] {"block_yellow", "Images\\Blocks\\yellow"},
            new string[] {"block_blue", "Images\\Blocks\\blue"},
            new string[] {"groundTexture", "Images\\Blocks\\blue"}, //Temporary, until we allow changes

            //Menu Buttons
#region MENU_BUTTONS
            new string[] {"startOn", "Images\\Menu\\startON"},
            new string[] {"startOff", "Images\\Menu\\startOFF"},
            new string[] {"continueOn", "Images\\Menu\\continueOn"},
            new string[] {"continueOff", "Images\\Menu\\continueOff"},
            new string[] {"exitOn", "Images\\Menu\\exitON"},
            new string[] {"exitOff", "Images\\Menu\\exitOFF"},
            new string[] {"controlsOn", "Images\\Menu\\controlsON"},
            new string[] {"controlsOff", "Images\\Menu\\controlsOFF"},
            new string[] {"levelselectOn", "Images\\Menu\\levelselectON"},
            new string[] {"levelselectOff", "Images\\Menu\\levelselectOFF"},
            new string[] {"creditsOn", "Images\\Menu\\creditsON"},
            new string[] {"creditsOff", "Images\\Menu\\creditsOFF"},
            new string[] {"mainmenuOn", "Images\\Menu\\mainmenuON"},
            new string[] {"mainmenuOff", "Images\\Menu\\mainmenuOFF"},
            new string[] {"achievementsOff", "Images\\Menu\\achievementsOFF"},
            new string[] {"achievementsOn", "Images\\Menu\\achievementsON"},
            new string[] {"keyboardOn", "Images\\Menu\\keyboardON"},
            new string[] {"keyboardOff", "Images\\Menu\\keyboardOFF"},
            new string[] {"xboxOn", "Images\\Menu\\xboxON"},
            new string[] {"xboxOff", "Images\\Menu\\xboxOFF"},
            new string[] {"backOn", "Images\\Menu\\backON"},
            new string[] {"backOff", "Images\\Menu\\backOFF"},
            new string[] {"world1On", "Images\\Menu\\world1ON"},
            new string[] {"world1Off", "Images\\Menu\\world1OFF"},
            new string[] {"world2On", "Images\\Menu\\world2ON"},
            new string[] {"world2OnLocked", "Images\\Menu\\world2ON_LOCKED"},
            new string[] {"world2Off", "Images\\Menu\\world2OFF"},
            new string[] {"world2OffLocked", "Images\\Menu\\world2OFF_LOCKED"},
            new string[] {"world3On", "Images\\Menu\\world3ON"},
            new string[] {"world3OnLocked", "Images\\Menu\\world3ON_LOCKED"},
            new string[] {"world3Off", "Images\\Menu\\world3OFF"},
            new string[] {"world3OffLocked", "Images\\Menu\\world3OFF_LOCKED"},
            new string[] {"world4On", "Images\\Menu\\world4ON"},
            new string[] {"world4OnLocked", "Images\\Menu\\world4ON_LOCKED"},
            new string[] {"world4Off", "Images\\Menu\\world4OFF"},
            new string[] {"world4OffLocked", "Images\\Menu\\world4OFF_LOCKED"},
            new string[] {"world5On", "Images\\Menu\\world5ON"},
            new string[] {"world5OnLocked", "Images\\Menu\\world5ON_LOCKED"},
            new string[] {"world5Off", "Images\\Menu\\world5OFF"},
            new string[] {"world5OffLocked", "Images\\Menu\\world5OFF_LOCKED"},
            new string[] {"world6On", "Images\\Menu\\world6ON"},
            new string[] {"world6OnLocked", "Images\\Menu\\world6ON_LOCKED"},
            new string[] {"world6Off", "Images\\Menu\\world6OFF"},
            new string[] {"world6OffLocked", "Images\\Menu\\world6OFF_LOCKED"},
            new string[] {"world7On", "Images\\Menu\\world7ON"},
            new string[] {"world7OnLocked", "Images\\Menu\\world7ON_LOCKED"},
            new string[] {"world7Off", "Images\\Menu\\world7OFF"},
            new string[] {"world7OffLocked", "Images\\Menu\\world7OFF_LOCKED"},
            new string[] {"mainbkg", "Images\\Menu\\MainMenuStrip"},

            new string[] {"resumeOn", "Images\\PauseMenu\\resumeON"},
            new string[] {"resumeOff", "Images\\PauseMenu\\resumeOFF"},
            new string[] {"quitOn", "Images\\PauseMenu\\quitON"},
            new string[] {"quitOff", "Images\\PauseMenu\\quitOFF"},
            new string[] {"restartOn", "Images\\PauseMenu\\restartON"},
            new string[] {"restartOff", "Images\\PauseMenu\\restartOFF"},
#endregion

#region LEVEL_RESOURCES
            new string[] {"box_jellyfish", "Images\\boxjellyfish"},
            new string[] {"bkg_movement", "Images\\background_movement"},
            new string[] {"bkg_diagonal", "Images\\background_diagonal"},
            new string[] {"bkg_vertical", "Images\\background_vertical"},
            new string[] {"bkg_horizontal", "Images\\background_horizontal"},
            new string[] {"bkg_hor_mult", "Images\\background_hor_mult"},
            new string[] {"bkg_vert_mult", "Images\\background_vert_mult"},
            new string[] {"bkgKeyboardControls", "Images\\KeyboardControls"},
            new string[] {"bkg_credits", "Images\\credits"},
            new string[] {"bkgXboxControls", "Images\\xbox360screen"},
            new string[] {"bkg_block", "Images\\background_block"},
            new string[] {"lineTexture", "Images\\line"},
            new string[] {"playerTexture", "Images\\koala"},
            new string[] {"jumpUp", "Images\\smallJumpUp"},
            new string[] {"jumpDown", "Images\\smallJumpDown"},
            new string[] {"deathStrip", "Images\\smallDeathStrip"},
            new string[] {"keyTexture", "Images\\key"},
            new string[] {"openDoorTexture", "Images\\opendoor"},
            new string[] {"openDoorStrip", "Images\\doorOpen"},
            new string[] {"closeDoorTexture", "Images\\door"},
            new string[] {"spikesUpTexture", "Images\\spikesUp"},
            new string[] {"spikesRightTexture", "Images\\spikesRight"},
            new string[] {"spikesDownTexture", "Images\\spikesDown"},
            new string[] {"spikesLeftTexture", "Images\\spikesLeft"},
            new string[] {"idleStrip", "Images\\koala_idle"},
            new string[] {"reflectionStrip", "Images\\reflectionStrip"},
            new string[] {"buddyBlock", "Images\\smallbuddyblock"},
            new string[] {"buddyBlockFlinch", "Images\\BuddyBlockFlinch"},
            new string[] {"buddyBlockFall", "Images\\BuddyBlockFall"},
            new string[] {"reflectionCircle", "Images\\smallUmbrella"},
            new string[] {"reflectionCircleSwitch", "Images\\smallUmbrellaSwitch"},
            new string[] {"switchTexture", "Images\\switch"},
            new string[] {"switchOnTexture", "Images\\switchOn"},
            new string[] {"zip", "Images\\zipfilmstrip"},
            new string[] {"unzip", "Images\\unzipfilmstrip"},
            new string[] {"zippedbkg", "Images\\zipped"},
            new string[] {"doorEat", "Images\\doorEat"},
#endregion

#region LEVEL_SEL_BKGS
            new string[] {"Movement", "Images\\LevelPreviews\\LargePreview\\tutorial_movement"}, //Tutorial
            new string[] {"Movement_small", "Images\\LevelPreviews\\Selected\\tutorial_movement_small"}, //Tutorial
            new string[] {"Movement_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_movement_bw"}, //Tutorial
            new string[] {"Movement_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_movement_bw_check"},
            new string[] {"Movement_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_movement_small_check"},

            new string[] {"DNA", "Images\\LevelPreviews\\LargePreview\\easy_dna"}, //Easy
            new string[] {"DNA_small", "Images\\LevelPreviews\\Selected\\easy_dna_small"}, //Easy
            new string[] {"DNA_bw", "Images\\LevelPreviews\\BlackWhite\\easy_dna_bw"}, //Easy
            new string[] {"DNA_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_dna_small_check"},
            new string[] {"DNA_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_dna_bw_check"},

            new string[] {"Vert. Reflection", "Images\\LevelPreviews\\LargePreview\\tutorial_vertical"}, //Tutorial
            new string[] {"Vert. Reflection_small", "Images\\LevelPreviews\\Selected\\tutorial_vertical_small"}, //Tutorial
            new string[] {"Vert. Reflection_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_vertical_bw"}, //Tutorial
            new string[] {"Vert. Reflection_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_vertical_bw_check"},
            new string[] {"Vert. Reflection_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_vertical_small_check"},

            new string[] {"Hor. Reflection", "Images\\LevelPreviews\\LargePreview\\tutorial_horizontal"}, //Tutorial
            new string[] {"Hor. Reflection_small", "Images\\LevelPreviews\\Selected\\tutorial_horizontal_small"}, //Tutorial
            new string[] {"Hor. Reflection_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_horizontal_bw"}, //Tutorial
            new string[] {"Hor. Reflection_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_horizontal_bw_check"},
            new string[] {"Hor. Reflection_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_horizontal_small_check"},

            new string[] {"Jump & Reflect", "Images\\LevelPreviews\\LargePreview\\tutorial_jump"}, //Tutorial
            new string[] {"Jump & Reflect_small", "Images\\LevelPreviews\\Selected\\tutorial_jump_small"}, //Tutorial
            new string[] {"Jump & Reflect_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_jump_bw"}, //Tutorial
            new string[] {"Jump & Reflect_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_jump_bw_check"},
            new string[] {"Jump & Reflect_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_jump_small_check"},

            new string[] {"Flips", "Images\\LevelPreviews\\LargePreview\\easy_flips"}, //Easy
            new string[] {"Flips_small", "Images\\LevelPreviews\\Selected\\easy_flips_small"}, //Easy
            new string[] {"Flips_bw", "Images\\LevelPreviews\\BlackWhite\\easy_flips_bw"}, //Easy
            new string[] {"Flips_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_flips_bw_check"},
            new string[] {"Flips_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_flips_small_check"},
            
            new string[] {"Vert. Reflection x2", "Images\\LevelPreviews\\LargePreview\\tutorial_vertical2"}, //Tutorial
            new string[] {"Vert. Reflection x2_small", "Images\\LevelPreviews\\Selected\\tutorial_vertical2_small"}, //Tutorial
            new string[] {"Vert. Reflection x2_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_vertical2_bw"}, //Tutorial
            new string[] {"Vert. Reflection x2_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_vertical2_bw_check"},
            new string[] {"Vert. Reflection x2_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_vertical2_small_check"},

            new string[] {"Hor. Reflection x2", "Images\\LevelPreviews\\LargePreview\\tutorial_horizontal2"}, //Tutorial
            new string[] {"Hor. Reflection x2_small", "Images\\LevelPreviews\\Selected\\tutorial_horizontal2_small"}, //Tutorial
            new string[] {"Hor. Reflection x2_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_horizontal2_bw"}, //Tutorial
            new string[] {"Hor. Reflection x2_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_horizontal2_bw_check"},
            new string[] {"Hor. Reflection x2_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_horizontal2_small_check"},

            new string[] {"Foursquare", "Images\\LevelPreviews\\LargePreview\\easy_foursquare"}, // Easy
            new string[] {"Foursquare_small", "Images\\LevelPreviews\\Selected\\easy_foursquare_small"}, // Easy
            new string[] {"Foursquare_bw", "Images\\LevelPreviews\\BlackWhite\\easy_foursquare_bw"}, // Easy
            new string[] {"Foursquare_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_foursquare_bw_check"},
            new string[] {"Foursquare_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_foursquare_small_check"},

            new string[] {"Cliff", "Images\\LevelPreviews\\LargePreview\\easy_cliff"}, //Easy
            new string[] {"Cliff_small", "Images\\LevelPreviews\\Selected\\easy_cliff_small"}, //Easy
            new string[] {"Cliff_bw", "Images\\LevelPreviews\\BlackWhite\\easy_cliff_bw"}, //Easy
            new string[] {"Cliff_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_cliff_bw_check"},
            new string[] {"Cliff_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_cliff_small_check"},

            new string[] {"Fish", "Images\\LevelPreviews\\LargePreview\\easy_fish"}, //Easy
            new string[] {"Fish_small", "Images\\LevelPreviews\\Selected\\easy_fish_small"}, //Easy
            new string[] {"Fish_bw", "Images\\LevelPreviews\\BlackWhite\\easy_fish_bw"}, //Easy
            new string[] {"Fish_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_fish_bw_check"},
            new string[] {"Fish_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_fish_small_check"},

            new string[] {"Block Over Troubled Spikes", "Images\\LevelPreviews\\LargePreview\\easy_troubledspikes"}, //Easy
            new string[] {"Block Over Troubled Spikes_small", "Images\\LevelPreviews\\Selected\\easy_troubledspikes_small"}, //Easy
            new string[] {"Block Over Troubled Spikes_bw", "Images\\LevelPreviews\\BlackWhite\\easy_troubledspikes_bw"}, //Easy
            new string[] {"Block Over Troubled Spikes_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_troubledspikes_bw_check"},
            new string[] {"Block Over Troubled Spikes_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_troubledspikes_small_check"},

            new string[] {"Diag. Reflection", "Images\\LevelPreviews\\LargePreview\\tutorial_diagonal"}, //Tutorial
            new string[] {"Diag. Reflection_small", "Images\\LevelPreviews\\Selected\\tutorial_diagonal_small"}, //Tutorial
            new string[] {"Diag. Reflection_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_diagonal_bw"}, //Tutorial
            new string[] {"Diag. Reflection_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_diagonal_bw_check"},
            new string[] {"Diag. Reflection_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_diagonal_small_check"},

            new string[] {"Non-Reflectable Objects", "Images\\LevelPreviews\\LargePreview\\tutorial_nonreflectable"}, //Tutorial
            new string[] {"Non-Reflectable Objects_small", "Images\\LevelPreviews\\Selected\\tutorial_nonreflectable_small"}, //Tutorial
            new string[] {"Non-Reflectable Objects_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_nonreflectable_bw"}, //Tutorial
            new string[] {"Non-Reflectable Objects_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_nonreflectable_bw_check"},
            new string[] {"Non-Reflectable Objects_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_nonreflectable_small_check"},

            new string[] { "Destruction", "Images\\LevelPreviews\\LargePreview\\tutorial_destruction"},//Tutorial
            new string[] { "Destruction_small", "Images\\LevelPreviews\\Selected\\tutorial_destruction_small"},//Tutorial
            new string[] { "Destruction_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_destruction_bw"},//Tutorial
            new string[] {"Destruction_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_destruction_bw_check"},
            new string[] {"Destruction_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_destruction_small_check"},

            new string[] {"Block Intro", "Images\\LevelPreviews\\LargePreview\\tutorial_block"}, //Tutorial
            new string[] {"Block Intro_small", "Images\\LevelPreviews\\Selected\\tutorial_block_small"}, //Tutorial
            new string[] {"Block Intro_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_block_bw"}, //Tutorial
            new string[] {"Block Intro_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_block_bw_check"},
            new string[] {"Block Intro_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_block_small_check"},

            new string[] {"Block Warfare", "Images\\LevelPreviews\\LargePreview\\easy_blockwarfare"}, //Easy
            new string[] {"Block Warfare_small", "Images\\LevelPreviews\\Selected\\easy_blockwarfare_small"}, //Easy
            new string[] {"Block Warfare_bw", "Images\\LevelPreviews\\BlackWhite\\easy_blockwarfare_bw"}, //Easy
            new string[] {"Block Warfare_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_blockwarfare_bw_check"},
            new string[] {"Block Warfare_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_blockwarfare_small_check"},

            new string[] {"Best Buddies...", "Images\\LevelPreviews\\LargePreview\\easy_bestbuddies"}, //Easy
            new string[] {"Best Buddies..._small", "Images\\LevelPreviews\\Selected\\easy_bestbuddies_small"}, //Easy
            new string[] {"Best Buddies..._bw", "Images\\LevelPreviews\\BlackWhite\\easy_bestbuddies_bw"}, //Easy
            new string[] {"Best Buddies..._bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_bestbuddies_bw_check"},
            new string[] {"Best Buddies..._small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_bestbuddies_small_check"},

            new string[] {"Switch Intro", "Images\\LevelPreviews\\LargePreview\\tutorial_switch"}, //Tutorial
            new string[] {"Switch Intro_small", "Images\\LevelPreviews\\Selected\\tutorial_switch_small"}, //Tutorial
            new string[] {"Switch Intro_bw", "Images\\LevelPreviews\\BlackWhite\\tutorial_switch_bw"}, //Tutorial
            new string[] {"Switch Intro_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\tutorial_switch_bw_check"},
            new string[] {"Switch Intro_small_check", "Images\\LevelPreviews\\SelectedCheck\\tutorial_switch_small_check"},

            new string[] {"Switches", "Images\\LevelPreviews\\LargePreview\\easy_switches"}, //Easy
            new string[] {"Switches_small", "Images\\LevelPreviews\\Selected\\easy_switches_small"}, //Easy
            new string[] {"Switches_bw", "Images\\LevelPreviews\\BlackWhite\\easy_switches_bw"}, //Easy
            new string[] {"Switches_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_switches_bw_check"},
            new string[] {"Switches_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_switches_small_check"},

            new string[] {"Spiky Staircase", "Images\\LevelPreviews\\LargePreview\\easy_staircase"}, //Easy
            new string[] {"Spiky Staircase_small", "Images\\LevelPreviews\\Selected\\easy_staircase_small"}, //Easy
            new string[] {"Spiky Staircase_bw", "Images\\LevelPreviews\\BlackWhite\\easy_staircase_bw"}, //Easy
            new string[] {"Spiky Staircase_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_staircase_bw_check"},
            new string[] {"Spiky Staircase_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_staircase_small_check"},

            new string[] { "Ladder", "Images\\LevelPreviews\\LargePreview\\easy_ladder"}, //Easy
            new string[] { "Ladder_small", "Images\\LevelPreviews\\Selected\\easy_ladder_small"}, //Easy
            new string[] { "Ladder_bw", "Images\\LevelPreviews\\BlackWhite\\easy_ladder_bw"}, //Easy
            new string[] {"Ladder_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\easy_ladder_bw_check"},
            new string[] {"Ladder_small_check", "Images\\LevelPreviews\\SelectedCheck\\easy_ladder_small_check"},

            //Medium Levels
            new string[] {"Bigger Cliff", "Images\\LevelPreviews\\LargePreview\\medium_biggercliff"}, //Medium
            new string[] {"Bigger Cliff_small", "Images\\LevelPreviews\\Selected\\medium_biggercliff_small"}, //Medium
            new string[] {"Bigger Cliff_bw", "Images\\LevelPreviews\\BlackWhite\\medium_biggercliff_bw"}, //Medium
            new string[] {"Bigger Cliff_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_biggercliff_bw_check"}, //Medium
            new string[] {"Bigger Cliff_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_biggercliff_small_check"},

            new string[] {"Wave", "Images\\LevelPreviews\\LargePreview\\medium_wave"}, //Medium
            new string[] {"Wave_small", "Images\\LevelPreviews\\Selected\\medium_wave_small"}, //Medium
            new string[] {"Wave_bw", "Images\\LevelPreviews\\BlackWhite\\medium_wave_bw"}, //Medium
            new string[] {"Wave_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_wave_bw_check"}, //Medium
            new string[] {"Wave_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_wave_small_check"},

            new string[] {"Cube", "Images\\LevelPreviews\\LargePreview\\medium_cube"}, //Medium
            new string[] {"Cube_small", "Images\\LevelPreviews\\Selected\\medium_cube_small"}, //Medium
            new string[] {"Cube_bw", "Images\\LevelPreviews\\BlackWhite\\medium_cube_bw"}, //Medium
            new string[] {"Cube_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_cube_bw_check"}, //Medium
            new string[] {"Cube_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_cube_small_check"},

            new string[] {"Cat", "Images\\LevelPreviews\\LargePreview\\medium_cat"}, //Medium
            new string[] {"Cat_small", "Images\\LevelPreviews\\Selected\\medium_cat_small"}, //Medium
            new string[] {"Cat_bw", "Images\\LevelPreviews\\BlackWhite\\medium_cat_bw"}, //Medium
            new string[] {"Cat_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_cat_bw_check"}, //Medium
            new string[] {"Cat_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_cat_small_check"},

            new string[] {"X", "Images\\LevelPreviews\\LargePreview\\medium_x"}, //Medium
            new string[] {"X_small", "Images\\LevelPreviews\\Selected\\medium_x_small"}, //Medium
            new string[] {"X_bw", "Images\\LevelPreviews\\BlackWhite\\medium_x_bw"}, //Medium
            new string[] {"X_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_x_bw_check"}, //Medium
            new string[] {"X_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_X_small_check"},

            new string[] {"Old School", "Images\\LevelPreviews\\LargePreview\\medium_oldschool"}, //Medium
            new string[] {"Old School_small", "Images\\LevelPreviews\\Selected\\medium_oldschool_small"}, //Medium
            new string[] {"Old School_bw", "Images\\LevelPreviews\\BlackWhite\\medium_oldschool_bw"}, //Medium
            new string[] {"Old School_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_oldschool_bw_check"}, //Medium
            new string[] {"Old School_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_oldschool_small_check"},

            new string[] {"Block Ladder", "Images\\LevelPreviews\\LargePreview\\medium_blockladder"}, //Medium
            new string[] {"Block Ladder_small", "Images\\LevelPreviews\\Selected\\medium_blockladder_small"}, //Medium
            new string[] {"Block Ladder_bw", "Images\\LevelPreviews\\BlackWhite\\medium_blockladder_bw"}, //Medium
            new string[] {"Block Ladder_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_blockladder_bw_check"}, //Medium
            new string[] {"Block Ladder_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_blockladder_small_check"},

            new string[] {"Sparse", "Images\\LevelPreviews\\LargePreview\\medium_sparse"}, //Medium
            new string[] {"Sparse_bw", "Images\\LevelPreviews\\BlackWhite\\medium_sparse_bw"}, //Medium
            new string[] {"Sparse_small", "Images\\LevelPreviews\\Selected\\medium_sparse_small"}, //Medium
            new string[] {"Sparse_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_sparse_bw_check"}, //Medium
            new string[] {"Sparse_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_sparse_small_check"},

            new string[] {"Crocodile", "Images\\LevelPreviews\\LargePreview\\medium_crocodile"}, //Medium
            new string[] {"Crocodile_small", "Images\\LevelPreviews\\Selected\\medium_crocodile_small"}, //Medium
            new string[] {"Crocodile_bw", "Images\\LevelPreviews\\BlackWhite\\medium_crocodile_bw"}, //Medium
            new string[] {"Crocodile_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_crocodile_bw_check"}, //Medium
            new string[] {"Crocodile_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_crocodile_small_check"},

            new string[] {"Cramped Spaces", "Images\\LevelPreviews\\LargePreview\\medium_cramped"}, //Medium
            new string[] {"Cramped Spaces_small", "Images\\LevelPreviews\\Selected\\medium_cramped_small"}, //Medium
            new string[] {"Cramped Spaces_bw", "Images\\LevelPreviews\\BlackWhite\\medium_cramped_bw"}, //Medium
            new string[] {"Cramped Spaces_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_cramped_bw_check"}, //Medium
            new string[] {"Cramped Spaces_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_cramped_small_check"},

            new string[] {"Broken Heart", "Images\\LevelPreviews\\LargePreview\\medium_brokenheart"}, //Medium
            new string[] {"Broken Heart_bw", "Images\\LevelPreviews\\BlackWhite\\medium_brokenheart_bw"}, //Medium
            new string[] {"Broken Heart_small", "Images\\LevelPreviews\\Selected\\medium_brokenheart_small"}, //Medium
            new string[] {"Broken Heart_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_brokenheart_bw_check"}, //Medium
            new string[] {"Broken Heart_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_brokenheart_small_check"},

            //Hard Levels
            new string[] {"Butterfly", "Images\\LevelPreviews\\LargePreview\\hard_butterfly"}, //Hard
            new string[] {"Butterfly_small", "Images\\LevelPreviews\\Selected\\hard_butterfly_small"}, //Hard
            new string[] {"Butterfly_bw", "Images\\LevelPreviews\\BlackWhite\\hard_butterfly_bw"}, //Hard
            new string[] {"Butterfly_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_butterfly_bw_check"},
            new string[] {"Butterfly_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_butterfly_small_check"},

            new string[] {"LOL", "Images\\LevelPreviews\\LargePreview\\hard_lol"}, //Hard
            new string[] {"LOL_small", "Images\\LevelPreviews\\Selected\\hard_lol_small"}, //Hard
            new string[] {"LOL_bw", "Images\\LevelPreviews\\BlackWhite\\hard_lol_bw"}, //Hard
            new string[] {"LOL_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_lol_small_check"},
            new string[] {"LOL_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_lol_bw_check"}, 

            new string[] {"Dense", "Images\\LevelPreviews\\LargePreview\\hard_dense"}, //Hard
            new string[] {"Dense_small", "Images\\LevelPreviews\\Selected\\hard_dense_small"}, //Hard
            new string[] {"Dense_bw", "Images\\LevelPreviews\\BlackWhite\\hard_dense_bw"}, //Hard
            new string[] {"Dense_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_dense_bw_check"}, 
            new string[] {"Dense_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_dense_small_check"},

            new string[] {"Boxes", "Images\\LevelPreviews\\LargePreview\\hard_boxes"}, //Hard
            new string[] {"Boxes_small", "Images\\LevelPreviews\\Selected\\hard_boxes_small"}, //Hard
            new string[] {"Boxes_bw", "Images\\LevelPreviews\\BlackWhite\\hard_boxes_bw"}, //Hard
            new string[] {"Boxes_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_boxes_bw_check"}, 
            new string[] {"Boxes_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_boxes_small_check"},

            new string[] {"Butterfly v2", "Images\\LevelPreviews\\LargePreview\\hard_butterfly2"}, //Hard
            new string[] {"Butterfly v2_small", "Images\\LevelPreviews\\Selected\\hard_butterfly2_small"}, //Hard
            new string[] {"Butterfly v2_bw", "Images\\LevelPreviews\\BlackWhite\\hard_butterfly2_bw"}, //Hard
            new string[] {"Butterfly v2_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_butterfly2_bw_check"},
            new string[] {"Butterfly v2_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_butterfly2_small_check"},

            new string[] {"Mushroom", "Images\\LevelPreviews\\LargePreview\\hard_mushroom"}, //Hard
            new string[] {"Mushroom_small", "Images\\LevelPreviews\\Selected\\hard_mushroom_small"}, //Hard
            new string[] {"Mushroom_bw", "Images\\LevelPreviews\\BlackWhite\\hard_mushroom_bw"}, //Hard
            new string[] {"Mushroom_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_mushroom_bw_check"}, 
            new string[] {"Mushroom_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_mushroom_small_check"},

            new string[] {"Hardcore", "Images\\LevelPreviews\\LargePreview\\hard_hardcore"},
            new string[] {"Hardcore_small", "Images\\LevelPreviews\\Selected\\hard_hardcore_small"},
            new string[] {"Hardcore_bw", "Images\\LevelPreviews\\BlackWhite\\hard_hardcore_bw"},
            new string[] {"Hardcore_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_hardcore_bw_check"}, 
            new string[] {"Hardcore_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_hardcore_small_check"},
            
            new string[] {"The Test", "Images\\LevelPreviews\\LargePreview\\hard_test"}, //Hard
            new string[] {"The Test_small", "Images\\LevelPreviews\\Selected\\hard_test_small"}, //Hard
            new string[] {"The Test_bw", "Images\\LevelPreviews\\BlackWhite\\hard_test_bw"}, //Hard
            new string[] {"The Test_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_test_bw_check"}, 
            new string[] {"The Test_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_test_small_check"},

            new string[] {"Spider", "Images\\LevelPreviews\\LargePreview\\hard_spider"}, //Hard
            new string[] {"Spider_bw", "Images\\LevelPreviews\\BlackWhite\\hard_spider_bw"}, //Hard
            new string[] {"Spider_small", "Images\\LevelPreviews\\Selected\\hard_spider_small"}, //Hard
            new string[] {"Spider_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_spider_bw_check"}, 
            new string[] {"Spider_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_spider_small_check"},

            new string[] {"Cliffhanger", "Images\\LevelPreviews\\LargePreview\\hard_cliffhanger"}, //Hard
            new string[] {"Cliffhanger_bw", "Images\\LevelPreviews\\BlackWhite\\hard_cliffhanger_bw"}, //Hard
            new string[] {"Cliffhanger_small", "Images\\LevelPreviews\\Selected\\hard_cliffhanger_small"}, //Hard
            new string[] {"Cliffhanger_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_cliffhanger_bw_check"}, 
            new string[] {"Cliffhanger_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_cliffhanger_small_check"},

            new string[] {"Credits", "Images\\LevelPreviews\\LargePreview\\hard_credits"}, //Hard
            new string[] {"Credits_small", "Images\\LevelPreviews\\Selected\\hard_credits_small"}, //Hard
            new string[] {"Credits_bw", "Images\\LevelPreviews\\BlackWhite\\hard_credits_bw"}, //Hard
            new string[] {"Credits_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_credits_bw_check"}, 
            new string[] {"Credits_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_credits_small_check"},

            new string[] {"Bear", "Images\\LevelPreviews\\LargePreview\\hard_bear"}, //Hard
            new string[] {"Bear_small", "Images\\LevelPreviews\\Selected\\hard_bear_small"}, //Hard
            new string[] {"Bear_bw", "Images\\LevelPreviews\\BlackWhite\\hard_bear_bw"}, //Hard
            new string[] {"Bear_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_bear_bw_check"},
            new string[] {"Bear_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_bear_small_check"},

            new string[] {"Honeycomb", "Images\\LevelPreviews\\LargePreview\\hard_honeycomb"}, //Hard
            new string[] {"Honeycomb_small", "Images\\LevelPreviews\\Selected\\hard_honeycomb_small"}, //Hard
            new string[] {"Honeycomb_bw", "Images\\LevelPreviews\\BlackWhite\\hard_honeycomb_bw"}, //Hard
            new string[] {"Honeycomb_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_honeycomb_bw_check"},
            new string[] {"Honeycomb_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_honeycomb_small_check"},
            
            new string[] {"Treacherous", "Images\\LevelPreviews\\LargePreview\\hard_treacherous"}, //Hard
            new string[] {"Treacherous_small", "Images\\LevelPreviews\\Selected\\hard_treacherous_small"}, //Hard
            new string[] {"Treacherous_bw", "Images\\LevelPreviews\\BlackWhite\\hard_treacherous_bw"}, //Hard
            new string[] {"Treacherous_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_treacherous_bw_check"},
            new string[] {"Treacherous_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_treacherous_small_check"},
            
            new string[] {"Zig-Zag", "Images\\LevelPreviews\\LargePreview\\medium_zigzag"}, //Hard
            new string[] {"Zig-Zag_small", "Images\\LevelPreviews\\Selected\\medium_zigzag_small"}, //Hard
            new string[] {"Zig-Zag_bw", "Images\\LevelPreviews\\BlackWhite\\medium_zigzag_bw"}, //Hard
            new string[] {"Zig-Zag_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_zigzag_bw_check"},
            new string[] {"Zig-Zag_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_zigzag_small_check"},
            
            new string[] {"Box Maze", "Images\\LevelPreviews\\LargePreview\\medium_boxmaze"}, //Hard
            new string[] {"Box Maze_small", "Images\\LevelPreviews\\Selected\\medium_boxmaze_small"}, //Hard
            new string[] {"Box Maze_bw", "Images\\LevelPreviews\\BlackWhite\\medium_boxmaze_bw"}, //Hard
            new string[] {"Box Maze_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_boxmaze_bw_check"},
            new string[] {"Box Maze_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_boxmaze_small_check"},
            
            new string[] {"Toss", "Images\\LevelPreviews\\LargePreview\\hard_toss"}, //Hard
            new string[] {"Toss_small", "Images\\LevelPreviews\\Selected\\hard_toss_small"}, //Hard
            new string[] {"Toss_bw", "Images\\LevelPreviews\\BlackWhite\\hard_toss_bw"}, //Hard
            new string[] {"Toss_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_toss_bw_check"},
            new string[] {"Toss_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_toss_small_check"},
            
            new string[] {"Timing", "Images\\LevelPreviews\\LargePreview\\medium_timing"}, //Hard
            new string[] {"Timing_small", "Images\\LevelPreviews\\Selected\\medium_timing_small"}, //Hard
            new string[] {"Timing_bw", "Images\\LevelPreviews\\BlackWhite\\medium_timing_bw"}, //Hard
            new string[] {"Timing_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\medium_timing_bw_check"},
            new string[] {"Timing_small_check", "Images\\LevelPreviews\\SelectedCheck\\medium_timing_small_check"},
            
            new string[] {"Multi-Level", "Images\\LevelPreviews\\LargePreview\\hard_multilevel"}, //Hard
            new string[] {"Multi-Level_small", "Images\\LevelPreviews\\Selected\\hard_multilevel_small"}, //Hard
            new string[] {"Multi-Level_bw", "Images\\LevelPreviews\\BlackWhite\\hard_multilevel_bw"}, //Hard
            new string[] {"Multi-Level_bw_check", "Images\\LevelPreviews\\BlackWhiteCheck\\hard_multilevel_bw_check"},
            new string[] {"Multi-Level_small_check", "Images\\LevelPreviews\\SelectedCheck\\hard_multilevel_small_check"},

#endregion
            //Dialog Cloud
            new string[] {"dialog_cloud", "Images\\Menu\\dialogcloud"},
            new string[] {"dialog_rect", "Images\\Menu\\dialogrect"},

#region LEVEL_TITLES
            new string[] {"Bear_text", "Images\\LevelTitles\\Bear"},
            new string[] {"Best Buddies..._text", "Images\\LevelTitles\\Best Buddies"},
            new string[] {"Bigger Cliff_text", "Images\\LevelTitles\\Bigger Cliff"},
            new string[] {"Block Intro_text", "Images\\LevelTitles\\Block Intro"},
            new string[] {"Block Ladder_text", "Images\\LevelTitles\\Block Ladder"},
            new string[] {"Block Over Troubled Spikes_text", "Images\\LevelTitles\\Block Over Troubled Spikes"},
            new string[] {"Block Warfare_text", "Images\\LevelTitles\\Block Warfare"},
            new string[] {"Box Maze_text", "Images\\LevelTitles\\Box Maze"},
            new string[] {"Boxes_text", "Images\\LevelTitles\\Boxes"},
            new string[] {"Broken Heart_text", "Images\\LevelTitles\\Broken Heart"},
            new string[] {"Butterfly v2_text", "Images\\LevelTitles\\Butterfly v2"},
            new string[] {"Butterfly_text", "Images\\LevelTitles\\Butterfly"},
            new string[] {"Cat_text", "Images\\LevelTitles\\Cat"},
            new string[] {"Cliff_text", "Images\\LevelTitles\\Cliff"},
            new string[] {"Cliffhanger_text", "Images\\LevelTitles\\Cliffhanger"},
            new string[] {"Cramped Spaces_text", "Images\\LevelTitles\\Cramped Spaces"},
            new string[] {"Credits_text", "Images\\LevelTitles\\Credits"},
            new string[] {"Crocodile_text", "Images\\LevelTitles\\Crocodile"},
            new string[] {"Cube_text", "Images\\LevelTitles\\Cube"},
            new string[] {"Dense_text", "Images\\LevelTitles\\Dense"},
            new string[] {"Destruction_text", "Images\\LevelTitles\\Destruction"},
            new string[] {"Diag. Reflection_text", "Images\\LevelTitles\\Diag. Reflection"},
            new string[] {"DNA_text", "Images\\LevelTitles\\DNA"},
            new string[] {"Fish_text", "Images\\LevelTitles\\Fish"},
            new string[] {"Flips_text", "Images\\LevelTitles\\Flips"},
            new string[] {"Foursquare_text", "Images\\LevelTitles\\Foursquare"},
            new string[] {"Hardcore_text", "Images\\LevelTitles\\Hardcore"},
            new string[] {"Honeycomb_text", "Images\\LevelTitles\\Honeycomb"},
            new string[] {"Hor. Reflection x2_text", "Images\\LevelTitles\\Hor. Reflection x2"},
            new string[] {"Hor. Reflection_text", "Images\\LevelTitles\\Hor. Reflection"},
            new string[] {"Jump & Reflect_text", "Images\\LevelTitles\\Jump & Reflect"},
            new string[] {"Ladder_text", "Images\\LevelTitles\\Ladder"},
            new string[] {"LOL_text", "Images\\LevelTitles\\LOL"},
            new string[] {"Movement_text", "Images\\LevelTitles\\Movement"},
            new string[] {"Multi-Level_text", "Images\\LevelTitles\\Multi-Level"},
            new string[] {"Mushroom_text", "Images\\LevelTitles\\Mushroom"},
            new string[] {"Non-Reflectable Objects_text", "Images\\LevelTitles\\Non-Reflectable Objects"},
            new string[] {"Old School_text", "Images\\LevelTitles\\Old School"},
            new string[] {"Sparse_text", "Images\\LevelTitles\\Sparse"},
            new string[] {"Spider_text", "Images\\LevelTitles\\Spider"},
            new string[] {"Spiky Staircase_text", "Images\\LevelTitles\\Spiky Staircase"},
            new string[] {"Switch Intro_text", "Images\\LevelTitles\\Switch Intro"},
            new string[] {"Switches_text", "Images\\LevelTitles\\Switches"},
            new string[] {"The Test_text", "Images\\LevelTitles\\The Test"},
            new string[] {"Timing_text", "Images\\LevelTitles\\Timing"},
            new string[] {"Toss_text", "Images\\LevelTitles\\Toss"},
            new string[] {"Treacherous_text", "Images\\LevelTitles\\Treacherous"},
            new string[] {"Vert. Reflection x2_text", "Images\\LevelTitles\\Vert. Reflection x2"},
            new string[] {"Vert. Reflection_text", "Images\\LevelTitles\\Vert. Reflection"},
            new string[] {"Wave_text", "Images\\LevelTitles\\Wave"},
            new string[] {"X_text", "Images\\LevelTitles\\X"},
            new string[] {"Zig-Zag_text", "Images\\LevelTitles\\Zig-Zag"},


#endregion

        };
        // Background Texture
        public Texture2D bkg;

        // Stores all the music
        private static Dictionary<String, SoundEffect> music = new Dictionary<String, SoundEffect>();
        // MUSIC MAPPING FROM NAME TO FILE
        private static string[][] music_files = 
        {
            new string[] {"box_jellyfish_music", "Musics\\box_jellyfish_music"},
            new string[] {"PolkaPanda", "Musics\\Polka_Panda"},
            new string[] {"Sleuth", "Musics\\Sleuth"},
            new string[] {"ToyBlocks", "Musics\\Toy Blocks"},
            new string[] {"PlayAllDay", "Musics\\Play_all_day"},
            new string[] {"cityA", "Musics\\Polka_Panda"},
            new string[] {"cityC", "Musics\\Sleuth"},
            new string[] {"cityB", "Musics\\Toy Blocks"},
            new string[] {"meadows", "Musics\\Play_all_day"},
            new string[] {"teardrop", "Musics\\Sleuth"},
            new string[] {"winter", "Musics\\Toy Blocks"},
            new string[] {"jumpMusic", "Musics\\Jump - SoundBible.com"},
            new string[] {"reflectMusic", "Musics\\swoosh_by_qubodup"},
            new string[] {"switchActive", "Musics\\womph_by_junggle"},
            new string[] {"zipperSlow", "Musics\\zipper_unzip_slowly_by_rutgermuller"},
            new string[] {"zipperFast", "Musics\\zipper_by_anton"},
            new string[] {"dunDunDun", "Musics\\dun-dun-dun_by_simon-lacelle"},
            new string[] {"destroy", "Musics\\crumple_by_j1987"},
            new string[] {"click", "Musics\\click_by_tictacshutup"}
        };
        
        //public SoundEffectInstance bkg_sound;
        //public String current_bkg_sound;

#region LEVEL_XML

        // LEVEL MAPPING FROM NAME TO FILE
        // Add a level => new string{"<Level_File_Name>", "<Level_Display_Name>", "<type_of_level>"},
        // where type of level is t for tutorial, e for easy, m for medium and h for hard
        public static string[][] level_files = 
        {   
            //Easy + Tutorial Levels
            new string[] {"tutorial_movement.xml", "Movement", "t"}, //Tutorial
            new string[] {"easy_dna.xml", "DNA", "e"},
            new string[] {"tutorial_vertical.xml", "Vert. Reflection", "t"}, //Tutorial
            new string[] {"tutorial_horizontal.xml", "Hor. Reflection", "t"}, //Tutorial
            new string[] {"tutorial_jump_reflect.xml", "Jump & Reflect", "t"}, //Tutorial
            new string[] {"flips.xml", "Flips", "e"},
            new string[] {"tutorial_vertical_multiple.xml", "Vert. Reflection x2", "t"}, //Tutorial
            new string[] {"tutorial_horizontal_multiple.xml", "Hor. Reflection x2", "t"}, //Tutorial
            
            new string[] {"boxes.xml", "Foursquare", "e"},
            new string[] {"easyfall.xml", "Cliff", "e"},
            new string[] {"easy_fish.xml", "Fish", "e"},
            new string[] {"easy_2.xml",  "Block Over Troubled Spikes", "e"},
            new string[] {"tutorial_diagonal.xml", "Diag. Reflection", "t"}, //Tutorial
            new string[] {"tutorial_nar_items.xml", "Non-Reflectable Objects", "t"}, //Tutorial
            new string[] {"tutorial_destruction.xml", "Destruction", "t"},//Tutorial
            new string[] {"block_intro.xml", "Block Intro", "t"}, //Tutorial
            
            new string[] {"block_destruction.xml", "Block Warfare", "e"},
            new string[] {"best_buddies.xml", "Best Buddies...", "e"},
            new string[] {"switch_intro.xml", "Switch Intro", "t"}, //Tutorial
            new string[] {"easy_switches.xml", "Switches", "e"},
            new string[] {"diagonal_spikes.xml", "Spiky Staircase", "e"},
            new string[] {"medium_1.xml", "Ladder", "e"},
            //Medium Levels
            new string[] {"cliff.xml", "Bigger Cliff", "m"},
            new string[] {"easy_wave.xml", "Wave", "m"},
            
            new string[] {"easy_cube.xml", "Cube", "m"},
            new string[] {"box_maze.xml", "Box Maze", "m"},
            new string[] {"medium_cat.xml", "Cat", "m"},
            new string[] {"easy_x.xml", "X", "m"},
            new string[] {"temp.xml", "Old School", "m"},
            new string[] {"timing.xml", "Timing", "h"},
            new string[] {"block_pickup.xml", "Block Ladder", "m"},
            new string[] {"sparse.xml", "Sparse", "m"},
            
            new string[] {"medium_crocodile.xml", "Crocodile", "m"},
            new string[] {"zigzag.xml", "Zig-Zag", "m"},
            new string[] {"easy_crampedspaces.xml", "Cramped Spaces", "m"},
            new string[] {"broken_heart.xml", "Broken Heart", "m"},
            //Hard Levels
            new string[] {"easy_butterfly.xml", "Butterfly", "h"},
            new string[] {"honeycomb.xml", "Honeycomb", "h"},
            new string[] {"lol.xml", "LOL", "h"},
            new string[] {"multilevel.xml", "Multi-Level", "h"},
            
            new string[] {"toss.xml", "Toss", "h"},
            new string[] {"dense.xml", "Dense", "h"},
            new string[] {"boxes_2.xml", "Boxes", "h"},
            new string[] {"treacherous.xml", "Treacherous", "h"},
            new string[] {"hardcore.xml", "Hardcore", "h"},
            new string[] {"cliffhanger.xml", "Cliffhanger", "h"},
            new string[] {"medium_mushroom.xml", "Mushroom", "h"},
            new string[] {"the_test.xml", "The Test", "h"},


            new string[] {"hard_spider.xml", "Spider", "h"},
            new string[] {"medium_butterfly.xml", "Butterfly v2", "h"},
            new string[] {"hard_bear.xml", "Bear", "h"},
        };
#endregion

        public int MAX_LEVEL_NUM = level_files.Length - 1;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //level_files = ToArrayArray(File.ReadAllLines("Content\\Levels\\levels.txt"));
            UpdateShiftAmount();
            achievement_state = AchievementState.fromSaveFile(); //Achievement logic - setup

            //font = GameEngine.Instance.Content.Load<SpriteFont>("Fonts\\DialogFont");

            /* INITIALIZE ALL THE TEXTURES */
            foreach (string[] texture in texture_files)
            {
                Texture2D temp = Reflexio.GameEngine.Instance.Content.Load<Texture2D>(texture[1]);
                temp.Name = texture[0];
                textures.Add(texture[0], temp);
            }
            foreach (string[] m in music_files)
            {
                music.Add(m[0], Reflexio.GameEngine.Instance.Content.Load<SoundEffect>(m[1]));
            }
            state = GameState.BOX_JELLYFISH;
            //state = GameState.MAIN_MENU;
            bkg = GetTexture("bkgTexture");
            box_jellyfish = GetTexture("box_jellyfish");
            if (full_screen)
                this.SetFullScreen();

            SoundManager.SetVolume(1.0f);
            SoundManager.SetBkgVolume(1.0f);
            base.Initialize();
        }

        public Texture2D GetTexture(string tex_name)
        {
            try
            {
                return (Texture2D)(textures[tex_name]);
            }
            catch(KeyNotFoundException)
            {
                return null;
            }
        }

        public SoundEffect GetMusic(string music_name)
        {
            return (SoundEffect)(music[music_name]);
        }

        public void ResizeWindow(int width, int height)
        {
            //if (!full_screen)
            //{
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.ApplyChanges();
            //}
        }

        public void SetFullScreen()
        {
            if (full_screen && !graphics.IsFullScreen)
            {
                graphics.ToggleFullScreen();
                UpdateShiftAmount();
            }
        }

        public void UpdateShiftAmount()
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w/650, h/650);
            int shiftw = (int)(w/2-Math.Min(w,h)/2);
            int shifth = (int)(h/2-Math.Min(w,h)/2);
            Vector2 size = new Vector2(graphics.GraphicsDevice.ScissorRectangle.Width, graphics.GraphicsDevice.ScissorRectangle.Height);
            shiftAmount = new Vector2(shiftw, shifth);
        }

        public Vector2 GetWindowSize()
        {
            //return new Vector2 (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            return new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
        }
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //polygonDrawer = new PolygonDrawer(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);
            StartNewLevel(0);
        }

        public void PlayBkgSound(String sound)
        {
            SoundManager.PlayBkgSound(sound);
        }

        public void PlayBkgSoundOnce(String sound)
        {
            SoundManager.PlayBkgSoundOnce(sound);
        }

        public void refreshMenus()
        {
            mainMenu = new MainMenu();
            worldoneMenu = new TutorialMenu(0);
            worldtwoMenu = new TutorialMenu(8);
            worldthreeMenu = new TutorialMenu(16);
            worldfourMenu = new TutorialMenu(24);
            worldfiveMenu = new TutorialMenu(32);
            worldsixMenu = new TutorialMenu(40);
            worldsevenMenu = new TutorialMenu(48);
        }

        public void StartNewLevel(int level_no)
        {
            SoundManager.StopBkgSounds();
            
            currentLevelPos = level_no;

            if (currentLevelPos == KEYBOARD_CONTROLS_LEVEL_ID)
                currentLevel = LevelCreator.ParseLevelFromFile("Content\\Levels\\keyboard_controls.xml");
            else if (currentLevelPos == XBOX_CONTROLS_LEVEL_ID)
                currentLevel = LevelCreator.ParseLevelFromFile("Content\\Levels\\xbox_controls.xml");
            else if (currentLevelPos == CREDITS_LEVEL_ID)
                currentLevel = LevelCreator.ParseLevelFromFile("Content\\Levels\\credits_level.xml");
            else
                currentLevel = LevelCreator.ParseLevelFromFile("Content\\Levels\\" + level_files[currentLevelPos][0]);

            control = new PlayerController(currentLevel);
            levelMenu = new LevelMenu();
            refreshMenus();
            achievementMenu = new AchievementMenu();
            controlMenu = new ControlMenu();
            winMenu = new WinMenu();
            ContactManager.ClearContactList();

            bkg = GetTexture(currentLevel.bkg_image);
            PlayBkgSound(currentLevel.bkg_sound);
            if (currentLevel.Keys.Count == 0)
            {
                currentLevel.door.OpenDoor();
            }
            currentLevelStopwatch.Restart(); //Achievement logic - All speed related achievements
            GameEngine.Instance.achievement_state.reset_current_reflections(); //Achievement logic - Eureka achievement
        }

        public void StartNextLevel()
        {
            //Achievement Logic - "Stubbornness"
            achievement_state.reset_consecutive_fails();

            if (currentLevelPos == KEYBOARD_CONTROLS_LEVEL_ID || currentLevelPos == XBOX_CONTROLS_LEVEL_ID)
            {
                state = GameState.CONTROL_MENU;
                return;
            }
            else if (currentLevelPos == CREDITS_LEVEL_ID)
            {
                state = GameState.MAIN_MENU;
                return;
            }

            if (achievement_state.can_progress(currentLevelPos))
            {
                if (++currentLevelPos == level_files.Length)
                    BeatGame();
                else
                    StartNewLevel(currentLevelPos);
            }
            else //Player cannot progress due to not unlocking enough levels
            {
                refreshMenus();
                state = achievement_state.get_menu_world_state(currentLevelPos);
            }

        }

        private void BeatGame()
        {
            currentLevelPos = 0;
            state = GameState.MAIN_MENU;

            //Achievement Logic - "Stubbornness"
            achievement_state.reset_consecutive_fails();
        }

        public void RestartLevel()
        {
            StartNewLevel(currentLevelPos);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                if (full_screen)
                {
                    full_screen = false;
                    graphics.ToggleFullScreen();
                    UpdateShiftAmount();
                    //RestartLevel();
                }
                else
                {
                    full_screen = true;
                    SetFullScreen();
                    RestartLevel();
                }
            }
            switch (state)
            {
                case GameState.BOX_JELLYFISH:
                    SoundManager.PlayBkgSoundOnceIfNotPlaying(BOX_JELLYFISH_MUSIC);
                    if (box_jellyfish_time == 0)
                        box_jellyfish_fade_slope = (BOX_JELLYFISH_FADE_END - BOX_JELLYFISH_FADE_START) / (float)BOX_JELLYFISH_FADE_IN_TIME;
                    else if (box_jellyfish_time == BOX_JELLYFISH_FADE_IN_TIME)
                        box_jellyfish_fade_slope = 0;
                    else if (box_jellyfish_time == BOX_JELLYFISH_FADE_IN_TIME + BOX_JELLYFISH_STAY_TIME)
                        box_jellyfish_fade_slope = (BOX_JELLYFISH_FADE_START - BOX_JELLYFISH_FADE_END) / (float)BOX_JELLYFISH_FADE_OUT_TIME;
                    else if (box_jellyfish_time == BOX_JELLYFISH_FADE_IN_TIME + BOX_JELLYFISH_STAY_TIME + BOX_JELLYFISH_FADE_OUT_TIME)
                    {
                        box_jellyfish_fade_current = 0;
                        box_jellyfish_fade_slope = 0;                        
                    }
                    else if (box_jellyfish_time == BOX_JELLYFISH_FADE_IN_TIME + BOX_JELLYFISH_STAY_TIME + BOX_JELLYFISH_FADE_OUT_TIME + BOX_JELLYFISH_WAIT_TIME 
                        || Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start))
                        state = GameState.MAIN_MENU;
                    box_jellyfish_time++;
                    box_jellyfish_fade_current += box_jellyfish_fade_slope;
                        
                    break;

                case GameState.MAIN_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    mainMenu.Update();
                    break;

                case GameState.CONTROL_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    controlMenu.Update();
                    break;

                case GameState.LEVEL_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    levelMenu.Update();
                    break;

                case GameState.WORLD1_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldoneMenu.Update();
                    break;

                case GameState.WORLD2_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldtwoMenu.Update();
                    break;

                case GameState.WORLD3_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldthreeMenu.Update();
                    break;

                case GameState.WORLD4_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldfourMenu.Update();
                    break;

                case GameState.WORLD5_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldfiveMenu.Update();
                    break;

                case GameState.WORLD6_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldsixMenu.Update();
                    break;

                case GameState.WORLD7_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    worldsevenMenu.Update();
                    break;

                case GameState.ACHIEVEMENT_MENU:
                    SoundManager.PlayBkgSoundIfNotPlaying("meadows");
                    control.GetInput(state);
                    achievementMenu.Update();
                    break;

                case GameState.PLAYING:
                    //Achievement Logic - "Endurance"
                    instance.achievement_state.inc_play_time(gameTime.ElapsedGameTime.Milliseconds);
                    control.GetInput(state);
                    currentLevel.Simulate((float)gameTime.ElapsedGameTime.TotalSeconds); 
                    break;

                case GameState.WIN:
                    control.GetInput(state);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            GraphicsDevice.Clear(Color.Black);
            switch (state)
            {
                case GameState.BOX_JELLYFISH:
                    Color color = new Color(box_jellyfish_fade_current, box_jellyfish_fade_current, box_jellyfish_fade_current, box_jellyfish_fade_current);
                    spriteBatch.Begin();
                    spriteBatch.Draw(box_jellyfish, new Rectangle(shiftw, shifth, (int)Math.Min(w, h), (int)Math.Min(w, h)), color);
                    spriteBatch.End();
                    break;
                case GameState.MAIN_MENU: mainMenu.Draw(); break;
                case GameState.LEVEL_MENU: levelMenu.Draw(); break;
                case GameState.CONTROL_MENU: controlMenu.Draw(); break;
                case GameState.WORLD1_MENU: worldoneMenu.Draw(); break;
                case GameState.WORLD2_MENU: worldtwoMenu.Draw(); break;
                case GameState.WORLD3_MENU: worldthreeMenu.Draw(); break;
                case GameState.WORLD4_MENU: worldfourMenu.Draw(); break;
                case GameState.WORLD5_MENU: worldfiveMenu.Draw(); break;
                case GameState.WORLD6_MENU: worldsixMenu.Draw(); break;
                case GameState.WORLD7_MENU: worldsevenMenu.Draw(); break;
                case GameState.ACHIEVEMENT_MENU: achievementMenu.Draw(); break;
                case GameState.PLAYING: this.currentLevel.Draw(); break;
                case GameState.WIN: winMenu.Draw(); break;
            }

            base.Draw(gameTime);
        }

        private string[][] ToArrayArray(string[] array)
        {
            string[][] temp = new string[array.Length][];

            for (int i = 0; i < array.Length; i++)
            {
                temp[i] = Regex.Split(array[i], ", *");
            }

            return temp;
        }
    }
}
