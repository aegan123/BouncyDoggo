using UnityEngine;


// Controls background repeats
public class RepeatingObject : MonoBehaviour
{
    public float fixedObjectLength; //Use this if the collider is wrong sized compared to the object
    private BoxCollider2D objectCollider;
    private float objectLength;


    // Called once on every gaming session before Start
    private void Awake()
    {
        objectCollider = GetComponent<BoxCollider2D>();
        if (fixedObjectLength != 0)
        {
            objectLength = fixedObjectLength;
        }
        else
        {
            objectLength = objectCollider.size.x;
        }
    }

    // Called on every game frame
    private void Update()
    {
        if (transform.position.x + objectLength < 0)
        {
            Vector2 backgroundOffSet = new Vector2(objectLength * 2f, 0);
            transform.position = (Vector2) transform.position + backgroundOffSet;
        }
    }
}
