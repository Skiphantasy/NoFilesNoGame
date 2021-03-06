using UnityEngine;

public class DestroyObjectFalling : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("ObjectFalling"))
        {
            Destroy(collision.gameObject);
        }
    }
}
