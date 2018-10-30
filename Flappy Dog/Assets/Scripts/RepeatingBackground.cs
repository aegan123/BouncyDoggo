using UnityEngine;


// Controls background repeats
public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D backgroundCollider;
    private float backgroundLength;


    // Called once on every gaming session before Start
    private void Awake()
    {
        backgroundCollider = GetComponent<BoxCollider2D>();
        backgroundLength = backgroundCollider.size.x;
    }

    // Called on every game frame
    private void Update()
    {
        if (transform.position.x + backgroundLength < 0)
        {
            Vector2 backgroundOffSet = new Vector2(backgroundLength * 2f, 0);
            transform.position = (Vector2) transform.position + backgroundOffSet;
            Debug.Log("Repeated: " + this.name);
        }
    }
}