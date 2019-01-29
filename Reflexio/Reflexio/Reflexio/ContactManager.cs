using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;

namespace Reflexio
{
    public class ContactManager
    {
        #region MEMBER VARIABLES
        static float CONTACT_ORIENTABLE_EPS = 0.0f;
        static float INSIDE_OBJECT_EPS = 0.01f;
        static List<object> objects_in_contact = new List<object>();
        static List<object> spikes_in_contact = new List<object>();
        #endregion

        #region MAIN_CONTACT_CLASSES

        public static bool BeginContact(Contact contact)
        {
            if (GameEngine.Instance.currentLevel.is_peeking)
                return false;

            var obj1 = contact.FixtureA.Body.UserData;
            var obj2 = contact.FixtureB.Body.UserData;
            var body1 = contact.FixtureA.Body;
            var body2 = contact.FixtureB.Body;
            var ud1 = contact.FixtureA.UserData;
            var ud2 = contact.FixtureB.UserData;

            if ((obj1 is Player && !(obj2 is Player)) ||
                (obj2 is Player && !(obj1 is Player)))
            {
                Player p = obj1 is Player ? (Player)obj1 : (Player)obj2;
                var other = obj1 is Player ? obj2 : obj1;
                if(!objects_in_contact.Contains(other))
                    objects_in_contact.Add(other);
                
                if (other is Reflexio.Wall)
                {
                    ReflectableObject obj = (ReflectableObject)other;
                    if (CheckPlayerInsideObject(obj))
                    {
                        GameEngine.Instance.currentLevel.SetGameOver(false);
                        GameEngine.Instance.achievement_state.death_by_wall(); //Achievement Logic - 'failure is an option'
                    }
                    PlayerGroundedStart(contact);
                    return true;
                }
                // Collision with Block
                else if (other is Reflexio.Block)
                {
                    
                    ReflectableObject obj = (ReflectableObject)other;
                    if (CheckPlayerInsideObject(obj))
                    {
                        GameEngine.Instance.currentLevel.SetGameOver(false);
                        GameEngine.Instance.achievement_state.death_by_block(); //Achievement logic - 'failure is an option'
                    }
                    PlayerGroundedStart(contact);
                    return true;
                }
                // Collision With collectible
                else if (other is Reflexio.Collectible)
                {
                    if(!((Collectible)other).is_inside_non_reflectable_object)
                        ((Collectible)other).CollectedByPlayer();
                }
                // Collision with Spike
                else if (other is Reflexio.Spike)
                {
                    Spike s = (Reflexio.Spike)other;
                    if (spikes_in_contact.Contains(s))
                        return false;
                    if (CheckPlayerInsideObject((ReflectableObject)s))
                    {
                        GameEngine.Instance.currentLevel.SetGameOver(false);
                        GameEngine.Instance.achievement_state.death_by_spike_collision(); //Achievement logic - 'failure is an option'
                    }
                    bool consider = PlayerSpikeContactStarted(s);
                    if (!consider)
                        return false;
                    PlayerGroundedStart(contact);
                    return true;
                }
                // Collision with Door
                else if (other is Reflexio.Door)
                {
                    Door d = (Reflexio.Door)other;
                    if (d.IsOpen() && !d.is_inside_non_reflectable_object && CheckPlayerInsideObject(d))
                    {
                        GameEngine.Instance.currentLevel.SetGameOver(true);
                        //Achievement logic - speed achievements and completion achievements
                        GameEngine.Instance.achievement_state.register_level_complete_time(GameEngine.Instance.currentLevelPos, GameEngine.Instance.currentLevelStopwatch.ElapsedMilliseconds);
                        GameEngine.Instance.achievement_state.complete_level(GameEngine.Instance.currentLevelPos,
                            GameEngine.Instance.currentLevel.buddydeath);
                        AchievementState.toSaveFile(GameEngine.Instance.achievement_state.toSaveState());
                    }
                    //else
                    //    PlayerGroundedStart(contact);
                    return false;
                }
            }

            if (obj1 is Switch && !(obj2 is Player) || obj2 is Switch && !(obj1 is Player))
            {
                Switch s = obj1 is Switch ? (Switch)obj1 : (Switch)obj2;
                s.PressSwitch((PhysicsObject)(obj1 is Switch ? obj2 : obj1));
                return false;
            }

            if (obj1 is Block || obj2 is Block)
            {
                object other = obj1 is Block ? obj2 : obj1;
                return !(other is Key || other is Switch);
            }
            return false;
        }

        public static void EndContact(Contact contact)
        {
            if (GameEngine.Instance.currentLevel.is_peeking)
                return;

            var obj1 = contact.FixtureA.Body.UserData;
            var obj2 = contact.FixtureB.Body.UserData;
            var body1 = contact.FixtureA.Body;
            var body2 = contact.FixtureB.Body;
            var ud1 = contact.FixtureA.UserData;
            var ud2 = contact.FixtureB.UserData;
            
            if ((obj1 is Player && !(obj2 is Player)) ||
                (obj2 is Player && !(obj1 is Player)))
            {
                Player p = obj1 is Player ? (Player)obj1 : (Player)obj2;
                var other = obj1 is Player ? obj2 : obj1;
                objects_in_contact.Remove(other);

                if (other is Reflexio.Wall)
                    PlayerGroundedEnd(contact);
                else if (other is Reflexio.Block)
                {
                    Block b = (Block)other;
                    PlayerGroundedEnd(contact);
                }
                else if (other is Reflexio.Spike)
                {
                    spikes_in_contact.Remove(other);
                    PlayerGroundedEnd(contact);
                }
                else if (other is Reflexio.Door)
                    PlayerGroundedEnd(contact);
            }

            /*if (!(obj1 is Wall || obj2 is Wall))
            {
                if (obj1 is Switch || obj2 is Switch)
                {
                    Switch s = obj1 is Switch ? (Switch)obj1 : (Switch)obj2;
                    s.ReleaseSwitch();
                }
            }*/
        }
        #endregion

        #region Check Grounded Methods
        public static void PlayerGroundedStart(Contact contact)
        {

            var body1 = contact.FixtureA.Body;
            var body2 = contact.FixtureB.Body;
            var ud1 = contact.FixtureA.UserData;
            var ud2 = contact.FixtureB.UserData;

            Player player = GameEngine.Instance.currentLevel.player;
            if ((Level.dudeSensorName.Equals(ud2) && player != body1.UserData) ||
               (Level.dudeSensorName.Equals(ud1) && player != body2.UserData))
            {
                player.IsGrounded = true;
                player.sensorFixtures.Add(player == contact.FixtureA.UserData ? contact.FixtureB : contact.FixtureA);
            }
        }

        public static void PlayerGroundedEnd(Contact contact)
        {
            var body1 = contact.FixtureA.Body;
            var body2 = contact.FixtureB.Body;
            var ud1 = contact.FixtureA.UserData;
            var ud2 = contact.FixtureB.UserData;

            Player player = GameEngine.Instance.currentLevel.player;
            if ((Level.dudeSensorName.Equals(ud2) && player != body1.UserData) ||
               (Level.dudeSensorName.Equals(ud1) && player != body2.UserData))
            {
                player.sensorFixtures.Remove(player == contact.FixtureA.UserData ? contact.FixtureB : contact.FixtureA);
                if (player.sensorFixtures.Count == 0)
                    player.IsGrounded = false;
            }
        }

        public static void CheckIsGroundedAfterReflection()
        {
            Player player = GameEngine.Instance.currentLevel.player;
            foreach(ReflectableObject obj in objects_in_contact)
            {
                try
                {
                    if (Math.Abs(player.Body.Position.X - obj.Body.Position.X) <= player.width / 2 + obj.width / 2 + 0.1f &&
                        Math.Abs(player.Body.Position.Y - obj.Body.Position.Y) <= player.height / 2 + obj.height / 2 + 0.1f)
                        return;
                }
                catch { }
            }
            if(!GameEngine.Instance.currentLevel.is_peeking)
                player.IsGrounded = false;
        }
        #endregion


        #region Player To Spike
        private static bool PlayerSpikeContactStarted(Reflexio.Spike spike)
        {
            if (spike.is_inside_non_reflectable_object)
                return true;

            Player p = GameEngine.Instance.currentLevel.player;
            bool spiked = ContactOrientableTest(p.Body.Position, spike.Body.Position, p.width, p.height,
                                    spike.width, spike.height, spike.direction);

            //if (spiked)
                //GameEngine.Instance.currentLevel.SetGameOver(false);
            if (spiked)
            {
                spikes_in_contact.Add(spike);
                return false;
            }
            else
                return true;
        }

        public static void CheckSpiked()
        {
            object[] spikes = (spikes_in_contact.ToArray());
            Player p = GameEngine.Instance.currentLevel.player;
            Vector2 ppos = p.Body.Position;
            float pw = p.width / 2;
            float ph = p.height / 2;
            float eps = 0.1f;
            int i = 0;
            foreach (object o in spikes)
            {
                Spike s = (Spike)(o);
                Vector2 spos = s.Body.Position;                
                float sw = s.width/2;
                float sh = s.height/2;                
                float diffX = ppos.X - spos.X;
                float diffY = ppos.Y - spos.Y;
                bool is_up = false;
                bool is_left = false;
                if (diffX < 0)
                    is_left = true;
                if (diffY < 0)
                    is_up = true;

                diffX = Math.Abs(diffX);
                diffY = Math.Abs(diffY);

                if (Math.Abs(ppos.X - spos.X) >= pw + sw || Math.Abs(ppos.Y - spos.Y) >= ph + sh)
                {
                    spikes_in_contact.RemoveAt(i);
                    i--;
                }
                else if (diffX < sw + pw - eps && diffY < sh + ph - eps)
                {
                    if ((s.direction == ReflectableAndOrientable.Direction.Up && is_up) ||
                        (s.direction == ReflectableAndOrientable.Direction.Down && !is_up) ||
                        (s.direction == ReflectableAndOrientable.Direction.Left && is_left) ||
                        (s.direction == ReflectableAndOrientable.Direction.Right && !is_left))
                    {
                        GameEngine.Instance.currentLevel.SetGameOver(false);
                        GameEngine.Instance.achievement_state.death_by_spike(); //Achievement logic - 'failure is an option'
                    }
                }
                i++;
            }
        }
        #endregion

        #region MISC
        public static void ClearContactList()
        {
            objects_in_contact.Clear();
            spikes_in_contact.Clear();
        }
        public static void CheckOrientationChangedOnReflection()
        {
            foreach (object o in objects_in_contact)
            {
                if (o is Spike)
                {
                    if (!PlayerSpikeContactStarted((Spike)o))
                    {
                        GameEngine.Instance.currentLevel.SetGameOver(false);
                        GameEngine.Instance.achievement_state.death_by_spike(); //Achievement logic - 'failure is an option'
                    }
                }
            }
        }

        private static bool CheckPlayerInsideObject(ReflectableObject obj)
        {
            float diff = (GameEngine.Instance.currentLevel.player.Body.Position - obj.Body.Position).Length();
            if (diff < obj.width / 2 - INSIDE_OBJECT_EPS)
                return true;
            else
                return false;
        }

        private static bool ContactOrientable(Vector2 ppos, Vector2 spos, float pw, float ph, float sw, float sh, ReflectableAndOrientable.Direction dir)
        {
            pw /= 2;
            ph /= 2;
            sw /= 2;
            sh /= 2;
            float eps = ContactManager.CONTACT_ORIENTABLE_EPS;

            if (Math.Abs(ppos.X - spos.X) >= pw + sw + 0.1 || Math.Abs(ppos.Y - spos.Y) >= ph + sh + 0.1)
                return false;

            /*if (dir == ReflectableAndOrientable.Direction.Left && ppos.X + pw - eps <= spos.X - sw)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Right && ppos.X - pw + eps >= spos.X + sw)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Up && ppos.Y + ph - eps <= spos.Y - sh)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Down && ppos.Y - ph + eps >= spos.Y + sh)
                return true;*/

            eps = 0.00f;
            if (dir == ReflectableAndOrientable.Direction.Left && ppos.X + pw - eps <= spos.X - sw && 
                ppos.Y + ph >= spos.Y - sh + eps && ppos.Y - ph <= spos.Y + sh - eps)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Right && ppos.X - pw + eps >= spos.X + sw && 
                ppos.Y + ph >= spos.Y - sh + eps && ppos.Y - ph <= spos.Y + sh - eps)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Up && ppos.Y + ph - eps <= spos.Y - sh && 
                ppos.X + pw >= spos.X - sw + eps && ppos.X - pw <= spos.X + sw - eps)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Down && ppos.Y - ph + eps >= spos.Y + sh && 
                ppos.X + pw >= spos.X - sw + eps && ppos.X - pw <= spos.X + sw - eps)
                return true;
            return false;
        }


        private static bool ContactOrientableTest(Vector2 ppos, Vector2 spos, float pw, float ph, float sw, float sh, ReflectableAndOrientable.Direction dir)
        {
            pw /= 2;
            ph /= 2;
            sw /= 2;
            sh /= 2;
            float eps = 0;

            if (Math.Abs(ppos.X - spos.X) >= pw + sw + 0.1 || Math.Abs(ppos.Y - spos.Y) >= ph + sh + 0.1)
                return false;

            eps = 0.05f;
            if (dir == ReflectableAndOrientable.Direction.Left && ppos.X + pw - eps <= spos.X - sw &&
                ppos.Y + ph >= spos.Y - sh + eps && ppos.Y - ph <= spos.Y + sh - eps)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Right && ppos.X - pw + eps >= spos.X + sw &&
                ppos.Y + ph >= spos.Y - sh + eps && ppos.Y - ph <= spos.Y + sh - eps)
            {
                return true;
            }
            else if (dir == ReflectableAndOrientable.Direction.Up && ppos.Y + ph - eps <= spos.Y - sh &&
                ppos.X + pw >= spos.X - sw + eps && ppos.X - pw <= spos.X + sw - eps)
                return true;
            else if (dir == ReflectableAndOrientable.Direction.Down && ppos.Y - ph + eps >= spos.Y + sh &&
                ppos.X + pw >= spos.X - sw + eps && ppos.X - pw <= spos.X + sw - eps)
                return true;
            return false;
        }
        #endregion
    }
}
