using UnityEngine;


// Scrolls objects towards player & handles falling objects
public class ScrollingObject : MonoBehaviour
{
    public float scrollingSpeed = 7;
    private Rigidbody2D objectBody;


    // Called once on every gaming session before Start
    private void Awake()
    {
        objectBody = GetComponent<Rigidbody2D>();
    }

    // Called on every start of game
    private void Start()
    {
        objectBody.velocity = new Vector2(-scrollingSpeed, objectBody.velocity.y);
    }

    // Called on every game frame
    private void Update()
    {
        //Stops background scrolling if the game ends
        if (GameControl.instance.gameOver == true)
        {
            objectBody.velocity = new Vector2(0, objectBody.velocity.y);
        }
    }
}