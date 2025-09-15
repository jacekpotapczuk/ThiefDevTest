using System;
using Interaction;
using TMPro;
using UnityEngine;

namespace Ui
{
    /// <summary>
    /// Ui for interaction tooltips.
    /// </summary>
    public class InteractableObjectUi : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text interactionText;
    
        private InteractionController _interactionController;
    
        private void Start()
        {
            var characterControllers = FindObjectsByType<InteractionController>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (characterControllers.Length != 1)
            {
                throw new Exception($"Expected one and only {nameof(InteractionController)} on scene.");
            }

            _interactionController = characterControllers[0];
        }

        private void Update()
        {
            if (!_interactionController.HasTooltip())
            {
                interactionText.enabled = false;
                return;
            }
        
            interactionText.enabled = true;
            interactionText.text = _interactionController.GetTooltip();
        }
    }
}