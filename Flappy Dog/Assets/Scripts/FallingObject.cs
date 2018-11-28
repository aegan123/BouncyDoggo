using UnityEngine;


// Scrolls objects towards player & handles falling objects
public class FallingObject : MonoBehaviour
{
    public float fallingSpeed = 4;
    public GameObject objectUnder;

    private Rigidbody2D objectBody;
    private bool onGround;


    // Called once on every gaming session before Start
    private void Awake()
    {
        objectBody = GetComponent<Rigidbody2D>();
    }

    // Called on every game frame
    private void Update()
    {
        if (objectUnder == null)
        {
            if (!onGround)
            {
                objectBody.velocity = new Vector2(objectBody.velocity.x, -fallingSpeed);
            }
            else
            {
                objectBody.velocity = new Vector2(objectBody.velocity.x, 0);
            }
        }
    }

    // Called when hitting ground after falling
    private void OnCollisionEnter2D(Collision2D collision)
    {
        onGround = true;
    }
}