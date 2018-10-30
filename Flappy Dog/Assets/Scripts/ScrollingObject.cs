using UnityEngine;


// Scrolls objects towards player
public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D objectBody;


    // Called once on every gaming session before Start
    private void Awake()
    {
        objectBody = GetComponent<Rigidbody2D>();
    }

    // Called on every start of game
    private void Start()
    {
        objectBody.velocity = new Vector2(-GameControl.instance.scrollingSpeed, 0);
    }

    // Called on every game frame
    private void Update()
    {
        //Stops background scrolling if the game ends
        if (GameControl.instance.gameOver == true)
        {
            objectBody.velocity = Vector2.zero;
        }
    }
}