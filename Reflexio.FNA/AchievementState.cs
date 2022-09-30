using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace Reflexio
{
    public class AchievementState
    {
        
        /*Protip: search for "Achievement logic" to find where exactly achievement methods are called"*/

        //All achievements in the game. 
        //Arrays of the form: short title, full (display) title, description, Icon Texture
        public static string[][] achievements = 
        {
            new string[] {"n00b", "n00b", "Complete all levels in World 1", "ach_noob"},
            new string[] {"joey", "Still Just a Joey", "Complete 20 levels", "ach_joey"},
            new string[] {"mirror", "Mirror Master", "Complete 30 levels", "ach_mirror"},
            new string[] {"umbrella", "Umbrella Wizard", "Complete 40 levels", "ach_umbrella"},
            new string[] {"master", "Master Reflect-er", "Complete every level in Reflexio", "ach_master"},
            new string[] {"bear", "What a Bear", "Complete \"The Bear\"", "ach_bear"},
            new string[] {"tunnel", "Carpal Tunnel", "Reflect 1000 times", "ach_tunnel"},
            new string[] {"endurance", "Endurance", "Total play time over 5 hours", "ach_endurance"},
            new string[] {"stubbornness", "Stubbornness", "Fail a level 10 consecutive times without quitting", "ach_stubbornness"},
            new string[] {"eureka", "Eureka!", "Complete \"Wave\" using only 1 reflection", "ach_eureka"},
            new string[] {"failure", "Failure is an Option", "Fail in every way possible", "ach_failure"},
            new string[] {"aplus", "A+", "Complete \"The Test\" without dropping the buddy block", "ach_aplus"},
            new string[] {"speed", "Speed Run", "Complete \"Ladder\" in under 20 seconds", "ach_speed"},
        };

        public struct SaveState
        {
            public List<String> ach_unlocked;
            public int reflections;
            public long play_time;
            public List<int> levels_completed;
            public bool fail_fall;
            public bool fail_spike;
            public bool fail_spike_collision;
            public bool fail_block;
            public bool fail_wall;
        }
        public static string save_filename = "Content\\reflexio.sav";

        private bool is_desura = false;
        private bool is_indiecity = false;

        private Dictionary<string, bool> status; //String key is short achievement title, value is true if unlocked
        private Dictionary<int, bool> levels_completed; //Value is true if level is completed
        private Dictionary<string, string> level_mapping; //Mapping from short title to long title
        private int ach_complete;
        private int levels_count;
        private int WORLD_COMPLETION_THRESHHOLD = 6; //Must complete 6/8 levels to advance
        public bool game_progress_exists;

        private static Stopwatch savetimer = new Stopwatch();


        //Specific achievement tracking variables
        #region VARS
        //Failure is an Option ach
        private bool fail_fall = false;
        private bool fail_spike = false;
        private bool fail_spike_collision = false;
        private bool fail_block = false;
        private bool fail_wall = false;

        //Stubbornness ach
        private int consecutive_fails = 0;

        //Carpal Tunnel ach
        private int reflection_count = 0;
        private int reflection_goal = 1000;

        //Endurance ach
        private long play_time = 0;
        private long play_time_goal = 5 * 60 * 60 * 1000; //in milliseconds
        private bool play_time_goal_met = false;

        //Level unlock achievements
        private bool noob_completed = false;
        private bool joey_completed = false;
        private bool mirror_completed = false;
        private bool umbrella_completed = false;
        private bool master_completed = false;

        //Bear ach
        private int bear_level_number = 50;
        
        //Test ach
        private int the_test_level_number = 47;

        //Speed ach
        private int ladder_time_goal_ms = 20000;
        private int ladder_level_num = 21;

        //Eureka ach
        private int current_lvl_reflections;
        private int wave_level_num = 23;
        #endregion

        private void Initialize()
        {
            ach_complete = 0;
            levels_count = 0;
            game_progress_exists = false;
            status = new Dictionary<string, bool>();
            levels_completed = new Dictionary<int, bool>();
            foreach (string[] s in achievements)
                status.Add(s[0], false);

            for (int i = 0; i < GameEngine.level_files.Length; i++)
                levels_completed.Add(i, false);
            
            if (!savetimer.IsRunning)
            {
                savetimer.Restart();
            }

            level_mapping = new Dictionary<string,string>();
            foreach (string[] arr in achievements)
            {
                level_mapping.Add(arr[0], arr[1]);
            }

            current_lvl_reflections = 0;
        }

        public AchievementState()
        {
            Initialize();
        }

        public AchievementState(string[] complete)
        {
            Initialize();
            int i = 0;
            foreach (string s in complete)
            {
                try
                {
                    status[s] = true;
                    i++;
                }
                catch (KeyNotFoundException) { continue; }
            }
            ach_complete = i;
        }

        public AchievementState(SaveState state)
        {
            Initialize();
            //Initialize achievements completed
            int i = 0;
            foreach (string s in state.ach_unlocked)
            {
                try
                {
                    status[s] = true;
                    i++;
                }
                catch (KeyNotFoundException) { continue; }
            }
            ach_complete = i;

            //Initialize levels completed
            i = 0;
            foreach (int lvl in state.levels_completed)
            {
                try
                {
                    levels_completed[lvl] = true;
                    i++;
                }
                catch (KeyNotFoundException) { continue; }
            }
            levels_count = i;

            //Initialize reflection count
            reflection_count = state.reflections;
            //Initialize play time
            play_time = state.play_time;

            fail_block = state.fail_block;
            fail_fall = state.fail_fall;
            fail_spike = state.fail_spike;
            fail_spike_collision = state.fail_spike_collision;
            fail_wall = state.fail_wall;
        }

        public bool isWorldUnlocked(int world_num)
        {
            if (world_num <= 0) { world_num = 1; }

            return isWorldCompleted(world_num - 1);
        }

        public bool isWorldCompleted(int world_num)
        {
            if (world_num < 1) { return true; }

            int count = 0;
            for (int i = 8 * (world_num - 1); i <= (world_num * 8) - 1; i++)
            {
                if (isLevelCompleted(i)) { count++; }
            }
            return count >= WORLD_COMPLETION_THRESHHOLD;
        }

        public bool isLevelCompleted(int level_num)
        {
            try
            {
                return levels_completed[level_num];
            }
            catch
            {
                return false;
            }
        }


        /** Marks each level in the input list as completed */
        public void update_completed_levels(int[] complete)
        {
            foreach (int i in complete)
            {
                try
                {
                    levels_completed[i] = true;
                }
                catch (KeyNotFoundException) { continue; }
            }
        }

        /** Returns true if the given achievement has already
         * been unlocked.  title should be the "short title" for the 
         * achievement **/
        public bool is_unlocked(string title)
        {
            try
            {
                return status[title];
            }
            catch (KeyNotFoundException) { return false; }
        }

        /** Unlocks an achievement with the given short title.
         * Returns true if the achievement is newly unlocked, and false
         * if the achievement is already unlocked **/
        public bool unlock_achievement(string title)
        {
            if (is_unlocked(title))
                return false;

            try
            {
                status[title] = true;
                ach_complete++;

                //Draw achievement notification
                try { GameEngine.Instance.currentLevel.drawAchievement("Achievement Unlocked: " + level_mapping[title]); }
                catch (KeyNotFoundException) { GameEngine.Instance.currentLevel.drawAchievement("Achievement Unlocked: " + title); }

                //Save progress
                toSaveFile(toSaveState());
            }
            catch (KeyNotFoundException) { return false; }

            if (is_desura) desura_unlock(title);
            else if (is_indiecity) indiecity_unlock(title);

            return true;
        }

        private void desura_unlock(string title)
        {
        }

        private void indiecity_unlock(string title)
        {
        }

        public bool can_progress(int current_level)
        {
            //Cannot progress past the end of the game
            if (current_level >= GameEngine.Instance.MAX_LEVEL_NUM)
                return false;

            //If level is not on a world boundary, you can progress
            if (current_level % 8 != 7)
                return true;
            else
            {
                int worldnum = (current_level / 8) + 1;
                return isWorldCompleted(worldnum);
            }
        }

        public GameEngine.GameState get_menu_world_state()
        {
            return get_menu_world_state(get_max_completed());
        }

        public GameEngine.GameState get_menu_world_state(int level)
        {
            switch ((level / 8) + 1)
            {
                case 1:
                    return GameEngine.GameState.WORLD1_MENU;
                case 2:
                    return GameEngine.GameState.WORLD2_MENU;
                case 3:
                    return GameEngine.GameState.WORLD3_MENU;
                case 4:
                    return GameEngine.GameState.WORLD4_MENU;
                case 5:
                    return GameEngine.GameState.WORLD5_MENU;
                case 6:
                    return GameEngine.GameState.WORLD6_MENU;
                case 7:
                    return GameEngine.GameState.WORLD7_MENU;
            }
            return GameEngine.GameState.LEVEL_MENU;
        }

        private int get_max_completed()
        {
            int max_completed = -1;
            foreach (int i in levels_completed.Keys)
            {
                if (i > max_completed && isLevelCompleted(i))
                    max_completed = i;
            }
            return max_completed;
        }


        public int get_starting_level()
        {
            int max_completed = get_max_completed();

            //No levels completed
            if (max_completed == -1)
                return 0;

            if (can_progress(max_completed))
                return max_completed + 1;
            else
            {
                return -1;
            }
        }

        #region FAILURE_ACHIEVEMENT_LOGIC
        public bool death_by_fall()
        {
            fail_fall = true;
            if (fail_block && fail_spike && fail_wall && fail_spike_collision)
                return unlock_achievement("failure");
            else
                return false;
        }
        public bool death_by_spike_collision()
        {
            fail_spike_collision = true;
            if (fail_fall && fail_block && fail_spike && fail_wall)
                return unlock_achievement("failure");
            else
                return false;
        }
        public bool death_by_block()
        {
            fail_block = true;
            if (fail_fall && fail_spike && fail_wall && fail_spike_collision)
                return unlock_achievement("failure");
            else
                return false;
        }
        public bool death_by_spike()
        {
            fail_spike = true;
            if (fail_fall && fail_block && fail_wall && fail_spike_collision)
                return unlock_achievement("failure");
            else
                return false;
        }
        public bool death_by_wall()
        {
            fail_wall = true;
            if (fail_fall && fail_block && fail_spike && fail_spike_collision)
                return unlock_achievement("failure");
            else
                return false;
        }
        #endregion

        #region STUBBORNNESS_ACH
        public bool inc_consecutive_fails()
        {
            consecutive_fails++;
            if (consecutive_fails >= 10)
                return unlock_achievement("stubbornness");
            else
                return false;
        }
        public void reset_consecutive_fails()
        {
            consecutive_fails = 0;
        }
        public int get_fails() { return consecutive_fails; }
        #endregion

        #region ENDURANCE_ACH
        public bool inc_play_time(int ms)
        {
            if (!play_time_goal_met)
            {
                play_time += ms;
                if (play_time >= play_time_goal)
                {
                    play_time_goal_met = true;
                    return unlock_achievement("endurance");
                }
                else
                    return false;
            }
            return false;
        }
        #endregion

        #region CARPAL_TUNNEL_ACH
        public bool inc_reflection_count()
        {
            reflection_count++;
            current_lvl_reflections++;
            if (reflection_count >= reflection_goal)
                return unlock_achievement("tunnel");
            else
                return false;
        }

        public void reset_current_reflections()
        {
            current_lvl_reflections = 0;
        }
        #endregion

        #region LEVEL_COMPLETION_ACH
        public void complete_level(int level_id, int blocks_destroyed)
        {
            if (level_id < 0)
                return;

            levels_completed[level_id] = true;
            game_progress_exists = true;

            if(!noob_completed && check_noob()){
                    unlock_achievement("n00b");
                    noob_completed = true;
            }

            int count = tally_levels_completed();
            if (!joey_completed && count >= 20)
            {
                unlock_achievement("joey");
                joey_completed = true;
            }

            if (!mirror_completed && count >= 30)
            {
                unlock_achievement("mirror");
                mirror_completed = true;
            }

            if (!umbrella_completed && count >= 40)
            {
                unlock_achievement("umbrella");
                umbrella_completed = true;
            }

            if (!master_completed && all_complete())
            {
                unlock_achievement("master");
                master_completed = true;
            }

            if (level_id == bear_level_number)
                unlock_achievement("bear");

            if (level_id == the_test_level_number && blocks_destroyed == 0)
                unlock_achievement("aplus");

            if (current_lvl_reflections == 1 && level_id == wave_level_num)
                unlock_achievement("eureka");
        }
        private bool check_noob()
        {
            for (int i = 0; i < 8; i++)
                if (!levels_completed[i])
                    return false;
            return true;
        }
        private int tally_levels_completed()
        {
            int count = 0;
            for (int i = 0; i < levels_completed.Count; i++)
            {
                try
                {
                    if (levels_completed[i])
                        count++;
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }
            }
            return count;
        }
        private bool all_complete()
        {
            bool ret = true;
            for (int i = 0; i < levels_completed.Count; i++)
            {
                try
                {
                    ret = levels_completed[i] && ret;
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }
            }
            return ret;
        }
        #endregion

        #region SPEED_ACHS
        /**All speed related achievement logic goes here.  Level num is the level just
         * completed in the provided number of milliseconds */
        public void register_level_complete_time(int level_num, long milliseconds)
        {
            if (level_num == ladder_level_num && milliseconds < ladder_time_goal_ms)
                unlock_achievement("speed");
        }
        #endregion

        #region SAVEGAME_SERIALIZATION

        public SaveState toSaveState()
        {
            SaveState state = new SaveState();

            state.ach_unlocked = new List<string>();
            foreach (string key in status.Keys)
                if (status[key])
                    state.ach_unlocked.Add(key);

            state.levels_completed = new List<int>();
            foreach (int key in levels_completed.Keys)
                if (levels_completed[key])
                    state.levels_completed.Add(key);

            state.play_time = play_time;
            state.reflections = reflection_count;

            state.fail_block = fail_block;
            state.fail_fall = fail_fall;
            state.fail_spike = fail_spike;
            state.fail_spike_collision = fail_spike_collision;
            state.fail_wall = fail_wall;

            return state;
        }

        public static void toSaveFile(SaveState state)
        {
            //Prevent multiple immediate saves (< 5 seconds ago)
            if (savetimer.ElapsedMilliseconds <= 5000) { return; }

            Console.WriteLine("Saving...");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(save_filename, settings);

            writer.WriteProcessingInstruction("xml", @"version = ""1.0""");
            writer.WriteStartElement("save");

            //Serialize levels completed
            //writer.WriteStartElement("levels_completed");
            foreach(int lvl in state.levels_completed)
                writer.WriteElementString("level", lvl.ToString());
            //writer.WriteEndElement();

            //Serialize achievements completed
            //writer.WriteStartElement("achievements_completed");
            foreach (string s in state.ach_unlocked)
                writer.WriteElementString("achievement", s);
            //writer.WriteEndElement();

            //Serialize other vars
            writer.WriteElementString("playtime", state.play_time.ToString());
            writer.WriteElementString("reflections", state.reflections.ToString());

            //writer.WriteStartElement("failures");
            writer.WriteElementString("fail_spike", state.fail_spike.ToString());
            writer.WriteElementString("fail_wall", state.fail_wall.ToString());
            writer.WriteElementString("fail_block", state.fail_block.ToString());
            writer.WriteElementString("fail_spike_collision", state.fail_spike_collision.ToString());
            writer.WriteElementString("fail_fall", state.fail_fall.ToString());
            //writer.WriteEndElement();

            writer.WriteEndElement();
            writer.Close();

            savetimer.Restart();
        }

        public static SaveState getSaveState()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            XmlReader reader = XmlReader.Create(save_filename, settings);

            SaveState state = new SaveState();
            state.ach_unlocked = new List<string>();
            state.levels_completed = new List<int>();
            state.reflections = 0;
            state.play_time = 0;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "level":
                            state.levels_completed.Add(reader.ReadElementContentAsInt());
                            break;
                        case "achievement":
                            state.ach_unlocked.Add(reader.ReadElementContentAsString());
                            break;
                        case "playtime":
                            state.play_time = reader.ReadElementContentAsLong();
                            break;
                        case "reflections":
                            state.reflections = reader.ReadElementContentAsInt();
                            break;
                        case "fail_spike":
                            state.fail_spike = reader.ReadElementContentAsString().Equals("True");
                            break;
                        case "fail_wall":
                            state.fail_wall = reader.ReadElementContentAsString().Equals("True");
                            break;
                        case "fail_fall":
                            state.fail_fall = reader.ReadElementContentAsString().Equals("True");
                            break;
                        case "fail_spike_collision":
                            state.fail_spike_collision = reader.ReadElementContentAsString().Equals("True");
                            break;
                        case "fail_block":
                            state.fail_block = reader.ReadElementContentAsString().Equals("True");
                            break;
                        case "*":
                            break;
                    }
                }
            }

            return state;

        }

        public static AchievementState fromSaveFile()
        {
            try
            {
                SaveState state = getSaveState();
                AchievementState s = new AchievementState(state);
                s.game_progress_exists = true;
                return s;
            }
            catch
            {
                return new AchievementState();
            }
        }
        #endregion

    }
}
