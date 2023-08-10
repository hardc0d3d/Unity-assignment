
using UnityEngine;

public class Circle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line"))
        {
            Destroy(gameObject);
        }
    }
}

