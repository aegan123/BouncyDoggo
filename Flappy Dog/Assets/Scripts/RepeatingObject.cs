using UnityEngine;


// Controls background repeats
public class RepeatingObject : MonoBehaviour
{
    public float fixedObjectLength; //Use this if the collider is wrong sized compared to the object
    private BoxCollider2D objectCollider;
    private Vector2 backgroundOffSet;
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

        backgroundOffSet = new Vector2(objectLength * 2f, 0);
    }

    // Called on every game frame
    private void Update()
    {
        float playerXPosition = Dog.instance.transform.position.x; //Apply dog's position (dog keeps falling behind x=0)
        if (transform.position.x + objectLength - playerXPosition < 0)
        {
            transform.position = (Vector2)transform.position + backgroundOffSet;
        }
    }
}