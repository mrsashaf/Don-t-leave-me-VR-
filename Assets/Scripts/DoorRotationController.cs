using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DoorRotationController : MonoBehaviour
{
    public enum DoorState { Closed, Creaked, FullyOpen }

    [Header("Начальное состояние")]
    [Tooltip("Укажите здесь, в каком состоянии дверь находится в НАЧАЛЕ сцены.")]
    public DoorState initialState = DoorState.Closed;

    private DoorState currentState;

    [Header("Настройки вращения")]
    public float rotationSpeed = 3.0f;
    public float openAngle = 90.0f;
    public float creakAngle = 30.0f;
    public bool opensInDefaultDirection = true;

    [Header("Блокировка")]
    public bool isLocked = false;
    public float jiggleDuration = 0.3f;
    public float jiggleIntensity = 1.5f;

    [Header("Звуки")]
    public AudioClip[] openSounds;
    public AudioClip[] closeSounds;
    public AudioClip[] lockedSounds;

    [Header("Настройки 3D звука")]
    public float soundMinDistance = 1.0f;
    public float soundMaxDistance = 15.0f;

    [Header("События (Events)")]
    public UnityEvent OnDoorOpened;
    public UnityEvent OnDoorClosed;
    public UnityEvent OnLockedDoorTried;

    [SerializeField, HideInInspector]
    private Quaternion savedClosedRotation;

    private Quaternion closedRotation;
    private AudioSource audioSource;
    private Quaternion openRotation, creakRotation;
    private int lastOpenSoundIndex = -1, lastCloseSoundIndex = -1, lastLockedSoundIndex = -1;

    // --- ИСПРАВЛЕНИЕ 1: Добавляем флаги состояния ---
    private bool _isJiggling = false;
    private bool _isMoving = false; // Главный флаг, который решает проблему

    [ContextMenu("Set Current Rotation as CLOSED")]
    private void SetClosedRotation()
    {
        savedClosedRotation = transform.rotation;
        Debug.Log($"Закрытое положение для двери '{gameObject.name}' сохранено!", this);
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = soundMinDistance;
        audioSource.maxDistance = soundMaxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
    }

    void Start()
    {
        if (savedClosedRotation.w != 0 || savedClosedRotation.x != 0 || savedClosedRotation.y != 0 || savedClosedRotation.z != 0)
        {
            closedRotation = savedClosedRotation;
        }
        else
        {
            closedRotation = transform.rotation;
            Debug.LogWarning($"'savedClosedRotation' не было установлено для двери '{gameObject.name}'. Используйте 'Set Current Rotation as CLOSED' в меню компонента.", this);
        }

        currentState = initialState;

        float finalOpenAngle = opensInDefaultDirection ? openAngle : -openAngle;
        float finalCreakAngle = opensInDefaultDirection ? creakAngle : -creakAngle;

        openRotation = closedRotation * Quaternion.Euler(0, finalOpenAngle, 0);
        creakRotation = closedRotation * Quaternion.Euler(0, finalCreakAngle, 0);
    }

    void Update()
    {
        if (_isJiggling) return;

        Quaternion targetRotation;
        switch (currentState)
        {
            case DoorState.FullyOpen:
                targetRotation = openRotation;
                break;
            case DoorState.Creaked:
                targetRotation = creakRotation;
                break;
            case DoorState.Closed:
            default:
                targetRotation = closedRotation;
                break;
        }

        // --- ИСПРАВЛЕНИЕ 2: Изменяем логику вращения ---
        // Если разница между текущим и целевым поворотом достаточно велика, то движемся
        if (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            _isMoving = true; // Сообщаем, что дверь в движении
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Если мы достигли цели, выравниваем поворот и сбрасываем флаг
            if (_isMoving)
            {
                transform.rotation = targetRotation;
                _isMoving = false; // Сообщаем, что дверь остановилась
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // --- ИСПРАВЛЕНИЕ 3: Добавляем проверки ---
            // Не открывать, если дверь уже движется или уже открыта
            if (_isMoving || currentState == DoorState.FullyOpen) return;

            if (isLocked)
            {
                if (_isJiggling) return;
                StartCoroutine(JiggleDoorRoutine());
                return;
            }

            currentState = DoorState.FullyOpen;
            PlayRandomSound(openSounds, ref lastOpenSoundIndex);
            OnDoorOpened.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // --- ИСПРАВЛЕНИЕ 4: Добавляем проверки ---
            // Не закрывать, если дверь уже движется или уже закрыта
            if (_isMoving || currentState == DoorState.Closed) return;

            currentState = DoorState.Closed;
            PlayRandomSound(closeSounds, ref lastCloseSoundIndex);
            OnDoorClosed.Invoke();
        }
    }

    // Остальные функции без изменений...
    public void CloseAndLock() { if (currentState == DoorState.Closed) { isLocked = true; return; } currentState = DoorState.Closed; isLocked = true; PlayRandomSound(closeSounds, ref lastCloseSoundIndex); OnDoorClosed.Invoke(); }
    public void UnlockAndCreakOpen() { isLocked = false; if (currentState == DoorState.Closed) { currentState = DoorState.Creaked; PlayRandomSound(openSounds, ref lastOpenSoundIndex); } }
    public void LockDoor() { isLocked = true; }
    public void UnlockDoor() { isLocked = false; }
    private IEnumerator JiggleDoorRoutine() { _isJiggling = true; PlayRandomSound(lockedSounds, ref lastLockedSoundIndex); OnLockedDoorTried.Invoke(); Quaternion originalRotation = transform.rotation; float elapsedTime = 0f; while (elapsedTime < jiggleDuration) { float randomAngle = Random.Range(-1.0f, 1.0f) * jiggleIntensity; transform.rotation = originalRotation * Quaternion.Euler(0, randomAngle, 0); elapsedTime += Time.deltaTime; yield return null; } transform.rotation = closedRotation; _isJiggling = false; }
    private void PlayRandomSound(AudioClip[] sounds, ref int lastIndex) { if (sounds == null || sounds.Length == 0) return; int newIndex; if (sounds.Length == 1) { newIndex = 0; } else { do { newIndex = Random.Range(0, sounds.Length); } while (newIndex == lastIndex); } lastIndex = newIndex; audioSource.PlayOneShot(sounds[newIndex]); }
}