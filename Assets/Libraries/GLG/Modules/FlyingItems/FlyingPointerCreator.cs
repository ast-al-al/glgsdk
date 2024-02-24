using GLG;
using UnityEngine;

public class FlyingPointerCreator : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    
    private void Awake()
    {
        Kernel.UI.Get<FlyingLabelsOverlay>().CreatePointer(_target, _offset);
    }

    private void OnDestroy()
    {
        Kernel.UI.Get<FlyingLabelsOverlay>().RemovePointer(_target);
    }
}
