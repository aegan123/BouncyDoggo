using UnityEngine;


// Scrolls objects towards player & handles falling objects
public class FallingObject : MonoBehaviour
{
    public float fallingSpeed = 4;
    public GameObject objectUnder;

    private Rigidbody2D objectBody;
    private bool active;
    private bool catJumped;


    // Called once on every gaming session before Start
    private void Awake()
    {
        objectBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (objectUnder != null)
        {
            active = true;
        }
    }

    // Called on every game frame
    private void Update()
    {
        if (active && objectUnder == null)
        {
            if (objectBody.gameObject.name.Contains("cat"))
            {
                foreach (Collider2D col in objectBody.gameObject.GetComponents<Collider2D>())
                {
                    Debug.Log("Disabled cat collider");
                    col.enabled = false;
                }
            }
            objectBody.velocity = new Vector2(objectBody.velocity.x, -fallingSpeed);
        }
    }

    public void SetObjectUnder(GameObject obj)
    {
        objectUnder = obj;
    }
}