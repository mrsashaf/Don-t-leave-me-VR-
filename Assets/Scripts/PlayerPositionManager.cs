using UnityEngine;
using UnityEngine.SceneManagement; // <-- ���������, ��� ��� ������ ����

public static class PlayerPositionManager
{
    public static Vector3 RelativePosition { get; set; }
    public static Quaternion RelativeRotation { get; set; }
    public static bool HasSavedPosition { get; set; } = false;

    // --- �������� ��� ������ ---
    // ����� ����� ��������� ������ �� ������� ��������
    public static AsyncOperation LoadingOperation { get; set; }
}