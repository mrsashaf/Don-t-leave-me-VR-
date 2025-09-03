using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class LightMalfunctionEffect : MonoBehaviour
{
    [Header("���� 1: �������������� �����")]
    [Tooltip("������� ��� ���� ������, ������ ��� ���������.")]
    public int initialBlinks = 3;
    [Tooltip("��� ������ ���������� ����� (����� ����� ����������/�����������).")]
    public float blinkSpeed = 0.1f;

    [Header("���� 2: �����")]
    [Tooltip("�� ������� ������ ���� ��������� �������� ����� ������.")]
    public float deadTime = 2.0f;

    [Header("���� 3: ��������� ��������")]
    [Tooltip("�������� ���������� '����������' ��������.")]
    public float flickerSpeed = 20f;
    [Tooltip("����������� ������� ��� �������� (� % �� �����������).")]
    [Range(0f, 1f)]
    public float minFlickerIntensity = 0.2f;
    [Tooltip("������������ ������� ��� �������� (� % �� �����������).")]
    [Range(1f, 2f)]
    public float maxFlickerIntensity = 1.1f;

    [Header("����")]
    [Tooltip("����, ������� ������������� � ������ ������ '�������'.")]
    public AudioClip malfunctionSound;

    // ��������� ����������
    private Light lightComponent;
    private AudioSource audioSource;
    private float initialIntensity;
    private bool isFlickering = false;
    private float randomOffset;
    private bool hasBeenTriggered = false; // ����, ����� ������ �� ���������� ��������

    void Awake()
    {
        lightComponent = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        initialIntensity = lightComponent.intensity;
        randomOffset = Random.Range(0f, 1000f);

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!isFlickering)
        {
            return;
        }

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        float targetIntensity = Mathf.Lerp(
            initialIntensity * minFlickerIntensity,
            initialIntensity * maxFlickerIntensity,
            noise
        );
        lightComponent.intensity = targetIntensity;
    }

    /// <summary>
    /// ��������� �����: ��������� ��� ������������������ �������.
    /// �������� ��� �� ������ �������� (TriggerEvent) ��� ������� UnityEvent.
    /// </summary>
    public void StartEffect()
    {
        // ���� ������ ��� ��� �������, ������ �� ������
        if (hasBeenTriggered)
        {
            return;
        }
        hasBeenTriggered = true; // ��������, ��� ������ �������

        // ��������� ��������, ������� ��������� ���� �������������������
        StartCoroutine(MalfunctionRoutine());
    }

    private IEnumerator MalfunctionRoutine()
    {
        // --- ����������� ���� � ����� ������ ---
        if (malfunctionSound != null)
        {
            audioSource.PlayOneShot(malfunctionSound);
        }

        // --- ���� 1: ����� ---
        for (int i = 0; i < initialBlinks; i++)
        {
            lightComponent.intensity = 0f;
            yield return new WaitForSeconds(blinkSpeed);
            lightComponent.intensity = initialIntensity;
            yield return new WaitForSeconds(blinkSpeed);
        }

        // --- ���� 2: ����� ---
        lightComponent.intensity = 0f;
        yield return new WaitForSeconds(deadTime);

        // --- ������ ���� 3 ---
        isFlickering = true;
    }
}