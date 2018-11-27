using System.Collections;
using UnityEngine;﻿

// Controls player's interactions
public class Dog : MonoBehaviour {

    public static Dog instance;

    // Public variables (configurable in Unity)
    public Sprite basicSprite;
    public Sprite powerupSprite;
    public Animator animator;
    public float bounceUpVelocity = 8;
    public float doubleJumpUpVelocity = 6;
    public float tiltTime = 1;
    public float powerupDuration = 15.0f;
    public int powerupFoodLimit = 5;

    // Other variables
    private Rigidbody2D playerBody;
    private Quaternion downRotation = Quaternion.Euler (0, 0, -45);
    private Quaternion upRotation = Quaternion.Euler (0, 0, 45);
    private bool isDead = false;
    private bool doubleJumpAvailable = true;
    private bool diveAvailable = false;
    private bool powerupOn = false;
    private float powerupTimer;
    private int foodCount = 0;

    // Animation variables
    public bool canSwitch = false;
    public bool waitActive = false;
    public bool waitActive2 = false;

    // Sounds
    public AudioClip basicJump;
    public AudioClip doubleJump;
    public AudioClip eatPizza;
    public AudioClip eatChocolate;
    public AudioClip destroyBox;

    //for testing purposes only!!!
    //When true the Dog doesn't collide with object and points are not counted.
    public static bool godMode = false;


    // Animation waiter
    IEnumerator Wait () {
        waitActive = true;
        yield return new WaitForSeconds (0.3f);
        canSwitch = true;
        waitActive = false;
        animator.SetBool ("jumped", false);
    }
    private void Waiter () {
        if (!waitActive) {
            StartCoroutine (Wait ());
        }
    }

    // Called once on every gaming session before Start
    private void Awake () {
        if (instance != this)
        {
            Destroy(instance);
        }
        instance = this;

        playerBody = GetComponent<Rigidbody2D> ();
    }

    // Called on every game frame
    private void Update () {
        if (isDead == false) {
            playerBody.velocity = new Vector2 (0, playerBody.velocity.y);
            if (powerupOn) {
                //Double jump always available once per bounce
                if (doubleJumpAvailable)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("up"))
                    {
                        DoubleJump();
                        //animator.SetBool("Clicked", false);
                    }
                }
                //Powerup timer
                powerupTimer -= Time.deltaTime;
                if (powerupTimer < 0) {
                    DeactivatePowerup ();
                }
            }
            else
            {
                //Double jump always available once per bounce
                if (doubleJumpAvailable)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("up"))
                    {
                        DoubleJump();
                        //animator.SetBool("Clicked", false);
                    }
                }
                //Diving is available  when not on poweup & once per bounce only when falling downwards
                if (diveAvailable && playerBody.velocity.y < 0)
                {
                    if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("down"))
                    {
                        Dive();
                        //animator.SetBool("Clicked", false);
                    }
                }
                //Adds downfall rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltTime * Time.deltaTime);
            }
        }
    }

    // Called on touching horizontal colliders (colliders not set as triggers)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDead)
        {
            if (powerupOn)
            {
                doubleJumpAvailable = true; //Enables jumping when powerup is on even without bouncing
            }
            else
            {
                Bounce();
            }
        }
    }

    // Called on touching colliders set as triggers
    private void OnTriggerEnter2D (Collider2D collision) {
        if (godMode) {
            Destroy (collision.gameObject);
            return;
        }
        if (!isDead) {
            //Collision with crates
            if (collision.gameObject.name.Contains("crate")) {
                if (powerupOn) {
                    Destroy(collision.gameObject);
                    SoundManager.instance.PlaySingle(destroyBox);
                } else {
                    Die ();
                }
            }
            //Collision with rocks
            else if (collision.gameObject.name.Contains("rock")) {
                Die ();
            }
            //Collision with pizzas
            else if (collision.gameObject.name.Contains("pizza")) {
                EatFood (1);
                Destroy(collision.gameObject);
            }
        }
    }

    // Double jump functionality
    private void DoubleJump()
    {
        playerBody.velocity = new Vector2(0, doubleJumpUpVelocity);
        doubleJumpAvailable = false;
        if (!powerupOn)
        {
            Waiter();
            animator.SetBool("jumped", true);
            transform.rotation = upRotation;
        }
        SoundManager.instance.PlaySingle(doubleJump);
    }

    // Diving functionality
    private void Dive()
    {
        playerBody.velocity = new Vector2(0, -doubleJumpUpVelocity);
        diveAvailable = false;
        Waiter();
        animator.SetBool("jumped", true);
        transform.rotation = downRotation;
        SoundManager.instance.PlaySingle(doubleJump);
    }

    // Bounce functionality
    private void Bounce()
    {
        playerBody.angularVelocity = 0; //Prevents player's collider from rolling
        playerBody.velocity = new Vector2(0, bounceUpVelocity);
        doubleJumpAvailable = true;
        diveAvailable = true;
        Waiter();
        animator.SetBool("jumped", true);
        transform.rotation = upRotation;
        SoundManager.instance.PlaySingle(basicJump);
    }

    // Dying functionality
    private void Die () {
        isDead = true;
        //Stops music when dead
        SoundManager.instance.superMode.Stop ();
        SoundManager.instance.backgroudMusic.Stop ();
        //Plays gameover music
        SoundManager.instance.gameOver.Play ();
        //Informs GameControl that game is over
        GameControl.instance.GameOver ();
    }

    // Food functionality
    private void EatFood (int foodValue) {
        SoundManager.instance.PlaySingle (eatPizza);
        GameControl.instance.AddPoint (10);
        foodCount += foodValue;
        if (foodCount >= powerupFoodLimit)
        {
            ActivatePowerup();
            foodCount = 0;
        }
    }

    // Powerup functionality
    private void ActivatePowerup () {
        powerupTimer = powerupDuration;
        powerupOn = true;
        //Prevents pizzas from spanwning while in superball mode.
        SpawnObjects.SetCanSpawnFood (false);
        GetComponent<SpriteRenderer> ().sprite = powerupSprite;
        GetComponent<CircleCollider2D> ().radius = 1.45f;
        SoundManager.instance.backgroudMusic.Pause ();
        SoundManager.instance.superMode.Play ();
    }
    private void DeactivatePowerup () {
        powerupOn = false;
        //Pizzas can spawn again.
        SpawnObjects.SetCanSpawnFood (true);
        GetComponent<SpriteRenderer> ().sprite = basicSprite;
        GetComponent<CircleCollider2D>().radius = 1;
        //Stop supermode music and resume background music
        SoundManager.instance.superMode.Stop ();
        SoundManager.instance.backgroudMusic.Play ();
        Bounce (); //Exit's rolling with a bounce
    }
}