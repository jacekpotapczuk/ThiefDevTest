using UnityEngine;

namespace Interaction
{
    /// <summary>
    /// Component to handle player holding/dropping (and throwing) objects
    /// </summary>
    public class PlayerObjectHolder : MonoBehaviour
    {
        public Transform ObjectHeld { get; private set; }
    
        [Header("General Settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform holdSocketTransform;
        [Header("Throw Settings")]
        [SerializeField][Range(1, 5)] private float linearThrowMultiplier = 1f;
        [SerializeField][Range(1, 5)] private float angularThrowMultiplier = 1f;
        [SerializeField][Range(10, 50)] private float maxThrowSpeed = 20f;
    
        private Transform _objectHeldParent;
        private bool _wasObjectHeldKinematic;

        private Vector3 _lastCamPos;
        private Quaternion _lastCamRot;
        private Vector3 _camLinearVelocity;
        private Vector3 _camAngularVelocity;
        
        public bool TryHold(Transform objectToHold, InteractionController interactionController)
        {
            if (ObjectHeld != null)
            {
                if (ObjectHeld == objectToHold)
                {
                    Drop();
                    return false;
                }
                
                return false;
            }

            ObjectHeld = objectToHold;

            var rb = ObjectHeld.GetComponent<Rigidbody>();
            if (rb != null)
            {
                _wasObjectHeldKinematic = rb.isKinematic;
                rb.isKinematic = true;
            }

            _objectHeldParent = ObjectHeld.parent;
            ObjectHeld.SetParent(holdSocketTransform, worldPositionStays: true);
            interactionController.ForceInteraction(new DropInteraction());

            return true;
        }

        public void Drop()
        {
            if (ObjectHeld == null)
            {
                return;
            }

            var objectRigidbody = ObjectHeld.GetComponent<Rigidbody>();
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = _wasObjectHeldKinematic;

                var cameraToObject = ObjectHeld.position - cameraTransform.position;
                var vRot = Vector3.Cross(_camAngularVelocity, cameraToObject) * angularThrowMultiplier;
                var vLin = _camLinearVelocity * linearThrowMultiplier;

                var vTotal = vLin + vRot;

                if (maxThrowSpeed > 0f)
                {
                    vTotal = Vector3.ClampMagnitude(vTotal, maxThrowSpeed);   
                }

                objectRigidbody.AddForce(vTotal, ForceMode.VelocityChange);
            }

            ObjectHeld.SetParent(_objectHeldParent, worldPositionStays: true);
            ObjectHeld = null;
        }

        private void Start()
        {
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main?.transform;
            }

            _lastCamPos = cameraTransform.position;
            _lastCamRot = cameraTransform.rotation;
        }

        private void Update()
        {
            UpdateCameraVelocities();
        }
        
        private void UpdateCameraVelocities()
        {
            // Linear velocity
            _camLinearVelocity = (cameraTransform.position - _lastCamPos) / Time.deltaTime;
            _lastCamPos = cameraTransform.position;

            // Angular velocity
            var delta = cameraTransform.rotation * Quaternion.Inverse(_lastCamRot);
            delta.ToAngleAxis(out var angleDeg, out var axis);
        
            if (angleDeg > 180f)
            {
                angleDeg -= 360f;
            }

            var angleRad = angleDeg * Mathf.Deg2Rad;
            _camAngularVelocity = axis * (angleRad / Time.deltaTime);
            _lastCamRot = cameraTransform.rotation;
        }
    }
}
