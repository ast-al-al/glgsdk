using System.Collections.Generic;
using UnityEngine;

public class FlyingInteractibleProgress : FlyingLabelsOverlay.FlyingItem
{
    private BaseInteractible _targetInteractible;
    private VisualFlyingInteractionProgress _itemPrefab;

    public override bool Visible { get => Visual.gameObject.activeSelf; set => Visual.gameObject.SetActive(value); }
    public override FlyingLabelsOverlay.LabelKind LabelKind => FlyingLabelsOverlay.LabelKind.InteractionProgress;
    public override RectTransform RectTransform => Visual.RectTransform;
    public VisualFlyingInteractionProgressGroup Visual { get; private set; }
    public List<(VisualFlyingInteractionProgress visual, IInteractor interactor)> Items { get; private set; } = new();

    public FlyingInteractibleProgress(
        Transform target,
        Vector3 offset,
        bool lockOnTarget,
        VisualFlyingInteractionProgressGroup groupPrefab,
        VisualFlyingInteractionProgress itemPrefab,
        BaseInteractible targetInteractible)
        : base(target, offset, lockOnTarget)
    {
        _targetInteractible = targetInteractible;
        _itemPrefab = itemPrefab;
        Visual = GameObject.Instantiate(groupPrefab, FlyingLabelsOverlay.ItemsContainer.transform);
        Visual.transform.localPosition = _target.PositionInContainer;
        _targetInteractible.OnInteractionStarted += InteractionStartedHandler;
        _targetInteractible.OnInteractionPerformed += InteractionPerformedHandler;
        _targetInteractible.OnInteractionComplete += InteractionCompletedHandler;
        _targetInteractible.OnInteractionCanceled += InteractionCanceledHandler;
    }

    public override void Dispose()
    {
        _targetInteractible.OnInteractionStarted -= InteractionStartedHandler;
        _targetInteractible.OnInteractionPerformed -= InteractionPerformedHandler;
        _targetInteractible.OnInteractionComplete -= InteractionCompletedHandler;
        _targetInteractible.OnInteractionCanceled -= InteractionCanceledHandler;
        if (Visual == null) return;
        GameObject.Destroy(Visual.gameObject);
        Visual = null;
        Items = null;
        _target = null;
    }
    public override void ManagedUpdate()
    {
        if (_lockOnTarget)
        {
            Visual.transform.localPosition = _target.PositionInContainer;
        }
    }
    protected override void DoAnimation()
    {

    }

    public VisualFlyingInteractionProgress AddProgressItem(IInteractor interactor, VisualFlyingInteractionProgress visualPrefab)
    {
        VisualFlyingInteractionProgress result = Visual.AddVisualProgress(visualPrefab);
        Items.Add((result, interactor));
        return result;
    }

    private void InteractionStartedHandler(IInteractor interactor)
    {
        AddProgressItem(interactor, _itemPrefab)
            .SetProgress(0f)
            .Show();
    }
    private void InteractionPerformedHandler(IInteractor interactor, float progress)
    {
        GetVisualItem(interactor).SetProgress(progress);
    }
    private void InteractionCompletedHandler(IInteractor interactor)
    {
        GetVisualItem(interactor).SetProgress(1f);
        RemoveItem(interactor);
    }
    private void InteractionCanceledHandler(IInteractor interactor)
    {
        RemoveItem(interactor);
    }


    public VisualFlyingInteractionProgress GetVisualItem(IInteractor interactor)
    {
        foreach (var item in Items)
        {
            if (item.interactor == interactor)
            {
                return item.visual;
            }
        }
        return null;
    }
    private void RemoveItem(IInteractor interactor)
    {

        for (int i = Items.Count - 1; i >= 0; i--)
        {
            if (Items[i].interactor == interactor)
            {
                Visual.RemoveVisualProgress(Items[i].visual);
                Items.RemoveAt(i);
                return;
            }
        }
    }
}