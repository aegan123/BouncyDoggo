using UnityEngine;
﻿using System.Collections;
using System.Collections.Generic;

// Controls player's interactions
public class Dog : MonoBehaviour
{
    //Public variables are configurable in Unity as well
    public Sprite basicSprite;
    public Sprite powerupSprite;
    public float bounceUpVelocity = 8;
    public float doubleJumpUpVelocity = 6;
    public float tiltTime = 1;
    public int powerupPizzaLimit = 5;
    public float powerupDuration = 15.0f;

    public Animator animator;
    //Sounds
    public AudioClip basicJump;
    public AudioClip doubleJump;
    public AudioClip eatPizza;
    public AudioClip eatChocolate;
    public AudioClip destroyBox;

    private Rigidbody2D playerBody;
    private Quaternion downRotation = Quaternion.Euler(0, 0, -45);
    private Quaternion upRotation = Quaternion.Euler(0, 0, 45);
    private bool isDead = false;
    private bool doubleJumpAvailable = true;
    private bool powerupOn = false;
    private int pizzaCount = 0;

    public bool canSwitch = false;
    public bool waitActive = false;
    public bool waitActive2 = false;
    
     IEnumerator Wait(){
        waitActive = true;
        yield return new WaitForSeconds (0.3f);
        canSwitch = true;
        waitActive = false;
        animator.SetBool("jumped", false);
        print("jump: false");
    }
    private void Waiter(){
        if(!waitActive){
            StartCoroutine(Wait());
            
        }
        
        
    }
    // Called once on every gaming session before Start
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Called on every game frame
    private void Update()
    {
        if (isDead == false)
        {
            playerBody.velocity = new Vector2(0, playerBody.velocity.y);
            if (powerupOn == true)
            {
                //Superball mode timer.
                powerupDuration -= Time.deltaTime;
                if (powerupDuration < 0)
                {
                    DeactivatePowerup();
                    powerupDuration = 5.0f;
                }
            }
            if (doubleJumpAvailable == true && Input.GetMouseButtonDown(0))
            {
                DoubleJump();
                Waiter();
                //animator.SetBool("Clicked", false);
            }
            //Adds downfall rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltTime * Time.deltaTime);
        }
    }

    // Double jump functionality
    private void DoubleJump()
    {
    	SoundManager.instance.PlaySingle(doubleJump);
        transform.rotation = upRotation;
        playerBody.velocity = new Vector2(0, doubleJumpUpVelocity);
        doubleJumpAvailable = false;
        animator.SetBool("jumped", true);
        print("jump = true");
    }

    // Called on touching normal colliders (colliders not set as triggers)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead == false)
        {
            if (powerupOn == false)
            {
                Bounce();
                Waiter();
            }
            else
            {
                doubleJumpAvailable = true;
            }
        }
    }

    // Bounce functionality
    private void Bounce()
    {
    	SoundManager.instance.PlaySingle(basicJump);
        playerBody.angularVelocity = 0; //Prevents player's collider from rolling
        transform.rotation = upRotation;
        playerBody.velocity = new Vector2(0, bounceUpVelocity);
        doubleJumpAvailable = true;
        animator.SetBool("jumped", true);
        print("jump = true");
    }

    // Called on touching colliders set as triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision with crates
        if (collision.gameObject.name == "crate")
        {
            Debug.Log("Crate collision");
            if (powerupOn)
            {
                SpawnObjects.instance.DestroyCrate();
            }
            else
            {
                Die();
            }
        }
        //Collision with crate piles
        else if (collision.gameObject.tag == "cratepile")
        {
            Debug.Log("Crate pile collision");
            if (powerupOn)
            {
                SpawnObjects.instance.DestroyCratePile();
            }
            else
            {
                Die();
            }
        }
        //Collision with vertical rocks
        else if (collision.gameObject.name == "rock")
        {
            Debug.Log("Rock collision");
            Die();
        }
        //Collision with pizzas
        else if (collision.gameObject.name == "pizza")
        {
            Debug.Log("Pizza collision");
            SpawnObjects.instance.DestroyPizza();
            EatPizza();
        }
    }

    // Dying functionality
    private void Die()
    {
    	//Stops music when dead
    	SoundManager.instance.superMode.Stop();
        SoundManager.instance.backgroudMusic.Stop();
        //Plays gameover music
        SoundManager.instance.gameOver.Play();
        isDead = true;
        Debug.Log("Player died");
        //Informs GameControl that game is over
        GameControl.instance.GameOver();
    }

    // Pizza functionality
    private void EatPizza()
    {
        Debug.Log(pizzaCount + " pizzas eaten");
        GameControl.instance.AddPoint();
        //Activates powerup after 5 pizzas
        if (pizzaCount < powerupPizzaLimit - 1)
        {
            pizzaCount++;
        }
        else
        {
            pizzaCount = 0;
            ActivatePowerup();
        }
    }

    // Powerup functionality
    private void ActivatePowerup()
    {
        powerupOn = true;
        Debug.Log("Powerup activated");
        //Prevents pizzas from spanwning while in superball mode.
        SpawnObjects.SetCanSpawnPizza(false);
        GetComponent<SpriteRenderer>().sprite = powerupSprite;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        SoundManager.instance.backgroudMusic.Pause();
		SoundManager.instance.superMode.Play();
    }
    private void DeactivatePowerup()
    {
        powerupOn = false;
        Debug.Log("Powerup deactivated");
        //Pizzas can spawn again.
        SpawnObjects.SetCanSpawnPizza(true);
        GetComponent<SpriteRenderer>().sprite = basicSprite;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = false;
        //Stop supermode music and resume background music
        SoundManager.instance.superMode.Stop();
        SoundManager.instance.backgroudMusic.Play();
        DoubleJump(); //Exit's rolling with a jump
    }
}
