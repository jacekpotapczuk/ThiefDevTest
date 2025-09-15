using Interaction;
using UnityEngine;

/// <summary>
/// Gameplay object that can be stolen
/// </summary>
public class TheftObject : MonoBehaviour, IInteractable
{
    public void Interact(Player interactingPlayer)
    {
        interactingPlayer?.ObjectHolder?.TryHold(transform, interactingPlayer.InteractionController);
    }

    public string GetTooltip()
    {
        return "hold object";
    }

    private void OnTriggerEnter(Collider other)
    {
        var theftAreaCollider = other.GetComponent<TheftAreaCollider>();

        if (theftAreaCollider != null)
        {
            theftAreaCollider.TheftArea.AddTheftObjectToArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var theftAreaCollider = other.GetComponent<TheftAreaCollider>();
        
        if (theftAreaCollider != null)
        {
            theftAreaCollider.TheftArea.RemoveTheftObjectToArea(this);
        }
    }
}
