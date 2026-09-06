using UnityEngine;

public class LightTriggerDetector : MonoBehaviour
{
    public static float damagePerSecond = 50f;

    private void OnTriggerStay2D(Collider2D other)
    {
        // 1. Check if the beam is powered (W key is held)
        if (!LightbeamController.IsBeamActive) return;

        // 2. Look for an Enemy or Boss component on the entering object
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
                Debug.Log($"[BEAM ATTACK] Dealing damage to {other.name}!");
                return;
            }
        }
    }
}