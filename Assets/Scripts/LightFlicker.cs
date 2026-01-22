using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Intensity Settings")]
    [SerializeField] private float minIntensity = 0.1f;
    [SerializeField] private float maxIntensity = 1.2f;

    [Header("Flicker Speed")]
    [SerializeField] private float flickerSpeed = 20f;

    [Header("Random Flicker Burst Settings")]
    [SerializeField] private float burstChance = 0.03f;
    [SerializeField] private float burstIntensity = 2f;
    [SerializeField] private float burstDuration = 0.1f;

    private Light _light;
    private float burstTimer = 0f;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        // Random chance for sudden horror burst
        if (burstTimer <= 0f && Random.value < burstChance)
        {
            burstTimer = burstDuration;
        }

        if (burstTimer > 0f)
        {
            burstTimer -= Time.deltaTime;
            _light.intensity = Mathf.Lerp(_light.intensity, burstIntensity, Time.deltaTime * flickerSpeed);
            return;
        }

        // Regular flicker (smooth noise)
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
