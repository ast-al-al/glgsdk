using UnityEngine;

public class FlyingInteractorProgress : FlyingLabelsOverlay.FlyingItem
{
    private BaseInteractor _targetInteractor;

    public override bool Visible { get => Visual.gameObject.activeSelf; set => Visual.gameObject.SetActive(value); }
    public override FlyingLabelsOverlay.LabelKind LabelKind => FlyingLabelsOverlay.LabelKind.InteractionProgress;
    public override RectTransform RectTransform => Visual.RectTransform;
    public VisualFlyingInteractionProgress Visual { get; private set; }

    public FlyingInteractorProgress(
        Transform target,
        Vector3 offset,
        bool lockOnTarget,
        VisualFlyingInteractionProgress prefab,
        BaseInteractor targetInteractible) 
        : base(target, offset, lockOnTarget)
    {
        _targetInteractor = targetInteractible;
        Visual = GameObject.Instantiate(prefab, FlyingLabelsOverlay.ItemsContainer.transform);
        Visual.transform.localPosition = _target.PositionInContainer;
        _targetInteractor.OnInteractionStarted += InteractionStartedHandler;
        _targetInteractor.OnInteractionPerformed += InteractionPerformedHandler;
        _targetInteractor.OnInteractionComplete += InteractionCompletedHandler;
        _targetInteractor.OnInteractionCanceled += InteractionCanceledHandler;
    }

    public override void Dispose()
    {
        _targetInteractor.OnInteractionStarted -= InteractionStartedHandler;
        _targetInteractor.OnInteractionPerformed -= InteractionPerformedHandler;
        _targetInteractor.OnInteractionComplete -= InteractionCompletedHandler;
        _targetInteractor.OnInteractionCanceled -= InteractionCanceledHandler;
        if (Visual == null) return;
        GameObject.Destroy(Visual.gameObject);
        Visual = null;
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

    private void InteractionStartedHandler(IInteractible interactible)
    {
        Visual.SetProgress(0f);
        Visual.Show();
    }
    private void InteractionPerformedHandler(IInteractible interactible, float progress)
    {
        Visual.SetProgress(progress);
    }
    private void InteractionCompletedHandler(IInteractible interactible)
    {
        Visual.SetProgress(1f);
        Visual.Hide();
    }
    private void InteractionCanceledHandler(IInteractible interactible)
    {
        Visual.Hide();
    }

}