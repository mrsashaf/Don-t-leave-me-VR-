using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class WalkingSoundSimulator : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Звуковые файлы")]
    public AudioClip[] footstepSounds;

    [Header("Параметры пути")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Настройки ходьбы")]
    public float walkDuration = 5.0f;
    public float timeBetweenSteps = 0.5f;

    [Header("События")]
    public UnityEvent onArrivalCompleted;
    public UnityEvent onRetreatCompleted;

    private bool isWalking = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
    }

    public void StartArrival()
    {
        if (!isWalking)
        {
            StartCoroutine(WalkRoutine(startPoint, endPoint, onArrivalCompleted));
        }
    }

    public void StartRetreat()
    {
        if (!isWalking)
        {
            StartCoroutine(WalkRoutine(endPoint, startPoint, onRetreatCompleted));
        }
    }

    private IEnumerator WalkRoutine(Transform fromPoint, Transform toPoint, UnityEvent onCompletedEvent)
    {
        isWalking = true;

        if (fromPoint == null || toPoint == null)
        {
            Debug.LogError("Не заданы начальная или конечная точки!", this);
            isWalking = false;
            yield break;
        }

        transform.position = fromPoint.position;
        float journeyTime = 0f;

        while (journeyTime < walkDuration)
        {
            if (footstepSounds.Length > 0)
            {
                audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
            }

            yield return new WaitForSeconds(timeBetweenSteps);
            journeyTime += timeBetweenSteps;

            float percentOfJourney = journeyTime / walkDuration;
            transform.position = Vector3.Lerp(fromPoint.position, toPoint.position, percentOfJourney);
        }

        // Гарантируем, что объект точно в конечной точке и ОСТАНАВЛИВАЕТСЯ там
        transform.position = toPoint.position;

        if (onCompletedEvent != null)
        {
            onCompletedEvent.Invoke();
        }

        isWalking = false;
    }
}