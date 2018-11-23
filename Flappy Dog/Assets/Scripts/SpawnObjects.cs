using UnityEngine;


// Controls all prefab object spawns
public class SpawnObjects : MonoBehaviour
{
	private static System.Random rnd=new System.Random();
    public static SpawnObjects instance;
    public Transform mainCameraTransform;
    public float spawnDistance = 20;

    //Obstacles: crates, rocks, cats
    public float obstacleMinFrequency = 4;
    public float obstacleMaxFrequency = 8;
    private float nextObstacleInterval;
    private float nextObstacleCountdown;

    public GameObject cratePrefab;
    private GameObject crate;

    public GameObject cratePilePrefab;
    private GameObject cratePile;

    //TODO
    public GameObject twoCratesPrefab;
    private GameObject twoCrates;
    public GameObject theeCratesPrefab;
    private GameObject threeCrates;

    public GameObject rockPrefab;
    public float rockMaxHeight = 4;
    private GameObject rock;

    //TODO
    //public GameObject catPrefab;
    //private GameObject cat;

    //Foods: pizzas, chocolate bars
    public float foodMinFrequency = 3;
    public float foodMaxFrequency = 6;
    private float nextFoodInterval;
    private float nextFoodCountdown;

    public GameObject pizzaPrefab;
    private GameObject pizza;
    private static bool canSpawnPizza;

    //TODO
    //public GameObject chocolatePrefab;
    //private GameObject chocolate;


    // Called on every start of game
    private void Start()
    {
        //Check if there is already an instance of SpawnObjects
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SpawnObjects.
            Destroy(gameObject);

        //Initialize prefabs
        nextObstacleCountdown = 0;
        nextObstacleInterval = Random.Range(obstacleMinFrequency, obstacleMaxFrequency);
        nextFoodCountdown = 0;
        nextFoodInterval = Random.Range(foodMinFrequency, foodMaxFrequency);
        canSpawnPizza = true;
    }

    // Called on every game frame
    private void Update()
    {
        nextObstacleCountdown += Time.deltaTime;
        nextFoodCountdown += Time.deltaTime;
        if (GameControl.instance.gameOver == false)
        {
            //Obstacle spawns
            if (nextObstacleCountdown >= nextObstacleInterval)
            {
                //Determines which obstacle to spawn by random
                float random = Random.Range(0, 10);
                if (random <= 5) //50% single crates
                {
                    SpawnCrate();
                }
                else if (random <= 7) //20% crate piles
                {
                    SpawnCratePile();
                }
                else if (random <= 9) //20% rocks
                {
                    SpawnRock();
                }
                else if (random <= 9) //10% cats
                {
                    //TODO: SpawnCat();
                }
                nextObstacleCountdown = 0;
                nextObstacleInterval = Random.Range(obstacleMinFrequency, obstacleMaxFrequency);
            }
            //Food spawns
            if (nextFoodCountdown >= nextFoodInterval)
            {
                //Determines which food to spawn by random
                float random = Random.Range(0, 3);
                if (canSpawnPizza && random <= 2) //66% pizzas
                {
                    SpawnPizza();
                }
                else
                {
                    //TODO: SpawnChocolate();
                }
                nextFoodCountdown = 0;
                nextFoodInterval = Random.Range(foodMinFrequency, foodMaxFrequency);
            }
        }
    }

    // Single crate functionality
    private void SpawnCrate()
    {
        crate = (GameObject) Instantiate(cratePrefab, new Vector2(spawnDistance, 0), Quaternion.identity);
        crate.gameObject.SetActive(true);
    }
    public void DestroyCrate()
    {
        Destroy(crate);
        //TODO: destroying animation
    }

    // Crate pile functionality
    private void SpawnCratePile()
    {
        cratePile = (GameObject) Instantiate(cratePilePrefab, new Vector2(spawnDistance, 0), Quaternion.identity);
        cratePile.gameObject.SetActive(true);
    }
    public void DestroyCratePile()
    {
        Destroy(cratePile);
        //TODO: destroying animation
    }

    // Rock spawning functionality
    private void SpawnRock()
    {
        float rockHeight = Random.Range(0, rockMaxHeight);
        rock = (GameObject) Instantiate(rockPrefab, new Vector2(spawnDistance, rockHeight), Quaternion.identity);
    }

    // Pizza functionality
    private void SpawnPizza()
    {
    	pizza = (GameObject) Instantiate(pizzaPrefab, new Vector2(spawnDistance, rnd.Next(6)), Quaternion.identity);
        pizza.gameObject.SetActive(true);
    }
    public void DestroyPizza()
    {
        Destroy(pizza);
        //TODO: Eating sound
    }
    public static void SetCanSpawnPizza(bool boolean)
    {
        canSpawnPizza = boolean;
    }
}
