using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AutoWallpaperTiling_PB_Stable : MonoBehaviour
{
    public enum MappingPlane { XY, XZ, ZY }

    [Header("Mapping")]
    public MappingPlane plane = MappingPlane.XY;

    [Tooltip("Сколько повторов на 1 метр (обе оси)")]
    public float tilesPerMeter = 1f;

    [Header("Material slot")]
    public int materialIndex = 0;

    [Header("Reapply policy")]
    [Tooltip("В редакторе (в т.ч. после Bake) переустанавливать каждый кадр")]
    public bool reapplyEveryFrameInEditor = true;

    [Tooltip("В игре переустанавливать каждый кадр (включай, если что-то сбрасывает ST во время рантайма)")]
    public bool reapplyEveryFrameInPlaymode = false;

    Renderer _r;
    MaterialPropertyBlock _mpb;

    static readonly int MAIN_TEX_ST = Shader.PropertyToID("_MainTex_ST");  // Built-in/Standard
    static readonly int BASEMAP_ST = Shader.PropertyToID("_BaseMap_ST");  // URP Lit

    void OnEnable()
    {
        _r = GetComponent<Renderer>();
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        Apply();
    }

    void OnValidate()
    {
        if (tilesPerMeter <= 0f) tilesPerMeter = 1f;
        if (_r == null) _r = GetComponent<Renderer>();
        if (_mpb == null) _mpb = new MaterialPropertyBlock();
        Apply();
    }

    void Update()
    {
        // Главное: в редакторе после лайтбейка PropertyBlock часто сбрасывается.
        // Этот реапплай каждый кадр гарантирует неизменный вид.
        bool need =
#if UNITY_EDITOR
            (!Application.isPlaying && reapplyEveryFrameInEditor) ||
#endif
            (Application.isPlaying && reapplyEveryFrameInPlaymode);

        if (need) Apply();
    }

    void OnTransformParentChanged() => Apply(); // на всякий случай при перестановках

    void Apply()
    {
        if (_r == null) return;
        if (materialIndex < 0 || materialIndex >= _r.sharedMaterials.Length) return;

        // Размер по локальному мешу * lossyScale (корректно при любых поворотах)
        Vector3 size = GetWorldSizeFromMesh();
        Vector2 axis = plane switch
        {
            MappingPlane.XY => new Vector2(size.x, size.y),
            MappingPlane.XZ => new Vector2(size.x, size.z),
            MappingPlane.ZY => new Vector2(size.z, size.y),
            _ => new Vector2(size.x, size.y)
        };

        Vector2 tiling = axis * tilesPerMeter;

        _r.GetPropertyBlock(_mpb, materialIndex);
        _mpb.SetVector(MAIN_TEX_ST, new Vector4(tiling.x, tiling.y, 0f, 0f));
        _mpb.SetVector(BASEMAP_ST, new Vector4(tiling.x, tiling.y, 0f, 0f));
        _r.SetPropertyBlock(_mpb, materialIndex);
    }

    Vector3 GetWorldSizeFromMesh()
    {
        var mf = GetComponent<MeshFilter>();
        if (mf && mf.sharedMesh)
        {
            Vector3 local = mf.sharedMesh.bounds.size;
            Vector3 s = transform.lossyScale;
            return new Vector3(Mathf.Abs(local.x * s.x),
                               Mathf.Abs(local.y * s.y),
                               Mathf.Abs(local.z * s.z));
        }
        // фолбек
        return transform.lossyScale;
    }
}
