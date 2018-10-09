using UnityEngine;


/*
 * This script makes objects move towards the dog. (Makes it look like the dog is moving)
 * Requires kinematic Rigidbody2D to control the objects.
 */
public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    /*
     * Start-method is called every time before the game is run.
     * Use it for variable declaration.
     */
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        //The scrolling speed is set in GameControl-script
        rigidBody.velocity = new Vector2(GameControl.instance.scrollSpeed, 0);
    }

    /*
     * Update-method is called on every new frame when the game is running.
     * Use it for managing player input & listening for state changes.
     */
    void Update()
    {
        //Stops background scrolling if the game ends
        if (GameControl.instance.gameOver == true)
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}