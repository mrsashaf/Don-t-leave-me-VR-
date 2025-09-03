using UnityEngine;
using System.Collections.Generic;

public class LightFlickerManager : MonoBehaviour
{
    [Header("������� �������")]
    [Tooltip("���������� ���� ������� �������, ���������� ����������� (��������, ������ ����). ������ ��� ������ ��������� Light.")]
    public List<GameObject> lightGameObjects = new List<GameObject>();

    [Header("��������� ��������")]
    [Tooltip("��� ������ ���������� ��������. ��� ���� ��������, ��� �������.")]
    [Range(0.1f, 50f)]
    public float flickerSpeed = 10f;

    [Tooltip("����������� ������� � ��������� �� ����������� (��������, 0.5 = 50%).")]
    [Range(0f, 1f)]
    public float minIntensityMultiplier = 0.5f;

    [Tooltip("������������ ������� � ��������� �� ����������� (��������, 1.5 = 150%).")]
    [Range(1f, 3f)]
    public float maxIntensityMultiplier = 1.2f;

    // --- ���������� ����������, ������� ������ �������� �� ���������� ������������� ---
    private List<Light> foundLights = new List<Light>(); // ������ ��� �������� ��������� ������������
    private Dictionary<Light, float> initialIntensities;
    private Dictionary<Light, float> randomOffsets;

    void Awake()
    {
        // �������������� ������� � ������
        initialIntensities = new Dictionary<Light, float>();
        randomOffsets = new Dictionary<Light, float>();
        foundLights = new List<Light>();

        // �������� �� ������ ������� ��������, ������� �� ������� � ����������
        foreach (GameObject containerObject in lightGameObjects)
        {
            if (containerObject == null) continue; // ���������� ������ ����� � ������

            // --- ����� ������ ������ ---
            // ���� ��������� Light � ����� ������� ��� � ��� �������� ��������.
            // ��� ����� �������� ������.
            Light lightSource = containerObject.GetComponentInChildren<Light>();

            // ���� ���������� ��� ������...
            if (lightSource != null)
            {
                // ��������� ��� � ��� ������� ������
                foundLights.Add(lightSource);

                // � ��������� �� �� ������, ��� � ������: ���������� ������� � ������� ��������
                initialIntensities.Add(lightSource, lightSource.intensity);
                randomOffsets.Add(lightSource, Random.Range(0f, 1000f));
            }
            // ���� ���������� �� ��� ������...
            else
            {
                // ������� � ������� ��������������, ����� �� �����, ����� ������ �������� �������
                Debug.LogWarning($"� ������� '{containerObject.name}' �� ������ ��������� 'Light'. �� �� ����� �������.", containerObject);
            }
        }
    }

    void Update()
    {
        // ������ �� �������� � ����� ���������� ������� ��������� ������������
        foreach (Light lightSource in foundLights)
        {
            if (lightSource != null && lightSource.enabled)
            {
                float initialIntensity = initialIntensities[lightSource];
                float offset = randomOffsets[lightSource];

                float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, offset);

                float targetIntensity = Mathf.Lerp(
                    initialIntensity * minIntensityMultiplier,
                    initialIntensity * maxIntensityMultiplier,
                    noise
                );

                lightSource.intensity = targetIntensity;
            }
        }
    }
}