using UnityEngine;


/*
 * This script controls the dog's interactions.
 * Requires Rigidbody2D to control the dog.
 */
public class Dog : MonoBehaviour
{
    //Public variables are configurable in Unity as well
    public float bounceForce = 400;
    public float doubleJumpForce = 400;
    public float tiltTime = 1;

    private Rigidbody2D dog;
    private Quaternion downRotation;
    private Quaternion upRotation;
    private bool isDead = false;
    private bool doubleJumpAvailable = true;


    /*
     * Start-method is called every time before the game is run.
     * Use it for variable declaration.
     */
    void Start()
    {
        dog = GetComponent<Rigidbody2D>(); //This attaches the Dog-GameObject to this script
        downRotation = Quaternion.Euler(0, 0, -45);
        upRotation = Quaternion.Euler(0, 0, 45);
    }

    /*
     * Update-method is called on every new frame when the game is running.
     * Use it for managing player input & listening for state changes.
     */
    void Update()
    {
        if (isDead == false)
        {
            if (doubleJumpAvailable == true && Input.GetMouseButtonDown(0))
            {
                //Double jump functionality
                transform.rotation = upRotation;
                dog.velocity = Vector2.zero;
                dog.AddForce(new Vector2(0, doubleJumpForce));
                doubleJumpAvailable = false;
            }
        }
        //Downfall rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltTime * Time.deltaTime);
    }

    /*
     * This method is called when touching normal colliders (not set as triggers).
     */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead == false)
        {
            //Bounce functionality - dog bounces everytime it hits ground
            dog.angularVelocity = 0; //Prevents dog's circle collider from starting to roll
            dog.velocity = Vector2.zero;
            transform.rotation = upRotation;
            dog.AddForce(new Vector2(0, bounceForce));
            doubleJumpAvailable = true;
        }
    }

    /*
     * This method is called when touching colliders set as triggers.
     */
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Dying fuctionality - dog dies if it hits trigger colliders
        isDead = true;

        //Dying motion
        dog.angularVelocity = -5;
        dog.AddForce(new Vector2(-200, 0));

        //Informs GameControl that game is over
        GameControl.instance.GameOver();
    }
}