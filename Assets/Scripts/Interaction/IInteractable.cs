namespace Interaction
{
    /// <summary>
    /// Interface for interactable objects
    /// </summary>
    public interface IInteractable
    {
        void Interact(Player interactingPlayer);
        string GetTooltip();
    }
}