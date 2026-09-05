using UnityEngine;

public class LightTriggerDetector : MonoBehaviour
{
    public static float _damage = 200f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Light beam detected!");
        }

        if (other.CompareTag("Ally"))
        {
            Debug.Log("Ally detected!");
        }
    }
    
}
