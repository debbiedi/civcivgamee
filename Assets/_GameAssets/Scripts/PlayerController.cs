using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform _orientationTransform;




    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    
    [Header("Ground Check")]
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private LayerMask _whatIsGround;

    private Rigidbody _playerRigidbody;
    private bool _isGrounded;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
    }

    private void Update()
    {
        // Karakterin merkezinden aşağı bir ışın (Raycast) yollayarak yerde olup olmadığını tespit ediyoruz
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _whatIsGround);

        _horizontalInput = 0f;
        _verticalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed) _horizontalInput += 1f;
            if (Keyboard.current.aKey.isPressed) _horizontalInput -= 1f;
            if (Keyboard.current.wKey.isPressed) _verticalInput += 1f;
            if (Keyboard.current.sKey.isPressed) _verticalInput -= 1f;

            // Hem tuşa basılmış olması hem de karakterin yerde olması gerekli
            if (Keyboard.current.spaceKey.wasPressedThisFrame && _isGrounded)
            {
                Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        _playerRigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
    }

    private void Jump()
    {
        // Önceki dikey hızı sıfırlıyoruz ki her zıplama aynı yükseklikte olsun
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
}

