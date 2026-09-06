using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    public Slider batterySlider;
    public TextMeshProUGUI percentageText;

    void Update()
    {
        if (LightbeamController.Instance == null) return;

        float current = LightbeamController.Instance.currentBattery;
        float max = LightbeamController.Instance.maxBattery;
        float ratio = current / max;

        if (batterySlider != null)
        {
            batterySlider.value = ratio;
        }

        if (percentageText != null)
        {
            percentageText.text = $"{Mathf.CeilToInt(ratio * 100f)}%";
        }
    }
}