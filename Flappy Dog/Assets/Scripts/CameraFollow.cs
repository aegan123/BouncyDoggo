using UnityEngine;


// Locks Main Camera to player's x-position
public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;


    // Called on every game frame
    private void FixedUpdate()
    {
        transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
    }
}