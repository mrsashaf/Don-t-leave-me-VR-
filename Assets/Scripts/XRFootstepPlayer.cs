using UnityEngine;
using System.Collections.Generic;

// ��������, ��� �� ������� ���� AudioSource ��� ������������ ������
[RequireComponent(typeof(AudioSource))]
public class XRFootstepPlayer : MonoBehaviour
{
    [Header("���� ��� ������������")]
    [Tooltip("���������� ���� ������� ������ ������ ������ (Main Camera). ��� ������� ����� �������������.")]
    public Transform playerHeadTransform;

    [Header("��������� �����")]
    [Tooltip("������ ������ �����. ����� ������������� ��������� ���� �� ����� ������.")]
    public List<AudioClip> footstepSounds;

    [Tooltip("����� ���������� � ������ ������ ������ �����, ����� ��������� ��������� ���.")]
    public float stepDistance = 0.8f;

    [Tooltip("��������� ������ ����� (�� 0.0 �� 1.0).")]
    [Range(0f, 1f)]
    public float volume = 0.5f;

    // ��������� ���������� ��� ������ �������
    private AudioSource audioSource;
    private Vector3 lastStepPosition;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // ������ ��������: ���� �� ������� ������ ������, ������ �� ����� ��������
        if (playerHeadTransform == null)
        {
            Debug.LogError("�� �������� 'playerHeadTransform'! ������ ����� �� ����� ��������. ���������� Main Camera � ���������.", this);
            enabled = false; // ��������� ������, ����� �� �������� ������ � Update
            return;
        }

        // ���������� ��������� ������� ������
        lastStepPosition = playerHeadTransform.position;
    }

    void Update()
    {
        // ���� ���-�� �� ���������, ������ �� ������
        if (playerHeadTransform == null || footstepSounds.Count == 0)
        {
            return;
        }

        // --- �������� ������ ---

        // �� ���������� ������ X � Z ����������, ����� ������������ �������� �����/���� (������, ����������)
        Vector3 currentPositionXZ = new Vector3(playerHeadTransform.position.x, 0, playerHeadTransform.position.z);
        Vector3 lastPositionXZ = new Vector3(lastStepPosition.x, 0, lastStepPosition.z);

        // �������, ����� ���������� ������ ����� �� ����������� � ������� ���������� ����
        float distanceMoved = Vector3.Distance(currentPositionXZ, lastPositionXZ);

        // ���� ���������� ���������� ������ ��� ����� ������...
        if (distanceMoved >= stepDistance)
        {
            // ...����������� ���� ����!
            PlayFootstepSound();

            // � ��������� ������� ���������� ���� �� ������� �������
            lastStepPosition = playerHeadTransform.position;
        }
    }

    private void PlayFootstepSound()
    {
        // �������� ��������� ���� �� ������
        AudioClip clipToPlay = footstepSounds[Random.Range(0, footstepSounds.Count)];

        // ����������� ��� � �������� ����������
        audioSource.PlayOneShot(clipToPlay, volume);
    }
}