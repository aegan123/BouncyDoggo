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
    public float powerupColliderRadius = 1.5f;
    public float baseColliderRadius = 1;
    public int powerupFoodLimit = 5;
    public int doubleJumpLimit = 2; //How many jumps available in air
    public int powerupJumpLimit = 1; //How many times can jump in rolling ball mode
    public int maxDistance = 5; // max distance to obstacle to continue superball mode to avoid instant death

    // Other variables
    private Rigidbody2D playerBody;
    private Quaternion downRotation = Quaternion.Euler (0, 0, -45);
    private Quaternion upRotation = Quaternion.Euler (0, 0, 45);
    private bool isDead = false;
    private bool diveAvailable = false;
    private bool powerupOn = false;
    private float powerupTimer;
    private int doubleJumpCount = 0;
    private int powerupJumpCount = 0;
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
        if (!isDead) {
            playerBody.velocity = new Vector2 (0, playerBody.velocity.y);
            if (powerupOn) {
                //Double jump always available once per bounce
                if (powerupJumpCount < powerupJumpLimit)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("up"))
                    {
                        DoubleJump();
                        //animator.SetBool("Clicked", false);
                    }
                }
                //Powerup timer
                powerupTimer -= Time.deltaTime;
                // If obstacle is too close, extend timer to avoid instant death.
                if (isObstacleTooClose ()) {
                    powerupTimer += Time.deltaTime;
                }
                if (powerupTimer < 0) {
                    DeactivatePowerup ();
                }
            }
            else
            {
                //Double jump always available once per bounce
                if (doubleJumpCount < doubleJumpLimit)
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
                powerupJumpCount = 0;
            }
            else
            {
                Bounce();
                doubleJumpCount = 0;
            }
            diveAvailable = true;
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
                EatFood (1); //Pizzas food value = 1
                Destroy(collision.gameObject);
            }
            //Collision with chocolates
            else if (collision.gameObject.name.Contains("chocolate"))
            {
                EatBadfood(1); //Chocolate poison value = 1
                Destroy(collision.gameObject);
            }
            //Collision with boxes
            else if (collision.gameObject.name.Contains("box"))
            {
                Destroy(collision.gameObject);
                SoundManager.instance.PlaySingle(destroyBox);
            }
        }
    }

    // Double jump functionality
    private void DoubleJump()
    {
        playerBody.velocity = new Vector2(0, doubleJumpUpVelocity);
        if (powerupOn)
        {
            powerupJumpCount++;
        }
        else
        {
            Waiter();
            animator.SetBool("jumped", true);
            doubleJumpCount++;
        }
        transform.rotation = upRotation;
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
        GameControl.instance.eatFood(foodCount);
        if (foodCount >= powerupFoodLimit)
        {
            ActivatePowerup();
            foodCount = 0;
        }
    }

    // Poison functionality
    private void EatBadfood(int poisonValue)
    {
        SoundManager.instance.PlaySingle(destroyBox);
        foodCount -= poisonValue;
        GameControl.instance.eatFood (foodCount);
        if (foodCount < 0)
        {
            if (powerupOn)
            {
                foodCount = 0;
                DeactivatePowerup();
            }
            else
            {
                Die();
            }
        }
    }

    // Powerup functionality
    private void ActivatePowerup () {
        powerupTimer = powerupDuration;
        powerupOn = true;
        animator.SetBool("powerupOn", true);
        //Debug.Log("Powerup activated");
        //Prevents pizzas from spanwning while in superball mode.
        SpawnObjects.SetCanSpawnFood (false);
        GetComponent<SpriteRenderer> ().sprite = powerupSprite;
        GetComponent<CircleCollider2D> ().radius = powerupColliderRadius;
        SoundManager.instance.backgroudMusic.Pause ();
        SoundManager.instance.superMode.Play ();
    }
    private void DeactivatePowerup () {
        animator.SetBool("powerupOn", false);
        //Debug.Log("Powerup deactivated");
        //Pizzas can spawn again.
        SpawnObjects.SetCanSpawnFood (true);
        GetComponent<SpriteRenderer> ().sprite = basicSprite;
        GetComponent<CircleCollider2D>().radius = baseColliderRadius;
        //Stop supermode music and resume background music
        SoundManager.instance.superMode.Stop ();
        SoundManager.instance.backgroudMusic.Play ();
        doubleJumpCount = 0;
        Bounce (); //Exit's rolling with a bounce
        powerupOn = false; //TODO: This only after certain safe time?
        GameControl.instance.eatFood(0);
    }

    // Called by other scripts.
    public bool isPowerUpOn () {
        return powerupOn;
    }

    // Calculates the distance to the next obstacle.
    private bool isObstacleTooClose () {
        var distance = Vector2.Distance (GameObject.FindWithTag ("Player").transform.position, SpawnObjects.instance.getCurrentObstacle ().transform.position);
        //Debug.Log ("Distance to obstacle: " + distance);
        return distance < maxDistance;
    }
}
