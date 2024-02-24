public interface IInteractible : IEntity
{
    public float RecieveInteraction(IInteractor interactor, float interactionTime);
    public void CancelInteraction(IInteractor interactor);
    public bool CanInteractWith(IInteractor interactor);
    public float InteractionDuration { get; }
    public bool IsInteracting { get; }
}
