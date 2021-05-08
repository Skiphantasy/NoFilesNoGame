using UnityEngine;

public class LifeTime : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.7f);   
    }
}
