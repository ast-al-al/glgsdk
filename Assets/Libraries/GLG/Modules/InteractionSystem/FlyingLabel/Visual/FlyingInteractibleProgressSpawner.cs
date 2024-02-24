using GLG;
using UnityEngine;

public class FlyingInteractibleProgressSpawner : MonoBehaviour
{
    [SerializeField] private BaseInteractible _targetInteractionObject;
    [SerializeField] private bool _spawnGroupOnStart = true;
    [SerializeField] private VisualFlyingInteractionProgressGroup _groupPrefab;
    [SerializeField] private VisualFlyingInteractionProgress _itemPrefab;
    [SerializeField] private Vector3 _offset = Vector3.one;

    private bool _isSpawned = false;

    public FlyingInteractibleProgress Instance { get; private set; }

    private void Start()
    {
        if (_spawnGroupOnStart)
        {
            SpawnGroup();
        }
    }
    private void OnDestroy()
    {
        Despawn();
    }

    public void SpawnGroup()
    {
        if (_isSpawned) return;
        _isSpawned = true;
        FlyingLabelsOverlay flyingLabelsOverlay = Kernel.UI.Get<FlyingLabelsOverlay>();
        Instance = new FlyingInteractibleProgress
            (
                transform,
                _offset,
                true,
                _groupPrefab,
                _itemPrefab,
                _targetInteractionObject
            );
        Instance.StayOnScreen = true;
        Instance.LayoutThisItem = true;
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
