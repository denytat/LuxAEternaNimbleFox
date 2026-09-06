using UnityEngine;

public class DarknessManager : MonoBehaviour
{
    [Header("References")]
    public RectTransform lightMaskTransform;

    [Header("Darkness Scaling Settings")]
    public Vector3 wave1Scale = new Vector3(5f, 5f, 1f);
    public Vector3 minScale = new Vector3(1.2f, 1.2f, 1f); 
    public int maxShrinkWave = 10;
    public float transitionSpeed = 2f;

    private Vector3 _targetScale;

    void Start()
    {
        _targetScale = wave1Scale;
        if (lightMaskTransform != null)
        {
            lightMaskTransform.localScale = wave1Scale;
        }
    }

    void Update()
    {
        if (WaveManager.Instance == null || lightMaskTransform == null) return;

        int currentWave = WaveManager.Instance.currentWave;
        float progress = Mathf.Clamp01((float)(currentWave - 1) / (maxShrinkWave - 1));

        _targetScale = Vector3.Lerp(wave1Scale, minScale, progress);

        lightMaskTransform.localScale = Vector3.Lerp(
            lightMaskTransform.localScale, 
            _targetScale, 
            Time.deltaTime * transitionSpeed
        );
    }
}