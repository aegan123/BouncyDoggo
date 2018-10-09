using UnityEngine;


/*
 * This script makes the ground objects repeat. (Endless scrolling)
 * Requires a BoxCollider2D to control the objects.
 */
public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D backgroundCollider;
    private float backgroundLength;


    /*
     * Awake-method is called only once per game session before the first Start-call.
     * Use it for instantiating essential objects & loading saves etc.
     */
    private void Awake()
    {
        backgroundCollider = GetComponent<BoxCollider2D>();
        backgroundLength = backgroundCollider.size.x;
    }

    /*
     * Update-method is called on every new frame when the game is running.
     * Use it for managing player input & listening for state changes.
     */
    private void Update()
    {
        //Triggers background repositioning
        if (transform.position.x < -backgroundLength)
        {
            MoveBackground();
        }
    }

    //Moves the bypassed background object in front of the other (currently active) background object.
    private void MoveBackground()
    {
        Vector2 backgroundOffSet = new Vector2(backgroundLength * 2f, 0);
        transform.position = (Vector2)transform.position + backgroundOffSet;
    }
}