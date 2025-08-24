using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AutoWallpaperTiling : MonoBehaviour
{
    public enum MappingPlane { XY, XZ, ZY }

    [Header("Масштаб")]
    [Tooltip("Сколько тайлов на метр. 1 = один повтор текстуры на 1 м.")]
    public float tilesPerMeter = 1f;

    [Tooltip("Какая плоскость у стены считается лицевой (ось UxV).")]
    public MappingPlane plane = MappingPlane.XY;

    [Header("Материал")]
    [Tooltip("Если у меша несколько материалов, выбери индекс целевого.")]
    public int materialIndex = 0;

    [Tooltip("Обновлять каждый кадр (удобно в редакторе). На билде можно выключить.")]
    public bool continuousUpdate = true;

    Renderer _renderer;
    MaterialPropertyBlock _mpb;

    void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        Apply();
    }

    void Update()
    {
        if (continuousUpdate) Apply();
    }

    void OnValidate()
    {
        if (tilesPerMeter <= 0f) tilesPerMeter = 1f;
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        Apply();
    }

    void Apply()
    {
        if (_renderer == null) return;

        // вычисляем физический размер объекта по двум осям выбранной плоскости
        Vector2 worldSize = GetWorldSizeOnPlane();

        // сколько раз должна повториться текстура по осям
        Vector2 tiling = worldSize * tilesPerMeter;

        // подаём в материал через PropertyBlock (не клонируем материал)
        _renderer.GetPropertyBlock(_mpb, materialIndex);

        // Для Standard/Built-in:
        _mpb.SetVector("_MainTex_ST", new Vector4(tiling.x, tiling.y, 0f, 0f));
        // Для URP (Base Map):
        _mpb.SetVector("_BaseMap_ST", new Vector4(tiling.x, tiling.y, 0f, 0f));

        _renderer.SetPropertyBlock(_mpb, materialIndex);
    }

    Vector2 GetWorldSizeOnPlane()
    {
        // Пытаемся взять размер из меша (точнее, чем просто lossyScale)
        Vector3 size = Vector3.one;
        var mf = GetComponent<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            // bounds в локале → переводим в мир
            Vector3 localSize = mf.sharedMesh.bounds.size;
            Vector3 s = transform.lossyScale;
            size = new Vector3(Mathf.Abs(localSize.x * s.x),
                               Mathf.Abs(localSize.y * s.y),
                               Mathf.Abs(localSize.z * s.z));
        }
        else
        {
            // запасной вариант
            size = transform.lossyScale;
        }

        switch (plane)
        {
            case MappingPlane.XY: return new Vector2(size.x, size.y);
            case MappingPlane.XZ: return new Vector2(size.x, size.z);
            case MappingPlane.ZY: return new Vector2(size.z, size.y);
            default: return new Vector2(size.x, size.y);
        }
    }
}
