using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;          // Movement speed
    public float rotationSpeed = 100f; // Rotation speed

    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        transform.Translate(movement);

        // Rotation logic
        if (Input.GetKey(KeyCode.LeftArrow)) // Rotate left
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Rotate right
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
