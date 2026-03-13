using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _orientationTransform;




    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _rotationSpeed = 15f; 
    [SerializeField] private Transform _playerVisuals; 
    
    [Tooltip("Karakterin modeli ters bakıyorsa burayı 180 yapın (Civciv gibi)")]
    [SerializeField] private float _modelRotationOffset = 0f;

    [SerializeField] private Key _forwardKey = Key.W;
    [SerializeField] private Key _backwardKey = Key.S;
    [SerializeField] private Key _leftKey = Key.A;
    [SerializeField] private Key _rightKey = Key.D;
    [SerializeField] private Key _jumpKey = Key.Space;
    
    [Header("Ground Check")]
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private LayerMask _whatIsGround;

    [Header("Movement Modes")]
    [SerializeField] private Key _normalModeKey = Key.Q;
    [SerializeField] private Key _slidingModeKey = Key.E;
    [SerializeField] private float _slideMultiplier = 2f;
    
    [Header("Drag (Sürtünme) Settings")]
    [SerializeField] private float _normalDrag = 5f; // Normal modda yere daha çok tutunur
    [SerializeField] private float _slideDrag = 1f;  // Kayma modunda zeminde daha çok kayar (buz pateni gibi)
    [SerializeField] private float _airDrag = 0f;    // Havadayken sürtünme olmasın ki zıplama düzgün çalışsın
    
    private bool _isSlidingMode;

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

        // O anki duruma göre sürtünmeyi (Drag) ayarlayalım
        if (_isGrounded)
        {
            _playerRigidbody.linearDamping = _isSlidingMode ? _slideDrag : _normalDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = _airDrag;
        }

        _horizontalInput = 0f;
        _verticalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current[_rightKey].isPressed) _horizontalInput += 1f;
            if (Keyboard.current[_leftKey].isPressed) _horizontalInput -= 1f;
            if (Keyboard.current[_forwardKey].isPressed) _verticalInput += 1f;
            if (Keyboard.current[_backwardKey].isPressed) _verticalInput -= 1f;

            // Hem tuşa basılmış olması hem de karakterin yerde olması gerekli
            if (Keyboard.current[_jumpKey].wasPressedThisFrame && _isGrounded)
            {
                Jump();
            }

            // Normal Moda geçiş (Q)
            if (Keyboard.current[_normalModeKey].wasPressedThisFrame)
            {
                _isSlidingMode = false;
            }

            // Kayma Moduna geçiş (E)
            if (Keyboard.current[_slidingModeKey].wasPressedThisFrame)
            {
                _isSlidingMode = true;
            }
        }

        // --- YUMUŞAK ROTASYON KISMI (Update içinde daha akıcı çalışır) ---
        Vector3 inputDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        
        if (inputDirection != Vector3.zero && _playerVisuals != null)
        {
            // Videodaki kodun birebir aynısı (Model tersse PlayerVisual'ın altındaki civciv modelini elle 180 derece döndürün)
            _playerVisuals.forward = Vector3.Slerp(_playerVisuals.forward, inputDirection.normalized, Time.deltaTime * _rotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        _moveDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        
        // Moduza göre hızı ayarlıyoruz
        float currentSpeed = _isSlidingMode ? (_moveSpeed * _slideMultiplier) : _moveSpeed;
        
        _playerRigidbody.AddForce(_moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    private void Jump()
    {
        // Önceki dikey hızı sıfırlıyoruz ki her zıplama aynı yükseklikte olsun
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
}

