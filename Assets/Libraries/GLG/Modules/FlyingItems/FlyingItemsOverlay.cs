using GLG;
using GLG.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum FlyingItemSpace { WorldToCanvas, WorldToWorld, CanvasToWorld, CanvasToCanvas }
public class FlyingItem : IDisposable
{
    public System.Action callbackOnArrived;
    public Transform animationObject;
    public Vector3 spawnPosition;
    public Vector3 start;
    public Vector3 end;
    public int stage; // -1 delay; 0 scattering; 1 animation
    public float animationDuration;
    public float scatteringDuration;
    public float elapsedTime;
    public float delayBeforeStart;
    public void Dispose()
    {
        callbackOnArrived = null;
        animationObject = null;
    }
}
public class FlyingItemsContainer : IDisposable
{
    public int itemsCount;
    public List<FlyingItem> items = new List<FlyingItem>();
    public System.Action callbackOnAllItemsArrived;

    public void Dispose()
    {
        items = null;
        callbackOnAllItemsArrived = null;
    }
}


public class FlyingItemsOverlay : UIController
{
    public GameObject[] itemsPrefabs;
    public float rotationRandom = 15f;
    public float scatteringDuration = 0.2f;
    public float scatteringRadius = 100f;
    public AnimationCurve scaleAnimationCurve;
    public AnimationCurve movingAnimationCurve;
    public AnimationCurve scatteringMovingAnimationCurve;

    private Camera _camera;
    private List<FlyingItem> _items = new List<FlyingItem>();
    private List<FlyingItemsContainer> _itemsContainers = new List<FlyingItemsContainer>();
    private int _itemsCount = 0;
    private int _itemsContainersCount = 0;

    private void Update()
    {
        ProcessItems();
        ProcessItemsContainers();

    }
    protected override void OnStartShow()
    {
        base.OnStartShow();
        if (!_camera) _camera = Kernel.CameraService.MainCamera;
    }


    /// <summary>
    /// Создает предмет и начинает перемещать его от стартовой точки в конечную.
    /// </summary>
    /// <param name="space">Тип начальной и конечной точек</param>
    /// <param name="prefabid">Префаб предмета для создания</param>
    /// <param name="start">Точка появления предмета</param>
    /// <param name="target">Точка, в которую прилетит предмет</param>
    /// <param name="duration">Общая длительность анимации перемещения</param>
    /// <param name="scatteringDurationProportion">Доля длительности анимации разлета относительно общей длительности. Если 0, то без разлета</param>
    /// <param name="onItemArrived">Обратный вызов по прибытию предмета на конечную точку</param>
    /// <returns></returns>
    public FlyingItemsOverlay Spawn(FlyingItemSpace space, int prefabid, Vector3 start, Vector3 target, float duration, float scatteringDurationProportion = 0.2f, Action onItemArrived = null)
    {
        Kernel.UI.Show<FlyingItemsOverlay>();
        Transform spawnedObject = Instantiate(itemsPrefabs[prefabid], _container).transform;
        FlyingItem item = CreateFlyingItem(ConvertPoints(space, (start, target)), spawnedObject, duration, scatteringDurationProportion, 0f, onItemArrived);
        _items.Add(item);
        _itemsCount++;
        return this;
    }
    /// <summary>
    /// Создает предметы и начинает перемещать их от стартовой точки в конечную.
    /// </summary>
    /// <param name="space">Тип начальной и конечной точек</param>
    /// <param name="prefabid">Префаб предметов для создания</param>
    /// <param name="count">Количество предметов</param>
    /// <param name="start">Точка появления предметов</param>
    /// <param name="target">Точка, в которую прилетят предметы</param>
    /// <param name="duration">Общая длительность анимации перемещения</param>
    /// <param name="spawnDelay">Задержка между появлением предметов</param>
    /// <param name="scatteringDurationProportion">Доля длительности анимации разлета относительно общей длительности. Если 0, то без разлета</param>
    /// <param name="onAllItemsArrived">Обратный вызов, когда все предметы достигли точки</param>
    /// <returns></returns>
    public FlyingItemsOverlay Spawn(FlyingItemSpace space, int prefabid, int count, Vector3 start, Vector3 target, float duration, float spawnDelay = 0.1f, float scatteringDurationProportion = 0.2f, Action onItemArrived = null, Action onAllItemsArrived = null)
    {
        Kernel.UI.Show<FlyingItemsOverlay>();
        FlyingItemsContainer itemsContainer = new FlyingItemsContainer();
        itemsContainer.callbackOnAllItemsArrived = onAllItemsArrived;
        (Vector3, Vector3) convertedPoints = ConvertPoints(space, (start, target));
        for (int i = 0; i < count; i++)
        {
            Transform spawnedObject = Instantiate(itemsPrefabs[prefabid], _container).transform;
            FlyingItem item = CreateFlyingItem(convertedPoints, spawnedObject, duration, scatteringDurationProportion, spawnDelay, onItemArrived);
            itemsContainer.items.Add(item);
        }
        itemsContainer.itemsCount = count;
        _itemsContainers.Add(itemsContainer);
        _itemsContainersCount++;
        return this;
    }
    public Vector3 ConvertWorldPositionToCanvasLocalPosition(Vector3 worldPosition)
    {
        return _camera.WorldToScreenPoint(worldPosition);
    }
    public FlyingItemsOverlay ForceCompleteAll(bool invokeAllCallbacks = true)
    {
        for (int i = 0; i < _itemsCount; i++)
        {
            FlyingItem item = _items[i];
            if (invokeAllCallbacks) item.callbackOnArrived?.Invoke();
            Destroy(item.animationObject);
            item.Dispose();
        }
        _items.Clear();
        _itemsCount = 0;

        for (int i = 0; i < _itemsContainersCount; i++)
        {
            FlyingItemsContainer container = _itemsContainers[i];
            for (int j = 0; j < container.itemsCount; j++)
            {
                FlyingItem item = container.items[j];
                if (invokeAllCallbacks) item.callbackOnArrived?.Invoke();
                Destroy(item.animationObject);
                item.Dispose();
            }
            if (invokeAllCallbacks) container.callbackOnAllItemsArrived?.Invoke();
            container.Dispose();
        }
        _itemsContainers.Clear();
        _itemsContainersCount = 0;
        return this;
    }

    private FlyingItem CreateFlyingItem((Vector3, Vector3) convertedPoints, Transform spawnedObject, float duration, float scatteringDurationProportion, float spawnDelay, Action onItemArrived)
    {
        spawnedObject.position = convertedPoints.Item1;
        spawnedObject.localEulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(-rotationRandom, rotationRandom));
        int stage = scatteringDurationProportion == 0f ? 1 : 0;
        if (spawnDelay > 0f) stage = -1;
        FlyingItem item = new FlyingItem();
        item.spawnPosition = convertedPoints.Item1;
        if (scatteringDurationProportion > 0f)
        {
            item.start = item.spawnPosition + (Vector3)(UnityEngine.Random.insideUnitCircle * scatteringRadius);
        }
        else
        {
            item.start = item.spawnPosition;
        }
        item.end = convertedPoints.Item2;
        item.animationObject = spawnedObject;
        item.callbackOnArrived = onItemArrived;
        item.stage = stage;
        item.animationDuration = duration;
        item.scatteringDuration = duration * scatteringDurationProportion;
        item.elapsedTime = 0f;
        item.delayBeforeStart = 0f;
        return item;
    }
    private (Vector3, Vector3) ConvertPoints(FlyingItemSpace space, (Vector3, Vector3) points)
    {
        (Vector3, Vector3) result;
        switch (space)
        {
            case FlyingItemSpace.WorldToCanvas:
                result.Item1 = ConvertWorldPositionToCanvasLocalPosition(points.Item1);
                result.Item2 = points.Item2;
                break;
            case FlyingItemSpace.WorldToWorld:
                result.Item1 = ConvertWorldPositionToCanvasLocalPosition(points.Item1);
                result.Item2 = ConvertWorldPositionToCanvasLocalPosition(points.Item2);
                break;
            case FlyingItemSpace.CanvasToWorld:
                result.Item1 = points.Item1;
                result.Item2 = ConvertWorldPositionToCanvasLocalPosition(points.Item2);
                break;
            case FlyingItemSpace.CanvasToCanvas:
                result.Item1 = points.Item1;
                result.Item2 = points.Item2;
                break;
            default:
                result.Item1 = points.Item1;
                result.Item2 = points.Item2;
                break;
        }
        return result;
    }
    /// <summary>
    /// Обрабатывает анимацию предметов.
    /// </summary>
    /// <returns>false - если предметы закончились</returns>
    private bool ProcessItems()
    {
        if (_itemsCount == 0) return false;
        for (int i = _itemsCount - 1; i >= 0; i--)
        {
            if (ProcessItem(_items[i]))
            {
                _items.RemoveAt(i);
                _itemsCount--;
                continue;
            }
        }
        return true;
    }
    /// <summary>
    /// Обрабатывает анимацию всех контейнеров предметов.
    /// </summary>
    /// <returns>fals - если контейнеров не осталось</returns>
    private bool ProcessItemsContainers()
    {
        if (_itemsContainersCount == 0) return false;
        for (int i = _itemsContainersCount - 1; i >= 0; i--)
        {
            FlyingItemsContainer container = _itemsContainers[i];
            for (int j = _itemsContainers[i].itemsCount - 1; j >= 0; j--)
            {

                if (ProcessItem(container.items[j]))
                {
                    container.items.RemoveAt(j);
                    container.itemsCount--;
                    continue;
                }
            }
            if (container.itemsCount == 0)
            {
                container.callbackOnAllItemsArrived?.Invoke();
                container.Dispose();
                _itemsContainers.RemoveAt(i);
                _itemsContainersCount--;
            }
        }
        return true;
    }
    /// <summary>
    /// Обрабатывает анимацию предмета.
    /// </summary>
    /// <param name="item">Прдмет для обработки</param>
    /// <returns>true - если анимация закончена</returns>
    private bool ProcessItem(FlyingItem item)
    {
        float progress;
        item.elapsedTime += Time.deltaTime;
        progress = item.elapsedTime / item.animationDuration;
        item.animationObject.localScale = Vector3.one * scaleAnimationCurve.Evaluate(progress);
        switch (item.stage)
        {
            case -1: // delay
                progress = item.elapsedTime / item.delayBeforeStart;
                if (progress >= 1f) item.stage++;
                break;
            case 0: // scattering
                progress = item.elapsedTime / scatteringDuration;
                item.animationObject.position = Vector3.Lerp(item.spawnPosition, item.start, scatteringMovingAnimationCurve.Evaluate(progress));
                if (progress >= 1f) item.stage++;
                break;
            case 1: // moving
                progress = (item.elapsedTime - item.scatteringDuration) / (item.animationDuration - item.scatteringDuration);
                item.animationObject.position = Vector3.Lerp(item.start, item.end, movingAnimationCurve.Evaluate(progress));
                if (progress >= 1f)
                {
                    item.callbackOnArrived?.Invoke();
                    Destroy(item.animationObject.gameObject);
                    item.Dispose();
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
}
