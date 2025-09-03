using UnityEngine;
using UnityEngine.AI; // �� �������� ��� ������ ��� ������ � ����������

// ��� ������ ������������� ������� ��������� NavMeshAgent, ���� ��� ���
[RequireComponent(typeof(NavMeshAgent))]
public class CockroachAI : MonoBehaviour
{
    private NavMeshAgent agent;

    // ����� � ��������, ����� ������� ������� ��������, ���� ���������
    public float maxLifetime = 15f;

    // Start() ���������� ���� ��� ��� ��������� ��������
    void Start()
    {
        // �������� ��������� ��� ���������� ���������
        agent = GetComponent<NavMeshAgent>();

        // ����� ���� ��������� ������� ����� ��� �������
        Transform targetSpot = CockroachSpawner.instance.GetRandomSpot();

        // ���� ������� ���� �������, ���� ������� ������ � ����
        if (targetSpot != null)
        {
            agent.SetDestination(targetSpot.position);
        }

        // ���������� �������� ����� 'maxLifetime' ������ � ����� ������
        Destroy(gameObject, maxLifetime);
    }

    // Update() ���������� ������ ����
    void Update()
    {
        // ���������, �������� �� ������� �� ����
        // !agent.pathPending ��������, ��� ���� ��� ��������
        // agent.remainingDistance - ���������� ���������� �� ����
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // ���� �� ����������� � � ���� ������ ��� ����
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // �� ��������. ���������� ��� ����������.
                Destroy(gameObject);
            }
        }
    }
}