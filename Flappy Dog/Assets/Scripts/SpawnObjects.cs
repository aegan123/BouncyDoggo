using UnityEngine;


// Controls all prefab object spawns
public class SpawnObjects : MonoBehaviour
{
    //public static SpawnObjects instance;
    public Transform mainCameraTransform;
    public float spawnDistance = 20;
    private Vector2 unusedObjectsPosition = new Vector2(-20, 0); //Some position to hide objects at start

    //Rocks
    public GameObject rocksPrefab;
    public int rockPoolSize = 5;
    public float rockMaxHeight = 4;
    public float rockMinFrequency = 3;
    public float rockMaxFrequency = 6;
    private GameObject[] rockPool;
    private int currentRock = 0;
    private float nextRockInterval;
    private float rextRockCountdown;

    //Crates
    public GameObject cratesPrefab;
    public int cratePoolSize = 10;
    public float crateMinFrequency = 3;
    public float crateMaxFrequency = 6;
    private GameObject[] cratePool;
    private int currentCrate = 0;
    private float nextCrateInterval;
    private float nextCrateCountdown;

    //Pizzas
    public GameObject pizzasPrefab;
    public int pizzaPoolSize = 10;
    public float pizzaMinFrequency = 3;
    public float pizzaMaxFrequency = 6;
    private GameObject[] pizzaPool;
    private int currentPizza = 0;
    private float nextPizzaInterval;
    private float nextPizzaCountdown;

    //Other
    private static bool canSpawnPizza=true;


    // Called on every start of game
    private void Start()
    {
        canSpawnPizza=true;
        //Initializes rocks
        rockPool = new GameObject[rockPoolSize];
        rextRockCountdown = 0;
        nextRockInterval = Random.Range(rockMinFrequency, rockMaxFrequency);
        for (int i = 0; i < rockPoolSize; i++)
        {
            rockPool[i] = (GameObject) Instantiate(rocksPrefab, unusedObjectsPosition, Quaternion.identity);
        }
        //Initializes crates
        cratePool = new GameObject[cratePoolSize];
        nextCrateCountdown = 0;
        nextCrateInterval = Random.Range(crateMinFrequency, crateMaxFrequency);
        for (int i = 0; i < cratePoolSize; i++)
        {
            cratePool[i] = (GameObject)Instantiate(cratesPrefab, unusedObjectsPosition, Quaternion.identity);
        }
        //Initializes pizzas
        pizzaPool = new GameObject[pizzaPoolSize];
        nextPizzaCountdown = 0;
        nextPizzaInterval = Random.Range(pizzaMinFrequency, pizzaMaxFrequency);
        for (int i = 0; i < pizzaPoolSize; i++)
        {
            pizzaPool[i] = (GameObject) Instantiate(pizzasPrefab, unusedObjectsPosition, Quaternion.identity);
        }
    }

    // Called on every game frame
    private void Update()
    {
        rextRockCountdown += Time.deltaTime;
        nextCrateCountdown += Time.deltaTime;
        nextPizzaCountdown += Time.deltaTime;
        if (GameControl.instance.gameOver == false)
        {
            if (rextRockCountdown >= nextRockInterval)
            {
                SpawnRock();
            }
            if (nextCrateCountdown >= nextCrateInterval)
            {
                SpawnCrate();
            }
            if (canSpawnPizza && nextPizzaCountdown >= nextPizzaInterval)
            {
                SpawnPizza();
            }
        }
    }

    // Rock spawning functionality
    private void SpawnRock()
    {
        rextRockCountdown = 0;
        float spawnHeight = Random.Range(0, rockMaxHeight);
        rockPool[currentRock].transform.position = new Vector2(spawnDistance, spawnHeight);
        nextRockInterval = Random.Range(rockMinFrequency, rockMaxFrequency);
        if (currentRock + 1 < rockPoolSize)
        {
            currentRock++;
        }
        else
        {
            currentRock = 0;
        }
    }

    // Crate spawning functionality
    private void SpawnCrate()
    {
        nextCrateCountdown = 0;
        //TODO: HOW TO TEMPORARILY DEACTIVATE?
        cratePool[currentCrate].gameObject.SetActive(true);
        cratePool[currentCrate].transform.position = new Vector2(spawnDistance, 0);
        nextCrateInterval = Random.Range(crateMinFrequency, crateMaxFrequency);
        if (currentCrate + 1 < cratePoolSize)
        {
            currentCrate++;
        }
        else
        {
            currentCrate = 0;
        }
    }

    // Pizza spawning functionality
    private void SpawnPizza()
    {
        nextPizzaCountdown = 0;
        //Debug.Log("REACTIVATING: " +  pizzaPool[currentPizza].gameObject.name); TODO: HOW TO TEMPORARILY DEACTIVATE?
        pizzaPool[currentPizza].gameObject.SetActive(true);
        pizzaPool[currentPizza].transform.position = new Vector2(spawnDistance, 0);
        nextPizzaInterval = Random.Range(pizzaMinFrequency, pizzaMaxFrequency);
        if (currentPizza + 1 < pizzaPoolSize)
        {
            currentPizza++;
        }
        else
        {
            currentPizza = 0;
        }
    }
    //Set whether or not pizza spawning is possible.
    public static void setCanSpawnPizza(bool boolean){
    	canSpawnPizza=boolean;
    }
}
