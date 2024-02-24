using GLG;
using UnityEngine;

public class FlyingInteractorProgressSpawner : MonoBehaviour
{
    [SerializeField] private BaseInteractor _targetInteractionObject;
    [SerializeField] private bool _spawnOnStart = true;
    [SerializeField] private VisualFlyingInteractionProgress _visualPrefab;
    [SerializeField] private Vector3 _offset = Vector3.one;

    private bool _isSpawned = false;

    public FlyingInteractorProgress Instance { get; private set; }

    private void Start()
    {
        if (_spawnOnStart)
        {
            Spawn();
        }
    }
    private void OnDestroy()
    {
        Despawn();
    }

    public void Spawn()
    {
        if (_isSpawned) return;
        _isSpawned = true;
        FlyingLabelsOverlay flyingLabelsOverlay = Kernel.UI.Get<FlyingLabelsOverlay>();
        Instance = new FlyingInteractorProgress
            (
                transform,
                _offset,
                true,
                _visualPrefab,
                _targetInteractionObject
            );
        Instance.StayOnScreen = false;
        Instance.LayoutThisItem = false;
        Instance.CombineWithSimilarItems = false;
        flyingLabelsOverlay.CreateItem(Instance);
    }
    public void Despawn()
    {
        if (!_isSpawned) return;
        _isSpawned = false;
        Instance.Dispose();
    }
}
