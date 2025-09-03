using UnityEngine;
using UnityEngine.AI; // Ќе забудьте эту строку дл€ работы с навигацией

// Ёта строка автоматически добавит компонент NavMeshAgent, если его нет
[RequireComponent(typeof(NavMeshAgent))]
public class CockroachAI : MonoBehaviour
{
    private NavMeshAgent agent;

    // ¬рем€ в секундах, через которое таракан исчезнет, если застр€нет
    public float maxLifetime = 15f;

    // Start() вызываетс€ один раз при по€влении таракана
    void Start()
    {
        // ѕолучаем компонент дл€ управлени€ движением
        agent = GetComponent<NavMeshAgent>();

        // —разу ищем случайное укрытие через наш спаунер
        Transform targetSpot = CockroachSpawner.instance.GetRandomSpot();

        // ≈сли укрытие было найдено, даем команду бежать к нему
        if (targetSpot != null)
        {
            agent.SetDestination(targetSpot.position);
        }

        // ”ничтожаем таракана через 'maxLifetime' секунд в любом случае
        Destroy(gameObject, maxLifetime);
    }

    // Update() вызываетс€ каждый кадр
    void Update()
    {
        // ѕровер€ем, добралс€ ли таракан до цели
        // !agent.pathPending означает, что путь уже построен
        // agent.remainingDistance - оставшеес€ рассто€ние до цели
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // ≈сли он остановилс€ и у него больше нет пути
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // ќн добралс€. ”ничтожаем его немедленно.
                Destroy(gameObject);
            }
        }
    }
}