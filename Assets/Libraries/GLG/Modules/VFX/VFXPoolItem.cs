using UnityEngine;

public class VFXPoolItem : MonoBehaviour
{
    public ParticleSystem mainParticleSystem;
    public ParticleSystemRenderer mainRenderer;
    private Transform _cashedTransform;
    private GameObject _cashedGameObject;

    public VFXPoolItem Activate(Vector3 pos, Vector3 up)
    {
        _cashedGameObject.SetActive(true);
        _cashedTransform.position = pos;
        _cashedTransform.up = up;
        mainParticleSystem.Play(true);
        return this;
    }
    public VFXPoolItem Initialize()
    {
        _cashedGameObject = gameObject;
        _cashedTransform = transform;
        _cashedGameObject.SetActive(false);
        return this;
    }
    public VFXPoolItem Disable()
    {
        _cashedGameObject.SetActive(false);
        return this;
    }
    public VFXPoolItem SetMainMaterial(Material material)
    {
        mainRenderer.material = material;
        return this;
    }
    public VFXPoolItem SetMesh(Mesh mesh)
    {
        mainRenderer.mesh = mesh;
        return this;
    }
}