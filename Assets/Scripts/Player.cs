using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public bool canMove = true;

    void Update()
    {
        if (!canMove) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
}