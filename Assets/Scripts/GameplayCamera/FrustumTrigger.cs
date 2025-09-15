using System;
using UnityEditor;
using UnityEngine;

namespace GameplayCamera
{
    /// <summary>
    /// Generates runtime mesh for specified Frustum.
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer))]
    public class FrustumTrigger : MonoBehaviour
    {
        public event Action<Collider> OnFrustumTriggerEnter;
        public event Action<Collider> OnFrustumTriggerExit;
    
        [Header("Frustum geometry")]
        [Tooltip("Vertical FOV in degrees.")]
        [SerializeField][Range(1f, 179f)] 
        private float verticalFOV = 60f;
    
        [SerializeField]
        private float aspectRatio = 16f / 9f;

        [SerializeField][Range(0.25f, 20f)]
        private float nearPlaneDistance = 0.25f;

        [SerializeField][Range(2f, 50f)]
        [Tooltip("Keep larger than nearPlaneDistance")]
        private float farPlaneDistance = 8f;
    
        [Header("Frustum Look")]
        [SerializeField]
        private Material frustumMaterial;
    
        [Header("Editor/Debug")]
        [SerializeField]
        private bool drawGizmos = true;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private MeshRenderer _meshRenderer;
    
        public void OverrideMaterial(Material material)
        {
            _meshRenderer.sharedMaterial = material;
        }

        public void SetStandardMaterial()
        {
            _meshRenderer.sharedMaterial = frustumMaterial;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            OnFrustumTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnFrustumTriggerExit?.Invoke(other);
        }

        private void Reset()
        {
            OnRestart();
        }

        private void Awake()
        {
            OnRestart();
        }

        private void OnRestart()
        {
            InitComponents();
            BuildFrustumMesh();
            ApplyToCollider();
            SetStandardMaterial();
        }

        private void InitComponents()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
            _meshRenderer = GetComponent<MeshRenderer>();
       
            if (_mesh == null)
            {
                _mesh = new Mesh { name = "FrustumRuntimeMesh" };
                _mesh.MarkDynamic();
            }
        }
    
        private void BuildFrustumMesh()
        {
            var tan = Mathf.Tan(verticalFOV * Mathf.Deg2Rad * 0.5f);
            var halfHNear = nearPlaneDistance * tan;
            var halfWNear = halfHNear * aspectRatio;
            var halfHFar  = farPlaneDistance  * tan;
            var halfWFar  = halfHFar  * aspectRatio;

            // Near (z = nearDist)
            var nBL = new Vector3(-halfWNear, -halfHNear, nearPlaneDistance); // 0
            var nBR = new Vector3(+halfWNear, -halfHNear, nearPlaneDistance); // 1
            var nTR = new Vector3(+halfWNear, +halfHNear, nearPlaneDistance); // 2
            var nTL = new Vector3(-halfWNear, +halfHNear, nearPlaneDistance); // 3

            // Far (z = farDist)
            var fBL = new Vector3(-halfWFar, -halfHFar, farPlaneDistance);    // 4
            var fBR = new Vector3(+halfWFar, -halfHFar, farPlaneDistance);    // 5
            var fTR = new Vector3(+halfWFar, +halfHFar, farPlaneDistance);    // 6
            var fTL = new Vector3(-halfWFar, +halfHFar, farPlaneDistance);    // 7

            var verts = new Vector3[8]
            {
                nBL, nBR, nTR, nTL,   // 0..3
                fBL, fBR, fTR, fTL    // 4..7
            };

            var tris = new int[]
            {
                // Near face
                0, 1, 2,  
                2, 3, 0,  

                // Far face
                4, 5, 6,
                4, 6, 7,

                // Left face
                0, 3, 7,
                0, 7, 4,

                // Right face
                1, 5, 6,
                1, 6, 2,

                // Top face
                3, 2, 6,
                6, 7, 3,

                // Bottom face 
                0, 1, 5,
                5, 4, 0,
            
            };

            _mesh.Clear();
            _mesh.vertices   = verts;
            _mesh.triangles  = tris;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            _meshFilter.sharedMesh = _mesh;
        }

        private void ApplyToCollider()
        {
            if (_meshCollider == null)
            {
                return;
            }

            _meshCollider.convex = true;
            _meshCollider.isTrigger = true;

            // Rebind mesh
            _meshCollider.sharedMesh = null;
            _meshCollider.sharedMesh = _mesh;
        }

#if UNITY_EDITOR
        public void SetPreviewValues(float previewSpeed, float previewAmplitude)
        {
            _previewSpeed = previewSpeed;
            _previewAmplitude = previewAmplitude;
        }

        private float _previewSpeed = 0f;
        private float _previewAmplitude = 0f;
        private void OnDrawGizmosSelected()
        {
            // Gizmos Preview is not that usefull with my approach since I'm already drawing mesh and we have MeshRenderer
            // but I still do it since I want to preview the movement of camera.
            // previewSpeed/previeAmplitude are passed from GameplayCamera since I want to keep FrustrumTrigger independent from camera funtiopnality 
            if (!drawGizmos)
            {
                return;
            }
        
            // transform to world coords
            Vector3 ToWorld(int i, Matrix4x4 matrix)
            {
                return matrix.MultiplyPoint3x4(_meshFilter.sharedMesh.vertices[i]);
            }
        
            var time = (float)EditorApplication.timeSinceStartup;
            float angle = _previewAmplitude * Mathf.Sin(_previewSpeed * Mathf.Deg2Rad * time);
        
            var previewRotation = Quaternion.Euler(0f, angle, 0f); 
            var rotationMatrix = Matrix4x4.Rotate(previewRotation);

            var finalMatrix = transform.localToWorldMatrix * rotationMatrix;

            Gizmos.matrix = Matrix4x4.identity;
        
            // Near
            Gizmos.DrawLine(ToWorld(0, finalMatrix), ToWorld(1, finalMatrix));
            Gizmos.DrawLine(ToWorld(1, finalMatrix), ToWorld(2, finalMatrix));
            Gizmos.DrawLine(ToWorld(2, finalMatrix), ToWorld(3, finalMatrix));
            Gizmos.DrawLine(ToWorld(3, finalMatrix), ToWorld(0, finalMatrix));

            // Far
            Gizmos.DrawLine(ToWorld(4, finalMatrix), ToWorld(5, finalMatrix));
            Gizmos.DrawLine(ToWorld(5, finalMatrix), ToWorld(6, finalMatrix));
            Gizmos.DrawLine(ToWorld(6, finalMatrix), ToWorld(7, finalMatrix));
            Gizmos.DrawLine(ToWorld(7, finalMatrix), ToWorld(4, finalMatrix));

            // In between
            Gizmos.DrawLine(ToWorld(0, finalMatrix), ToWorld(4, finalMatrix));
            Gizmos.DrawLine(ToWorld(1, finalMatrix), ToWorld(5, finalMatrix));
            Gizmos.DrawLine(ToWorld(2, finalMatrix), ToWorld(6, finalMatrix));
            Gizmos.DrawLine(ToWorld(3, finalMatrix), ToWorld(7, finalMatrix));
        }
#endif
    }
}