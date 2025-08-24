using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPositionManager.HasSavedPosition)
        {
            Debug.Log("Обнаружены сохраненные данные. Ищем портал для спауна.");

            // --- ЛОГИКА ИЗМЕНЕНА ---

            // Находим объект триггера в новой сцене по нашему тегу
            GameObject exitPortal = GameObject.FindGameObjectWithTag("PortalTrigger");

            if (exitPortal != null)
            {
                // Применяем смещение к найденному порталу

                // Конвертируем относительную позицию обратно в мировую, но уже от нового портала
                transform.position = exitPortal.transform.TransformPoint(PlayerPositionManager.RelativePosition);

                // Применяем относительное вращение к вращению нового портала
                transform.rotation = exitPortal.transform.rotation * PlayerPositionManager.RelativeRotation;

                Debug.Log("Игрок перемещен в относительную позицию новой сцены.");
            }
            else
            {
                Debug.LogError("Не удалось найти объект с тегом 'PortalTrigger' в новой сцене! Игрок не будет телепортирован.");
            }

            // Сбрасываем флаг в любом случае, чтобы избежать проблем
            PlayerPositionManager.HasSavedPosition = false;
        }
    }
}