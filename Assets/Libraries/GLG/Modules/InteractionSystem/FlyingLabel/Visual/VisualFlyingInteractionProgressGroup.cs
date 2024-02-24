using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualFlyingInteractionProgressGroup : MonoBehaviour
{
    [SerializeField] private RectTransform _cashedRectTransform;
    [SerializeField] private RectTransform _itemsParent;
    [SerializeField] private LayoutGroup _layoutGroup;

    public List<VisualFlyingInteractionProgress> Items { get; } = new List<VisualFlyingInteractionProgress>();
    public RectTransform RectTransform => _cashedRectTransform;

    public VisualFlyingInteractionProgress AddVisualProgress(VisualFlyingInteractionProgress visualProgressPrefab)
    {
        Vector3[] positions = new Vector3[Items.Count];
        for (int i = 0; i < Items.Count; i++)
        {
            positions[i] = Items[i].VisualRoot.position;
        }
        VisualFlyingInteractionProgress instance = Instantiate(visualProgressPrefab, _itemsParent, false);
        Items.Add(instance);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsParent);
        for (int i = 0; i < positions.Length; i++)
        {
            Items[i].VisualRoot.position = positions[i];
            Items[i].SmoothResetVisualPosition();
        }
        return instance;
    }
    public void RemoveVisualProgress(VisualFlyingInteractionProgress visualProgressInstance)
    {
        Items.Remove(visualProgressInstance);
        Vector3[] positions = new Vector3[Items.Count];
        for (int i = 0; i < Items.Count; i++)
        {
            positions[i] = Items[i].VisualRoot.position;
        }
        visualProgressInstance.Hide();
        visualProgressInstance.RectTransform.SetParent(_cashedRectTransform.parent);
        visualProgressInstance.RectTransform.SetAsFirstSibling();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsParent);

        for (int i = 0; i < positions.Length; i++)
        {
            Items[i].VisualRoot.position = positions[i];
            Items[i].SmoothResetVisualPosition();
        }
    }
}
