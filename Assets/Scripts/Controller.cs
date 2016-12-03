
namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The controller.
    /// </summary>
    public class Controller : MonoBehaviour
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static Controller sInstance;

        /// <summary>
        /// The prefab.
        /// </summary>
        [SerializeField]
        private GameObject prefab;

        /// <summary>
        /// The number.
        /// </summary>
        [SerializeField]
        private int numberOfBoids;

        /// <summary>
        /// The number of flocks.
        /// </summary>
        [SerializeField]
        private int numberOfFlocks;

        /// <summary>
        /// The color.
        /// </summary>
        [SerializeField]
        private Color color;

        /// <summary>
        /// The canvas.
        /// </summary>
        [SerializeField]
        private Canvas canvas;

        /// <summary>
        /// The co text.
        /// </summary>
        private Text coText;

        /// <summary>
        /// The al text.
        /// </summary>
        private Text alText;

        /// <summary>
        /// The di text.
        /// </summary>
        private Text diText;

        /// <summary>
        /// The cohesion slider.
        /// </summary>
        [SerializeField]
        private Slider cohesionSlider;

        /// <summary>
        /// The alignment slider.
        /// </summary>
        [SerializeField]
        private Slider alignmentSlider;

        /// <summary>
        /// The dispersion slider.
        /// </summary>
        [SerializeField]
        private Slider dispersionSlider;

        /// <summary>
        /// The flock empties.
        /// </summary>
        private List<GameObject> flockEmpties;

        /// <summary>
        /// Gets the self.
        /// </summary>
        public static Controller Self
        {
            get
            {
                return sInstance;
            }
        }

        /// <summary>
        /// Gets the number of flocks.
        /// </summary>
        public int NumberOfFlocks
        {
            get
            {
                return this.numberOfFlocks;
            }
        }

        /// <summary>
        /// Gets the flocks property.
        /// </summary>
        public List<GameObject> FlocksProperty
        {
            get
            {
                return this.flockEmpties;
            }
        }

        /// <summary>
        /// Gets the Cohesion value.
        /// </summary>
        public float Cohesionvalue
        {
            get
            {
                return Mathf.Clamp(this.cohesionSlider.value, 0, 1);
            }
        }

        /// <summary>
        /// Gets the Alignment value.
        /// </summary>
        public float Alignmentvalue
        {
            get
            {
                return Mathf.Clamp(this.alignmentSlider.value, 0, 1);
            }
        }

        /// <summary>
        /// Gets the Dispersion value.
        /// </summary>
        public float Dispersionvalue
        {
            get
            {
                return Mathf.Clamp(this.dispersionSlider.value, 0, 1);
            }
        }

        /// <summary>
        /// Gets the number.
        /// </summary>
        public int NumberOfBoids
        {
            get
            {
                return this.numberOfBoids;
            }
        }

        /// <summary>
        /// The update UI text.
        /// </summary>
        public void UpdateUiText()
        {
            this.coText.text = "Cohesion: " + this.cohesionSlider.value;
            this.alText.text = "Alignment: " + this.alignmentSlider.value;
            this.diText.text = "Dispersion: " + this.dispersionSlider.value;
        }

        /// <summary>
        /// The awake.
        /// </summary>
        private void Awake()
        {
            sInstance = this;

            this.flockEmpties = new List<GameObject>();

            Mathf.Clamp(this.numberOfBoids, 0, 50);
            Mathf.Clamp(this.numberOfFlocks, 0, 10);

            this.FlockCheck();

            this.GenerateFlockEmptyObjects();

            this.GenerateAgents();

            if (this.canvas == null)
            {
                this.canvas = GameObject.Find("HUD").GetComponent<Canvas>();
            }

            if (this.coText == null)
            {
                this.coText = this.canvas.GetComponentsInChildren<Text>()[0];
            }

            if (this.alText == null)
            {
                this.alText = this.canvas.GetComponentsInChildren<Text>()[1];
            }

            if (this.diText == null)
            {
                this.diText = this.canvas.GetComponentsInChildren<Text>()[2];
            }

            if (this.cohesionSlider == null)
            {
                this.cohesionSlider = this.canvas.GetComponentsInChildren<Slider>()[0];
            }

            if (this.alignmentSlider == null)
            {
                this.alignmentSlider = this.canvas.GetComponentsInChildren<Slider>()[1];
            }

            if (this.dispersionSlider == null)
            {
                this.dispersionSlider = this.canvas.GetComponentsInChildren<Slider>()[2];
            }
        }

        /// <summary>
        /// The start.
        /// </summary>
        private void Start()
        {
            if (!this.prefab.GetComponent<BoidsAlgorithm>())
            {
                this.gameObject.AddComponent<BoidsAlgorithm>();
            }
        }

        /// <summary>
        /// The generate agents.
        /// </summary>
        private void GenerateAgents()
        {
            for (int i = 0; i < this.numberOfBoids; i++)
            {
                float x = Random.Range(-10, 10);
                float y = Random.Range(-10, 10);

                this.prefab.gameObject.transform.position = new Vector3(x, y, 0);
                GameObject sphereInstance = Instantiate(this.prefab) as GameObject;
                sphereInstance.gameObject.GetComponent<MeshRenderer>().material.color = this.color;
                sphereInstance.gameObject.name = "Boid " + i;

                if (!this.prefab.GetComponent<MonoBoid>())
                {
                    sphereInstance.gameObject.AddComponent<MonoBoid>();
                }
            }
        }

        /// <summary>
        /// The flock check.
        /// </summary>
        private void FlockCheck()
        {
            // Set default number of boids to 10 if its 0
            if (this.numberOfBoids <= 0)
            {
                this.numberOfBoids = 10;
            }

            // If the number inputed is an odd number..make it positive
            if (this.numberOfBoids % 2 != 0)
            {
                this.numberOfBoids++;
            }

            // Set default number of flocks to 2 if its 0
            if (this.numberOfFlocks <= 0)
            {
                this.numberOfFlocks = 2;
            }

            // If the number of flocks is greater than the number of boids
            if (this.numberOfFlocks > this.numberOfBoids)
            { // Set number of flocks to the quotient of Number of boids divided by 2
                this.numberOfFlocks = this.numberOfBoids / 2;
            }
        }

        /// <summary>
        /// The generate flock empty objects.
        /// </summary>
        private void GenerateFlockEmptyObjects()
        {
            for (int i = 0; i < this.numberOfFlocks; i ++)
            {
                int num = i;
                this.flockEmpties.Add(new GameObject("Flock " + ++num));
            }
        }
    }
}
