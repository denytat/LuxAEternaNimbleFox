using UnityEngine;

public class LightBeamConrtoller : MonoBehaviour
{
    public Camera _camera;

    void Update()
    {
        RotateTowardsMouse();
    }
    
    void RotateTowardsMouse()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (mousePosition - transform.position).normalized;

        float _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, _angle);
    }
}
