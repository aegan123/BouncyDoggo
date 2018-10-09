using UnityEngine;


/*
 * This script controls the rock obastacles.
 * Requires Rocks to be set as prefabs.
 */
public class RockPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 5;
    public float spawnRateTime = 3f;
    public float minHeight = -1f;
    public float maxHeight = 3.5f;
    public float spawnDistance = 10f;

    private GameObject[] rockPool;
    private Vector2 unusedRocksPosition = new Vector2(-20, 0); //Random position to hide the rocks at start
    private int currentRock = 0;
    private float lastSpawnTime;


    /*
     * Start-method is called every time before the game is run.
     * Use it for variable declaration.
     */
    void Start()
    {
        //Initializes the rock obstacles
        lastSpawnTime = 0f;
        rockPool = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            rockPool[i] = (GameObject) Instantiate(prefab, unusedRocksPosition, Quaternion.identity);
        }
    }

    /*
     * Update-method is called on every new frame when the game is running.
     * Use it for managing player input & listening for state changes.
     */
    void Update()
    {
        //Moves rocks from rock pool in front of the dog according to configurations
        lastSpawnTime += Time.deltaTime;
        if (GameControl.instance.gameOver == false && lastSpawnTime >= spawnRateTime)
        {
            lastSpawnTime = 0f;
            float spawnHeight = Random.Range(minHeight, maxHeight);
            rockPool[currentRock].transform.position = new Vector2(spawnDistance, spawnHeight);
            if (currentRock + 1 < poolSize)
            {
                currentRock++;
            }
            else
            {
                currentRock = 0;
            }
        }
    }
}