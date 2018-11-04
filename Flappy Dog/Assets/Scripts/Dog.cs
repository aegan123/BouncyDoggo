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
    public float powerupColliderRadius = 0.75f;
    public int powerupPizzaLimit = 5;

    private Rigidbody2D playerBody;
    private Quaternion downRotation = Quaternion.Euler(0, 0, -45);
    private Quaternion upRotation = Quaternion.Euler(0, 0, 45);
    private bool isDead = false;
    private bool doubleJumpAvailable = true;
    private bool powerupOn = false;
    private int pizzaCount = 0;
    private float basicColliderRadius;
    //Superball mode timer
    private float timeLeft = 5.0f;


    // Called once on every gaming session before Start
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        basicColliderRadius = GetComponent<CircleCollider2D>().radius;
    }

    // Called on every game frame
    private void Update()
    {
        if (isDead == false)
        {
            if (powerupOn == false)
            {
                playerBody.velocity = new Vector2(0, playerBody.velocity.y);
            }
            else
            {
                playerBody.velocity = new Vector2(0, playerBody.velocity.y);
                //Superball mode timer.
                timeLeft -= Time.deltaTime;
 			    	if(timeLeft < 0 ){
         				DeactivatePowerup();
         				timeLeft=5.0f;
         				}
            }
            if (doubleJumpAvailable == true && Input.GetMouseButtonDown(0))
            {
                DoubleJump();
            }
            //Adds downfall rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltTime * Time.deltaTime);
        }
    }

    // Double jump functionality
    private void DoubleJump()
    {
        transform.rotation = upRotation;
        playerBody.velocity = new Vector2(playerBody.velocity.x, doubleJumpUpVelocity);
        doubleJumpAvailable = false;
    }

    // Called on touching normal colliders (colliders not set as triggers)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead == false)
        {
            if (powerupOn == false)
            {
                Bounce();
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
        playerBody.angularVelocity = 0; //Prevents player's collider from rolling
        transform.rotation = upRotation;
        playerBody.velocity = new Vector2(playerBody.velocity.x, bounceUpVelocity);
        doubleJumpAvailable = true;
    }

    // Called on touching colliders set as triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collision with vertical rocks
        if (collision.gameObject.name == "rock")
        {
            Debug.Log("Rock collision");
            Die();
        }
        //Collision with crates
        else if (collision.gameObject.name == "crate")
        {
            Debug.Log("Crate collision");
            if(powerupOn){
           		DestroyCrate();
            }
            else{
            Die();
            }
            //TODO: Destroy crates while on powerup
        }
        //Collision with pizzas
        else if (collision.gameObject.name == "pizza")
        {
            Debug.Log("Pizza collision");
            EatPizza(collision);
        }
    }

    // Dying functionality
    private void Die()
    {
    	//Stops music when dead
    	SoundManager.instance.superMode.Stop();
        SoundManager.instance.backgroudMusic.Stop();
        //Plays gameove music
        SoundManager.instance.gameOver.Play();
        isDead = true;
        Debug.Log("Player died");
        playerBody.velocity = new Vector2(-2, 0);
        //Informs GameControl that game is over
        GameControl.instance.GameOver();
    }

    // Crate destroying functionality
    private void DestroyCrate()
    {
    	SpawnObjects.instance.destroyCrate();
        //TODO: HOW TO TEMPORARILY DEACTIVATE?
    }

    // Pizza functionality
    private void EatPizza(Collider2D collision)
    {
        //collision.gameObject.SetActive(false); TODO: HOW TO TEMPORARILY DEACTIVATE ?
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
        SpawnObjects.setCanSpawnPizza(false);
        GetComponent<SpriteRenderer>().sprite = powerupSprite;
        GetComponent<CircleCollider2D>().radius = powerupColliderRadius;
        //Start supermode music and pause background music
        SoundManager.instance.backgroudMusic.Pause();
		SoundManager.instance.superMode.Play();
    }
    private void DeactivatePowerup()
    {
        powerupOn = false;
        Debug.Log("Powerup deactivated");
        //Pizzas can spawn again.
        SpawnObjects.setCanSpawnPizza(true);
        GetComponent<SpriteRenderer>().sprite = basicSprite;
        GetComponent<CircleCollider2D>().radius = basicColliderRadius;
        //Stop supermode music and resume background music
        SoundManager.instance.superMode.Stop();
        SoundManager.instance.backgroudMusic.Play();
        DoubleJump(); //Exit's rolling with a jump
    }
}
