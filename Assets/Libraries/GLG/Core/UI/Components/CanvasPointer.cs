using DG.Tweening;
using UnityEngine;

public class CanvasPointer : MonoBehaviour
{
    public enum PointerDirection {Up = 180, Down = 0, Left = 90, Right = -90}

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _turningAnimationDuration;
    [SerializeField] private RectTransform _visualPointer;

    private Transform _cashedTransform;
    private PointerDirection _currenDirection = PointerDirection.Down;

    public RectTransform RectTransform => _rectTransform;

    public void SetDirection(PointerDirection direction)
    {
        if(_currenDirection == direction) return;
        _currenDirection = direction;
        float angle;
        switch (direction)
        {
            case PointerDirection.Right:
                angle = 90f;
                break;
            case PointerDirection.Left:
                angle = -90f;
                break;
            case PointerDirection.Up:
                angle = 180f;
                break;
            case PointerDirection.Down:
                angle = 0f;
                break;
            default:
                angle = 0f;
                break;
        }
        _visualPointer.DOKill();
        _visualPointer.DOLocalRotate(new Vector3(0f, 0f, angle), _turningAnimationDuration).SetEase(Ease.OutCubic);
    }
    
}
