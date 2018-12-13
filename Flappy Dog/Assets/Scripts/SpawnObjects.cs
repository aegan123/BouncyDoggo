using UnityEngine;

// Controls all prefab object spawns
public class SpawnObjects : MonoBehaviour {

    public static SpawnObjects instance;
    public Transform mainCameraTransform;
    private static System.Random rnd = new System.Random();

    // Public variables (configurable in Unity)
    public GameObject cratePrefab;
    public GameObject cratePilePrefab;
    public GameObject twoCratesPrefab;
    public GameObject threeCratesPrefab;
    public GameObject powerUpBoxPrefab;
    public GameObject rockPrefab;
    public GameObject catPrefab;
    public GameObject pizzaPrefab;
    public GameObject chocolatePrefab;
    public float spawnDistance = 20;
    public float obstacleMinFrequency = 2;
    public float obstacleMaxFrequency = 7;
    public float rockMaxHeight = 4;
    public float foodMinFrequency = 3;
    public float foodMaxFrequency = 7;
    public int obstaclePoolSize = 5;
    public int doubleObstaclePoolSize = 5;
    public int foodPoolSize = 10;

    // Prefab variables
    private GameObject[] obstacles;
    private GameObject[] doubleObstacles;
    private GameObject[] foods;
    private int currentObstacle = 0;
    private int currentDoubleObstacle = 0;
    private int currentFood = 0;
    private float nextObstacleInterval;
    private float nextObstacleCountdown;
    private float nextFoodInterval;
    private float nextFoodCountdown;

    // Other variables
    private static bool foodAllowedToSpawn;
    private float difficultyTimer = 0;
    private bool doubleObstacle = false;

    // Called on every start of game
    private void Start () {
        //Check if there is already an instance of SpawnObjects
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SpawnObjects.
            Destroy (gameObject);

        //Initialize prefabs
        nextObstacleCountdown = 0;
        nextObstacleInterval = (float) rnd.Next ((int) obstacleMinFrequency, (int) obstacleMaxFrequency);
        nextFoodCountdown = 0;
        nextFoodInterval = (float)rnd.Next((int)foodMinFrequency, (int)foodMaxFrequency);
        foodAllowedToSpawn = true;
        obstacles = new GameObject[obstaclePoolSize];
        doubleObstacles = new GameObject[doubleObstaclePoolSize];
        foods = new GameObject[foodPoolSize];
    }
private void FixedUpdate() {
    nextObstacleCountdown += Time.deltaTime;
        nextFoodCountdown += Time.deltaTime;
        difficultyTimer += Time.deltaTime;
        if (!GameControl.instance.gameOver) {
            // TODO: IDEA: set timer for medium lower if high score is high enough
            if (difficultyTimer < 30.0f) {
                spawnEasy ();
            } else if (difficultyTimer < 60.0f) {
                spawnMedium ();
            } else {
                spawnHard ();
            }
        }
}
    // Called on every game frame
    //private void Update () {
        
    //}

    private void spawnEasy () {
        //Spawn obstacles
        if (CanSpwanObstacle ()) {
            int random = rnd.Next (12);
            if (random <= 6) //80% single obstacles
           {
                SpawnCrate (false, false);
            } else if (random <= 8) {
                SpawnRock (false, false, 0);
            } else if (!Dog.instance.isPowerUpOn() && random > 10) //Special powerup box. Not while in superball mode.
           {
                SpawnCrate (true, false);
            } else { // box with rock right after
                nextObstacleCountdown = -5;
                doubleObstacle = true;
                SpawnDoubleObstacle (1);
            }
            if (doubleObstacle) {
                doubleObstacles[currentDoubleObstacle - 2].gameObject.SetActive (true);
                doubleObstacles[currentDoubleObstacle - 1].gameObject.SetActive (true);
                doubleObstacle = false;
            } else {
                obstacles[currentObstacle].gameObject.SetActive (true);
            }
            currentObstacle++;
            // Reset back to the beginning of the array.
            if (currentObstacle >= obstacles.Length - 1) {
                currentObstacle = 0;
            }
            if (currentDoubleObstacle >= doubleObstacles.Length - 1) {
                currentDoubleObstacle = 0;
            }

        }
        //Food spawns
        if(foodAllowedToSpawn && CanSpawnFood()){
            spawnFood();
        }
    }

    private void spawnMedium () {
        obstacleMaxFrequency = 4;
        //Spawn obstacles
        if (CanSpwanObstacle ()) {
            int random = rnd.Next (0, 12);
            if (random <= 5) //50% single obstacles
           {
                if (rnd.Next (3) == 1) { //spawn a cat on top of crate
                    doubleObstacle = true;
                    SpawnDoubleObstacle (2);

                } else {
                    SpawnCrate (false, false);
                }

            } else if (random <= 7) { // 20% double boxes
                SpawnDoubleCrate ();

            } else if (random <= 9) { // 20% cratepiles
                if (rnd.Next (2) == 1) { // a pile followed immediately by a rock
                    //nextObstacleCountdown = -5;
                    doubleObstacle = true;
                    //SpawnDoubleObstacle (3);
                    SpawnDoubleObstacle(1);
                } else {
                    //SpawnCratePile (false);
                    SpawnDoubleCrate();
                }
            } else if(!Dog.instance.isPowerUpOn() && random > 10) //Special powerup box. Not while in superball mode.
           {
                SpawnCrate (true, false);
            } else {
                SpawnRock (false, false, 0);
            }
            if (doubleObstacle) {
                doubleObstacles[currentDoubleObstacle - 2].gameObject.SetActive (true);
                doubleObstacles[currentDoubleObstacle - 1].gameObject.SetActive (true);
                doubleObstacle = false;
            } else {
                obstacles[currentObstacle].gameObject.SetActive (true);
            }
            currentObstacle++;
            // Reset back to the beginning of the array.
            if (currentObstacle >= obstacles.Length - 1) {
                currentObstacle = 0;
            }
            if (currentDoubleObstacle >= doubleObstacles.Length - 1) {
                currentDoubleObstacle = 0;
            }
        }
        //Food spawns
        if(foodAllowedToSpawn && CanSpawnFood()){
            spawnFood();
        }

    }

    private void spawnHard () {
        obstacleMaxFrequency = 3;
        //spawn obstacle
        if (CanSpwanObstacle ()) {
            int random = rnd.Next (12);
            if (random <= 2) {
                if (rnd.Next (3) == 1) { //spawn a cat on top of crate
                    doubleObstacle = true;
                    SpawnDoubleObstacle (2);

                } else {
                    SpawnCrate (false, false);
                }
            } else if (random <= 5) { // 20% double boxes
                SpawnDoubleCrate ();

            } else if (random <= 8) { // 20% cratepiles
                if (rnd.Next (2) == 1) { // a pile followed immediately by a rock
                    //nextObstacleCountdown = -5;
                    doubleObstacle = true;
                    //SpawnDoubleObstacle (3);
                    SpawnDoubleObstacle(1);
                } else {
                    //SpawnCratePile (false);
                    SpawnDoubleCrate();
                }
            } else if (random <= 10) { // one box followed by three
                nextObstacleCountdown = -5;
                doubleObstacle = true;
                SpawnDoubleObstacle (4);
            } else if(!Dog.instance.isPowerUpOn() && random > 10) //Special powerup box. Not while in superball mode.
           {
                SpawnCrate (true, false);
            } else {
                SpawnRock (false, false, 0);
            }
            if (doubleObstacle) {
                doubleObstacles[currentDoubleObstacle - 2].gameObject.SetActive (true);
                doubleObstacles[currentDoubleObstacle - 1].gameObject.SetActive (true);
                doubleObstacle = false;
            } else {
                obstacles[currentObstacle].gameObject.SetActive (true);
            }
            currentObstacle++;
            // Reset back to the beginning of the array.
            if (currentObstacle >= obstacles.Length - 1) {
                currentObstacle = 0;
            }
            if (currentDoubleObstacle >= doubleObstacles.Length - 1) {
                currentDoubleObstacle = 0;
            }
        }
        //Food spawns
        if(foodAllowedToSpawn && CanSpawnFood()){
            spawnFood();
        }
    }

    //***************************************
    //Item spawning and destroying functions*
    //***************************************

    //*********
    //Spawning*
    //*********

    // Single crate (normal and special powerupbox)
    // params: special: spawns powerup box
    //params: tupla: spawn a box as part of a double obstacle
    private GameObject SpawnCrate (bool special, bool tupla) {
        //Destroy(obstacles[currentObstacle]);
        if (special) {
            obstacles[currentObstacle] = (GameObject) Instantiate (powerUpBoxPrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
        } else {
            if (!tupla) {
                obstacles[currentObstacle] = (GameObject) Instantiate (cratePrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
            } else {
                doubleObstacles[currentDoubleObstacle] = (GameObject) Instantiate (cratePrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
                return doubleObstacles[currentDoubleObstacle]; //Return the spawned box (cat will be able to get a ref to this)
            }
        }
        return null;
    }

    // Crate pile
    // Param: tupla: spawn the pile as part of a double obstacle
    private void SpawnCratePile (bool tupla) {
        //Destroy(obstacles[currentObstacle]);
        if (!tupla) {
            obstacles[currentObstacle] = (GameObject) Instantiate (cratePilePrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
        } else {
            doubleObstacles[currentDoubleObstacle] = (GameObject) Instantiate (cratePilePrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
        }
    }

    // Two obstacles on top of each other
    private void SpawnDoubleCrate () {
        //Destroy(obstacles[currentObstacle]);
        obstacles[currentObstacle] = (GameObject) Instantiate (twoCratesPrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
    }

    // Three obstacles on top of each other
    private void SpawnTripleCrate () {
        //Destroy(obstacles[currentObstacle]);
        doubleObstacles[currentDoubleObstacle] = (GameObject) Instantiate (threeCratesPrefab, new Vector2 (spawnDistance + spawnDistance / 2, 0), Quaternion.identity);
    }

    // Rock spawning
    // Params: tupla: rock is part of a double obstacle
    // Params: pile rock is part of a double obstacle with crate pile
    // Params: height: height placement of rock in a double obstacle
    private void SpawnRock (bool tupla, bool pile, int height) {
        //Destroy(obstacles[currentObstacle]); //(TEST) Ensures the deletion of the earlier obstacle
        if (tupla) {
            if (!pile) {
                doubleObstacles[currentDoubleObstacle] = (GameObject) Instantiate (rockPrefab, new Vector2 (spawnDistance + spawnDistance / 2, rockMaxHeight / height), Quaternion.identity);
            } else {
                doubleObstacles[currentDoubleObstacle] = (GameObject) Instantiate (rockPrefab, new Vector2 (spawnDistance * 2.5f, rockMaxHeight / height), Quaternion.identity);
            }
        } else {
            obstacles[currentObstacle] = (GameObject) Instantiate (rockPrefab, new Vector2 (spawnDistance, rnd.Next (0, (int) rockMaxHeight)), Quaternion.identity);
        }
    }

    // Cat spawning
    // Param: tupla: cat is part of a double obstacle
    // param: height: height where to spawn. 0==ground, else is on top of a crate.
    private void spawnCat (bool tupla, float height, GameObject boxUnder) {
        //Destroy(obstacles[currentObstacle]); //(TEST) Ensures the deletion of the earlier obstacle
        if (tupla && boxUnder != null) {
            doubleObstacles[currentDoubleObstacle] = (GameObject) Instantiate (catPrefab, new Vector2 (spawnDistance, height + 0.95f), Quaternion.identity);
            doubleObstacles[currentDoubleObstacle].GetComponent<FallingObject>().SetObjectUnder(boxUnder);
        } else{
            obstacles[currentObstacle] = (GameObject) Instantiate (catPrefab, new Vector2 (spawnDistance, 0), Quaternion.identity);
        }
    }

    // Spawns the selected double obstacle type.
    // params: 1 == single box + rock, 2 == single box with cat on top
    // params: 3 == create pile followed by a rock, 4 == one box followed by three boxes
    private void SpawnDoubleObstacle(int choice)
    {
        switch (choice)
        {
            case (1):
                SpawnCrate(false, true);
                currentDoubleObstacle++;
                SpawnRock(true, false, 3);
                break;
            case (2):
                GameObject catBoxPrefab = SpawnCrate(false, true);
                GameObject catBox = catBoxPrefab.transform.GetChild(0).gameObject;
                Debug.Log("OBJECT UNDER CAT: " + catBox.name);
                currentDoubleObstacle++;
                spawnCat(true, doubleObstacles[currentDoubleObstacle - 1].transform.TransformPoint(1, 1, 0).y - doubleObstacles[currentDoubleObstacle - 1].transform.TransformPoint(0, 0, 0).y, catBox);
                break;
            case (3):
                SpawnCratePile(true);
                currentDoubleObstacle++;
                SpawnRock(true, true, 2);
                break;
            case (4):
                SpawnCrate(false, true);
                currentDoubleObstacle++;
                SpawnTripleCrate();
                break;
        }
        currentDoubleObstacle++;
    }

    // Spawns all types of food.
    private void spawnFood(){
            //Determines which food to spawn by random
            int random = rnd.Next(4);
            if(random <= 2) //66% pizzas
           {
                SpawnPizza();
            }
            else
            {
                SpawnChocolate();
            }
            currentFood++;
            // Reset back to the beginning of the array.
            if (currentFood >= foods.Length - 1)
            {
                currentFood = 0;
            }
    }
    // Pizza spawning
    private void SpawnPizza () {
        //Destroy(foods[currentFood]);
        foods[currentFood] = (GameObject) Instantiate (pizzaPrefab, FoodSpawnPoint(spawnDistance, rnd.Next(6)), Quaternion.identity);
    }

    // Chocolate spawning
    private void SpawnChocolate () {
        //Destroy(foods[currentFood]);
        foods[currentFood] = (GameObject) Instantiate (chocolatePrefab, FoodSpawnPoint(spawnDistance, rnd.Next(6)), Quaternion.identity);
    }

    //****************************
    //Additional helper functions*
    //****************************

    // Sets if food objects are allowed to spawn.
    // No food objects during super mode.
    public static void SetCanSpawnFood (bool boolean) {
        foodAllowedToSpawn = boolean;
    }

    //Checks if obstacle is allowed to spawn.
    //If it is allowed, resets counters.
    private bool CanSpwanObstacle () {
        if (nextObstacleCountdown >= nextObstacleInterval) {
            nextObstacleCountdown = 0;
            nextObstacleInterval = (float) rnd.Next ((int) obstacleMinFrequency, (int) obstacleMaxFrequency);
            return true;
        }
        return false;
    }

    //Checks if food is allowed to spawn.
    //If it is allowed, resets counters.
    private bool CanSpawnFood () {
        if (nextFoodCountdown >= nextFoodInterval) {
            nextFoodCountdown = 0;
            nextFoodInterval = (float) rnd.Next ((int) foodMinFrequency, (int) foodMaxFrequency);
            return true;
        }
        return false;
    }

    // Called by other scripts.
    // Returns current obstacle on screen.
    public GameObject getCurrentObstacle(){
        GameObject object1=null;
        if(doubleObstacle){
            //Debug.Log("double obstacle");
            //Debug.Log("currentDoubleObstacle = "+currentDoubleObstacle);
            if(currentDoubleObstacle==0){
                if(doubleObstacles[doubleObstacles.Length-1]!=null){
                    object1=doubleObstacles[doubleObstacles.Length-1];
                }else
                {
                    object1=doubleObstacles[currentDoubleObstacle];
                }
            }else{
                if(doubleObstacles[currentDoubleObstacle]==null){
                    object1=doubleObstacles[currentDoubleObstacle-1];
                }else{
                    object1=doubleObstacles[currentDoubleObstacle];
                }
            
            }
        }else{
            //Debug.Log("single obstacle");
            //Debug.Log("currentObstacle = "+currentObstacle);
            if(currentObstacle==0){
                if(obstacles[obstacles.Length-1]!=null){
                    object1=obstacles[obstacles.Length-1];
                }else{
                    object1=obstacles[currentObstacle];
                }
            }else{
                if(obstacles[currentObstacle]==null){
                    object1=obstacles[currentObstacle-1];
                }else{
                    object1=obstacles[currentObstacle];
                }
            }
        }
        return object1;
    }

    // Ensures a spawnpoint with no obstacles on
    private Vector2 FoodSpawnPoint(float x, float y)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(x, y), 3);
        foreach (Collider2D col in hitColliders)
        {
            //Debug.Log("collider hit, moving onward");
            return FoodSpawnPoint(x + 2, y);
        }
        return new Vector2(x, y);
    }
}
