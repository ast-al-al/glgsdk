using GLG;
using UnityEngine;

public class EmissionAnimator : MonoBehaviour, IManaged
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Color _fromColor;
    [SerializeField] private Color _toColor;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeOffset;

    private Color _currentColor;

    private void Awake()
    {
        Kernel.RegisterManaged(this);
    }
    private void OnDestroy()
    {
        Kernel.UnregisterManaged(this);
    }

    public void ManagedUpdate()
    {
        float t = Mathf.Sin((Time.time + _timeOffset) * _speed);
        _currentColor = Color.Lerp(_fromColor, _toColor, t);
        _renderer.material.SetColor("_Emission", _currentColor);
    }
}
