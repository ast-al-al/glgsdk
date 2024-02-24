public interface IInteractor : IEntity
{
    public float InteractWith(IInteractible interactible);
    public void ForceInteractionComplete(IInteractible interactible);
}
