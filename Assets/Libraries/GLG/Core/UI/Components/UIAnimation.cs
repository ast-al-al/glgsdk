using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [Header("Scale pulsation")]
    public bool useScale;
    public float defaultScale = 1f;
    public float scaleAmplitude = 0.1f;
    public float scaleSpeed = 1f;
    public float scaleTimeOffset = 0f;
    [Header("Rotation pulsation")]
    public bool useRotation;
    public float defaultZRotation = 0f;
    public float rotationAmplitude = 20f;
    public float rotationSpeed = 1f;
    public float rotationTimeOffset = 0f;
    [Header("Continous rotation")]
    public bool useContinousRotation;
    public float continousRotationSpeed = 15f;

    private void Update()
    {
        if (useScale)
        {
            transform.localScale
                = (Vector3.one * defaultScale) + Vector3.one * scaleAmplitude * Mathf.Sin((Time.time + scaleTimeOffset) * scaleSpeed);
        }
        if (useRotation)
        {
            transform.eulerAngles
                = new Vector3(0f, 0f, defaultZRotation + (rotationAmplitude * Mathf.Sin((Time.time + rotationTimeOffset) * rotationSpeed)));
        }
        if (useContinousRotation)
        {
            transform.localRotation *= Quaternion.Euler(0f, 0f, continousRotationSpeed * Time.deltaTime);
        }
    }
}
