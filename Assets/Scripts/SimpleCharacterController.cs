using UnityEngine;

/// <summary>
/// Simple player controller to handle movement/rotation
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField][Range(2, 20)]
    private float moveSpeed = 5f;
    
    [SerializeField][Range(1, 10)]
    private float jumpHeight = 1.5f;

    [Header("Mouse Look")]
    [SerializeField] 
    private Transform cameraTransform;
    
    [SerializeField][Range(50, 500)] 
    private float mouseSensitivity = 100f;
    
    private CharacterController _controller;

    private Vector3 _velocity;
    private float _xRotation = 60f;
    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
        }
        
        _xRotation = cameraTransform.rotation.eulerAngles.x; 
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        // Handle Rotation XZ Plane
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, GameConstants.MinXRotation, GameConstants.MaxXRotation);
        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        
        // Handle Rotation XY Plane
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;   
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        var isGrounded = _controller.isGrounded;
        if (isGrounded && _velocity.y < 0)
            _velocity.y = -2.0f; // intentionally uses small lower than zero value to get "stable" behavior

        var xInput = Input.GetAxis("Horizontal");
        var zInput = Input.GetAxis("Vertical");

        var inputVector = transform.right * xInput + transform.forward * zInput;
        
        if (inputVector.magnitude > 1)
            inputVector.Normalize();
        
        var moveVector = inputVector * moveSpeed;

        // kinematic equation: start speed that is needed to achieve desired height
        if (isGrounded && Input.GetButtonDown("Jump"))
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * GameConstants.Gravity);

        _velocity.y += GameConstants.Gravity * Time.deltaTime;

        var moveDir = _velocity + moveVector;

        _controller.Move(moveDir * Time.deltaTime);   
    }
}