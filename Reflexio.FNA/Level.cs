/**
 *  LIST ALL CHANGES TO THIS FILE:
 *  PERSON - DATE - CHANGE
 *  
 *  Meng - 24th Sept - Added ROW_SCALE and COL_SCALE, reflection variables and Wall list
 *  Devansh - 28th Sept - Made changes to wall logic and removed wall list, integrated with Wall class
 *  Devansh - 29th Sept - Added Discretization to the AddWall and in the wall input
 *  Arthur - 29th Septh - Added PlayerController, sensors and listeners for jumping only from ground, and a floor and ceiling to temp level
 *  Devansh - 8th Oct - Moved Arthurs Contact Listener (just moved not changed) to separate from the temp level
 *  Devansh - 8th Oct - Added several variables (reflection_line_position, horizontal_skip, vertical_skip) for variable line of reflection
 *  Devansh - 8th Oct - Added logic for variable line of reflection in Reflect()
 *  Devansh - 8th Oct - Added logic to 'pause' the game while reflecting. Set pause time by changing REFLECTION_PAUSE_TIME.
 *  Devansh - 8th Oct - Changed Wall Constructor so that now it includes a field 'is_reflectable'.
 *  Devansh - 9th Oct - Changed the temp level so now its ends a XML description of the level.
 */

/*
 * LEVEL CREATOR DETAILS:
 * 
 * INITIALIZABLE PARAMS:
 * SetWidth(float) Default 13.0
 * SetHeight(float) Default 13.0
 * SetGravity(float) No Default
 * SetScale(float) Default 50
 * SetDimensions(int, int) Default 20,20
 * SetReflectionPauseTime(int) Default 25
 * SetCanReflectHorizontal(bool) Default true
 * SetCanReflectVertical(bool) Default false
 * SetCanReflectDiagonal(bool) Default false
 * SetReflectionLinePosition(int) Default 10 (20/2)
 * SetReflectionOrientation(H|V|D) Default H
 * AddHRLine(int) Rows that can be used as a horizontal line of reflection
 * AddVRLine(int) Columns that can be used as a vertical line of reflection
 * AddDLine(int) Diagonal lines that can be used as a diagonal line of reflection. 1 => (TL,BR), -1 => (TR, BL)
 * AddDialogDisplay
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using System.Diagnostics;

namespace Reflexio
{
    public class Level
    {
        #region MEMBER VARIABLES
        public enum GameState
        {
            Unzip, Playing, Paused, DoorEat, Zip, Over, Buffer
        }

        //static SpriteFont Font1 = GameEngine.Font;

        AnimationTexture unzipTexture;
        AnimationTexture zipTexture;
        Texture2D zippedTexture;

        private Stopwatch achievewatch;

        private int bufferTime;
        private int bufferTimeElapsed;
        private GameState stateAfterBuffer;
        private int TIME_BETWEEN_DOOR_EAT_AND_ZIP = 100;
        public bool hidePlayer = false;

        private int ACHIEVE_TIME = 5000; //Time the achievement notification shows
        

        public GameState gamestate;
        public PauseMenu pauseMenu;
        public GameOverMenu gameOverMenu;

        // Dimensions of the game world
        public static float WIDTH = 13.0f;
        public static float HEIGHT = 13.0f;
        // Scale from game space -> screen space
        public static float SCALE = 50.0f;
        
        public int NUM_ROWS = 20;
        public int NUM_COLS = 20;
        
        // added these variables for keep track of the scale from the dimensions to the number of rows and columns
        // by Mengxiang
        public float ROW_SCALE;
        public float COL_SCALE;


        // added these for setting reflection for level
        public enum ReflectionOrientation
        {
            HORIZONTAL, VERTICAL, DIAGONAL, NONE
        };

        private bool achieve;
        private string achievestring;

        private bool canReflectHorizontal = true;
        private bool canReflectVertical = false;
        private bool canReflectDiagonal = false;
        private ReflectionOrientation ref_orientation = ReflectionOrientation.HORIZONTAL;
        private int reflection_line_position = -2;
        // Used for defining which lines of reflection can be used
        public List<int> horizontal_lines = new List<int>();
        public List<int> h_switch_lines = new List<int>();
        public List<int> vertical_lines = new List<int>();
        public List<int> v_switch_lines = new List<int>();
        public List<int> diagonal_lines = new List<int>();
        public List<int> d_switch_lines = new List<int>();
        public string bkg_sound;
        public string bkg_image;
        public string bkg_text;
        private int current_pos_in_list = 0;
        public int buddydeath;

        //The direction of gravity and magnitude
        private Vector2 gravity;

        // Amount of offgame area covered by the world AABB
        protected const float MARGIN = 2.0f;

        // All the objects in the world
        public LinkedList<PhysicsObject> objects = new LinkedList<PhysicsObject>();

        private List<Key> keys = new List<Key>();
        public List<Key> Keys
        {
            get { return keys; }
        }
        private Door d;
        public Door door
        {
            get { return d; }
        }

        // queue for adding objects
        private Queue<PhysicsObject> addQueue = new Queue<PhysicsObject>();

        private World world;

        //for player control and jumping only from ground sensing
        public Player player;
        public static string dudeSensorName = "Dude Ground Sensor";

        // Have we won yet?
        private bool succeeded;

        // Used to 'pausing' movement while reflecting
        public int reflection_pause_remaining_time = 0;
        public static int REFLECTION_PAUSE_TIME = 30;

        // Used for displaying hints and dialogs on screen
        DialogDisplay dialog_displayer;
        private static int ACHIEVE_TEXT_Y = 43;
        Texture2D cloud = GameEngine.Instance.GetTexture("dialog_cloud");
        Texture2D dialog = GameEngine.Instance.GetTexture("dialog_rect");

        // Variables for displaying hazed out pause menu
        Texture2D haze;
        Rectangle rect;

        // Sound Effects
        //private SoundEffectInstance se = GameEngine.Instance.GetMusic("dunDunDun").CreateInstance();
        //private SoundEffectInstance se2 = GameEngine.Instance.GetMusic("zipperSlow").CreateInstance();

        // Go back to main menu afterwards
        public bool GoBackToMainMenu = false;

        // For scrolling background
        bool is_scrolling_bkg = false;
        int scrolling_bkg_speed = 10;
        int current_bkg_pos = 0;

        // For reflection line color
        int reflection_color_time = 0;
        int REFLECTION_COLOR_END_TIME = 120;
        
        float MIN_REFLECTION_COLOR_RED = 0.9f;
        float MAX_REFLECTION_COLOR_RED = 0.95f;

        float MIN_REFLECTION_COLOR_GREEN = 0.77f;
        float MAX_REFLECTION_COLOR_GREEN = 0.95f;

        float MIN_REFLECTION_COLOR_BLUE = 0;
        float MAX_REFLECTION_COLOR_BLUE = 0.95f;

        float reflection_current_red = 0;
        float reflection_current_green = 0;
        float reflection_current_blue = 0;
        float slope_red = 0;
        float slope_green = 0;
        float slope_blue = 0;

        // For peeking
        public bool is_peeking = false;
        #endregion

        #region Setters

        public void SetBkgImage(string s)
        {
            bkg_image = s;
        }

        public void SetBkgSound(string s)
        {
            bkg_sound = s;
        }

        public void SetBkgText(string s)
        {
            bkg_text = s;
        }

        public void SetWidth(float width)
        {
            Level.WIDTH = width;
        }

        public void SetHeight(float height)
        {
            Level.HEIGHT = height;
        }

        public void SetGravity(float x, float y)
        {
            this.gravity = new Vector2(x, y);
        }

        public void SetScale(float scale)
        {
            if (!GameEngine.Instance.full_screen)
            {
                Level.SCALE = scale;
                GameEngine.Instance.ResizeWindow((int)(WIDTH * SCALE), (int)(HEIGHT * SCALE));
            }
        }

        public void SetDimensions(int rows, int cols)
        {
            this.NUM_ROWS = rows;
            this.NUM_COLS = cols;
        }

        public void SetReflectionPauseTime(int time)
        {
            REFLECTION_PAUSE_TIME = time;
        }

        public void SetCanReflectHorizontal(bool val)
        {
            canReflectHorizontal = val;
        }

        public void SetCanReflectVertical(bool val)
        {
            canReflectVertical = val;
        }

        public void SetCanReflectDiagonal(bool val)
        {
            canReflectDiagonal = val;
        }

        public void SetReflectionLinePosition(int pos)
        {
            reflection_line_position = pos;

            switch (ref_orientation)
            {
                case ReflectionOrientation.HORIZONTAL: current_pos_in_list = horizontal_lines.IndexOf(pos); break;
                case ReflectionOrientation.VERTICAL: current_pos_in_list = vertical_lines.IndexOf(pos); break;
                case ReflectionOrientation.DIAGONAL: current_pos_in_list = diagonal_lines.IndexOf(pos); break;
            }
        }

        public void SetReflectionOrientation(string orientation)
        {
            if(orientation.Equals("H", StringComparison.OrdinalIgnoreCase))
                this.ref_orientation = ReflectionOrientation.HORIZONTAL;
            if(orientation.Equals("V", StringComparison.OrdinalIgnoreCase))
                this.ref_orientation = ReflectionOrientation.VERTICAL;
            if(orientation.Equals("D", StringComparison.OrdinalIgnoreCase))
                this.ref_orientation = ReflectionOrientation.DIAGONAL;
        }

        public void AddHRLine(int line)
        {
            this.horizontal_lines.Add(line);
            this.horizontal_lines.Sort();
            if (this.ref_orientation == ReflectionOrientation.HORIZONTAL)
                current_pos_in_list = this.horizontal_lines.IndexOf(reflection_line_position);
        }

        public void RemoveHRLine(int line)
        {
            if (this.ref_orientation == ReflectionOrientation.HORIZONTAL && line == reflection_line_position)
            {
                reflection_line_position = -2;
                ref_orientation = ReflectionOrientation.NONE;
            }
            this.horizontal_lines.Remove(line);
        }

        public void AddVRLine(int line)
        {
            this.vertical_lines.Add(line);
            this.vertical_lines.Sort();
            if (this.ref_orientation == ReflectionOrientation.VERTICAL)
                current_pos_in_list = this.vertical_lines.IndexOf(reflection_line_position);
        }

        public void RemoveVRLine(int line)
        {
            if (this.ref_orientation == ReflectionOrientation.VERTICAL && line == reflection_line_position)
            {
                reflection_line_position = -2;
                ref_orientation = ReflectionOrientation.NONE;
            }
            this.vertical_lines.Remove(line);
        }

        public void AddDLine(int line)
        {
            this.diagonal_lines.Add(line);
            this.diagonal_lines.Sort();
            if (this.ref_orientation == ReflectionOrientation.DIAGONAL)
                current_pos_in_list = this.diagonal_lines.IndexOf(reflection_line_position);
        }

        public void RemoveDLine(int line)
        {
            if (this.ref_orientation == ReflectionOrientation.DIAGONAL && line == reflection_line_position)
            {
                reflection_line_position = -2;
                ref_orientation = ReflectionOrientation.NONE;
            }
            this.diagonal_lines.Remove(line);
        }

        public void AddDialogDisplay(DialogDisplay dd)
        {
            this.dialog_displayer = dd;
        }

        public void SetIsScrolling(bool val)
        {
            this.is_scrolling_bkg = val;
        }

        public void SetScrollingSpeed(int speed)
        {
            this.scrolling_bkg_speed = speed;
        }
        #endregion

        #region Initialize and Constructors
        public void Initialize()
        {
            buddydeath = 0;
            // If game is being in full screen update the SCALE
            /*if (GameEngine.Instance.full_screen)
            {
                int min = Math.Min(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                GameEngine.Instance.ResizeWindow(min, min);
                Vector2 size = GameEngine.Instance.GetWindowSize();
                SCALE = size.X < size.Y ? size.X / WIDTH : size.Y / HEIGHT;
            }
            else // Else rescale to the size you want
            {
                GameEngine.Instance.ResizeWindow((int)(WIDTH * SCALE), (int)(HEIGHT * SCALE));
            }*/
            GameEngine.Instance.UpdateShiftAmount();
            ROW_SCALE = WIDTH / NUM_ROWS;
            COL_SCALE = HEIGHT / NUM_COLS;
            AABB aabb = new AABB();
            aabb.LowerBound = new Vector2(-MARGIN, -MARGIN);
            aabb.UpperBound = new Vector2(WIDTH + MARGIN, HEIGHT + MARGIN);
            world = new World(gravity);
            succeeded = false;
            World.ContactManager.BeginContact += ContactManager.BeginContact;
            World.ContactManager.EndContact += ContactManager.EndContact;
            pauseMenu = new PauseMenu(this);
            gameOverMenu = new GameOverMenu(this);
            gamestate = GameState.Unzip;
            unzipTexture = new AnimationTexture(GameEngine.Instance.GetTexture("unzip"), 5, 3, 3);
            zipTexture = new AnimationTexture(GameEngine.Instance.GetTexture("zip"), 5, 3, 3);
            unzipTexture.ResetCurrentFrame();
            zipTexture.ResetCurrentFrame();
            zippedTexture = GameEngine.Instance.GetTexture("zippedbkg");
            haze = new Texture2D(GameEngine.Instance.GraphicsDevice, 1, 1);
            rect = new Rectangle(0, 0, (int)(Level.WIDTH * Level.SCALE + GameEngine.shiftAmount.X), (int)(Level.HEIGHT * Level.SCALE + GameEngine.shiftAmount.Y));
            haze.SetData(new Color[] { new Color(0, 0, 0, 255) });
            // Flush all the non-reflectable objects from the previous levels
            ReflectableObject.FlushNonReflectableObjects();
            ContactManager.ClearContactList();
            Block.FlushGameBlocks();

            reflection_color_time = 0;

            reflection_current_blue = MIN_REFLECTION_COLOR_BLUE;
            reflection_current_green = MIN_REFLECTION_COLOR_GREEN;
            reflection_current_red = MIN_REFLECTION_COLOR_RED;

            slope_red = 2 * (MAX_REFLECTION_COLOR_RED - MIN_REFLECTION_COLOR_RED) / (REFLECTION_COLOR_END_TIME);
            slope_green = 2 * (MAX_REFLECTION_COLOR_GREEN - MIN_REFLECTION_COLOR_GREEN) / (REFLECTION_COLOR_END_TIME);
            slope_blue = 2 * (MAX_REFLECTION_COLOR_BLUE - MIN_REFLECTION_COLOR_BLUE) / (REFLECTION_COLOR_END_TIME);
        }

        public Level()
        {
        }

        public Level(float width, float height, Vector2 gravity) 
        {
            // Create the world's axis-aligned bounding box
            WIDTH = width;
            HEIGHT = height;
            this.gravity = gravity;
            Initialize();
        }
        #endregion

        #region Adders
        /// <summary>
        /// Puts the physics object in the add queue, to be added
        /// before the next step.  This is required as objects
        /// cannot be added during engine processing.
        /// </summary>
        /// <param name="obj"></param>
        public void AddQueuedObject(PhysicsObject obj)
        {
            addQueue.Enqueue(obj);
        }

        /// <summary>
        /// Adds the object to the physics world
        /// </summary>
        /// <param name="obj"></param>
        protected void AddObject(PhysicsObject obj)
        {
            obj.AddToWorld();
            objects.AddLast(obj);            
            obj.AddJointsToWorld();
        }

        public void AddWall(Wall w)
        {
            this.AddQueuedObject(w);
        }

        public void AddPlayer(Player p)
        {
            this.player = p;
            this.AddQueuedObject(player);
        }

        public void AddKey(Key k)
        {
            this.keys.Add(k);
            this.AddQueuedObject(k);
        }

        public void AddDoor(Door d)
        {
            this.d = d;
            this.AddQueuedObject(d);
        }

        public void AddSpike(Spike s)
        {
            this.AddQueuedObject(s);
        }

        public void AddBlock(Block b)
        {
            this.AddQueuedObject(b);
        }

        public void AddSwitch(Switch s)
        {
            this.AddQueuedObject(s);
        }
        #endregion

        #region UPDATE AND DRAW LOGIC
        public virtual void Simulate(float dt)
        {
            // add any objects
            foreach (var physicsObject in addQueue)
            {
                AddObject(physicsObject);
            }
            addQueue.Clear();
            if (gamestate == GameState.Playing)
            {
                reflection_color_time++;
                if (reflection_color_time > REFLECTION_COLOR_END_TIME)
                {
                    reflection_current_blue = MIN_REFLECTION_COLOR_BLUE;
                    reflection_current_green = MIN_REFLECTION_COLOR_GREEN;
                    reflection_current_red = MIN_REFLECTION_COLOR_RED;
                    reflection_color_time = 0;
                }
                int neg = reflection_color_time > REFLECTION_COLOR_END_TIME / 2 ? -1 : 1;
                reflection_current_red += slope_red * neg;
                reflection_current_green += slope_green * neg;
                reflection_current_blue += slope_blue * neg;


                if (reflection_pause_remaining_time > 0)
                {
                    reflection_pause_remaining_time -= 1;
                }
                else if(!is_peeking)
                {
                    world.Step(dt);

                    // iterate through the linked list and remove if needed.
                    var node = objects.First;
                    while (node != null)
                    {
                        var obj = node.Value;
                        var next = node.Next;
                        if (obj.IsDead)
                        {
                            objects.Remove(node);
                        }
                        else
                        {
                            obj.Update(this, dt);
                        }
                        node = next;
                    }

                    ContactManager.CheckSpiked();

                    // Update the DialogDisplay
                    if (dialog_displayer != null)
                        dialog_displayer.Update();
                }
            }
            else if (gamestate == GameState.Unzip)
            {
                if (unzipTexture.num_rotations > 0)
                    gamestate = GameState.Playing;
            }
            else if (gamestate == GameState.DoorEat)
            {
                if (this.door.animTexture.num_rotations > 0)
                {
                    this.door.animTexture.stop_animating = true;
                    this.door.animTexture.SetToLastFrame();
                    //if(achieve.isShowingaC... 
                        //SetBufferState(GameState.Zip, TIME_BETWEEN_DOOR_EAT_AND_ZIP, true);
                    //else
                    gamestate = GameState.Zip;
                }
            }
            else if (gamestate == GameState.Zip)
            {
                //if(se.State != SoundState.Playing)
                //se2.Play();
                SoundManager.PlaySoundIfNotPlaying("zipperSlow");
                if (zipTexture.num_rotations > 0)
                {
                    if (GoBackToMainMenu)
                        GameEngine.Instance.State = GameEngine.GameState.MAIN_MENU;
                    else if (succeeded)
                        GameEngine.Instance.StartNextLevel();
                    //gamestate = GameState.Over;
                }
            }
            else if (gamestate == GameState.Paused)
            {
            }
            else if (gamestate == GameState.Over)
            {
                //if (succeeded)
                //GameEngine.Instance.StartNextLevel();
            }
            else if (gamestate == GameState.Buffer)
            {
                if (bufferTimeElapsed >= bufferTime)
                {
                    gamestate = stateAfterBuffer;
                }
                else
                    bufferTimeElapsed++;
            }
        }

        private void SetBufferState(GameState gs, int frames, bool hidePlayer)
        {
            this.gamestate = GameState.Buffer;
            stateAfterBuffer = gs;
            bufferTimeElapsed = 0;
            bufferTime = frames;
            this.hidePlayer = hidePlayer;
        }

        public void drawAchievement(string s)
        {
            achieve = true;
            achievestring = s;
            achievewatch = new Stopwatch();
            achievewatch.Restart();
        }

        /**
         * Draws all objects in the physics world
         */
        public virtual void Draw()
        {
            float w = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Width;
            float h = GameEngine.Instance.GraphicsDevice.ScissorRectangle.Height;
            float scale = Math.Min(w / 650, h / 650);
            int shiftw = (int)(w / 2 - Math.Min(w, h) / 2);
            int shifth = (int)(h / 2 - Math.Min(w, h) / 2);
            float cloudscale = Level.SCALE * Level.WIDTH / cloud.Width;
            var spriteBatch = Reflexio.GameEngine.Instance.SpriteBatch;
            Texture2D bkg = Reflexio.GameEngine.Instance.bkg;
            if (is_scrolling_bkg)
            {
                Vector2 bkg_pos = new Vector2(0, current_bkg_pos);
                current_bkg_pos -= scrolling_bkg_speed;
                if (current_bkg_pos < -bkg.Height + WIDTH * SCALE)
                    current_bkg_pos = (int)bkg_pos.Y;
                spriteBatch.Begin();
                spriteBatch.Draw(bkg, bkg_pos * scale + GameEngine.shiftAmount, null, Color.White, 0.0f, Vector2.Zero,
                                scale, SpriteEffects.None, 1.0f);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();

                

                Vector2 backgroundScale = new Vector2(
                    Level.SCALE * Level.WIDTH / bkg.Width,
                    Level.SCALE * Level.HEIGHT / bkg.Height);


                spriteBatch.Draw(bkg, new Rectangle(shiftw, shifth, (int)Math.Min(w, h), (int)Math.Min(w, h)), Color.White);
                spriteBatch.End();
            }

            #region Playing
            foreach (PhysicsObject obj in objects)
            {
                if(!(obj is Player) || (gamestate != GameState.DoorEat && gamestate != GameState.Zip))
                    obj.Draw();
            } 
        
            Texture2D lineTexture = Reflexio.GameEngine.Instance.GetTexture("lineTexture");
            var origin = new Vector2(lineTexture.Width, lineTexture.Height) / 2;

            spriteBatch.Begin();
            // Draw the line of reflection           
            Color ref_c = new Color(reflection_current_red, reflection_current_green, reflection_current_blue);
                
            if (ref_orientation == ReflectionOrientation.HORIZONTAL)
                spriteBatch.Draw(lineTexture, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2, SCALE * reflection_line_position * COL_SCALE)*scale, null, ref_c, 0.0f, origin, new Vector2(WIDTH * SCALE, 3)*scale, SpriteEffects.None, 0);
            else if (ref_orientation == ReflectionOrientation.VERTICAL)
                spriteBatch.Draw(lineTexture, GameEngine.shiftAmount + new Vector2(SCALE * reflection_line_position * ROW_SCALE, SCALE * HEIGHT / 2)*scale, null, ref_c, 0.0f, origin, new Vector2(3, HEIGHT * SCALE)*scale, SpriteEffects.None, 0);
            else if (ref_orientation == ReflectionOrientation.DIAGONAL)
            {
                int min = GameEngine.Instance.Window.ClientBounds.Width < GameEngine.Instance.Window.ClientBounds.Height ? GameEngine.Instance.Window.ClientBounds.Width : GameEngine.Instance.Window.ClientBounds.Height;
                if (reflection_line_position == 1)
                {
                    for (int i = 0; i < min; i++)
                        spriteBatch.Draw(lineTexture, GameEngine.shiftAmount + new Vector2(i, i)*scale, null, ref_c, 0.0f, origin, new Vector2(3, 1)*scale, SpriteEffects.None, 0);
                }
                else if (reflection_line_position == -1)
                {
                    for (int i = 0; i < min; i++)
                        spriteBatch.Draw(lineTexture, GameEngine.shiftAmount + new Vector2(min - i, i)*scale, null, ref_c, 0.0f, origin, new Vector2(3, 1)*scale, SpriteEffects.None, 0);
                }
            }

            //Draw all positions of the line of reflections
            Texture2D circle = Reflexio.GameEngine.Instance.GetTexture("reflectionCircle"); ;
            float xscale = (COL_SCALE * SCALE) / circle.Width;
            float yscale = (ROW_SCALE * SCALE) / circle.Height;
            float deltaX = (COL_SCALE * SCALE) / 2;
            float deltaY = (ROW_SCALE * SCALE) / 2;
            origin = new Vector2(circle.Width, circle.Height) / 2;
            int[] arr = horizontal_lines.ToArray();
            foreach (int i in horizontal_lines)
            {
                circle = Reflexio.GameEngine.Instance.GetTexture("reflectionCircle");
                if(h_switch_lines.Contains(i))
                    circle =  Reflexio.GameEngine.Instance.GetTexture("reflectionCircleSwitch");
                spriteBatch.Draw(circle, GameEngine.shiftAmount + new Vector2(deltaX, SCALE * (i) * COL_SCALE) * scale, null,
                    Color.White, (float)Math.PI / 2, origin, new Vector2(xscale, yscale) * scale, SpriteEffects.None, 0);
            }
            arr = vertical_lines.ToArray();
            foreach (int i in vertical_lines)
            {
                circle = Reflexio.GameEngine.Instance.GetTexture("reflectionCircle");
                if (v_switch_lines.Contains(i))
                    circle = Reflexio.GameEngine.Instance.GetTexture("reflectionCircleSwitch");
                spriteBatch.Draw(circle, GameEngine.shiftAmount + new Vector2(SCALE * (i) * ROW_SCALE, (HEIGHT - ROW_SCALE) * SCALE + deltaY) * scale, null,
                    Color.White, 0.0f, origin, new Vector2(xscale, yscale) * scale, SpriteEffects.None, 0);
            }
            arr = diagonal_lines.ToArray();
            foreach (int i in diagonal_lines)
            {
                circle = Reflexio.GameEngine.Instance.GetTexture("reflectionCircle");
                if (d_switch_lines.Contains(i))
                    circle = Reflexio.GameEngine.Instance.GetTexture("reflectionCircleSwitch");
                if (i == 1)
                {
                    spriteBatch.Draw(circle, GameEngine.shiftAmount + new Vector2(deltaX, deltaY) * scale, null,
                        Color.White, (float)(3 * Math.PI / 4), origin, new Vector2(xscale, yscale) * scale, SpriteEffects.None, 0);
                }
                else if (i == -1)
                {
                    spriteBatch.Draw(circle, GameEngine.shiftAmount + new Vector2(deltaX, (HEIGHT - ROW_SCALE) * SCALE + deltaY) * scale, null,
                        Color.White, (float)Math.PI / 4, origin, new Vector2(xscale, yscale) * scale, SpriteEffects.None, 0);
                }
            }
            if (achievestring != null && achieve && achievewatch.ElapsedMilliseconds <= ACHIEVE_TIME)
            {
                spriteBatch.Draw(cloud, GameEngine.shiftAmount + new Vector2(10, 10) * scale, null, Color.White, 0.0f, Vector2.Zero, 1 * scale, SpriteEffects.None, 0);
                //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2, ACHIEVE_TEXT_Y) * scale, Color.White, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2 + 2, ACHIEVE_TEXT_Y + 2) * scale, Color.Black, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
            }

            if (bkg_text != null && !player.pressed_enter)
            {
                spriteBatch.Draw(dialog, GameEngine.shiftAmount + new Vector2(0, 10) * scale, null, Color.White, 0.0f, Vector2.Zero, cloudscale * scale, SpriteEffects.None, 1.0f);
                //spriteBatch.DrawString(Font1, bkg_text, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2, ACHIEVE_TEXT_Y) * scale, Color.White, 0, Font1.MeasureString(bkg_text) / 2, 1f * scale, SpriteEffects.None, 0);
                //spriteBatch.DrawString(Font1, bkg_text, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2 + 2, ACHIEVE_TEXT_Y + 2) * scale, Color.Black, 0, Font1.MeasureString(bkg_text) / 2, 1f * scale, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            // Draw the dialogs
            if (dialog_displayer != null)
                dialog_displayer.Draw(spriteBatch);
            #endregion

            #region REST
            if (gamestate == GameState.Paused || is_peeking)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(haze, new Rectangle(shiftw, shifth, (int)Math.Min(w, h), (int)Math.Min(w, h)), new Color(255, 255, 255, 100));
                spriteBatch.End();
                pauseMenu.Draw();
            }
            else if (gamestate == GameState.Unzip)
            {
                unzipTexture.DrawWholeScreen(spriteBatch);
                spriteBatch.Begin();
                if (achievestring != null && achieve && achievewatch.ElapsedMilliseconds <= ACHIEVE_TIME)
                {
                    spriteBatch.Draw(cloud, GameEngine.shiftAmount + new Vector2(10, 10) * scale, null, Color.White, 0.0f, Vector2.Zero, 1 * scale, SpriteEffects.None, 0);
                    //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2, 30) * scale, Color.White, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                    //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2 + 2, 32) * scale, Color.Black, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                }
                spriteBatch.End();
            }
            else if (gamestate == GameState.Zip)
            {
                zipTexture.DrawWholeScreen(spriteBatch);
                spriteBatch.Begin();
                if (achievestring != null && achieve && achievewatch.ElapsedMilliseconds <= ACHIEVE_TIME)
                {
                    spriteBatch.Draw(cloud, GameEngine.shiftAmount + new Vector2(10, 10) * scale, null, Color.White, 0.0f, Vector2.Zero, 1 * scale, SpriteEffects.None, 0);
                    //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2, 30) * scale, Color.White, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                    //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2 + 2, 32) * scale, Color.Black, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                }
                spriteBatch.End();
            }
            if (gamestate == GameState.Over)
            {
                if (succeeded)
                {
                    Vector2 scale2 = new Vector2(Level.SCALE * Level.WIDTH / zippedTexture.Width, Level.SCALE * Level.HEIGHT / zippedTexture.Height);
                    spriteBatch.Begin();
                    spriteBatch.Draw(zippedTexture, GameEngine.shiftAmount, null, Color.White, 0.0f, Vector2.Zero, scale2 * scale, SpriteEffects.None, 0);
                    if (achievestring != null && achieve && achievewatch.ElapsedMilliseconds <= ACHIEVE_TIME)
                    {
                        spriteBatch.Draw(cloud, GameEngine.shiftAmount + new Vector2(10, 10) * scale, null, Color.White, 0.0f, Vector2.Zero, 1 * scale, SpriteEffects.None, 0);
                        //spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2, 30) * scale, Color.White, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                       // spriteBatch.DrawString(Font1, achievestring, GameEngine.shiftAmount + new Vector2(SCALE * WIDTH / 2 + 2, 32) * scale, Color.Black, 0, Font1.MeasureString(achievestring) / 2, 1f * scale, SpriteEffects.None, 0);
                    }
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(haze, new Rectangle(shiftw, shifth, (int)Math.Min(w, h), (int)Math.Min(w, h)), new Color(255, 255, 255, 100));
                    spriteBatch.End();
                    gameOverMenu.Draw();
                }
            }
            #endregion
        }
        #endregion

        #region ChangeState
        public void TogglePaused()
        {
            if (gamestate != GameState.Paused)
                gamestate = GameState.Paused;
            else
                gamestate = GameState.Playing;
        }

        public void SetGameOver(bool has_won)
        {
            if (gamestate == GameState.Playing)
            {
                SoundManager.StopBkgSounds();
                this.succeeded = has_won;
                if (has_won)
                {
                    GameEngine.Instance.achievement_state.reset_consecutive_fails(); //Achievement logic - 'stubbornness'
                    gamestate = GameState.DoorEat;
                    this.door.DoorEat();
                }
                else
                {
                    GameEngine.Instance.achievement_state.inc_consecutive_fails(); //Achievement logic - 'stubbornness'
                    gamestate = GameState.Over;
                    SoundManager.PlaySound("dunDunDun");
                }
            }
            
        }

        public void SetGameOverAndNotZip(bool has_won)
        {
            SoundManager.StopBkgSounds();
            this.succeeded = has_won;
            if (has_won)
            {
                SoundManager.PlaySound("zipperSlow");
            }
            else
                SoundManager.PlaySound("dunDunDun");
            gamestate = GameState.Over;
        }
        #endregion

        #region Getters
        /**
         * Gets the Box2DX physics world
         */
        public World World
        {
            get { return world; }
        }

        /**
         * Whether or not we've beaten this level
         */
        public bool Succeeded
        {
            get { return succeeded; }
        }

        public LinkedList<PhysicsObject> Objects
        {
            get { return objects; }
        }
        #endregion

        #region REFLECTION LOGIC
        public void Reflect()
        {
            if (reflection_line_position >= -1 && ref_orientation == ReflectionOrientation.DIAGONAL)
            {
                foreach (PhysicsObject obj in objects)
                    obj.Reflect(reflection_line_position, reflection_line_position, ref_orientation);
            }
            else if(reflection_line_position >= 0)
            {
                int start, end, size;
                if (ref_orientation == ReflectionOrientation.HORIZONTAL)
                    size = NUM_COLS - 2;
                else
                    size = NUM_ROWS  - 2;
                int min_dist = Math.Min(reflection_line_position - 1, size - reflection_line_position + 1);
                start = reflection_line_position - min_dist;
                end = reflection_line_position + min_dist - 1;
                foreach (PhysicsObject obj in objects)
                    obj.Reflect(start, end, ref_orientation);
            }
            reflection_pause_remaining_time = REFLECTION_PAUSE_TIME;
            ContactManager.CheckOrientationChangedOnReflection();
            ContactManager.CheckIsGroundedAfterReflection();
        }

        public void Peek()
        {
            Reflect();
        }

        public void MoveReflectionLineLeft()
        {
            if (canReflectVertical && vertical_lines.Count > 0)
            {
                if (ref_orientation == ReflectionOrientation.VERTICAL)
                {
                    int next = current_pos_in_list - 1;
                    if (next < 0)
                        next = vertical_lines.Count - 1;
                    reflection_line_position = (int)(vertical_lines.ToArray()[next]);
                    current_pos_in_list = next;
                }
                else
                {
                    ref_orientation = ReflectionOrientation.VERTICAL;
                    current_pos_in_list = vertical_lines.Count / 2;
                    reflection_line_position = (int)(vertical_lines.ToArray()[current_pos_in_list]);
                }
            }
        }

        public void MoveReflectionLineRight()
        {
            if (canReflectVertical && vertical_lines.Count > 0)
            {
                if (ref_orientation == ReflectionOrientation.VERTICAL)
                {
                    int next = (current_pos_in_list + 1) % vertical_lines.Count;
                    reflection_line_position = (int)(vertical_lines.ToArray()[next]);
                    current_pos_in_list = next;
                }
                else
                {
                    ref_orientation = ReflectionOrientation.VERTICAL;
                    current_pos_in_list = vertical_lines.Count/2;
                    reflection_line_position = (int)(vertical_lines.ToArray()[current_pos_in_list]);
                }
            }
        }

        public void MoveReflectionLineUp()
        {
            if (canReflectHorizontal && horizontal_lines.Count > 0)
            {
                if (ref_orientation == ReflectionOrientation.HORIZONTAL)
                {
                    int next = current_pos_in_list - 1;
                    if (next < 0)
                        next = horizontal_lines.Count - 1;
                    reflection_line_position = (int)(horizontal_lines.ToArray()[next]);
                    current_pos_in_list = next;
                }
                else
                {
                    ref_orientation = ReflectionOrientation.HORIZONTAL;
                    current_pos_in_list = horizontal_lines.Count / 2;
                    reflection_line_position = (int)(horizontal_lines.ToArray()[current_pos_in_list]);
                }
            }
        }

        public void MoveReflectionLineDown()
        {
            if (canReflectHorizontal && horizontal_lines.Count > 0)
            {
                if (ref_orientation == ReflectionOrientation.HORIZONTAL)
                {
                    int next = (current_pos_in_list + 1) % horizontal_lines.Count;
                    reflection_line_position = (int)(horizontal_lines.ToArray()[next]);
                    current_pos_in_list = next;
                }
                else
                {
                    ref_orientation = ReflectionOrientation.HORIZONTAL;
                    current_pos_in_list = horizontal_lines.Count / 2;
                    reflection_line_position = (int)(horizontal_lines.ToArray()[current_pos_in_list]);
                }
            }
        }

        public void ToggleDiagonalReflection()
        {
            if (canReflectDiagonal && diagonal_lines.Count > 0)
            {
                if (ref_orientation == ReflectionOrientation.DIAGONAL)
                {
                    int next = (current_pos_in_list + 1) % diagonal_lines.Count;
                    reflection_line_position = (int)(diagonal_lines.ToArray()[next]);
                    current_pos_in_list = next;
                }
                else
                {
                    ref_orientation = ReflectionOrientation.DIAGONAL;
                    current_pos_in_list = 0;
                    reflection_line_position = (int)(diagonal_lines.ToArray()[current_pos_in_list]);
                }
            }
        }
        #endregion

        #region MISC
        public Vector2 DiscreteToContinuous(int x, int y)
        {
            return new Vector2(x * ROW_SCALE, y * COL_SCALE);
        }

        public Vector2 DiscreteToContinuousMidPoint(int x, int y, float width, float height)
        {
            return new Vector2(x * ROW_SCALE + width / 2, y * COL_SCALE + height / 2);
        }

        public int[] ContinuousToDiscrete(Vector2 pos)
        {
            return new int[2] { (int)(pos.X / COL_SCALE), (int)(pos.Y / ROW_SCALE) };
        }
        #endregion

        #region TEMP_LEVEL
        private static bool USE_XML_LEVEL = false;
        public void AddMultipleWalls(int x1, int y1, int x2, int y2, bool is_reflectable)
        {
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    this.AddQueuedObject(new Wall(this, groundTexture, i, j, 0, 1.0f, 0.0f, is_reflectable));
                }
            }
        }

        public void AddPlayerTemp(int x1, int y1)
        {
            player = new Player(this, playerTexture, GameEngine.Instance.GetTexture("idleStrip"), GameEngine.Instance.GetTexture("reflectionStrip"), x1, y1, 1.5f, 0.0f, 0.0f);
            this.AddQueuedObject(player);
        }

        private static Texture2D groundTexture;
        private static Texture2D playerTexture;

        private static int[][] platforms_disc = 
        {
            new int[] {0, 0, 0, 19},
            new int[] {19, 0, 19, 19},
            new int[] {0,0,19,0},
            new int[] {0,19,19,19},
            new int[] {4,12,5,12},
            new int[] {4,16,5,16},
            new int[] {7,9,8,9}
        };

        public static Level CreateTempLevel()
        {
            if (USE_XML_LEVEL)
            {
                return LevelCreator.ParseLevelFromFile("Content\\Levels\\something.xml");
            }

            else
            {
                groundTexture = Reflexio.GameEngine.Instance.Content.Load<Texture2D>("Images\\block");
                playerTexture = Reflexio.GameEngine.Instance.Content.Load<Texture2D>("Images\\koala");
                Level level = new Level(WIDTH, HEIGHT, new Vector2(0, 9.8f));
                
                
                foreach (int[] platform_discrete in platforms_disc)
                    level.AddMultipleWalls(platform_discrete[0], platform_discrete[1], platform_discrete[2], platform_discrete[3], platform_discrete[2] - platform_discrete[0] != 19 && platform_discrete[3] - platform_discrete[1] != 19);
                level.AddMultipleWalls(7, 2, 7, 2, true);
                level.AddMultipleWalls(8, 3, 8, 3, true);
                
                level.AddMultipleWalls(1, 15, 2, 15, false);
                level.AddMultipleWalls(2, 14, 2, 14, false);
                level.AddMultipleWalls(1, 13, 1, 13, false);
                level.AddMultipleWalls(1, 5, 2, 5, false);

                //level.AddQueuedObject(new Wall(level, groundTexture, 1, 1, 1.0f, 1.0f, 0.0f, true));

                Door d = new Door(level, GameEngine.Instance.GetTexture("openDoorTexture"), GameEngine.Instance.GetTexture("closeDoorTexture"), 18, 14, 0, 0.0f, 0.0f, true);
                level.AddDoor(d);
                level.AddKey(new Key(level, GameEngine.Instance.GetTexture("keyTexture"), 8, 2, 0, 0.0f, 0.0f, false));

                
                Texture2D[] spikesTextures = new Texture2D[4]
                {
                    GameEngine.Instance.GetTexture("spikesUpTexture"),
                    GameEngine.Instance.GetTexture("spikesRightTexture"),
                    GameEngine.Instance.GetTexture("spikesDownTexture"),
                    GameEngine.Instance.GetTexture("spikesLeftTexture")
                };

                for (int i = 1; i <= 18; i++)
                {
                    Spike s = new Spike(level, spikesTextures, 12, i, 0, 0.0f, 0.0f, true, Spike.Direction.Left);
                    level.AddQueuedObject(s);
                }
                for (int i = 13; i <= 18; i++)
                {
                    Spike s = new Spike(level, spikesTextures, i, 5, 0, 0.0f, 0.0f, true, Spike.Direction.Up);
                    level.AddQueuedObject(s);
                }

                Block b = new Block(level, GameEngine.Instance.GetTexture("buddyBlock"), 2, 16, 1.5f, 1.0f, 0.0f, true);
                level.AddQueuedObject(b);
                //level.AddMultipleWalls(3, 17, 3, 17, true);
                //Spike sx = new Spike(level, spikesTextures, 17, 18, 0, 0.0f, 0.0f, true, Spike.Direction.Up);
                //level.AddQueuedObject(sx);
                


                DialogDisplay dd = new DialogDisplay();
                dd.AddDialog("Welcome to Reflexio", 100);
                dd.AddDialog("Use WASDX to change the lines of reflection", 100);
                dd.AddDialog("Use SPACE to reflect", 100);
                dd.AddDialog("Get the key to open the door", 100);
                dd.AddDialog("All the best!", 100);
                dd.Initialize();
                level.AddDialogDisplay(dd);
                
                level.AddPlayerTemp(1, 18);

                level.SetReflectionOrientation("H");
                level.AddHRLine(10);
                level.AddVRLine(10);
                level.AddDLine(1);
                level.AddDLine(-1);
                level.SetCanReflectVertical(true);
                level.SetCanReflectDiagonal(true);
                level.SetReflectionLinePosition(10);

                return level;
            }
        }
        #endregion

        public void Serialize(System.Xml.XmlWriter writer)
        {
            writer.WriteProcessingInstruction("xml", @"version = ""1.p0""");
            writer.WriteStartElement("Level");
            writer.WriteAttributeString("hash", "level");
            writer.WriteElementString("Width", WIDTH.ToString());
            writer.WriteElementString("Height", HEIGHT.ToString());
            writer.WriteElementString("Gravity", gravity.X.ToString() + "," + gravity.Y.ToString());
            writer.WriteElementString("Scale", SCALE.ToString());
            writer.WriteElementString("Dimensions", NUM_ROWS.ToString() + ", " + NUM_COLS.ToString());
            writer.WriteElementString("ReflectionPauseTime", REFLECTION_PAUSE_TIME.ToString());
            writer.WriteElementString("CanReflectHorizontal", canReflectHorizontal.ToString());
            writer.WriteElementString("CanReflectVertical", canReflectVertical.ToString());
            writer.WriteElementString("CanReflectDiagonal", canReflectDiagonal.ToString());
            writer.WriteElementString("ReflectionLinePosition", reflection_line_position.ToString());
            writer.WriteStartElement("ReflectionOrientation");
            switch (ref_orientation)
            {
                case ReflectionOrientation.DIAGONAL:
                    writer.WriteString("D"); break;
                case ReflectionOrientation.HORIZONTAL:
                    writer.WriteString("H"); break;
                case ReflectionOrientation.VERTICAL:
                    writer.WriteString("V"); break;
            }
            writer.WriteEndElement();

            foreach (Object o in horizontal_lines)
            {
                writer.WriteElementString("HRLine", o.ToString());
            }
            foreach (Object o in vertical_lines)
            {
                writer.WriteElementString("VRLine", o.ToString());
            }
            foreach (Object o in diagonal_lines)
            {
                writer.WriteElementString("DLine", o.ToString());
            }

            if (dialog_displayer != null)
                dialog_displayer.Serialize(writer);

            writer.WriteElementString("Initialize", "");

            foreach (PhysicsObject o in objects)
            {
                o.Serialize(writer);
            }

            writer.WriteEndElement();
        }

        internal void Buddydeath()
        {
            buddydeath++;
        }
    }
}
