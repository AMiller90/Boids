
namespace Assets.Scripts
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The Agent Class.
    /// </summary>
    [Serializable]
    public class Agent
    {
        /// <summary>
        /// The velocity.
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// The position.
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// The mass.
        /// </summary>
        private float mass;

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="a_Mass">
        /// The a_ Mass.
        /// </param>
        public Agent(float a_Mass)
        {
            this.velocity = new Vector3();
            this.position = new Vector3();

            this.mass = (a_Mass <= 0) ? 1 : a_Mass;
        }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        public Vector3 Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        /// <summary>
        /// Gets or sets the mass.
        /// </summary>
        public float Mass
        {
            get { return this.mass; }
            set { this.mass = value; }
        }

    }
}
