using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    public TextMeshProUGUI waveText; 

    void Update()
    {
        if (WaveManager.Instance != null && waveText != null)
        {
            waveText.text = $"Wave: {WaveManager.Instance.currentWave}";
        }
    }
}