using System;
using System.Collections.Generic;
using UnityEngine;

public enum VFXKind { None, RifleBulletHit, RocketHit, RifleShot, BigExplosion }

public class VFXFactory : MonoBehaviour, IManagedService
{
    [SerializeField] private VFXPool _universalParticles;
    [SerializeField] private VFXPool[] _pools;

    private Dictionary<VFXKind, VFXPool> _poolsDictionary;

    public Type Type => typeof(VFXFactory);

    private void Awake()
    {
        _poolsDictionary = new Dictionary<VFXKind, VFXPool>(_pools.Length);
        for (int i = 0; i < _pools.Length; i++)
        {
            VFXPool pool = _pools[i];
            _poolsDictionary.Add(pool.vfxKind, pool);
            pool.Initialize(transform);
        }
        _universalParticles.Initialize(transform);
    }

    public void Spawn(VFXKind vfxKind, Vector3 position, Vector3 up)
    {
        if (vfxKind == VFXKind.None) return;
        _poolsDictionary[vfxKind].Spawn(position, up);
    }
    public void SpawnUniversal(Vector3 position, Vector3 up, Material material, Mesh mesh)
    {
        _universalParticles.Spawn(position, up).SetMainMaterial(material).SetMesh(mesh);
    }
}

[System.Serializable]
public class VFXPool
{
    public VFXKind vfxKind;
    public GameObject prefab;
    public int quantity;

    private int _counter = 0;
    private VFXPoolItem[] _items;

    public void Initialize(Transform parent)
    {
        _items = new VFXPoolItem[quantity];
        for (int i = 0; i < quantity; i++)
        {
            VFXPoolItem item = GameObject.Instantiate(prefab, parent).GetComponent<VFXPoolItem>();
            item.Initialize();
            _items[i] = item;
        }
    }
    public VFXPoolItem Spawn(Vector3 position, Vector3 up)
    {
        _counter++;
        if (_counter == quantity) _counter = 0;
        return _items[_counter].Activate(position, up);
    }
}