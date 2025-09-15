namespace Interaction
{
    /// <summary>
    /// Dummy interaction to handle dropping objects and get valid tooltip
    /// </summary>
    public class DropInteraction : IInteractable
    {
        public void Interact(Player interactingPlayer)
        {
            interactingPlayer.ObjectHolder.Drop();
        }

        public string GetTooltip()
        {
            return "to drop";
        }
    }
}