using UnityEngine;

public class MovementObjects : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
    } 
}
