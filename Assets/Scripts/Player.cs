using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;         // Speed for forward/backward movement
    public float rotationSpeed = 100f;  // Speed for rotation

    void Update()
    {
        
        float vertical = Input.GetAxis("Vertical"); 
        Vector3 forwardMovement = transform.forward * vertical * moveSpeed * Time.deltaTime;
        transform.position += forwardMovement;

       
        float horizontal = Input.GetAxis("Horizontal"); 
        transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
    }
}
