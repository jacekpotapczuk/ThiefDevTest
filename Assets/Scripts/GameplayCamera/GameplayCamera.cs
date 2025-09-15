#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameplayCamera
{
    /// <summary>
    /// Controls gameplay camera to detect player.
    /// </summary>
    public class GameplayCamera : MonoBehaviour
    {
        [Header("Gameplay settings")]
        [SerializeField] 
        private FrustumTrigger cameraFrustumTrigger;
    
        [SerializeField] 
        private Transform cameraArm;
    
        [SerializeField] 
        private float rotationSpeed = 30.0f;
    
        [SerializeField][Tooltip("How much camera can rotate to left and right. Starting rotation is in the middle.")]
        private float rotationAngle = 180.0f;
    
        [SerializeField] 
        private Material playerDetectedFrustumMaterial;

#if UNITY_EDITOR
        [Header("Editor settings")] 
        [SerializeField]
        private bool previewMovement = true;
#endif
    
        private float _currentRotationAngle;
        private bool _isMovingRight;
        private void Awake()
        {
            cameraFrustumTrigger.OnFrustumTriggerEnter += OnCameraFrustumTriggerEnter;
            cameraFrustumTrigger.OnFrustumTriggerExit += OnCameraFrustumTriggerExit;

            _currentRotationAngle = rotationAngle / 2f;
        }
    
        private void OnCameraFrustumTriggerEnter(Collider eCollider)
        {
            var playerComponent = eCollider.GetComponent<Player>();
        
            if (playerComponent == null)
            {
                return;
            }
        
            cameraFrustumTrigger.OverrideMaterial(playerDetectedFrustumMaterial);
            playerComponent.MarkDetected();
        }
    
        private void OnCameraFrustumTriggerExit(Collider eCollider)
        {
            var playerComponent = eCollider.GetComponent<Player>();
        
            if (playerComponent == null)
            {
                return;
            }
        
            cameraFrustumTrigger.SetStandardMaterial();
            playerComponent.MarkUnDetected();
        }

        private void Update()
        {
            var angle = (_isMovingRight ? 1f : -1f) * rotationSpeed * Time.deltaTime;
            cameraArm.Rotate(cameraArm.up, angle);

            _currentRotationAngle += angle;
        
            if (_currentRotationAngle >= rotationAngle || _currentRotationAngle <= 0f)
            {
                _isMovingRight = !_isMovingRight;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected() 
        {
            SceneView.RepaintAll();
        
            if (previewMovement)
            {
                cameraFrustumTrigger.SetPreviewValues(rotationSpeed, rotationAngle / 2f);    
            }
            else
            {
                cameraFrustumTrigger.SetPreviewValues(0f, 0f);
            }
        }
#endif
    }
}