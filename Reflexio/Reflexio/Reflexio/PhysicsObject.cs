using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Reflexio
{
    public abstract class PhysicsObject
    {
        /// <summary>
        /// The physics world of this object
        /// </summary>
        private World world;

        private bool isDead;
        private bool isAdded;
        private Body body;


        /// <summary>
        /// If our object has been flagged for destruction
        /// </summary>
        public bool IsDead
        {
            get { return isDead; }
            protected set { isDead = value; }
        }

        /// <summary>
        /// If our object has been added to the physics world
        /// </summary>
        public bool IsAdded
        {
            get { return isAdded; }
        }

        /// <summary>
        /// This object's body, if we have one.
        /// </summary
        public Body Body
        {
            get { return body; }
        }

        /// <summary>
        /// The world of this object.
        /// </summary>
        public World World
        {
            get { return world; }
        }
        
        // buffered values to set on the physics body after
        // it's created
        public Vector2? BufferedPosition { get; set; }
        public float? BufferedAngle { get; set; }
        public Vector2? BufferedLinearVelocity { get; set; }

        protected PhysicsObject(World world)
        {
            this.world = world;
            Initialize();
        }

        /// <summary>
        /// Initializes object state.
        /// </summary>
        protected void Initialize()
        {
            isDead = false;
            isAdded = false;
            BufferedAngle = null;
            BufferedPosition = null;
            BufferedLinearVelocity = null;
        }

        /// <summary>
        /// Draws the physics object.
        /// </summary>
        public virtual void Draw()
        {
        }

        /// <summary>
        /// Updates the object's game logic.  By default this does nothing.
        /// </summary>
        /// <param name="demoWorld"></param>
        /// <param name="dt"></param>
        public virtual void Update(Level level, float dt)
        {
        }

        /// <summary>
        /// Flags this object for death
        /// </summary>
        public void Die()
        {
            isDead = true;
        }

        public void CompletelyRemove()
        {
            Die();
            RemoveFromWorld();
        }

        /// <summary>
        /// Calls the physics creation method of this body.  Also
        /// populates the buffered position + angle if applicable.
        /// </summary>
        public virtual void AddToWorld()
        {
            body = CreatePhysics();
            if (body != null)
            {
                body.UserData = this;
                if (BufferedPosition.HasValue) body.Position = BufferedPosition.Value;
                if (BufferedAngle.HasValue) body.Rotation = BufferedAngle.Value;
                if (BufferedLinearVelocity.HasValue) body.LinearVelocity = BufferedLinearVelocity.Value;
            }
            isAdded = true;
        }

        /// <summary>
        /// Removes all of this body's physics and joints
        /// </summary>
        public virtual void RemoveFromWorld()
        {
            DestroyPhysics();
        }

        /// <summary>
        /// Calls the joints creation method.
        /// </summary>
        public virtual void AddJointsToWorld()
        {
            CreateJoints();
        }

        /// <summary>
        /// Reflects the body. Reflect if start &lt;= Discrete Position &lt;= end
        /// </summary>
        /// <param name="start">The start of the reflection region, in discrete units. INCLUSIVE.</param>
        /// <param name="end">The end of the reflection region, in discrete units. INCLUSIVE</param>
        public abstract void Reflect(int start, int end, Reflexio.Level.ReflectionOrientation ref_or);

        /// <summary>
        /// Creates the physics of this object.
        /// </summary>
        /// <returns>The resulting physics body</returns>
        protected abstract Body CreatePhysics();

        /// <summary>
        /// Creates the joints for this object.
        /// </summary>
        protected abstract void CreateJoints();

        /// <summary>
        /// Destroys the body of this object if it exists.
        /// </summary>
        protected virtual void DestroyPhysics()
        {
            if (body != null)
            {
                world.RemoveBody(body);
                body = null;
            }
        }

        public abstract void Serialize(XmlWriter writer);
    }
}
