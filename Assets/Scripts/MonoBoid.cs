
namespace Assets.Scripts
{
    using UnityEngine;

    /// <summary>
    /// The class to represent the object.
    /// </summary>
    [SerializeField]
    public class MonoBoid : MonoBehaviour
    {
        /// <summary>
        /// The a.
        /// </summary>
        private Agent agent;

        /// <summary>
        /// The mass.
        /// </summary>
        [SerializeField]
        private float mass;

        /// <summary>
        /// Gets the agent.
        /// </summary>
        public Agent Agent
        {
            get { return this.agent; }
        }
     

        /// <summary>
        /// The awake.
        /// </summary>
        private void Awake()
        {
            this.mass = 10;
            this.agent = new Agent(this.mass) { Position = this.gameObject.transform.position, Velocity = Vector3.zero };
        }

        /// <summary>
        /// The late update.
        /// </summary>
        private void LateUpdate()
        {
            this.agent.Position = this.agent.Position + this.agent.Velocity;
            this.gameObject.transform.position = this.agent.Position;
        }
    }
}

