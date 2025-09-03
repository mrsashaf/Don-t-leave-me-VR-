using System.Collections;
using UnityEngine;

public class CockroachSpawner : MonoBehaviour
{
    // --- ��� �����������, ����� 1 ---
    // ������� ����������� ����������, ����� � ��� ����� ���� ���������� �� ������ ������� �������
    public static CockroachSpawner instance;

    [Header("������ �� �������")]
    public GameObject cockroachPrefab;
    public Transform spawnPoint;

    [Header("��������� �����")]
    public int cockroachesInWave = 25;
    public float timeBetweenSpawns = 0.1f;

    [Header("���� ��� ���������")]
    public Transform[] hidingSpots;

    // --- ��� �����������, ����� 2 ---
    // Awake() ���������� ��� �������� �������, ��� �� ������ ����
    void Awake()
    {
        // ���������, �� ���� �� ���������� instance ��� ���-�� ������
        if (instance == null)
        {
            // ���� ���, �� "�" (���� ���������� ������ ��������) ���������� �������
            instance = this;
        }
        else
        {
            // ���� � ����� ��� ���� ������ �������, ���� ���������, ����� �� ���� ����������
            Destroy(gameObject);
        }
    }

    // ��������� ������� ��� ������ �����
    public void TriggerWave()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    // ������� ��� ��������� ���������� �������
    public Transform GetRandomSpot()
    {
        if (hidingSpots.Length == 0)
        {
            Debug.LogError("� �������� �� ������� �� ������ ������� (Hiding Spot)!");
            return null;
        }
        int randomIndex = Random.Range(0, hidingSpots.Length);
        return hidingSpots[randomIndex];
    }

    // �������� ��� �������� �����
    IEnumerator SpawnWaveCoroutine()
    {
        for (int i = 0; i < cockroachesInWave; i++)
        {
            Instantiate(cockroachPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}