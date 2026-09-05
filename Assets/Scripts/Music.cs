using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProceduralMusic : MonoBehaviour
{
    [Header("Настройки темпа и тональности")]
    public float tempo = 60f; // Удары в минуту (медленный эмбиент)

    // Ноты минорной гаммы (Ля-минор / A minor)
    private float[] frequencies = new float[] { 220.00f, 246.94f, 261.63f, 293.66f, 329.63f, 349.23f, 392.00f };

    private double sampleRate;
    private double phase;
    private float currentFreq = 220f;
    private float timer = 0f;
    private float targetVolume = 0.15f;

    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 60f / tempo)
        {
            timer = 0f;
            // Случайный выбор ноты из гаммы для создания атмосфера
            int randomIndex = Random.Range(0, frequencies.Length);
            currentFreq = frequencies[randomIndex];
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        double increment = currentFreq * 2.0 * System.Math.PI / sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            float waveValue = (float)System.Math.Sin(phase) * targetVolume;

            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = waveValue;
            }

            if (phase > 2.0 * System.Math.PI)
            {
                phase -= 2.0 * System.Math.PI;
            }
        }
    }
}