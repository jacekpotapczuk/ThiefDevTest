using UnityEngine;

namespace Interaction
{
    /// <summary>
    /// Basic controller for player interactions 
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class InteractionController : MonoBehaviour
    {
        [SerializeField]
        private float interactionDistance = 3f;
    
        [SerializeField]
        private KeyCode interactKey = KeyCode.E;
    
        [SerializeField]
        private LayerMask interactionLayer;
    
        [SerializeField]
        private Camera playerCamera;

        private Player _player;

        private IInteractable _forcedInteractable;
        private IInteractable _lookInteractable;
        private IInteractable _nullInteractable;

        public void ForceInteraction(IInteractable interactable)
        {
            _forcedInteractable = interactable;
        }

        public bool HasTooltip()
        {
            return GetCurrentInteractable() != null;
        }

        public string GetTooltip()
        {
            var currentInteractable = GetCurrentInteractable();
        
            if (currentInteractable != null)
            {
                return $"Click {interactKey.ToString()} to {currentInteractable.GetTooltip()}";
            }

            return string.Empty;
        }
    
        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            _lookInteractable = null;
            var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (!Physics.Raycast(ray, out var hit, interactionDistance, interactionLayer))
            {
                return;
            } 
            
            _lookInteractable = hit.collider.GetComponent<IInteractable>();
        
            if (Input.GetKeyDown(interactKey))
            {
                TryInteract();
            }
        }

        private bool TryInteract()
        {
            // can be expanded as list of interactions with different priorities (e.x. using PriorityQueue)
            // or keep it simple and set up priorities here

            ref var currentInteractable = ref GetCurrentInteractableRef();
        
            if (currentInteractable != _nullInteractable)
            {
                currentInteractable.Interact(_player);
                currentInteractable = null;
                return true;
            }
        
            return false;
        }
        
        private IInteractable GetCurrentInteractable()
        {
            var interactable = GetCurrentInteractableRef();
        
            return interactable == _nullInteractable ? null : interactable;
        }
    
        private ref IInteractable GetCurrentInteractableRef()
        {
            // can be expanded as list of interactions with different priorities (e.x. using PriorityQueue)
            // or keep it simple and set up priorities here
        
            if (_forcedInteractable != null)
            {
                return ref _forcedInteractable;
            }
        
            if (_lookInteractable != null)
            {
                return ref _lookInteractable;
            }
        
            return ref _nullInteractable;
        }
    }
}