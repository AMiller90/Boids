
namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    /// The algorithm.
    /// </summary>
    public class BoidsAlgorithm : MonoBehaviour
    {
        /// <summary>
        /// The flocks.
        /// </summary>
        [SerializeField]
        private Dictionary<int, List<MonoBoid>> flocks;

        /// <summary>
        /// The awake.
        /// </summary>
        private void Awake()
        {
            this.flocks = new Dictionary<int, List<MonoBoid>>();

            this.SortBoidsIntoFlocks(FindObjectsOfType<MonoBoid>().ToList());
        }

        /// <summary>
        /// The update.
        /// </summary>
        private void Update()
        {
            this.Algorithm();
            Controller.Self.UpdateUiText();
        }

        /// <summary>
        /// The algorithm.
        /// </summary>
        private void Algorithm()
        {
            for (int i = 0; i < Controller.Self.NumberOfFlocks; i++)
            {
                foreach (MonoBoid monoagent in this.flocks[i])
                {
                    Vector3 v1 = this.Cohesion(monoagent);
                    Vector3 v2 = this.Alignment(monoagent);
                    Vector3 v3 = this.Dispersion(monoagent);

                    monoagent.Agent.Velocity = monoagent.Agent.Velocity + v1 + v2 + v3;
                    this.LimitSpeed(monoagent);
                }
            }
        }

        /// <summary>
        /// The cohesion, rule 1.
        /// </summary>
        /// <param name="a_Boid">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 Cohesion(MonoBoid a_Boid)
        {
            Vector3 centreMass = Vector3.zero;

            for (int i = 0; i < Controller.Self.NumberOfFlocks; i++)
            {
                foreach (MonoBoid a in this.flocks[i])
                {
                    if (a.Agent != a_Boid.Agent)
                    {
                        centreMass = centreMass + a.Agent.Position;
                    }
                }
            }

            centreMass = centreMass / (Controller.Self.NumberOfBoids - 1);

            return ((centreMass - a_Boid.Agent.Position) / 1000) * Controller.Self.Cohesionvalue;
        }

        /// <summary>
        /// The alignment, rule 2.
        /// </summary>
        /// <param name="a_Boid">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 Alignment(MonoBoid a_Boid)
        {
            Vector3 c = Vector3.zero;

            for (int i = 0; i < Controller.Self.NumberOfFlocks; i++)
            {
                foreach (MonoBoid monoagent in this.flocks[i])
                {
                    if (monoagent.Agent != a_Boid.Agent)
                    {
                        Vector3 distanceVec = monoagent.Agent.Position - a_Boid.Agent.Position;

                        float dist = Vector3.Magnitude(distanceVec);

                        if (dist < Controller.Self.Alignmentvalue)
                        {
                            c = c - (monoagent.Agent.Position - a_Boid.Agent.Position);
                        }
                    }
                }
            }
            return c;
        }

        /// <summary>
        /// The dispersion, rule 3.
        /// </summary>
        /// <param name="a_Boid">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="Vector3"/>.
        /// </returns>
        private Vector3 Dispersion(MonoBoid a_Boid)
        {
            Vector3 vel = Vector3.zero;

            for (int i = 0; i < Controller.Self.NumberOfFlocks; i++)
            {
                foreach (MonoBoid a in this.flocks[i])
                {
                    if (a.Agent != a_Boid.Agent)
                    {
                        vel = vel + a.Agent.Velocity;
                    }
                }
            }
            vel = vel / (Controller.Self.NumberOfBoids - 1);

            return ((vel - a_Boid.Agent.Velocity) / 8) * Controller.Self.Dispersionvalue;
        }

        /// <summary>
        /// The limit speed.
        /// </summary>
        /// <param name="a_Boid">
        /// The object.
        /// </param>
        private void LimitSpeed(MonoBoid a_Boid)
        {
            int vlim = 1000;
            if (a_Boid.Agent.Velocity.magnitude > vlim)
            {
                a_Boid.Agent.Velocity = (a_Boid.Agent.Velocity / a_Boid.Agent.Velocity.magnitude) * vlim;
            }
        }

        /// <summary>
        /// The sort objects into flocks.
        /// </summary>
        /// <param name="a_Boids">
        /// The list of objects.
        /// </param>
        private void SortBoidsIntoFlocks(List<MonoBoid> a_Boids)
        {
            int boidsperflock = Controller.Self.NumberOfBoids / Controller.Self.NumberOfFlocks;

            int totalBoids = 0;

            int num = 0;
            for (int i = 0; i < Controller.Self.NumberOfFlocks; i++)
            {
                num++;
                this.flocks.Add(i, new List<MonoBoid>());
            
                for (int j = 0; j < boidsperflock; j++)
                {
                    this.flocks[i].Add(a_Boids[totalBoids]);

                    if (Controller.Self.FlocksProperty[i].name == "Flock " + num)
                    {
                        a_Boids[totalBoids].transform.SetParent(Controller.Self.FlocksProperty[i].transform);
                    }

                    totalBoids++;
                }
            }
        }
    }
}
