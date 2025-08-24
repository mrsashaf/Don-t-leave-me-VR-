using UnityEngine;
using UnityEngine.SceneManagement; // <-- ”бедитесь, что эта строка есть

public static class PlayerPositionManager
{
    public static Vector3 RelativePosition { get; set; }
    public static Quaternion RelativeRotation { get; set; }
    public static bool HasSavedPosition { get; set; } = false;

    // --- ƒќЅј¬№“≈ Ё“” —“–ќ ” ---
    // «десь будет хранитьс€ ссылка на процесс загрузки
    public static AsyncOperation LoadingOperation { get; set; }
}